using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeLink.API;
using TradeLink.Common;
using TradingLib;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.ComponentModel;
using CTP;
using System.Threading;
using System.Diagnostics;//记得加入此引用
using TradingLib.Data;
using TradingLib.Broker;
using TradingLib.Data.database;
using TradingLib.GUI;
using TradingLib.Broker.CTP;

namespace TradingLib.Core
{
    /// <summary>
    /// //CTPServer负责TLServer与外部Provider之间的通讯以及服务器内部组件之间的逻辑调用
    /// </summary>
    public class CTPServer
    {
        public event VoidDelegate ServerConnectedEvent;
        public event DebugDelegate SendDebugEvent;
        public event AccountUpdateDel SendAccountUpdateEvent;
        public event TickDelegate GotTick;
        public event OrderDelegate GotOrder;
        public event LongDelegate GotCancel;
        public event FillDelegate GotFill;

        public event RiskCheckOrderDel SendOrderRiskCheckEvent;

        BackgroundWorker bw = new BackgroundWorker();

        public ClearCentre ClearCentre { get; set; }

        
        bool _valid = false;

        //模拟交易服务
        bool _papertradebidask = true;
        public bool isPaperTradeUsingBidAsk { get { return _papertradebidask; } set { _papertradebidask = value; } }

        bool _papertrade = false;
        public bool isPaperTradeEnabled { get { return _papertrade; } set { _papertrade = value; } }
        
        PapertradeEngine  ptt = null;
        //虚拟账户集中交易服务
        MiddleBroker mbroker = null;

        public int DefaultBarsBack {get;set;}
        private bool _noverb = true;
        public bool VerboseDebugging { get { return _noverb; } set { 
            _noverb = value;
            if(tl!=null)
                tl.VerboseDebugging = value;
        
        } }//控制Verbose是否输出程序信息
        public int WaitBetweenEvents = 50;
        public bool ReleaseDeadSymbols = false;
        public bool AllowSendInvalidBars = false;
        public bool ReleaseBarHistoryAfteRequest = true;
        //string _tmpregister = string.Empty;

        //int qc, qr;


        Dictionary<int, BarRequest> _barhandle2barrequest = new Dictionary<int, BarRequest>();
        //RingBuffer<BarRequest> _barrequests = new RingBuffer<BarRequest>(500);
        //barslist tracker保存symbol的barts


        //我们目前只保存1分钟的bars 用于给Client回补bars
        BarListTracker _blt = new BarListTracker(BarInterval.Minute);
        //TickTracker _tt = new TickTracker();

        public TLServer tl;


        //bool _go = true;
        public bool isValid { get { return _valid; } }
        bool _barrequestsgetalldata = true;
        public bool BarRequestsGetAllData { get { return _barrequestsgetalldata; } set { _barrequestsgetalldata = value; } }


        //服务器维护的basket 用于向服务器请求实事数据
        Basket _mb = null;//TLServer 于保存Symbol


        //用于储存信息进行缓存处理的缓存区域
        RingBuffer<Tick> _tickCahce = new RingBuffer<Tick>(10000);//缓存tick用于处理
        RingBuffer<BarRequest> _brCache = new RingBuffer<BarRequest>(500);//缓存BarRequest进行处理历史数据回补请求
        //储存CTP lastTick数据 用于得到上一个tick,用来计算size
        //Dictionary<string, ThostFtdcDepthMarketDataField> _symTickSnapMap = new Dictionary<string, ThostFtdcDepthMarketDataField>();

        //用于储存tick数据
        TickArchiver _tickArchiver = new TickArchiver("d:\\data");
        //异步处理tick数据 当主线程处理生成tick数据后,调用 _ar.newTick(tick)写入缓存并进行tick的处理
        AsyncResponse _asynTickData = new AsyncResponse();
        
        //异步处理BarRequest数据 
        AsyncBarRequest _asynBarRequest = new AsyncBarRequest();

        //异步记录交易数据到数据库
        AsyncTransactionLoger _asynLoger = new AsyncTransactionLoger();

        public CTPMD _CTMDSrv = null;
        public CTPTrader  _CTPTrader = null;

        //数据库连接用于交易账户密码
        mysqlDB mysqldb = null;

