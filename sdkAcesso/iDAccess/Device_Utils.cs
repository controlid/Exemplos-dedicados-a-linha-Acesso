﻿using ControliD.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Security.Cryptography;

namespace ControliD.iDAccess
{
    //ADICIONAR DEVICE À LISTA

    public enum DeviceModels
    {
        iDAccess = 0,
        iDAccess_Pro = 1,
        iDLight = 2,
        iDAccess_Nano = 3,
        iDFit = 4,
        iDAccessProx = 5,
        iDBlock = 6,
        iDBlock_Braco = 7,
        iDBlock_Balcao = 8,
        iDBlock_Pne = 9,
        iDBox = 10,
        iDFlex = 11,
        iDX = 12,
        iDClass = 13,
        //iDAccess_Legacy = 14,
        //iDFlex_V2 = 15
    }

    public enum DeviceNames
    {

//Modelos iDAccess
        [Description("iDAccess Bio")]                       B,
        [Description("iDAccess Bio ASK")]                   A,
        [Description("iDAccess Bio ASK+FSK")]               C,
        [Description("iDAccess Bio ASK+PSK")]               D,
        [Description("iDAccess Bio ASK+MIFARE")]            E,
        [Description("iDAccess Prox ASK")]                  F,
        [Description("iDAccess Prox ASK+FSK")]              G,
        [Description("iDAccess Prox ASK+PSK")]              H,
        [Description("iDAccess Prox ASK+MIFARE")]           I,

//Modelos iDAccess Pro
        [Description("iDAccess Pro Bio")]                   _0E01,
        [Description("iDAccess Pro Bio ASK")]               _0E02,
        [Description("iDAccess Pro Bio MIFARE")]            _0E03,
        [Description("iDAccess Pro Prox ASK")]              _0E04,
        [Description("iDAccess Pro Prox MIFARE")]           _0E05,
        [Description("iDAccess Pro Bio Prox HID")]          _0E06,
        [Description("iDAccess Pro Prox HID")]              _0E07,

        //Modelos iDAccess Nano
        [Description("iDAccess Nano Bio")]                  _0F01,
        [Description("iDAccess Nano Bio ASK")]              _0F02,
        [Description("iDAccess Nano Bio MIFARE")]           _0F03,
        [Description("iDAccess Nano Prox ASK")]             _0F04,
        [Description("iDAccess Nano Prox MIFARE")]          _0F05,

//Modelos iDFlex V2
        [Description("iDFlex V2 Bio")]                      _0G01,
        [Description("iDFlex V2 Bio Prox ASK")]             _0G02,
        [Description("iDFlex V2 Bio Prox MIFARE")]          _0G03,
        [Description("iDFlex V2 Prox ASK")]                 _0G04,
        [Description("iDFlex V2 Prox MIFARE")]              _0G05,

//Modelos iDAccess Legacy 
        [Description("iDAccess Legacy Bio")]                _0I01,
        [Description("iDAccess Legacy Bio Prox ASK")]       _0I02,
        [Description("iDAccess Legacy Bio Prox MIFARE")]    _0I03,
        [Description("iDAccess Legacy Prox ASK")]           _0I04,
        [Description("iDAccess Legacy Prox MIFARE")]        _0I05,

// Modelos iDFit
        [Description("iDFit Bio")]                          N,
        [Description("iDFit Bio ASK")]                      M,
        [Description("iDFit Bio ASK+FSK")]                  O,
        [Description("iDFit Bio ASK+PSK")]                  P,
        [Description("iDFit Bio ASK+MIFARE")]               Q,

// Modelos Jalapeno
        [Description("iDBox Appliance")]                    J,
        [Description("iDBox Controler")]                    L,
        [Description("iDBlock")]                            K,
        [Description("iDBlock Plus")]                       S,
        [Description("x86")]                                R,
// Novos Seriais
        [Description("iDFlex Bio")]                         _0A01,
        [Description("iDFlex Bio Prox ASK")]                _0A02,
        [Description("iDFlex Bio Prox MIFARE")]             _0A03,
        [Description("iDFlex Prox ASK")]                    _0A04,
        [Description("iDFlex Prox MIFARE")]                 _0A05,

// Modelos Light (descontinuado)
        //[Description("iDAccess Light Bio")]               Z,
        //[Description("iDAccess Light Bio ASK")]           Y,
        //[Description("iDAccess Light Bio ASK+FSK")]       X,
        //[Description("iDAccess Light Bio ASK+PSK")]       W,
        //[Description("iDAccess Light Bio ASK+MIFARE")]    V,

