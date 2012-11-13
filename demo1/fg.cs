using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeLink.Common;
using TradeLink.API;
using TradeLink.AppKit;
using TradingLib;
using TradingLib.GUI;
using TradingLib.Data;

using Easychart.Finance.DataProvider;
using Easychart.Finance;

namespace demo1
{
    public class fg
    {
        public event DebugDelegate SendDebugEvent;
        private bool _verb = true;
        ConfigHelper cfg = new ConfigHelper(CfgConstBF.XMLFN);
        public string _PROGRAM = null;

        private bool vdebugEnable = true;

        //用于与服务端建立数据/交易通讯
        public BrokerFeed _bf;//BrokerFeed定义了Data Exec两个Provider用于得到市场数据以及执行交易命令
        TLTracker _tlt;//TLServer 维护控件


        //代码列表
        private BasketImpl mb = new BasketImpl();//OrderForm监控的symbol列表
        private Basket _defaultBasket = BasketTracker.getBasket("Default");


        //用于维护Symbol列表中的bar数据 根据tick生成新的bar数据 同时读取本地bar数据或与服务器同步
        private BarListTracker _blt = new BarListTracker();
        //用于形成内存数据管理,为ChartControl提供实时数据,图表控件或窗口需要有MemoryDataManager才可以有效工作
        private MemoryDataManager _mdm = null;
        private BLTDataManager _bltdm = null;

        //OrderTracker ord = new OrderTracker();
        //IdTracker idt = new IdTracker();
        //public int GetDate { get { DateTime d = DateTime.Now; int i = (d.Year * 10000) + (d.Month * 100) + d.Day; return i; } }
        //public int GetTime { get { DateTime d = DateTime.Now; int i = (d.Hour * 100) + (d.Minute); return i; } }

        public event TickDelegate spillTick;
        //public event OrderStatusDel orderStatus;

        AsyncResponse _ar = new AsyncResponse();

        //Timer statfade = new Timer();

        //BackgroundWorker bw = new BackgroundWorker();
        //bool _constructed = false;


        public fg(string program)
        {
            _PROGRAM = program;
        }

        public void Init()
        {
            InitFeeds();
            InitTLTracker();

        }

        public void debug(string s)
        {
            if (SendDebugEvent != null)
            {
                SendDebugEvent(s);
            }

        }


        private void InitFeeds()
        {
            //初始化BrokerFeed用于绑定回调函数到特定的事件上
            string[] servers = cfg.GetConfig(CfgConstBF.IPAddress).Split(',');
            int serverport = Convert.ToInt16(cfg.GetConfig(CfgConstBF.ServerPort));

            _bf = new BrokerFeed(Providers.CTP, Providers.CTP, true, false, _PROGRAM, servers, serverport);
            _bf.VerboseDebugging = true;

            _bf.SendDebugEvent += new DebugDelegate(debug);
            debug("initfeeds is called:初始化BrokerFeed");
            _bf.Reset();

            // if our machine is multi-core we use seperate thread to process ticks
            //针对多核进行多线程处理tick
            if (Environment.ProcessorCount == 1)
            {
                _bf.gotTick += new TickDelegate(_bf_gotTick);//bf得到一个新的Tick
                //_bf.gotTick += new TickDelegate(quoteView1.RefreshRow);
            }
            else
            {
                _bf.gotTick += new TickDelegate(_bf_gotTickasync);
                _ar.GotTick += new TickDelegate(_bf_gotTick);
            }
            //ord.VerboseDebugging = true;
            //ord.SendDebugEvent += new DebugDelegate(debug);

            //定义DataFeed的回调函数
            debug("绑定DataFeed回调函数");

            _bf.gotFill += new FillDelegate(_bf_gotFill);//bf得到一个新的成交回报
            _bf.gotOrder += new OrderDelegate(_bf_gotOrder);//bf得到一个新的委托回报
            _bf.gotOrderCancel += new LongDelegate(_bf_gotOrderCancel);//bf得到一个新的取消回报
            _bf.gotPosition += new PositionDelegate(_bf_gotPosition);//bf得到position回报
            _bf.gotAccounts += new DebugDelegate(_bf_gotAccounts);//bf得到accounts回报
            //处理系统扩展的消息回报类型
            _bf.gotUnknownMessage += new MessageDelegate(_bf_gotUnknownMessage);



        }

