using ExemploAPI.Properties;
using System;
using System.Windows.Forms;

namespace ExemploAPI
{
    public partial class frmExemplos : Form
    {
        private string urlDevice = null;
        private string session = null;

        public frmExemplos()
        {
            InitializeComponent();
        }

        void AddLog(Exception ex)
        {
            txtOut.Text += "\r\nERRO: " + ex.Message + "\r\n" + ex.StackTrace;
        }

        void AddLog(string cInfo)
        {
            txtOut.Text += "\r\n" + cInfo;
        }

        private void btnTestar_Click(object sender, EventArgs e)
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

                string response = WebJson.Send(urlDevice + "login", "{\"login\":\""+ txtUser.Text + "\",\"password\":\"" + txtPassword.Text + "\"}");
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

        private void frmExemplos_Load(object sender, EventArgs e)
        {
            // apenas para facilitar os testes, lê sempre os dados pré configurados
            txtIP.Text = Settings.Default.ip;
            nmPort.Value= Settings.Default.port;
            chkSSL.Checked= Settings.Default.ssl;
            txtUser.Text = Settings.Default.user;
            txtPassword.Text = Settings.Default.password;
        }

        private void tbc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tbc.SelectedIndex>0 && session == null)
            {
                AddLog("Faça login no equipamento primeiro para criar a sessão");
                tbc.SelectedIndex = 0;
            }
        }
    }
}