        [Description("(não identificado)")]                 none
    }

    public partial class Device
    {

        public static bool IsCatraca (DeviceModels model)
        {
            return (model == DeviceModels.iDBlock || model == DeviceModels.iDBlock_Balcao || model == DeviceModels.iDBlock_Braco || model == DeviceModels.iDBlock_Pne);
        }

        public static bool IsCatraca(int model)
        {
            return (model == (int)DeviceModels.iDBlock || model == (int)DeviceModels.iDBlock_Balcao || model == (int)DeviceModels.iDBlock_Braco || model == (int)DeviceModels.iDBlock_Pne);
        }

        public static bool IsFlex(DeviceModels model)
        {
            return (model == DeviceModels.iDFlex || model == DeviceModels.iDAccess_Nano || model == DeviceModels.iDAccess_Pro);
        }


        public static DeviceNames GetDeviceName(string cSerial)
        {
            if(cSerial==null)
                return DeviceNames.none;

            DeviceNames dvm;
            if (cSerial.Length == 4)
            {
                if (Enum.TryParse(cSerial.Substring(0, 1), true, out dvm))
                    // TODO: identificar sub tipos: ask, fsk, ...
                    return dvm;
            }
            else if (cSerial.Length > 5) // 0A0210/000001
            {
                if (Enum.TryParse("_" + cSerial.Substring(0, 4), true, out dvm))
                    // TODO: identificar sub tipos: ask, fsk, ...
                    return dvm;
            }
            return DeviceNames.none;
        }

        public static string GetDeviceNameDescription(DeviceNames dvm)
        {
            Type tp = typeof(DeviceNames);
            FieldInfo fi = tp.GetField(dvm.ToString());
            DescriptionAttribute da = fi.GetCustomAttribute<DescriptionAttribute>();
            if (da != null)
                return da.Description;
            else
                return "(não especificado)";
        }

        public static string GetDeviceNameDescription(string Serial)
        {
            return GetDeviceNameDescription(GetDeviceName(Serial));
        }

        public static bool ModelHasCard(string cSerial)
        {
            string cModel = GetDeviceNameDescription(cSerial);
            if (cModel.Contains("iDBlock") || cModel.Contains("iDBox"))
                return true;
            else
                return cModel.Contains("ASK") || cModel.Contains("MIFARE") || cModel.Contains("Prox");
        }

        public static bool ModelHasBio(string cSerial)
        {
            string cModel = GetDeviceNameDescription(cSerial);
            if (cModel == "iDBlock" || cModel.Contains("iDBlock"))
                return true;
            else
                return cModel.Contains("Bio");
        }

        public static DeviceModels GetDeviceModel(string cSerial)
        {
            DeviceNames ds = GetDeviceName(cSerial);
            string cName = GetDeviceNameDescription(ds);
            if (cName.Contains("iDBlock"))
                return DeviceModels.iDBlock;
            else if (cName.Contains("iDBox"))
                return DeviceModels.iDBox;
            else if (cName.Contains("iDFit"))
                return DeviceModels.iDFit;
            else if (cName.Contains("iDLight"))
                return DeviceModels.iDLight;
            else if (cName.Contains("iDAccessProx") || cName.Contains("iDAccess Prox"))
                return DeviceModels.iDAccessProx;
            else if (cName.Contains("iDFlex"))
                return DeviceModels.iDFlex;
            else if (cName.Contains("iDAccess Pro"))
                return DeviceModels.iDAccess_Pro;
            else if (cName.Contains("iDAccess Nano"))
                return DeviceModels.iDAccess_Nano;
            else
                return DeviceModels.iDAccess;
        }

        public void ClearWigandPortal()
        {
            // Obtem o scripts relacionado a leitoras
            Scripts script = First<Scripts>(new Scripts() { script = "card%" });

            // Cria uma área inativa
            long inativo = LoadOrAdd<Areas>("inativo");

            // Obtem as leitoras disponíveis
            var prm = List<Script_Parameters>(new Script_Parameters() { script_id = script.id });
            for (int i = 0; i < prm.Length; i++)
            {
                Modify(i + 1, new Portals { name = "Portal " + (i + 1) + " inativo", area_from_id = inativo, area_to_id = inativo });
                var where = WhereByObject(new Portal_Script_Parameters() { script_parameter_id = prm[i].id });
                ModifyWhere(where, new Portal_Script_Parameters() { value = i + 1 });
            }
        }