        private void InitTLTracker()
        {
            // monitor quote feed
            debug("查看DataFeed是否连接成功");
            if (_bf.isFeedConnected)
            {
                //初始化TLServer维护控件的回调函数
                int poll = (int)(Convert.ToInt16(cfg.GetConfig(CfgConstBF.BrokerTimeoutSec)) * 1000 / 2);
                debug(poll == 0 ? "connection timeout disabled." : "using connection timeout: " + poll);

                _tlt = new TLTracker(poll, Convert.ToInt16(cfg.GetConfig(CfgConstBF.BrokerTimeoutSec)), _bf.FeedClient, Providers.Unknown, true);
                _tlt.GotConnectFail += new VoidDelegate(_tlt_GotConnectFail);
                _tlt.GotConnect += new VoidDelegate(_tlt_GotConnect);
                _tlt.GotDebug += new DebugDelegate(_tlt_GotDebug);
                debug("Connected:" + _bf.Feed);
                //status("Connected: " + _bf.Feed);
            }

            //当quoteView1触发sendDebugEvent事件时 调用本类中的debug用于输出
            //this.quoteView1.SendDebugEvent += new DebugDelegate(debug);
            //this.orderView1.SendDebugEvent += new DebugDelegate(debug);
            //this.positionView1.SendDebugEvent += new DebugDelegate(debug);

        }

        #region TLTracker回调函数
        //TLServer维护控件的回调函数
        void _tlt_GotDebug(string msg)
        {
            debug(msg);
        }

        void _tlt_GotConnect()
        {
            debug("_tlt_GotConnect is called");
            try
            {
                if (_tlt.tw.RecentTime != 0)
                {
                    debug(_bf.BrokerName + " " + _bf.ServerVersion + " refreshed.");
                    //status(_bf.BrokerName + " connected.");
                }
                // if we have a quote provider
                if ((_bf.ProvidersAvailable.Length > 0))
                {
                    // don't track tradelink
                    if (_bf.BrokerName == Providers.TradeLink)
                    {
                        _tlt.Stop();
                    }

                    // if we have a quote provid
                    if (mb.Count > 0)
                    {
                        //subscribe();
                    }
                }
            }
            catch { }
        }

        void _tlt_GotConnectFail()
        {
            debug("_tlt_GotConnectFail is called");
            if (_tlt.tw.RecentTime != 0)
            {
            }
        }
        #endregion

        #region BrokerFeed 功能操作与回调函数

        //
        void RegisterBasket(Basket b)
        {
            try
            {
                debug("BrokerFeed请求订阅数据");
                _bf.Subscribe(b);
                // foreach (Security sec in b.ToArray())
                {
                    //     subscribe(sec.Symbol);
                }

                //subscribe(sec.Symbol);
            }
            catch (TLServerNotFound) { debug("no broker or feed server running."); }

        }
        //订阅市场数据
        void subscribe(string sym)
        {
            debug("主线程调用subscribe 请求数据:" + sym);
            Security sec = SecurityImpl.Parse(sym);
            if (!hassym(sym))
                mb.Add(sym);
            //symindex();
            subscribe();
        }
        bool subscribe()
        {

            try
            {
                debug("BrokerFeed请求订阅数据");
                _bf.Subscribe(mb);
                return true;
            }
            catch (TLServerNotFound) { debug("no broker or feed server running."); }

            return false;

        }
        bool hassym(string sym)
        {
            foreach (Security s in mb)
                if ((s.FullName == sym) || (s.Symbol == sym))
                    return true;
            return false;
        }