        public CTPServer(TLServer tls,bool verb)
            : base()
        {
            //用于进行交易账号数据库认证
            mysqldb = new mysqlDB();
            //缓存记录交易日志
            _asynLoger.SendDebugEvent +=new DebugDelegate(debug);
            //数据服务
            _CTMDSrv = new CTPMD();
            //交易服务
            _CTPTrader = new CTPTrader();

            //本地历史数据管理器
            _fileDM = new QSFileDataManager("d:\\data\\");
            _fileDM.SendDebugEvent += new DebugDelegate(debug);
            //内存历史数据管理器
            _memoryDM = new QSMemoryDataManager(_fileDM);
            _memoryDM.SendDebugEvent += new DebugDelegate(debug);
            //模拟交易
            ptt = new PapertradeEngine (new IdTracker(),verb);
            //mbroker = new MiddleBroker(_CTPTrader);

            tl = tls;
            VerboseDebugging = verb;
            // use a background thread to queue up COM-events
            //bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            //bw.WorkerSupportsCancellation = true;
            //tl.SendDebugEvent += new DebugDelegate(debug);
            // set provider
            tl.newProviderName = Providers.CTP;
            //设定日志输出
            tl.VerboseDebugging = false;
            //交易员请求交易账户授权
            tl.newAcctRequest += new AccountRequestDel(tl_newAcctRequest);
            //处理Symbol数据请求
            tl.newRegisterSymbols += new SymbolRegisterDel(tl_newRegisterSymbols);
            //处理Feature请求
            tl.newFeatureRequest += new MessageArrayDelegate(tl_newFeatureRequest);
            //处理Order发送
            tl.newSendOrderRequest += new OrderDelegateStatus(tl_newSendOrderRequest);
            //处理Order取消
            tl.newOrderCancelRequest += new LongDelegate(tl_newOrderCancelRequest);
            tl.newPosList += new PositionArrayDelegate(tl_newPosList);
            //处理其他请求消息
            tl.newUnknownRequest += new UnknownMessageDelegate(tl_newUnknownRequest);
            //触发账户状态更新信息
            tl.SendAccountUpdateEvent +=new AccountUpdateDel(tl_SendAccountUpdateEvent);
            //异步处理tick数据的回调函数
            _asynTickData.GotTick += new TickDelegate(_ar_GotTick);
            //异步处理BarRequest的回调函数
            _asynBarRequest.GotBarRequest += new BarRequestDel(_asynBarRequest_GotBarRequest);

            _blt.GotNewBar += new SymBarIntervalDelegate(blt_GotNewBar);

            //启动tradelink内置服务程序
            debug("TLServer Starting");
            tl.Start();

            //数据连接与交易连接的事件触发回调函数
            _CTMDSrv.SendDebugEvent += new DebugDelegate(debug);
            _CTMDSrv.SendCTPMDReady += new VoidDelegate(CTP_DataSrvReady);
            _CTMDSrv.GotTick += new TickDelegate(CTP_GotTick);

            _CTPTrader.SendDebugEvent += new DebugDelegate(debug);
            _CTPTrader.SendCTPTraderReady += new VoidDelegate(CTP_ExectionSrvReady);
            _CTPTrader.GotOrderEvent += new OrderDelegate(_CTPTrader_GotOrderEvent);
            _CTPTrader.GotFillEvent += new FillDelegate(_CTPTrader_GotFillEvent);

            //ServerConnectedEvent += new VoidDelegate(CTPServer_onUserLogin);

            //模拟交易设定
            ptt.SendDebugEvent += new DebugDelegate(debug);
            ptt.GotOrderEvent += new OrderDelegate(ptt_newOrder);//ptt处理完order后 进行order回报
            ptt.GotCancelEvent += new LongDelegate(ppt_newCancel);//ptt取消完order后 进行cancel回报
            ptt.GotFillEvent += new FillDelegate(ppt_newFill);//ptt成交完order后 进行fill回报
            ptt.UseBidAskFills = isPaperTradeUsingBidAsk;
            

            //背对背账户
            //mbroker.SendDebugEvent += new DebugDelegate(debug);
            //mbroker.GotOrder += new OrderDelegate(tl.newOrder);
            //mbroker.GotOrderCancelEvent += new LongDelegate(tl.newCancel);
        }

       
        #region 真实交易事件触发
        void _CTPTrader_GotOrderEvent(Order o)
        {
            v("ptt send order to tlserver");
            tl.newOrder(o);
            if (GotOrder != null)
                GotOrder(o);
            //_asynLoger.newOrder(o);
        }

