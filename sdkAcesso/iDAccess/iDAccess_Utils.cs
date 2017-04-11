using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ControliD.iDAccess
{
    // http://www.controlid.com.br/suporte/api_idaccess_V2.2-1.html#50_introduction
    // { login: 'admin', password: 'admin'}
    [DataContract]
    public class LoginRequest
    {
        [DataMember]
        public string login;
        [DataMember]
        public string password;
    }

    [DataContract]
    public class LoginResult
    {
        [DataMember]
        public string session;
        [DataMember]
        public string error;
    }

    // http://www.controlid.com.br/suporte/api_idaccess_V2.2-1.html#50_execute_actions
    // execute_actions.fcgi?session=" + session
    // actions: [ { action: "door", parameters: "door=1" } ]
    [DataContract()]
    public class ActionsRequest
    {
        [DataMember()]
        public ActionItem[] actions; // array de actionItem
    }

    [DataContract]
    public class ActionItem
    {
        [DataMember]
        public string action;
        [DataMember]
        public string parameters;
    }

    // data: {frequency: 4000, duty_cycle: 50, timeout: 100},
    [DataContract]
    public class BuzzerRequest
    {
        [DataMember]
        public int frequency;
        [DataMember]
        public int duty_cycle; // Volume (maximo 50)
        [DataMember]
        public int timeout; // Tempo
    }

    [DataContract]
    public class SpinRequest
    {
        [DataMember]
        public string allow;
    }

    [DataContract]
    public class PasswordResult
    {
        [DataMember]
        public string password;
        [DataMember]
        public string salt;
    }

    [DataContract]
    public class RemoteEnrollRequest
    {
        [DataMember]
        public string type; //: <string; biometry, card>
        [DataMember(EmitDefaultValue =false)]
        public string message;
        [DataMember(EmitDefaultValue = false)]
        public long user_id;
        [DataMember(EmitDefaultValue = false)]
        public bool save;
        [DataMember(EmitDefaultValue = false)]
        public int panic_finger;
        [DataMember(EmitDefaultValue = false)]
        public string registration;
    }

    public enum RemoteEnrollType
    {
        Biometry,
        Card
    }

    //{"uptime":{"days":0,"hours":9,"minutes":14,"seconds":5},
    // "time":1430403259,
    // "memory":{"disk":{"free":33853440,"total":33976320},"ram":{"free":12103680,"total":61280256}},
    // "license":{"users":1000,"device":2,"type":0},
    // "network":{"mac":"FC:52:CE:10:04:5D","ip":"192.168.0.104","netmask":"255.255.254.0","gateway":"192.168.0.1","web_server_port":80,"ssl_enabled":false},
    // "serial":"A0V1",
    // "version":"2.6.0",
    // "online":true}
    [DataContract]
    public class InformationResult
    {
        [DataMember()]
        public TimeTotal uptime;
        /// <summary>
        /// Contém a data e hora do equipamento em Unix Timestamp.
        /// </summary>
        /// <see cref="http://pt.pedia.org/wiki/Era_Unix"/>
        [DataMember()]
        public long time;
        [DataMember()]
        public MemoryInfo memory;
        [DataMember()]
        public License license;
        [DataMember()]
        public NetworkConfig network;
        [DataMember()]
        public string serial;
        [DataMember()]
        public string version;
        [DataMember()]
        public bool online;
        public long deviceId {
            get { return Util.DeviceIDbySerial(serial); }
        }

        public DeviceNames DeviceName { get { return Device.GetDeviceName(serial); } }
        public DeviceModels DeviceModelType { get { return Device.GetDeviceModel(serial); } }
        public string DeviceModelDescription { get { return Device.GetDeviceNameDescription(DeviceName); } }

        public DateTime CurrentTime { get { return time.FromUnix(); } }

        public TimeSpan UpTimeSpan
        {
            get
            {
                return new TimeSpan(uptime.days, uptime.hours, uptime.minutes, uptime.seconds);
            }
        }
    }

    [DataContract]
    public class TimeTotal
    {
        [DataMember()]
        public int days;
        [DataMember()]
        public int hours;
        [DataMember()]
        public int minutes;
        [DataMember()]
        public int seconds;
    }

    // {"day":17,"month":6,"year":2015,"hour":17,"minute":25,"second":43}
    // set_system_time
    [DataContract]
    public class SystemTime
    {
        [DataMember()]
        public int day;
        [DataMember()]
        public int month;
        [DataMember()]
        public int year;
        [DataMember()]
        public int hour;
        [DataMember()]
        public int minute;
        [DataMember()]
        public int second;

        public SystemTime()
        {
        }

        public SystemTime(DateTime dt)
        {
            Current = dt;
        }

        public DateTime Current
        {
            set
            {
                year = value.Year;
                month=value.Month;
                day = value.Day ;
                hour = value.Hour;
                minute = value.Minute;
                second= value.Second;
            }
            get
            {
                return new DateTime(year, month, day, hour, minute, second);
            }
        }
    }

    [DataContract]
    public class License
    {
        [DataMember()]
        public int users;
        [DataMember()]
        public int device;
        [DataMember()]
        public string type;

        public override string ToString()
        {
            return string.Format("{0} equipamentos, {1} usuários, {2}", device, users, type);
        }
    }

    [DataContract]
    public class MemoryInfo
    {
        [DataMember()]
        public MemoryStatus disk;
        [DataMember()]
        public MemoryStatus ram;

        public override string ToString()
        {
            return "Disco " + disk.ToString() + " RAM " + ram.ToString();
        }
    }

    [DataContract]
    public class MemoryStatus
    {
        [DataMember()]
        public long free;
        [DataMember()]
        public long total;

        public override string ToString()
        {
            double mega = 1024 * 1024;
            return string.Format("{0:0.0}/{1:0.0} MB ({2:P0})", (total - free) / mega, total / mega, 1 - (free / (double)total));
        }
    }

    // // "network":{"mac":"FC:52:CE:10:04:5D","ip":"192.168.0.104","netmask":"255.255.254.0","gateway":"192.168.0.1","web_server_port":80,"ssl_enabled":false},
    [DataContract]
    public class NetworkConfig
    {
        [DataMember()]
        public string mac;
        [DataMember()]
        public string ip;
        [DataMember()]
        public string netmask;
        [DataMember()]
        public string gateway;
        [DataMember()]
        public int web_server_port;
        [DataMember()]
        public bool ssl_enabled;

        public override string ToString()
        {
            return MakeLink(ssl_enabled, ip, web_server_port);
            //return string.Format("{0}://{1}{2}", ssl_enabled ? "https" : "http", ip,
            //    ssl_enabled ? (web_server_port == 443 ? "" : (":" + web_server_port))
            //                : (web_server_port == 80 ? "" : (":" + web_server_port)));
        }

        public static string MakeLink(bool lSSL, string cIP, int nPort)
        {
            return string.Format("{0}://{1}{2}", lSSL ? "https" : "http", cIP,
                lSSL ? (nPort == 443 ? "" : (":" + nPort))
                            : (nPort == 80 ? "" : (":" + nPort)));
        }
    }

    // {"general":{"daylight_savings_time_start":"25062015","daylight_savings_time_end":"26062015"}}
    // set_configuration

    // {"quality":75,
    // "template":"SUNSUzIxAAAE7gEBAAAAAMUAxQBAAeABAAAAhMAplgB1APEPfACXAFYP2wCXABgPzACzAB8PMAGzAB4PrgDAAP8P2QDDAJUPIAHDACQPtgDWAF0PeADdAFAPwwDpACYOpQDyAOIPngDzAE8PuQD4AE8OxwAAATQOlAAUAdAP3gAYAToP6AAYATMP3gAyAUIPYQA3AdIP2ABGAUcMEgFVAawPvwBaAdQO2ABdAT4LewBiAdAPxgBxAfMOCgFzAaIP9wB1AZkOqgB3AdkPkwB+AdUP0gB+AQQOvgCDAfoOLgGGAZgP0gCMAQwPdwCTAeAPIgGaAZEPkQCcAeEPEAGiAYgPbgC0Ae8PqwC1AfYPbQDFAfMP6p+j28f3BmdLW28Lvviqh8sbhY02CNIgxf7fi/MLtaZy4IZvkXlaOm6bPQrfk/cPTTrVYnp8RgNDcR9TtdkBypX2GJaplZa8LG45A36BSRwJKb1t4eQFEm78foGG8peASAuJ++oIzPud8+oCif0BCw0T2wWXB9f/BQV5Cp50IgwyFwMW4eKxmpKMBfrJbqFP1v338F8BsfHh+UEiWQmq+PIO5fleloZeveIdBqHrof7u9BL1vfgdEfUNWQl9+ZnyQQp++koSNQgFFiYYrQPu8YYNYQnF+pvwLQNiDMLt6fna8rfo1fxeEXoTKhVb/WvuUQRXFW8WSlQAnCkAIDsBAsgqWg0Apzl0wJTCWMFvBABtPFD7wQYAhTxrwsBtDgC7QYDCkcDAwHJqBgD5Woxiwg0Ar0x6mcFke8APAJpy8P/9OEDBTMAXAR9/mnJ+gMPCXMB7UxkBG4SXZ8CFw8PBw1llXcFqEgB/lNz/KjA2RE4NAN+XFz44wWT+CwB3mlpr/sFbwgwAfJ1Q/ltW/4sTAH6l3MD/Kv04//7AwMA3EQDPsRb+/j5kZMH/ewgAx7OAxcLBwXYEALG/9Pz7BAEjxiRKEADcxyT/N8DAwMDAcHAMAHfiU1tkWMMeATzYomRkjMHAn8HAwcDD/sFY/8B+EgC/53HExMDDN/7C/f89cwwAx+gg//1UwMDBhw0Axe0pwP7+M/zD/ML/FACo7uDA9vsr/sP+/8DCwmrA/xIAu+5ciETB/sBAQsEOAMDuRsH8eDf+wcD9CwCG9VDAwMDA/W3/DwCa9lDBU1bA/EPCEgC5/FN4wEP/QUeVIAE89qnBR4P+xMP/wcPDwsHBeFZUwcDC/woQxQM3wvz8Uj8IEMoDLf/8//xgChDAD0lYP1sHEJoTU21XEBDFFkAxXf//wCmDChDpGjH/cXbDBRDfHDpcIhE+JrOL/8H9wsDBhMHCp8JpwVpUwcBvHxBjNNBlc/82/f/7//wx/1LAwFhyBhDeOUDBwMjAJBE9OKv/eP5dwMJ1wsPDxP+UwsJKUVdkHxEPUqtEZGulxsLCwcL/wf/BVP7/VQgQ11ZewEpcAxDuXiDAJBBBd+RvdGTARyz9+/3+/8D/wMDAwMDBcW8DEQ53GsEGEPt5E8D/dwkQp3tewTVnCRC+f+T5K/9WCBDChQDA/jf/BxC6hmnCOMAFENmNEGoREQykgDPAxZ3AQMJ2ChCusvczwHb+DxElsozBw/43n0zADxEdw4nCW/6Liy8OERLNg2v9wX+H/wsRFtd6/ldxeERCAQEAAAAWAAAAAAIFAAAAAAAARUI="}
    [DataContract]
    public class TemplateResult : StatusResult
    {
        [DataMember()]
        public int quality;
        [DataMember()]
        public string template;
    }

    /// <summary>
    /// Resultado de requisicao de cartao RFID do leitor
    /// </summary>
    [DataContract]
    public class CardResult : StatusResult
    {
        /// <summary>
        /// Numero do cartao
        /// </summary>
        [DataMember()]
        public string cardnumber;
    }
}