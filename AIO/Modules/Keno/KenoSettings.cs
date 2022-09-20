using AIO.Common;
using System.Configuration;

namespace AIO.Modules.Keno
{
    public class KenoSettings : AIOSettings
    {
        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string ApiKey
        {
            get
            {
                return this["Keno.ApiKey"].ToString();
            }
            set
            {
                this["Keno.ApiKey"] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string Domain
        {
            get
            {
                return this["Keno.Domain"].ToString();
            }
            set
            {
                this["Keno.Domain"] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string Currency
        {
            get
            {
                return this["Keno.Currency"].ToString();
            }
            set
            {
                this["Keno.Currency"] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string LuaScript
        {
            get
            {
                return this["Keno.LuaScript"].ToString();
            }
            set
            {
                this["Keno.LuaScript"] = value;
            }
        }
    }

}