        void _CTPTrader_GotFillEvent(Trade t)
        {
            v("ptt send fill to tlserver");
            tl.newFill(t);
            //return;
            if (GotFill != null)
                GotFill(t);

        }


        #endregion


        #region 模拟交易事件触发
        //关于交易信息的记录
        //当client 发送 order 到 服务端  AsycnServer中的Worker线程会调用TLserver中的handlemessage 处理消息,handlemessage处理消息后会触发相关事件
        //事件触发后在CTPServer中对事件的绑定,会进行对Order的检查与papter trading.paper trading回对提交的order cancle 以及fill 反馈出来。
        //这个过程中流程比较负责,paper trading会对order进行修改,同时order记录会出现问题.因此paper trading/ asynclog在记录order fill等信息的时候需要copy新建实体。否则
        //会产生对个线程对某个对象进行访问 出现数据部同步的问题。
        //系统将交易记录储存到数据库中，储存过程是异步进行的,交易记录用于结算与分析
        //
        private void ppt_newCancel(long oid)
        {
            v("ppt send cancle to tlserver");
            tl.newCancel(oid);
            if (GotCancel != null)
                GotCancel(oid);

            //数据库记录
            _asynLoger.newCancle(oid);

        }
        private void ppt_newFill(Trade f)
        {
            v("ptt send fill to tlserver");
            tl.newFill(f);
            //return;
            if (GotFill != null)
                GotFill(f);

            _asynLoger.newTrade(f);

        }
        
        //当ppt接收到order后准备接收tick来fill order,这个时候 order又被传递到_asynloger记录order.会发生一个情况:当order filled之后 order还没有被记录,
        //所以记录order , 记录 trade 需要用copy模式
        private void ptt_newOrder(Order o)
        {
            v("ptt send order to tlserver");
            tl.newOrder(o);
            if (GotOrder != null)
                GotOrder(o);
            _asynLoger.newOrder(o);
        }
        #endregion



        //服务器登入成功后 初始化basket然后请求市场数据
        private void CTP_DataSrvReady()
        {
            debug("CTP Data Server Ready!");
            //debug("Loading Security Basket....");
            _mb = BasketTracker.getBasket();
            debug(_mb.Count.ToString() + " Securities Loaded.");

            debug("Subscribe Market Data From CTP Server....");
            _CTMDSrv.SubscribeMarketData(_mb.ToSymArray());
        }

        private void CTP_ExectionSrvReady()
        {
            debug("CTP Execution Server Ready!");
        }
        #region CTP接口接收到数据后触发出的相关事件
        //tick处理逻辑
        public void demoTick(Tick k)
        {
            CTP_GotTick(k);
        }
        //
        private void CTP_GotTick(Tick k)
        {
            //tlserver给client发送tick
            tl.newTick(k);
            //异步处理实时储存tick数据
            _asynTickData.newTick(k);
            //模拟交易引擎通过tick触发交易
            ptt.newTick(k);



        }

        #endregion

        #region CTPServer事件触发

        private void ServerConnectedHandler()
        {
            if (ServerConnectedEvent != null)
            {
                ServerConnectedEvent();
            }
        }



        #endregion


        #region BLT事件回调
        private QSMemoryDataManager _memoryDM = null;
        private QSFileDataManager _fileDM = null;
        void blt_GotNewBar(string sym, int interval)
        {
            //debug(sym+"got new bar");
            //debug(_blt[sym].RecentBar.ToString());
            //debug(_blt[sym].Symbol);
            //当有新的1分钟K线形成时,我们将其记录到系统中

        }
        #endregion

