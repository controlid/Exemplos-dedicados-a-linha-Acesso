using ControliD.iDAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Runtime.Serialization;

namespace ControliD
{
    public class WebJson
    {
        public static bool Debug = true;
        public static int TimeOut = 10000;
        public static string LogPath;

        // Para configurar o 'ServicePointManager' no primeiro uso
        static WebJson()
        {
            // Para autorizar qualquer certificado SSL
            // http://stackoverflow.com/questions/18454292/system-net-certificatepolicy-to-servercertificatevalidationcallback-accept-all-c
            SSLValidator.OverrideValidation();
            // ServicePointManager.Expect100Continue = false;
            // Lê configurações
            String cConfigApp;

            cConfigApp = ConfigurationManager.AppSettings["sdk_debug"];
            if (cConfigApp != null)
                bool.TryParse(cConfigApp, out Debug);

            cConfigApp = ConfigurationManager.AppSettings["sdk_log"];
            if (cConfigApp != null)
                LogPath = cConfigApp;
        }

        public static string Stringify<T>(T data)
        {
            bool hasDataContract = Attribute.IsDefined(typeof(T), typeof(DataContractAttribute));
            
            if (hasDataContract)
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                MemoryStream stream = new MemoryStream();
                serializer.WriteObject(stream, data);
                byte[] bt = new byte[stream.Position];
                stream.Position = 0;
                stream.Read(bt, 0, bt.Length);
                return Encoding.UTF8.GetString(bt);
            }
            else
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                string data_str = js.Serialize(data);
                return data_str;
            }
            
        }

        public static T Parse<T>(string json)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            byte[] byteArray = Encoding.UTF8.GetBytes(json);
            MemoryStream stream = new MemoryStream(byteArray);
            return (T)serializer.ReadObject(stream);
        }

        /// <summary>
        /// Se 'WriteLog' estiver ligado será criado um arquivo com nome "log-cidsdk.txt" contendo informações enviadas a esta função
        /// </summary>
        /// <param name="logMessage">Valores a serem incluidos no Log</param>
        public static void WriteLog(string logMessage = null, bool lAddTime = true)
        {
            if (LogPath == null)
                return;
            try
            {
                FileInfo fi = new FileInfo(LogPath);
                //Console.WriteLine(fi.FullName);
                using (StreamWriter w = fi.AppendText())
                {
                    if (logMessage == null)
                        w.WriteLine();
                    else if (lAddTime)
                        w.WriteLine("{0:dd/MM/yyyy HH:mm:ss.fff} {1}", DateTime.Now, logMessage);
                    else
                        w.WriteLine(logMessage);

                    w.Close();
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Envia um comando JSON a um equipamento iDAccess/REPiDClass
        /// </summary>
        /// <param name="cURL">URL da exata requisição</param>
        /// <param name="objRequest">Objeto a ser serializado</param>
        /// <param name="cMethod">formato da imagem</param>
        /// <returns>Retorna a instância prevista, ou gera algum erro</returns>
        /// <exception cref="cidException">Erro com os dados recebidos e enviados quando possível</exception>
        public static T JsonCommand<T>(string cURL, object objRequest = null, string cMethod = null, int reqTimeout = 0, ImageFormat format = null)
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
                        if (objRequest_string.Length<2000)
                            WriteLog(hash + " Vai enviar objRequest:" + objRequest.GetType().ToString() + " " + objRequest_string);
                        else
                            WriteLog(hash + " Vai enviar objRequest:" + objRequest.GetType().ToString() + " " + objRequest_string.Length + " bytes");
                    }
                }
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var request = (HttpWebRequest)WebRequest.Create(cURL);
                request.Timeout = reqTimeout;
                request.KeepAlive = false;
                request.ProtocolVersion = HttpVersion.Version10;
                request.ServicePoint.Expect100Continue = false;

                if (tpResult == typeof(Byte[]) || tpResult == typeof(Bitmap) || cMethod == "GET")
                    request.Method = "GET";
                else
                {
                    request.Method = cMethod ?? "POST";
                    if (objRequest != null && objRequest.GetType() == typeof(Bitmap))
                    {
                        request.ContentType = "application/octet-stream";
                        Bitmap bmp = (Bitmap)objRequest;
                        using (Task<Stream> send = request.GetRequestStreamAsync())
                        {
                            send.Wait(reqTimeout);
                            bmp.Save(send.Result, format);
                        }
                    }
                    else if (objRequest != null && objRequest.GetType() == typeof(byte[]))
                    {
                        request.ContentType = "application/octet-stream";
                        using (Task<Stream> send = request.GetRequestStreamAsync())
                        {
                            byte[] bt = (byte[])objRequest;
                            send.Wait(reqTimeout);
                            send.Result.Write(bt, 0, bt.Length);
                        }
                    }
                    else
                    {
                        request.ContentType = "application/json";
                        using (Stream send = request.GetRequestStream())
                        //using (Task<Stream> send = request.GetRequestStreamAsync())
                        {
                            // Faz outras coisas enquanto aguarda a resposta da conexão
                            if (objRequest != null && objRequest.GetType() == typeof(string))
                            {
                                WriteLog(cSend = (string)objRequest);
                                Byte[] bt = UTF8Encoding.UTF8.GetBytes(cSend);
                                //send.Wait(reqTimeout);
                                //send.Result.Write(bt, 0, bt.Length);
                                send.Write(bt, 0, bt.Length);
                            }
                            else if (objRequest != null)
                            {
                                DataContractJsonSerializer serializer = new DataContractJsonSerializer(objRequest.GetType());
                                if (Debug)
                                {
                                    using (MemoryStream ms = new MemoryStream())
                                    {
                                        serializer.WriteObject(ms, objRequest);
                                        Byte[] bt = new byte[ms.Position];
                                        ms.Position = 0;
                                        ms.Read(bt, 0, bt.Length);
                                        cSend = UTF8Encoding.UTF8.GetString(bt);
                                        WriteLog(cSend);
                                        //send.Wait(reqTimeout);
                                        //send.Result.Write(bt, 0, bt.Length);
                                        send.Write(bt, 0, bt.Length);
                                    }
                                }
                                else
                                {
                                    //send.Wait(reqTimeout);
                                    //serializer.WriteObject(send.Result, objRequest);
                                    serializer.WriteObject(send, objRequest);
                                }
                            }
                            else
                            {
                                //send.Wait(reqTimeout);
                            }
                        }
                    }
                }

                using (WebResponse response = request.GetResponse())
                //using (Task<WebResponse> response = request.GetResponseAsync())
                {
                    //response.Wait(reqTimeout);

                    if (tpResult == typeof(Bitmap))
                        //result = (T)(object)Bitmap.FromStream(response.Result.GetResponseStream());
                        result = (T)(object)Bitmap.FromStream(response.GetResponseStream());

                    else if (tpResult == typeof(Byte[]))
                    {
                        List<byte> receive = new List<byte>();
                        //using (Stream str = response.Result.GetResponseStream())
                        using (Stream str = response.GetResponseStream())
                        {
                            // Baixa em um cache de 4K 
                            int nCount;
                            byte[] cache = new byte[4096];
                            while ((nCount = str.Read(cache, 0, cache.Length)) == cache.Length)
                                receive.AddRange(cache);

                            // Remove do cache a área extra
                            if (nCount > 0 && nCount < cache.Length)
                                receive.RemoveRange(cache.Length - nCount, nCount);
                        }
                        result = (T)((object)receive.ToArray());
                    }
                    else if (tpResult != typeof(string))
                    {
                        DataContractJsonSerializer deserializer = new DataContractJsonSerializer(tpResult);
                        if (Debug)
                        {
                            //using (StreamReader sr = new StreamReader(response.Result.GetResponseStream()))
                            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
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
                            T obj_retorno = (T)deserializer.ReadObject(response.GetResponseStream());
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
                        //result = (T)deserializer.ReadObject(response.Result.GetResponseStream());
                    }
                    else
                    {
                        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                            //using (StreamReader sr = new StreamReader(response.Result.GetResponseStream()))
                            cReceive = sr.ReadToEnd();

                        result = (T)((object)cReceive);
                    }
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
            catch (Exception ex)
            {
                // TODO: já como tudo deriva de StatusResult, o ideal seria converter qualquer tipo de erro para este tipo de objeto
                WebException wex = null;
                if (tpResult == typeof(Bitmap))
                    return (T)((object)null); // Sem imagem
                else if (ex is cidException)
                    throw ex;
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
                    if (wex.Response != null)
                    {
                        HttpWebResponse response = (HttpWebResponse)wex.Response;
                        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                            cReceive = sr.ReadToEnd();

                        WriteLog(cReceive, false);
                        if (cReceive.Contains("{")) // provavel JSON!
                        {
                            object er = null;
                            try
                            {
                                DataContractJsonSerializer deserializer;
                                if(tpResult.IsSubclassOf(typeof(StatusResult)))
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
                                    throw new cidException(ErroCodes.JsonCommand, wex, cSend, cReceive, response.StatusCode);
                            }
                            else
                                throw new cidException(ErroCodes.JsonCommand, wex, cSend, cReceive, response.StatusCode);
                        }
                        else
                            throw new cidException(ErroCodes.JsonCommand, wex, cSend, cReceive, 0);
                    }
                    else
                    {
                        if (ex.Message.Contains("tempo limite")
                         || ex.Message.Contains("timeout")
                         || ex.Message.Contains("RanToCompletion"))// (RanToCompletion, Faulted ou Canceled).
                            throw new cidException(ex.Message + " - timeout: " + reqTimeout, ErroCodes.JsonCommand, ex, cSend, cReceive, 0);
                        else
                            throw wex;
                    }
                }
                else
                {
                    WriteLog("ERRO: " + ex.Message + "\r\n" + ex.StackTrace);

                    if (ex.Message.Contains("RanToCompletion"))// (RanToCompletion, Faulted ou Canceled).
                        ex = new cidException("timeout: " + reqTimeout, ErroCodes.JsonCommand, ex, cSend, cReceive, 0);
                    else
                        ex = new cidException(ErroCodes.JsonCommand, ex, cSend, cReceive, 0);

                    throw ex;
                }
            }
        }
    }
}