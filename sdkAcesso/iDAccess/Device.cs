using System;

namespace ControliD.iDAccess
{
    public partial class Device
    {
        public string URL { get; private set; }
        public string Login { get; private set; }
        public int Port { get; private set; }
        public bool SSL { get; private set; }
        public string Session { get; private set; }
        public object Tag { get; set; } // Qualquer coisa (por exemplo uma classe que tem os dados de origem, ou identificadores proprios)
        public Exception LastError { get; private set; }

        private string Password;
        private DateTime dtLastCommand;
        private DateTime dtConnection;
        public int TimeOut;

        public Device(string cIP_DNS_URL = "http://192.168.0.129", string cLogin = "admin", string cPassword = "admin", bool useSSL = false, int nPort = 80, object oTag = null, int ForceTimeout = 0)
        {
            URL = cIP_DNS_URL;
            Login = cLogin;
            Password = cPassword;
            SSL = useSSL;
            if (SSL && nPort == 80)
                nPort = 443;
            Port = nPort;
            Tag = oTag;
            if (ForceTimeout > 0)
                TimeOut = ForceTimeout;
            else
                TimeOut = WebJson.TimeOut;
        }

        public void Connect(string cIP_DNS_URL = null, string cLogin = null, string cPassword = null, bool? useSSL = null, int? nPort = null)
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

            var result = WebJson.JsonCommand<LoginResult>(URL + "login.fcgi", lreq, null, TimeOut);
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

        public string TestConnect(string cURL = null, string cLogin = null, string cPassword = null, bool? useSSL = null, int? nPort = null)
        {
            try
            {
                Connect(cURL, cLogin, cPassword, useSSL, nPort);
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public void Disconnect()
        {
            try
            {
                string cLastSession = Session;
                Session = null;
                WebJson.JsonCommand<string>(URL + "logout.fcgi?session=" + cLastSession, null);
            }
            catch(Exception)
            {
            }
        }

        private void CheckSession()
        {
            WebJson.WriteLog();

            if(Session == null)
            {
                Connect();
            }
            else
            {
                var result = WebJson.JsonCommand<SessionResult>(URL + "session_is_valid.fcgi?session=" + Session);
                if (!result.session_is_valid)
                {
                    Connect();
                }
            }
        }        
    }
}