        #region 异步处理BarRequest
        void _asynBarRequest_GotBarRequest(BarRequest br)
        {

            debug("异步处理barrequest调用处理程序");
            //1.根据barrequest提供的参数我们得到客户所需要的barlist.
            int numbars = _blt[br.Symbol, br.Interval].Count;
            if (numbars == 0)
            {
                debug("There is no history bar data in cache for:" + br.Symbol);
                return;
            }

            //2.遍历所有存在的bar history然后统一发送给对应的client
            //numbars *= -1;
            debug("共计发送:"+numbars.ToString()+"Bars");
            for (int i = numbars; i > 0; i--)
            {
                try
                {
                    //debug("........................");
                    //debug(br.Symbol);
                    //debug(br.Interval.ToString());
                    Bar b = _blt[br.Symbol, br.Interval][i];
                    //debug("symbol is:" + b.Symbol);
                    //debug("interval is:" + b.Interval.ToString());
                    //检查bar是否有效 有效则发送给Client
                    if (b.isValid)
                    {
                        string msg = BarImpl.Serialize(b);
                        tl.TLSend(msg, MessageTypes.BARRESPONSE, br.Client);
                        //debug(msg);
                    }
                }
                catch (Exception ex)
                {
                    debug(ex.Message + ex.StackTrace);

                }

            }
            //string sym = br.Symbol;
            //BarList bl = _blt[sym];
            //debug(br.ToString());
            //debug(_blt[sym].Count.ToString());
            /* 
             foreach (Bar bar in bl)
             {
                 string msg = BarImpl.Serialize(bar);
                 tl.TLSend(msg, MessageTypes.BARRESPONSE, br.Client);
                 verb("send bar data to client");
             }*/


        }
        #endregion

        #region 异步处理tick回调
        //AsyncResponse中 我们在AsyncRsponse开启的单独线程中触发GotTick
        void _ar_GotTick(Tick k)
        {
            //将有可能产生延迟或者错误的操作放置到异步数据处理里面来,这样Tick处理与转发在不同的
            //线程中运行不会影响客户端接收Tick
            //debug("处理器核数：" + Environment.ProcessorCount.ToString());
            _tickArchiver.newTick(k);//储存tick数据
            _blt.newTick(k);//维护生成本地bars数据
            if (GotTick != null)
                GotTick(k);
        }

        #endregion

        #region Client-->TLServer发出请求事件触发的回调函数
        void tl_SendAccountUpdateEvent(string acc, bool w, string socketname)
        {
            if (SendAccountUpdateEvent != null)
                SendAccountUpdateEvent(acc, w, socketname);
        }
        //客户端用于请求交易账户
        string tl_newAcctRequest(string ac, int pass)
        {
            debug("交易账户验证");
            if (mysqldb.validAccount(ac, pass.ToString()))
                return ac;
            return string.Empty;
        }

        //客户端请求客户的仓位列表,当每次请求账户时候 服务端返回客户端对应交易账户的持仓列表
        Position[] tl_newPosList(string account)
        {
            //throw new NotImplementedException();
           PositionTracker pt= ClearCentre.getPositionTracker(account);
           return pt.ToArray();
            
        }

        //服务端启动时,统一请求列表中的数据,具体客户端需求的数据根据需要发送。
        void tl_newRegisterSymbols(string client, string symbols)
        {
            debug(client + "请求symbol数据: " + symbols);

        }
        void tl_newOrderCancelRequest(long val)
        {
            v("CTPServer Got CancelOrder :" + val);
            //模拟交易
            if (isPaperTradeEnabled)
            {
                ptt.sendcancel(val);
            }

        }

        long tl_newSendOrderRequest(Order o)
        {
            v("CTPServer Got Order: " + o.ToString());
            //Order风控检查
            string msg="";
            if (SendOrderRiskCheckEvent != null)
            {
                if (!SendOrderRiskCheckEvent(o, out msg))
                {
                    v("orde is out of risk control we reject it ");
                    tl.newOrderReject(o, msg);
                    return -1;
                }
            }
            //模拟交易
            if (isPaperTradeEnabled)
            {
                ptt.sendorder(o);
            }

            _CTPTrader.SendOrder(o);
           

            return 0;
        }

        long tl_newUnknownRequest(MessageTypes t, string msg)
        {
            switch (t)
            {
                //请求回补历史数据时候进行的处理
                case MessageTypes.BARREQUEST:
                    {
                        debug("CTPServer Got barrequest: " + msg);
                        try
                        {   //将barrequest储存于asyn中用于异步处理
                            BarRequest br = BarImpl.ParseBarRequest(msg);
                            _asynBarRequest.newBarRequest(br);
                        }
                        catch (Exception ex)
                        {
                            debug("error parsing bar request: " + msg);
                            debug(ex.Message + ex.StackTrace);
                        }
                        return 0;
                    }
            }
            return (long)MessageTypes.FEATURE_NOT_IMPLEMENTED;
        }