        /// <summary>
        /// Altera a leitora relacionada a cada portal
        /// </summary>
        /// <param name="Leitora">Numero da leitora</param>
        /// <param name="Portal">Numero do portal (relê)</param>
        /// <returns></returns>
        public bool SetWigandPortal(int Leitora, int Portal, long AreaFrom = 0, long AreaTo = 0, string PortalName = null)
        {
            try
            {
                LastError = null;
                // Obtem o scripts relacionado a leitoras
                Scripts script = First<Scripts>(new Scripts() { script = "card%" });
                // Obtem as leitoras disponíveis
                Script_Parameters[] sParameters = List<Script_Parameters>(new Script_Parameters() { script_id = script.id });
                if (Leitora > 0 && Leitora <= sParameters.Length && Portal > 0 && Portal <= sParameters.Length)
                {
                    if (AreaFrom != 0 && AreaTo != 0)
                    {
                        if (PortalName == null)
                            PortalName = "Portal " + Leitora + ": " + AreaFrom + " para " + AreaTo;

                        Modify(Leitora, new Portals { name = PortalName, area_from_id = AreaFrom, area_to_id = AreaTo });
                    }

                    Leitora--; // ajusta base 0
                    var where = WhereByObject(new Portal_Script_Parameters() { script_parameter_id = sParameters[Leitora].id });
                    if (ModifyWhere(where, new Portal_Script_Parameters() { value = Portal }) > 0)
                        return true;
                }
            }
            catch (Exception ex)
            {
                LastError = ex;
            }
            return false;
        }

        //public bool SetWigandPortal(int Leitora, int Reley, long AreaFrom = 0, long AreaTo = 0, string PortalName = null)
        //{
        //    try
        //    {
        //        if (AreaFrom != 0 && AreaTo != 0 && AreaFrom != AreaTo)
        //        {
        //            if (PortalName == null)
        //                PortalName = "Portal " + Leitora + ": " + AreaFrom + " para " + AreaTo;

        //            Modify(Leitora, new Portals { name = PortalName, area_from_id = AreaFrom, area_to_id = AreaTo });
        //        }

        //        ModifyWhere(new Portal_Actions { portal_id = Leitora }, new Portal_Actions { portal_id = Leitora, action_id = Reley });

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        LastError = ex;
        //    }
        //    return false;
        //}

        /// <summary>
        /// Configura e libera um portal (leitora e relê) para um grupo de uma área a outra dentro de um horário
        /// </summary>
        /// <param name="Leitora">Número da Leitora a associar</param>
        /// <param name="Portal">Número da Saida(Rele)´que é um portal de uma área a outra</param>
        /// <param name="AreaFrom">Nome da área de origem</param>
        /// <param name="AreaTo">Nome da área de destino</param>
        /// <param name="Grupo">Nome do Grupo que terá acesso</param>
        /// <param name="Time">iD do horário</param>
        /// <returns>Verdadeiro se deu certo</returns>
        public bool SetWigandRuleAreaGroupTime(int Leitora, int Portal, long AreaFrom, long AreaTo, long Grupo, long Time)
        {
            // Leitora 2, libera o Relê 2, cuja oriem é o Hall e dá acesso a Engenharia
            if (SetWigandPortal(Leitora, Portal, AreaFrom, AreaTo))
            {
                // Cria uma nova regra de autorização: Tipo 1 é de autorizar, e a prioridade é a ordem a ser executado
                Access_Rules rule = LoadOrSet(0, new Access_Rules() { name = "(auto " + AreaFrom + " - " + AreaTo + ")", type = 1, priority = 0 });

                SetGroupAreaTime(rule.id, Grupo, AreaTo, Time);
                return true;
            }
            else
                return false;
        }

        public void SetGroupAreaTime(long Rule, long Grupo, long Area, long Time)
        {
            // Define que as a regra de acesso a 'Engenharia'
            Set(new Area_Access_Rules() { access_rule_id = Rule, area_id = Area }); // liberando por área

            // Dá acesso as pessoas do grupo engenharia a regra de acesso
            Set(new Group_Access_Rules() { access_rule_id = Rule, group_id = Grupo });

            // Libera o horario padrão (id:1) para o acesso da regra
            Set(new Access_Rule_Time_Zones() { access_rule_id = Rule, time_zone_id = Time });

        }

        public void SetUserAreaTime(long Rule, long User, long Area, long Time)
        {
            // Define que as a regra de acesso a 'Engenharia'
            Set(new Area_Access_Rules() { access_rule_id = Rule, area_id = Area }); // liberando por área

            // Dá acesso as pessoas do grupo engenharia a regra de acesso
            Set(new User_Access_Rules() { access_rule_id = Rule, user_id = User });

            // Libera o horario padrão (id:1) para o acesso da regra
            Set(new Access_Rule_Time_Zones() { access_rule_id = Rule, time_zone_id = Time });

        }

