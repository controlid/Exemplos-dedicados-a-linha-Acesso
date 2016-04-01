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
        [TestMethod, TestCategory("sdk BOX")]
        public void sdk_AreasPortal_Set()
        {
            // -------------
            // A - B - C - D
            long areaFora = eqpt.LoadOrAdd<Areas>("API Fora");
            long areaA = eqpt.LoadOrAdd<Areas>("API A");
            long areaB = eqpt.LoadOrAdd<Areas>("API B");
            long areaC = eqpt.LoadOrAdd<Areas>("API C");
            long areaD = eqpt.LoadOrAdd<Areas>("API D");
            Console.WriteLine("Areas Fora: " + areaFora + " Areas Dentro: " + areaA + " / " + areaB + " / " + areaC + " / " + areaD);

            Portals portal1 = eqpt.LoadOrSet<Portals>(1, new Portals() { name = "Portal 1", area_from_id = areaFora, area_to_id = areaA });
            Portals portal2 = eqpt.LoadOrSet<Portals>(2, new Portals() { name = "Portal 2", area_from_id = areaFora, area_to_id = areaB });
            Portals portal3 = eqpt.LoadOrSet<Portals>(3, new Portals() { name = "Portal 3", area_from_id = areaFora, area_to_id = areaC });
            Portals portal4 = eqpt.LoadOrSet<Portals>(4, new Portals() { name = "Portal 4", area_from_id = areaFora, area_to_id = areaD });
            Console.WriteLine("Portais OK");

            //eqpt.LoadOrAdd<Portal_Actions>(new Portal_Actions() { portal_id = portal1.id, action_id = 2 });
            
        }

        [TestMethod, TestCategory("sdk BOX")]
        public void sdk_Actions_Load()
        {
            Actions action1 = eqpt.Load<Actions>(1);
            Actions action2 = eqpt.Load<Actions>(2);
            Actions action3 = eqpt.Load<Actions>(3);
            Actions action4 = eqpt.Load<Actions>(4);
            Console.WriteLine("Relê OK");
        }

        [TestMethod, TestCategory("sdk BOX")]
        public void sdk_LeitoraRele()
        {
            // Obtem s scripts relacionado a leitoras
            Scripts script = eqpt.First<Scripts>(new Scripts() { script = "card%" });
            Console.WriteLine("Script: " + script.script);

            // Obtem as leitoras disponíveis
            Script_Parameters[] sParameters = eqpt.List<Script_Parameters>(new Script_Parameters() { script_id = script.id });
            foreach (var prm in sParameters)
            {
                var rele_where = new Portal_Script_Parameters() { script_parameter_id = prm.id };
                Portal_Script_Parameters rele = eqpt.First<Portal_Script_Parameters>(rele_where);
                Console.WriteLine("Leitora: " + prm.name + " rele: " + rele.value);
            }

            // Altera a Leitora em relação ao Rele
            eqpt.SetWigandPortal(1, 1, "A", "B");
            eqpt.SetWigandPortal(2, 2, "B", "A");
            eqpt.SetWigandPortal(3, 3);
            eqpt.SetWigandPortal(4, 4);
        }
    }
}