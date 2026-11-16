using ControliD.iDAccess;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Resources;
using System.Runtime.ExceptionServices;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ControliD
{
    public partial class WebJson
    {
        /// <summary>
        /// Versão assíncrona de <see cref="JsonCommand{T}"/> (HttpClient). Mesma montagem da requisição
        /// (método, URL, Content-Type, corpo), mesmo tratamento da resposta e mesmo mapeamento de erros:
        /// <see cref="cidException"/> / <see cref="WebException"/>, nunca HttpRequestException/TaskCanceledException.
        /// </summary>
        /// <param name="cURL">URL da exata requisição</param>
        /// <param name="objRequest">Objeto a ser serializado (string, byte[], Bitmap ou DataContract)</param>
        /// <param name="cMethod">Método HTTP (null = POST, ou GET para retorno Bitmap/byte[])</param>
        /// <param name="reqTimeout">Timeout em ms (0 = <see cref="TimeOut"/>). Cobre toda a troca (envio, cabeçalhos e corpo).</param>
        /// <param name="format">Formato da imagem quando objRequest é Bitmap</param>
        /// <param name="clientProvider">Provedor de HttpClient desta chamada (null = <see cref="SdkHttp.ClientProvider"/> ou handler padrão)</param>
        /// <param name="ct">Cancelamento do chamador (gera OperationCanceledException; o timeout gera cidException)</param>
        /// <returns>Retorna a instância prevista, ou gera algum erro</returns>
        /// <exception cref="cidException">Erro com os dados recebidos e enviados quando possível</exception>
        public static async Task<T> JsonCommandAsync<T>(string cURL, object objRequest = null, string cMethod = null, int reqTimeout = 0, ImageFormat format = null, Func<HttpClient> clientProvider = null, CancellationToken ct = default(CancellationToken))
        {
            Random r = new Random();
            string hash = r.Next(999999).ToString("000000");
            string cSend = null;
            string cReceive = null;
            Type tpResult = typeof(T);
            if (reqTimeout == 0)
                reqTimeout = TimeOut;

            try
            {
                T result;
                // Apenas serializa o objeto para fins de teste
                if (cURL == null && objRequest != null)
                {
                    Type tpRequest = objRequest.GetType();
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(objRequest.GetType());
                    using (MemoryStream ms = new MemoryStream())
                    {
                        serializer.WriteObject(ms, objRequest);
                        Byte[] bt = new byte[ms.Position];
                        ms.Position = 0;
                        ms.Read(bt, 0, bt.Length);
                        cSend = UTF8Encoding.UTF8.GetString(bt);
                        WriteLog("TESTE:\r\n" + cSend);
                        return result = (T)((object)cSend); // Deve ser uma string o retorno!
                    }
                }

                WriteLog(hash + " URL: " + cURL);
                if (objRequest != null)
                {
                    if (objRequest.GetType() == typeof(string))
                        WriteLog(hash + " Vai enviar objRequest:" + objRequest.GetType().ToString() + " " + objRequest);
                    else
                    {
                        string objRequest_string = "";
                        using (MemoryStream ms = new MemoryStream())
                        {
                            DataContractJsonSerializer serializer = new DataContractJsonSerializer(objRequest.GetType());
                            serializer.WriteObject(ms, objRequest);
                            Byte[] bt = new byte[ms.Position];
                            ms.Position = 0;
                            ms.Read(bt, 0, bt.Length);
                            objRequest_string = UTF8Encoding.UTF8.GetString(bt);
                        }
                        if (objRequest_string.Length < 2000)
                            WriteLog(hash + " Vai enviar objRequest:" + objRequest.GetType().ToString() + " " + objRequest_string);
                        else
                            WriteLog(hash + " Vai enviar objRequest:" + objRequest.GetType().ToString() + " " + objRequest_string.Length + " bytes");
                    }
                }

                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

                // Mesma validação de WebRequest.Create(cURL)
                if (cURL == null)
                    throw new ArgumentNullException("requestUriString");
                Uri uri = new Uri(cURL);

                // Monta o corpo inteiro antes de enviar (Content-Length sempre presente, como no HttpWebRequest com buffer)
                HttpMethod method;
                byte[] body = null;
                string contentType = null;
                if (tpResult == typeof(Byte[]) || tpResult == typeof(Bitmap) || cMethod == "GET")
                    method = HttpMethod.Get;
                else
                {
                    method = new HttpMethod(cMethod ?? "POST");
                    if (objRequest != null && objRequest.GetType() == typeof(Bitmap))
                    {
                        contentType = "application/octet-stream";
                        Bitmap bmp = (Bitmap)objRequest;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            bmp.Save(ms, format);
                            body = ms.ToArray();
                        }
                    }
                    else if (objRequest != null && objRequest.GetType() == typeof(byte[]))
                    {
                        contentType = "application/octet-stream";
                        body = (byte[])objRequest;
                    }
                    else
                    {
                        contentType = "application/json";
                        if (objRequest != null && objRequest.GetType() == typeof(string))
                        {
                            WriteLog(cSend = (string)objRequest);
                            body = UTF8Encoding.UTF8.GetBytes(cSend);
                        }
                        else if (objRequest != null)
                        {
                            DataContractJsonSerializer serializer = new DataContractJsonSerializer(objRequest.GetType());
                            using (MemoryStream ms = new MemoryStream())
                            {
                                serializer.WriteObject(ms, objRequest);
                                Byte[] bt = new byte[ms.Position];
                                ms.Position = 0;
                                ms.Read(bt, 0, bt.Length);
                                if (Debug)
                                {
                                    cSend = UTF8Encoding.UTF8.GetString(bt);
                                    WriteLog(cSend);
                                }
                                body = bt;
                            }
                        }
                        else
                            body = new byte[0]; // POST sem corpo: Content-Type application/json e Content-Length 0
                    }
                }

                HttpStatusCode status;
                string reason;
                byte[] data;
                using (HttpRequestMessage req = new HttpRequestMessage(method, uri))
                {
                    if (SdkHttp.KeepAlive)
                        req.Version = HttpVersion.Version11;
                    else
                    {
                        // Paridade com o caminho síncrono: HTTP/1.0 e KeepAlive=false
                        req.Version = HttpVersion.Version10;
                        req.Headers.ConnectionClose = true;
                    }
                    req.Headers.ExpectContinue = false;

                    if (body != null)
                    {
                        req.Content = new ByteArrayContent(body);
                        req.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType); // sem charset
                    }

                    using (SdkHttp.Lease lease = SdkHttp.Acquire(clientProvider))
                    using (CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(ct))
                    {
                        cts.CancelAfter(reqTimeout);
                        using (HttpResponseMessage resp = await lease.Client.SendAsync(req, HttpCompletionOption.ResponseContentRead, cts.Token).ConfigureAwait(false))
                        {
                            status = resp.StatusCode;
                            reason = resp.ReasonPhrase;
                            if (resp.Content != null)
                                data = await resp.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                            else
                                data = new byte[0];
                        }
                    }
                }

                // HttpWebRequest gera WebException(ProtocolError) para status final >= 300 (redirecionamentos já seguidos pelo handler)
                if ((int)status >= 300)
                    throw new HttpStatusError(status, reason, data);

                if (tpResult == typeof(Bitmap))
                    result = (T)(object)Bitmap.FromStream(new MemoryStream(data)); // o stream deve permanecer aberto enquanto a imagem existir

                else if (tpResult == typeof(Byte[]))
                    result = (T)((object)data);

                else if (tpResult != typeof(string))
                {
                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(tpResult);
                    if (Debug)
                    {
                        using (StreamReader sr = new StreamReader(new MemoryStream(data)))
                            cReceive = sr.ReadToEnd();

                        WriteLog(cReceive);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            byte[] bt = UTF8Encoding.UTF8.GetBytes(cReceive);
                            ms.Write(bt, 0, bt.Length);
                            ms.Position = 0;
                            result = (T)deserializer.ReadObject(ms);
                        }
                    }
                    else
                    {
                        T obj_retorno;
                        using (MemoryStream rs = new MemoryStream(data))
                            obj_retorno = (T)deserializer.ReadObject(rs);
                        string obj_retorno_string = "";
                        using (MemoryStream ms = new MemoryStream())
                        {
                            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj_retorno.GetType());
                            serializer.WriteObject(ms, obj_retorno);
                            Byte[] bt = new byte[ms.Position];
                            ms.Position = 0;
                            ms.Read(bt, 0, bt.Length);
                            obj_retorno_string = UTF8Encoding.UTF8.GetString(bt);
                        }
                        WriteLog(hash + " retorno: " + obj_retorno.GetType().ToString() + " " + obj_retorno_string + "\r\n");

                        if (tpResult == typeof(StatusResult) && ((StatusResult)(object)obj_retorno).Codigo == 0 && ((StatusResult)(object)obj_retorno).Status == null)
                            ((StatusResult)(object)obj_retorno).Codigo = 200;

                        return obj_retorno;
                    }
                }
                else
                {
                    using (StreamReader sr = new StreamReader(new MemoryStream(data)))
                        cReceive = sr.ReadToEnd();

                    result = (T)((object)cReceive);
                }

                if (result == null)
                {
                    WriteLog("NULL RESULT");
                    throw new cidException(ErroCodes.JsonCommand, "No response");
                }
                else
                {
                    WriteLog(hash + " retorno: " + result.ToString() + "\r\n");
                    if (tpResult == typeof(StatusResult) && ((StatusResult)(object)result).Codigo == 0 && ((StatusResult)(object)result).Status == null)
                        ((StatusResult)(object)result).Codigo = 200;

                    return result;
                }
            }
            catch (OperationCanceledException oce) when (ct.IsCancellationRequested)
            {
                // Cancelado pelo chamador (não é timeout)
                throw new OperationCanceledException(oce.Message, oce, ct);
            }
            catch (Exception caught)
            {
                // Converte as exceções do HttpClient para o equivalente do HttpWebRequest e aplica as mesmas regras do JsonCommand
                Exception ex = caught;
                WebException wex = null;
                HttpStatusError httpError = caught as HttpStatusError;
                bool timedOut = false;

                if (httpError != null)
                    ex = wex = httpError.WebException;
                else if (caught is OperationCanceledException)
                {
                    timedOut = true;
                    ex = wex = WithStackTrace(new WebException(NetTimeoutMessage(), caught, WebExceptionStatus.Timeout, null));
                }
                else if (caught is HttpRequestException && caught.InnerException != null)
                    ex = caught.InnerException; // no .NET Framework é a WebException real (ConnectFailure, NameResolutionFailure...)

                if (tpResult == typeof(Bitmap))
                    return (T)((object)null); // Sem imagem
                else if (ex is cidException)
                    ExceptionDispatchInfo.Capture(ex).Throw();
                else if (wex != null)
                {
                }
                else if (ex is WebException)
                    wex = (WebException)ex;
                else if (ex.InnerException != null && ex.InnerException is WebException)
                    wex = (WebException)ex.InnerException;

                WriteLog(hash + " ERRO HTTP cURL:" + cURL + " " + ex.Message);
                if (objRequest != null)
                {
                    WriteLog(hash + " ERRO HTTP objRequest:" + objRequest.GetType().ToString() + " " + Stringify(objRequest));
                }

                if (wex != null)
                {
                    WriteLog(hash + " ERRO HTTP: " + wex.Message + " " + cURL + " " + "\r\n" + wex.StackTrace);
                    if (httpError != null || wex.Response != null)
                    {
                        HttpStatusCode statusCode;
                        if (httpError != null)
                        {
                            using (StreamReader sr = new StreamReader(new MemoryStream(httpError.Body)))
                                cReceive = sr.ReadToEnd();
                            statusCode = httpError.StatusCode;
                        }
                        else
                        {
                            HttpWebResponse response = (HttpWebResponse)wex.Response;
                            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                                cReceive = sr.ReadToEnd();
                            statusCode = response.StatusCode;
                        }

                        WriteLog(cReceive, false);
                        if (cReceive.Contains("{")) // provavel JSON!
                        {
                            object er = null;
                            try
                            {
                                DataContractJsonSerializer deserializer;
                                if (tpResult.IsSubclassOf(typeof(StatusResult)))
                                    deserializer = new DataContractJsonSerializer(typeof(T));
                                else
                                    deserializer = new DataContractJsonSerializer(typeof(StatusResult));

                                using (MemoryStream ms = new MemoryStream())
                                {
                                    byte[] bt = UTF8Encoding.UTF8.GetBytes(cReceive);
                                    ms.Write(bt, 0, bt.Length);
                                    ms.Position = 0;
                                    er = deserializer.ReadObject(ms);
                                }
                            }
                            catch (Exception)
                            {
                            }

                            if (er != null)
                            {
                                if (tpResult == typeof(StatusResult) || tpResult.IsSubclassOf(typeof(StatusResult)))
                                    return (T)((object)er);
                                else
                                    throw new cidException(ErroCodes.JsonCommand, wex, cSend, cReceive, statusCode);
                            }
                            else
                                throw new cidException(ErroCodes.JsonCommand, wex, cSend, cReceive, statusCode);
                        }
                        else
                            throw new cidException(ErroCodes.JsonCommand, wex, cSend, cReceive, 0);
                    }
                    else
                    {
                        if (timedOut) // sempre cidException, com "timed out" e "timeout" na mensagem
                            throw new cidException(TimedOutMessage + " - timeout: " + reqTimeout, ErroCodes.JsonCommand, wex, cSend, cReceive, 0);
                        else if (ex.Message.Contains("tempo limite")
                         || ex.Message.Contains("timeout")
                         || ex.Message.Contains("RanToCompletion"))// (RanToCompletion, Faulted ou Canceled).
                            throw new cidException(ex.Message + " - timeout: " + reqTimeout, ErroCodes.JsonCommand, ex, cSend, cReceive, 0);
                        else
                            ExceptionDispatchInfo.Capture(wex).Throw();
                    }
                }
                else
                {
                    WriteLog("ERRO: " + ex.Message + "\r\n" + ex.StackTrace);

                    if (ex.Message.Contains("RanToCompletion"))// (RanToCompletion, Faulted ou Canceled).
                        throw new cidException("timeout: " + reqTimeout, ErroCodes.JsonCommand, ex, cSend, cReceive, 0);
                    else
                        throw new cidException(ErroCodes.JsonCommand, ex, cSend, cReceive, 0);
                }
                throw; // inalcançável (ExceptionDispatchInfo.Throw)
            }
        }

        /// <summary>
        /// Mensagem dos timeouts do caminho assíncrono (contém "timed out" e, com o sufixo, "timeout").
        /// </summary>
        private const string TimedOutMessage = "The operation has timed out";

        /// <summary>
        /// Resposta HTTP com status >= 300 (equivalente ao WebException ProtocolError do HttpWebRequest).
        /// Uso interno do JsonCommandAsync: nunca sai do método.
        /// </summary>
        private sealed class HttpStatusError : Exception
        {
            public HttpStatusCode StatusCode { get; private set; }
            public byte[] Body { get; private set; }
            public WebException WebException { get; private set; }

            public HttpStatusError(HttpStatusCode status, string reason, byte[] body)
                : base("HTTP " + (int)status)
            {
                StatusCode = status;
                Body = body ?? new byte[0];
                // Sem HttpWebResponse (não é possível criar um), Response fica null
                WebException = WithStackTrace(new WebException(ServerErrorMessage(status, reason), null, WebExceptionStatus.ProtocolError, null));
            }
        }

        /// <summary>
        /// Lança e captura a exceção para que StackTrace seja preenchido (usado no log, como no caminho síncrono)
        /// </summary>
        private static WebException WithStackTrace(WebException wex)
        {
            try
            {
                throw wex;
            }
            catch (WebException e)
            {
                return e;
            }
        }

        private static ResourceManager _systemResources;

        /// <summary>
        /// Texto localizado do System.dll (o mesmo usado pelo HttpWebRequest), ou null
        /// </summary>
        private static string SystemResource(string name)
        {
            try
            {
                ResourceManager rm = _systemResources;
                if (rm == null)
                    _systemResources = rm = new ResourceManager("System", typeof(WebException).Assembly);
                return rm.GetString(name);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Mesma mensagem do HttpWebRequest para ProtocolError: "The remote server returned an error: (400) Bad Request."
        /// </summary>
        private static string ServerErrorMessage(HttpStatusCode status, string reason)
        {
            string statusText = "(" + ((int)status).ToString(NumberFormatInfo.InvariantInfo) + ")";
            string statusMessage = SystemResource("net_httpstatuscode_" + status.ToString());
            if (!String.IsNullOrEmpty(statusMessage))
                statusText += " " + statusMessage;
            else if (!String.IsNullOrEmpty(reason))
                statusText += " " + reason;

            string fmt = SystemResource("net_servererror");
            if (!String.IsNullOrEmpty(fmt))
            {
                try
                {
                    return String.Format(CultureInfo.CurrentCulture, fmt, statusText);
                }
                catch (FormatException)
                {
                }
            }
            return "The remote server returned an error: " + statusText + ".";
        }

        /// <summary>
        /// Mesma mensagem do WebException de timeout do HttpWebRequest (localizada)
        /// </summary>
        private static string NetTimeoutMessage()
        {
            string msg = SystemResource("net_timeout");
            return String.IsNullOrEmpty(msg) ? TimedOutMessage : msg;
        }
    }
}
