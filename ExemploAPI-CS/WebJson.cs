using System;
using System.IO;
using System.Net;

namespace ExemploAPI
{
    class WebJson
    {
        static WebJson()
        {
            ServicePointManager.Expect100Continue = false;
        }

        static public string Send(string uri, string data, string session = null)
        {
            if (session != null)
                uri += ".fcgi?session=" + session;
            else
                uri += ".fcgi";

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.ContentType = "application/json";
                request.Method = "POST";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(data);
                }

                var response = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    string responseData = streamReader.ReadToEnd();
                    Console.WriteLine(responseData);
                    return responseData;
                }
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream responseData = response.GetResponseStream())
                    using (var reader = new StreamReader(responseData))
                    {
                        string text = reader.ReadToEnd();
                        Console.WriteLine(text);
                        throw new Exception(text);
                    }
                }
            }
        }
    }
}
