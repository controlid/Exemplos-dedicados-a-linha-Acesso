using System;
using System.Collections.Generic;
using System.Drawing;

namespace ControliD.iDAccess
{
    public partial class Device
    {
        public Exception Try(Action a)
        {
            LastError = null;
            try
            {
                a.Invoke();
                return null;
            }
            catch (Exception ex)
            {
                return LastError = ex;
            }
        }

        public T Command<T>(string cmd, object req)
        {
            CheckSession();
            return WebJson.JsonCommand<T>(URL + cmd + ".fcgi?session=" + Session, req, null, TimeOut);
        }

        public string Send(string cmd, string data)
        {
            CheckSession();
            return WebJson.JsonCommand<string>(URL + cmd + ".fcgi?session=" + Session, data, null, TimeOut);
        }

        public long Count<T>(WhereObjects oWhere = null)
        {
            CheckSession();
            ObjectResult result = Command<ObjectResult>("load_objects", new ObjectRequest<T, WhereObjects>() { fields = new string[] { "COUNT(*)" }, where = oWhere });
            Type tpT = typeof(T);
            Type tpR = result.GetType();
            object[] itens = (object[])tpR.GetField(tpT.Name.ToLower()).GetValue(result);
            return ((GenericCount)itens[0]).Count;
        }

        //[Obsolete("Use Door()")]
        //public void Action(int nPort = 1)
        //{
        //    Door(nPort);
        //}

        public void ActionDoor(int nPort = 1)
        {
            Action("door", "door=" + nPort);
        }

        // {"actions":[{"action":"sec_box","parameters":"id=65793, reason=3"}]}
        public void ActionSecBox(int id = 65793)
        {
            Action("sec_box", "id=" + id);
        }

        public void ActionSpin(iDBlockDirection direction)
        {
            Action("catra", "allow=" + direction.ToString().ToLower());
        }

        public void Action(string cAction, string cParameter)
        {
            Command<string>("execute_actions", new ActionsRequest()
            {
                actions = new ActionItem[] { new ActionItem() {
                    action = cAction ,
                    parameters = cParameter} }
            });
        }

        public void Buzzer(int nFrequence = 4000, int nTime = 250, int nVolume = 50)
        {
            Command<string>("buzzer_buzz", new BuzzerRequest()
            {
                frequency = nFrequence,
                timeout = nTime,
                duty_cycle = nVolume
            });
        }

        public void RemoteEnroll(RemoteEnrollType eType, string cMessage = null, int user=0, bool panic = false, string reg = null)
        {

            Command<string>("remote_enroll", new RemoteEnrollRequest()
            {
                type = eType.ToString().ToLower(),
                message = cMessage,
                user_id = user,
                save = user > 1,
                panic_finger = panic ? 1 : 0,
                registration = reg
            });
        }

        public InformationResult SystemInformation()
        {
            CheckSession();
            return WebJson.JsonCommand<InformationResult>(URL + "system_information.fcgi?session=" + Session, null, null, TimeOut);
        }

        public string SetSystemTime(DateTime dt)
        {
            CheckSession();
            return WebJson.JsonCommand<string>(URL + "set_system_time.fcgi?session=" + Session, new SystemTime(dt),null, TimeOut);
        }

        public string DeleteAdmin()
        {
            CheckSession();
            return WebJson.JsonCommand<string>(URL + "delete_admins.fcgi?session=" + Session, null, null, TimeOut);
        }

        public string Reboot()
        {
            CheckSession();
            return WebJson.JsonCommand<string>(URL + "reboot.fcgi?session=" + Session, null, null, TimeOut);
        }

        public TemplateResult TemplateExtract(Bitmap digital)
        {
            return TemplateExtract(digital.Width, digital.Height, Util.GetBytesRAWG(digital));
        }

        public TemplateResult TemplateExtract(int Width, int Height, byte[] btRequest)
        {
            CheckSession();
            return WebJson.JsonCommand<TemplateResult>(URL + "template_extract.fcgi?session=" + Session + "&width=" + Width + "&height=" + Height, btRequest, null, TimeOut);
        }

