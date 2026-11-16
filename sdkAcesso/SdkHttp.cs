using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;

namespace ControliD
{
    /// <summary>
    /// Origem dos <see cref="HttpClient"/> usados pelos métodos assíncronos do SDK (XxxAsync).
    /// </summary>
    /// <remarks>
    /// Contrato (no estilo IHttpClientFactory): cada chamada a <see cref="ClientProvider"/> deve devolver um
    /// <b>novo</b> <see cref="HttpClient"/>, que o SDK descarta (Dispose) ao fim de UMA requisição.
    /// O host deve criá-lo com <c>disposeHandler: false</c> sobre um handler compartilhado/rotacionado
    /// (por exemplo <c>IHttpClientFactory.CreateClient(...)</c>). O SDK nunca usa <see cref="HttpClient.Timeout"/>
    /// (define <see cref="Timeout.InfiniteTimeSpan"/>); o timeout de cada requisição é aplicado por CancellationToken.
    /// <para>
    /// Sem provedor injetado (global ou por <c>Device.HttpClientProvider</c>), o SDK usa um
    /// <see cref="HttpClientHandler"/> interno, compartilhado e rotacionado a cada <see cref="DefaultHandlerLifetime"/>.
    /// </para>
    /// Os métodos síncronos existentes (HttpWebRequest) não são afetados por esta classe.
    /// </remarks>
    public static class SdkHttp
    {
        private static Func<HttpClient> _clientProvider;
        private static long _handlerLifetimeTicks = TimeSpan.FromMinutes(2).Ticks;
        private static long _gracePeriodTicks = TimeSpan.FromMinutes(5).Ticks;
        private static int _keepAlive; // 0 = false (padrão)

        /// <summary>
        /// Provedor global de <see cref="HttpClient"/> (null = handler padrão interno do SDK).
        /// Cada chamada deve devolver um NOVO HttpClient (o SDK o descarta após a requisição),
        /// criado com <c>disposeHandler: false</c> sobre um handler compartilhado.
        /// Pode ser sobrescrito por instância em <c>Device.HttpClientProvider</c>.
        /// </summary>
        public static Func<HttpClient> ClientProvider
        {
            get { return Volatile.Read(ref _clientProvider); }
            set { Volatile.Write(ref _clientProvider, value); }
        }

        /// <summary>
        /// Opcional (padrão: false). Quando false, as requisições assíncronas usam HTTP/1.0 com
        /// "Connection: close", igual ao caminho síncrono (HttpWebRequest KeepAlive=false, HTTP/1.0).
        /// Quando true, usam HTTP/1.1 com keep-alive (reaproveitamento de conexões pelo handler).
        /// </summary>
        public static bool KeepAlive
        {
            get { return Volatile.Read(ref _keepAlive) != 0; }
            set { Volatile.Write(ref _keepAlive, value ? 1 : 0); }
        }

        /// <summary>
        /// Tempo de vida do handler padrão interno (usado quando nenhum provedor é injetado).
        /// Após esse tempo um novo handler é criado para as próximas requisições. Padrão: 2 minutos.
        /// </summary>
        public static TimeSpan DefaultHandlerLifetime
        {
            get { return TimeSpan.FromTicks(Interlocked.Read(ref _handlerLifetimeTicks)); }
            set
            {
                if (value <= TimeSpan.Zero)
                    throw new ArgumentOutOfRangeException(nameof(value));
                Interlocked.Exchange(ref _handlerLifetimeTicks, value.Ticks);
            }
        }

        /// <summary>
        /// Tempo mínimo, após expirar, antes de um handler padrão ser descartado. Deve ser maior que o maior
        /// timeout de requisição usado. Além disso, um handler expirado nunca é descartado enquanto houver
        /// requisições em andamento sobre ele (contagem de referências). Padrão: 5 minutos.
        /// </summary>
        public static TimeSpan ExpiredHandlerGracePeriod
        {
            get { return TimeSpan.FromTicks(Interlocked.Read(ref _gracePeriodTicks)); }
            set
            {
                if (value < TimeSpan.Zero)
                    throw new ArgumentOutOfRangeException(nameof(value));
                Interlocked.Exchange(ref _gracePeriodTicks, value.Ticks);
            }
        }

        /// <summary>
        /// Cria o HttpClient de UMA requisição: provedor da instância, senão o global, senão o handler padrão rotacionado.
        /// O retorno deve ser descartado ao fim da requisição (descarta o HttpClient e libera o handler padrão).
        /// </summary>
        internal static Lease Acquire(Func<HttpClient> perInstanceProvider)
        {
            Func<HttpClient> provider = perInstanceProvider ?? ClientProvider;
            if (provider != null)
            {
                HttpClient client = provider();
                if (client == null)
                    throw new InvalidOperationException("SdkHttp: o provedor de HttpClient retornou null");
                SetInfiniteTimeout(client);
                return new Lease(client, null);
            }

            DefaultHandlerPool.Entry entry = DefaultHandlerPool.Rent();
            try
            {
                HttpClient client = new HttpClient(entry.Handler, false);
                SetInfiniteTimeout(client);
                return new Lease(client, entry);
            }
            catch
            {
                DefaultHandlerPool.Return(entry);
                throw;
            }
        }

