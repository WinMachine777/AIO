using System.Windows.Forms;


public class CommonData
{

    public static void FillCurrencies(ComboBox combo)
    {
        // fill currency combobox

        combo.Items.Clear();

        foreach (var item in CommonData.CurrenciesAvailable)
        {
            combo.Items.Add(item);
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

        foreach (var item in CommonData.MirrorsAvailable)
        {
            combo.Items.Add(item);
        }

    }

}
