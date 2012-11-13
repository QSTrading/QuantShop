using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradeLink.API;
using TradeLink.Common;

namespace TradingLib.GUI.Server
{
    public partial class fmSrvDemoTick : Form
    {
        public event TickDelegate SendTickEvent;
        public event DebugDelegate SendDebugEvent;

        public fmSrvDemoTick()
        {
            InitializeComponent();
        }

        private void debug(string msg)
        {
            if (SendDebugEvent != null)
                SendDebugEvent(msg);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Tick k = new TickImpl(sym.Text);
            if (tradeprice.Value > 0 && size.Text != string.Empty)
            {
                k.trade = tradeprice.Value;
                k.size = Convert.ToInt16(size.Text);
            }
            k.ask = (decimal)askprice.Value;
            k.bid = (decimal)bidprice.Value;
            k.os = Convert.ToInt16(asksize.Text);
            k.bs = Convert.ToInt16(bidsize.Text);
            DateTime t = DateTime.Now;

            k.time = Util.ToTLTime(t);
            k.date = Util.ToTLDate(t.Date);
            debug("time:" + k.time.ToString() + " | date:" + k.date.ToString());
            k.OpenInterest = 100;
            k.Vol = 100;
            k.PreOpenInterest = 100;
            k.PreSettlement = 100;
            k.Settlement = 100;
            k.Open = k.ask;
            k.High = k.ask;
            k.Low = k.ask;
            

            if (SendTickEvent != null)
                SendTickEvent(k);
        }
    }
}
