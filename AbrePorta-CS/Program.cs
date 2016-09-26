using System;
using System.IO;
using System.Net;

namespace AbrePortas
{
    class Program
    {
        /// <summary>
        /// Exemplo super simples sem usar nenhuma função para simplesmente: logar no equipamento, tocar o buzzer e abrir a porta
        /// (o buzzer é necessário para que a pessoa que está do lado de fora perceber que o equipamento abriu)
        /// </summary>
        /// <param name="args">IP usuário e senha</param>
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Informe o IP, usuário e senha");
                return;
            }

            string cIP = args[0];
            string cLogin = args[1];
            string cPassword = args[2];

            try
            {
                // Configura o .Net para não quebrar pacotes TCP/IP
                System.Net.ServicePointManager.Expect100Continue = false;

                // Etapa 1: Login
                var reqLogin = WebRequest.Create("http://" + cIP + "/login.fcgi");
                reqLogin.ContentType = "application/json";
                reqLogin.Method = "POST";

                using (var sw = new StreamWriter(reqLogin.GetRequestStream()))
                    sw.Write("{\"login\":\"" + cLogin + "\",\"password\":\"" + cPassword + "\"}");

                var respLogin = reqLogin.GetResponse(); // Envia o comando

                using (var streamReader = new StreamReader(respLogin.GetResponseStream()))
                {
                    // Se não der erro obtem a sessão da forma mais simples possivel supondo que o terceiro parenteses é o valor da sessão
                    string cResult = streamReader.ReadToEnd();
                    string session = cResult.Split('"')[3];
                    // Console.WriteLine(session);

                    // Etapa 2: Toca o Buzzer para a pessoa perceber que algo ocorreu
                    var reqBuzzer = WebRequest.Create("http://" + cIP + "/buzzer_buzz.fcgi?session=" + session);
                    reqBuzzer.ContentType = "application/json";
                    reqBuzzer.Method = "POST";

                    using (var sw = new StreamWriter(reqBuzzer.GetRequestStream()))
                        sw.Write("{\"duty_cycle\":50,\"frequency\":4000,\"timeout\":250}");

                    reqBuzzer.GetResponse(); // Envia o comando

                    // Etapa 3: Abre a porta
                    var reqAction = WebRequest.Create("http://" + cIP + "/execute_actions.fcgi?session=" + session);
                    reqAction.ContentType = "application/json";
                    reqAction.Method = "POST";

                    using (var sw = new StreamWriter(reqAction.GetRequestStream()))
                        // {"actions":[{"action":"sec_box","parameters":"id=65793, reason=3"}]}
                        sw.Write("{\"actions\":[{\"action\": \"door\", \"parameters\":\"door=1\"}, {\"action\": \"door\", \"parameters\":\"door=2\"}]}");

                    reqAction.GetResponse();  // Envia o comando
                }
            }
            catch (WebException e)
            {
                // Em caso de qualquer erro exibe o que houve
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    if (httpResponse==null)
                        Console.WriteLine("Response Erro: " + e.Message);
                    else
                    {
                        Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data))
                            Console.WriteLine(reader.ReadToEnd());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro Geral: " + e.Message);
            }

            // Console.ReadKey(); // util em desenvolvimento para dar um pause
        }
    }
}