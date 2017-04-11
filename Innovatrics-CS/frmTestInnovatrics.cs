using ControliD.USB;
using Innovatrics.IEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TestInnovatrics
{
    public partial class frmTestInnovatrics : Form
    {
        Bitmap bmpLast;
        SortedList<string, Byte[]> NomeDigitais;
        IDKit idKit = null;

        public frmTestInnovatrics()
        {
            InitializeComponent();
        }

        void ShowError(Exception ex)
        {
            txtOut.Text += "\r\nERRO =========== ";
            while (ex != null)
            {
                txtOut.Text += ex.Message + "\r\n" + ex.StackTrace;
                ex = ex.InnerException;
            }
        }

        private void frmTestInnovatrics_Load(object sender, EventArgs e)
        {
            try
            {
                NomeDigitais = new SortedList<string, Byte[]>();

                Futronic.Device.onInfo += info => txtOut.Text += "\r\n" + info;
                Futronic.Device.onInfoAppend += extra => txtOut.Text += extra;
                Futronic.Device.onError += erro => ShowError(erro);

                fbd.SelectedPath = txtPath.Text;
                LoadFiles();

                idKit = new IDKit();
                txtOut.Text = IDKit.ProductString + " Threshold: " + idKit.Threshold;
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void LoadFiles()
        {
            try
            {
                ChangeBMP(null);
                lst.SelectedIndex = -1;

                DirectoryInfo di = new DirectoryInfo(txtPath.Text);
                foreach (var file in di.GetFiles("*.png"))
                    lst.Items.Add(file.Name);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Futronic.Device.Connected)
                {
                    if (!Futronic.Device.Init())
                        return;
                }
                if (Futronic.Device.IsFinger(TimeSpan.FromSeconds(5), () => Application.DoEvents()))
                {
                    var image = Futronic.Device.GetFingerprint();
                    if (image != null)
                    {
                        txtOut.Text += "\r\nDedo OK";
                        ChangeBMP(image.Image);
                    }
                    else
                    {
                        txtOut.Text = "\r\nSem imagem";
                        ChangeBMP(null);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (picDedo.Image == null)
            {
                txtOut.Text = "Selecione uma imagem, ou capture uma nova pelo leitor";
                return;
            }
            try
            {
                Fingerprint fp;
                Bitmap bmp = picDedo.Image as Bitmap;
                if (chkBitmap.Checked)
                {
                    txtOut.Text = "BITMAP " + bmp.Width + "x" + bmp.Height;
                    fp = new Fingerprint(idKit, picDedo.Image);
                }
                else
                {
                    txtOut.Text = "RAW IMAGE " + bmp.Width + "x" + bmp.Height;
                    var img = BitmapArray(bmp);
                    fp = new Fingerprint(img.ToArray(), bmp.Width, bmp.Height);
                }

                txtOut.Text += "Quality: " + fp.Quality;
                SearchResult result = idKit.Find(fp, NomeDigitais.Values.ToArray());

                txtOut.Text += " Count: " + result.Count + " Best(Id): " + result.Best.Id;
                if (result.Count == 1 && result.Best.Id >= 0)
                    txtOut.Text += "\r\nIdentificado: " + NomeDigitais.Keys[result.Best.Id];
                else
                    txtOut.Text += "\r\nNão identificado";

            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (picDedo.Image == null)
            {
                txtOut.Text = "Selecione uma imagem, ou capture uma nova pelo leitor";
                return;
            }
            try
            {
                Fingerprint fp;
                Bitmap bmp = picDedo.Image as Bitmap;
                if (chkBitmap.Checked)
                {
                    txtOut.Text = "BITMAP " + bmp.Width + "x" + bmp.Height;
                    fp = new Fingerprint(idKit, picDedo.Image);
                }
                else
                {
                    txtOut.Text = "RAW IMAGE " + bmp.Width + "x" + bmp.Height;
                    var img = BitmapArray(bmp);
                    fp = new Fingerprint(img.ToArray(), bmp.Width, bmp.Height);
                }

                User usr = new User();
                usr.Add(fp);
                var bt = usr.ExportTemplate(TemplateFormat.ICS);
                NomeDigitais.Add(txtNome.Text, bt);
                txtOut.Text = "Usuário adicionado: " + txtNome.Text + " Quality: " + fp.Quality;
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void ChangeBMP(Bitmap bmp)
        {
            picDedo.Image = bmp;
            picHist.Image = null;
            if (bmp == null)
                return;

            bmpLast = new Bitmap(bmp);
            int width = bmp.Width;
            int height = bmp.Height;
            try
            {
                var hist = new SortedList<byte, int>();
                int max = 0;
                var img = new List<byte>();
                for (int iy = 0; iy < bmp.Height; iy++)
                {
                    for (int ix = 0; ix < bmp.Width; ix++)
                    {
                        var c = bmp.GetPixel(ix, iy);

                        if (c.R < tr1.Value)
                        {
                            //byte j = (byte)((double)c.R * img / 100);
                            //c = Color.FromArgb(255, j, j, j);
                            c = Color.FromArgb(255, 0, 0, 0);
                        }
                        else if (c.R > tr2.Value)
                        {
                            //byte j = (byte)((double)c.R * (100-img) / 100);
                            //c = Color.FromArgb(255, j, j, j);
                            c = Color.FromArgb(255, 255, 255, 255);
                        }

                        if (!hist.ContainsKey(c.R))
                            hist.Add(c.R, 1);
                        else
                            hist[c.R]++;

                        if (hist[c.R] > max)
                            max = hist[c.R];

                        bmp.SetPixel(ix, iy, c);
                        img.Add(c.R);
                    }
                }

                //height = image.Length / width; // 83200
                //using (var fp = new Fingerprint(image, width, height))

                if (chkBitmap.Checked)
                {
                    using (var fp = new Fingerprint(bmp))
                        txtOut.Text += string.Format("\r\nBITMAP {0}x{1} Quality: {2}", width, height, fp.Quality);
                }
                else
                {
                    using (var fp = new Fingerprint(img.ToArray(), width, height))
                        txtOut.Text += string.Format("\r\nRAW IMAGE {0}x{1} Quality: {2}", width, height, fp.Quality);
                }

                int pico = max / 10;
                Bitmap bHist = new Bitmap(hist.Count, pico);
                Graphics g = Graphics.FromImage(bHist);
                g.Clear(Color.White);
                foreach (var k in hist.Keys)
                {
                    int m = hist[k];
                    if (m > pico)
                        m = pico;
                    g.DrawLine(Pens.Black, k, pico, k, pico - m);
                    txtOut.Text += "\r\n" + k + ": " + hist[k];
                }

                picHist.Image = bHist;
                picDedo.Image = bmp;

            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        byte[] BitmapArray(Bitmap bmp)
        {
            var img = new List<byte>();
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                    img.Add(bmp.GetPixel(x, y).R);
            }
            return img.ToArray();
        }

        private void tr_Scroll(object sender, EventArgs e)
        {
            txtOut.Text = "";
            ChangeBMP(bmpLast);
        }

        private void lst_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtOut.Text = "";
                if (lst.SelectedIndex >= 0)
                    ChangeBMP(new Bitmap(txtPath.Text + @"\" + lst.SelectedItem));
                else
                    ChangeBMP(null);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void btnDirectory_Click(object sender, EventArgs e)
        {
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = fbd.SelectedPath;
                LoadFiles();
            }
        }

        private void txtNome_TextChanged(object sender, EventArgs e)
        {
            btnAdd.Enabled = !string.IsNullOrEmpty(txtNome.Text);
        }

        private void chkBitmap_CheckedChanged(object sender, EventArgs e)
        {
            txtOut.Text = "";
            ChangeBMP(bmpLast);
        }
    }
}