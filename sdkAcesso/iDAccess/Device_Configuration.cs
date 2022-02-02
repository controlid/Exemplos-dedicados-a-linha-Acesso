using System;
using System.Drawing;
using System.Globalization;

namespace ControliD.iDAccess
{
    // https://www.controlid.com.br/suporte/api_idaccess_V2.6.8.html#40_configurations
    public partial class Device
    {
        /// <summary>
        /// Obtêm horário de verão
        /// </summary>
        /// <returns>Objeto de configuração com horário de verão setado</returns>
        public ConfigValues GetDaylightSavingTime()
        {
            ConfigKeys config = new ConfigKeys();
            config.AskDayLightSavingTime();

            return GetConfiguration(config);
        }

        public ConfigValues GetMultiFactorIdentification()
        {
            ConfigKeys config = new ConfigKeys();
            config.AskMultiFactorIdentification();
            return GetConfiguration(config);
        }

        /// <summary>
        /// Configura horário de verão
        /// </summary>
        /// <param name="start">data de início</param>
        /// <param name="end">data de fim</param>
        /// <returns></returns>
        public StatusResult SetDaylightSavingTime(DateTime start, DateTime end)
        {
            ConfigValues config = new ConfigValues(true);
            config.general.Daylight_savings_time_start = start;
            config.general.Daylight_savings_time_end = end;

            return SetConfiguration(config);
        }

        /// <summary>
        /// Modos de utilização online em caso de evento biométrico (do menor ao maior controle)
        /// </summary>
        public enum OnlineMode
        {
            /// <summary>
            /// Não é modo online
            /// </summary>
            Standalone = 0,
            /// <summary>
            /// Equipamento faz o match e envia ID do usuário (Modo Paiva)
            /// </summary>
            ReturnUserId = 1,
            /// <summary>
            /// Equipamento envia imagem do dedo
            /// </summary>
            ReturnImage = 2
        }

        /// <summary>
        /// Coloca o equipamento em modo online informando um servidor já existente)
        /// </summary>
        public StatusResult GoOnline(OnlineMode mode, long serverId)
        {
            ConfigValues config = new ConfigValues(true, true);
            config.general.Online = true;
            config.general.LocalIdentification = mode == OnlineMode.ReturnUserId ? true : false;
            config.online_client.ExtractTemplate = false; // mode == OnlineMode.ReturnTemplate ? true : false;
            config.online_client.ServerId = serverId;

            return SetConfiguration(config);
        }

        /// <summary>
        /// Define um servidor, e coloca o equipamento online
        /// </summary>
        public StatusResult SetOnline(OnlineMode mode, string cName, string url)
        {
            var devSRV = new Devices()
            {
                id = -1,
                name = cName,
                IP = url,
                PublicKey = "anA="
            };

            LoadOrSet(-1, devSRV);

            ConfigValues config = new ConfigValues(true, true);
            config.general.Online = true;
            config.general.LocalIdentification = mode == OnlineMode.ReturnUserId ? true : false;
            //config.online_client.ExtractTemplate = false; // mode == OnlineMode.ReturnTemplate ? true : false;
            config.online_client.ExtractTemplate = mode == OnlineMode.ReturnImage ? false : true;
            config.online_client.ServerId = devSRV.id;

            return SetConfiguration(config);
        }

        /// <summary>
        /// Coloca o equipamento em modo offline
        /// </summary>
        public StatusResult GoOffline()
        {
            ConfigValues config = new ConfigValues(true);
            config.general.Online = false;

            return SetConfiguration(config);
        }

        /// <summary>
        /// Coloca o equipamento em modo de emergência
        /// </summary>
        public StatusResult ActiveEmergencyMode()
        {
            return SetExceptionMode("emergency");
        }

        /// <summary>
        /// Coloca o equipamento em modo de lock-down
        /// </summary>
        public StatusResult ActiveLockDownMode()
        {
            return SetExceptionMode("lock_down");
        }

        /// <summary>
        /// Retira o equipamento do modo de exceção
        /// </summary>
        public StatusResult ActiveNormalMode()
        {
            return SetExceptionMode(string.Empty);
        }

