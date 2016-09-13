using ControleRemoto.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ControleRemoto
{
    public partial class frmControle : Form
    {
        public frmControle()
        {
            InitializeComponent();
        }

        private void btnAcionar_Click(object sender, EventArgs e)
        {
            LoginRequest acesso = new LoginRequest();
            acesso.login = txtUser.Text;
            acesso.password = txtPassword.Text;

            object result1 = WebJson.JsonCommand("http://" + txtIP.Text + "/login.fcgi", acesso, typeof(LoginResult));
            if (result1 is LoginResult)
            {
                LoginResult dados = (LoginResult)result1;
                //                Console.WriteLine("Sessão: " + dados.session);
                if (dados.session != null)
                {
                    ActionsRequest ar = new ActionsRequest();
                    ActionItem p1 = new ActionItem() { action = "door", parameters = "door=1" };
                    ActionItem p2 = new ActionItem() { action = "door", parameters = "door=2" };
                    if (chkRele1.AutoCheck && chkRele2.AutoCheck)
                        ar.actions = new ActionItem[] { p1, p2 };
                    if (chkRele1.Checked)
                        ar.actions = new ActionItem[] { p1 };
                    else if (chkRele2.Checked)
                        ar.actions = new ActionItem[] { p2 };
                    else
                    {
                        MessageBox.Show("Selecione algum relê", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    WebJson.JsonCommand("http://" + txtIP.Text + "/execute_actions.fcgi?session=" + dados.session, ar, null);

                    Settings.Default.ip = txtIP.Text;
                    Settings.Default.user = txtUser.Text;
                    Settings.Default.password = txtPassword.Text;
                    Settings.Default.rele1 = chkRele1.Checked;
                    Settings.Default.rele2 = chkRele2.Checked;
                    Settings.Default.Save();
                }
                else
                    MessageBox.Show("Login invalido", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show(result1.ToString(), "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmControle_Load(object sender, EventArgs e)
        {
            txtIP.Text = Settings.Default.ip;
            txtUser.Text = Settings.Default.user;
            txtPassword.Text = Settings.Default.password;
            chkRele1.Checked = Settings.Default.rele1;
            chkRele2.Checked = Settings.Default.rele2;
        }
    }
}