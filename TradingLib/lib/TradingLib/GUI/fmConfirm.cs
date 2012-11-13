using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradeLink.API;
namespace TradingLib.GUI
{
    public partial class fmConfirm : Form
    {
        public event VoidDelegate SendConfimEvent;
        public fmConfirm(string s)
        {
            InitializeComponent();
            label1.Text = s;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            confirm();

        }

        private void confirm()
        {

            if (SendConfimEvent != null)
            {
                SendConfimEvent();
            }
            Close();

        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        public static void show(string s)
        {
            fmConfirm fm = new fmConfirm(s);
            fm.ShowDialog();
        }
    }
}