        /// <summary>
        /// Atribui o modo de exceção do equipamento
        /// </summary>
        private StatusResult SetExceptionMode(string mode)
        {
            ConfigValues config = new ConfigValues(true);
            config.general.exception_mode = mode;

            return SetConfiguration(config);
        }

        /// <summary>
        /// Ativa o modo Senior de funcionamento no equipamento
        /// </summary>
        public StatusResult SetSeniorModeOn(int request_timeout)
        {
            ConfigValues config = new ConfigValues(true, true);
            config.general.senior_mode = "1";
            config.online_client.ExtractTemplate = true;
            config.online_client.MaxRequest = 1;
            config.online_client.RequestTimeout = request_timeout;

            return SetConfiguration(config);
        }

        /// <summary>
        /// Desativa o modo Senior de funcionamento no equipamento
        /// </summary>
        public StatusResult SetSeniorModeOff()
        {
            ConfigValues config = new ConfigValues(true, true);
            config.general.senior_mode = "0";
            config.online_client.ExtractTemplate = false;

            return SetConfiguration(config);
        }

        /// <summary>
        /// Seta o multi-factor-identification
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public StatusResult SetMultiFactorIdentification(string mode = "0")
        {
            var config = new ConfigValues(new Identifier
            {
                multi_factor_authentication = mode
            });
            return SetConfiguration(config);
        }

        public ConfigValues GetConfiguration(ConfigKeys config)
        {
            CheckSession();
            return WebJson.JsonCommand<ConfigValues>(URL + "get_configuration.fcgi?session=" + Session, config, null, TimeOut);
        }

        public StatusResult SetConfiguration(ConfigValues config)
        {
            CheckSession();
            return WebJson.JsonCommand<StatusResult>(URL + "set_configuration.fcgi?session=" + Session, config, null, TimeOut);
        }
        public StatusResult SendConfiguration(string config)
        {
            CheckSession();
            return WebJson.JsonCommand<StatusResult>(URL + "set_configuration.fcgi?session=" + Session, config, null, TimeOut);
        }
        public UserImagesFacialResponse UserSetImageList(UserImagesFacialRequest info)
        {
            CheckSession();
            return WebJson.JsonCommand<UserImagesFacialResponse>(URL + "user_set_image_list.fcgi?session=" + Session, info, null, TimeOut);
        }

        /// <summary>
        /// Configura o Beep
        /// </summary>
        public void SetBeep(bool lEnable)
        {
            var cfg = new ConfigValues(true);
            cfg.general.beep_enabled = lEnable ? "1" : "0";
            CheckSession();
            WebJson.JsonCommand<string>(URL + "set_configuration.fcgi?session=" + Session, cfg, null, TimeOut);
        }

        /// <summary>
        /// Configura o uso da campainha em um RELÊ específico
        /// </summary>
        public void SetBell(int nRele)
        {
            var cfg = new ConfigValues(true);
            cfg.general.beep_enabled = nRele == 0 ? "0":"1";
            cfg.general.bell_relay = nRele.ToString();
            CheckSession();
            WebJson.JsonCommand<string>(URL + "set_configuration.fcgi?session=" + Session, cfg, null, TimeOut);
        }

        public void SetGeneralConfigValues(General cfgGeneral)
        {
            var cfg = new ConfigValues(cfgGeneral);
            CheckSession();
            WebJson.JsonCommand<string>(URL + "set_configuration.fcgi?session=" + Session, cfg, null, TimeOut);
        }

        // TODO: unificar na configuração geral
        public void SetLeds(LedsColors leds)
        {
            ConfigValues cfg = new ConfigValues(leds);
            CheckSession();
            WebJson.JsonCommand<string>(URL + "set_configuration.fcgi?session=" + Session, cfg, null, TimeOut);
            WebJson.JsonCommand<string>(URL + "led_rgb_refresh.fcgi?session=" + Session, null, null, TimeOut);
        }

