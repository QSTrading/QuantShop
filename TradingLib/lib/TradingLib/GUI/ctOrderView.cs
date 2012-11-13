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
using TradingLib.API;

namespace TradingLib.GUI
{
    public partial class OrderView : UserControl,IOrderWatcher
    {


        //该用户控件需要设定BrokerFeed,
        string _dispdecpointformat = "N" + ((int)2).ToString();
       
        private BrokerFeed _bf = null;

        private IBroker _broker = null;

        public OrderView()
        {
            InitializeComponent();
            //orderGrid.Rows.Add(new object[] { 12, "IF1209", "买入", 2,2305.2,0,"demo" });
            //orderGrid.Rows.Add(new object[] { 13, "IF1209", "买入", 2, 2305.2, 0, "demo" });
            //orderGrid.Rows.Add(new object[] { 14, "IF1209", "买入", 2, 2305.2, 0, "demo" });
            //orderGrid.Rows.Add(new object[] { 15, "IF1209", "买入", 2, 2305.2, 0, "demo" });
            initGrid();
            debug("orderView初始化");
            //ord.SendDebugEvent +=new DebugDelegate(debug);
        }

        public IBroker Broker
        {
            set { _broker = value; }
        
        }

        public event DebugDelegate SendDebugEvent;

        //QuoteView中的debug函数，通过触发sendDebugEvent来调用对应的函数
        void debug(string msg)
        {
            if (SendDebugEvent != null)
                SendDebugEvent(msg);

        }

        private void initGrid()
        {
            //orderGrid.hi
            orderGrid.ContextMenuStrip = new ContextMenuStrip();
            orderGrid.ContextMenuStrip.Items.Add("取消", null, new EventHandler(cancelorder));
        }


        //新建orderTracker用于储存并处理Order信息
        OrderTracker ord = new OrderTracker();

        //系统返回一个委托回报 处理order根据需要并插入新的Order更新orderGrid表格
        public void GotOrder(Order o)
        {
            if (InvokeRequired)
                Invoke(new OrderDelegate(GotOrder), new object[] { o });
            else
            {
                debug("orderView得到新的委托");
                //ord.GotOrder(o);
                //orderGrid.Rows.Add(new object[] { o.id, o.symbol, (o.side ? "BUY" : "SELL"), o.UnsignedSize, (o.price == 0 ? "Market" : o.price.ToString(_dispdecpointformat)), (o.stopp == 0 ? "" : o.stopp.ToString(_dispdecpointformat)), o.Account });
                //debug("orderidx:"+orderidx(o.id).ToString());
                if (orderidx(o.id) == -1) // if we don't have this order, add it
                    orderGrid.Rows.Add(new object[] { DateTime.Now.ToShortTimeString(), o.id, o.symbol, (o.side ? "买" : "卖"), getOrderType(o), o.UnsignedSize, 0, (o.price == 0 ? "市价" : o.price.ToString(_dispdecpointformat)), (o.stopp == 0 ? "" : o.stopp.ToString(_dispdecpointformat)), "TIF", "有效" });
            }
        }

        private string getOrderType(Order o)
        {
            if (o.isMarket) return "市价";
            if (o.isStop) return "追价";
            if (o.isLimit) return "限价";
            else
                return "";
        }

        //系统得到成交回报时 更新对应order
        public void GotFill(Trade t)
        {
           if (InvokeRequired)
               Invoke(new FillDelegate(GotFill), new object[] { t });
            else
            {
                debug("orderview got trade");
                ord.GotFill(t);
                if (!t.isValid) return;
                int oidx = orderidx(t.id); // get order id for this order
                if (oidx != -1)
                {
                    //int osign = (t.side ? 1 : -1);
                    //int signedtsize = t.xsize * osign;
                    int filledsize = (int)orderGrid["filled", oidx].Value;
                    //int signedosize = (int)orderGrid["osize", oidx].Value;
                    //if (signedosize == signedtsize) // if sizes are same whole order was filled, remove
                        //orderGrid.Rows.RemoveAt(oidx);
                    //else // otherwise remove portion that was filled and leave rest on order
                    //更新已经成交的数量
                    orderGrid["filled", oidx].Value = Math.Abs(Math.Abs(t.xsize) + filledsize);//Math.Abs(signedosize - signedtsize) * osign;
                }
            }
        }

        //得到取消委托回报
        public void GotOrderCancel(long number)
        {
            debug("got order cancel");
            ord.GotCancel(number);

            //if (ordergrid.InvokeRequired)
            //    ordergrid.Invoke(new LongDelegate(tl_gotOrderCancel), new object[] { number });
           // else
            //{
                int oidx = orderidx(number); // get row number of this order in the grid
                if ((oidx > -1) && (oidx < orderGrid.Rows.Count)) // if row number is valid
                    orderGrid["cancelled", oidx].Value = "已取消";
                    //orderGrid.Rows.RemoveAt(oidx); // remove the canceled order
           // }
        }
        
        //通过orderid得到我们需要得到的index
        int orderidx(long orderid)
        {
            for (int i = 0; i < orderGrid.Rows.Count; i++) // see if's an existing existing order
                if ((long)orderGrid["oid", i].Value == orderid)
                    return i;
            return -1;
        }


        //取消orderb
        void cancelorder(object sender, EventArgs e)
        {
            debug("menu cancel order called");
            for (int i = 0; i < orderGrid.SelectedRows.Count; i++)
            {
                long oid = (long)orderGrid["oid", orderGrid.SelectedRows[i].Index].Value;
                _broker.CancelOrder(oid);
                //_bf.CancelOrder(oid);
                debug("cancel: " + oid.ToString());
            }
        }

        private void orderGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
        }





    }
}
