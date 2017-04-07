using ControlID.USB;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace UnitTestAcesso
{
    [TestClass]
    public class sdk_USB
    {
        [TestMethod, TestCategory("USB Futronic")]
        public void Futronic_Test()
        {
            if (Futronic.Device.Init())
            {
                Console.WriteLine("Init OK");
                if (Futronic.Device.IsFinger(TimeSpan.FromSeconds(5)))
                {
                    Console.WriteLine("Tem Dedo");
                    Bitmap bmp = Futronic.Device.GetFingerprint().Image;
                    bmp.Save("digital.bmp", ImageFormat.Png);
                    Console.WriteLine("Digital gravada");
                }
                else
                    Assert.Inconclusive("Teste inserindo um dedo");
            }
            else
                Assert.Inconclusive("Init Error: O sensor pode não estar conectado");
        }
    }
}
