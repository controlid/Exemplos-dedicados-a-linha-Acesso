using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ControlID.iDAccess
{
    // ação e configuração
    public enum iDBlockDirection
    {
        Clockwise = 0,
        Anticlockwise = 1,
        Both = 3
    }

    //// configuração
    //public enum iDBlockGateway
    //{
    //    Clockwise = 0,
    //    Anticlockwise = 1,
    //    Both = 3
    //}

    public enum iDBlockOperationMode
    {
        Blocked = 0,
        Entrance_Open = 1,
        Exit_Open = 2,
        Both_Open = 3
    }

   
    [DataContract]
    public class StatusResult
    {
        [DataMember(Name = "code", EmitDefaultValue = false)]
        public int Codigo;

        [DataMember(Name = "error", EmitDefaultValue = false)]
        public string Status;

        [DataMember(Name = "stack", EmitDefaultValue = false)]
        public string Stack;

        [DataMember(Name = "newID", EmitDefaultValue = false)]
        public long NewID;

        public bool isOK { get { return Codigo == 200; } }

        public StatusResult()
        { }

        public StatusResult(int code, string status, long id = 0, string stack = null)
        {
            this.Codigo = code;
            this.Status = status;
            this.NewID = id;
            this.Stack = stack;
        }
    }

    /// <summary>
    /// Parâmetros de configuração desejados do equipamento
    /// </summary>
    [DataContract]
    public class ConfigKeys
    {
        [DataMember(EmitDefaultValue = false)]
        public List<string> general = new List<string>();
        [DataMember(EmitDefaultValue = false)]
        public List<string> online_client = new List<string>();

        public void AskDayLightSavingTime()
        {
            general.Add("daylight_savings_time_start");
            general.Add("daylight_savings_time_end");
        }

        public void AskOnlineInfo()
        {
            general.Add("online");
            general.Add("local_identification");
            online_client.Add("extract_template");
            online_client.Add("server_id");
        }
    }

    public class ImageFingerprint
    {
        public string image;
        public int width;
        public int height;
    }

    public class NotificationTemplate
    {
        public string template;
        public long user_id;
        public long finger_type;
        public long device_id;
        public ImageFingerprint [] fingerprints;
    }

    public class NotificationCard
    {
        public string value;
        public long user_id;
        public long device_id;
        public long card_value;
    }
    ////"event":{"type": 7, "name": 'TURN LEFT', "time": 1484126902 },"device_id": 935107 
    //public class NotificationCatraEvents
    //{
    //    public NotificatonCatra[] @event;
    //    public long device_id;
    //}
    //public class NotificatonCatra
    //{
    //    public string type;
    //    public string name;
    //    public string time;
    //}
    public class NotificationItem
    {
        public NotificationObject[] object_changes;
        public NotificationConfig[] config_changes;
        public long device_id;
    }

    //{"object":"tabela","type":"inserted"}
    public class NotificationObject
    {
        public string @object;
        public string type;
    }

    //{"module": "modulo", "param":"parametro","value":"valor"}
    public class NotificationConfig
    {
        [DataMember(EmitDefaultValue = false)]
        public string module;

        [DataMember(EmitDefaultValue = false)]
        public string param;

        [DataMember(EmitDefaultValue = false)]
        public string value;
    }

    [DataContract]
    public class FirmwareUpdateFS
    {
        [DataMember]
        public string url;
        [DataMember]
        public string no_override_list;
        [DataMember(EmitDefaultValue = false)]
        public string patch;

        public FirmwareUpdateFS()
        {
        }
        public FirmwareUpdateFS(string cURL, string noList)
        {
            url = cURL;
            no_override_list = noList;
        }
    }

    [DataContract]
    public class FirmwareUpdateConfig
    {
        [DataMember]
        public string config_update;
        [DataMember]
        public string database_update;

        public FirmwareUpdateConfig()
        {
        }
        public FirmwareUpdateConfig(string cfg, string sql)
        {
            config_update = cfg;
            database_update = sql;
        }
    }

    [DataContract]
    public class FirmwareUpdate
    {
        [DataMember]
        public FirmwareUpdateFS configuration;
        [DataMember]
        public FirmwareUpdateFS fs_root;
        [DataMember]
        public FirmwareUpdateFS fs_user;
        [DataMember]
        public FirmwareUpdateConfig update;
        [DataMember]
        public string kernel_standard;
        [DataMember]
        public string kernel_recovery;
        [DataMember]
        public string boot;
        [DataMember]
        public string acfw;
        [DataMember]
        public string version;

        /* http://controlid.com.br/idaccess/acfw_update.php?SERIAL=A123&VERSION=3.6.0
        {  
           "configuration":{  
              "url":"http:\/\/controlid.com.br\/idaccess\/ACFW\/V3.6.0\/config.tar",
              "no_override_list":"http:\/\/controlid.com.br\/idaccess\/ACFW\/V3.6.0\/configuration_no_override_list"
           },
           "fs_root":{  
              "url":"http:\/\/controlid.com.br\/idaccess\/ACFW\/V3.6.0\/fs_root.tar",
              "no_override_list":"http:\/\/controlid.com.br\/idaccess\/ACFW\/V3.6.0\/rootfs_no_override_list"
           },
           "fs_user":{  
              "url":"http:\/\/controlid.com.br\/idaccess\/ACFW\/V3.6.0\/fs_user.tar",
              "no_override_list":"http:\/\/controlid.com.br\/idaccess\/ACFW\/V3.6.0\/userfs_no_override_list"
           },
           "update":{  
              "config_update":"http:\/\/controlid.com.br\/idaccess\/ACFW\/V3.6.0\/config_update.cid",
              "database_update":"http:\/\/controlid.com.br\/idaccess\/ACFW\/V3.6.0\/database_update.sql"
           },
           "kernel_standard":"http:\/\/controlid.com.br\/idaccess\/ACFW\/V3.6.0\/kernel_standard.bin",
           "kernel_recovery":"http:\/\/controlid.com.br\/idaccess\/ACFW\/V3.6.0\/kernel_recovery.bin",
           "boot":"http:\/\/controlid.com.br\/idaccess\/ACFW\/V3.6.0\/boot.bin",
           "acfw":"http:\/\/controlid.com.br\/idaccess\/ACFW_V3.6.0.zip",
           "version":"3.6.0"
        }
        */

        /* http://controlid.com.br/idaccess/acfw_update.php?SERIAL=J123&VERSION=3.6.0
        {  
           "configuration":{  
              "url":"http:\/\/controlid.com.br\/idaccess\/ACFW\/V3.6.0\/config.tar",
              "no_override_list":"http:\/\/controlid.com.br\/idaccess\/ACFW\/V3.6.0\/configuration_no_override_list"
           },
           "fs_root":{  
              "url":"http:\/\/controlid.com.br\/idaccess\/ACFW\/V3.6.0\/fs_root.tar",
              "no_override_list":"http:\/\/controlid.com.br\/idaccess\/ACFW\/V3.6.0\/rootfs_no_override_list",
              "patch":"http:\/\/controlid.com.br\/idaccess\/ACFW\/V3.6.0\/jalapeno\/fs_root.tar"
           },
           "fs_user":{  
              "url":"http:\/\/controlid.com.br\/idaccess\/ACFW\/V3.6.0\/fs_user.tar",
              "no_override_list":"http:\/\/controlid.com.br\/idaccess\/ACFW\/V3.6.0\/userfs_no_override_list",
              "patch":"http:\/\/controlid.com.br\/idaccess\/ACFW\/V3.6.0\/jalapeno\/fs_user.tar"
           },
           "update":{  
              "config_update":"http:\/\/controlid.com.br\/idaccess\/ACFW\/V3.6.0\/jalapeno\/config_update.cid",
              "database_update":"http:\/\/controlid.com.br\/idaccess\/ACFW\/V3.6.0\/database_update.sql"
           },
           "kernel_standard":"http:\/\/controlid.com.br\/idaccess\/ACFW\/V3.6.0\/jalapeno\/kernel_standard.bin",
           "kernel_recovery":"http:\/\/controlid.com.br\/idaccess\/ACFW\/V3.6.0\/jalapeno\/kernel_recovery.bin",
           "boot":"http:\/\/controlid.com.br\/idaccess\/ACFW\/V3.6.0\/jalapeno\/boot.bin",
           "acfw":"http:\/\/controlid.com.br\/idaccess\/ACFW_V3.6.0.zip",
           "version":"3.6.0"
        }
        */

        public FirmwareUpdate(string cURL, string cVersao, string cSerial)
        {
            if (!cURL.EndsWith("/"))
                cURL += "/";

            string cURL2 = cURL;

            if (cSerial.StartsWith("J") || cSerial.StartsWith("K") || cSerial.StartsWith("L"))
                cURL2 += "jalapeno/";

            configuration = new FirmwareUpdateFS(cURL + "config.tar", cURL + "configuration_no_override_list");
            fs_root = new FirmwareUpdateFS(cURL + "fs_root.tar", cURL + "rootfs_no_override_list");
            fs_user = new FirmwareUpdateFS(cURL + "fs_user.tar", cURL + "userfs_no_override_list");
            if(cURL!=cURL2) // jalapeno
            {
                fs_root.patch = cURL2 + "fs_root.tar";
                fs_user.patch = cURL2 + "fs_user.tar";
            }
            update = new FirmwareUpdateConfig(cURL2 + "config_update.cid", cURL + "database_update.sql");
            kernel_standard = cURL2 + "kernel_standard.bin";
            kernel_recovery = cURL2 + "kernel_recovery.bin";
            boot = cURL2 + "boot.bin";
            acfw = cURL + "ACFW_V" + cVersao + ".zip";
            version = cVersao;
        }
    }

    [DataContract]
    public class ConfigMonitor
    {
        [DataMember()]
        public string request_timeout;
        [DataMember()]
        public string hostname;
        [DataMember()]
        public string port;
        [DataMember()]
        public string path;
    }

    [DataContract]
    public class ConfigCatra
    {
        [DataMember(EmitDefaultValue =false)]
        public string anti_passback;

        /// <summary>
        /// Habilita ou desabilita o controle de anti-dupla entrada. (verdadeiro ou falso)
        /// </summary>
        [IgnoreDataMember]
        public bool AntiPassback
        {
            get
            {
                return anti_passback == "1";
            } 
            set
            {
                anti_passback = value ? "1" : "0";
            } 
        }

        [DataMember(EmitDefaultValue = false)]
        public string daily_reset;

        /// <summary>
        /// Habilita o resete de logs para o controle de anti-dupla entrada. Os acessos serão resetados todos os dias a meia-noite.
        /// </summary>
        [IgnoreDataMember]
        public bool DailyReset
        {
            get
            {
                return daily_reset == "1";
            }
            set
            {
                daily_reset = value ? "1" : "0";
            }
        }

        [DataMember(EmitDefaultValue = false)]
        public string gateway;
        /// <summary>
        /// sentido da entrada. deve ser "clockwise" ou "anticlockwise" (horário ou anti-horário respectivamente)
        /// </summary>
        [IgnoreDataMember]
        public iDBlockDirection GatewayMode
        {
            get
            {
                return (iDBlockDirection)Enum.Parse(typeof(iDBlockDirection), gateway, true);
            }
            set
            {
                gateway = value.ToString().ToLower();
            }
        }

        [DataMember(EmitDefaultValue = false)]
        public string operation_mode;
        /// <summary>
        /// Modo de operação da catraca. Controla quais sentidos da catraca serão controlados ou liberados. Deve ser "blocked", "entrance_open", "exit_open", "both_open". 
        /// (Ambas controladas, entrada liberada, saída liberada e ambas liberadas respectivamente).
        /// </summary>
        [IgnoreDataMember]
        public iDBlockOperationMode OperationMode
        {
            get
            {
                return (iDBlockOperationMode)Enum.Parse(typeof(iDBlockOperationMode), operation_mode, true);
            }
            set
            {
                operation_mode = value.ToString().ToLower();
            }
        }
    }

    [DataContract]
    public class OnlineClient
    {
        [DataMember(EmitDefaultValue = false)]
        string server_id;
        public long? ServerId
        {
            get { return ConfigValues.GetLongString(server_id); }
            set { server_id = ConfigValues.SetLongString(value); }
        }
        [DataMember(EmitDefaultValue = false)]
        string extract_template;
        public bool? ExtractTemplate
        {
            get { return ConfigValues.GetBoolString(extract_template); }
            set { extract_template = ConfigValues.SetBoolString(value); }
        }
    }

    [DataContract]
    public class General
    {
        [DataMember(EmitDefaultValue = false)]
        public string beep_enabled;

        [DataMember(EmitDefaultValue = false)]
        public string buttonhole1_enabled;

        [DataMember(EmitDefaultValue = false)]
        public string buttonhole2_enabled;

        [DataMember(EmitDefaultValue = false)]
        public string buttonhole1_idle;

        [DataMember(EmitDefaultValue = false)]
        public string buttonhole2_idle;

        [DataMember(EmitDefaultValue = false)]
        public string door_sensor1_enabled;

        [DataMember(EmitDefaultValue = false)]
        public string door_sensor2_enabled;

        [DataMember(EmitDefaultValue = false)]
        public string door_sensor3_enabled;

        [DataMember(EmitDefaultValue = false)]
        public string door_sensor4_enabled;

        [DataMember(EmitDefaultValue = false)]
        public string door_sensor1_idle;

        [DataMember(EmitDefaultValue = false)]
        public string door_sensor2_idle;

        [DataMember(EmitDefaultValue = false)]
        public string door_sensor3_idle;

        [DataMember(EmitDefaultValue = false)]
        public string door_sensor4_idle;

        [DataMember(EmitDefaultValue = false)]
        public string relay1_enabled;

        [DataMember(EmitDefaultValue = false)]
        public string relay2_enabled;

        [DataMember(EmitDefaultValue = false)]
        public string relay3_enabled;

        [DataMember(EmitDefaultValue = false)]
        public string relay4_enabled;

        [DataMember(EmitDefaultValue = false)]
        public string relay1_timeout;

        [DataMember(EmitDefaultValue = false)]
        public string relay2_timeout;

        [DataMember(EmitDefaultValue = false)]
        public string relay3_timeout;

        [DataMember(EmitDefaultValue = false)]
        public string relay4_timeout;

        [DataMember(EmitDefaultValue = false)]
        public string bell_enabled;

        [DataMember(EmitDefaultValue = false)]
        public string bell_relay;

        [DataMember(EmitDefaultValue = false)]
        string daylight_savings_time_start;
        public DateTime? Daylight_savings_time_start
        {
            get { return StringToDateTime(daylight_savings_time_start); }
            set { daylight_savings_time_start = DateTimeToString(value); }
        }
        [DataMember(EmitDefaultValue = false)]
        string daylight_savings_time_end;
        public DateTime? Daylight_savings_time_end
        {
            get { return StringToDateTime(daylight_savings_time_end); }
            set { daylight_savings_time_end = DateTimeToString(value); }
        }
        [DataMember(EmitDefaultValue = false)]
        string online;
        public bool? Online
        {
            get { return ConfigValues.GetBoolString(online); }
            set { online = ConfigValues.SetBoolString(value); }
        }
        [DataMember(EmitDefaultValue = false)]
        string local_identification;
        public bool? LocalIdentification
        {
            get { return ConfigValues.GetBoolString(local_identification); }
            set { local_identification = ConfigValues.SetBoolString(value); }
        }

        DateTime? StringToDateTime(string s)
        {
            if (s.Length == 8)
                return new DateTime(
                    int.Parse(s.Substring(4, 4)),
                    int.Parse(s.Substring(2, 2)),
                    int.Parse(s.Substring(0, 2)));
            return null;
        }

        string DateTimeToString(DateTime? dt)
        {
            return dt.Value.Day.ToString("00") + dt.Value.Month.ToString("00") + dt.Value.Year.ToString("0000");
        }
    }

    /// <summary>
    /// Valores dos parâmetros de configuração dos a serem setados nos equipamentos
    /// </summary>
    [DataContract]
    public class ConfigValues
    {
        [DataMember(EmitDefaultValue = false)]
        public General general;
        [DataMember(EmitDefaultValue = false)]
        public OnlineClient online_client;
        [DataMember(EmitDefaultValue = false)]
        public ConfigMonitor monitor;
        [DataMember(EmitDefaultValue = false)]
        public ConfigCatra catra;
        [DataMember(EmitDefaultValue = false)]
        public LedsColors led_rgb;

        public ConfigValues(bool lGeneral = false, bool lOnline = false)
        {
            if (lGeneral)
                general = new General();
            if (lOnline)
                online_client = new OnlineClient();
        }

        public ConfigValues(General oGeneral)
        {
            general = oGeneral;
        }

        public ConfigValues(OnlineClient oOnLine)
        {
            online_client = oOnLine;
        }

        public ConfigValues(ConfigMonitor oMonitor)
        {
            monitor = oMonitor;
        }

        public ConfigValues(ConfigCatra oCatra)
        {
            catra = oCatra;
        }

        public ConfigValues(LedsColors oled_rgb)
        {
            led_rgb = oled_rgb;
        }

        // TODO: Migrar para util
        /*** helper functions ***/
        internal static bool? GetBoolString(string s)
        {
            if (s == null)
                return null;
            return s == "0" ? false : true;
        }

        internal static string SetBoolString(bool? b)
        {
            if (b.HasValue)
                return b.Value ? "1" : "0";
            return null;
        }

        internal static long? GetLongString(string s)
        {
            if (s == null)
                return null;
            return long.Parse(s);
        }

        internal static string SetLongString(long? i)
        {
            if (i.HasValue)
                return i.Value.ToString();
            return null;
        }
    }

    // {"led_rgb": {"state":"3","solid_red":"0","solid_green":"0","solid_blue":"0","transition_start_red":"0","transition_start_green":"0","transition_start_blue":"0","transition_end_red":"65535","transition_end_green":"0","transition_end_blue":"0"}}
  
    [DataContract]
    public class LedsColors
    {
        /// <summary>
        /// Tipo de transição: 
        /// 1 - Desligado;
        /// 2 - Cor Fixa;
        /// 3 - Transição;
        /// </summary>
        [DataMember]
        public string state;
        [DataMember]
        public string solid_red;
        [DataMember]
        public string solid_green;
        [DataMember]
        public string solid_blue;

        [DataMember]
        public string transition_start_red;
        [DataMember]
        public string transition_start_green;
        [DataMember]
        public string transition_start_blue;

        [DataMember]
        public string transition_end_red;
        [DataMember]
        public string transition_end_green;
        [DataMember]
        public string transition_end_blue;
    }

    [DataContract]
    public class UpdateFirmware
    {
        [DataMember(EmitDefaultValue = true)]
        public string server_url;
        [DataMember]
        public string update_mode;
    }

}