        MessageTypes[] tl_newFeatureRequest()
        {
            //debug("Client request Feature");
            // features supported by connecotr
            List<MessageTypes> f = new List<MessageTypes>();
            f.Add(MessageTypes.REGISTERSTOCK);
            f.Add(MessageTypes.TICKNOTIFY);
            f.Add(MessageTypes.LIVEDATA);
            f.Add(MessageTypes.BARRESPONSE);
            f.Add(MessageTypes.BARREQUEST);
            //if (isPaperTradeEnabled)
            {
                f.Add(MessageTypes.SIMTRADING);
                f.Add(MessageTypes.SENDORDER);
                f.Add(MessageTypes.SENDORDERLIMIT);
                f.Add(MessageTypes.SENDORDERMARKET);
                f.Add(MessageTypes.SENDORDERSTOP);
                f.Add(MessageTypes.ORDERNOTIFY);
                f.Add(MessageTypes.ORDERCANCELREQUEST);
                f.Add(MessageTypes.ORDERCANCELRESPONSE);
                f.Add(MessageTypes.EXECUTENOTIFY);
            }
            return f.ToArray();
        }
        #endregion






        void ptt_SendDebugEvent(string msg)
        {
            if (!isPaperTradeEnabled)
                return;
            v(msg);
        }

        //debug输出
        void debug(string msg)
        {
            if (SendDebugEvent != null)
                SendDebugEvent(msg);
        }
        //将消息输出
        void v(string msg)
        {
            if (!_noverb)
            {
                debug(msg);
            }
        }


       
        /// <summary>
        /// 开启服务
        /// </summary>
        public void Start()
        {
            try
            {
                // paper trading
                //ptt.GotCancelEvent += new LongDelegate(tl.newCancel);
                //ptt.GotFillEvent += new FillDelegate(tl.newFill);
                //ptt.GotOrderEvent += new OrderDelegate(tl.newOrder);
                // ptt.SendDebugEvent += new DebugDelegate(ptt_SendDebugEvent);
                // ptt.UseBidAskFills = isPaperTradeUsingBidAsk;
                debug("Start Connecting to CTP ServerSide");
                


                //初始化CTP MarketDataFeed
                //_CTMDSrv.InitCTPMD();
                //初始化交易连接
                _CTPTrader.InitCTPTrader();

                //启动TL交易服务器
                tl.Start();
                
                //InitCTPMD();

                // connect to esignal
                //esig = new Hooks();
                // handle historical bars
                //esig.OnBarsReceived += new _IHooksEvents_OnBarsReceivedEventHandler(esig_OnBarsReceived);
                // handle esignal quotes
                //esig.OnQuoteChanged += new _IHooksEvents_OnQuoteChangedEventHandler(esig_OnQuoteChanged);
            }
            catch (Exception ex)
            {
                const string url = @"http://code.google.com/p/tradelink/wiki/EsignalConfig";
                System.Diagnostics.Process.Start(url);
                debug("Exception loading esignal: " + ex.Message + ex.StackTrace); _valid = false;
                debug("For more info see: " + url);
                _valid = false;
                return;
            }
            //if ((user==null) || (user==string.Empty)) return;
            //esig.SetApplication(user);
            //_valid = esig.IsEntitled != 0;
            _valid = true;
            if (_valid)
            {
                debug("CTPServer Starting success");
                //_go = true;
                // start background processing
                //if (!bw.IsBusy)
                //    bw.RunWorkerAsync();
            }
            else
                debug("failed.");
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            // request thread be stopped
            debug("##########退出ServerCTP###################");
            //_go = false;
            if (tl != null)
                tl.Stop();
            //关闭CTPDataFeed连接
            if (_CTMDSrv.IsCTPMDLive)
            {
                debug("准备关闭CTPDataFeed");
                _CTMDSrv.ExitCTPMD();
            }
            if (_CTPTrader.IsCTPTraderLive)
            {
                debug("准备关闭CTPTrader");
                _CTPTrader.ExitCTPTrader();
            }
           
            try
            {

                // stop thread
                bw.CancelAsync();
                // release symbols
                //foreach (Security sec in _mb)
                //esig.ReleaseSymbol(sec.Symbol);
            }
            catch (Exception ex)
            {
                if (SendDebugEvent != null)
                    SendDebugEvent(ex.Message + ex.StackTrace);
            }
            // garbage collect esignal object
            //esig = null;
        }


        // see if we already subscribed to this guy
        bool contains(string sym)
        {
            foreach (Security sec in _mb)
                if (sec.Symbol == sym) return true;
            return false;
        }
    }
}
