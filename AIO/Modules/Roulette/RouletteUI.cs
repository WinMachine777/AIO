using FastColoredTextBoxNS;
using Newtonsoft.Json;
using RestSharp;
using SharpLua;
//using SuperSocket.ClientEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AIO.Common;
using AIO.Common.Response;
using System.Net;
using AIO.Common.Request;

namespace AIO.Modules.Roulette
{
    public partial class RouletteUI : Form,IGameEngine, ICrossStrategy
    {

        //public CookieContainer cc;
        //RestClient SharedRestClient;

        APIClientManager APIClientManager;

        public string GameDomain
        {
            get
            {
                return authorizeControl1.Domain;
            }
        }

        public string token
        {
            get
            {
                return authorizeControl1.ApiKey;
            }
        }

        #region CROSS STRATEGY

        protected delegate void dStartNewGameEngine(string game, string currency, string stratFile);

        public void startNewGameEngine(string game, string currency, string stratFile)
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                (this.Owner as Dashboard).OpenNewGameEngine(this, game, token, GameDomain, currency, stratFile);
                //this.Close();
            });
        }


        public RouletteUI(object source, string game, string apiKey, string mirror, string currency, string stratFile) : this()
        {
            InitializeComponent();
            this.Show(source as Form);
            if (game == null || apiKey == null || mirror == null || currency == null || stratFile == null)
            {
                return;
            }
        }





        public string ReadScriptBox()
        {
            return luaCodeBox.Text;
        }

        public void WriteScriptBox(string scriptText)
        {
            luaCodeBox.Text = scriptText;
        }

        #endregion








        //private WebSocket chat_socket { get; set; }
        private FastColoredTextBox luaCodeBox;

        LuaInterface lua = LuaRuntime.GetLua();
        delegate void LogConsole(string text);
        delegate void dtip(string username, decimal amount);
        delegate void dvault(decimal sentamount);
        delegate void dStop();
        delegate void dResetSeed();
        delegate void dResetStat();
        delegate void dResetChip();

        delegate void dSeedEmpty();
        delegate void dVaultEmpty(decimal sentamount);
        delegate void dTipEmpty(string username, decimal amount);

        public List<TabControl> listLua = new List<TabControl>();
        public List<ListView> listLogs = new List<ListView>();
        public List<Panel> listGraph = new List<Panel>();


        List<double> xList = new List<double>();
        List<double> yList = new List<double>();

        List<Color> colorTable = new List<Color> {
            Color.LimeGreen,
            Color.Red,
            Color.Black,
            Color.Red,
            Color.Black,
            Color.Red,
            Color.Black,
            Color.Red,
            Color.Black,
            Color.Red,
            Color.Black,
            Color.Black,
            Color.Red,
            Color.Black,
            Color.Red,
            Color.Black,
            Color.Red,
            Color.Black,
            Color.Red,
            Color.Red,
            Color.Black,
            Color.Red,
            Color.Black,
            Color.Red,
            Color.Black,
            Color.Red,
            Color.Black,
            Color.Red,
            Color.Black,
            Color.Black,
            Color.Red,
            Color.Black,
            Color.Red,
            Color.Black,
            Color.Red,
            Color.Black,
            Color.Red
        };




        public string clientSeed = "";
        public string serverSeed = "";
        public int nonce = 1;
        public decimal balanceSim = 0;
        public int stopNonce = 0;

        public bool running = false;
        public bool ready = true;
        public bool sim = false;
        public int counter = 0;

        public string currencySelected
        {
            get
            {
                return authorizeControl1.Currency;                    
            }
            set
            {
                authorizeControl1.SetCurrency(value);
            }
        }



        public double target = 0;
        public decimal BaseBet = 0;
        public decimal amount = 0;
        public decimal currentBal = 0;
        public decimal currentProfit = 0;
        public decimal currentWager = 0;
        public bool isWin = false;
        public int wins = 0;
        public int losses = 0;
        public int winstreak = 0;
        public int losestreak = 0;
        public decimal Lastbet = 0;
        long beginMs = 0;

        List<decimal> highestProfit = new List<decimal> { 0 };
        List<decimal> lowestProfit = new List<decimal> { 0 };
        List<decimal> highestBet = new List<decimal> { 0 };

        List<int> highestStreak = new List<int> { 0 };
        List<int> lowestStreak = new List<int> { 0 };

        public lastbet last = new lastbet();
        public List<Values> color = new List<Values> { };
        public List<Values> number = new List<Values> { };
        public List<Values> parity = new List<Values> { };
        public List<Values> range = new List<Values> { };
        public List<Values> row = new List<Values> { };

        private bool is_connected = false;

        public RouletteUI()
        {

            authorizeControl1 = new AuthorizeControl();

            InitializeComponent();

            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);

            this.listView1.ItemChecked += this.listView1_ItemChecked;
            this.CommandBox2.KeyDown += this.CmdBox_KeyDown;
            this.listBox3.KeyDown += this.listBox3_KeyDown;
            //this.listBox3.Click += this.listBox3_Click;

            richTextBox2.ReadOnly = true;
            richTextBox2.BackColor = Color.FromArgb(249, 249, 249);
            listView1.BackColor = Color.FromArgb(249, 249, 249);
            listBox3.BackColor = Color.FromArgb(249, 249, 249);
            //listView1.Hide();

            Text += " - AIO " + Application.ProductVersion;
            Icon = Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);

            listView1.SetDoubleBuffered(true);

            //RegisterLua();
            EnableTab(tabPage5, false);
            richTextBox3.BackColor = Color.White;
            luaCodeBox = new FastColoredTextBox();
            luaCodeBox.Dock = DockStyle.Fill;
            luaCodeBox.Language = Language.Lua;
            luaCodeBox.BorderStyle = BorderStyle.None;
            luaCodeBox.BackColor = Color.FromArgb(249, 249, 249);
            tabPageLua.Controls.Add(luaCodeBox);

            luaCodeBox.TextChanged += this.richTextBox1_TextChanged;

            //richTextBox1.Text = Properties.Settings.Default.textCode;

            APIClientManager = new APIClientManager();

        }


        ///// <summary>
        ///// CreateOrUseDefaultRestClient
        ///// </summary>
        ///// <param name="dispose"></param>
        //private void CreateOrUseDefaultRestClient(bool dispose = false)
        //{
        //    if (dispose == true)
        //    {
        //        SharedRestClient = null;
        //        cc = null;
        //    }

        //    if (SharedRestClient != null)
        //    {
        //        return;
        //    }

        //    var mainurl = "https://api." + StakeSite + "/graphql";

        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        //    this.cc = new CookieContainer();

        //    SharedRestClient = new RestClient();
        //    SharedRestClient.BaseUrl = new Uri(mainurl);
        //    SharedRestClient.CookieContainer = this.cc;
        //    SharedRestClient.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.163 Safari/537.36";
        //}

        ///// <summary>
        ///// CreateDefaultRestRequest
        ///// </summary>
        ///// <param name="apiKey"></param>
        ///// <returns></returns>
        //private RestRequest CreateDefaultRestRequest(string apiKey)
        //{
        //    RestRequest restRequest = new RestRequest(Method.POST);
        //    restRequest.AddHeader("authorization", string.Format("Bearer {0}", apiKey));
        //    restRequest.AddHeader("x-access-token", apiKey);
        //    restRequest.AddHeader("Content-type", "application/json");
        //    return restRequest;
        //}


        public static void EnableTab(TabPage page, bool enable)
        {
            foreach (Control ctl in page.Controls) ctl.Enabled = enable;
        }
        private void listBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.C)
            {
                string tmpStr = "";
                foreach (var item in listBox3.SelectedItems)
                {
                    tmpStr += listBox3.GetItemText(item) + "\n";
                }
                Clipboard.SetData(DataFormats.StringFormat, tmpStr);
            }
            if (e.Control == true && e.KeyCode == Keys.A)
            {
                for (int i = 0; i < listBox3.Items.Count; i++)
                {
                    listBox3.SetSelected(i, true);
                }
            }
        }

        private void Application_ApplicationExit(object sender, EventArgs e)
        {

            //Properties.Settings.Default.Save();

        }
        private void RegisterLua()
        {

            lua.RegisterFunction("vault", this, new dvault(luaVault).Method);
            lua.RegisterFunction("tip", this, new dtip(luatip).Method);
            lua.RegisterFunction("print", this, new LogConsole(luaPrint).Method);
            lua.RegisterFunction("stop", this, new dStop(luaStop).Method);
            lua.RegisterFunction("resetseed", this, new dResetSeed(luaResetSeed).Method);
            lua.RegisterFunction("resetstats", this, new dResetStat(luaResetStat).Method);
            //lua.RegisterFunction("resetchips", this, new dResetChip(luaResetChip).Method);

            lua.RegisterFunction("startNewGameEngine", this, new dStartNewGameEngine(startNewGameEngine).Method);

        }

        private void SetLuaVariables(decimal profitCurr)
        {
            lua["balance"] = currentBal;
            lua["profit"] = currentProfit;
            lua["currentstreak"] = (winstreak > 0) ? winstreak : -losestreak;
            lua["previousbet"] = Lastbet;
            lua["bets"] = wins + losses;
            lua["wins"] = wins;
            lua["losses"] = losses;
            lua["currency"] = currencySelected;
            lua["wagered"] = currentWager;
            lua["win"] = isWin;

            lua["lastBet"] = last;
            lua["currentprofit"] = profitCurr;
        }

        private void UnSetVariables()
        {
            lua["balance"] = null;
            lua["chips"] = null;
        }

        private void GetLuaVariables()
        {
            try
            {
                currencySelected = (string)lua["currency"];
            }
            catch (Exception e)
            {
                ready = false;
                bSta();
                luaPrint("Please set 'currency = x'");
            }

            number.Clear();
            color.Clear();
            row.Clear();
            range.Clear();
            parity.Clear();

            try
            {


                LuaTable tbl_numbers = lua.GetTable("chips");
                System.Collections.Specialized.ListDictionary dict_numbers = lua.GetTableDict(tbl_numbers);

                foreach (DictionaryEntry s in dict_numbers)
                {
                    string val1 = "";
                    decimal amt = 0;
                    LuaTable tbl1 = (LuaTable)s.Value;
                    System.Collections.Specialized.ListDictionary dict1 = lua.GetTableDict(tbl1);

                    foreach (DictionaryEntry i in dict1)
                    {
                        if ((string)i.Key == "value")
                        {
                            val1 = Convert.ToString(i.Value);

                        }
                        if ((string)i.Key == "amount")
                        {
                            amt = Convert.ToDecimal(i.Value);
                        }
                    }
                    if (val1.Contains("number"))
                    {
                        number.Add(new Values()
                        {
                            value = val1,
                            amount = amt
                        });
                    }
                    if (val1.Contains("color"))
                    {
                        color.Add(new Values()
                        {
                            value = val1,
                            amount = amt
                        });
                    }
                    if (val1.Contains("row"))
                    {
                        row.Add(new Values()
                        {
                            value = val1,
                            amount = amt
                        });
                    }
                    if (val1.Contains("parity"))
                    {
                        parity.Add(new Values()
                        {
                            value = val1,
                            amount = amt
                        });
                    }
                    if (val1.Contains("range"))
                    {
                        range.Add(new Values()
                        {
                            value = val1,
                            amount = amt
                        });
                    }

                }
            }
            catch (Exception e)
            {

            }

        }
        public void bSta()
        {
            running = false;
            button1.Enabled = true;
            //comboBox1.Enabled = true;

            authorizeControl1.CanChangeCurrency(true);

            button1.Text = "Start";
        }

        public void Log(Data response)
        {
            string[] row = { response.data.rouletteBet.id, String.Format("{0}x|{1}", response.data.rouletteBet.payoutMultiplier.ToString("0.00"), response.data.rouletteBet.state.result.ToString()), response.data.rouletteBet.amount.ToString("0.00000000") + " " + currencySelected, (response.data.rouletteBet.payout - response.data.rouletteBet.amount).ToString("0.00000000"), response.data.rouletteBet.game };
            var log = new ListViewItem(row);
            listView1.Items.Insert(0, log);
            if (listView1.Items.Count > 15)
            {
                listView1.Items[listView1.Items.Count - 1].Remove();
            }
            if (response.data.rouletteBet.payoutMultiplier >= 1)
            {
                log.BackColor = Color.FromArgb(170, 250, 190);
            }
            else
            {
                log.BackColor = Color.FromArgb(250, 185, 170);
            }
        }

        private void SetStatistics()
        {
            balanceLabel.Text = String.Format("{0} {1}", currentBal.ToString("0.00000000"), currencySelected);
            profitLabel.Text = currentProfit.ToString("0.00000000");
            wagerLabel.Text = currentWager.ToString("0.00000000");
            wltLabel.Text = String.Format("{0} / {1} / {2}", wins.ToString(), losses.ToString(), (wins + losses).ToString());
            currentStreakLabel.Text = String.Format("{0} / {1} / {2}", (winstreak > 0) ? winstreak.ToString() : (-losestreak).ToString(), highestStreak.Max().ToString(), lowestStreak.Min().ToString());
            lowestProfitLabel.Text = lowestProfit.Min().ToString("0.00000000");
            highestProfitLabel.Text = highestProfit.Max().ToString("0.00000000");
            highestBetLabel.Text = highestBet.Max().ToString("0.00000000");
        }
        void luaStop()
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                luaPrint("Called stop.");
                running = false;
                sim = false;
                bSta();
            });

        }

        void luaResetChip()
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                color.Clear();
                row.Clear();
                number.Clear();
                parity.Clear();
                range.Clear();
                //GetLuaVariables();
            });

        }
        void luaVault(decimal sentamount)
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                VaultSend(sentamount);
            });
        }
        void luatip(string user, decimal amount)
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                luaPrint("Tipping not available.");

            });
        }
        void luaPrint(string text)
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                richTextBox2.AppendText(text + "\r\n");
            });

        }
        void luaResetSeed()
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                ResetSeeds();
            });
        }

        void luaResetStat()
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                currentProfit = 0;
                currentWager = 0;
                wins = 0;
                losses = 0;
                winstreak = 0;
                losestreak = 0;
                lowestStreak = new List<int> { 0 };
                highestStreak = new List<int> { 0 };
                highestProfit = new List<decimal> { 0 };
                lowestProfit = new List<decimal> { 0 };
                highestBet = new List<decimal> { 0 };
                wltLabel.Text = "0 / 0 / 0";
                currentStreakLabel.Text = "0 / 0 / 0";
                profitLabel.Text = currentProfit.ToString("0.00000000");
                wagerLabel.Text = currentWager.ToString("0.00000000");
                wltLabel.Text = String.Format("{0} / {1} / {2}", wins.ToString(), losses.ToString(), (wins + losses).ToString());
                currentStreakLabel.Text = String.Format("{0} / {1} / {2}", (winstreak > 0) ? winstreak.ToString() : (-losestreak).ToString(), highestStreak.Max().ToString(), lowestStreak.Min().ToString());
                lowestProfitLabel.Text = lowestProfit.Min().ToString("0.00000000");
                highestProfitLabel.Text = highestProfit.Max().ToString("0.00000000");
                highestBetLabel.Text = highestBet.Max().ToString("0.00000000");
            });
        }





        private void LogButton_Click(object sender, EventArgs e)
        {

        }

        //private async void textBox1_TextChanged(object sender, EventArgs e)
        //{
        //    token = textBox1.Text;
        //    //Properties.Settings.Default.token = token;
        //    if (token.Length == 96)
        //    {
        //        // Connect();
        //        await Authorize();

        //    }
        //    else
        //    {
        //        toolStripStatusLabel1.Text = "Disconnected";
        //    }
        //}

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //currencySelected = currenciesAvailable[comboBox1.SelectedIndex].ToLower();
            //// Properties.Settings.Default.indexCurrency = comboBox1.SelectedIndex;
            //string[] current = comboBox1.Text.Split(' ');
            //if (current.Length > 1)
            //{
            //    balanceLabel.Text = current[1] + " " + currencySelected;
            //}
        }

        private void SiteComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //StakeSite = SiteComboBox2.Text.ToLower();
            // Properties.Settings.Default.indexSite = SiteComboBox2.SelectedIndex;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (running == false)
            {
                ready = true;
                RegisterLua();
                await CheckBalance();

                try
                {
                    UnSetVariables();
                    SetLuaVariables(0);
                    LuaRuntime.SetLua(lua);
                    LuaRuntime.Run(luaCodeBox.Text);
                }
                catch (Exception ex)
                {
                    luaPrint("Lua ERROR!!");
                    luaPrint(ex.Message);
                    running = false;
                    bSta();
                }

                GetLuaVariables();

                authorizeControl1.SetCurrency(currencySelected);

                if (ready == true)
                {
                    button1.Enabled = false;
                    running = true;
                    button1.Text = "Stop";
                    //comboBox1.Enabled = false;
                    authorizeControl1.CanChangeCurrency(false);
                    beginMs = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                    StartBet();
                }
                else
                {
                    bSta();
                }


            }
            else
            {
                running = false;
                bSta();
            }
        }
        async Task StartBet()
        {
            while (running == true)
            {
                if (beginMs == 0)
                {
                    beginMs = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                }
                await RouletteBet();
            }
        }

        /*
        private async Task Balances()
        {

            messagePayload messagePayload2 = new messagePayload();
            messagePayload2.accessToken = token;
            messagePayload2.query = "subscription AvailableBalances {\n  availableBalances {\n    amount\n    identifier\n    balance {\n      amount\n      currency\n    }\n  }\n}\n";
            messageData messageData2 = new messageData();
            messageData2.id = "6cc429c1-a18a-4a6a-819e-1c78c724b5f8";
            messageData2.type = "subscribe";
            messageData2.payload = messagePayload2;
            this.chat_socket.Send(JsonConvert.SerializeObject(messageData2));

        }
        public void Connect()
        {
            try
            {
                Debug.WriteLine(StakeSite);
                this.chat_socket = new WebSocket("wss://api." + StakeSite + "/websockets", "graphql-transport-ws", new List<KeyValuePair<string, string>>()
                {
                     new KeyValuePair<string, string>("jwt", token)
                }, userAgent: "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.163 Safari/537.36", origin: "https://" + StakeSite, version: WebSocketVersion.Rfc6455, sslProtocols: SslProtocols.Tls12);
                this.chat_socket.EnableAutoSendPing = true;
                this.chat_socket.AutoSendPingInterval = 1000;
                //this.lastmessage = DateTime.UtcNow;
                this.chat_socket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(this.chat_socket_MessageReceived);
                this.chat_socket.Opened += new EventHandler(this.chat_socket_Opened);
                this.chat_socket.Error += new EventHandler<ErrorEventArgs>(this.chat_socket_Error);
                this.chat_socket.Closed += new EventHandler(this.chat_socket_Closed);
                this.chat_socket.Open();
            }
            catch (Exception ex)
            {
                //Bsta();
                Debug.WriteLine(ex.ToString());

            }


        }

        private async void chat_socket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            messageData _msg = JsonConvert.DeserializeObject<messageData>(e.Message);
            string type = _msg.type;

            if (type == "connection_ack")
            {

                is_connected = true;

                await Balances();
            }
            else
            {

                if (type == "next")
                {
                    if (_msg.payload.errors.Count > 0)
                    {
                        if (_msg.payload.errors[0].message.Contains("invalid") || _msg.payload.errors[0].message.Contains("expired"))
                        {
                            this.Invoke((MethodInvoker)delegate ()
                            {
                                this.chat_socket.Close();
                            });
                        }
                    }
                    else
                    {
                        if (_msg.payload.data.availableBalances != null)
                        {
                            this.Invoke((MethodInvoker)delegate ()
                            {
                                if (_msg.payload.data.availableBalances.balance.currency == currencySelected.ToLower())
                                {
                                    currentBal = _msg.payload.data.availableBalances.balance.amount;

                                    try
                                    {
                                        lua["balance"] = currentBal;
                                        LuaRuntime.SetLua(lua);
                                    }
                                    catch (Exception ex)
                                    {
                                        luaPrint("Lua ERROR!!");
                                        luaPrint(ex.Message);
                                        running = false;
                                        bSta();
                                    }
                                    balanceLabel.Text = String.Format("{0} {1}", currentBal.ToString("0.00000000"), currencySelected);

                                }

                                var index = Array.FindIndex(curr, row => row.Contains(_msg.payload.data.availableBalances.balance.currency.ToUpper()));
                                comboBox1.Items[index] = string.Format("{0} {1}", curr[index], _msg.payload.data.availableBalances.balance.amount.ToString("0.00000000"));





                            });

                        }
                    }


                }
                else
                {

                }

            }
        }

        private void chat_socket_Opened(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabel1.Text = string.Format("{0}", "Connected");
                this.chat_socket.Send(JsonConvert.SerializeObject(new messageData()
                {
                    type = "connection_init",
                    payload = new messagePayload()
                    {
                        accessToken = token,
                        language = "en"
                    }
                }));

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void chat_socket_Error(object sender, ErrorEventArgs e)
        {
            try
            {
                //Bsta();
                Debug.WriteLine(e.Exception);
            }
            catch (Exception ex)
            {
                //Bsta();
                Debug.WriteLine(ex.Message);
            }
        }

        private async void chat_socket_Closed(object sender, EventArgs e)
        {
            try
            {


                if (!this.is_connected)
                    return;
                //this._from_chat_close = true;
                await Task.Delay(400);
                this.Invoke((MethodInvoker)delegate ()
                {
                    toolStripStatusLabel1.Text = string.Format("{0}", "Re-connecting...");
                });
                await Task.Delay(1000);
                this.chat_socket.Open();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        */



        private void RefreshBalances(List<Balances> balances)
        {
            try
            {
                for (var i = 0; i < balances.Count; i++)
                {
                    if (balances[i].available.currency == currencySelected.ToLower())
                    {
                        currentBal = balances[i].available.amount;
                        balanceLabel.Text = String.Format("{0} {1}", currentBal.ToString("0.00000000"), currencySelected);
                    }
                    //  var index = Array.FindIndex(currenciesAvailable, row => row.Contains(balances[i].available.currency.ToLower()));
                    //  if (index != -1)
                    //  {
                    //       currencySelector.Items[index] = string.Format("{0} {1}", currenciesAvailable[index], balances[i].available.amount.ToString("0.00000000"));
                    //   }
                }

            }
            catch (Exception)
            {
            }
        }
        public async Task RouletteBet()
        {
            try
            {
                if (running)
                {

                    var restResponse = await APIClientManager.PlaceRouletteBet(currencySelected);

                    button1.Enabled = true;

                    Data response = JsonConvert.DeserializeObject<Data>(restResponse.Content);

                    if (response.errors != null)
                    {
                        luaPrint(String.Format("{0}:{1}", response.errors[0].errorType, response.errors[0].message));

                        //if(response.errors[0].errorType == "graphQL")

                        if (running == true)
                        {
                            await Task.Delay(2000);

                        }
                        else
                        {
                            running = false;
                            bSta();
                        }
                    }
                    else
                    {
                        TimerFunc(beginMs);

                        currentWager += response.data.rouletteBet.amount;
                        if (response.data.rouletteBet.payoutMultiplier >= 1)
                        {
                            losestreak = 0;
                            winstreak++;
                            isWin = true;
                            wins++;
                            //ResultLabeL.ForeColor = Color.Gold;
                        }
                        else
                        {
                            losestreak++;
                            winstreak = 0;
                            isWin = false;
                            losses++;
                            //ResultLabeL.ForeColor = Color.Black;

                        }
                        ResultLabeL.ForeColor = colorTable[(int)response.data.rouletteBet.state.result];
                        Log(response);
                        //CheckBalance();
                        List<string> number11 = new List<string>();
                        List<string> colors11 = new List<string>();
                        List<string> parity11 = new List<string>();
                        List<string> range11 = new List<string>();
                        List<string> rows11 = new List<string>(); ;

                        foreach (Values i in response.data.rouletteBet.state.numbers)
                        {
                            number11.Add(i.value.ToString().Replace("number", ""));
                        }

                        numsLabel.Text = String.Format("[{0}]", string.Join(",", number11));

                        foreach (Values i in response.data.rouletteBet.state.colors)
                        {
                            colors11.Add(i.value.ToString().Replace("color", ""));
                        }
                        colorsLabel.Text = String.Format("[{0}]", string.Join(",", colors11));
                        foreach (Values i in response.data.rouletteBet.state.parities)
                        {
                            parity11.Add(i.value.ToString().Replace("parity", ""));
                        }
                        parityLabel.Text = String.Format("[{0}]", string.Join(",", parity11));
                        foreach (Values i in response.data.rouletteBet.state.ranges)
                        {
                            range11.Add(i.value.ToString());
                        }
                        rangesLabel.Text = String.Format("[{0}]", string.Join(",", range11));
                        foreach (Values i in response.data.rouletteBet.state.rows)
                        {
                            rows11.Add(i.value.ToString());
                        }
                        rowsLabel.Text = String.Format("[{0}]", string.Join(",", rows11));

                        decimal profitCurr = response.data.rouletteBet.payout - response.data.rouletteBet.amount;
                        currentProfit += response.data.rouletteBet.payout - response.data.rouletteBet.amount;
                        //profitLabel.Text = currentProfit.ToString("0.00000000");
                        //TargetLabeL.Text = response.data.rouletteBet.state.multiplierTarget.ToString("0.00") + "x";
                        ResultLabeL.Text = response.data.rouletteBet.state.result.ToString();

                        //last.target = response.data.rouletteBet.state.multiplierTarget;
                        last.result = response.data.rouletteBet.state.result;
                        last.multiplier = response.data.rouletteBet.payoutMultiplier;

                        highestStreak.Add(winstreak);
                        highestStreak = new List<int> { highestStreak.Max() };
                        lowestStreak.Add(-losestreak);
                        lowestStreak = new List<int> { lowestStreak.Min() };

                        if (currentProfit < 0)
                        {
                            lowestProfit.Add(currentProfit);
                            lowestProfit = new List<decimal> { lowestProfit.Min() };
                        }
                        else
                        {
                            highestProfit.Add(currentProfit);
                            highestProfit = new List<decimal> { highestProfit.Max() };
                        }

                        highestBet.Add(amount);
                        highestBet = new List<decimal> { highestBet.Max() };

                        SetStatistics();

                        try
                        {
                            SetLuaVariables(profitCurr);
                            LuaRuntime.SetLua(lua);


                            LuaRuntime.Run("dobet()");

                        }
                        catch (Exception ex)
                        {
                            luaPrint("Lua ERROR!!");
                            luaPrint(ex.Message);
                            running = false;
                            bSta();
                        }
                        GetLuaVariables();
                    }
                }
            }
            catch (Exception ex)
            {
                //luaPrint(ex.Message);
            }
        }


        public async Task CheckBalance()
        {
            try
            {

                var restResponse = await APIClientManager.CheckBalance();

                //Debug.WriteLine(restResponse.Content);
                BalancesData response = JsonConvert.DeserializeObject<BalancesData>(restResponse.Content);

                if (response.errors != null)
                {

                }
                else
                {
                    if (response.data != null)
                    {

                        for (var i = 0; i < response.data.user.balances.Count; i++)
                        {
                            if (response.data.user.balances[i].available.currency == currencySelected.ToLower())
                            {
                                currentBal = response.data.user.balances[i].available.amount;
                                balanceLabel.Text = String.Format("{0} {1}", currentBal.ToString("0.00000000"), currencySelected);
                            }
                            //if (true)
                            //{
                            //    for (int s = 0; s < currenciesAvailable.Length; s++)
                            //    {
                            //        if (response.data.user.balances[i].available.currency == currenciesAvailable[s].ToLower())
                            //        {
                            //            comboBox1.Items[s] = string.Format("{0} {1}", currenciesAvailable[s], response.data.user.balances[i].available.amount.ToString("0.00000000"));

                            //            break;
                            //        }
                            //    }
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //luaPrint(ex.Message);
            }

        }





        public void TimerFunc(long begin)
        {
            decimal diff = (decimal)((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - begin);
            decimal seconds = Math.Floor((diff / 1000) % 60);
            decimal minutes = Math.Floor((diff / (1000 * 60)) % 60);
            decimal hours = Math.Floor((diff / (1000 * 60 * 60)));

            Time.Text = String.Format("{0} : {1} : {2}", hours, minutes, seconds);
        }
        //private void clearLinkbtn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    textBox1.Clear();
        //    textBox1.Enabled = true;
        //    token = "";
        //    toolStripStatusLabel1.Text = "Disconnected";
        //}

        //private async void CheckBtn_Click(object sender, EventArgs e)
        //{
        //    CheckBtn.Enabled = false;
        //    await CheckBalance();
        //    CheckBtn.Enabled = true;
        //}

        public string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            //Properties.Settings.Default.textCode = richTextBox1.Text;
        }
        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            if (richTextBox2.Lines.Length > 200)
            {
                List<string> lines = richTextBox2.Lines.ToList();
                lines.RemoveAt(0);
                richTextBox2.Lines = lines.ToArray();
            }

            richTextBox2.SelectionStart = richTextBox2.Text.Length;
            richTextBox2.ScrollToCaret();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            currentProfit = 0;
            currentWager = 0;
            wins = 0;
            losses = 0;
            winstreak = 0;
            losestreak = 0;
            lowestStreak = new List<int> { 0 };
            highestStreak = new List<int> { 0 };
            highestProfit = new List<decimal> { 0 };
            lowestProfit = new List<decimal> { 0 };
            highestBet = new List<decimal> { 0 };
            beginMs = 0;
            Time.Text = "0 : 0 : 0";
            wltLabel.Text = "0 / 0 / 0";
            currentStreakLabel.Text = "0 / 0 / 0";
            counter = 0;
            yList.Clear();
            xList.Clear();
            xList.Add(0);
            yList.Add(0);

            profitLabel.Text = currentProfit.ToString("0.00000000");
            wagerLabel.Text = currentWager.ToString("0.00000000");
            wltLabel.Text = String.Format("{0} / {1} / {2}", wins.ToString(), losses.ToString(), (wins + losses).ToString());
            currentStreakLabel.Text = String.Format("{0} / {1} / {2}", (winstreak > 0) ? winstreak.ToString() : (-losestreak).ToString(), highestStreak.Max().ToString(), lowestStreak.Min().ToString());
            lowestProfitLabel.Text = lowestProfit.Min().ToString("0.00000000");
            highestProfitLabel.Text = highestProfit.Max().ToString("0.00000000");
            highestBetLabel.Text = highestBet.Max().ToString("0.00000000");
        }

        private void CommandButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (CommandBox2.Text.Length > 0)
                {
                    LuaRuntime.Run(CommandBox2.Text);
                }
            }
            catch (Exception ex)
            {
                luaPrint("Lua ERROR!!");
                luaPrint(ex.Message);
                running = false;
                bSta();
            }
        }



        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            //
            ListViewItem item = e.Item as ListViewItem;
            if (e.Item.Checked == true)
            {
                Process.Start(new ProcessStartInfo(string.Format("https://{1}/casino/home?betId={0}&modal=bet", e.Item.Text, GameDomain)) { UseShellExecute = true });
            }
        }

        private void CmdBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CommandButton2_Click(this, new EventArgs());
            }
        }



        private async Task VaultSend(decimal amount)
        {
            try
            {

                var restResponse = await APIClientManager.SendToVault(currencySelected, amount);

                Data response = JsonConvert.DeserializeObject<Data>(restResponse.Content);

                if (response.errors != null)
                {
                    luaPrint(response.errors[0].errorType + ":" + response.errors[0].message);
                }
                else
                {
                    if (response.data != null)
                    {
                        luaPrint(string.Format("Deposited to vault: {0} {1}", amount.ToString("0.00000000"), currencySelected));
                    }
                }
            }
            catch (Exception ex)
            {
                //luaPrint(ex.Message);
            }
        }

        private async Task ResetSeeds()
        {
            try
            {

                var restResponse = await APIClientManager.ResetSeeds();

                // Will output the HTML contents of the requested page
                //Debug.WriteLine(restResponse.Content);
                Data response = JsonConvert.DeserializeObject<Data>(restResponse.Content);

                //System.Diagnostics.Debug.WriteLine(restResponse.Content);
                if (response.errors != null)
                {
                    luaPrint(response.errors[0].errorType + ":" + response.errors[0].message);
                }
                else
                {
                    if (response.data != null)
                    {
                        luaPrint("Seed was reset.");

                    }

                }
            }
            catch (Exception ex)
            {
                //luaPrint(ex.Message);
            }
        }

        private async Task SendTip()
        {
            try
            {

                var restResponse = await APIClientManager.SendTip();

                // Will output the HTML contents of the requested page
                //Debug.WriteLine(restResponse.Content);
                Data response = JsonConvert.DeserializeObject<Data>(restResponse.Content);

                //System.Diagnostics.Debug.WriteLine(restResponse.Content);
                if (response.errors != null)
                {
                    luaPrint(response.errors[0].errorType + ":" + response.errors[0].message);
                }
                else
                {
                    if (response.data != null)
                    {
                        luaPrint("Not functional.");

                    }

                }
            }
            catch (Exception ex)
            {
                //luaPrint(ex.Message);
            }
        }

        public async Task<bool> Authorize()
        {
            try
            {

                var restResponse = await APIClientManager.Authorize();

                ActiveData response = JsonConvert.DeserializeObject<ActiveData>(restResponse.Content);

                if (response.errors != null)
                {
                    toolStripStatusLabel1.Text = "Disconnected";
                }
                else
                {
                    if (response.data != null)
                    {
                        toolStripStatusLabel1.Text = String.Format("Connected");

                        //textBox1.Enabled = false;
                        authorizeControl1.CanChangeAuthorization(false);

                        for (var i = 0; i < response.data.user.balances.Count; i++)
                        {
                            if (response.data.user.balances[i].available.currency == currencySelected.ToLower())
                            {
                                currentBal = response.data.user.balances[i].available.amount;
                                balanceLabel.Text = String.Format("{0} {1}", currentBal.ToString("0.00000000"), currencySelected);
                            }
                        }

                        authorizeControl1.SyncCurrencies(response.data.user.balances);

                    }

                }
            }
            catch (Exception ex)
            {
                //luaPrint(ex.Message);
            }

            return true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*
            textBox1.Text = Properties.Settings.Default.token;
            comboBox1.SelectedIndex = Properties.Settings.Default.indexCurrency;
            SiteComboBox2.SelectedIndex = Properties.Settings.Default.indexSite;
            ServerSeedBox.Text = Properties.Settings.Default.serverSeed;
            ClientSeedBox.Text = Properties.Settings.Default.clientSeed;
            NonceBox.Text = Properties.Settings.Default.nonce.ToString();
            NonceStopBox.Text = Properties.Settings.Default.nonceStop.ToString();
            */
        }



        private void SimulateRun()
        {
            while (sim == true)
            {
                if (nonce > stopNonce || balanceSim < amount || target <= 1)
                {
                    if (target <= 1)
                    {
                        luaPrint("Lua ERROR!!");
                        luaPrint("Lua: Target must be above 1.");
                    }
                    SimulateButton_Click_1(this, new EventArgs());
                    sim = false;
                    break;
                }
                decimal result = LimboResult(serverSeed, clientSeed, nonce);
                nonce += 1;

                decimal payout = 0;
                decimal payoutMultiplier = 0;
                currentWager += amount;
                string winStatus = "lose";
                if (result > (decimal)target)
                {
                    losestreak = 0;
                    winstreak++;
                    isWin = true;
                    wins++;
                    payout = (decimal)target * amount;
                    payoutMultiplier = (decimal)target;
                    winStatus = "win";
                    ResultLabeL.ForeColor = Color.LimeGreen;
                }
                else
                {
                    losestreak++;
                    winstreak = 0;
                    isWin = false;
                    losses++;
                    ResultLabeL.ForeColor = Color.Red;
                }

                decimal profitCurr = payout - amount;
                currentProfit += payout - amount;
                balanceSim += payout - amount;
                //profitLabel.Text = currentProfit.ToString("0.00000000");
                //TargetLabeL.Text = target.ToString("0.00") + "x";
                ResultLabeL.Text = result.ToString("0.00") + "x";

                //last.target = target;
                last.result = (double)result;


                highestStreak.Add(winstreak);
                highestStreak = new List<int> { highestStreak.Max() };
                lowestStreak.Add(-losestreak);
                lowestStreak = new List<int> { lowestStreak.Min() };

                if (currentProfit < 0)
                {
                    lowestProfit.Add(currentProfit);
                    lowestProfit = new List<decimal> { lowestProfit.Min() };
                }
                else
                {
                    highestProfit.Add(currentProfit);
                    highestProfit = new List<decimal> { highestProfit.Max() };
                }

                highestBet.Add(amount);
                highestBet = new List<decimal> { highestBet.Max() };
                this.Invoke((MethodInvoker)delegate ()
                {
                    balanceLabel.Text = String.Format("{0}", balanceSim.ToString("0.00000000"));
                    profitLabel.Text = currentProfit.ToString("0.00000000");
                    wagerLabel.Text = currentWager.ToString("0.00000000");
                    wltLabel.Text = String.Format("{0} / {1} / {2}", wins.ToString(), losses.ToString(), (wins + losses).ToString());
                    currentStreakLabel.Text = String.Format("{0} / {1} / {2}", (winstreak > 0) ? winstreak.ToString() : (-losestreak).ToString(), highestStreak.Max().ToString(), lowestStreak.Min().ToString());
                    lowestProfitLabel.Text = lowestProfit.Min().ToString("0.00000000");
                    highestProfitLabel.Text = highestProfit.Max().ToString("0.00000000");
                    highestBetLabel.Text = highestBet.Max().ToString("0.00000000");
                    Application.DoEvents();
                });
                //SetStatistics();
                string box = String.Format("[{0}] {4}x  |  {1}   |  bet: {5}  |  profit:  {2}   [{3}]", nonce - 1, result.ToString("0.0000"), currentProfit.ToString("0.00000000"), winStatus, target.ToString("0.00"), amount.ToString("0.00000000"));
                listBox3.Items.Insert(0, box);
                if (listBox3.Items.Count > 200)
                {
                    listBox3.Items.RemoveAt(listBox3.Items.Count - 1);
                }
                try
                {
                    lua["balance"] = balanceSim;
                    lua["profit"] = currentProfit;
                    lua["currentstreak"] = (winstreak > 0) ? winstreak : -losestreak;
                    lua["previousbet"] = Lastbet;
                    lua["bets"] = wins + losses;
                    lua["wins"] = wins;
                    lua["losses"] = losses;
                    lua["currency"] = currencySelected;
                    lua["wagered"] = currentWager;
                    lua["win"] = isWin;

                    lua["lastBet"] = last;
                    lua["currentprofit"] = profitCurr;
                    LuaRuntime.SetLua(lua);


                    LuaRuntime.Run("dobet()");

                }
                catch (Exception ex)
                {
                    luaPrint("Lua ERROR!!");
                    luaPrint(ex.Message);
                    sim = false;
                }
                Lastbet = (decimal)(double)lua["nextbet"];
                amount = Lastbet;
                currencySelected = (string)lua["currency"];
                target = (double)lua["target"];

            }
        }
        static decimal LimboResult(string serverSeed, string clientSeed, int nonce)
        {
            string nonceSeed = string.Format("{0}:{1}:{2}", clientSeed, nonce, 0);

            string hex = HmacSha256Digest(nonceSeed, serverSeed);
            decimal end = 0;
            for (int i = 0; i < 4; i++)
            {
                end += (decimal)(Convert.ToInt32(hex.Substring(i * 2, 2), 16) / Math.Pow(256, i + 1));
            }
            end *= 16777216;
            end = 16777216 / (Math.Floor(end) + 1) * (decimal)(1 - 0.01);
            return end;
        }

        public static string HmacSha256Digest(string message, string secret)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] keyBytes = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            System.Security.Cryptography.HMACSHA256 cryptographer = new System.Security.Cryptography.HMACSHA256(keyBytes);

            byte[] bytes = cryptographer.ComputeHash(messageBytes);

            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        private void ServerSeedBox_TextChanged(object sender, EventArgs e)
        {
            serverSeed = ServerSeedBox.Text;
            //Properties.Settings.Default.serverSeed = serverSeed;
        }

        private void ClientSeedBox_TextChanged(object sender, EventArgs e)
        {
            clientSeed = ClientSeedBox.Text;
            //Properties.Settings.Default.clientSeed = clientSeed;
        }

        private void NonceBox_TextChanged(object sender, EventArgs e)
        {
            nonce = Int32.Parse(NonceBox.Text);
            //Properties.Settings.Default.nonce = nonce;
        }

        private void SimulateButton_Click_1(object sender, EventArgs e)
        {
            if (SimulateButton.Text.Contains("Off"))
            {
                SimulateButton.Text = "Simulate";
                sim = false;
            }
            else
            {
                nonce = Int32.Parse(NonceBox.Text);
                serverSeed = ServerSeedBox.Text;
                clientSeed = ClientSeedBox.Text;
                linkLabel1_LinkClicked(this, new LinkLabelLinkClickedEventArgs(new LinkLabel.Link()));
                SimulateButton.Text = "Off";
                sim = true;
                RegisterSim();
                lua["balance"] = null;
                lua["nextbet"] = null;
                //lua["target"] = null;
                try
                {

                    lua["profit"] = currentProfit;
                    lua["currentstreak"] = (winstreak > 0) ? winstreak : -losestreak;
                    lua["previousbet"] = Lastbet;
                    lua["bets"] = wins + losses;
                    lua["wins"] = wins;
                    lua["losses"] = losses;
                    lua["currency"] = currencySelected;
                    lua["wagered"] = currentWager;
                    lua["win"] = isWin;
                    lua["lastBet"] = last;
                    LuaRuntime.SetLua(lua);


                    LuaRuntime.Run(luaCodeBox.Text);


                }
                catch (Exception ex)
                {
                    luaPrint("Lua ERROR!!");
                    luaPrint(ex.Message);
                    sim = false;
                }


                try
                {
                    Lastbet = (decimal)(double)lua["nextbet"];
                    amount = Lastbet;
                    currencySelected = (string)lua["currency"];
                    target = (double)lua["target"];
                    balanceSim = (decimal)(double)lua["balance"];
                }
                catch (Exception ex)
                {
                    luaPrint("Please set 'balance = x' and 'target = x' and 'nextbet = x' variable on top of script.");
                    SimulateButton_Click_1(this, new EventArgs());
                    return;
                }

                balanceLabel.Text = String.Format("{0}", balanceSim.ToString("0.00000000"));
                Task.Run(() => SimulateRun());
            }
        }



        private void wltLabel_Click(object sender, EventArgs e)
        {

        }

        private void NonceStopBox_TextChanged(object sender, EventArgs e)
        {
            stopNonce = Int32.Parse(NonceStopBox.Text);
            // Properties.Settings.Default.nonceStop = stopNonce;
        }
        private void listBox3_Click(object sender, EventArgs e)
        {
            //listBox3.ClearSelected();
        }
        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ResetBoxSeed_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listBox3.Items.Clear();
        }

        private void RegisterSim()
        {

            lua.RegisterFunction("vault", this, new dVaultEmpty(EmptyVaultFunc).Method);
            lua.RegisterFunction("tip", this, new dTipEmpty(EmptyTipFunc).Method);
            lua.RegisterFunction("print", this, new LogConsole(luaPrint).Method);
            lua.RegisterFunction("stop", this, new dStop(luaStop).Method);
            lua.RegisterFunction("resetseed", this, new dSeedEmpty(EmptySeedFunc).Method);
            lua.RegisterFunction("resetstats", this, new dResetStat(luaResetStat).Method);
        }

        public void EmptySeedFunc()
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                luaPrint("Function not available in simulation. (resetseed)");

            });
        }
        public void EmptyVaultFunc(decimal amount)
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                luaPrint("Function not available in simulation. (vault)");

            });
        }
        public void EmptyTipFunc(string user, decimal amount)
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                luaPrint("Function not available in simulation. (tip)");

            });
        }

        private void ResultLabeL_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            startNewGameEngine("LIMBO", "bnb", "strategies/strat_limbolua");
        }
    }

    public static class ListViewExtensions
    {
        /// <summary>
        /// Sets the double buffered property of a list view to the specified value
        /// </summary>
        /// <param name="listView">The List view</param>
        /// <param name="doubleBuffered">Double Buffered or not</param>
        public static void SetDoubleBuffered(this System.Windows.Forms.ListView listView, bool doubleBuffered = true)
        {
            listView
                .GetType()
                .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .SetValue(listView, doubleBuffered, null);
        }
    }

    public class lastbet
    {
        public double result { get; set; }
        public double multiplier { get; set; }
    }


}