        //
        void t_neworder(Order sendOrder)
        {
            /*
            if (accountname.Text != "")
                sendOrder.Account = accountname.Text;
            if (exchdest.Text != "")
                sendOrder.Exchange = exchdest.Text;
            if (sendOrder.id == 0)
                sendOrder.id = idt.AssignId;
            int res = _bf.SendOrder(sendOrder);
            if (res != 0)
            {
                string err = Util.PrettyError(_bf.BrokerName, res);
                status(err);
                debug(sendOrder.ToString() + "( " + err + " )");
            }
            else
                debug("order: " + sendOrder.ToString());
             */
        }
        //取消委托
        void cancelorder(object sender, EventArgs e)
        {
            /*
            for (int i = 0; i < ordergrid.SelectedRows.Count; i++)
            {
                long oid = (long)ordergrid["oid", ordergrid.SelectedRows[i].Index].Value;
                _bf.CancelOrder(oid);
                debug("cancel: " + oid.ToString());
            }
             */
        }


        string[] accts;
        Dictionary<string, Position> posdict = new Dictionary<string, Position>();



        #endregion


        #region BrokerFeed与服务端通讯后触发事件的回调函数
        //从服务器回补历史数据从这里得到消息
        void _bf_gotUnknownMessage(MessageTypes type, long source, long dest, long msgid, string request, ref string msg)
        {

            //debug("we got unknowmessage"+type.ToString());
            switch (type)
            {
                case MessageTypes.BARRESPONSE:
                    Bar b = BarImpl.Deserialize(msg);
                    //DateTime dt = Util.ToDateTime(b.Bardate, b.Bartime);

                    //debug(b.ToString()+"|"+b.Symbol+"|"+b.Interval.ToString()+"|"+b.Bardate.ToString()+":"+b.Bartime.ToString()+"##"+dt.ToString());
                    //m_ChartDataClient.GotBarBackFilled(b);//chartdataclient获得历史数据
                    break;
                default:
                    break;

            }

        }
        void _bf_gotAccounts(string msg)
        {
            debug("avail accounts: " + msg);
            accts = msg.Split(',');

        }
        //position回报的处理
        void _bf_gotPosition(Position pos)
        {
            debug("pos: " + pos.ToString());

            /*
            pt.Adjust(pos);
            int[] rows = new int[0];
            if (symidx.TryGetValue(pos.Symbol, out rows))
            {
                foreach (int r in rows)
                {
                    qt.Rows[r]["AvgPrice"] = pos.AvgPrice.ToString(_dispdecpointformat);
                    qt.Rows[r]["PosSize"] = pos.Size.ToString();
                    qt.Rows[r]["ClosedPL"] = pos.ClosedPL.ToString(_dispdecpointformat);
                }

            }
             */
        }

