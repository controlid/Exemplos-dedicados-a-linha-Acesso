using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ControleRemoto
{
    class WebJson
    {
        public static object JsonCommand(string cURL, object objRequest, Type tpResult)
        {
            System.Net.ServicePointManager.Expect100Continue = false;
            try
            {
                var request = WebRequest.Create(cURL);
                request.ContentType = "application/json";
                request.Method = "POST";

                using (Stream send = request.GetRequestStream())
                {
                    //using (Task<Stream> send = request.GetRequestStreamAsync())
                    // faz outras coisas enquanto aguarda
                    Type tpRequest = objRequest.GetType();
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(tpRequest);
                    //send.Wait();
                    serializer.WriteObject(send, objRequest);
                    //serializer.WriteObject(send.Result, objRequest);
                }

                using (WebResponse response = request.GetResponse())
                {
                    //using (Task<WebResponse> response = request.GetResponseAsync())
                    //response.Wait();
                    if (tpResult != null)
                    {
                        DataContractJsonSerializer deserializer = new DataContractJsonSerializer(tpResult);
                        return deserializer.ReadObject(response.GetResponseStream());
                        //return deserializer.ReadObject(response.Result.GetResponseStream());
                    }
                    else
                        return null;
                }
            }
            catch (WebException e)
            {
                try
                {
                    if (tpResult != null)
                    {
                        DataContractJsonSerializer deserializer = new DataContractJsonSerializer(tpResult);
                        return deserializer.ReadObject(e.Response.GetResponseStream());
                    }
                    else
                    {
                        // o tratamento de erro não usa a mesma logica assincrona, ou implementa da mesma forma ou trata especial
                        WebResponse response = e.Response;
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data))
                            return reader.ReadToEnd();
                    }
                }
                catch(Exception ex2)
                {
                    return "Erro Interno: " + e.Message + "\n" + ex2.Message;
                }
            }
            catch (Exception e)
            {
                return "Erro Geral: " + e.Message;
            }
        }
    }
}
