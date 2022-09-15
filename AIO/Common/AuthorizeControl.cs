using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel.Design;
using AIO.Common.Response;

namespace AIO.Common
{
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    public partial class AuthorizeControl : UserControl
    {
        public AuthorizeControl()
        {
            InitializeComponent();

            CommonData.FillMirrors(cbMirrorDomain);
            CommonData.FillCurrencies(cbCurrency);


        }

        public string GetApiKey
        {
            get
            {
                return tbApiKey.Text;
            }
        }

        public string GetMirror
        {
            get
            {
                return cbMirrorDomain.SelectedItem.ToString();
            }
        }

        public string GetCurrency
        {
            get
            {
                return cbCurrency.SelectedItem.ToString();
            }
        }



        public void ChangeCurrency(string currency)
        {


           // comboBox1.SelectedIndex = Array.FindIndex(currenciesAvailable, row => row == currencySelected.ToUpper());

        }

        public void CanChangeAuthorization(bool state = true)
        {
            tbApiKey.Enabled = state;
            cbMirrorDomain.Enabled = state;

        }

        public void CanChangeMirror(bool state = true)
        {
            cbMirrorDomain.Enabled = state;
        }
        public void CanChangeCurrency(bool state = true)
        {
            cbCurrency.Enabled = state;
        }


        public void SyncCurrencies(List<Balances> balances)
        {
            for (var i = 0; i < balances.Count; i++)
            {
                for (int s = 0; s < cbCurrency.Items.Count; s++)
                {
                    if (balances[i].available.currency == cbCurrency.Items[s].ToString().ToLower())
                    {
                        cbCurrency.Items[s] = string.Format("{0} {1}", cbCurrency.Items[s].ToString(), balances[i].available.amount.ToString("0.00000000"));
                        //currencySelect.Items.Add(string.Format("{0} {1}", s, response.data.user.balances[i].available.amount.ToString("0.00000000")));
                        break;
                    }
                }
            }
        }



        private async void tbApiKey_TextChanged(object sender, EventArgs e)
        {
            //token = textBox1.Text;
            ////Properties.Settings.Default.token = token;
            //if (token.Length == 96)
            //{
            //    // Connect();
            //    //await Authorize();

            //}
            //else
            //{
            //    toolStripStatusLabel1.Text = "Disconnected";
            //}
        }

        private void btnLoginLogout_Click(object sender, EventArgs e)
        {
            //this.
        }

        private void cbCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
