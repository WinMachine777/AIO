using AIO.Modules.Keno;
using AIO.Modules.Limbo;
using AIO.Modules.Rouletee;
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
            w.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var w = new KenoUI();
            w.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var w = new RouleteeUI();
            w.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
