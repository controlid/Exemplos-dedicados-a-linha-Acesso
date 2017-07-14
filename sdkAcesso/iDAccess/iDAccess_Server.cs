using System;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace ControliD.iDAccess
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
    /// Serviços a serem implementados para criar servidor de acesso com identificação no servidor.
    /// Será enviado a imagem do dedo, e você terá que implementar ou usar um algoritimo biometrico para identificar o usuário, e depois validar sua regra de acesso.
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
        [WebInvoke(UriTemplate = "device_is_alive.fcgi?session={session}&device_id={device_id}", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        DeviceIsAliveResult IsAlive(string session, string device_id, IsAliveResult item);

        /// <summary>
        /// Recebe as imagens do retorno do Cadastro Remoto
        /// </summary>
        /// <param name="item"></param>
        [OperationContract]
        [WebInvoke(UriTemplate = "fingerprint_create.fcgi", Method = "POST", RequestFormat = WebMessageFormat.Json)]
        StatusResult TemplateCreate(NotificationTemplate item);

        /// <summary>
        /// Recebe os dados de cartão do cadastro Remoto
        /// </summary>
        /// <param name="item"></param>
        [OperationContract]
        [WebInvoke(UriTemplate = "card_create.fcgi", Method = "POST", RequestFormat = WebMessageFormat.Json)]
        void CardCreate(NotificationCard item);

        [OperationContract]
        [WebInvoke(UriTemplate = "catra_event", Method = "POST", RequestFormat = WebMessageFormat.Json)]
        void NotifyCatra(NotificationCatraEvents item);

        /// <summary>
        /// Receber eventos da DAO (quando ocorrem novos logs)
        /// </summary>
        [OperationContract]
        [WebInvoke(UriTemplate = "dao", Method = "POST", RequestFormat = WebMessageFormat.Json)]
        void NotifyDAO(NotificationItem item);

    }

    /// <summary>
    /// Serviços a serem implementados para criar servidor de acesso com identificação no equipamento do usuário.
    /// Será enviado o ID do usuário, e o seu servidor deverá validar a regra de acesso
    /// </summary>
    [ServiceContract]
    public interface IUserLocal
    {
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
        [WebInvoke(UriTemplate = "device_is_alive.fcgi?session={session}&device_id={device_id}", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        DeviceIsAliveResult IsAlive(string session, string device_id, IsAliveResult item);

        /// <summary>
        /// Checa se existem cartão sendo cadastrado
        /// </summary>
        [OperationContract]
        [WebInvoke(UriTemplate = "card_create.fcgi", Method = "POST", RequestFormat = WebMessageFormat.Json)]
        void CardCreate(NotificationCard item);

        /// <summary>
        /// Checa se houve desistencia da catraca
        /// </summary>
        [OperationContract]
        [WebInvoke(UriTemplate = "catra_event", Method = "POST", RequestFormat = WebMessageFormat.Json)]
        void NotifyCatra(NotificationCatraEvents item);

        /// <summary>
        /// Receber eventos da DAO (quando ocorrem novos logs)
        /// </summary>
        [OperationContract]
        [WebInvoke(UriTemplate = "dao", Method = "POST", RequestFormat = WebMessageFormat.Json)]
        void NotifyDAO(NotificationItem item);


        /// <summary>
        /// cria biometria cadastrada remotamente
        /// </summary>
        [OperationContract]
        [WebInvoke(UriTemplate = "template_create.fcgi?session={session}&device_id={device_id}", Method = "POST", RequestFormat = WebMessageFormat.Json)]
        void TemplateCreate(string session, string device_id, Stream stream);
    }

    /// <summary>
    /// A extração biometrica é feita no rerminal e é enviado o tempalte, não o ID do usuário.
    /// O Find e Regras deverão ser feitos pelo seu servidor
    /// </summary>
    [ServiceContract]
    [Obsolete("Será descontinuado em breve")]
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
        [WebInvoke(UriTemplate = "device_is_alive.fcgi", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        DeviceIsAliveResult IsAlive(Stream stream);
    }

    /// <summary>
    /// Estrutura de notificações
    /// </summary>
    [ServiceContract]
    public interface INotification
    {
        /// <summary>
        /// Receber eventos da DAO (quando ocorrem novos logs)
        /// </summary>
        [OperationContract]
        [WebInvoke(UriTemplate = "dao", Method = "POST", RequestFormat = WebMessageFormat.Json)]
        void NotifyDAO(NotificationItem item);

        /// <summary>
        /// Quando chega um template apos ser solicitado um cadastramento via remoteEnroll/{id}/bio/{message}
        /// </summary>
        [OperationContract]
        [WebInvoke(UriTemplate = "template", Method = "POST", RequestFormat = WebMessageFormat.Json)]
        StatusResult NotifyTemplate(NotificationTemplate item);

        /// <summary>
        /// Quando chega um cartão apos ser solicitado um cadastramento via remoteEnroll/{id}/card/{message}
        /// </summary>
        [OperationContract]
        [WebInvoke(UriTemplate = "card", Method = "POST", RequestFormat = WebMessageFormat.Json)]
        StatusResult NotifyCard(NotificationCard item);

        [OperationContract]
        [WebInvoke(UriTemplate = "catra_event", Method = "POST", RequestFormat = WebMessageFormat.Json)]
        void NotifyCatra(NotificationCatraEvents item);
    }
}