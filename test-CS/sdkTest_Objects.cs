using ControlID.iDAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;

namespace UnitTestAcesso
{
    public partial class sdkTest
    {
        [TestMethod, TestCategory("sdk generic")]
        public void sdk_Generic_UserList()
        {
            Users[] list = eqpt.List<Users>();
            foreach (Users u in list)
                Console.WriteLine(
                    u.id + ": " +
                    u.name + " - " +
                    u.registration +
                    (u.password == "" ? " " : "(senha) ")
                    ); // (u.expires == 0 ? "" : u.expires.FromUnix().ToString("dd/MM/yyyy"))
        }

        [TestMethod, TestCategory("sdk generic")]
        public void sdk_Generic_GroupList()
        {
            Groups[] list = eqpt.List<Groups>();
            foreach (Groups g in list)
                Console.WriteLine(g.id + ": " + g.name);
        }

        [TestMethod, TestCategory("sdk generic")]
        public void sdk_Generic_UserGroupCRUD()
        {
            Console.WriteLine("inclusão de Usuários");
            long idU = eqpt.Add<Users>(new Users() { name = "Generic Usuários", registration = "X" });
            Console.WriteLine("alteração de Usuários Generico");
            Assert.IsTrue(eqpt.Modify<Users>(idU, new Users() { name = "Generic Usuários Alterado" }));
            Console.WriteLine("load do usuário");
            Users usr = eqpt.Load<Users>(idU);
            Console.WriteLine(usr.name);

            Console.WriteLine("inclusão de grupos Generico");
            long idG = eqpt.Add<Groups>(new Groups() { name = "Generic Group" });
            Console.WriteLine("alteração de grupo Generico");
            Assert.IsTrue(eqpt.Modify<Groups>(idG, new Groups() { name = "Generic Group Alterado" }));
            Groups grp = eqpt.Load<Groups>(idG);
            Console.WriteLine("load do grupo");
            Console.WriteLine(grp.name);

            Console.WriteLine("inclusão de Usuários no Grupo");
            long idUG = eqpt.Add<User_Groups>(new User_Groups() { group_id = idG, user_id = idU });

            Console.WriteLine("exclusão de Usuários do Grupo");
            eqpt.DestroyWhere<User_Groups, WhereObjects>(new WhereObjects() { user_groups = new User_Groups() { group_id = idG, user_id = idU } });

            Console.WriteLine("exclusão de Usuários Generico");
            Assert.IsTrue(eqpt.Destroy<Users>(idU));

            Console.WriteLine("exclusão de grupo Generico");
            Assert.IsTrue(eqpt.Destroy<Groups>(idG));
        }

        [TestMethod, TestCategory("sdk generic")]
        public void sdk_Generic_Counts()
        {
            Console.WriteLine("Total de Usuários: " + eqpt.Count<Users>());
            Console.WriteLine("Total de Grupos: " + eqpt.Count<Groups>());
            Console.WriteLine("Total de Usuários em Grupos: " + eqpt.Count<User_Groups>());
        }

        [TestMethod, TestCategory("sdk generic")]
        public void sdk_Generic_Cards()
        {
            Cards[] list = eqpt.List<Cards>();
            foreach (Cards c in list)
                Console.WriteLine(c.id + ": " + c.value);
        }

        [TestMethod, TestCategory("sdk generic")]
        public void sdk_Generic_Templates()
        {
            Templates[] list = eqpt.List<Templates>();
            foreach (Templates t in list)
                Console.WriteLine(t.id + ": " + t.template);
        }

        [TestMethod, TestCategory("sdk generic")]
        public void sdk_UserPassword()
        {
            Console.WriteLine("Apagando o usuário 111: " + eqpt.Destroy<Users>(111));

            Console.WriteLine("inclusão de um usuário com senha");
            long idU = eqpt.Add<Users>(new Users()
            {
                id = 111,
                name = "Usuario com Senha",
                registration = "senha 123",
                Senha = 123
            });
            Console.WriteLine("Usuário ID: " + idU + " criado com a senha '123'");
        }

