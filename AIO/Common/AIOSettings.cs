using System.Configuration;

namespace AIO.Common
{
    public class AIOSettings : ApplicationSettingsBase
    {
        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string GlobalApiKey
        {
            get
            {
                return this["Global.ApiKey"].ToString();
            }
            set
            {
                this["Global.ApiKey"] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string GlobalDomain
        {
            get
            {
                return this["Global.Domain"].ToString();
            }
            set
            {
                this["Global.Domain"] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string GlobalCurrency
        {
            get
            {
                return this["Global.Currency"].ToString();
            }
            set
            {
                this["Global.Currency"] = value;
            }
        }

    }

}
