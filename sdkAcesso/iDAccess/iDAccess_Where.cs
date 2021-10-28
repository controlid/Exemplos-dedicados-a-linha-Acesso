using System;
using System.Runtime.Serialization;

namespace ControliD.iDAccess
{
    [DataContract]
    public class WhereObjects
    {
        [DataMember(EmitDefaultValue = false)]
        public Users users;
        [DataMember(EmitDefaultValue = false)]
        public Groups groups;
        [DataMember(EmitDefaultValue = false)]
        public User_Groups user_groups;
        [DataMember(EmitDefaultValue = false)]
        public User_Roles user_roles;
        [DataMember(EmitDefaultValue = false)]
        public Access_Logs access_logs;
        [DataMember(EmitDefaultValue = false)]
        public Alarm_Logs alarm_logs;
        [DataMember(EmitDefaultValue = false)]
        public Access_Log_Access_Rules access_log_access_rules;
        [DataMember(EmitDefaultValue = false)]
        public Cards cards;
        [DataMember(EmitDefaultValue = false)]
        public Contingency_Cards contingency_cards;
        [DataMember(EmitDefaultValue = false)]
        public Templates templates;
        [DataMember(EmitDefaultValue = false)]
        public Devices devices;
        [DataMember(EmitDefaultValue = false)]
        public Areas areas;
        [DataMember(EmitDefaultValue = false)]
        public Portals portals;
        [DataMember(EmitDefaultValue = false)]
        public Portal_Actions portal_actions;
        [DataMember(EmitDefaultValue = false)]
        public Actions actions;
        [DataMember(EmitDefaultValue = false)]
        public Scripts scripts;
        [DataMember(EmitDefaultValue = false)]
        public Script_Parameters script_parameters;
        [DataMember(EmitDefaultValue = false)]
        public Portal_Script_Parameters portal_script_parameters;
        [DataMember(EmitDefaultValue = false)]
        public Group_Access_Rules group_access_rules;
        [DataMember(EmitDefaultValue = false)]
        public User_Access_Rules user_access_rules;
        [DataMember(EmitDefaultValue = false)]
        public Validations validations;
        [DataMember(EmitDefaultValue = false)]
        public Access_Rule_Validations access_rule_validations;
        [DataMember(EmitDefaultValue = false)]
        public Access_Rules access_rules;
        [DataMember(EmitDefaultValue = false)]
        public Portal_Access_Rules portal_access_rules;
        [DataMember(EmitDefaultValue = false)]
        public Access_Rule_Actions access_rule_actions;
        [DataMember(EmitDefaultValue = false)]
        public Area_Access_Rules area_access_rules;
        [DataMember(EmitDefaultValue = false)]
        public Access_Rule_Time_Zones access_rule_time_zones;
        [DataMember(EmitDefaultValue = false)]
        public Contingency_Card_Access_Rules contingency_card_access_rules;
        [DataMember(EmitDefaultValue = false)]
        public Time_Zones time_zones;
        [DataMember(EmitDefaultValue = false)]
        public Time_Spans time_spans;
        [DataMember(EmitDefaultValue = false)]
        public Holidays holidays;
        [DataMember(EmitDefaultValue = false)]
        public Log_Types log_types;
    }

    [DataContract]
    public class ObjectResult
    {
        [DataMember()]
        public long[] ids;
        [DataMember()]
        public int changes;
        [DataMember()]
        public Users[] users;
        [DataMember()]
        public Groups[] groups;
        [DataMember()]
        public Areas[] areas;
        [DataMember()]
        public Portals[] portals;
        [DataMember()]
        public Portal_Actions[] portal_actions;
        [DataMember()]
        public User_Groups[] user_groups;
        [DataMember()]
        public User_Roles user_roles;
        [DataMember()]
        public Access_Logs[] access_logs;
        [DataMember()]
        public Alarm_Logs[] alarm_logs;
        [DataMember()]
        public Change_Logs[] change_logs;
        [DataMember()]
        public Access_Log_Access_Rules[] access_log_access_rules;
        [DataMember()]
        public Cards[] cards;
        [DataMember()]
        public Contingency_Cards[] contingency_cards;
        [DataMember()]
        public Templates[] templates;
        [DataMember()]
        public Devices[] devices;
        [DataMember()]
        public Actions[] actions;
        [DataMember()]
        public Scripts[] scripts;
        [DataMember()]
        public Script_Parameters[] script_parameters;
        [DataMember()]
        public Portal_Script_Parameters[] portal_script_parameters;
        [DataMember()]
        public Group_Access_Rules[] group_access_rules;
        [DataMember()]
        public User_Access_Rules[] user_access_rules;
        [DataMember()]
        public Access_Rule_Validations[] access_rule_validations;
        [DataMember()]
        public Access_Rules[] access_rules;
        [DataMember()]
        public Access_Rule_Actions[] access_rule_actions;
        [DataMember()]
        public Portal_Access_Rules[] portal_access_rules;
        [DataMember()]
        public Area_Access_Rules[] area_access_rules;
        [DataMember()]
        public Contingency_Card_Access_Rules[] contingency_card_access_rules;
        [DataMember()]
        public Validations[] validations;
        [DataMember()]
        public Access_Rule_Time_Zones[] access_rule_time_zones;
        [DataMember()]
        public Time_Zones[] time_zones;
        [DataMember()]
        public Time_Spans[] time_spans;
        [DataMember()]
        public Holidays[] holidays;
        [DataMember()]
        public Log_Types[] log_types;
        [DataMember()]
        public User_Image[] image_info;
        [DataMember()]
        public User_Images[] user_images;
    }