        [TestMethod, TestCategory("sdk generic")]
        public void sdk_UserAdmin()
        {
            // Apaga as informações do usuário 222 anterior 
            Console.WriteLine("Apagando o usuário 222: " + eqpt.Destroy<Users>(222));
            eqpt.DestroyWhere<User_Roles, WhereObjects>(new WhereObjects() { user_roles = new User_Roles() { user_id = 222 } });

            Console.WriteLine("inclusão de um usuário admin");
            long idU = eqpt.Add<Users>(new Users()
            {
                id = 222,
                name = "Usuario Admin com Senha",
                registration = "senha 456",
                Senha = 456
            });
            eqpt.Add<User_Roles>(new User_Roles() { user_id = idU, RoleType = RoleTypes.Admin });
            Console.WriteLine("Usuário Admin ID: " + idU + " criado com a senha '456'");

            // Forma simples de definição
            eqpt.SetRole(idU, RoleTypes.User);
        }

        [TestMethod, TestCategory("sdk generic")]
        public void sdk_SetGetImage()
        {
            long idU = eqpt.Add<Users>(new Users()
            {
                name = "Usuario Admin com Imagem",
                registration = "imagem",
            });
            Bitmap bmp = new Bitmap("../../ControliD.png");
            eqpt.SetUserImage(idU, bmp);
            bmp = eqpt.GetUserImage(idU);
        }

