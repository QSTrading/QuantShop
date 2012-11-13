using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradeLink.API;
using WeifenLuo.WinFormsUI.Docking;

namespace TradingLib.GUI
{
    public partial class demo : DockContent
    {
        public event DebugDelegate SendDebugEvent;
        public demo()
        {
            InitializeComponent();
            ctTimesLineChart1.SendDebugEvent +=new DebugDelegate(debug);

        }
        public void debug(string msg)
        {
            if (SendDebugEvent != null)
            {
                SendDebugEvent(msg);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            debug("加载数据");
            //ctTimesLineChart1.loadData();
        }
    }
}
