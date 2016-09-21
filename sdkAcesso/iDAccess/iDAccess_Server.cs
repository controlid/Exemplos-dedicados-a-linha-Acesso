using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace ControlID.iDAccess
{
    public partial class Device
    {

        /// <summary>
        /// Altera nome do dispositivo
        /// </summary>
        /// <param name="sName">Nome a ser exibido na tela</param>
        public void ChangeDeviceName(string sName)
        {
            long devId = SystemInformation().deviceId;
            Devices dev = Load<Devices>(devId);
            if (dev != null)
            {
                dev.name = sName;
                Modify(devId, dev);
            }
        }

        /// <summary>
        /// Configura o equipamento como cliente de um servidor
        /// </summary>
        /// <param name="server_url">IP ou URL completa do servidor</param>
        /// <param name="mode">Modo de operação online</param>
        /// <param name="Overwrite">Apaga server antigo</param>
        public StatusResult ConfigureServer(string server_url, OnlineMode mode, bool Overwrite = false)
        {
            const long serverId = -1;
            Devices server = new Devices() { id = serverId, IP = server_url, name = "Server", PublicKey = "anA=" };

            if (Overwrite)
                Destroy<Devices>(serverId);

            if (Add(server) == serverId)
                return GoOnline(mode, serverId);
            else
                return new StatusResult(500, "Não foi possível incluir o servidor no equipamento");
        }
    }

    /// <summary>
    /// Serviços a a serem implementados para criar servidor de acesso com identificação no servidor
    /// </summary>
    [ServiceContract]
    public interface IServerUser
    {
        /// <summary>
        /// Usuário identificado por biometria
        /// </summary>
        [OperationContract]
        [WebInvoke(UriTemplate = "new_biometric_image.fcgi?session={session}&device_id={device_id}&identifier_id={identifier_id}&width={width}&height={height}", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        IdentifyResult UserImage(string session, string device_id, string identifier_id, string width, string height, Stream stream);

        ///// <summary>
        ///// Um template não identificado foi extraido
        ///// </summary>
        //[OperationContract]
        //[WebInvoke(UriTemplate = "new_biometric_template.fcgi?session={session}&device_id={device_id}&identifier_id={identifier_id}", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        //IdentifyResult UserTemplate(string session, string device_id, string identifier_id, Stream stream);

        /// <summary>
        /// Usuário identificado por código e senha
        /// </summary>
        [OperationContract]
        [WebInvoke(UriTemplate = "new_user_id_and_password.fcgi?session={session}", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        IdentifyResult UserCodePassword(string session, Stream stream);

        /// <summary>
        /// Usuário identificado por cartão de proximidade
        /// </summary>
        [OperationContract]
        [WebInvoke(UriTemplate = "new_card.fcgi?session={session}", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        IdentifyResult UserCard(string session, Stream stream);

        /// <summary>
        /// Equipamento solicitando imagem do usuário
        /// </summary>
        [OperationContract]
        [WebInvoke(UriTemplate = "user_get_image.fcgi?session={session}&user_id={user_id}", Method = "GET")]
        Stream GetImage(string session, string user_id);

        /// <summary>
        /// Responde que o servidor está vivo
        /// </summary>
        [OperationContract]
        [WebInvoke(UriTemplate = "device_is_alive.fcgi?session={session}", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        DeviceIsAliveResult IsAlive(string session, Stream stream);
    }

    /// <summary>
    /// Serviços a a serem implementados para criar servidor de acesso com identificação no equipamento
    /// </summary>
    [ServiceContract]
    public interface IUserLocal
    {
        // Descontinuado!
        ///// <summary>
        ///// Registra template pelo equipamento
        ///// </summary>
        //[OperationContract]
        //[WebInvoke(UriTemplate = "register_template.fcgi?userid={user_id}&registration={registration}", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        //StatusResult RegisterTemplate(long user_id, string registration, Stream stream);

        /// <summary>
        /// Usuário identificado pelo template local
        /// </summary>
        [OperationContract]
        [WebInvoke(UriTemplate = "new_user_identified.fcgi?session={session}", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        IdentifyResult UserIdentified(string session, Stream stream);

        /// <summary>
        /// Equipamento solicitando imagem do usuário
        /// </summary>
        [OperationContract]
        [WebInvoke(UriTemplate = "user_get_image.fcgi?session={session}&user_id={user_id}", Method = "GET")]
        Stream GetImage(string session, string user_id);

        /// <summary>
        /// Usuário identificado por código e senha
        /// </summary>
        [OperationContract]
        [WebInvoke(UriTemplate = "new_user_id_and_password.fcgi?session={session}", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        IdentifyResult UserCodePassword(string session, Stream stream);

        /// <summary>
        /// Usuário identificado por cartão de proximidade
        /// </summary>
        [OperationContract]
        [WebInvoke(UriTemplate = "new_card.fcgi?session={session}", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        IdentifyResult UserCard(string session, Stream stream);

        /// <summary>
        /// Responde que o servidor está vivo
        /// </summary>
        [OperationContract]
        [WebInvoke(UriTemplate = "device_is_alive.fcgi?session={session}", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        DeviceIsAliveResult IsAlive(string session, Stream stream);
    }

    /// <summary>
    /// Extract Template - uso futuro?
    /// </summary>
    [ServiceContract]
    public interface IUserTemplate
    {
        /// <summary>
        /// Um template não identificado foi extraido
        /// </summary>
        [OperationContract]
        [WebInvoke(UriTemplate = "new_biometric_template.fcgi?session={session}&device_id={device_id}&identifier_id={identifier_id}", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        IdentifyResult UserTemplate(string session, string device_id, string identifier_id, Stream stream);

        /// <summary>
        /// Equipamento solicitando imagem do usuário
        /// </summary>
        [OperationContract]
        [WebInvoke(UriTemplate = "user_get_image.fcgi?session={session}&user_id={user_id}", Method = "GET")]
        Stream GetImage(string session, string user_id);

        /// <summary>
        /// Usuário identificado por código e senha
        /// </summary>
        [OperationContract]
        [WebInvoke(UriTemplate = "new_user_id_and_password.fcgi?session={session}", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        IdentifyResult UserCodePassword(string session, Stream stream);

        /// <summary>
        /// Usuário identificado por cartão de proximidade
        /// </summary>
        [OperationContract]
        [WebInvoke(UriTemplate = "new_card.fcgi?session={session}", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        IdentifyResult UserCard(string session, Stream stream);

        /// <summary>
        /// Responde que o servidor está vivo
        /// </summary>
        [OperationContract]
        [WebInvoke(UriTemplate = "device_is_alive.fcgi?session={session}", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        DeviceIsAliveResult IsAlive(string session, Stream stream);
    }

    [ServiceContract]
    public interface INotification
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "dao", Method = "POST", RequestFormat = WebMessageFormat.Json)]
        void NotifyDAO(NotificationItem item);

        [OperationContract]
        [WebInvoke(UriTemplate = "template", Method = "POST", RequestFormat = WebMessageFormat.Json)]
        void NotifyTemplate(NotificationTemplate item);

        [OperationContract]
        [WebInvoke(UriTemplate = "card", Method = "POST", RequestFormat = WebMessageFormat.Json)]
        void NotifyCard(NotificationCard item);
    }
}
