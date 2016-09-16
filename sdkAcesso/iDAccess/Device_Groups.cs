using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlID.iDAccess
{
    // https://www.controlid.com.br/suporte/api_idaccess_V2.2.html#51_load_objects
    public partial class Device
    {
        [Obsolete("Experimente usar as chamadas Genericas<Objeto> para padronizar")]
        public Groups[] GroupList(WhereObjects oWhere = null)
        {
            return Command<ObjectResult>("load_objects", new ObjectRequest<Groups, WhereObjects>() { where = oWhere }).groups;
        }

        [Obsolete("Experimente usar as chamadas Genericas<Objeto> para padronizar")]
        public Groups GroupLoad(long nID)
        {
            Groups[] group = GroupList(new WhereObjects()
            {
                groups = new Groups() { id = nID }
            });
            if (group.Length == 1)
                return group[0];
            else
                return null;
        }

        [Obsolete("Experimente usar as chamadas Genericas<Objeto> para padronizar")]
        public long GroupAdd(string cName)
        {
            return Command<ObjectResult>("create_objects", new ObjectRequest<Groups[], WhereObjects>()
            {
                values = new Groups[] { new Groups() { name = cName } }
            }).ids[0];
        }

        [Obsolete("Experimente usar as chamadas Genericas<Objeto> para padronizar")]
        public bool GroupModify(long nID, string cName)
        {
            return Command<ObjectResult>("modify_objects", new ObjectRequest<Groups, WhereObjects>()
            {
                values = new Groups() { name = cName },
                where = new WhereObjects() { groups = new Groups() { id = nID } }
            }).changes == 1;
        }

        [Obsolete("Experimente usar as chamadas Genericas<Objeto> para padronizar")]
        public bool GroupDestroy(long nID)
        {
            return Command<ObjectResult>("destroy_objects", new ObjectRequest<Groups, WhereObjects>()
            {
                where = new WhereObjects() { groups = new Groups() { id = nID } }
            }).changes == 1;
        }
    }
}