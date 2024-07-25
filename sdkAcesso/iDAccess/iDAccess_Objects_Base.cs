using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ControliD.iDAccess
{
    /// <summary>
    /// Item Generico do Acesso
    /// </summary>
    [DataContract]
    public abstract class GenericItem : StatusResult
    {
        static ConcurrentDictionary<Type, FieldInfo[]> concurrentDictionary = new ConcurrentDictionary<Type, FieldInfo[]>();
        private static FieldInfo[] GetFieldInfoList(Type tp)
        {
            FieldInfo[] retorno = null;
            if (concurrentDictionary.TryGetValue(tp, out retorno))
                return retorno;

            List<FieldInfo> list = new List<FieldInfo>();
            foreach (FieldInfo fi in tp.GetFields())
            {
                if (Attribute.IsDefined(fi, typeof(DataMemberAttribute)))
                {
                    list.Add(fi);
                }
            }
            retorno = list.ToArray();
            concurrentDictionary.TryAdd(tp, retorno);
            return retorno;
        }

        /// <summary>
        /// Compara cada membro do objeto
        /// </summary>
        public virtual bool IsEquals(GenericItem obj2)
        {
            Type tp = this.GetType();
            if (obj2.GetType() != tp)
                return false; // Se for outro tipo já é diferente

            FieldInfo[] fi_list = GetFieldInfoList(tp);
            foreach (FieldInfo fi in fi_list)
            {                    
                // primeira diferença, já retorna que os objetos são diferentes
                if (fi.FieldType == typeof(string))
                {
                    if ((string)fi.GetValue(obj2) != (string)fi.GetValue(this))
                        return false;
                }
                else if (fi.FieldType == typeof(long))
                {
                    if ((long)fi.GetValue(obj2) != (long)fi.GetValue(this))
                        return false;
                }
                else if (fi.FieldType == typeof(int))
                {
                    if ((int)fi.GetValue(obj2) != (int)fi.GetValue(this))
                        return false;
                }
                else if (fi.FieldType == typeof(bool))
                {
                    if ((bool)fi.GetValue(obj2) != (bool)fi.GetValue(this))
                        return false;
                }
                else if (fi.FieldType == typeof(DateTime))
                {
                    if ((DateTime)fi.GetValue(obj2) != (DateTime)fi.GetValue(this))
                        return false;
                }
                else
                {
                    if (!fi.GetValue(obj2).Equals(fi.GetValue(this)))
                        return false;
                }
            }
            return true; // por eliminatória, é tudo igual!
        }
    }

    /// <summary>
    /// Objetos que permitem ser contados, pela função Count()
    /// </summary>
    [DataContract]
    public abstract class GenericCount : GenericItem
    {
        [DataMember(Name = "COUNT(*)", EmitDefaultValue = false)]
        public long Count;
    }

    /// <summary>
    /// Objetos que contem ID para identificação unica
    /// </summary>
    [DataContract]
    public abstract class GenericObject : GenericCount
    {
        [DataMember(EmitDefaultValue = false)]
        public long id; // pode ser que nunca seja usado esse campo em um objeto como é o cado de user_groups

        public override string ToString()
        {
            return id + ": (" + this.GetType().Name + ")";
        }
    }

    /// <summary>
    /// Objetos com ID e Nome
    /// </summary>
    [DataContract]
    public abstract class GenericObjectName : GenericObject
    {
        [DataMember(EmitDefaultValue = false)]
        public string name;

        public override string ToString()
        {
            return id + ": " + name + " (" + this.GetType().Name + ")";
        }
    }

    public enum EventTypes
    {
        /// <summary>
        /// ERRO ?
        /// </summary>
        none = 0,

        /// <summary>
        /// 1 - equipamento inválido
        /// </summary>
        DeviceInvalid = 1,

        /// <summary>
        /// 2 - parâmetros de regra de identificação inválidos
        /// </summary>
        parameter_invalid = 2,

        /// <summary>
        /// 3 - não identificado
        /// </summary>
        NotIdentify = 3,

        /// <summary>
        /// 4 - identificação pendente
        /// </summary>
        IdentifyPending = 4,

        /// <summary>
        /// 5 - timeout na identificação
        /// </summary>
        identifyTimeOut = 5,

        /// <summary>
        /// 6 - acesso negado
        /// </summary>
        AccessDeny = 6,

        /// <summary>
        /// 7 - acesso autorizado
        /// </summary>
        Allow = 7,

        /// <summary>
        /// 8 - acesso pendente
        /// </summary>
        AccessPending = 8,
        /// <summary>
        /// 13 - Desistencia
        /// </summary>
        GiveUp = 13,
        /// <summary>
        /// 11 - acesso pendente
        /// </summary>
        Buttonhole = 11,
         /// <summary>
         /// 12 - acesso pendente
         /// </summary>
        InterfaceWeb = 12

    }

    public enum RoleTypes
    {
        NoSet = -1,
        User = 0,
        Admin = 1
    }

    /// <summary>
    /// Retorno de um modo de autenticação por biometria, cartão ou código e senha
    /// </summary>
    public class IdentifyResult
    {
        public BiometricImage result;

        public IdentifyResult()
        {
        }

        public IdentifyResult(BiometricImage bio)
        {
            result = bio;
        }
    }

    public class DeviceIsAlive
    {
        public HttpStatusCode @event;
    }
    // { "result": { "event": 200 } }
    public class DeviceIsAliveResult
    {
        public DeviceIsAliveResult()
        {
        }

        public DeviceIsAliveResult(HttpStatusCode code)
        {
            result = new DeviceIsAlive();
            result.@event = code;
        }

        public DeviceIsAlive result;
    }

    /// <summary>
    /// Retorno de um processo de identificação
    /// </summary>
    /// 
    [DataContract]
    public class BiometricImage
    {
        [DataMember]
        public int @event;
        [DataMember]
        public long user_id;
        [DataMember]
        public string user_name;
        [DataMember]
        public int portal_id;
        [DataMember]
        public ActionItem[] actions;
        [DataMember]
        public bool user_image;
        [DataMember]
        public string user_image_hash;

        [DataMember(EmitDefaultValue = false)]
        public string next_event;

        [IgnoreDataMember]
        public EventTypes Event { get { return (EventTypes)@event; } set { @event = (int)value; } }

        [DataMember(EmitDefaultValue = false)]
        public string message;
    }
}