        public TemplateResult TemplateMatch(string cTemplate1, string cTemplate2, string cTemplate3)
        {
            List<byte> btRequest = new List<byte>();
            byte[] bt1 = Convert.FromBase64String(cTemplate1);
            btRequest.AddRange(bt1);
            byte[] bt2 = Convert.FromBase64String(cTemplate2);
            btRequest.AddRange(bt2);
            byte[] bt3 = Convert.FromBase64String(cTemplate3);
            btRequest.AddRange(bt3);
            return TemplateMatch(bt1.Length, bt2.Length, bt3.Length, btRequest.ToArray());
        }

        /// <summary>
        /// Verifica se um template existe no equipamento
        /// </summary>
        public TemplateResult TemplateMatch(string cTemplate)
        {
            byte[] bt = Convert.FromBase64String(cTemplate);
            return TemplateMatch(bt);
        }

        /// <summary>
        /// procurar alguem
        /// </summary>
        public TemplateResult TemplateMatch(int size1, int size2, int size3, byte[] btRequest)
        {
            CheckSession();
            return WebJson.JsonCommand<TemplateResult>(URL + "template_match.fcgi?session=" + Session + "&size0=" + size1 + "&size1=" + size2 + "&size2=" + size3, btRequest, null, TimeOut);
        }

        public TemplateResult TemplateMatch(byte[] btRequest)
        {
            CheckSession();
            return WebJson.JsonCommand<TemplateResult>(URL + "template_match.fcgi?session=" + Session + "&size0=" + btRequest.Length + "&temp_num=1", btRequest, null, TimeOut);
        }

        public TemplateResult TemplateCreate(long nUserID, string cTemplate1, string cTemplate2, string cTemplate3, int nFingerType = 0)
        {
            List<byte> btRequest = new List<byte>();
            byte[] bt1 = Convert.FromBase64String(cTemplate1);
            btRequest.AddRange(bt1);
            byte[] bt2 = Convert.FromBase64String(cTemplate2);
            btRequest.AddRange(bt2);
            byte[] bt3 = Convert.FromBase64String(cTemplate3);
            btRequest.AddRange(bt3);
            return TemplateCreate(nUserID, bt1.Length, bt2.Length, bt3.Length, btRequest.ToArray(), nFingerType);
        }

        public void SetNetwork(string host, int port, bool ssl, string netmask, string gateway)
        {
            Command<string>("set_system_network", "{\"ip\":\"" + host + "\",\"netmask\":\"" + netmask + "\",\"gateway\":\"" + gateway + "\",\"web_server_port\":" + port + ",\"ssl_enabled\":" + (ssl ? "true" : "false") + "}");
        }

        public TemplateResult TemplateCreate(long nUserID, int size1, int size2, int size3, byte[] btRequest, int nFingerType = 0)
        {
            CheckSession();
            return WebJson.JsonCommand<TemplateResult>(URL + "template_create.fcgi?session=" + Session + "&user_id=" + nUserID + "&size0=" + size1 + "&size1=" + size2 + "&size2=" + size3 + "&finger_type=" + nFingerType, btRequest, null, TimeOut);
        }

        public TemplateResult TemplateCreate(long nUserID, Bitmap bitmap1, Bitmap bitmap2, Bitmap bitmap3, int nFingerType = 0)
        {
            TemplateResult tr1 = TemplateExtract(bitmap1);
            TemplateResult tr2 = TemplateExtract(bitmap2);
            TemplateResult tr3 = TemplateExtract(bitmap3);
            if (tr1.quality > 50 && tr2.quality > 50 && tr3.quality > 50)
                return TemplateCreate(nUserID, tr1.template, tr2.template, tr3.template, nFingerType);
            else
                return new TemplateResult() { Codigo = 1, Status = "low quality" };
        }
    }
}