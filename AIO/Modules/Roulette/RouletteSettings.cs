using AIO.Common;
using System.Configuration;

namespace AIO.Modules.Roulette
{
    public class RouletteSettings : AIOSettings
    {
        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string ApiKey
        {
            get
            {
                return this["Roulette.ApiKey"].ToString();
            }
            set
            {
                this["Roulette.ApiKey"] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string Domain
        {
            get
            {
                return this["Roulette.Domain"].ToString();
            }
            set
            {
                this["Roulette.Domain"] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string Currency
        {
            get
            {
                return this["Roulette.Currency"].ToString();
            }
            set
            {
                this["Roulette.Currency"] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string LuaScript
        {
            get
            {
                return this["Roulette.LuaScript"].ToString();
            }
            set
            {
                this["Roulette.LuaScript"] = value;
            }
        }
    }

}
