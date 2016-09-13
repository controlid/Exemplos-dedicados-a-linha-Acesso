using ExemploAPI.Properties;
using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Forms;

namespace ExemploAPI
{
    public partial class frmExemplos : Form
    {
        private string urlDevice = null;
        private string session = null;

        #region Controles do Formulario

        public frmExemplos()
        {
            InitializeComponent();
        }

        public void AddLog(Exception ex)
        {
            AddLog("ERRO: " + ex.Message + "\r\n" + ex.StackTrace);
        }

        public void AddLog(string cInfo)
        {
            txtOut.Text += "\r\n" + cInfo;
            txtOut.SelectionStart = txtOut.Text.Length;
            txtOut.ScrollToCaret();
        }

        private void frmExemplos_Load(object sender, EventArgs e)
        {
            cmbGiro.SelectedIndex = 0;

            // apenas para facilitar os testes, lê sempre os dados pré configurados
            txtIP.Text = Settings.Default.ip;
            nmPort.Value = Settings.Default.port;
            chkSSL.Checked = Settings.Default.ssl;
            txtUser.Text = Settings.Default.user;
            txtPassword.Text = Settings.Default.password;
        }

        #endregion

        #region Login e Validação de sessão

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                txtIP.Text = txtIP.Text.Trim();
                session = null; // invalida sessão anterior

                if (this.chkSSL.Checked)
                {
                    urlDevice = "https://" + txtIP.Text;
                    if (nmPort.Value != 443)
                        urlDevice += ":" + nmPort.Value;
                }
                else
                {
                    urlDevice = "http://" + txtIP.Text;
                    if (nmPort.Value != 80)
                        urlDevice += ":" + nmPort.Value;
                }
                urlDevice += "/";

                txtOut.Text = "Device: " + urlDevice;

                string response = WebJson.Send(urlDevice + "login", "{\"login\":\"" + txtUser.Text + "\",\"password\":\"" + txtPassword.Text + "\"}");
                AddLog(response);

