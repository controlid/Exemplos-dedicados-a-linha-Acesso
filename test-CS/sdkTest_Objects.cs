using ControlID;
using ControlID.iDAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
            eqpt.Destroy<User_Groups, WhereObjects>(new WhereObjects() { user_groups = new User_Groups() { group_id = idG, user_id = idU } });

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
            eqpt.Destroy<User_Roles, WhereObjects>(new WhereObjects() { user_roles = new User_Roles() { user_id = 222 } });

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
    }
}