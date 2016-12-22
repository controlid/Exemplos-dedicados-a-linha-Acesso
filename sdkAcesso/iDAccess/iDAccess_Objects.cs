using System;
using System.Runtime.Serialization;

namespace ControlID.iDAccess
{
    [DataContract]
    public class Groups : GenericObjectName
    {
    }

    [DataContract]
    public class Cards : GenericObject
    {
        [DataMember(EmitDefaultValue = false)]
        public long value;
        [DataMember(EmitDefaultValue = false)]
        public long user_id;
    }

    [DataContract]
    public class Templates : GenericObject
    {
        [DataMember(EmitDefaultValue = false)]
        public int finger_position;
        [DataMember(EmitDefaultValue = false)]
        public int finger_type;
        [DataMember(EmitDefaultValue = false)]
        public string template;
        [DataMember(EmitDefaultValue = false)]
        public long user_id;
    }

    //{"users",
    // "fields":["id","name","registration","password","salt","expires"],
    // "values":[{"id":456,"name":"Novo Visitante","registration":"mmm","password":"eb2bcaa3ad322ab680be8bd805bbde04ee898528fb8d94516afe1c33bc4fb062","salt":"A`\"a/?sk$*HA~BF(hpl|Z;RR?w4LGx35","expires":1435795200}]}
    // Na alteração tudo pode ser opcional, defini-se somente o valor necessário
    // Na inclusão o nome e a matrocula são obrigatórios
    [DataContract]
    public class Users : GenericObjectName
    {
        // Não pode haver valores default pois a mesma estrutura é usado em buscas

        [DataMember(EmitDefaultValue = false)]
        public string registration;
        [DataMember(EmitDefaultValue = false)]
        public string password;
        [DataMember(EmitDefaultValue = false)]
        public string salt;

        /// <summary>
        /// Gera a senha de um usuário 
        /// </summary>
        [IgnoreDataMember]
        public long Senha
        {
            set { password = Device.GeneratePassword(value, out salt); }
        }

        // Apenas para facilitar inclusão/alteração dos itens relacionados

        [IgnoreDataMember]
        public Templates[] Templates { set; get; }
        [IgnoreDataMember]
        public Cards[] Cards { set; get; }
        [IgnoreDataMember]
        public User_Groups[] Groups { set; get; }
        [IgnoreDataMember]
        public RoleTypes RoleType { set; get; }

        /// <summary>
        /// Variável de leitura da role por left join
        /// </summary>
        [DataMember(Name = "user_roles.role", EmitDefaultValue = false)]
        public int role;
    }

    [DataContract]
    public class User_Groups : GenericCount
    {
        [DataMember(EmitDefaultValue = false)]
        public long user_id;
        [DataMember(EmitDefaultValue = false)]
        public long group_id;
    }

    [DataContract]
    public class User_Roles : GenericObject
    {
        [DataMember(EmitDefaultValue = false)]
        public long user_id;
        [DataMember(EmitDefaultValue = false)]
        public int role;

        [IgnoreDataMember]
        public RoleTypes RoleType
        {
            get { return (RoleTypes)role; }
            set { role = (int)value; }
        }
    }

    [DataContract]
    public class Access_Rules : GenericObjectName
    {
        [DataMember(EmitDefaultValue = false)]
        public int? type;
        [DataMember(EmitDefaultValue = false)]
        public int? priority;
    }

    [DataContract]
    public abstract class Item_Access_Rules : GenericItem
    {
        [DataMember(EmitDefaultValue = false)]
        public long access_rule_id;

        public abstract void SetItem(long value);
    }

    [DataContract]
    public class Group_Access_Rules : Item_Access_Rules
    {
        [DataMember(EmitDefaultValue = false)]
        public long group_id;

        public override void SetItem(long value)
        {
            group_id = value;
        }
    }

    [DataContract]
    public class User_Access_Rules : Item_Access_Rules
    {
        [DataMember(EmitDefaultValue = false)]
        public long user_id;

        public override void SetItem(long value)
        {
            user_id = value;
        }
    }

    [DataContract]
    public class Access_Rule_Time_Zones : Item_Access_Rules
    {
        [DataMember(EmitDefaultValue = false)]
        public long time_zone_id;

