using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

using TradeLink.Common;
using TradeLink.API;
using TradeLink.AppKit;
using TradingLib.Core;
using TradingLib.API;

namespace TradingLib.GUI
{
    public partial class fmOrdEntry : DockContent, GotTickIndicator,GotFillIndicator,GotOrderIndicator,GotCancelIndicator,GotPositionIndicator
    {
        public event DebugDelegate SendDebugEvent;
        public event SymDelegate SendRegisterSymbols;
        //public event SymDelegate SendQuoteSymbolSelected;

        public event SecurityDelegate EOpenChart;

        public event SecurityDelegate SymbolSelectedEvent;//当所选symbol发生变化触发事件
        public event SecurityDelegate SendSymbolRequestEvent;//当有新的数据请求时触发请求数据事件
        public event OrderDelegate SendOrderEvent;//发送Order
        public event LongDelegate SendCancleEvent;//发送取消
        public event SecurityDelegate SendOpenTimeSalesEvent;

        //private CoreCentre _coreCentre = null;
        private IBroker _broker = null;

        private PositionCheckCentre _poscheckCentre=null;
        public PositionCheckCentre PositionCheckCentre { get { return _poscheckCentre; }
            set { _poscheckCentre = value;
            positionView1.PositionCheckCentre = _poscheckCentre;
                }
        }

        public TradingTrackerCentre _ttc = null;
        public TradingTrackerCentre TradingTrackerCentre { get { return _ttc; } 
            set { 
                _ttc = value;
                positionView1.TradingTrackerCentre = _ttc;
        
        } }
        public fmOrdEntry(Basket mb)
        {
            
            InitializeComponent();
            
            //quoteView1.SendRegisterSymbols += new SymDelegate(srv_RegisterSymbols);
            //quoteView1.SendDebugEvent += new DebugDelegate(debug);
            //quoteView1.SendOrderEvent += new OrderDelegate(SendOrder);
            //quoteView1.EOpenChart += new SecurityDelegate(openChart);
            quoteView1.SendOpenChartEvent +=new SecurityDelegate(openChart);
            quoteView1.SymbolSelectedEvent += new SecurityDelegate(quoteView_SymbolSelected);
            quoteView1.SendOpenTimeSalesEvent +=new SecurityDelegate(oepnTimeSales);
            quoteView1.SendDebugEvent += new DebugDelegate(debug);

            //quoteView1.SendDebugEvent +=new DebugDelegate(debug);
            positionView1.SendDebugEvent+=new DebugDelegate(debug);
            positionView1.SendOrderEvent += (order) => 
            {
                if (SendOrderEvent != null)
                    SendOrderEvent(order);
            };
            //orderView1.SendDebugEvent += new DebugDelegate(debug);

            spillTick1.SendDebugEvent +=new DebugDelegate(debug);
            spillTick1.SendOrderEvent += new OrderDelegate(SendOrder);
            
            //quoteView1.SendOrderEvent += new OrderDelegate(SendOrder);
            //this.quoteView1.SymbolSelected += new SymDelegate(quoteView_SymbolSelected);

            //初始化QuoteView的Basket清单,默认情况下orderEntry中的quoteView只显示defaultBasket
            quoteView1.SetBasket(mb);
        }

        void oepnTimeSales(Security sec)
        {
            if (SendOpenTimeSalesEvent != null)
            {
                SendOpenTimeSalesEvent(sec);
            }
        }
        void SendOrder(Order o)
        {
            if (SendOrderEvent != null)
                SendOrderEvent(o);
        }

        private void openChart(Security sec)
        {
            if (EOpenChart != null)
                EOpenChart(sec);
            
        }
        public void AddSecToDefault(Security s)
        {
            quoteView1.addSecurity(s);
        }
        public void DelSecFromDefault(Security s)
        {
            //quoteView1.delSecurity(s);
        }

        public void orderForm_gotPosition(Position pos)
        { 
            //this.po
            
        }

        //封装了debug回调函数
        void debug(string msg)
        {
            if (SendDebugEvent != null)
                SendDebugEvent(msg);

        }

        void quoteView_SymbolSelected(Security sec)
        {
            //quoteView_选择某个特定合约时,我们需要更新下单面板的合约
            spillTick1.setSecurity(sec);

            if (SymbolSelectedEvent != null)
                SymbolSelectedEvent(sec);
        }
        void srv_RegisterSymbols(string sym)
        {
            debug("orderForm's send registerSymbols is called");
            if (SendRegisterSymbols != null)
            {
                SendRegisterSymbols(sym);
            }
        }

        //
        
        public void GotTick(Tick k)
        {
            try
            {
                //quote控件动态更新tick信息
                quoteView1.GotTick(k);
                
                spillTick1.GotTick(k);//更新下单面板的tick数据
                positionView1.GotTick(k);
            }
            catch (Exception ex)
            { }
        
        }

        public void GotOrder(Order o)
        {
            debug("order entry got a order");
            orderView1.GotOrder(o);
        }

        //public void 

        public void GotCancel(long oid)
        {
            orderView1.GotOrderCancel(oid);
        }

        public void GotFill(Trade f)
        {
            orderView1.GotFill(f);
            tradeView1.GotFill(f);
            positionView1.GotFill(f);
        }

        public void GotPosition(Position p)
        {
            
        }

        private void fmOrdEntry_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
