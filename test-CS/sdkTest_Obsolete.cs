using ControlID;
using ControlID.iDAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestAcesso
{
    // A existencia destas funções mais simples marcadas como 'Obsolete' são para um facil entendimento
    public partial class sdkTest
    {
        [TestMethod, TestCategory("sdk obsolete")]
        public void sdk_Users_List()
        {
            Users[] list = eqpt.UserList();
            foreach (Users u in list)
                Console.WriteLine(
                    u.id + ": " +
                    u.name + " - " +
                    u.registration +
                    (u.password == "" ? " " : "(senha) ")
                    ); // (u.expires == 0 ? "" : u.expires.FromUnix().ToString("dd/MM/yyyy"))
        }

        [TestMethod, TestCategory("sdk obsolete")]
        public void sdk_Users_CRUD()
        {
            Console.WriteLine("inclusão simples");
            long id1 = eqpt.UserAdd("sdk nome matricula", "sdk Matricula");
            Console.WriteLine("alteração simples"); 
            eqpt.UserModify(id1, "sdk novo alterado", "sdk X");

            Console.WriteLine("inclusão com senha"); 
            long id2 = eqpt.UserAdd("sdk senha", "sdk Matricula", 123);
            Console.WriteLine("exclusão"); 
            eqpt.UserDestroy(id2);

            Console.WriteLine("inclusão com id customizada"); 
            long id3 = eqpt.UserAdd("sdk id", "sdk Matricula", 0, new Random().Next());
            Console.WriteLine("modificando ID customizado");
            long id4 = new Random().Next();
            eqpt.UserModify(id3, "sdk alterado ID", "sdk OK", 0, id4);

            Console.WriteLine("ledo usuário");
            Users usr=eqpt.UserLoad(id4);
            Console.WriteLine(usr.name + " / " + usr.registration);
        }

        [TestMethod, TestCategory("sdk obsolete")]
        public void sdk_Groups_List()
        {
            Groups[] list = eqpt.GroupList();
            foreach (Groups g in list)
                Console.WriteLine(g.id + ": " + g.name);
        }

        [TestMethod, TestCategory("sdk obsolete")]
        public void sdk_Groups_CRUD()
        {
            Console.WriteLine("inclusão de grupos");
            long id1 = eqpt.GroupAdd("sdk grupo");
            Console.WriteLine("alteração de grupo");
            eqpt.GroupModify(id1, "sdk novo alterado");
            Console.WriteLine("leitura de grupo");
            Groups grp=eqpt.GroupLoad(id1);
            Console.WriteLine(grp.name);
            Console.WriteLine("exclusão de grupo");
            eqpt.GroupDestroy(id1);
        }
    }
}
