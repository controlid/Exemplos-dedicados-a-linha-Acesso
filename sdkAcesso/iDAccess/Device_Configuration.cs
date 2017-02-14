using System;
using System.Drawing;

namespace ControlID.iDAccess
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
            config.online_client.ExtractTemplate = false; // mode == OnlineMode.ReturnTemplate ? true : false;
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
             * if(dbDev.Modelo==ControlID.iDAccess.DeviceModels.iDBlock)
             *     dev.SetCatra(dbDev.antiPassback)
             */
            CheckSession();
            var cfg = new ConfigValues(config);
            WebJson.JsonCommand<string>(URL + "set_configuration.fcgi?session=" + Session, cfg, null, TimeOut);
        }

        public StatusResult UpdateFirmware(string url = null, string mode = "default")
        {
            // = "http://controlid.com.br/idaccess/acfw_update.php"
            CheckSession();
            var updt = new UpdateFirmware()
            {
                server_url = url,
                update_mode = mode // factory_reset
            };
            return WebJson.JsonCommand<StatusResult>(URL + "update_from_custom_server.fcgi?session=" + Session, updt, null, TimeOut);
        }

        public StatusResult FactoryReset()
        {
            CheckSession();
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
                    WebJson.JsonCommand<string>(URL + "logo_change.fcgi?session=" + Session, oFoto, null, TimeOut);
            }
            catch (Exception)
            {
            }
        }
    }
}
