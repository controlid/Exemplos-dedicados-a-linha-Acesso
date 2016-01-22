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
    }
}