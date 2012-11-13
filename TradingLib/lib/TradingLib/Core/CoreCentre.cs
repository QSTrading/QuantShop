using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TradeLink.API;

using TradeLink.Common;
using TradingLib.Data;
using TradingLib.GUI;
using TradingLib.API;
using Easychart.Finance.DataProvider;
using System.Windows.Forms;
using TradingLib.Data.database;
using System.Threading;
using TradingLib.Misc;
namespace TradingLib.Core
{
    
    //组合了组件用于数据管理与交易管理
    public class CoreCentre : IBroker
    {
       

        public event DebugDelegate SendDebugEvent;
        public event VoidDelegate EDataFeedConnect;
        public event VoidDelegate EDataFeedDisConnect;
        public event VoidDelegate EExecutionConnect;
        public event VoidDelegate EExecutionDisConnect;

        public event StringParamDelegate EventAccountAvabile;
        public event VoidDelegate EventAccountDisable;

        public event SecurityDelegate SendStreamDataEvent;

        public event TickDelegate GotTick;

        private bool _verb = true;
        private bool VerboseDebugging { get { return _verb; } set { _verb = value; } }


        ConfigHelper cfg = new ConfigHelper(CfgConstBF.XMLFN);
        public string _PROGRAM = null;

        

        //用于与服务端建立数据/交易通讯
        public BrokerFeed _bf;//BrokerFeed定义了Data Exec两个Provider用于得到市场数据以及执行交易命令
        TLTracker _tlt;//TLServer 维护控件


        //代码列表
        private BasketImpl mb = null;//OrderForm监控的symbol列表
        private Basket _defaultBasket = null;
        public Basket DefaultBasket { get { return _defaultBasket; } }

        //Account
        Account acc=null;
        public Account Account { get { return acc; } }

        TradingTrackerCentre _ttc;

        //用于维护Symbol列表中的bar数据 根据tick生成新的bar数据 同时读取本地bar数据或与服务器同步
        private BarListTracker _blt = null;
        private PositionTracker _post = null;
        private OrderTracker _ordt = null;
        //用于形成内存数据管理,为ChartControl提供实时数据,图表控件或窗口需要有MemoryDataManager才可以有效工作
        //private MemoryDataManager _mdm = null;
        //private MStockDataManager _fileDM = null;
        //private BLTDataManager _bltdm = null;



        //数据通知注册单
        List<GotTickIndicator> watchTick = new List<GotTickIndicator>();
        List<GotOrderIndicator> watchOrder = new List<GotOrderIndicator>();
        List<GotCancelIndicator> watchOrderCancel = new List<GotCancelIndicator>();
        List<GotFillIndicator> watchTrade = new List<GotFillIndicator>();
        List<IBarWatcher> watchBar = new List<IBarWatcher>();
        

        //OrderTracker ord = new OrderTracker();
        IdTracker idt = new IdTracker();//用于管理order等项目的ID
        //public int GetDate { get { DateTime d = DateTime.Now; int i = (d.Year * 10000) + (d.Month * 100) + d.Day; return i; } }
        //public int GetTime { get { DateTime d = DateTime.Now; int i = (d.Hour * 100) + (d.Minute); return i; } }

        public event TickDelegate spillTick;
        //public event OrderStatusDel orderStatus;

        AsyncResponse _ar = new AsyncResponse();

        //Timer statfade = new Timer();
        //后台工作线程,用于检查信息并做弹出窗口
        private System.ComponentModel.BackgroundWorker bg;
        //BackgroundWorker bw = new BackgroundWorker();
        //bool _constructed = false;

        public Providers Provider { get { return _bf.BrokerName; } }

       


        public CoreCentre(string program)
        {
            _PROGRAM = program;
            //_bf.VerboseDebugging
        }

        //初始化
        public void Init(TradingTrackerCentre ttc)
        {
            _ttc = ttc;
            _blt = _ttc.BarListTracker;
            _post = _ttc.PositionTracker;
            _ordt = _ttc.OrderTracker;

            InitBasket();//symbol列表初始化
            InitFeeds();//brokfeed初始化
            InitTLTracker();//数据tlserver跟踪维护初始化
            //InitDataManager();
            InitBW();
            

        }
        //析构时需要做的工作
        public void Dispose()
        { 
            
        }

        public void v(string msg)
        {
            if (_verb)
                debug(msg);
        }
        public void debug(string s)
        {
            if (SendDebugEvent != null && _verb)
            {
                SendDebugEvent(s);
            }

        }

