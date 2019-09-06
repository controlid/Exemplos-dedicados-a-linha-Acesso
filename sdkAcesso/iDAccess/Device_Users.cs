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
        /// Largura em pixel da imagem a ser enviada a qualquer device
        /// </summary>
        public const int DeviceImageWidth= 216;

        /// <summary>
        /// Altura em pixel da imagem a ser enviada a qualquer device
        /// </summary>
        public const int DeviceImageHeight = 178;

        /// <summary>
        /// Largura em pixel da imagem a ser enviada exclusivamente para a catraca que tem um display maior
        /// </summary>
        public const int DeviceBlockImageWidth = 185;

        /// <summary>
        /// Altura em pixel da imagem a ser enviada exclusivamente para a catraca que tem um display maior
        /// </summary>
        public const int DeviceBlockImageHeight = 185;

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
                        var width = DeviceImageWidth;
                        var height = DeviceImageHeight;
                        if (oFoto.Height > oFoto.Width)
                        {
                            height = oFoto.Height * width / oFoto.Width;
                        }
                        else
                        {
                            width = oFoto.Width * height / oFoto.Height;
                        }
                        Bitmap bmp = new Bitmap(width, height);
                        Graphics graph = Graphics.FromImage(bmp);
                        
                        graph.DrawImage(oFoto, 0, 0, width, height);
                        oSend = bmp;
                    }
                    else
                        oSend = oFoto;

                    WebJson.JsonCommand<string>(URL + "user_set_image.fcgi?user_id=" + nUserID + "&session=" + Session, oSend, null, TimeOut, System.Drawing.Imaging.ImageFormat.Jpeg);

                    oFoto.Dispose();
                    oSend.Dispose();
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

        

        /// <summary>
        /// Define a foto de uma lista de usuário
        /// </summary>
        public void SetUserImageList(UserImage[] userImages, bool lTry = false, bool resize = false)
        {
            CheckSession();
            try
            {
                System.Collections.Generic.List<UserImage> listUserImagePayload = new System.Collections.Generic.List<UserImage>();
                int byteLength = 0;
                foreach (UserImage userImage in userImages)
                {
                    Image oFoto = userImage.photo;
                    if (resize)
                    {
                        var width = DeviceImageWidth;
                        var height = DeviceImageHeight;
                        if (oFoto.Height > oFoto.Width)
                        {
                            height = oFoto.Height * width / oFoto.Width;
                        }
                        else
                        {
                            width = oFoto.Width * height / oFoto.Height;
                        }
                        Bitmap bmp = new Bitmap(width, height);
                        using (Graphics graph = Graphics.FromImage(bmp))
                            graph.DrawImage(oFoto, 0, 0, width, height);

                        oFoto.Dispose();
                        userImage.photo.Dispose(); //Remove a foto velha
                        userImage.photo = bmp;
                        bmp.Dispose();
                    }
                    else
                    {
                        userImage.photo = oFoto;
                        oFoto.Dispose();
                    }
                    using (System.IO.MemoryStream m = new System.IO.MemoryStream())
                    {
                        userImage.photo.Save(m, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] imageBytes = m.ToArray();                        
                        userImage.image = Convert.ToBase64String(imageBytes);
                        byteLength += imageBytes.Length;
                        listUserImagePayload.Add(userImage);
                    }
                    
                    if(byteLength > 1000000) // Se payload com mais de 1MB, envia para o device
                    {
                        UserImagesRequest payload = new UserImagesRequest()
                        {
                            user_images = listUserImagePayload.ToArray(),
                        };
                        byteLength = 0;
                        WebJson.JsonCommand<string>(URL + "user_set_image_list.fcgi?&session=" + Session, payload, null, TimeOut, System.Drawing.Imaging.ImageFormat.Jpeg);
                        foreach (var disposePayload in listUserImagePayload )
                        {
                            disposePayload.photo.Dispose();
                        }
                        listUserImagePayload.Clear();                        
                    }
                }
                if (listUserImagePayload.Count > 0)
                {
                    UserImagesRequest payload = new UserImagesRequest()
                    {
                        user_images = listUserImagePayload.ToArray(),
                    };
                    WebJson.JsonCommand<string>(URL + "user_set_image_list.fcgi?&session=" + Session, payload, null, TimeOut, System.Drawing.Imaging.ImageFormat.Jpeg);
                    foreach (var disposePayload in listUserImagePayload)
                    {
                        disposePayload.photo.Dispose();
                    }
                    listUserImagePayload.Clear();
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

        /// <summary>
        /// Retorna uma lista de id de pessoas com uma foto
        /// </summary>
        /// <returns></returns>
        public long[] UserListImages()
        {
            var result = WebJson.JsonCommand<UserListImagesResult>(URL + "user_list_images.fcgi?&session=" + Session);
            return result.user_ids;
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
