using System;
using System.Drawing;

namespace ControliD.iDAccess
{
    // https://www.controlid.com.br/suporte/api_idaccess_V2.2.html#51_load_objects
    public partial class Device
    {
        /// <summary>
        /// Define se os passwords serão gerados local, ou por meio de request ao equipamento
        /// </summary>
        public static bool GenerateLocalPassword = true;

        public void SetPassword(ref Users usr, long nSenha)
        {
            if (GenerateLocalPassword)
                usr.password = Device.GeneratePassword(nSenha, out usr.salt);
            else
            {
                PasswordResult pw = Command<PasswordResult>("user_hash_password", "{\"password\":\"" + nSenha.ToString() + "\"}");
                usr.password = pw.password;
                usr.salt = pw.salt;
            }
        }

        public string SetRole(long idUser, RoleTypes roleTypes)
        {
            // Sempre apaga a permissão
            int d = DestroyWhere<User_Roles, WhereObjects>(new WhereObjects()
            {
                user_roles = new User_Roles()
                {
                    user_id = idUser
                }
            });
            // Somente admin devem estar na lista de admin
            if (roleTypes == RoleTypes.Admin)
            {
                long a = Add<User_Roles>(new User_Roles()
                {
                    user_id = idUser,
                    RoleType = RoleTypes.Admin
                });
                return a > 0 ? "ROLE ADMIN" : "ERRO ADMIN";
            }
            else
                return d > 0 ? "ROLE USER" : "USER";
        }

        public Bitmap GetUserImage(long nUserID)
        {
            CheckSession();
            return WebJson.JsonCommand<Bitmap>(URL + "user_get_image.fcgi?user_id=" + nUserID + "&session=" + Session, null, null, TimeOut);
        }

        /// <summary>
        /// Define a foto de um usuário, ou a remove se for informado 'null'
        /// </summary>
        public void SetUserImage(long nUserID, Image oFoto, bool lTry=false, bool lRecise=false)
        {
            CheckSession();
            try
            {
                if (oFoto == null)
                    WebJson.JsonCommand<string>(URL + "user_destroy_image.fcgi?&session=" + Session, "{\"user_id\":" + nUserID + "}", null, TimeOut);
                else
                {
                    Image oSend;
                    if (lRecise)
                    {
                        Bitmap bmp = new Bitmap(200, 150);
                        Graphics graph = Graphics.FromImage(bmp);
                        graph.DrawImage(oFoto, 0, 0, 200, 150);
                        oSend = bmp;
                    }
                    else
                        oSend = oFoto;

                    WebJson.JsonCommand<string>(URL + "user_set_image.fcgi?user_id=" + nUserID + "&session=" + Session, oSend, null, TimeOut, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
            catch (Exception ex)
            {
                if (lTry)
                    LastError = ex;
                else
                    throw ex;
            }
        }

        [Obsolete("Experimente usar as chamadas Genericas<Objeto> para padronizar")]
        public Users[] UserList(WhereObjects oWhere = null)
        {
            return Command<ObjectResult>("load_objects", new ObjectRequest<Users, WhereObjects>()
            {
                where = oWhere
            }).users;
        }

        [Obsolete("Experimente usar as chamadas Genericas<Objeto> para padronizar")]
        public Users UserLoad(long nID)
        {
            Users[] usr = UserList(new WhereObjects() { users = new Users() { id = nID } });
            if (usr.Length == 1)
                return usr[0];
            else
                return null;
        }

        [Obsolete("Experimente usar as chamadas Genericas<Objeto> para padronizar")]
        public long UserAdd(string cName, string cRegistrarion, long nSenha = 0, long nID = 0)
        {
            Users usr = new Users()
            {
                id = nID,
                name = cName,
                registration = cRegistrarion
            };
            if (nSenha > 0)
                SetPassword(ref usr, nSenha);

            return Command<ObjectResult>("create_objects", new ObjectRequest<Users[], WhereObjects>()
            {
                values = new Users[] { usr }
            }).ids[0];
        }

        [Obsolete("Experimente usar as chamadas Genericas<Objeto> para padronizar")]
        public bool UserModify(long nID, string cName = null, string cRegistrarion = null, long nSenha = 0, long nNewID = 0)
        {
            Users usr = new Users() { id = nNewID, name = cName, registration = cRegistrarion };
            if (nSenha > 0)
                SetPassword(ref usr, nSenha);

            ObjectResult or = Command<ObjectResult>("modify_objects", new ObjectRequest<Users, WhereObjects>()
            {
                values = usr,
                where = new WhereObjects() { 
                    users = new Users() { id = nID } }
            });
            return or.changes == 1;
        }

        [Obsolete("Experimente usar as chamadas Genericas<Objeto> para padronizar")]
        public bool UserDestroy(long nID)
        {
            return Command<ObjectResult>("destroy_objects", new ObjectRequest<Users, WhereObjects>()
            {
                where = new WhereObjects()
                {
                    users = new Users() { id = nID }
                }
            }).changes == 1;
        }
    }
}