        public override void SetItem(long value)
        {
            time_zone_id = value;
        }
    }

    [DataContract]
    public class Area_Access_Rules : Item_Access_Rules
    {
        [DataMember(EmitDefaultValue = false)]
        public long area_id;

        public override void SetItem(long value)
        {
            area_id = value;
        }
    }

    [DataContract]
    public class Access_Rule_Validations : Item_Access_Rules
    {
        [DataMember(EmitDefaultValue = false)]
        public long validation_id;

        public override void SetItem(long value)
        {
            validation_id = value;
        }
    }


    [DataContract]
    public class Portal_Access_Rules : Item_Access_Rules
    {
        [DataMember(EmitDefaultValue = false)]
        public long portal_id;

        public override void SetItem(long value)
        {
            portal_id = value;
        }
    }

    [DataContract]
    public class Access_Rule_Actions : Item_Access_Rules
    {
        [DataMember(EmitDefaultValue = false)]
        public long action_id;

        public override void SetItem(long value)
        {
            action_id = value;
        }
    }

    [DataContract]
    public class Validations : GenericObjectName
    {
    }

    [DataContract]
    public class Time_Zones : GenericObjectName
    {
    }

    [DataContract]
    public class Time_Spans : GenericObject
    {
        [DataMember(EmitDefaultValue = false)]
        public long time_zone_id;
        [DataMember()]
        public long start;
        [DataMember()]
        public long end;
        [DataMember()]
        public long sun;
        [DataMember()]
        public long mon;
        [DataMember()]
        public long tue;
        [DataMember()]
        public long wed;
        [DataMember()]
        public long thu;
        [DataMember()]
        public long fri;
        [DataMember()]
        public long sat;
    }

    // {"access_logs":[{
    //  "id":65,
    //  "time":1433955372,
    //  "event":3,
    //  "device_id":467700,
    //  "identifier_id":1651076864,
    //  "user_id":0,
    //  "portal_id":0,
    //  "identification_rule_id":0}]}
    [DataContract]
    public class Access_Logs : GenericObject
    {
        [DataMember(EmitDefaultValue = false)]
        public long time;
        [DataMember(EmitDefaultValue = false, Name = "event")]
        public int Event;
        [DataMember(EmitDefaultValue = false)]
        public long device_id;
        [DataMember(EmitDefaultValue = false)]
        public long identifier_id;
        [DataMember(EmitDefaultValue = false)]
        public long user_id;
        [DataMember(EmitDefaultValue = false)]
        public long portal_id;
        [DataMember(EmitDefaultValue = false)]
        public long identification_rule_id;

        [IgnoreDataMember]
        public EventTypes EventType { get { return (EventTypes)Event; } }

        /// <summary>
        /// Contém a data e hora do equipamento em Unix Timestamp.
        /// </summary>
        /// <see cref="http://pt.wikipedia.org/wiki/Era_Unix"/>
        [IgnoreDataMember]
        public DateTime Date { get { return time.FromUnix(); } }
    }

    [DataContract]
    public class Alarm_Logs : GenericObject
    {
        [DataMember(EmitDefaultValue = false)]
        public long time;
        [DataMember(EmitDefaultValue = false, Name = "event")]
        public int Event;
        /*
        1 - Alarm ativado
        2 - Alarm desativado
        */
        [DataMember(EmitDefaultValue = false)]
        public long user_id;
        [DataMember(EmitDefaultValue = false)]
        public int cause;
        /*
        Causas
        1 - Zona de alarme 1
        2 - Zona de alarme 2
        3 - Zona de alarme 3
        4 - Zona de alarme 4
        5 - Zona de alarme 5
        6 - Porta aberta
        7 - Arrombamento
        8 - Dedo de pânico
        9 - Tamper
        */
        [IgnoreDataMember]
        public DateTime Date { get { return time.FromUnix(); } }
    }

    /*
     "access_log_access_rules": [
    {
      "access_log_id": 6,
      "access_rule_id": 2
    },*/
    public class Access_Log_Access_Rules : GenericItem
    {
        [DataMember(EmitDefaultValue = false)]
        public long access_log_id;
        [DataMember(EmitDefaultValue = false)]
        public long access_rule_id;
    }

}