    [DataContract]
    public class ObjectResult2<T>
    {
        [DataMember()]
        public T[] users;
        [DataMember()]
        public T[] access_logs;
    }
    [DataContract]
    public class EnrollmentStateResult
    {
        [DataMember()]
        public string enroller_state;
        [DataMember()]
        public string biometry_sate;
        [DataMember()]
        public string last_enroll;
        [DataMember()]
        public string last_enroll_error;
    }
    public enum OrderTypes
    {
        None,
        Ascending,
        Descending
    }

    [DataContract]
    public class WhereCondicionalTemplate
    {
        [DataMember(EmitDefaultValue = false)]
        public WhereTemplate templates;
    }

    [DataContract]
    public class WhereCondicionalCard
    {
        [DataMember(EmitDefaultValue = false)]
        public WhereTemplate templates;
        [DataMember(EmitDefaultValue = false)]
        public WhereCard cards;
    }

    [DataContract]
    public class WhereCard
    {
        [DataMember(EmitDefaultValue = false)]
        public WhereFields user_id;
        [DataMember(EmitDefaultValue = false)]
        public WhereFields value;
    }

    [DataContract]
    public class WhereTemplate
    {
        [DataMember(EmitDefaultValue = false)]
        public WhereFields user_id;
    }

    [DataContract]
    public class WhereCondional
    {
        [DataMember(EmitDefaultValue = false)]
        public AccessLogsWhere access_logs;
    }

    [DataContract]
    public class UserListImagesResult
    {
        [DataMember()]
        public long[] user_ids;
    }

    [DataContract]
    public class AccessLogsWhere
    {
        [DataMember(EmitDefaultValue = false)]
        public WhereFields time;
        [DataMember(EmitDefaultValue = false)]
        public WhereFields id;
    }

    // "where":[{"field":"time","value":1433825471,"operator":">","connector":") AND ("}],
    [DataContract]
    public class WhereFields
    {
        [DataMember(EmitDefaultValue = false, Name = ">=")]
        public long MoreEqual;
        [DataMember(EmitDefaultValue = false, Name = "<=")]
        public long LessEqual;
        [DataMember(EmitDefaultValue = false, Name = ">")]
        public long More;
        [DataMember(EmitDefaultValue = false, Name = "<")]
        public long Less;
        [DataMember(EmitDefaultValue = false, Name = "=")]
        public long Equal;
        [DataMember(EmitDefaultValue = false)]
        public string field;
        [DataMember(EmitDefaultValue = false)]
        public string value;
        [DataMember(EmitDefaultValue = false, Name = "operator")]
        public string Operator;
        [DataMember(EmitDefaultValue = false)]
        public string connector;
        [DataMember(EmitDefaultValue = false, Name = "IN")]
        public long[] In;
        [DataMember(EmitDefaultValue = false, Name = "NOT IN")]
        public long[] NotIn;
    }

    [DataContract]
    public class ObjectRequest<T, W>
    {
        [DataMember(EmitDefaultValue = false)]
        public long offset;
        [DataMember(EmitDefaultValue = false)]
        public long limit=1000;
        [DataMember(EmitDefaultValue = false)] // O nome do objeto simples apenas no .Net é diferente
        public T values;
        [DataMember(EmitDefaultValue = false)]
        public W where;
        [DataMember(EmitDefaultValue = false)]
        public string[] fields;
        [DataMember(EmitDefaultValue = false)]
        public string[] order;
        [DataMember(Name = "object")] // Foi necessário por object é uma palavra reservada
        public string ObjectName { get; private set; }

        public ObjectRequest()
        {
            Type tp = typeof(T);
            if (tp.IsArray)
                ObjectName = tp.Name.ToLower().Replace("[]", ""); // Para inclusão remover a definição do grupo no nome do objeto
            else
                ObjectName = tp.Name.ToLower();
        }

        public ObjectRequest(string cObjectName)
        {
            ObjectName = cObjectName;
        }

        [IgnoreDataMember]
        public OrderTypes OrderType 
        {
            set{
                if (value == OrderTypes.Descending)
                    order = new string[] { "descending", "time" };
                else if (value == OrderTypes.Ascending)
                    order = new string[] { "ascending", "time" };
                else
                    order = null;
            }
        }
    }
}