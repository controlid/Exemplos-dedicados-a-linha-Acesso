using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading;
using System.Threading.Tasks;
using ControlID.iDAccess;
using ControlID;

namespace UnitTestAcesso
{
    [TestClass]
    public partial class BaseTest
    {
        [TestMethod(), TestCategory("Basic")]
        public void Connect_Simple()
        {
            System.Net.ServicePointManager.Expect100Continue = false;
            try
            {
                var request = WebRequest.Create(URL + "login.fcgi");
                request.ContentType = "application/json";
                request.Method = "POST";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write("{\"login\":\"" + Login + "\",\"password\":\"" + Password + "\"}");
                }

                var response = request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    string cResult = streamReader.ReadToEnd();
                    Console.WriteLine(cResult);
                    Assert.IsTrue(cResult.Contains("session"), "Login Invalido");
                }
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();
                        Console.WriteLine(text);
                    }
                }
                Assert.Fail("Erro Web: " + e.Message);
            }
            catch (Exception e)
            {
                Assert.Fail("Erro Geral: " + e.Message);
            }
        }

        [TestMethod, TestCategory("Basic")]
        public void Connect_Json()
        {
            LoginRequest acesso = new LoginRequest();
            acesso.login = Login;
            acesso.password = Password;

            object result = WebJson.JsonCommand<LoginResult>(URL + "login.fcgi", acesso);
            if (result is LoginResult)
            {
                LoginResult dados = (LoginResult)result;
                Console.WriteLine("Sessão: " + dados.session);
                Console.WriteLine("Erro:" + dados.error);
                if (dados.session == null)
                    Assert.Inconclusive("Login invalido");
            }
            else
            {
                Assert.Fail((string)result);
            }
        }

        [TestMethod, TestCategory("Basic")]
        public void AcionaRele_Json()
        {
            LoginRequest acesso = new LoginRequest();
            acesso.login = Login;
            acesso.password = Password;

            object result1 = WebJson.JsonCommand<LoginResult>(URL + "login.fcgi", acesso);
            if (result1 is LoginResult)
            {
                LoginResult dados = (LoginResult)result1;
                Console.WriteLine("Sessão: " + dados.session);
                if (dados.session != null)
                {
                    ActionsRequest ar = new ActionsRequest();
                    ar.actions = new ActionItem[] { new ActionItem() { action = "door", parameters = "door=1" } };
                    // Não retorna saida
                    WebJson.JsonCommand<string>(URL + "execute_actions.fcgi?session=" + dados.session, ar);
                }
                else
                    Assert.Inconclusive("Login invalido");
            }
            else
            {
                Assert.Fail((string)result1);
            }
        }

        [TestMethod, TestCategory("Basic")]
        public void Serializacao()
        {
            // Exemplo de um registro de log de acesso
            // 10: 1433820558 - 09/06/2015 03:29:18 u 1
            DateTime dtOrig = new DateTime(2015, 6, 9, 3, 29, 18);
            long dt = dtOrig.ToUnix();
            Assert.IsTrue(dt == 1433820558, "Erro na conversão Date => Unix: " + dt);
            Assert.IsTrue(dt.FromUnix().Equals(dtOrig), "Erro na conversão Unix => Date: " + dt);

            // Exemplo de serialização de where
            AccessLogsWhere where = new AccessLogsWhere() { time = new WhereFields() { More = dt } };
            string cJSON = ControlID.WebJson.JsonCommand<string>(null, where);
            Console.WriteLine(cJSON);

        }
    }
}