        /// <summary>
        /// Limpa as regras de acesso (sem excluir usuários, grupos, ou áreas)
        /// </summary>
        public string ClearAreasTimesRules()
        {
            string cOut = "";
            cOut += TryDestroyAll<Portal_Access_Rules>();
            cOut += TryDestroyAll<Area_Access_Rules>();
            cOut += TryDestroyAll<Access_Rule_Time_Zones>();
            cOut += TryDestroyAll<Group_Access_Rules>();
            cOut += TryDestroyAll<Access_Rules>();
            return cOut;
        }

        /// <summary>
        /// Tente apagar todos os objetos de um dado tipo, um a um
        /// </summary>
        public string TryDestroyAll<T>(bool lNoList = false) where T : GenericItem, new()
        {
            Exception ex = null;
            string cOut = "";
            if (lNoList)
            {
                ex = Try(() => DestroyWhere<T, WhereObjects>(null));
                if (ex != null)
                    cOut = "Não foi possível excluir todos os itens";
            }
            else
            {
                foreach (var i in List<T>())
                {
                    ex = Try(() => DestroyWhere<T, WhereObjects>(WhereByObject<T>(i)));
                    if (ex != null)
                        cOut += "Não foi possível excluir o item: " + i.ToString() + "\r\n";
                }
            }
            return cOut;
        }

        ///// <summary>
        ///// Imagens dos modelos
        ///// </summary>
        //public static Image[] ImageModels
        //{
        //    get
        //    {
        //        return new Image[] {
        //            Resources.ic_idaccess,
        //            Resources.ic_idfit,
        //            Resources.ic_idlight,
        //            Resources.ic_idaccessprox,
        //            Resources.ic_idblock,
        //            Resources.ic_idbox,
        //            Resources.ic_idflex,
        //        };
        //    }
        //}

        //public static Image ImageModel(DeviceModels model)
        //{
        //    switch (model)
        //    {
        //        case DeviceModels.iDAccess:
        //            return Resources.ic_idaccess;
        //        case DeviceModels.iDFit:
        //            return Resources.ic_idfit;
        //        case DeviceModels.iDLight:
        //            return Resources.ic_idlight;
        //        case DeviceModels.iDAccessProx:
        //            return Resources.ic_idaccessprox;
        //        case DeviceModels.iDBlock:
        //            return Resources.ic_idblock;
        //        case DeviceModels.iDBox:
        //            return Resources.ic_idbox;
        //        case DeviceModels.iDFlex:
        //            return Resources.ic_idflex;
        //    }
        //    return null;
        //}

        const int pbkdf_salt_length = 32;
        const int pbkdf_iterations = 1;
        const int pbkdf_digest_length = 32;

        // http://stackoverflow.com/questions/18137003/is-rfc2898derivebytes-equivalent-to-pkcs5-pbkdf2-hmac-sha1
        public static string GeneratePassword(long nValue, out string cSalt)
        {
            if (nValue == 0)
            {
                cSalt = "";
                return "";
            }

            // Gera um numero 'SALT' aleatório em um buffer de 32 bytes
            byte[] salt = new byte[pbkdf_salt_length];
            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < pbkdf_salt_length; i++)
                salt[i] = (byte)rnd.Next((int)' ', (int)'~');

            cSalt = System.Text.ASCIIEncoding.ASCII.GetString(salt);
            using (Rfc2898DeriveBytes password = new Rfc2898DeriveBytes(nValue.ToString(), salt, pbkdf_iterations))
            {
                byte[] keyBytes = password.GetBytes(pbkdf_digest_length);
                return BitConverter.ToString(keyBytes, 0).Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// Devolve hash da senha digitada
        /// </summary>
        /// <param name="nValue">senha textual</param>
        /// <param name="cSalt">salt</param>
        /// <returns>Hash da senha digitada</returns>
        public static string VerifyPassword(long nValue, string cSalt)
        {
            if (nValue == 0 || cSalt==null)
                return "";

            byte[] salt = System.Text.ASCIIEncoding.ASCII.GetBytes(cSalt);
            using (Rfc2898DeriveBytes password = new Rfc2898DeriveBytes(nValue.ToString(), salt, pbkdf_iterations))
            {
                byte[] keyBytes = password.GetBytes(pbkdf_digest_length);
                return BitConverter.ToString(keyBytes, 0).Replace("-", "").ToLower();
            }
        }
    }
}