        public bool IsBrokerConnected { get { return _bf.isBrokerConnected; } }
        public bool IsFeedConnected { get { return _bf.isFeedConnected; } }
        public bool IsLive { get { return IsBrokerConnected && IsFeedConnected; } }
        //初始化数据服务
        //
        /*
        private void InitDataManager()
        {
            //_blt = _ttc.BarListTracker;
            //读取本地历史数据并向服务器更新最新数据 形成有效的数据集
            //1.
            _fileDM = new MStockDataManager("d:\\data\\");
            //_bltdm = new BLTDataManager(_blt);
            //_bltdm.SendDebugEvent += new DebugDelegate(debug);
            _mdm = new MemoryDataManager(_fileDM);
            //得到本地历史数据之后,我们需要向服务端发送更新命令。当有新的tick或者历史数据回补时,所有的数据均会发送到memoryDataManager
        }
        public MemoryDataManager DataManager { get { return _mdm; } }
        */

        private void InitBasket()
        {
            mb = new BasketImpl();
            _defaultBasket = BasketTracker.getBasket("Default");
            mb.Add(_defaultBasket);
            
        }
        private void InitFeeds()
        {
            //初始化BrokerFeed用于绑定回调函数到特定的事件上
            string[] servers = cfg.GetConfig(CfgConstBF.IPAddress).Split(',');
            int serverport = Convert.ToInt16(cfg.GetConfig(CfgConstBF.ServerPort));
            
            _bf = new TradingLib.Core.BrokerFeed(Providers.eSignal, Providers.eSignal, true, false, _PROGRAM, servers, serverport);
            _bf.VerboseDebugging = false;

            
            _bf.SendDebugEvent += new DebugDelegate(debug);
            debug("Initfeeds Called:Init BrokerFeed");
            
            try
            {
                _bf.Start();
            }
            catch (QSNoServerException ex)
            {
                throw ex;
            }
            
            // if our machine is multi-core we use seperate thread to process ticks
            //针对多核进行多线程处理tick
            if (Environment.ProcessorCount == 1)
            {
                _bf.gotTick += new TickDelegate(_bf_gotTick);//bf得到一个新的Tick
                //_bf.gotTick += new TickDelegate(quoteView1.RefreshRow);
            }
            else
            {   //建立缓存处理tick数据
                _bf.gotTick += new TickDelegate(_bf_gotTickasync);
                _ar.GotTick += new TickDelegate(_bf_gotTick);
            }
            //ord.VerboseDebugging = true;
            //ord.SendDebugEvent += new DebugDelegate(debug);

            //定义DataFeed的回调函数
            debug("#########Set DataFeed CallBack Function######");

            _bf.gotFill += new FillDelegate(_bf_gotFill);//bf得到一个新的成交回报
            _bf.gotOrder += new OrderDelegate(_bf_gotOrder);//bf得到一个新的委托回报
            _bf.gotOrderCancel += new LongDelegate(_bf_gotOrderCancel);//bf得到一个新的取消回报
            _bf.gotPosition += new PositionDelegate(_bf_gotPosition);//bf得到position回报
            _bf.gotAccounts += new DebugDelegate(_bf_gotAccounts);//bf得到accounts回报
            //处理系统扩展的消息回报类型
            _bf.gotUnknownMessage += new MessageDelegate(_bf_gotUnknownMessage);



            //客户端连接事件绑定
            _bf.DataFeedConnet += new Int32Delegate(onDataFeedConnect);
            _bf.DataFeedDisconnect += new Int32Delegate(onDataFeedDisConnect);

            _bf.ExecutionConnect += new Int32Delegate(onExecutionConnect);
            _bf.ExecutionDisconnect += new Int32Delegate(onExecutionDisConnect);
        }

