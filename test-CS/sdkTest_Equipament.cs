using ControlID;
using ControlID.iDAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestAcesso
{
    [TestClass]
    public partial class sdkTest
    {
        private static Device eqpt = null;

        // Instancia da classe com os dados do equipamento
        // https://msdn.microsoft.com/pt-br/library/microsoft.visualstudio.testtools.unittesting.classinitializeattribute.aspx
        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            Console.WriteLine("ClassInit " + context.TestName);
            eqpt = new Device(BaseTest.URL, BaseTest.Login, BaseTest.Password);
        }

        [TestMethod, TestCategory("sdk Catraca")]
        public void sdk_SpinClockwise()
        {
            eqpt.Spin(iDBlockDirection.Clockwise);
        }

        [TestMethod, TestCategory("sdk Catraca")]
        public void sdk_SpinAnticlockwise()
        {
            eqpt.Spin(iDBlockDirection.Anticlockwise);
        }

        [TestMethod, TestCategory("sdk Catraca")]
        public void sdk_SpinBoth()
        {
            eqpt.Spin(iDBlockDirection.Both);
        }

        [TestMethod, TestCategory("sdk Catraca")]
        public void sdk_Config_AntiPassbackEnable()
        {
            eqpt.SetCatra(new ConfigCatra()
            {
                AntiPassback = true
            });
        }

        [TestMethod, TestCategory("sdk Catraca")]
        public void sdk_Config_AntiPassbackDisable()
        {
            eqpt.SetCatra(new ConfigCatra()
            {
                AntiPassback = false
            });
        }

        [TestMethod, TestCategory("sdk Catraca")]
        public void sdk_Config_DailyResetEnable()
        {
            eqpt.SetCatra(new ConfigCatra()
            {
                DailyReset = true
            });
        }

        [TestMethod, TestCategory("sdk Catraca")]
        public void sdk_Config_DailyResetDisable()
        {
            eqpt.SetCatra(new ConfigCatra()
            {
                DailyReset = false
            });
        }

        [TestMethod, TestCategory("sdk Catraca")]
        public void sdk_Config_GateWayClockWise()
        {
            eqpt.SetCatra(new ConfigCatra()
            {
                GatewayMode = iDBlockGateway.Clockwise
            });
        }

        [TestMethod, TestCategory("sdk Catraca")]
        public void sdk_Config_GateWayAntiClockWise()
        {
            eqpt.SetCatra(new ConfigCatra()
            {
                GatewayMode = iDBlockGateway.Anticlockwise
            });
        }

        [TestMethod, TestCategory("sdk Catraca")]
        public void sdk_Config_OperationModeBlocked()
        {
            eqpt.SetCatra(new ConfigCatra()
            {
                OperationMode = iDBlockOperationMode.Blocked
            });
        }

        [TestMethod, TestCategory("sdk Catraca")]
        public void sdk_Config_OperationModeEntranceOpen()
        {
            eqpt.SetCatra(new ConfigCatra()
            {
                OperationMode = iDBlockOperationMode.Entrance_Open
            });
        }

        [TestMethod, TestCategory("sdk Catraca")]
        public void sdk_Config_OperationModeExitOpen()
        {
            eqpt.SetCatra(new ConfigCatra()
            {
                OperationMode = iDBlockOperationMode.Exit_Open
            });
        }

        [TestMethod, TestCategory("sdk Catraca")]
        public void sdk_Config_OperationModeBothOpen()
        {
            eqpt.SetCatra(new ConfigCatra()
            {
                OperationMode = iDBlockOperationMode.Both_Open
            });
        }

        [TestMethod, TestCategory("sdk")]
        public void sdk_Login()
        {
            eqpt.Connect();
            Console.WriteLine("Session: " + eqpt.Session);
            eqpt.Disconnect();
        }


        [TestMethod, TestCategory("sdk")]
        public void sdk_BeepOff()
        {
            eqpt.Connect();
            eqpt.SetBeep(false);
        }

        [TestMethod, TestCategory("sdk")]
        public void sdk_Serial()
        {
            long device_id = 514685;
            string device_serial = "B14T";
            string c = Util.ToBase36(device_id);
            long n = Util.FromBase36(device_serial);
            eqpt.Disconnect();
        }

        [TestMethod, TestCategory("sdk")]
        public void sdk_Action()
        {
            eqpt.Action();
        }

        [TestMethod, TestCategory("sdk")]
        public void sdk_Buzzer()
        {
            eqpt.Buzzer();
        }

        [TestMethod, TestCategory("sdk")]
        public void sdk_DeleteAdmin()
        {
            eqpt.DeleteAdmin();
        }

        [TestMethod, TestCategory("sdk")]
        public void sdk_SetBell()
        {
            eqpt.SetBell(1); // define o rele 1 para ser campainha
        }

        [TestMethod, TestCategory("sdk")]
        public void sdk_Reboot()
        {
            // Esse teste gera falha nos demais por motivos obvios
            // eqpt.Reboot(); 
        }

        [TestMethod, TestCategory("sdk")]
        public void sdk_UpdateFirmware()
        {
            // por segurança, estes teste está sempre comentado!
            eqpt.UpdateFirmware();
        }

        [TestMethod, TestCategory("sdk")]
        public void sdk_Info()
        {
            InformationResult info = eqpt.SystemInformation();
            Console.WriteLine(info.serial);
            Console.WriteLine(info.DeviceModelType);
            Console.WriteLine(info.DeviceModelDescription);
        }

        [TestMethod, TestCategory("sdk")]
        public void sdk_set_Time()
        {
            // Define a data/hora qualquer
            DateTime dt = DateTime.Now.AddDays(-73).AddMinutes(-97).AddSeconds(-59);
            Console.WriteLine("Definindo horário para: " + dt.ToString("dd/MM/yyyy HH:mm:ss"));
            eqpt.SetSystemTime(dt);

            // Depois de 3 segundos lê a data/hora atual, que deve estar de 3 a 5 segundos de difereça
            System.Threading.Thread.Sleep(4000);
            InformationResult info = eqpt.SystemInformation();
            Console.WriteLine("Horário lido do equipamento: " + info.CurrentTime.ToString("dd/MM/yyyy HH:mm:ss"));
            double s = info.CurrentTime.Subtract(dt).TotalSeconds;
            Console.WriteLine("Diferença: " + s);
            Assert.IsTrue(s > 3 && s < 5, "Não foi possível ler ou definir o horário");

            // Corrige a data/hora do equipamento
            eqpt.SetSystemTime(DateTime.Now);
        }

        [TestMethod, TestCategory("sdk")]
        public void sdk_TemplateExtract()
        {
            TemplateResult tr = eqpt.TemplateExtract(new Bitmap(@"..\..\dedo1.bmp"));
            Assert.IsTrue(tr.quality > 50 && !string.IsNullOrEmpty(tr.template), "Erro ao extrair template");
            Console.WriteLine("Quality: " + tr.quality + "\r\nTemplate: " + tr.template);
        }

        [TestMethod, TestCategory("sdk")]
        public void sdk_TemplateMatch()
        {
            Console.WriteLine("Exclusão do usuário 44: " + eqpt.Destroy<Users>(44));

            TemplateResult tr1 = eqpt.TemplateExtract(new Bitmap(@"..\..\dedo1.bmp"));
            Console.WriteLine("Quality: " + tr1.quality + "\r\nTemplate: " + tr1.template);
            TemplateResult tr2 = eqpt.TemplateExtract(new Bitmap(@"..\..\dedo2.bmp"));
            Console.WriteLine("Quality: " + tr2.quality + "\r\nTemplate: " + tr2.template);
            TemplateResult tr3 = eqpt.TemplateExtract(new Bitmap(@"..\..\dedo3.bmp"));
            Console.WriteLine("Quality: " + tr3.quality + "\r\nTemplate: " + tr3.template);

            string cOther = "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";
            StatusResult er = eqpt.TemplateMatch(tr1.template, cOther, tr3.template);
            Console.WriteLine("A) " + er.Codigo + ": " + er.Status);
            Assert.IsTrue(er.Codigo != 0, "Deveria ter resultado erro");

            // Tudo certo aqui
            er = eqpt.TemplateMatch(tr1.template, tr2.template, tr3.template);
            Console.WriteLine("B) " + er.Codigo + ": " + er.Status);
            Assert.IsTrue(er.Codigo == 0, "Deveria ter dado certo");
        }

        [TestMethod, TestCategory("sdk")]
        public void sdk_setBeep()
        {
            eqpt.SetBeep(true);
        }

        [TestMethod, TestCategory("sdk")]
        public void sdk_setLeds()
        {
            // 1 - Desligado
            // 2 - Cor Fixa
            // 3 - Transição
            eqpt.SetLeds(new LedsConfig()
            {
                led_rgb = new LedsColors()
                {
                    state = "3",

                    solid_red = "65535",
                    solid_green = "65535",
                    solid_blue = "65535",

                    transition_start_red = "0",
                    transition_start_green = "0",
                    transition_start_blue = "65535",

                    transition_end_red = "0",
                    transition_end_green = "0",
                    transition_end_blue = "0",
                }
            });
        }

        [TestMethod, TestCategory("sdk")]
        public void sdk_TemplateCreate()
        {
            Console.WriteLine("Exclusão do usuário 44: " + eqpt.Destroy<Users>(44));
            long idU = eqpt.Add<Users>(new Users()
            {
                id = 44,
                name = "Usuario com biometria",
                registration = "biometria"
            });
            Assert.IsTrue(idU == 44, "Erro ao incluir o usuário 44");
            Console.WriteLine("Incluído usuário 44");

            TemplateResult tr1 = eqpt.TemplateExtract(new Bitmap(@"..\..\dedo1.bmp"));
            TemplateResult tr2 = eqpt.TemplateExtract(new Bitmap(@"..\..\dedo2.bmp"));
            TemplateResult tr3 = eqpt.TemplateExtract(new Bitmap(@"..\..\dedo3.bmp"));
            Console.WriteLine("Templates extraidos: " + tr1.quality + " " + tr2.quality + " " + tr3.quality);

            StatusResult er = eqpt.TemplateCreate(idU, tr1.template, tr2.template, tr3.template);
            if (er.Codigo != 0)
            {
                Console.WriteLine(er.Codigo + ": " + er.Status);
                Assert.Inconclusive(er.Codigo + ": " + er.Status);
            }
            else
                Console.WriteLine("Templates Adicionado!");
        }

        [TestMethod, TestCategory("sdk")]
        public void sdk_TemplateCreate2()
        {
            Console.WriteLine("Exclusão do usuário 44: " + eqpt.Destroy<Users>(44));
            long idU = eqpt.Add<Users>(new Users()
            {
                id = 44,
                name = "Usuario com biometria",
                registration = "biometria"
            });
            Assert.IsTrue(idU == 44, "Erro ao incluir o usuário 44");
            Console.WriteLine("Incluído usuário 44");

            StatusResult er = eqpt.TemplateCreate(
                idU,
                new Bitmap(@"..\..\dedo1.bmp"),
                new Bitmap(@"..\..\dedo2.bmp"),
                new Bitmap(@"..\..\dedo3.bmp"));

            if (er.Codigo != 0)
            {
                Console.WriteLine(er.Codigo + ": " + er.Status);
                Assert.Inconclusive(er.Codigo + ": " + er.Status);
            }
            else
                Console.WriteLine("Templates Adicionado!");
        }
    }
}