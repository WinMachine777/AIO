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
using AIO.Modules;

namespace AIO.Common
{
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    public partial class AuthorizeControl : UserControl
    {

        // private const int CB_SETCUEBANNER = 0x1703;
        // [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        // private static extern int SetPlaceholder(IntPtr hWnd, int msg, int wParam, [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lParam);

        public AuthorizeControl()
        {
            InitializeComponent();

            // SetPlaceholder(this.cbMirrorDomain.Handle, CB_SETCUEBANNER, 0, "Please select an item...");
            // SetPlaceholder(this.cbCurrency.Handle, CB_SETCUEBANNER, 0, "Please select an item...");

            CommonData.FillMirrors(cbMirrorDomain);
            CommonData.FillCurrencies(cbCurrency);


        }

        public string ApiKey
        {
            get
            {
                return tbApiKey.Text;
            }
            set
            {
                tbApiKey.Text = value;
            }
        }

        public string Domain
        {
            get
            {
                return cbMirrorDomain.SelectedItem.ToString();
            }
            set
            {
                cbMirrorDomain.SelectedIndex = cbCurrency.FindStringExact(value);
            }
        }

        public string Currency
        {
            get
            {
                return cbCurrency.SelectedItem.ToString();
            }
            set
            {
                cbCurrency.SelectedIndex = cbCurrency.FindStringExact(value);
            }
        }

        private bool isConnected = false;

        public bool IsConnected
        {
            get
            {
                return isConnected;
            }
            set
            {
                isConnected = value;
                //CanChangeAuthorization();
            }
        }

        public void ChangeCurrency(string currency)
        {

            Currency = currency;

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

        public void SetCurrency(string currency)
        {
            cbCurrency.SelectedIndex = cbCurrency.FindStringExact(currency);
        }

        public void SetMirror(string mirror)
        {
            cbMirrorDomain.SelectedIndex = cbCurrency.FindStringExact(mirror);
        }

        public void SetApiKey(string apiKey)
        {
            tbApiKey.Text = apiKey;
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

            (this.ParentForm as IGameEngine).Authorize();
        }

        private void cbCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void IsRunning(bool state)
        {
            tbApiKey.Enabled = !state;
            cbMirrorDomain.Enabled = !state;
            cbCurrency.Enabled = !state;
        }
    }
}
