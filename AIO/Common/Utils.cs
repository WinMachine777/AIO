using System;
using System.Linq;
using System.Windows.Forms;

namespace AIO.Common
{



    public partial class Utils
    {


        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }



        public static void FillCurrencies(ComboBox combo)
        {
            // fill currency combobox

            combo.Items.Clear();

            foreach (var item in Utils.CurrenciesAvailable.OrderBy(x => x))
            {
                combo.Items.Add(item.ToUpperInvariant());
            }

        }

        public static string[] CurrenciesAvailable = {
            "BTC",
            "ETH",
            "LTC",
            "DOGE",
            "BCH",
            "XRP",
            "TRX",
            "EOS",
            "BNB",
            "USDT",
            "APE",
            "BUSD",
            "CRO",
            "DAI",
            "LINK",
            "SAND",
            "SHIB",
            "UNI",
            "USDC"
       };

        public static string[] MirrorsAvailable = {
        "Stake.com",
        "Stake.bet",
        "Stake.games",
        "Staketr.com",
        "Staketr2.com",
        "Staketr3.com",
        "Staketr4.com",
        "Staketr5.com",
        "Stake.bz",
        "Stake.jp",
        "Stake.ac",
        "Stake.icu",
        "Stake.us"
       };

        public static void FillMirrors(ComboBox combo)
        {
            // fill currency combobox

            combo.Items.Clear();

            foreach (var item in Utils.MirrorsAvailable.OrderBy(x => x))
            {
                combo.Items.Add(item.ToUpperInvariant());
            }

        }

    }
}