        public void SetCatra(ConfigCatra config)
        {
            /* synctask...
             *
             * if(dbDev.Modelo==ControliD.iDAccess.DeviceModels.iDBlock)
             *     dev.SetCatra(dbDev.antiPassback)
             */
            CheckSession();
            var cfg = new ConfigValues(config);
            WebJson.JsonCommand<string>(URL + "set_configuration.fcgi?session=" + Session, cfg, null, TimeOut);
        }

        public StatusResult UpdateFirmware(string url = "http://controlid.com.br/idaccess/acfw_update.php", string mode = "default")
        {
            CheckSession();
            var updt = new UpdateFirmware()
            {
                server_url = url,
                update_mode = mode // factory_reset
            };
            var sr = WebJson.JsonCommand<StatusResult>(URL + "update_from_custom_server.fcgi?session=" + Session, updt, null, TimeOut);
            if (sr.Status == null) // Se não retornou nada é porque deu certo e o equipamento reiniciou
                return new StatusResult(200, "OK");
            else // caso contrario houve algum erro
                return sr;
        }

        /// <summary>
        /// Comando para setar uma nova senha master para o dispositivo
        /// </summary>
        /// <param name="password">Nova senha a ser definida no dispositivo</param>
        /// <returns></returns>
        public StatusResult SetMasterPassword(string password)
        {
            CheckSession();
            var obj = new MasterPassword() { password = password };
            return WebJson.JsonCommand<StatusResult>(URL + "master_password.fcgi?session=" + Session, obj, null, TimeOut);
        }

        /// <summary>
        /// Comando para executar um Factory Reset (reset de fábrica) no equipamento
        /// </summary>
        /// <param name="keepNetworkInfo">Parâmetro que permite que o equipamento mantenha as configurações de rede após o reset.</param>
        /// <returns></returns>
        public StatusResult FactoryReset(bool keepNetworkInfo = false)
        {
            CheckSession();
            if (keepNetworkInfo)
            {
                var obj = new FactoryReset() { keep_network_info = true };
                return WebJson.JsonCommand<StatusResult>(URL + "reset_to_factory_default.fcgi?session=" + Session, obj, null, TimeOut);
            }
            else
                return WebJson.JsonCommand<StatusResult>(URL + "reset_to_factory_default.fcgi?session=" + Session, null, null, TimeOut);
        }

        public StatusResult RebootRecovery()
        {
            CheckSession();
            return WebJson.JsonCommand<StatusResult>(URL + "reboot_recovery.fcgi?session=" + Session, null, null, TimeOut);
        }

        public void SetLogoImage(Image oFoto)
        {
            CheckSession();
            try
            {
                if (oFoto == null)
                    WebJson.JsonCommand<string>(URL + "logo_destroy.fcgi?&session=" + Session, null, null, TimeOut);
                else
                {
                    WebJson.JsonCommand<string>(URL + "logo_change.fcgi?session=" + Session, oFoto, null, TimeOut, System.Drawing.Imaging.ImageFormat.Png);
                    oFoto.Dispose();
                }
            }
            catch (Exception)
            {
            }
        }

        public StatusResult ChangeDeviceLanguage(string language)
        {
            var cfg = new ConfigValues(true);
            cfg.general.language = language;
            CheckSession();
            return WebJson.JsonCommand<StatusResult>(URL + "set_configuration.fcgi?session=" + Session, cfg, null, TimeOut);
        }

        public StatusResult SetFacialConfiguration(int maskDetectionEnabled, long? identificationDistance)
        {
            var face_id = new Face_id();
            face_id.mask_detection_enabled = maskDetectionEnabled.ToString();

            if (identificationDistance.HasValue)
            {
                double val = 11.6 / (long)identificationDistance;
                var nfi = new NumberFormatInfo() { NumberDecimalSeparator = "." };
                face_id.min_detect_bounds_width = val.ToString(nfi);
            }

            var cfg = new ConfigValues(face_id);

            CheckSession();
            return WebJson.JsonCommand<StatusResult>(URL + "set_configuration.fcgi?session=" + Session, cfg, null, TimeOut);
        }

    }
}