        //得到最新的市场数据tick
        void _bf_gotTick(Tick t)
        {
            try
            {
                //1.更新本地BarListTracker用于生辰新的Bar
                _blt.newTick(t);//tick的检查统一由服务端进行,客户端接收的tick均是有效的tick
                //得到该symbol最新的Bar
                Bar b = _blt[t.symbol].RecentBar;
                //将该bar封装成DataPackage发布到MemoryDataManager中去,这样Chart就会自动更新
                _mdm.AddNewPacket(new DataPacket(t.symbol, Util.ToDateTime(b.Bardate, b.Bartime), (double)b.Open, (double)b.High, (double)b.Low, (double)b.Close, (double)b.Volume, (double)b.Close));

                //2.更新报价表格中的价格信息
                //m_OrdersForm.gotTick(t);
                //m_QuoteListForm.GotTick(t);
                //m_ChartDataClient.GotTick(t);
                /*
                _tlt.newTick(t);
                _kt.newTick(t);
                if (spillTick != null)
                    spillTick(t);
                RefreshRow(t);
                //更新quoteview控件
                this.quoteView1.RefreshRow(t);
                this.positionView1.PositionView_GotTick(t);
                */

                /*
                BarList bl = null;
                if (bardict.TryGetValue(t.symbol, out bl))
                {
                    if (SecurityImpl.Parse(t.symbol).Type == SecurityType.CASH)
                    {
                        Tick k = _kt[t.symbol];
                        decimal p = usebidonfx ? k.bid : k.ask;
                        int s = usebidonfx ? k.bs : k.os;
                        bardict[t.symbol].newPoint(t.symbol, p, k.time, k.date, s);
                    }
                    else
                        bardict[t.symbol].newTick(t);
                }
                 */
            }
            catch (System.Threading.ThreadInterruptedException) { }
        }
        void _bf_gotTickasync(Tick t)
        {
            // on multi-core machines, this will be invoked to write ticks
            // to a cache where they will be processed by a seperate thread
            // asynchronously
            _ar.newTick(t);
        }
        //得到委托回报
        void _bf_gotOrder(Order o)
        {
            debug("系统得到委托回报");
            //this.orderView1.OrderView_GotOrder(o);
            //ord.GotOrder(o);
            /*
            if (orderidx(o.id) == -1) // if we don't have this order, add it
                ordergrid.Rows.Add(new object[] { o.id, o.symbol, (o.side ? "BUY" : "SELL"), o.UnsignedSize, (o.price == 0 ? "Market" : o.price.ToString(_dispdecpointformat)), (o.stopp == 0 ? "" : o.stopp.ToString(_dispdecpointformat)), o.Account });

            this.orderView1.OrderView_GotOrder(o);
            */
        }
        //得到取消委托回报
        void _bf_gotOrderCancel(long number)
        {

            /*
            ord.GotCancel(number);
            this.orderView1.OrderView_gotOrderCancel(number);
            if (ordergrid.InvokeRequired)
                ordergrid.Invoke(new LongDelegate(tl_gotOrderCancel), new object[] { number });
            else
            {
                int oidx = orderidx(number); // get row number of this order in the grid
                if ((oidx > -1) && (oidx < ordergrid.Rows.Count)) // if row number is valid
                    ordergrid.Rows.RemoveAt(oidx); // remove the canceled order
            }
             */
        }
        //当有成交回报时,对成交进行修改
        void _bf_gotFill(Trade t)
        {
            /*
            this.tradeView1.TradeView_GotFill(t);
            this.orderView1.OrderView_GotFill(t);
            this.positionView1.PositionView_GotFill(t);

            ord.GotFill(t);

            if (InvokeRequired)
                Invoke(new FillDelegate(tl_gotFill), new object[] { t });
            else
            {
                if (!t.isValid) return;
                int oidx = orderidx(t.id); // get order id for this order
                if (oidx != -1)
                {
                    int osign = (t.side ? 1 : -1);
                    int signedtsize = t.xsize * osign;
                    int signedosize = (int)ordergrid["osize", oidx].Value;
                    if (signedosize == signedtsize) // if sizes are same whole order was filled, remove
                        ordergrid.Rows.RemoveAt(oidx);
                    else // otherwise remove portion that was filled and leave rest on order
                        ordergrid["osize", oidx].Value = Math.Abs(signedosize - signedtsize) * osign;
                }



                pt.Adjust(t);

                UpdateSymbolTrade(GetSymbolRows(t.Sec.FullName), t);
                UpdateSymbolTrade(GetSymbolRows(t.Sec.Symbol), t);

                TradesView.Rows.Add(t.xdate, t.xtime, t.symbol, (t.side ? "BUY" : "SELL"), t.xsize, t.xprice.ToString(_dispdecpointformat), t.comment, t.Account.ToString()); // if we accept trade, add it to list
            }
             */
        }
        #endregion
    }
}
