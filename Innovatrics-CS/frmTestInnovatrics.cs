using ControlID.USB;
using Innovatrics.IEngine;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace TestInnovatrics
{
    public partial class frmTestInnovatrics : Form
    {
        Futronic device;
        Bitmap bmp;
        byte[][] digitais;
        string[] names;
        IDKit idKit = null;

        public frmTestInnovatrics()
        {
            InitializeComponent();
        }

        private void lnkLic_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://shop.innovatrics.com/collections/server-sdks");
        }

        private void frmTestInnovatrics_Load(object sender, EventArgs e)
        {
            try
            {
                //cFile = Path.GetDirectoryName(GetType().Assembly.Location) + @"\Foto.jpg";
                //bmp = new Bitmap(cFile);
                //picDedo.Image = bmp;
                device = new Futronic();

                // adiciona Teste
                string template = "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";
                digitais = new byte[1][];
                digitais[0] = Convert.FromBase64String(template);
                names = new string[1];
                names[0] = "Teste";

                idKit = new IDKit();
                idKit.ICSTemplateVersion = 21;
            }
            catch (Exception ex)
            {
                txtOut.Text = ex.Message + "\r\n" + ex.StackTrace;
            }
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            if (!device.Connected)
                device.Init();

            int n = 0;
            while (n < 10)
            {
                if (device.IsFinger)
                {
                    picDedo.Image = bmp = device.ExportBitMap();
                    txtOut.Text = "Dedo OK";
                    break;
                }
                n++;
                txtOut.Text = n.ToString();
                Thread.Sleep(500);
                Application.DoEvents();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                using (var fp = new Fingerprint(idKit, bmp))
                {
                    txtOut.Text = "Quality: " + fp.Quality;
                    // versão 21, e salvar imagens
                    SearchResult result = idKit.Find(fp, digitais);

                    txtOut.Text += "Count: " + result.Count + " Best(Id): " + result.Best.Id;
                    if (result.Count == 1 && result.Best.Id >= 0 && result.Best.Id < names.Length)
                        txtOut.Text += "\r\n" + names[result.Best.Id];
                }
            }
            catch (Exception ex)
            {
                txtOut.Text = ex.Message + "\r\n" + ex.StackTrace;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

        }
    }
}