                // Forma mais simples de pegar a sessão!
                if (response.Contains("session"))
                {
                    // O recomendado é criar um parse JSON com DataContract/DataMember para cada menssagem, mas a serialização não é o foco deste exemplo!
                    session = response.Split('"')[3];
                    AddLog("OK Conectado!");

                    // Persiste a conexão nas configurações do aplicativo para facilitar
                    Settings.Default.ip = txtIP.Text;
                    Settings.Default.port = (int)nmPort.Value;
                    Settings.Default.ssl = chkSSL.Checked;
                    Settings.Default.user = txtUser.Text;
                    Settings.Default.password = txtPassword.Text;
                    Settings.Default.Save();
                }
            }
            catch (Exception ex)
            {
                AddLog(ex);
            }
        }

        private void tbc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tbc.SelectedIndex > 0 && session == null)
            {
                AddLog("Primeiro se conecte ao equipamento");
                tbc.SelectedIndex = 0;
            }
        }

        #endregion

        #region Ações

        private void btnInfo_Click(object sender, EventArgs e)
        {
            try
            {
                AddLog(WebJson.Send(urlDevice + "system_information", null, session));
            }
            catch (Exception ex)
            {
                AddLog(ex);
            }
        }

        private void btnRele_Click(object sender, EventArgs e)
        {
            try
            {
                // Identifica qual botão foi apertado
                int nPorta;
                var btn = sender as Button;
                if (btn.Name == btnRele1.Name)
                    nPorta = 1;
                else if (btn.Name == btnRele2.Name)
                    nPorta = 2;
                else if (btn.Name == btnRele3.Name)
                    nPorta = 3;
                else if (btn.Name == btnRele4.Name)
                    nPorta = 4;
                else
                    throw new Exception("Botão não identificado");

                // Eventualmente pode ser necessário habilitar o rele em questão
                // WebJson.Send(urlDevice + "set_configuration", "{\"general\":{\"relay1_enabled\": \"1\",\"relay2_enabled\": \"1\"}}");
                string cmd = "{\"actions\":[{\"action\": \"door\", \"parameters\":\"door=" + nPorta + "\"}]}";
                AddLog(WebJson.Send(urlDevice + "execute_actions", cmd, session));
            }
            catch (Exception ex)
            {
                AddLog(ex);
            }
        }

        private void btnGiro_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbGiro.SelectedIndex == 1) // Horario
                    AddLog(WebJson.Send(urlDevice + "execute_actions", "{\"actions\":[{\"action\": \"catra\", \"parameters\":\"allow=clockwise\"}]}", session));
                else if (cmbGiro.SelectedIndex == 2) // Anti-Horario
                    AddLog(WebJson.Send(urlDevice + "execute_actions", "{\"actions\":[{\"action\": \"catra\", \"parameters\":\"allow=anticlockwise\"}]}", session));
                else  // Ambos
                    AddLog(WebJson.Send(urlDevice + "execute_actions", "{\"actions\":[{\"action\": \"catra\", \"parameters\":\"allow=both\"}]}", session));

            }
            catch (Exception ex)
            {
                AddLog(ex);
            }
        }

        private void btnReboot_Click(object sender, EventArgs e)
        {
            try
            {
                AddLog(WebJson.Send(urlDevice + "reboot", null, session));
            }
            catch (Exception ex)
            {
                AddLog(ex);
            }
        }

        #endregion

        #region Configurações

        private void btnDataHora_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime dt = dateTimePicker1.Value;
                string cmd = "{" +
                    "\"day\":" + dt.Day +
                    ",\"month\":" + dt.Month +
                    ",\"year\":" + dt.Year +
                    ",\"hour\":" + dt.Hour +
                    ",\"minute\":" + dt.Minute +
                    ",\"second\":" + dt.Second +
                    "}";

                AddLog(WebJson.Send(urlDevice + "set_system_time", cmd, session));
            }
            catch (Exception ex)
            {
                AddLog(ex);
            }
        }

        private void btnAgora_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
        }

        #endregion

        #region Usuários

        private void btnUserList_Click(object sender, EventArgs e)
        {
            try
            {
                AddLog(WebJson.Send(urlDevice + "load_objects", "{\"object\":\"users\"}", session));
            }
            catch (Exception ex)
            {
                AddLog(ex);
            }
        }

        private void btnUserBioList_Click(object sender, EventArgs e)
        {
            try
            {
                AddLog(WebJson.Send(urlDevice + "load_objects", "{\"object\":\"templates\"}", session));
            }
            catch (Exception ex)
            {
                AddLog(ex);
            }
        }

        private void btnUserCardList_Click(object sender, EventArgs e)
        {
            try
            {
                AddLog(WebJson.Send(urlDevice + "load_objects", "{\"object\":\"cards\"}", session));
            }
            catch (Exception ex)
            {
                AddLog(ex);
            }
        }

        private void btnUserListParse_Click(object sender, EventArgs e)
        {
            try
            {
                // Este exemplo irá fazer um parse simples do retorno dos objetos automaticamente, usando uma estrutura de classes com os memos nomes
                string users = WebJson.Send(urlDevice + "load_objects", "{\"object\":\"users\"}", session); // Consulte a documentação para fazer 'Where'
                // https://www.controlid.com.br/produtos/controlador-de-acesso
                // https://www.controlid.com.br/suporte/api_idaccess_V2.6.8.html

                // Basta referenciar o System.Runtime.Serialization, a partir do .Net 4.0
                var serializer = new DataContractJsonSerializer(typeof(UserList));

                // aqui vou transformar a string em um stream, mas o ideal é ter esse parse dentro do WebJson que usarei em outro exemplo
                var ms = new System.IO.MemoryStream(UTF8Encoding.UTF8.GetBytes(users));

                // A mágina acontece aqui! (veja as estruturas de classes auxiliares, mais abaixo)
                var list = serializer.ReadObject(ms) as UserList;

                // Só listo os dados
                var sb = new StringBuilder(); // uso um StringBuilder, apenas para otimizar o código, e mandar para a tela tudo de uma vez
                for (int i = 0; i < list.users.Length; i++)
                    sb.AppendFormat("{0}: {1} - {2}\r\n", list.users[i].id, list.users[i].name, list.users[i].registration);

                // Exibe de fato os dados
                AddLog(sb.ToString());
            }
            catch (Exception ex)
            {
                AddLog(ex);
            }
        }

        [DataContract]
        public class UserList
        {
            [DataMember(EmitDefaultValue = false)] // Os EmitDefaultValue são necessários mais para postar informações omitindo itens não definidos
            public User[] users;
        }

        [DataContract]
        public class User
        {
            [DataMember(EmitDefaultValue = false)]
            public long id; // Atenção no iDAccess todos os numeros são sempre long (64bits)
            [DataMember(EmitDefaultValue = false)]
            public string name;
            [DataMember(EmitDefaultValue = false)]
            public string registration;
            // Já que neste exemplo não irei usar, vou remover, o que no DataContractJsonSerializer, não interfere em nada, pois ele só processa o que estiver definido
            //[DataMember(EmitDefaultValue = false)]
            //public string password;
            //[DataMember(EmitDefaultValue = false)]
            //public string salt;
        }

        private void btnUserAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Note que trabalhar totalmente por string há várias situações que precisam ser tratadas manualmente
                // por isso fazer via parse JSON é bem melhor, mas vai requerer mais conhecimento em .Net
                string cmd = "{" +
                    "\"object\" : \"users\"," +
                    "\"values\" : [{" +
                            (txtUserID.Text == "" ? "" : ("\"id\" :" + txtUserID.Text + ",")) + // O iD é opcional
                            "\"name\" :\"" + txtUserName.Text + "\"," +
                            "\"registration\" : \"" + txtUserRegistration.Text + "\"" +
                        "}]" +
                    "}";
                AddLog(WebJson.Send(urlDevice + "create_objects", cmd, session));
            }
            catch (Exception ex)
            {
                AddLog(ex);
            }
        }

        private void btnUserDelete_Click(object sender, EventArgs e)
        {
            try
            {
                long id = long.Parse(txtUserID.Text);
                AddLog(WebJson.Send(urlDevice + "destroy_objects", "{\"object\":\"users\",\"where\":{\"users\":{\"id\":[" + id + "]}}}", session));
            }
            catch (Exception ex)
            {
                AddLog(ex);
            }
        }

        // Exemplo de leitura usando o parse JSON
        private void btnUserRead_Click(object sender, EventArgs e)
        {
            try
            {
                long id = long.Parse(txtUserID.Text);
                var usrList = WebJson.Send<UserList>(urlDevice + "load_objects", "{\"object\":\"users\",\"where\":{\"users\":{\"id\":[" + id + "]}}}", session);
                // Note que é sempre retornada um lista de acordo com a Where, que neste caso por ser um ID, só deve vir 1 se achou
                if (usrList.users.Length == 1)
                {
                    txtUserName.Text = usrList.users[0].name;
                    txtUserRegistration.Text = usrList.users[0].registration;
                    AddLog("Usuário " + id + " lido com sucesso");
                }
                else
                    AddLog("Usuário " + id + " não existe");
            }
            catch (Exception ex)
            {
                AddLog(ex);
            }
        }

        private void btnUserModify_Click(object sender, EventArgs e)
        {
            try
            {
                long id = long.Parse(txtUserID.Text);
                string cmd = "{" +
                    "\"object\" : \"users\"," +
                    "\"where\":{\"users\":{\"id\":[" + id + "]}}," +
                    "\"values\" : {" +
                            "\"name\" :\"" + txtUserName.Text + "\"," +
                            "\"registration\" : \"" + txtUserRegistration.Text + "\"" +
                        "}" +
                    "}";
                AddLog(WebJson.Send(urlDevice + "modify_objects", cmd, session));
            }
            catch (Exception ex)
            {
                AddLog(ex);
            }
        }

        #endregion
    }
}