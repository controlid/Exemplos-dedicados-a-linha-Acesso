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
    // {"id":1,"name":"João Paulo Ferreira Nunes 111","user_roles.role":0}
    [DataContract]
    public class MeuUser : GenericCount
    {
        [DataMember()]
        public long id;
        [DataMember()]
        public string name;
        [DataMember(Name = "user_roles.role")]
        public int role;
    }

    [DataContract]
    public class MeuRetorno
    {
        [DataMember()]
        public MeuUser[] users;
    }

    public partial class sdkTest
    {
        [TestMethod, TestCategory("sdk custom")]
        public void sdk_CustomListJoin()
        {
            String cRequest = "{'fields':['id','name',{'object':'user_roles','field':'role'}],'object':'users','join':'LEFT'}";
            MeuRetorno result = eqpt.Command<MeuRetorno>("load_objects", cRequest.Replace("'", "\""));
            foreach (MeuUser u in result.users)
                Console.WriteLine(
                    u.id + ": " +
                    u.name + " - " +
                    u.role);
        }

        [TestMethod, TestCategory("sdk custom")]
        public void sdk_CustomTableJoin()
        {
            String cRequest = "{'fields':['id','name',{'object':'user_roles','field':'role'}],'object':'users','join':'LEFT'}";
            // Ontem já o array tipado, dentro do nó 'users'
            MeuUser[] musr = eqpt.List<MeuUser>(cRequest.Replace("'", "\""), OrderTypes.None); //, "users");
            DataTable tb = Util.Create<MeuUser>(musr);
            foreach (DataRow row in tb.Rows)
                Console.WriteLine(
                    row["id"] + ": " +
                    row["name"] + " - " +
                    row["role"]);
        }

        [TestMethod, TestCategory("sdk custom")]
        public void sdk_DualAction()
        {
            // Esse é um exemplo com o minimo de configuração para acionar reles diferentes para cada regra de acesso
            //eqpt.LoadOrSet(2, new Access_Rules() { name = "Acesso 1", type = 1, priority = 0 }); // o tipo e prioridade são obrigatórios!
            //eqpt.LoadOrSet(3, new Access_Rules() { name = "Acesso 2", type = 1, priority = 0 });

            //eqpt.LoadOrSet(2, new Groups() { name = "Para Acesso 1" });
            //eqpt.LoadOrSet(3, new Groups() { name = "Para Acesso 2" });

            //eqpt.LoadOrSet(1, new Areas() { name = "Area 1" });
            //eqpt.LoadOrSet(2, new Areas() { name = "Area 2" });
            //eqpt.LoadOrSet(3, new Areas() { name = "Fora" });

            //eqpt.Add(new Area_Access_Rules() { access_rule_id = 2, area_id = 1 });
            //eqpt.Add(new Area_Access_Rules() { access_rule_id = 3, area_id = 2 });

            //eqpt.LoadOrSet(2, new Portals { name = "Rele A", area_from_id = 3, area_to_id = 1 });
            //eqpt.LoadOrSet(3, new Portals { name = "Rele B", area_from_id = 3, area_to_id = 2 });

            //eqpt.Add(new Area_Access_Rules(){  access_rule_id=2, area_id=2 });

            //eqpt.TryDestroyAll<Group_Access_Rules>();
            //eqpt.Add(new Group_Access_Rules() { access_rule_id = 2, group_id = 2 });
            //eqpt.Add(new Group_Access_Rules() { access_rule_id = 3, group_id = 3 });

            //eqpt.Destroy(new Portal_Script_Parameters() { script_instance_id = 2, script_parameter_id = 3, sequence = 2, value = 2 });

            // eqpt.Add(new Portal_Script_Parameters() { script_parameter_id = 7, script_instance_id = 2, sequence = 2, value = 2 });

            //eqpt.Modify(new Portal_Actions() { portal_id = 1, action_id = 2 }, new Portal_Actions() { portal_id = 1, action_id = 1 });

            //eqpt.Destroy(new Portal_Access_Rules() { portal_id=1, access_rule_id=5 });
            //eqpt.Add(new Portal_Access_Rules() { portal_id = 1, access_rule_id = 5 });


            //eqpt.TryDestroyAll<Area_Access_Rules>();
            //eqpt.Add(new Area_Access_Rules() { area_id = 1, access_rule_id = 1 });
            //eqpt.Add(new Area_Access_Rules() { area_id = 2, access_rule_id = 2 });

            //eqpt.TryDestroyAll<Portal_Actions>();
            //eqpt.Add(new Portal_Actions() { portal_id = 2, action_id = 1 });
            //eqpt.Add(new Portal_Actions() { portal_id = 2, action_id = 2 });
            //eqpt.Add(new Portal_Actions() { portal_id = 3, action_id = 1 });
            //eqpt.Add(new Portal_Actions() { portal_id = 3, action_id = 2 });

            //eqpt.TryDestroyAll<Portal_Access_Rules>();
           // eqpt.Add(new Portal_Access_Rules() { portal_id = 1, access_rule_id = 2 });
           // eqpt.Add(new Portal_Access_Rules() { portal_id = 1, access_rule_id = 3 });

            //eqpt.TryDestroyAll<Access_Rule_Actions>();
            //eqpt.Add(new Access_Rule_Actions() { access_rule_id = 2, action_id = 2 }); 
            //eqpt.Add(new Access_Rule_Actions() { access_rule_id = 1, action_id = 1 });

            //eqpt.SetWigandPortal

            //var where = Device.WhereByObject(new Portal_Script_Parameters() { script_parameter_id = 1 });
            //eqpt.ModifyWhere(where, new Portal_Script_Parameters() { value = 1 });

            //eqpt.Add(new Access_Rule_Time_Zones() { access_rule_id = 2,  time_zone_id = 1}); // usando horario padrão
            //eqpt.Add(new Access_Rule_Time_Zones() { access_rule_id = 3, time_zone_id= 1 });

            //eqpt.Add(new Access_Rule_Actions() { access_rule_id = 2, action_id = 2 }); // supondo que a ação padrão já é abrir a porta 1
            //eqpt.Add(new Access_Rule_Actions() { access_rule_id = 1, action_id = 1 }); // supondo que a ação padrão já é abrir a porta 1
            //eqpt.Add(new Access_Rule_Actions() { access_rule_id = acesso2, action_id = 2 }); // supondo que a ação padrão já é abrir a porta 2
            // eqpt.TryDestroyAll<Access_Rule_Actions>();

            //eqpt.Add(new Portal_Access_Rules () { access_rule_id = acesso1, portal_id = 1 }); // supondo que o portal 1 está configurado o relê 1
            //eqpt.Add(new Portal_Access_Rules() { access_rule_id = acesso2, portal_id = 2 }); // supondo que o portal 1 está configurado o relê 1

            eqpt.Add(new Portal_Actions() {  portal_id = 1, action_id=1 }); // supondo que o portal 1 está configurado o relê 1

        }
    }
}