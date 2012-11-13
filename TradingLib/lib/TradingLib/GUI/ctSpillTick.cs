using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using TradeLink.Common;
using TradeLink.API;
using TradeLink.AppKit;

namespace TradingLib.GUI
{
    public partial class SpillTick : UserControl
    {
        bool _assumenoordermod = true;
        public bool AssumeNewOrder { get { return _assumenoordermod; } set { _assumenoordermod = value; } }
   
        //public event LongDelegate SendCancleEvent;//发送取消
        public event OrderDelegate SendOrderEvent;//发送Order

        string _dispdecpointformat = "N" + ((int)2).ToString();
        bool touched = false;//鼠标是否在价格或者数量控件里面
        public event DebugDelegate SendDebugEvent;

        //本控件触发sendOrder事件,买入 卖出按钮通过触发sendOrder事件从而实现对其他组件的调用
        //外部使用该控件时,需要绑定其SendOrder
        //public event OrderDelegate SendOrder;

        private Security _sec;
        private Order work;
        public SpillTick()
        {
            InitializeComponent();

            price.MouseWheel += new MouseEventHandler(price_MouseWheel);
        }

        //spilltick根据最新的Tick数据更新界面
        public void SpillTick_GotTick()
        { 
            
        }

        private void debug(string s)
        {
            if (SendDebugEvent != null)
                SendDebugEvent(s);
        }

        public void setSecurity(Security sec)
        {
            symbol.Text = sec.Symbol;
            _sec = sec;
            work = new OrderImpl(_sec.Symbol, 0);
            work.ex = _sec.DestEx;
            work.LocalSymbol = _sec.Symbol;

        }
        public void GotTick(Tick tick)
        {
            /*
            if (this.InvokeRequired)
            {
                try
                {
                    Invoke(new TickDelegate(GotTick), new object[] { tick });
                }
                catch (ObjectDisposedException) { return; }
            }
            
            else */
          //  {
               if ((tick == null) || (tick.symbol != symbol.Text)) return;
                //if (touched) return;

               // decimal changedval = obuybut.Checked ? (limitbut.Checked ? tick.ask : tick.bid) : (limitbut.Checked ? tick.bid : tick.ask);
               // if (changedval != 0)
               // {
               //     if (this.oprice.InvokeRequired)
               //     {
               //         this.Invoke(new TickDelegate(newTick), new object[] { tick });
               //     }
               //     else oprice.Value = (decimal)changedval;
                if (tick.hasAsk) askPrice.Text = tick.ask.ToString(_dispdecpointformat);
                if (tick.hasBid) bidPrice.Text = tick.bid.ToString(_dispdecpointformat);

                //更新价格
                try
                {
                    if (tick.isTrade && !touched) price.Value = tick.trade;
                }
                catch(Exception e)
                {
                    debug(e.Message);
                }
                
                    //price.Value = tick.trade;
                //price.Value = tick.ask;

                //}
           // }
        
        }

        bool isValid()
        {
            bool castexcep = false;
            decimal p = 0;
            int s = 0;
            try
            {
                p = price.Value;
                s = (int)size.Value;
            }
            catch (InvalidCastException) { castexcep = true; }
            if (isMarket.Checked) p = 0;
            return (s > 0) && !castexcep && ((p > 0) || isMarket.Checked);
        }

        //买入操作
        private void butBuy_Click(object sender, EventArgs e)
        {
            genOrder(true);
        }

        //卖出
        private void butSell_Click(object sender, EventArgs e)
        {

            genOrder(false);
        }

        private void genOrder(bool f)
        {
            //debug("3333333333333333333333");
            if (!isValid()) return;
            //debug("22222222222222222222222");
            work.side = f;
            work.size = Math.Abs((int)size.Value);
            work.date = Util.ToTLDate(DateTime.Now);
            work.time = Util.ToTLTime(DateTime.Now);
            if (isMarket.Checked)
            {
                work.price = 0;
                work.stopp = 0;
            }
            else
            {
                bool islimit = isLimit.Checked;
                decimal limit = islimit ? price.Value : 0;
                decimal stop = !islimit ? price.Value : 0;
                work.price = limit;
                work.stopp = stop;
            }
            if (AssumeNewOrder)
                work.id = 0;
            debug("Order ID:"+work.id.ToString());
            if (SendOrderEvent != null) SendOrderEvent(work);
        }

        private void isMarket_CheckedChanged(object sender, EventArgs e)
        {
            if (isMarket.Checked == true)
            {
                isLimit.Checked = false;
                isStop.Checked = false;
                price.Enabled = false;
            }
        }

        private void isLimit_CheckedChanged(object sender, EventArgs e)
        {
            if (isLimit.Checked == true)
            {
                isMarket.Checked = false;
                isStop.Checked = false;
                price.Enabled = true;
            }
        }

        private void isStop_CheckedChanged(object sender, EventArgs e)
        {
            if (isStop.Checked == true)
            {
                isMarket.Checked = false;
                isLimit.Checked = false;
                price.Enabled = true;
            }
        }


        //鼠标滚轮调整价格与数量
        void price_MouseWheel(object sender, MouseEventArgs e)
        {
            touched = true;
            /*
            if (e.Button == MouseButtons.Middle) 
            {
                touched = false;
            }*/

        }

        private void price_KeyUp(object sender, KeyEventArgs e)
        {
            touched = true;
        }

        private void price_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            touched = false;
        }

        private void price_DoubleClick(object sender, EventArgs e)
        {
            touched = false;
        }






    }

    

}
