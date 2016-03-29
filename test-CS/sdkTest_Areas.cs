using ControlID;
using ControlID.iDAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestAcesso
{
    public partial class sdkTest
    {
        [TestMethod, TestCategory("sdk generic")]
        public void sdk_Areas_CRUD()
        {
            long areaFora = eqpt.LoadOrAdd<Areas>("API Fora");
            long areaDentro = eqpt.LoadOrAdd<Areas>("API Dentro");
            Console.WriteLine("Areas de Dentro: " + areaDentro + " e Fora: " + areaFora);

            long Entrando = eqpt.LoadOrAdd<Portals>(new Portals() { name = "API Entrando", area_from_id = areaFora, area_to_id = areaDentro });
            long Saindo = eqpt.LoadOrAdd<Portals>(new Portals() { name = "API Saindo", area_from_id = areaDentro, area_to_id = areaFora });
            Console.WriteLine("Portais Entrando: " + Entrando + " e Saindo: " + Saindo);


        }
    }
}