        [TestMethod, TestCategory("sdk BOX")]
        public void sdk_Generic_Rules()
        {
            eqpt.ClearWigandPortal(); // Inativa as leitoras e relês
            eqpt.ClearAreasTimesRules(); // Limpa todas as regras!
            
            // Parametros da Autorização: supondo o acesso a duas áreas
            int nLeitoraWigandEntrada = 2;
            int nLeitoraWigandSaida = 3;
            int nRelePortal = 2;
            long nAreaFora = eqpt.LoadOrAdd<Groups>("Hall");
            long nAreaDentro = eqpt.LoadOrAdd<Groups>("Area Engenharia");
            long nGrupo = eqpt.LoadOrAdd<Groups>("Grupo Engenharia");

            // As duas linhas seguintes abaixo representão toda a logica existente em seguida
            //if(eqpt.SetWigandRuleAreaGroupTime(nLeitoraWigandEntrada, nRelePortal, cAreaHall, cAreaEngenharia, cGrupo, 1))
            //    eqpt.SetWigandRuleAreaGroupTime(nLeitoraWigandSaida, nRelePortal, cAreaEngenharia, cAreaHall, cGrupo, 1);

            // Leitora 2, libera o Relê 2, cuja oriem é o Hall e dá acesso a Engenharia
            if (eqpt.SetWigandPortal(nLeitoraWigandEntrada, nRelePortal, nAreaFora, nAreaDentro))
            {
                // Cria uma nova regra de autorização: Tipo 1 é de autorizar, e a prioridade é a ordem a ser executado
                Access_Rules rule = eqpt.LoadOrSet(0, new Access_Rules() { name = "(auto Dentro)", type = 1, priority = 0 });

                // Define que as a regra de acesso a 'Engenharia'
                eqpt.Set(new Area_Access_Rules() { access_rule_id = rule.id, area_id = nAreaDentro }); // liberando por área
                // eqpt.Set(new Portal_Access_Rules() { access_rule_id = rule.id, portal_id = nRelePortal }); // liberando por portal

                // Dá acesso as pessoas do grupo engenharia a regra de acesso
                eqpt.Set(new Group_Access_Rules() { access_rule_id = rule.id, group_id = nGrupo });

                // Libera o horario padrão (id:1) para o acesso da regra
                eqpt.Set(new Access_Rule_Time_Zones() { access_rule_id = rule.id, time_zone_id = 1 });

                // Agora para sair, usando outro leitor é preciso criar novas regras e acessos
                if (eqpt.SetWigandPortal(nLeitoraWigandSaida, nRelePortal, nAreaDentro, nAreaFora))
                {
                    // Cria uma nova regra de autorização: Tipo 1 é de autorizar, e a prioridade é a ordem a ser executado
                    Access_Rules ruleOut = eqpt.LoadOrSet(0, new Access_Rules() { name = "(auto Fora)", type = 1, priority = 0 });

                    // Define que as a regra de acesso ao 'Hall'
                    eqpt.Set(new Area_Access_Rules() { access_rule_id = ruleOut.id, area_id = nAreaFora }); // liberando por área
                    // eqpt.Set(new Portal_Access_Rules() { access_rule_id = ruleOut.id, portal_id = nRelePortal }); // liberando por portal

                    // Dá acesso as pessoas do grupo engenharia a regra de acesso
                    eqpt.Set(new Group_Access_Rules() { access_rule_id = ruleOut.id, group_id = nGrupo });

                    // Libera o horario padrão (id:1) para o acesso da regra de saida
                    eqpt.Set(new Access_Rule_Time_Zones() { access_rule_id = ruleOut.id, time_zone_id = 1 });
                }
            }
            else
                Assert.Inconclusive("Eoo ao definir a Leitora ao Portal");
            
            //Console.WriteLine("\r\nCards:");
            //var cards = eqpt.List<Cards>();
            //foreach (var i in cards)
            //    Console.WriteLine(i.id + ": " + i.value + " user: " + i.user_id);

            Users user = eqpt.Load<Users>(1);
            Console.WriteLine("\r\nUser Card Test: " + user.id + " - " + user.name);

            Console.WriteLine("\r\nGrupos:");
            foreach (var i in eqpt.List<Groups>())
                Console.WriteLine(i.id + ": " + i.name);

            Console.WriteLine("\r\nAreas:");
            foreach (var i in eqpt.List<Areas>())
                Console.WriteLine(i.id + ": " + i.name);

            Console.WriteLine("\r\nPortals:");
            foreach (var i in eqpt.List<Portals>())
                Console.WriteLine(i.id + ": " + i.name + (i.name.EndsWith("inativo") ? "" : (" - Areas: " + i.area_from_id + " => " + i.area_to_id)));

            Console.WriteLine("\r\nAccess Rules:");
            foreach (var i in eqpt.List<Access_Rules>())
                Console.WriteLine(i.id + ": " + i.name + " - tipo " + i.type + " prioridade " + i.priority);

            Console.WriteLine("\r\nRules Groups:");
            foreach (var i in eqpt.List<Group_Access_Rules>())
                Console.WriteLine("Group: " + i.group_id + " Rule: " + i.access_rule_id);

            Console.WriteLine("\r\nArea Access Rules:");
            foreach (var i in eqpt.List<Area_Access_Rules>())
                Console.WriteLine("Area: " + i.area_id + " Rule: " + i.access_rule_id);

            Console.WriteLine("\r\nAccess Rule Time Zone:");
            foreach (var i in eqpt.List<Access_Rule_Time_Zones>())
                Console.WriteLine("Time: " + i.time_zone_id + " Rule: " + i.access_rule_id);

            Console.WriteLine("\r\nPortal Access Rules:");
            foreach (var i in eqpt.List<Portal_Access_Rules>())
                Console.WriteLine("Portal: " + i.portal_id + " Rule: " + i.access_rule_id);

            Console.WriteLine("\r\nPortal Script Parameters:");
            foreach (var i in eqpt.List<Portal_Script_Parameters>())
                Console.WriteLine("script_parameter_id: " + i.script_parameter_id + ": script_instance_id: " + i.script_instance_id + " sequence: " + i.sequence + " value: " + i.value);

            Console.WriteLine("\r\nActions:");
            foreach (var i in eqpt.List<Actions>())
                Console.WriteLine(i.id + ": " + i.name + " - action: " + i.action + " parameters: " + i.parameters);
        }
    }
}