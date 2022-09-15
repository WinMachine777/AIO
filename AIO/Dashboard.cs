using AIO.Modules.Keno;
using AIO.Modules.Limbo;
using AIO.Modules.Roulette;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AIO
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var w = new LimboUI();
            w.Show(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var w = new KenoUI();
            w.Show(this);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var w = new RouletteUI();
            w.Show(this);
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }



        public void OpenNewGameEngine(object sender, string game, string apiKey, string mirror, string currency, string stratFile)
        {

            game = game.ToLower();

            string[] games = { "dice", "keno", "limbo", "roulette" };

            if (games.Contains(game))
            {
                switch (game.ToUpperInvariant())
                {
                    case "DICE":
                        //var dice = new KenoUI();
                        //dice.Show(this);
                        break;

                    case "KENO":
                        new KenoUI(this, game, apiKey, mirror, currency, stratFile);
                        break;

                    case "LIMBO":
                        new LimboUI(this, game, apiKey, mirror, currency, stratFile);
                        break;
                    case "ROULETTE":
                        new RouletteUI(this, game, apiKey, mirror, currency, stratFile);
                        break;

                    default:
                        break;
                }

                (sender as Form).Close();
            }
            else
            {
                throw new NotSupportedException();
            }

            /*
            switch (switch_on)
            {
                default:
            }*/
        }

    }
}
