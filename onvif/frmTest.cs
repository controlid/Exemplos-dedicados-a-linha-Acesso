using onvif.OnvifMedia;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace onvif
{
    public partial class frmTest : Form
    {
        public frmTest()
        {
            InitializeComponent();
        }

        // (thread-safe) https://msdn.microsoft.com/pt-br/library/ms171728(v=vs.110).aspx
        public void AddInfo(string cInfo)
        {
            if (txtOut.InvokeRequired)
                this.Invoke(new Action<string>(AddInfo), new object[] { cInfo });
            else
            {
                txtOut.Text += cInfo + "\r\n";
                txtOut.SelectionStart = txtOut.TextLength;
                txtOut.ScrollToCaret();
            }
        }

        public void AddInfo(Exception ex)
        {
            string cInfo = "ERRO: ";
            while (ex != null)
            {
                cInfo += ex.Message + "\r\n" + ex.StackTrace + "\r\n";
                ex = ex.InnerException;
            }
            AddInfo(cInfo);
        }

        // Assincrono para não travar a tela
        private async void btnFoto_Click(object sender, EventArgs e)
        {
            btnFoto.Enabled = false; // apenas para só permitir uma requisição de cada vez
            var bmp = await Task.Run(() => onvifPicture(txtIPDNS.Text, txtUser.Text, txtPassword.Text));
            picOut.Image = bmp;
            btnFoto.Enabled = true;
        }

        private async void btnVideo_Click(object sender, EventArgs e)
        {
            btnVideo.Enabled = false; // apenas para só permitir uma requisição de cada vez
            await Task.Run(() => onvifVideo(txtIPDNS.Text, txtUser.Text, txtPassword.Text));
            btnVideo.Enabled = true;
        }

        private void onvifVideo(string url, string user, string password)
        {
            try
            {
                AddInfo("Efetuando login em " + url);
                // http://stackoverflow.com/questions/31007828/onvif-getstreamurl-c-sharp
                // Adicionar a referencia de serviço: http://www.onvif.org/onvif/ver10/media/wsdl/media.wsdl
                // http://stackoverflow.com/questions/32779467/onvif-api-capture-image-in-c-sharp
                var messageElement = new TextMessageEncodingBindingElement();
                messageElement.MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None);
                HttpTransportBindingElement httpBinding = new HttpTransportBindingElement();
                httpBinding.AuthenticationScheme = AuthenticationSchemes.Basic;
                CustomBinding bind = new CustomBinding(messageElement, httpBinding);
                EndpointAddress mediaAddress = new EndpointAddress(url + "/onvif/Media");
                MediaClient mediaClient = new MediaClient(bind, mediaAddress);
                mediaClient.ClientCredentials.UserName.UserName = user;
                mediaClient.ClientCredentials.UserName.Password = password;
                Profile[] profiles = mediaClient.GetProfiles();
                StreamSetup streamSetup = new StreamSetup();
                streamSetup.Stream = StreamType.RTPUnicast;
                streamSetup.Transport = new Transport();
                streamSetup.Transport.Protocol = TransportProtocol.RTSP;
                var uri = mediaClient.GetStreamUri(streamSetup, profiles[1].token);
                AddInfo(uri.Uri);
            }
            catch (Exception ex)
            {
                AddInfo(ex);
            }
        }

        MediaClient mediaClient = null;
        
        private Bitmap onvifPicture(string url, string user, string password)
        {
            try
            {
                if(mediaClient==null)
                {
                    AddInfo("Efetuando login em: " + url);
                    // Adicionar a referencia de serviço: http://www.onvif.org/onvif/ver10/media/wsdl/media.wsdl
                    // http://stackoverflow.com/questions/32779467/onvif-api-capture-image-in-c-sharp
                    var messageElement = new TextMessageEncodingBindingElement();
                    messageElement.MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None);
                    HttpTransportBindingElement httpBinding = new HttpTransportBindingElement();
                    httpBinding.AuthenticationScheme = AuthenticationSchemes.Basic;
                    CustomBinding bind = new CustomBinding(messageElement, httpBinding);
                    EndpointAddress mediaAddress = new EndpointAddress(url + "/onvif/Media");
                    mediaClient = new MediaClient(bind, mediaAddress);
                    mediaClient.ClientCredentials.UserName.UserName = user;
                    mediaClient.ClientCredentials.UserName.Password = password;
                }
                else
                    AddInfo("Obtem imagem: " + url);

                Profile[] profiles = mediaClient.GetProfiles();
                string profileToken = profiles[1].token;
                AddInfo("profileToken: " + profileToken);

                MediaUri mediaUri = mediaClient.GetSnapshotUri(profileToken);
                WebRequest requestPic = WebRequest.Create(mediaUri.Uri);
                WebResponse responsePic = requestPic.GetResponse();
                return (Bitmap)Bitmap.FromStream(responsePic.GetResponseStream());
            }
            catch (Exception ex)
            {
                AddInfo(ex);
                return null;
            }
        }
    }
}
