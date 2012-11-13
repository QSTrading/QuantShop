using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradeLink.API;
using TradingLib.GUI;

namespace TradingLib.GUI
{
    
    public partial class fmTimeSales : Form,GotTickIndicator
    {
        private Security _sec;

        
        public fmTimeSales(Security sec)
        {
            InitializeComponent();
            _sec = sec;
            ctTimeSales1.SendStyleChangeEvent += new VoidDelegate(ctTimeSales1_SendStyleChangeEvent);
            this.Focus();
        }

        void ctTimeSales1_SendStyleChangeEvent()
        {
            if (ctTimeSales1.IsSmall)
                Height = 224;
            else
                Height = 624;

        }

        public Security Security 
        {
            get { return _sec; }
            set 
            {
                _sec = value;
                ctTimeSales1.SetSeurity(_sec);
                Text = "Tick数据 " + _sec.FullName;
            }
        }

        public void GotTick(Tick k)
        {

            if (this.InvokeRequired)
            {
                try
                {
                    Invoke(new TickDelegate(GotTick), new object[] { k });
                }
                catch (ObjectDisposedException) { return; }
            }
            else
            {
                if (k.symbol != _sec.Symbol)
                    return;

                ctTimeSales1.GotTick(k);
            }
        
        }

        private void fmTimeSales_Load(object sender, EventArgs e)
        {

        }

        private void fmTimeSales_KeyDown(object sender, KeyEventArgs e)
        {
            MessageBox.Show("key is press:" + e.KeyCode.ToString());
            if (e.KeyCode == Keys.W)
                this.Close();
        }



        /*
        private void fmTimeSales_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (ctTimeSales1.IsSmall)
                ctTimeSales1.IsSmall = true;
            else
                ctTimeSales1.IsSmall = false;
        }
         * */

    }
}
