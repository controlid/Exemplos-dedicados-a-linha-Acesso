using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ControliD.iDAccess
{
    // Camada assíncrona (XxxAsync): cada método espelha o equivalente síncrono (mesmos endpoints, corpos,
    // timeouts e atualizações de estado), usando WebJson.JsonCommandAsync. A classe continua NÃO thread-safe:
    // use uma operação por vez por instância de Device.
    public partial class Device
    {
        /// <summary>
        /// Provedor de HttpClient desta instância para os métodos XxxAsync (null = <see cref="SdkHttp.ClientProvider"/>,
        /// e se este também for null, o handler padrão interno do SDK). Mesmo contrato de <see cref="SdkHttp.ClientProvider"/>:
        /// cada chamada deve devolver um NOVO HttpClient (descartado pelo SDK após a requisição).
        /// </summary>
        public Func<HttpClient> HttpClientProvider { get; set; }

        #region Sessão

        /// <summary>
        /// Versão assíncrona de <see cref="Connect"/>
        /// </summary>
        public async Task ConnectAsync(string cIP_DNS_URL = null, string cLogin = null, string cPassword = null, bool? useSSL = null, int? nPort = null, CancellationToken ct = default(CancellationToken))
        {
            URL = cIP_DNS_URL ?? URL;
            Login = cLogin ?? Login;
            Password = cPassword ?? Password;
            SSL = useSSL ?? SSL;
            Port = nPort ?? Port;

            // Sem dados não fa nada!
            if (URL == null || Login == null || Password == null)
                throw new cidException(ErroCodes.LoginRequestFields, "Invalid Request Start");

            // Limpa qualquer espaço desnecessário (evita erros de colagem)
            URL = URL.Trim().ToLower();

            // Foi passado o IP/DNS em vez da URL, então converte para a URL direto
            if (!URL.StartsWith("http") && !URL.Contains("://"))
            {
                if (SSL)
                    URL = "https://" + URL + (Port == 443 ? "" : (":" + Port));
                else
                    URL = "http://" + URL + (Port == 80 ? "" : (":" + Port));
            }

            // Deve ser sempre terminado por '/' pois os comandos serão concatenados diretamente
            if (!URL.EndsWith("/"))
                URL += "/";

            LoginRequest lreq = new LoginRequest();
            lreq.login = Login;
            lreq.password = Password;

#if HID
            var result = await WebJson.JsonCommandAsync<LoginResult>(URL + "hidlogin.fcgi", lreq, null, TimeOut, null, HttpClientProvider, ct).ConfigureAwait(false);
#else
            var result = await WebJson.JsonCommandAsync<LoginResult>(URL + "login.fcgi", lreq, null, TimeOut, null, HttpClientProvider, ct).ConfigureAwait(false);
#endif

            if (result is LoginResult)
            {
                LoginResult dados = (LoginResult)result;
                if (dados.session == null)
                    throw new cidException(ErroCodes.LoginRequestFields, "Invalid User/Password");

                dtConnection = dtLastCommand = DateTime.Now;
                Session = dados.session;
            }
            else
                throw new cidException(ErroCodes.LoginInvalid, result.error ?? "Erro de conexão");
        }

        /// <summary>
        /// Versão assíncrona de <see cref="Disconnect"/>: ignora qualquer erro (exceto o cancelamento pelo chamador)
        /// </summary>
        public async Task DisconnectAsync(CancellationToken ct = default(CancellationToken))
        {
            try
            {
                string cLastSession = Session;
                Session = null;
                await WebJson.JsonCommandAsync<string>(URL + "logout.fcgi?session=" + cLastSession, null, null, 0, null, HttpClientProvider, ct).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception)
            {
            }
        }

        private async Task CheckSessionAsync(CancellationToken ct)
        {
            WebJson.WriteLog();

            if (Session == null)
                await ConnectAsync(ct: ct).ConfigureAwait(false);
            else
            {
                var result = await WebJson.JsonCommandAsync<SessionResult>(URL + "session_is_valid.fcgi?session=" + Session, null, null, 0, null, HttpClientProvider, ct).ConfigureAwait(false);
                if (!result.session_is_valid)
                    await ConnectAsync(ct: ct).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Executa um comando (com verificação de sessão) no Device usando o TimeOut da instância
        /// </summary>
        private async Task<T> JsonAsync<T>(string url, object req, CancellationToken ct, string cMethod = null, System.Drawing.Imaging.ImageFormat format = null)
        {
            return await WebJson.JsonCommandAsync<T>(url, req, cMethod, TimeOut, format, HttpClientProvider, ct).ConfigureAwait(false);
        }

        #endregion

        #region Comandos genéricos

        /// <summary>
        /// Versão assíncrona de <see cref="Command{T}"/>
        /// </summary>
        public async Task<T> CommandAsync<T>(string cmd, object req, string query = null, CancellationToken ct = default(CancellationToken))
        {
            await CheckSessionAsync(ct).ConfigureAwait(false);
            var urlRequest = URL + cmd + ".fcgi?session=" + Session;
            if (!String.IsNullOrEmpty(query))
                urlRequest += $"&{query}";
            return await WebJson.JsonCommandAsync<T>(urlRequest, req, null, TimeOut, null, HttpClientProvider, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Versão assíncrona de <see cref="Send"/>
        /// </summary>
        public async Task<string> SendAsync(string cmd, string data, CancellationToken ct = default(CancellationToken))
        {
            await CheckSessionAsync(ct).ConfigureAwait(false);
            return await JsonAsync<string>(URL + cmd + ".fcgi?session=" + Session, data, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Versão assíncrona de <see cref="Count{T}"/> (como no síncrono, verifica a sessão duas vezes)
        /// </summary>
        public async Task<long> CountAsync<T>(WhereObjects oWhere = null, CancellationToken ct = default(CancellationToken))
        {
            await CheckSessionAsync(ct).ConfigureAwait(false);
            ObjectResult result = await CommandAsync<ObjectResult>("load_objects", new ObjectRequest<T, WhereObjects>() { fields = new string[] { "COUNT(*)" }, where = oWhere }, null, ct).ConfigureAwait(false);
            Type tpT = typeof(T);
            Type tpR = result.GetType();
            object[] itens = (object[])tpR.GetField(tpT.Name.ToLower()).GetValue(result);
            return ((GenericCount)itens[0]).Count;
        }

        /// <summary>
        /// Versão assíncrona de <see cref="List{T}(object, OrderTypes, string, int, int)"/>
        /// </summary>
        public async Task<T[]> ListAsync<T>(Object oWhere = null, OrderTypes ot = OrderTypes.None, string cName = null, int nOffSet = 0, int nLimit = 0, CancellationToken ct = default(CancellationToken)) where T : GenericItem
        {
            if (oWhere == null)
                return await ListAsync<T, WhereObjects>(null, ot, cName, nOffSet, nLimit, ct).ConfigureAwait(false);
            else if (oWhere is string)
                return await ListAsync<T, string>(oWhere as string, ot, cName, nOffSet, nLimit, ct).ConfigureAwait(false);
            else if (oWhere is WhereCondional)
                return await ListAsync<T, WhereCondional>(oWhere as WhereCondional, ot, cName, nOffSet, nLimit, ct).ConfigureAwait(false);
            else if (oWhere is WhereObjects)
                return await ListAsync<T, WhereObjects>(oWhere as WhereObjects, ot, cName, nOffSet, nLimit, ct).ConfigureAwait(false);
            else if (oWhere is GenericItem)
                return await ListAsync<T, WhereObjects>(WhereByObject(oWhere as GenericItem), ot, cName, nOffSet, nLimit, ct).ConfigureAwait(false);
            else if (oWhere is GenericCount)
                return await ListAsync<T, WhereObjects>(WhereByObject(oWhere as GenericCount), ot, cName, nOffSet, nLimit, ct).ConfigureAwait(false);
            else if (oWhere is GenericObject)
                return await ListAsync<T, WhereObjects>(WhereByObject(oWhere as GenericObject), ot, cName, nOffSet, nLimit, ct).ConfigureAwait(false);
            else if (oWhere is GenericObjectName)
                return await ListAsync<T, WhereObjects>(WhereByObject(oWhere as GenericObjectName), ot, cName, nOffSet, nLimit, ct).ConfigureAwait(false);
            else
                throw new NotSupportedException("Tipo do 'Where' não suportado: " + oWhere.GetType().Name);
        }

        /// <summary>
        /// Lista todos os itens do tipo T (sem filtro), observando o token de cancelamento.
        /// Sobrecarga exata para que <c>ListAsync&lt;T&gt;(ct)</c> associe o token a <paramref name="ct"/>
        /// em vez de convertê-lo (boxing) para o parâmetro 'oWhere' do tipo Object.
        /// </summary>
        public Task<T[]> ListAsync<T>(CancellationToken ct) where T : GenericItem
        {
            return ListAsync<T>((Object)null, OrderTypes.None, null, 0, 0, ct);
        }

        /// <summary>
        /// Retorna o primeiro item do tipo T (sem filtro), observando o token de cancelamento.
        /// Sobrecarga exata para que <c>FirstAsync&lt;T&gt;(ct)</c> associe o token a <paramref name="ct"/>
        /// em vez de convertê-lo (boxing) para o parâmetro 'oWhere' do tipo Object.
        /// </summary>
        public Task<T> FirstAsync<T>(CancellationToken ct) where T : GenericItem
        {
            return FirstAsync<T>((Object)null, OrderTypes.None, null, 0, 0, ct);
        }

        /// <summary>
        /// Versão assíncrona de <see cref="First{T}"/>
        /// </summary>
        public async Task<T> FirstAsync<T>(Object oWhere = null, OrderTypes ot = OrderTypes.None, string cName = null, int nOffSet = 0, int nLimit = 0, CancellationToken ct = default(CancellationToken)) where T : GenericItem
        {
            T[] item = await ListAsync<T>(oWhere, ot, cName, nOffSet, nLimit, ct).ConfigureAwait(false);
            if (item.Length == 0)
                return null;
            else
                return item[0];
        }

        private async Task<T[]> ListAsync<T, W>(W oWhere, OrderTypes ot, string cName, int nOffSet, int nLimit, CancellationToken ct) where T : GenericItem
        {
            object result;
            if (oWhere is string)
                result = await CommandAsync<ObjectResult2<T>>("load_objects", oWhere as string, null, ct).ConfigureAwait(false);
            else
                result = await CommandAsync<ObjectResult>("load_objects", new ObjectRequest<T, W>()
                {
                    where = oWhere,
                    OrderType = ot,
                    offset = nOffSet,
                    limit = nLimit
                }, null, ct).ConfigureAwait(false);
            return GetResults<T>(result, cName);
        }

        /// <summary>
        /// Versão assíncrona de <see cref="GetAccessEvents"/>
        /// </summary>
        public async Task<AccessEvent[]> GetAccessEventsAsync(string uuid, CancellationToken ct = default(CancellationToken))
        {
            var request = new AccessEventsRequest()
            {
                where = new AccessEventsWhere()
                {
                    access_events = new AccessEventsFilter() { identification = new[] { uuid } }
                }
            };
            var result = await CommandAsync<AccessEventsResult>("load_objects", request, null, ct).ConfigureAwait(false);
            return result.access_events;
        }

        /// <summary>
        /// Versão assíncrona de <see cref="Load{T}"/>
        /// </summary>
        public async Task<T> LoadAsync<T>(long nID, CancellationToken ct = default(CancellationToken)) where T : GenericObject, new()
        {
            return await FirstAsync<T>(WhereByObject<T>(new T() { id = nID }), ct: ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Versão assíncrona de <see cref="AddRange{T}(T[], int, bool)"/>
        /// </summary>
        public Task<long[]> AddRangeAsync<T>(T[] objArray, int maxBlock = 250, bool lTryRange = false, CancellationToken ct = default(CancellationToken)) where T : GenericItem
        {
            if (objArray == null || objArray.Length == 0)
                return Task.FromResult(new long[] { });

            var itens = new List<T>();
            itens.AddRange(objArray);
            return AddRangeAsync<T>(itens, maxBlock, lTryRange, ct);
        }

        /// <summary>
        /// Versão assíncrona de <see cref="AddRange{T}(List{T}, int, bool)"/>. Com lTryRange os erros vão para LastError
        /// (exceto o cancelamento pelo chamador, que é propagado).
        /// </summary>
        public async Task<long[]> AddRangeAsync<T>(List<T> objArray, int maxBlock = 250, bool lTryRange = false, CancellationToken ct = default(CancellationToken)) where T : GenericItem
        {
            if (objArray == null || objArray.Count == 0)
                return new long[] { };

            int start = 0;
            int count;
            var ids = new List<long>();
            while (start < objArray.Count)
            {
                count = objArray.Count - start;
                if (count > maxBlock)
                    count = maxBlock;

                T[] itens = new T[count];
                objArray.CopyTo(start, itens, 0, count);
                start += count;
                try
                {
                    ids.AddRange(
                    (await CommandAsync<ObjectResult>("create_objects", new ObjectRequest<T[], WhereObjects>()
                    {
                        values = itens,
                        limit = 0 // para não enviar limit!
                    }, null, ct).ConfigureAwait(false)).ids);
                }
                catch (OperationCanceledException) when (ct.IsCancellationRequested)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    if (lTryRange)
                        LastError = ex;
                    else
                        throw;
                }
            }
            return ids.ToArray();
        }

        /// <summary>
        /// Versão assíncrona de <see cref="DestroyRange{T}"/>. Como no síncrono, NÃO verifica a sessão (usa a Session atual).
        /// </summary>
        public async Task<long> DestroyRangeAsync<T>(long[] ids, int maxBlock = 250, CancellationToken ct = default(CancellationToken)) where T : GenericItem
        {
            if (ids == null || ids.Length == 0)
                return 0;

            int changes = 0;
            string cName = typeof(T).Name.ToLower();
            int offset = 0;

            while (offset < ids.Length)
            {
                string cList = "";

                int blockSize = ids.Length - offset;
                if (blockSize > maxBlock)
                    blockSize = maxBlock;

                for (int i = offset; i < offset + blockSize; i++)
                    cList += ids[i] + ",";

                if (cList.Length > 0)
                {
                    cList = cList.Substring(0, cList.Length - 1);
                    offset += blockSize;

                    string cmd = "{\"object\":\"" + cName + "\",\"where\":{\"" + cName + "\":{\"id\":[" + cList + "]}}}";
                    var or = await JsonAsync<ObjectResult>(URL + "destroy_objects.fcgi?session=" + Session, cmd, ct).ConfigureAwait(false);
                    changes += or.changes;
                }
                else
                    break;
            }
            return changes;
        }

        /// <summary>
        /// Versão assíncrona de <see cref="Add{T}"/>
        /// </summary>
        public async Task<long> AddAsync<T>(T obj, CancellationToken ct = default(CancellationToken)) where T : GenericItem
        {
            return (await AddRangeAsync<T>(new T[] { obj }, ct: ct).ConfigureAwait(false))[0];
        }

        /// <summary>
        /// Versão assíncrona de <see cref="LoadOrAdd{T}"/>
        /// </summary>
        public async Task<long> LoadOrAddAsync<T>(string cName, CancellationToken ct = default(CancellationToken)) where T : GenericObjectName, new()
        {
            T item = new T() { name = cName };
            T[] items = await ListAsync<T>(item, ct: ct).ConfigureAwait(false);
            if (items.Length == 0)
            {
                long newID = await AddAsync<T>(item, ct).ConfigureAwait(false);
                return newID;
            }
            else if (items.Length > 1)
                throw new Exception("Mais de 1 item com esse nome");

            else if (item.name != items[0].name)
            {
                if (!await ModifyAsync<T>(items[0].id, item, ct).ConfigureAwait(false))
                    return 0; // não conseguiu modificar
            }
            return items[0].id; // Valor Lido, Modificado ou Criado
        }

        /// <summary>
        /// Versão assíncrona de <see cref="Set{T}"/>
        /// </summary>
        public async Task<bool> SetAsync<T>(T item, CancellationToken ct = default(CancellationToken)) where T : GenericItem, new()
        {
            T[] items = await ListAsync<T>(item, ct: ct).ConfigureAwait(false);
            if (items.Length == 0)
            {
                return await AddAsync<T>(item, ct).ConfigureAwait(false) > 0;
            }
            else if (items.Length > 1)
                throw new Exception("Mais de 1 item com esse nome");

            else if (item.IsEquals(items[0]))
                return true;

            else if (await ModifyWhereAsync<T, WhereObjects>(WhereByObject<T>(item), item, ct).ConfigureAwait(false) == 1)
                return true;

            return false;
        }

        /// <summary>
        /// Versão assíncrona de <see cref="LoadOrSet{T}"/>
        /// </summary>
        public async Task<T> LoadOrSetAsync<T>(long id, T item, CancellationToken ct = default(CancellationToken)) where T : GenericObject, new()
        {
            T itemExist;
            if (id != 0)
                itemExist = await LoadAsync<T>(id, ct).ConfigureAwait(false);
            else
                itemExist = await FirstAsync<T>(item, ct: ct).ConfigureAwait(false);

            if (itemExist == null)
            {
                item.id = id;
                long newID = await AddAsync<T>(item, ct).ConfigureAwait(false);
                if (newID > 0 && newID != id)
                    item.id = newID; // Ops... não conseguiu criar com o ID esperado, mas criou!
            }
            else if (item.IsEquals(itemExist))
                return itemExist;

            else if (!await ModifyAsync<T>(id, item, ct).ConfigureAwait(false))
                return itemExist; // Ops... não conseguiu alterar, mas leu!

            return item; // Valor Modificado ou Criado
        }

        /// <summary>
        /// Versão assíncrona de <see cref="Modify{T}(long, T)"/>
        /// </summary>
        public async Task<bool> ModifyAsync<T>(long nID, T obj, CancellationToken ct = default(CancellationToken)) where T : GenericObject, new()
        {
            return await ModifyWhereAsync<T, WhereObjects>(WhereByObject<T>(new T() { id = nID }), obj, ct).ConfigureAwait(false) == 1;
        }

        /// <summary>
        /// Versão assíncrona de <see cref="Modify{T}(T, T)"/>
        /// </summary>
        public async Task<int> ModifyAsync<T>(T item, T obj, CancellationToken ct = default(CancellationToken)) where T : GenericItem, new()
        {
            return await ModifyWhereAsync<T, WhereObjects>(WhereByObject<T>(item), obj, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Versão assíncrona de <see cref="ModifyWhere{T, W}"/>
        /// </summary>
        public async Task<int> ModifyWhereAsync<T, W>(W where, T obj, CancellationToken ct = default(CancellationToken)) where T : GenericItem, new()
        {
            return (await CommandAsync<ObjectResult>("modify_objects", new ObjectRequest<T, W>()
            {
                values = obj,
                limit = 0, // para não enviar limit!
                where = where
            }, null, ct).ConfigureAwait(false)).changes;
        }

        /// <summary>
        /// Versão assíncrona de <see cref="Destroy{T}(long)"/>
        /// </summary>
        public async Task<bool> DestroyAsync<T>(long nID, CancellationToken ct = default(CancellationToken)) where T : GenericObject, new()
        {
            return await DestroyWhereAsync<T, WhereObjects>(WhereByObject(new T() { id = nID }), ct).ConfigureAwait(false) == 1;
        }

        /// <summary>
        /// Versão assíncrona de <see cref="Destroy{T}(T)"/>
        /// </summary>
        public async Task<bool> DestroyAsync<T>(T item, CancellationToken ct = default(CancellationToken)) where T : GenericItem, new()
        {
            return await DestroyWhereAsync<T, WhereObjects>(WhereByObject(item), ct).ConfigureAwait(false) > 0;
        }

        /// <summary>
        /// Versão assíncrona de <see cref="DestroyWhere{T, W}"/>
        /// </summary>
        public Task<int> DestroyWhereAsync<T, W>(W Where, CancellationToken ct = default(CancellationToken)) where T : GenericItem
        {
            return DestroyWherePagedAsync<T, W>(Where, 200, ct);
        }

        /// <summary>
        /// Versão assíncrona de <see cref="DestroyWherePaged{T, W}"/>
        /// </summary>
        public async Task<int> DestroyWherePagedAsync<T, W>(W Where, int pageSize = 200, CancellationToken ct = default(CancellationToken)) where T : GenericItem
        {
            int totalDestroyed = 0;
            int changes;

            do
            {
                changes = (await CommandAsync<ObjectResult>("destroy_objects", new ObjectRequest<T, W>()
                {
                    limit = pageSize,
                    where = Where
                }, null, ct).ConfigureAwait(false)).changes;

                totalDestroyed += changes;
            }
            while (changes > 0);

            return totalDestroyed;
        }

        #endregion
    }
}
