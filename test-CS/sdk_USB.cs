using ControlID.USB;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestAcesso
{
    [TestClass]
    public class sdk_USB
    {
        [TestMethod, TestCategory("USB Futronic")]
        public void Futronic_Test()
        {
            Futronic futronic = new Futronic();
            if (futronic.Init())
            {
                Console.WriteLine("Init OK");
                if (futronic.IsFinger)
                {
                    Console.WriteLine("Tem Dedo");
                    Bitmap bmp = futronic.ExportBitMap();
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
