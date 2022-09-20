using AIO.Common;
using System.Configuration;

namespace AIO.Modules.Dice
{
    public class DiceSettings : AIOSettings
    {
        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string ApiKey
        {
            get
            {
                return this["Dice.ApiKey"].ToString();
            }
            set
            {
                this["Dice.ApiKey"] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string Domain
        {
            get
            {
                return this["Dice.Domain"].ToString();
            }
            set
            {
                this["Dice.Domain"] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string Currency
        {
            get
            {
                return this["Dice.Currency"].ToString();
            }
            set
            {
                this["Dice.Currency"] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string LuaScript
        {
            get
            {
                return this["Dice.LuaScript"].ToString();
            }
            set
            {
                this["Dice.LuaScript"] = value;
            }
        }
    }

}
