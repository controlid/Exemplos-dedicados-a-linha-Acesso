using System;
using System.Threading;
using System.Threading.Tasks;

namespace ControliD.iDAccess
{
    // Camada assíncrona (XxxAsync) dos comandos, configurações, usuários e hardware.
    // Cada método espelha o equivalente síncrono (mesmos endpoints, corpos, timeouts e estado).
    public partial class Device
    {
        #region Comandos (Device_Commands.cs)

        /// <summary>
        /// Versão assíncrona de <see cref="ActionDoor"/>
        /// </summary>
        public Task ActionDoorAsync(int nPort = 1, CancellationToken ct = default(CancellationToken))
        {
            return ActionAsync("door", "door=" + nPort, ct);
        }

        /// <summary>
        /// Versão assíncrona de <see cref="Action(string, string)"/>
        /// </summary>
        public async Task ActionAsync(string cAction, string cParameter, CancellationToken ct = default(CancellationToken))
        {
            await CommandAsync<string>("execute_actions", new ActionsRequest()
            {
                actions = new ActionItem[] { new ActionItem() {
                    action = cAction ,
                    parameters = cParameter} }
            }, null, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Versão assíncrona de <see cref="Buzzer"/>
        /// </summary>
        public async Task BuzzerAsync(int nFrequence = 4000, int nTime = 250, int nVolume = 50, CancellationToken ct = default(CancellationToken))
        {
            await CommandAsync<string>("buzzer_buzz", new BuzzerRequest()
            {
                frequency = nFrequence,
                timeout = nTime,
                duty_cycle = nVolume
            }, null, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Versão assíncrona de <see cref="IsAlive"/>: false para qualquer erro (exceto o cancelamento pelo chamador)
        /// </summary>
        public async Task<bool> IsAliveAsync(CancellationToken ct = default(CancellationToken))
        {
            try
            {
                var result = await CommandAsync<StatusResult>("device_is_alive", null, null, ct).ConfigureAwait(false);
                return true;
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Versão assíncrona de <see cref="SystemInformation"/>
        /// </summary>
        public async Task<InformationResult> SystemInformationAsync(CancellationToken ct = default(CancellationToken))
        {
            await CheckSessionAsync(ct).ConfigureAwait(false);
            return await JsonAsync<InformationResult>(URL + "system_information.fcgi?session=" + Session, null, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Versão assíncrona de <see cref="SetSystemTime"/>
        /// </summary>
        public async Task<string> SetSystemTimeAsync(DateTime dt, CancellationToken ct = default(CancellationToken))
        {
            var jsonObject = new DateTimeConfig()
            {
                day = dt.Day,
                month = dt.Month,
                year = dt.Year,
                hour = dt.Hour,
                minute = dt.Minute,
                second = dt.Second,
            };
            await CheckSessionAsync(ct).ConfigureAwait(false);
            return await JsonAsync<string>(URL + "set_system_time.fcgi?session=" + Session, jsonObject, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Versão assíncrona de <see cref="TemplateExtract(int, int, byte[])"/>
        /// </summary>
        public async Task<TemplateResult> TemplateExtractAsync(int Width, int Height, byte[] btRequest, CancellationToken ct = default(CancellationToken))
        {
            await CheckSessionAsync(ct).ConfigureAwait(false);
            return await JsonAsync<TemplateResult>(URL + "template_extract.fcgi?session=" + Session + "&width=" + Width + "&height=" + Height, btRequest, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Versão assíncrona de <see cref="TemplateCreate(long, int, int, int, byte[], int)"/>
        /// </summary>
        public async Task<TemplateResult> TemplateCreateAsync(long nUserID, int size1, int size2, int size3, byte[] btRequest, int nFingerType = 0, CancellationToken ct = default(CancellationToken))
        {
            await CheckSessionAsync(ct).ConfigureAwait(false);
            return await JsonAsync<TemplateResult>(URL + "template_create.fcgi?session=" + Session + "&user_id=" + nUserID + "&size0=" + size1 + "&size1=" + size2 + "&size2=" + size3 + "&finger_type=" + nFingerType, btRequest, ct).ConfigureAwait(false);
        }

        #endregion

        #region Configurações (Device_Configuration.cs)

        /// <summary>
        /// Versão assíncrona de <see cref="GetDaylightSavingTime"/>
        /// </summary>
        public Task<ConfigValues> GetDaylightSavingTimeAsync(CancellationToken ct = default(CancellationToken))
        {
            ConfigKeys config = new ConfigKeys();
            config.AskDayLightSavingTime();

            return GetConfigurationAsync(config, ct);
        }

        /// <summary>
        /// Versão assíncrona de <see cref="GetMultiFactorIdentification"/>
        /// </summary>
        public Task<ConfigValues> GetMultiFactorIdentificationAsync(CancellationToken ct = default(CancellationToken))
        {
            ConfigKeys config = new ConfigKeys();
            config.AskMultiFactorIdentification();

            return GetConfigurationAsync(config, ct);
        }

        /// <summary>
        /// Versão assíncrona de <see cref="GetNTPConfiguration"/>
        /// </summary>
        public Task<ConfigValues> GetNTPConfigurationAsync(CancellationToken ct = default(CancellationToken))
        {
            ConfigKeys config = new ConfigKeys();
            config.AskNTP();

            return GetConfigurationAsync(config, ct);
        }

        /// <summary>
        /// Versão assíncrona de <see cref="SetDaylightSavingTime"/>
        /// </summary>
        public Task<StatusResult> SetDaylightSavingTimeAsync(DateTime start, DateTime end, CancellationToken ct = default(CancellationToken))
        {
            ConfigValues config = new ConfigValues(true);
            config.general.Daylight_savings_time_start = start;
            config.general.Daylight_savings_time_end = end;

            return SetConfigurationAsync(config, ct);
        }

        /// <summary>
        /// Versão assíncrona de <see cref="SetNTPConfiguration(string, string)"/>
        /// </summary>
        public Task<StatusResult> SetNTPConfigurationAsync(String enable, String timezone, CancellationToken ct = default(CancellationToken))
        {
            ConfigValues config = new ConfigValues(new NTP() { enabled = enable, timezone = timezone });

            return SetConfigurationAsync(config, ct);
        }

        /// <summary>
        /// Versão assíncrona de <see cref="GoOnline"/>
        /// </summary>
        public Task<StatusResult> GoOnlineAsync(OnlineMode mode, long serverId, CancellationToken ct = default(CancellationToken))
        {
            ConfigValues config = new ConfigValues(true, true);
            config.general.Online = true;
            config.general.LocalIdentification = mode == OnlineMode.ReturnUserId ? true : false;
            config.online_client.ExtractTemplate = false; // mode == OnlineMode.ReturnTemplate ? true : false;
            config.online_client.ServerId = serverId;

            return SetConfigurationAsync(config, ct);
        }

        /// <summary>
        /// Versão assíncrona de <see cref="GoOffline"/>
        /// </summary>
        public Task<StatusResult> GoOfflineAsync(CancellationToken ct = default(CancellationToken))
        {
            ConfigValues config = new ConfigValues(true);
            config.general.Online = false;

            return SetConfigurationAsync(config, ct);
        }

        /// <summary>
        /// Versão assíncrona de <see cref="ActiveEmergencyMode"/>
        /// </summary>
        public Task<StatusResult> ActiveEmergencyModeAsync(CancellationToken ct = default(CancellationToken))
        {
            return SetExceptionModeAsync("emergency", ct);
        }

        /// <summary>
        /// Versão assíncrona de <see cref="ActiveLockDownMode"/>
        /// </summary>
        public Task<StatusResult> ActiveLockDownModeAsync(CancellationToken ct = default(CancellationToken))
        {
            return SetExceptionModeAsync("lock_down", ct);
        }

        /// <summary>
        /// Versão assíncrona de <see cref="ActiveNormalMode"/>
        /// </summary>
        public Task<StatusResult> ActiveNormalModeAsync(CancellationToken ct = default(CancellationToken))
        {
            return SetExceptionModeAsync(string.Empty, ct);
        }

        private Task<StatusResult> SetExceptionModeAsync(string mode, CancellationToken ct)
        {
            ConfigValues config = new ConfigValues(true);
            config.general.exception_mode = mode;

            return SetConfigurationAsync(config, ct);
        }

        /// <summary>
        /// Versão assíncrona de <see cref="SetSeniorModeOn"/>
        /// </summary>
        public Task<StatusResult> SetSeniorModeOnAsync(int request_timeout, CancellationToken ct = default(CancellationToken))
        {
            ConfigValues config = new ConfigValues(true, true);
            config.general.senior_mode = "1";
            config.online_client.ExtractTemplate = true;
            config.online_client.MaxRequest = 1;
            config.online_client.RequestTimeout = request_timeout;

            return SetConfigurationAsync(config, ct);
        }

        /// <summary>
        /// Versão assíncrona de <see cref="SetSeniorModeOff"/>
        /// </summary>
        public Task<StatusResult> SetSeniorModeOffAsync(CancellationToken ct = default(CancellationToken))
        {
            ConfigValues config = new ConfigValues(true, true);
            config.general.senior_mode = "0";
            config.online_client.ExtractTemplate = false;

            return SetConfigurationAsync(config, ct);
        }

        /// <summary>
        /// Versão assíncrona de <see cref="SetMultiFactorIdentification"/>
        /// </summary>
        public Task<StatusResult> SetMultiFactorIdentificationAsync(string mode = "0", CancellationToken ct = default(CancellationToken))
        {
            var config = new ConfigValues(new Identifier
            {
                multi_factor_authentication = mode
            });
            return SetConfigurationAsync(config, ct);
        }

        /// <summary>
        /// Versão assíncrona de <see cref="GetConfiguration"/>
        /// </summary>
        public async Task<ConfigValues> GetConfigurationAsync(ConfigKeys config, CancellationToken ct = default(CancellationToken))
        {
            await CheckSessionAsync(ct).ConfigureAwait(false);
            return await JsonAsync<ConfigValues>(URL + "get_configuration.fcgi?session=" + Session, config, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Versão assíncrona de <see cref="SetConfiguration"/> (em erro HTTP com JSON retorna o StatusResult de erro, como no síncrono)
        /// </summary>
        public async Task<StatusResult> SetConfigurationAsync(ConfigValues config, CancellationToken ct = default(CancellationToken))
        {
            await CheckSessionAsync(ct).ConfigureAwait(false);
            return await JsonAsync<StatusResult>(URL + "set_configuration.fcgi?session=" + Session, config, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Versão assíncrona de <see cref="SendConfiguration"/>
        /// </summary>
        public async Task<StatusResult> SendConfigurationAsync(string config, CancellationToken ct = default(CancellationToken))
        {
            await CheckSessionAsync(ct).ConfigureAwait(false);
            return await JsonAsync<StatusResult>(URL + "set_configuration.fcgi?session=" + Session, config, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Versão assíncrona de <see cref="UserSetImageList"/>
        /// </summary>
        public async Task<UserImagesFacialResponse> UserSetImageListAsync(UserImagesFacialRequest info, CancellationToken ct = default(CancellationToken))
        {
            await CheckSessionAsync(ct).ConfigureAwait(false);
            return await JsonAsync<UserImagesFacialResponse>(URL + "user_set_image_list.fcgi?session=" + Session, info, ct).ConfigureAwait(false);
        }

        #endregion

        #region Hardware (Device_Hardware.cs)

        /// <summary>
        /// Versão assíncrona de <see cref="GetDoorState"/>
        /// </summary>
        public async Task<DoorState> GetDoorStateAsync(CancellationToken ct = default(CancellationToken))
        {
            await CheckSessionAsync(ct).ConfigureAwait(false);
            return await JsonAsync<DoorState>(URL + "doors_state.fcgi?session=" + Session, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Servidor (iDAccess_Server.cs)

        /// <summary>
        /// Versão assíncrona de <see cref="ChangeDeviceName"/>
        /// </summary>
        public async Task ChangeDeviceNameAsync(string sName, CancellationToken ct = default(CancellationToken))
        {
            long devId = (await SystemInformationAsync(ct).ConfigureAwait(false)).deviceId;
            Devices dev = await LoadAsync<Devices>(devId, ct).ConfigureAwait(false);
            if (dev != null)
            {
                dev.name = sName;
                await ModifyAsync(devId, dev, ct).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Versão assíncrona de <see cref="ConfigureServer"/>
        /// </summary>
        public async Task<StatusResult> ConfigureServerAsync(string server_url, OnlineMode mode, bool Overwrite = false, CancellationToken ct = default(CancellationToken))
        {
            const long serverId = -1;
            Devices server = new Devices() { id = serverId, IP = server_url, name = "Server", PublicKey = "anA=" };

            if (Overwrite)
                await DestroyAsync<Devices>(serverId, ct).ConfigureAwait(false);

            if (await AddAsync(server, ct).ConfigureAwait(false) == serverId)
                return await GoOnlineAsync(mode, serverId, ct).ConfigureAwait(false);
            else
                return new StatusResult(500, "Não foi possível incluir o servidor no equipamento");
        }

        #endregion

        #region Usuários (Device_Users.cs)

        /// <summary>
        /// Versão assíncrona de <see cref="SetPassword"/> (métodos async não aceitam 'ref'):
        /// atualiza password e salt do próprio objeto <paramref name="usr"/>, como o síncrono.
        /// </summary>
        public async Task SetPasswordAsync(Users usr, string sSenha, CancellationToken ct = default(CancellationToken))
        {
            if (GenerateLocalPassword)
                usr.password = Device.GeneratePassword(sSenha, out usr.salt);
            else
            {
                PasswordResult pw = await CommandAsync<PasswordResult>("user_hash_password", "{\"password\":\"" + sSenha + "\"}", null, ct).ConfigureAwait(false);
                usr.password = pw.password;
                usr.salt = pw.salt;
            }
        }

        /// <summary>
        /// Versão assíncrona de <see cref="DeleteUserImageListFacial"/>
        /// </summary>
        public async Task DeleteUserImageListFacialAsync(long[] listPhotos, CancellationToken ct = default(CancellationToken))
        {
            await CheckSessionAsync(ct).ConfigureAwait(false);
            var payload = new UserImagesListRequest()
            {
                user_ids = listPhotos,
            };
            await JsonAsync<string>(URL + "user_destroy_image.fcgi?&session=" + Session, payload, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Versão assíncrona de <see cref="UserListImages"/> (como no síncrono, usa o timeout global WebJson.TimeOut)
        /// </summary>
        public async Task<long[]> UserListImagesAsync(CancellationToken ct = default(CancellationToken))
        {
            await CheckSessionAsync(ct).ConfigureAwait(false);
            var result = await WebJson.JsonCommandAsync<UserListImagesResult>(URL + "user_list_images.fcgi?&session=" + Session, null, null, 0, null, HttpClientProvider, ct).ConfigureAwait(false);
            return result.user_ids;
        }

        /// <summary>
        /// Versão assíncrona de <see cref="UserListImagesInfo"/> (como no síncrono, usa o timeout global WebJson.TimeOut)
        /// </summary>
        public async Task<User_Image[]> UserListImagesInfoAsync(CancellationToken ct = default(CancellationToken))
        {
            await CheckSessionAsync(ct).ConfigureAwait(false);
            return (await WebJson.JsonCommandAsync<ObjectResult>(URL + "user_list_images.fcgi?&session=" + Session + "&get_timestamp=1", null, null, 0, null, HttpClientProvider, ct).ConfigureAwait(false)).image_info;
        }

        #endregion
    }
}
