using ExemploAPI.Properties;
using System;
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
            catch(Exception ex)
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
                if(cmbGiro.SelectedIndex == 1) // Horario
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
    }
}