        private void InitTLTracker()
        {
            // monitor quote feed
            //debug("查看DataFeed是否连接成功");
            if (_bf.isFeedConnected)
            {
                //初始化TLServer维护控件的回调函数
                int poll = (int)(Convert.ToInt16(cfg.GetConfig(CfgConstBF.BrokerTimeoutSec)) * 1000 / 2);
                debug(poll == 0 ? "connection timeout disabled." : "using connection timeout: " + poll);

                _tlt = new TLTracker(poll, Convert.ToInt16(cfg.GetConfig(CfgConstBF.BrokerTimeoutSec)), _bf.FeedClient, Providers.Unknown, true);

                _tlt.GotConnectFail += new VoidDelegate(_tlt_GotConnectFail);
                _tlt.GotConnect += new VoidDelegate(_tlt_GotConnect);
                _tlt.GotDebug += new DebugDelegate(debug);
                _tlt.GotDisConnect += new Int32Delegate(_tlt_GotDisConnect);

                debug("Connected:" + _bf.Feed);
                //debug(_bf.FeedClient.ProvidersAvailable.Length.ToString() +"|"+_bf.FeedClient.ProviderSelected.ToString());
                //status("Connected: " + _bf.Feed);
                //debug(_tlt.isConnected.ToString());
            }
        }


        private string _title = string.Empty;
        private string _msg = string.Empty;

        private PopMessage pmsg = new PopMessage();

