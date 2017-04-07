using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using idAccess_Rest.Model.Util;
namespace idAccess_Rest
{

    class Device
    {
        public static string IPAddress;
        public static string ServerIp;
        private string session = null;
        Util config = new Util();

        public Device()
        {
            var ip_terminal = config.ipServer;
            var ip_servidor = config.ipTerminal;
        }

        public Device(string IPTerminal, string IPServer)
        {
            IPAddress = IPTerminal;
            ServerIp = IPServer;
        }

        public string[] CadastrarNoSevidor(out bool success)
        {
            List<string> response = new List<string>();
            response.Add("Cadastrando Servidor no equipamento");
            try
            {
                //Verifica se o equipamento já está cadastrado no servidor e cadastra caso seja necessário
                if (ListObjects("{" +
                        "\"object\" : \"devices\"," +
                        "\"where\" : [{" +
                                "\"id\" : -1," +
                                "\"object\" : \"devices\"," +
                                "\"field\" : \"ip\"," +
                                "\"value\" : \"" + ServerIp + "\"" +

                            "}]" +
                        "}").Length == 0)
                {
                    try
                    {
                        sendJson("create_objects", "{" +
                                "\"object\" : \"devices\"," +
                                "\"values\" : [{" +
                                        "\"id\" : -1," +
                                        "\"name\" : \"Servidor\"," +
                                        "\"ip\" : \"" + ServerIp + "\"," +
                                        "\"public_key\" : \"anA=\"" +

                                    "}]" +
                                "}");
                        response.Add("Equipamento Servidor cadastrado com sucesso no Cliente.");
                    }
                    catch (Exception ex)
                    {
                        response.Add("Erro ao cadastrar Servidor:");
                        response.Add("  - " + ex.Message);
                    }
                }
                else
                {
                    response.Add("Equipamento Servidor já cadastrado no Cliente");
                }
                ChangeType(true);
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                response.Add("Erro ao cadastrar Servidor:");
                response.Add("  - " + ex.Message);
            }


            //Inicia o monitoramento de identificações do equipamento
            try
            {

                ServiceHost host = new WebServiceHost(typeof(Server));
                WebHttpBinding binding = new WebHttpBinding();
                binding.MaxReceivedMessageSize = 999999999;
                ServiceEndpoint point = host.AddServiceEndpoint(typeof(IServer), binding, "http://" + ServerIp);
                point.Behaviors.Add(new WebHttpBehavior());
                host.Open();
            }
            catch (Exception ex)
            {
                success = false;
                response.Add("Erro ao monitorar identificações:");
                response.Add("  - " + ex.Message);
            }

            return response.ToArray();
        }

        public string[] ChangeType(bool isOnline)
        {
            List<string> response = new List<string>();
            response.Add("Alterando modo de operação do equipamento");
            try
            {
                sendJson("set_configuration",
                    "{" +
                        "\"online_client\" : {" +
                                "\"server_id\" : \"-1\"," +
                                "\"extract_template\" : \"0\"" +

                            "}," +
                         "\"general\" : {" +
                         "\"online\" : \"" + (isOnline ? 1 : 0) + "\"" +

                            "}" +
                        "}"
                );
                response.Add("Modo de operação alteredo");
            }
            catch (Exception ex)
            {
                response.Add("Erro ao alterar Modo de Operação:");
                response.Add("  - " + ex.Message);
            }
            return response.ToArray();
        }

        public Dictionary<string, string>[] ListObjects(string data)
        {
            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();

            string response = sendJson("load_objects", data);
            int i = response.IndexOf("[") + 1;
            response = response.Substring(i, response.Length - 1 - i).Replace("\"", "");
            string[] listObjects = response.Split('}', '{');
            foreach (string sObj in listObjects)
            {
                if (sObj.Length <= 1)
                    continue;
                Dictionary<string, string> obj = new Dictionary<string, string>();
                string[] objFilds = sObj.Split(',');
                foreach (string field in objFilds)
                {
                    string[] value = field.Split(':');
                    obj.Add(value[0], value[1]);
                }
                list.Add(obj);
            }
            return list.ToArray();
        }

        public string Login()
        {
            if (session == null)
            {
                string response = sendJson("login", "{\"login\":\"admin\",\"password\":\"admin\"}", false);
                session = response.Split('"')[3];
            }
            return session;
        }


        public string sendJson(string uri, string data, bool checkLogin = true)
        {
            if (checkLogin)
            {
                Login();
                uri += ".fcgi?session=" + session;
            }
            else
                uri += ".fcgi";
            ServicePointManager.Expect100Continue = false;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create("http://" + IPAddress + "/" + uri);
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