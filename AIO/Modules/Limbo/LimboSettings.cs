using AIO.Common;
using System.Configuration;

namespace AIO.Modules.Limbo
{
    public class LimboSettings : AIOSettings
    {


        // ServerSeedBox.Text = Properties.Settings.Default.serverSeed;
        // ClientSeedBox.Text = Properties.Settings.Default.clientSeed;
        // NonceBox.Text = Properties.Settings.Default.nonce.ToString();
        // NonceStopBox.Text = Properties.Settings.Default.nonceStop.ToString();


        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string ServerSeed
        {
            get
            {
                return this["Limbo.ServerSeed"].ToString();
            }
            set
            {
                this["Limbo.ServerSeed"] = value;
            }
        }
        [UserScopedSetting()]
        //[DefaultSettingValue("")]
        public string ClientSeed
        {
            get
            {
                return this["Limbo.ClientSeed"].ToString();
            }
            set
            {
                this["Limbo.ClientSeed"] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public int Nonce
        {
            get
            {
                return (int)this["Limbo.Nonce"];
            }
            set
            {
                this["Limbo.Nonce"] = value;
            }
        }

        [UserScopedSetting()]
        //[DefaultSettingValue("")]
        public int NonceStop
        {
            get
            {
                return (int)this["Limbo.NonceStop"];
            }
            set
            {
                this["Limbo.NonceStop"] = value;
            }
        }



        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string ApiKey
        {
            get
            {
                return this["Limbo.ApiKey"].ToString();
            }
            set
            {
                this["Limbo.ApiKey"] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string Domain
        {
            get
            {
                return this["Limbo.Domain"].ToString();
            }
            set
            {
                this["Limbo.Domain"] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string Currency
        {
            get
            {
                return this["Limbo.Currency"].ToString();
            }
            set
            {
                this["Limbo.Currency"] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string LuaScript
        {
            get
            {
                return this["Limbo.LuaScript"].ToString();
            }
            set
            {
                this["Limbo.LuaScript"] = value;
            }
        }
    }

}
