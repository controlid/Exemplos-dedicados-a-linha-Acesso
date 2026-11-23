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

        public Device(string cIP_DNS_URL = "http://192.168.0.129", string cLogin = "admin", string cPassword = "admin", bool? useSSL = null, int nPort = 80, object oTag = null, int ForceTimeout = 0)
        {
            URL = cIP_DNS_URL;
            Login = cLogin;
            Password = cPassword;
            if (useSSL == null)
                useSSL = nPort == 443;

            SSL = useSSL.Value;
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

#if HID
            var result = WebJson.JsonCommand<LoginResult>(URL + "hidlogin.fcgi", lreq, null, TimeOut);
#else
            var result = WebJson.JsonCommand<LoginResult>(URL + "login.fcgi", lreq, null, TimeOut);
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

        // ---------------------------------------------------------------- verificação de sessão
        //
        // Antes de cada comando a sessão é verificada com session_is_valid.fcgi. O equipamento atende as requisições em
        // fila: ocupado (base grande, carga de cartões/fotos), até essa verificação espera a fila. Por isso ela:
        //  - usa o TimeOut desta instância (antes usava o WebJson.TimeOut global, 10 s, ignorando o timeout configurado
        //    pelo chamador, e qualquer fila acima de 10 s abortava a operação);
        //  - se o equipamento não responder (timeout ou falha de rede, sem resposta HTTP), tenta de novo com espera
        //    crescente; esgotadas as tentativas, restabelece a sessão com um novo login (que lança o erro se o
        //    equipamento continuar sem responder);
        //  - sessão inválida (expirada, encerrada, equipamento reiniciado) é restabelecida com um novo login.

        /// <summary>Tentativas do session_is_valid sem resposta antes de restabelecer a sessão com um novo login (mínimo 1).</summary>
        public int SessionCheckAttempts = 3;

        /// <summary>Espera antes da 2ª tentativa do session_is_valid; dobra a cada tentativa (2 s, 4 s...).</summary>
        public int SessionCheckRetryDelayMs = 2000;

        /// <summary>
        /// true se a falha da verificação de sessão foi falta de resposta do equipamento (timeout, conexão recusada/
        /// resetada): vale tentar de novo. Com resposta HTTP é um erro do equipamento e é propagado.
        /// </summary>
        private static bool IsSessionCheckTransient(Exception ex)
        {
            var cex = ex as cidException;
            if (cex != null)
                return (int)cex.StatusCode == 0;
            var wex = ex as System.Net.WebException;
            return wex != null && wex.Response == null;
        }

        private int SessionCheckDelay(int attempt)
        {
            return Math.Max(0, SessionCheckRetryDelayMs) * (1 << Math.Min(attempt - 1, 4));
        }

        private string SessionCheckLog(int attempt, Exception ex, bool reconnect)
        {
            return "session_is_valid sem resposta (tentativa " + attempt + "/" + Math.Max(1, SessionCheckAttempts) + ", timeout " + TimeOut + " ms): " +
                   ex.Message + (reconnect ? " - restabelecendo a sessão (login)" : " - nova tentativa em " + SessionCheckDelay(attempt) + " ms");
        }

        private void CheckSession()
        {
            WebJson.WriteLog();

            if (Session == null)
            {
                Connect();
                return;
            }

            for (int attempt = 1; ; attempt++)
            {
                try
                {
                    var result = WebJson.JsonCommand<SessionResult>(URL + "session_is_valid.fcgi?session=" + Session, null, null, TimeOut);
                    if (!result.session_is_valid)
                        Connect();
                    return;
                }
                catch (Exception ex) when (IsSessionCheckTransient(ex))
                {
                    if (attempt >= SessionCheckAttempts)
                    {
                        WebJson.WriteLog(SessionCheckLog(attempt, ex, true));
                        Connect();
                        return;
                    }
                    WebJson.WriteLog(SessionCheckLog(attempt, ex, false));
                    System.Threading.Thread.Sleep(SessionCheckDelay(attempt));
                }
            }
        }
    }
}