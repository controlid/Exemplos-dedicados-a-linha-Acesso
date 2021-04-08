using System.Runtime.Serialization;

namespace ControliD.iDAccess
{
    // https://www.controlid.com.br/suporte/api_idaccess_V2.6.8.html#50_gpio_state
    public partial class Device
    {
        /// <summary>
        /// Parâmetros de configuração desejados do equipamento
        /// </summary>
        [DataContract]
        public class GpioState
        {
            [DataMember]
            string pin;
            public string Pin { get { return pin; } }
            [DataMember]
            int enabled;
            public bool Enabled { get { return enabled == 1; } }
            [DataMember]
            int value;
            public bool Value { get { return value == 1; } }
            [DataMember]
            int idle;
            public bool Idle { get { return idle == 1; } }
            [DataMember]
            int @in;
            public bool In { get { return @in == 1; } }
            [DataMember]
            int pullup;
            public bool Pullup { get { return pullup == 1; } }
            [DataMember]
            int notify;
            public bool Notify { get { return notify == 1; } }
            [DataMember]
            int notified;
            public int Notified { get { return notified; } }
        }

        [DataContract]
        class GpioObject
        {
            [DataMember]
            public int gpio;
        }
        public GpioState GetGpio(int gpio)
        {
            GpioObject o = new GpioObject();
            o.gpio = gpio;

            CheckSession();
            return WebJson.JsonCommand<GpioState>(URL + "gpio_state.fcgi?session=" + Session, o, null, TimeOut);
        }

        [DataContract]
        public class DoorStateDetail
        {
            [DataMember]
            public int id;
            [DataMember]
            public bool open;
        }
        [DataContract]
        public class DoorState
        {
            [DataMember]
            public DoorStateDetail[] doors;
            [DataMember]
            public DoorStateDetail[] sec_boxes;
        }
        public DoorState GetDoorState()
        {
            CheckSession();
            return WebJson.JsonCommand<DoorState>(URL + "doors_state.fcgi?session=" + Session, null, null, TimeOut);
        }
    }
}