        private void bgDoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (true)
            {

                if (pmsg.enable)
                {
                    bg.ReportProgress(1);
                    lock (pmsg)
                    {
                        pmsg.enable = false;
                        
                    }
                    if(pmsg._type=="Notice")
                        Sound.SoundNotice();
                }
                Thread.Sleep(200);
            }
        }
        //启动后台工作进程 用于检查信息并调用弹出窗口
        private void InitBW()
        {
            bg = new System.ComponentModel.BackgroundWorker();
            bg.WorkerReportsProgress = true;
            bg.DoWork += new System.ComponentModel.DoWorkEventHandler(bgDoWork);
            bg.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bg_ProgressChanged);
            bg.RunWorkerAsync();
        }
        //显示服务端返回过来的信息窗口
        private void bg_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            
            fmPopupWindow.Show(pmsg._title,pmsg._msg, true, false);
        }


        #region 数据使用端注册
        //通过检测是否实现某些数据接口来注册观察者，某个观察者考验实现多个观察接口。用于接收多种类型的数据
        public void Subscribe(object o)
        {
            lock (this)
            {
                if (o.GetType().GetInterface("GotTickIndicator") != null)
                {
                    debug(o.ToString() + "subscribe Tick");
                    GotTickIndicator n = o as GotTickIndicator;
                    watchTick.Add(n);

                }
                if (o.GetType().GetInterface("GotOrderIndicator") != null)
                {
                    debug("add a new order watcher");
                    GotOrderIndicator n = o as GotOrderIndicator;
                    watchOrder.Add(n);
                }
                if (o.GetType().GetInterface("GotCancelIndicator") != null)
                {
                    debug("add a new ordercancel watcher");
                    GotCancelIndicator n = o as GotCancelIndicator;
                    watchOrderCancel.Add(n);

                    //watchOrder.Add(n);
                }
                if (o.GetType().GetInterface("GotFillIndicator") != null)
                {
                    GotFillIndicator n = o as GotFillIndicator;
                    watchTrade.Add(n);
                }
            }
        }

        public void unSubscribe(object o)
        {
            lock (this)
            {
                if (o.GetType().GetInterface("GotTickIndicator") != null)
                {
                    debug(o.ToString() + "unsubscribe Tick");
                    GotTickIndicator n = o as GotTickIndicator;
                    watchTick.Remove(n);
                }
                if (o.GetType().GetInterface("GotOrderIndicator") != null)
                {
                    GotOrderIndicator n = o as GotOrderIndicator;
                    watchOrder.Remove(n);
                }
                if (o.GetType().GetInterface("GotCancelIndicator") != null)
                {
                    GotCancelIndicator n = o as GotCancelIndicator;
                    watchOrderCancel.Remove(n);
                    //watchOrder.Add(n);
                }
                if (o.GetType().GetInterface("GotFillIndicator") != null)
                {
                    GotFillIndicator n = o as GotFillIndicator;
                    watchTrade.Remove(n);
                }
            }
        }


        #endregion


        #region Start Stop 服务器连接
        public void Disconnect()
        {
            debug("########Stop Broker Feed....");
            if(_bf.FeedClient.IsConnected)
                _bf.FeedClient.Disconnect();//停止服务的同时我们需要向服务器发送cliearClient消息
            if (_bf.BrokerClient.IsConnected)
            {
                acc = null;
                _bf.BrokerClient.Disconnect();
                if (EventAccountDisable != null)
                    EventAccountDisable();
            }
        }
        public void Connect()
        {
            debug("########Connect Broker Feed....");
            Thread t = new Thread(new ThreadStart(_bf.Connect));
            t.Start();
        }
        //broker使用reset后 tltracker将会失效,应为tltracker需要知道 FeedClient的连接事件进行相应触发
        public void Start()
        {
            //注意这里的行为改变
            debug("########Start Broker Feed....");
            Thread t = new Thread(new ThreadStart(_bf.Start));
            t.Start();
            //_bf.Start();
            /*
            if (!_bf.FeedClient.IsConnected)
                debug("start here");
                _bf.FeedClient.Start();//用于启动服务
            if(!_bf.BrokerClient.IsConnected)
                _bf.BrokerClient.Start();
             * */

        }
        //重新启动数据或交易连接
        public void Stop()
        {
            Disconnect();
            //_bf.Disconnect();
        }
        //关闭窗口时用于彻底关闭连接
        public void Stop(bool shutdown)
        {
            if (shutdown)
                _bf.Disconnect();
        }
        public void Reconnect()
        {
            Disconnect();
            Start();
            debug(_bf.BrokerName + " " + _bf.ServerVersion + " refreshed.");
        }
        public void Reset()
        {
            Init(_ttc);
        }

        private void onDataFeedConnect(int i)
        {
            debug("DataFeed connected Event");
            if (mb.Count > 0)
            {
                debug(" Have Basket,going to subscribe market data");
                subscribe();
            }
            
            if (EDataFeedConnect != null)
            {
                EDataFeedConnect();
            }
        }

        private void onDataFeedDisConnect(int i)
        {
            debug("DataFeed disconnected");
            //debug(_bf.BrokerName + " " + _bf.ServerVersion + " refreshed.");
            if (EDataFeedDisConnect != null)
            {
                EDataFeedDisConnect();
            }
        }

        private void onExecutionConnect(int i)
        {
            debug("Execution connected");
            if (EExecutionConnect != null)
            {
                EExecutionConnect();
            }
        }

        private void onExecutionDisConnect(int i)
        {
            debug("Execution disconnected");
            if (EExecutionDisConnect != null)
            {
                EExecutionDisConnect();
            }
        }


        #endregion






        #region TLTracker回调函数

        void _tlt_GotDisConnect(int val)
        {
            debug("feed client dissconnected");
        }
        //TLServer维护控件的回调函数
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
        //请求barlist
        public BarList getBarlist(string sym)
        {
            return _blt[sym];
        }
        //请求认证交易账号
        public void RequestAccount(string acc, int pass)
        {
            //调用bf请求有效交易账户，请求交易账户时我们需要将目前的Accts设置为null
            //当accts为null时,所有的交易均不可以发出
            _bf.RequestAccounts(acc, pass);
        
        }
        //
        /*
        void RegisterBasket(Basket b)
        {
            try
            {
                //debug("BrokerFeed请求订阅数据");
                _bf.Subscribe(b);
                // foreach (Security sec in b.ToArray())
                {
                    //     subscribe(sec.Symbol);
                }

                //subscribe(sec.Symbol);
            }
            catch (TLServerNotFound) { debug("no broker or feed server running."); }

        }*/
        //订阅市场数据
        private void subscribe(string sym)
        {
            debug("主线程调用subscribe 请求数据:" + sym);
            Security sec = SecurityImpl.Parse(sym);
            if (!hassym(sym))
                mb.Add(sym);
            //symindex();
            subscribe();
        }
        //
        private bool subscribe()
        {
            try
            {
                //debug("BrokerFeed请求订阅数据");
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


        //向服务器发送Order
        public void SendOrder(Order sendOrder)
        {
            debug("CoreCentre Got New Order");
            //检测是否请求了交易账户
            if ((acc==null) || !acc.isValid)
            {
                fmMessage fm = new fmMessage("没有有效交易账户");
                fm.ShowDialog();
                return;
            }
            if (acc.isValid)
                sendOrder.Account = acc.ID;

            //设定Order ID
            if (sendOrder.id == 0)
                sendOrder.id = idt.AssignId;
            //设定发单时间
            sendOrder.date = Util.ToTLDate(DateTime.Now);
            sendOrder.time = Util.ToTLTime(DateTime.Now);
            int res = _bf.SendOrder(sendOrder);
            if (res != 0)
            {
                string err = Util.PrettyError(_bf.BrokerName, res);
                debug(err);
                debug(sendOrder.ToString() + "( " + err + " )");
            }
            else
                debug("order: " + sendOrder.ToString());
             
        }
        //向服务器取消委托指令
        public void CancelOrder(long oid)
        {

            _bf.CancelOrder(oid);
            debug("cancel: " + oid.ToString());
           
        }


        //请求历史数据回补,根据提供的开始时间参数,服务端会形成一定的历史数据然后发送给Client
        public void RequestHistBar(string barrequest)
        {
            v("CoreCentre Send History BarRequest");
            _bf.TLSend(MessageTypes.BARREQUEST, barrequest);
        }
        //当我们打开k线图或者策略中从QSMemoryDataManager中请求历史数据的时候 我们均请求
        //CC进行历史数据的回补 如果程序启动以来已经进行过了历史数据回补那么我们就不需要重复回补
        //QSMemorydatamanager会自动帮我们保存数据,因此我们需要建立一个回补开关 回补过得我们记录就无需多次回补
        //请求当天日内数据回补
        Dictionary<string, bool> _histRequestMap = new Dictionary<string, bool>();
        public void RequestInterDayBar(Security sec)
        {
            bool requested = false;
            if (!_histRequestMap.TryGetValue(sec.Symbol, out requested))
            {

                v("CoreCentre Send Inter Day BarRequest");
                _bf.TLSend(MessageTypes.BARREQUEST, BarImpl.BuildBarRequest(sec.Symbol, BarInterval.Minute));
                _histRequestMap.Add(sec.Symbol, true);
            }
            else
            {
                if (requested == false)
                {
                    v("CoreCentre Send Inter Day BarRequest");
                    _bf.TLSend(MessageTypes.BARREQUEST, BarImpl.BuildBarRequest(sec.Symbol, BarInterval.Minute));
                    _histRequestMap.Add(sec.Symbol, true);
                }
                
            }
            
        }
        //string[] accts;
        Dictionary<string, Position> posdict = new Dictionary<string, Position>();



        #endregion


        #region BrokerFeed与服务端通讯后触发事件的回调函数
        
        void _bf_gotUnknownMessage(MessageTypes type, long source, long dest, long msgid, string request, ref string msg)
        {
            //debug("we got unknowmessage"+type.ToString());
            switch (type)
            {
                //从服务器回补历史数据的处理
                //本地的barlist是由以前的数据插入后形成的，如果多次更新会形成重复的Bar或者有些Bar会错误,这里需要想办法解决
                //memmory的Bar数据倒是完好，通过内置的回补函数可以正确的处理 多次回补也不会出现数据重复或者错误。
                case MessageTypes.BARRESPONSE:
                    //debug("got bar response");
                    Bar b = BarImpl.Deserialize(msg);
                    if(b.isValid)
                    {
                        //将历史数据回补到memory data manager当中去 这样需要历史数据的client就可以从_mdm中得到历史数据
                        //_mdm.AddNewPacket(new DataPacket(b.Symbol,Util.ToDateTime(b.Bardate, b.Bartime),(double)b.Open,(double)b.High,(double)b.Low,(double)b.Close,(double)b.Volume,(double)b.Close));
                        _ttc.GotBarResponse(b);
                        // get bar list
                        BarList bl = _blt[b.Symbol, b.Interval];
                        // get nearest intrday bar
                        int preceed = BarListImpl.GetBarIndexPreceeding(bl, b.Bardate, b.Bartime);
                        // increment by one to get new position
                        int newpos = preceed + 1;
                        // insert bar
                        _blt[b.Symbol] = BarListImpl.InsertBar(bl, b, newpos);
                    }       
                //DateTime dt = Util.ToDateTime(b.Bardate, b.Bartime);

                    //debug(b.ToString()+"|"+b.Symbol+"|"+b.Interval.ToString()+"|"+b.Bardate.ToString()+":"+b.Bartime.ToString()+"##"+dt.ToString());
                    //m_ChartDataClient.GotBarBackFilled(b);//chartdataclient获得历史数据
                    break;
                case MessageTypes.POPMESSAGE:
                    debug("Order被拒绝 请检查相关信息:"+msg);
                    //UTF8Encoding utf8 = new UTF8Encoding();
                    //debug(msg);
                    lock (pmsg)
                    {
                        pmsg._title = "提示:";
                        pmsg._msg = msg;
                        pmsg._type="Notice";
                        pmsg.enable = true;
                    }
                    /*
                    Thread t = new Thread(new ThreadStart(CreateForm));
                    t.IsBackground=true;
                    t.Start();
                    //fmPopupWindow.Show("Order被拒绝", msg, true, false);
                    break;
                    * */
                    break;

                    
                default:
                    break;

            }

        }




        //CoreCentre得到回报账户的处理
        void _bf_gotAccounts(string msg)
        {
            debug("avail accounts: " + msg);
            if (msg == null)
                return;
            else
            {
                acc = new Account(msg);
                if (EventAccountAvabile != null)
                    EventAccountAvabile(acc.ID);
            
            }

        }
        //position回报的处理
        void _bf_gotPosition(Position pos)
        {
            debug("pos: " + pos.ToString());
            //ttc记录了该position 则ordEntry就可以通过ttc来获得对应的信息
            _ttc.GotPosition(pos);
            foreach (GotFillIndicator watcher in watchTrade)
            {
                Trade t = pos.ToTrade();
                t.Account = pos.Account;
                watcher.GotFill(t);
            }
            //_post.Adjust(pos);
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

        public void newdemoTick()
        {

            Tick t = new TickImpl();
            t.symbol = "IF1210";
            t.ask = 2000;
            t.bid = 2001;
            t.os = 20;
            t.bs = 10;
            t.trade = (decimal)2000.5;
            t.size = 5;
            t.ex = "CN_IFFEX";
            _bf_gotTick(t);

        }
        //得到最新的市场数据tick
        void _bf_gotTick(Tick t)
        {
            try
            {
                
                //0.通知tickwather列表
                foreach (GotTickIndicator watcher in watchTick)
                    watcher.GotTick(t);
                
                //1.更新本地BarListTracker用于生成新的Bar
                _ttc.GotTick(t);
                //_blt.newTick(t);//tick的检查统一由服务端进行,客户端接收的tick均是有效的tick
                //_post.GotTick(t);
                //得到该symbol最新的Bar
                //Bar b = _ttc.BarListTracker[t.symbol].RecentBar;
                //通知barwatcher列表
                //foreach (IBarWatcher watcher in watchBar)
                //    watcher.GotBar(b);
                //将该bar封装成DataPackage发布到MemoryDataManager中去,这样Chart就会自动更新
                //_mdm.AddNewPacket(new DataPacket(t.symbol, Util.ToDateTime(b.Bardate, b.Bartime), (double)b.Open, (double)b.High, (double)b.Low, (double)b.Close, (double)b.Volume, (double)b.Close));
                //debug("<<<<<<<<<<<<<k price:" + t.trade.ToString());
                //debug("**********lastprice:(double)b.Close" + b.Close.ToString());
                
                //2.通知tltracker用于检查tick数据是否正常 用于重新连接quote服务器
                _tlt.newTick(t);
                
                if (GotTick != null)
                    GotTick(t);
                 

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
            debug("CoreCentre 得到委托回报");
            _ttc.GotOrder(o);
            //this.orderView1.OrderView_GotOrder(o);
            //orderTracker记录Order
            //_ordt.GotOrder(o);
            //通知Order回报观察者
            foreach (GotOrderIndicator watcher in watchOrder)
                watcher.GotOrder(o);
            //ord.GotOrder(o);
            /*
            if (orderidx(o.id) == -1) // if we don't have this order, add it
                ordergrid.Rows.Add(new object[] { o.id, o.symbol, (o.side ? "BUY" : "SELL"), o.UnsignedSize, (o.price == 0 ? "Market" : o.price.ToString(_dispdecpointformat)), (o.stopp == 0 ? "" : o.stopp.ToString(_dispdecpointformat)), o.Account });

            this.orderView1.OrderView_GotOrder(o);
            */
        }
        //得到取消委托回报
        void _bf_gotOrderCancel(long oid)
        {
            debug("CoreCentre 得到委托撤销回报");
            _ttc.GotCancel(oid);
            foreach (GotCancelIndicator watcher in watchOrderCancel)
                watcher.GotCancel(oid);
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
        void _bf_gotFill(Trade f)
        {
            debug("CoreCentre 得到成交回报");
            _ttc.GotFill(f);
            debug(f.ToString());
            foreach (GotFillIndicator watcher in watchTrade)
                watcher.GotFill(f);
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

    class PopMessage
    {
        internal bool enable=false;
        internal string _title;
        internal string _type;
        internal string _msg;

    }
}