        private static void SetInfiniteTimeout(HttpClient client)
        {
            try
            {
                client.Timeout = Timeout.InfiniteTimeSpan;
            }
            catch (InvalidOperationException)
            {
                // Cliente já usado (fora do contrato): o timeout do SDK continua valendo via CancellationToken.
            }
        }

        /// <summary>
        /// HttpClient de uma única requisição. Dispose descarta o cliente e devolve o handler padrão ao pool.
        /// </summary>
        internal sealed class Lease : IDisposable
        {
            private DefaultHandlerPool.Entry _entry;
            public HttpClient Client { get; private set; }

            internal Lease(HttpClient client, DefaultHandlerPool.Entry entry)
            {
                Client = client;
                _entry = entry;
            }

            public void Dispose()
            {
                HttpClient c = Client;
                Client = null;
                try
                {
                    if (c != null)
                        c.Dispose();
                }
                finally
                {
                    DefaultHandlerPool.Entry e = Interlocked.Exchange(ref _entry, null);
                    if (e != null)
                        DefaultHandlerPool.Return(e);
                }
            }
        }

        /// <summary>
        /// Handler padrão interno: um HttpClientHandler compartilhado, trocado a cada DefaultHandlerLifetime.
        /// Handlers expirados só são descartados quando não há requisições em andamento sobre eles
        /// E já passou ExpiredHandlerGracePeriod desde a expiração.
        /// </summary>
        internal static class DefaultHandlerPool
        {
            private static readonly object _gate = new object();
            private static Entry _current;
            private static readonly List<Entry> _expired = new List<Entry>();

            internal sealed class Entry
            {
                internal readonly HttpClientHandler Handler;
                internal readonly long CreatedTimestamp;
                internal long ExpiredTimestamp;
                internal bool Expired;
                internal bool Disposed;
                internal int Leases;

                internal Entry(HttpClientHandler handler, long created)
                {
                    Handler = handler;
                    CreatedTimestamp = created;
                }
            }

            internal static HttpClientHandler CreateHandler()
            {
                var handler = new HttpClientHandler();
                handler.UseCookies = false;
                // Mesma política do SSLValidator (aceita qualquer certificado), porém restrita a este handler
                handler.ServerCertificateCustomValidationCallback = (message, certificate, chain, errors) => true;
                return handler;
            }

            private static long ToStopwatchTicks(long timeSpanTicks)
            {
                return (long)(timeSpanTicks * ((double)Stopwatch.Frequency / TimeSpan.TicksPerSecond));
            }

            internal static Entry Rent()
            {
                List<Entry> toDispose;
                Entry entry;
                lock (_gate)
                {
                    long now = Stopwatch.GetTimestamp();
                    if (_current == null || now - _current.CreatedTimestamp >= ToStopwatchTicks(Interlocked.Read(ref _handlerLifetimeTicks)))
                    {
                        if (_current != null)
                        {
                            _current.Expired = true;
                            _current.ExpiredTimestamp = now;
                            _expired.Add(_current);
                        }
                        _current = new Entry(CreateHandler(), now);
                    }
                    entry = _current;
                    entry.Leases++;
                    toDispose = CollectDisposable_NoLock(now);
                }
                DisposeAll(toDispose);
                return entry;
            }

            internal static void Return(Entry entry)
            {
                List<Entry> toDispose;
                lock (_gate)
                {
                    entry.Leases--;
                    toDispose = CollectDisposable_NoLock(Stopwatch.GetTimestamp());
                }
                DisposeAll(toDispose);
            }

            private static List<Entry> CollectDisposable_NoLock(long now)
            {
                if (_expired.Count == 0)
                    return null;

                long grace = ToStopwatchTicks(Interlocked.Read(ref _gracePeriodTicks));
                List<Entry> result = null;
                for (int i = _expired.Count - 1; i >= 0; i--)
                {
                    Entry e = _expired[i];
                    if (e.Leases <= 0 && !e.Disposed && now - e.ExpiredTimestamp >= grace)
                    {
                        e.Disposed = true;
                        _expired.RemoveAt(i);
                        if (result == null)
                            result = new List<Entry>();
                        result.Add(e);
                    }
                }
                return result;
            }

            private static void DisposeAll(List<Entry> entries)
            {
                if (entries == null)
                    return;
                foreach (Entry e in entries)
                {
                    try
                    {
                        e.Handler.Dispose();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
    }
}
