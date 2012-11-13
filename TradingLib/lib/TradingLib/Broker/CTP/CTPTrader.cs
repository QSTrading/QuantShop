using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTP;
using TradeLink.API;
using TradeLink.Common;
using System.Threading;
using TradingLib.API;
using TradingLib.GUI.Server;
using System.Data;
using System.Windows.Forms;


namespace TradingLib.Broker.CTP
{
    public class CTPTrader:IBroker
    {
        /// <summary>
        /// 当有日志信息输出时调用
        /// </summary>
        public event DebugDelegate SendDebugEvent;
        /// <summary>
        /// 当数据服务器登入成功后调用
        /// </summary>
        public event VoidDelegate SendCTPTraderReady;


        private bool CTPTraderConnected = false;
        private bool ThreadStarted = false;
        private Thread CTPTraderThread= null;
        private bool CTPTraderLive = false;


        CTPTraderAdapter tradeApi = null;
        private string strInfo = "";				//结算信息
        private FormLoginTrade ul;//登入窗口
        
        //关于order 序列号管理 以及requestid映射
        //order ID 记录该Order
        //orderID --> iRequestID 


        string _FRONT_ADDR = "tcp://asp-sim2-front1.financial-trading-platform.com:26205";  // 前置地址
        string _BROKER_ID = "4070";                       // 经纪公司代码
        string _INVESTOR_ID = "00295";                    // 投资者代码
        string _PASSWORD = "123456";                     // 用户密码
       

        /*
        string _FRONT_ADDR = "tcp://asp-sim2-front1.financial-trading-platform.com:26205";  // 前置地址
        string _BROKER_ID = "2030";                       // 经纪公司代码
        string _INVESTOR_ID = "888888";                    // 投资者代码
        string _PASSWORD = "888888";                     // 用户密码
        */

        //string INSTRUMENT_ID = "IF1211";
        //EnumDirectionType DIRECTION = EnumDirectionType.Sell;
        //double LIMIT_PRICE = 3200;
        int iRequestID = 0;

        // 会话参数
        int FRONT_ID;	//前置编号
        int SESSION_ID;	//会话编号
        int MaxOrderRef;//最大报单引用
        

        bool ORDER_ACTION_SENT = false;		//是否发送了报单
        private fmAccountMoniter _accMoniter;
        public fmAccountMoniter AccountMoniter
        {
            set
            {
                _accMoniter = value;
                dtInstruments = _accMoniter.dtInstruments;
                _accMoniter.QueryAccountEvent += new TradeLink.API.VoidDelegate(QueryAccount);


            }
        }
        RingBuffer<ObjectAndKey> _objectCache = new RingBuffer<ObjectAndKey>(1000);
        RingBuffer<StateInfo> _stateinfoCache = new RingBuffer<StateInfo>(1000);
        void onNewState(EnumProgessState state, string info)
        {
            _stateinfoCache.Write(new StateInfo(state, info));
        }
        void onNewObject(ObjectAndKey oak)
        {
            _objectCache.Write(oak);
        }
        bool _processgo = false;
        Thread processThread = null;
        void process()
        {
            try
            {

                while (_processgo)
                {
                    while (_objectCache.hasItems)
                    {
                        ObjectAndKey oak = _objectCache.Read();

                        _accMoniter.onNewObject(oak);
                    }

                    while (_stateinfoCache.hasItems)
                    {
                        StateInfo si = _stateinfoCache.Read();
                        progressStateInfo(si.State, si.Info);

                    }

                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            { }

        }

        //日志记录
        void progressStateInfo(EnumProgessState _state, string _msg)
        {
            if (ul.Visible)
            {
                if (ul.Progressbar.Value <= 85)
                    ul.Progressbar.Value += 15;
                ul.labelState.Text = _msg;
                if (_state == EnumProgessState.OnLogin && !_msg.EndsWith("完成")) //未完成登录
                {
                    //MessageBox.Show(_msg);
                    ul.btnLogin.Enabled = true;
                    ul.Progressbar.Value = 0;
                }
            }
            else if (!(_state.ToString().StartsWith("Qry") || _state.ToString().StartsWith("OnQry"))) //查询事件不显示
            {

                _accMoniter.onNewState(_state, _msg);
                /*
                this.comboBoxErrMsg.Items.Insert(0, DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss.fff") + "|" + _state.ToString() + ":" + _msg);
                this.comboBoxErrMsg.SelectedIndex = 0;
                if (_state == EnumProgessState.OnError || _state == EnumProgessState.OnDisConnect || _state == EnumProgessState.OnErrOrderInsert ||
                    _state == EnumProgessState.OnErrOrderAction || _state == EnumProgessState.OnMdDisConnected)
                    this.toolTipInfo.ToolTipIcon = ToolTipIcon.Warning;
                else
                    this.toolTipInfo.ToolTipIcon = ToolTipIcon.Info;
                this.toolTipInfo.Show(_msg, this.tabControSystem, 0, this.tabControSystem.Height - 50, 5000);	//冒泡
                 * */
            }
            //Properties.Settings.Default.Log.Add(DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss.fff") + "|" + _state.ToString() + ":" + _msg);

            if (_state == EnumProgessState.OnConnected)		//连接断开标志:只在断开重连时有效,在首次连接时,要放在form_load中
            {
                //this.radioButtonTrade.ForeColor = Color.Green;
                //this.radioButtonTrade.Checked = true;
            }
            else if (_state == EnumProgessState.OnDisConnect)
            {
                //this.radioButtonTrade.ForeColor = Color.Red;
                //this.radioButtonTrade.Checked = false;
            }
            else if (_state == EnumProgessState.OnMdConnected) //行情连接/断开
            {
                //this.radioButtonMd.ForeColor = Color.Green;
                //this.radioButtonMd.Checked = true;
            }
            else if (_state == EnumProgessState.OnMdDisConnected)
            {
                //this.radioButtonMd.ForeColor = Color.Red;
                //this.radioButtonMd.Checked = false;
            }
            //声音
            /*
            if (Properties.Settings.Default.PlaySound)
            {
                switch (_state)
                {
                    case EnumProgessState.OnErrOrderInsert: //下单错误
                        snd = new SoundPlayer(@"Resources\指令单错误.wav");
                        snd.Play();
                        break;
                    case EnumProgessState.OnRtnOrder:
                        snd = new SoundPlayer(@"Resources\报入成功.wav");
                        snd.Play();
                        break;
                    case EnumProgessState.OnRtnTrade: //成交
                        this.listViewOrder.Sort();
                        snd = new SoundPlayer(@"Resources\成交通知.wav");
                        snd.Play();
                        break;
                    case EnumProgessState.OnErrOrderAction: //撤单错误
                        snd = new SoundPlayer(@"Resources\指令单错误.wav");
                        snd.Play();
                        break;
                    case EnumProgessState.OnOrderAction:	//撤单成功
                        snd = new SoundPlayer(@"Resources\撤单.wav");
                        snd.Play();
                        break;
                    case EnumProgessState.OnConnected:		//连接成功
                        if (!ul.Visible)	//登录后发声
                        {
                            snd = new SoundPlayer(@"Resources\信息到达.wav");
                            snd.Play();
                        }
                        break;
                    case EnumProgessState.OnDisConnect:		//连接中断
                        snd = new SoundPlayer(@"Resources\连接中断.wav");
                        snd.Play();
                        break;
                    case EnumProgessState.OnRtnTradingNotice:	//事件通知
                        snd = new SoundPlayer(@"Resources\信息到达.wav");
                        snd.Play();
                        break;
                    case EnumProgessState.OnRtnInstrumentStatus:	//合约状态
                        snd = new SoundPlayer(@"Resources\信息到达.wav");
                        snd.Play();
                        break;
                    case EnumProgessState.OnError:
                        snd = new SoundPlayer(@"Resources\指令单错误.wav");
                        snd.Play();
                        break;
                }
             * */
            //}
        }
        #region 查询线程 用于将查询任务推送到任务队列
        private DataTable dtInstruments;// = new DataTable("Instruments");			//合约用表
        private Thread threadQry = null;					//执行查询队列
        private List<QryOrder> listQry = new List<QryOrder>();	//待查询的队列
        private bool apiIsBusy = false;		//接口是否处于查询中
        private string instrument4QryRate = null;//正在查询手续费的合约:因有时返回合约类型,而加以判断

        class QryOrder
        {
            public QryOrder(EnumQryOrder _qryType, string _params = null, object _field = null)
            { this.QryOrderType = _qryType; Params = _params; Field = _field; }
            public EnumQryOrder QryOrderType { get; set; }
            public string Params = null;
            public object Field = null;
        }

        //查询类型
        enum EnumQryOrder
        {
            QryOrder, QryTrade, QryIntorverPosition, QryInstrumentCommissionRate, QryTradingAccount, QryParkedOrderAction,
            QryParkedOrder, QryContractBank, QueryBankAccountMoneyByFuture, QrySettlementInfo,
            QryHistoryTrade,
            QryTransferSerial
        }

        //查询列表
        void execQryList()
        {
            while (true)
            {
                if (apiIsBusy || this.listQry.Count == 0)
                {
                    //debug("busy"+"|"+listQry.Count.ToString());
                    Thread.Sleep(100);
                    continue;
                }
                QryOrder qry = listQry[0];
                Thread.Sleep(1000);
                apiIsBusy = true;
                switch (qry.QryOrderType)
                {
                    case EnumQryOrder.QryInstrumentCommissionRate: //手续费
                        //debug("查询手续费");
                        this.instrument4QryRate = qry.Params;		//正在查询的合约
                        this.ReqQryInstrumentCommissionRate(qry.Params);
                        break;
                    case EnumQryOrder.QryIntorverPosition:	//持仓
                        //debug("查询持仓");
                        this.ReqQryInvestorPosition(qry.Params);
                        break;
                    case EnumQryOrder.QryOrder: //查委托
                        //debug("查询委托");
                        this.ReqQryOrder();
                        break;
                    case EnumQryOrder.QryParkedOrder:	//查预埋
                        //debug("查询预埋");
                        //this.tradeApi.ReqQryParkedOrder();
                        break;
                    case EnumQryOrder.QryParkedOrderAction:
                        //debug("查询委托操作");
                        //this.tradeApi.ReqQryParkedOrderAction();
                        break;
                    case EnumQryOrder.QrySettlementInfo:
                        //debug("查询结算信息");
                        this.ReqQrySettlementInfo();
                        break;
                    case EnumQryOrder.QryTrade:
                        //debug("查询成交");
                        this.ReqQryTrade();
                        break;
                    //case EnumQryOrder.QryHistoryTrade:
                    //    this.tradeApi.QryTrade(this.dateTimePickerStart.Value, this.dateTimePickerEnd.Value);	//查历史成交
                    //    break;
                    case EnumQryOrder.QryTradingAccount:
                        //debug("查询交易账户");
                        this.ReqQryTradingAccount();
                        break;
                    case EnumQryOrder.QryTransferSerial:
                        //debug("查询转账");
                        //this.tradeApi.ReqQryTransferSerial(qry.Params);
                        break;
                    case EnumQryOrder.QueryBankAccountMoneyByFuture:
                        //this.tradeApi.ReqQueryBankAccountMoneyByFuture((CThostFtdcReqQueryAccountField)qry.Field);
                        break;
                    default:
                        apiIsBusy = false;	//恢复正常查询
                        break;
                }
                listQry.Remove(qry);
            }
        }
        #endregion

        //日志输出函数
        private void debug(string s)
        {
            if (SendDebugEvent != null)
                SendDebugEvent(s);
        }
        public CTPTrader()
        {
            _processgo = true;
            processThread = new Thread(new ThreadStart(process));
            processThread.Start();
        }
        //初始化接口 连接交易服务器
        public void InitCTPTrader()
        {
            if (CTPTraderConnected != true)
            {
                debug("Initing CTP Trader Connection");
                debug("CTP Trader Server Address:" + _FRONT_ADDR.ToString());

                ul = new FormLoginTrade();
                ul.btnLogin.Click += new EventHandler(btnLogin_Click); //登录画面:按钮事件
                ul.btnExit.Click += new EventHandler(btnExit_Click);
                if (ul.ShowDialog() == DialogResult.OK)
                {
                    if (strInfo != string.Empty)	//在userlogin中调用qrysettleinfo确保此条件成立
                    {
                        //显示确认结算窗口
                        using (SettleInfo info = new SettleInfo())
                        {
                            info.richTextInfo.Text = strInfo;
                            if (info.ShowDialog() != DialogResult.OK)//注有结算信息就会弹出结算对话框，如果确认则往下运行，如果不确认则直接返回。
                            {
                                //this.Close();
                                debug("初始化连接失败");
                                //tradeApi.DisConnect();
                                CTPTraderConnected = false;
                                return;//直接返回
                            }
                        }
                    }
                }

                onNewState(EnumProgessState.SettleConfirm, "确认结算...");
                this.ReqSettlementInfoConfirm();	//确认结算

                //debug("登入完成后执行后续操作");
                Thread.Sleep(2000);
                this.listQry.Insert(0, new QryOrder(EnumQryOrder.QryIntorverPosition, null));//持仓
                this.listQry.Insert(0, new QryOrder(EnumQryOrder.QryTrade, null));	//成交
                this.listQry.Insert(0, new QryOrder(EnumQryOrder.QryOrder, null));//委托

                //初始化服务线程
                this.threadQry = new Thread(new ThreadStart(execQryList));
                this.threadQry.Start();				//刷新查询

                
            
            }
        
        }
        //登录:生成tradeApi/注册事件
        void btnLogin_Click(object sender, EventArgs e)
        {
            (sender as Button).Enabled = false;
            //if (ul.cbServer.SelectedIndex >= 0)
            {
                //string[] server = Properties.Settings.Default.Servers[ul.cbServer.SelectedIndex].Split('|');
                //if (server.Length >= 8)
                {
                    debug("we logined here");
                    string path = System.IO.Directory.GetCurrentDirectory();
                    path = System.IO.Path.Combine(path, "Cache4Trade\\");
                    System.IO.Directory.CreateDirectory(path);
                    tradeApi = new CTPTraderAdapter(path, false);
                    regEvents();//注册事件
                    // 注册私有流
                    CTPTraderConnected = true;
                    tradeApi.RegisterFront(_FRONT_ADDR);
                    tradeApi.Init();
                    onNewState(EnumProgessState.Connect, "连接...");
                    //tradeApi.Connect();
                }
                //else
                {
                    //MessageBox.Show("服务器配置有误!");
                    //(sender as Button).Enabled = true;
                }
            }
        }
        //退出
        void btnExit_Click(object sender, EventArgs e)
        {
            //this.Close();
        }
        //注册tradeapi事件:包括4trade的事件,两个api在此区分功能
        void regEvents()
        {
            tradeApi.OnRspError += new RspError(tradeApi_OnRspError);
            tradeApi.OnRspUserLogin += new RspUserLogin(tradeApi_OnRspUserLogin);

            tradeApi.OnFrontConnected +=new FrontConnected(tradeApi_OnFrontConnected);
            tradeApi.OnFrontDisconnected +=new FrontDisconnected(tradeApi_OnFrontDisconnected);
            tradeApi.OnErrRtnOrderAction += new ErrRtnOrderAction(tradeApi_OnErrRtnOrderAction);
            tradeApi.OnErrRtnOrderInsert += new ErrRtnOrderInsert(tradeApi_OnErrRtnOrderInsert);

            tradeApi.OnHeartBeatWarning += new HeartBeatWarning(tradeApi_OnHeartBeatWarning);
            tradeApi.OnRspOrderAction += new RspOrderAction(tradeApi_OnRspOrderAction);
            tradeApi.OnRspOrderInsert += new RspOrderInsert(tradeApi_OnRspOrderInsert);
            tradeApi.OnRtnOrder += new RtnOrder(tradeApi_OnRtnOrder);
            tradeApi.OnRtnTrade += new RtnTrade(tradeApi_OnRtnTrade);


            //tradeApi.OnRspQryBrokerTradingAlgos += new TradeApi.RspQryBrokerTradingAlgos(tradeApi_OnRspQryBrokerTradingAlgos);
            //tradeApi.OnRspQryBrokerTradingParams += new TradeApi.RspQryBrokerTradingParams(tradeApi_OnRspQryBrokerTradingParams);
            //tradeApi.OnRspQryCFMMCTradingAccountKey += new TradeApi.RspQryCFMMCTradingAccountKey(tradeApi_OnRspQryCFMMCTradingAccountKey);
            //tradeApi.OnRspQryDepthMarketData += new TradeApi.RspQryDepthMarketData(tradeApi_OnRspQryDepthMarketData);
            //tradeApi.OnRspQryExchange += new TradeApi.RspQryExchange(tradeApi_OnRspQryExchange);
            tradeApi.OnRspQryInstrument += new RspQryInstrument(tradeApi_OnRspQryInstrument);//查询合约回报
            tradeApi.OnRspQryInstrumentCommissionRate += new RspQryInstrumentCommissionRate(tradeApi_OnRspQryInstrumentCommissionRate);
            tradeApi.OnRspQryInstrumentMarginRate += new RspQryInstrumentMarginRate(tradeApi_OnRspQryInstrumentMarginRate);
            //tradeApi.OnRspQryInvestor += new TradeApi.RspQryInvestor(tradeApi_OnRspQryInvestor);

            //tradeApi.OnRspQryInvestorPositionCombineDetail += new TradeApi.RspQryInvestorPositionCombineDetail(tradeApi_OnRspQryInvestorPositionCombineDetail);
            //tradeApi.OnRspQryInvestorPositionDetail += new TradeApi.RspQryInvestorPositionDetail(tradeApi_OnRspQryInvestorPositionDetail);
            //tradeApi.OnRspQryNotice += new TradeApi.RspQryNotice(tradeApi_OnRspQryNotice);

            tradeApi.OnRspQrySettlementInfo += new RspQrySettlementInfo(tradeApi_OnRspQrySettlementInfo);//查询结算信息
            tradeApi.OnRspQrySettlementInfoConfirm += new RspQrySettlementInfoConfirm(tradeApi_OnRspQrySettlementInfoConfirm);//查询结算确认信息
            tradeApi.OnRspQryTrade += new RspQryTrade(tradeApi_OnRspQryTrade);//查询成交
            tradeApi.OnRspQryOrder += new RspQryOrder(tradeApi_OnRspQryOrder);//查询委托
            tradeApi.OnRspQryInvestorPosition += new RspQryInvestorPosition(tradeApi_OnRspQryInvestorPosition);//查询仓位
            tradeApi.OnRspQryTradingAccount += new RspQryTradingAccount(tradeApi_OnRspQryTradingAccount);//查询交易账户

            //tradeApi.OnRspQryTradingCode += new TradeApi.RspQryTradingCode(tradeApi_OnRspQryTradingCode);
            //tradeApi.OnRspQryTradingNotice += new TradeApi.RspQryTradingNotice(tradeApi_OnRspQryTradingNotice);
            //tradeApi.OnRspQueryMaxOrderVolume += new TradeApi.RspQueryMaxOrderVolume(tradeApi_OnRspQueryMaxOrderVolume);
            tradeApi.OnRspSettlementInfoConfirm += new RspSettlementInfoConfirm(tradeApi_OnRspSettlementInfoConfirm);
            //tradeApi.OnRspTradingAccountPasswordUpdate += new TradeApi.RspTradingAccountPasswordUpdate(tradeApi_OnRspTradingAccountPasswordUpdate);
            //tradeApi.OnRspUserLogout += new TradeApi.RspUserLogout(tradeApi_OnRspUserLogout);
            //tradeApi.OnRspUserPasswordUpdate += new TradeApi.RspUserPasswordUpdate(tradeApi_OnRspUserPasswordUpdate);
            //tradeApi.OnRtnErrorConditionalOrder += new TradeApi.RtnErrorConditionalOrder(tradeApi_OnRtnErrorConditionalOrder);
            //tradeApi.OnRtnInstrumentStatus += new TradeApi.RtnInstrumentStatus(tradeApi_OnRtnInstrumentStatus);
            //tradeApi.OnRtnTradingNotice += new TradeApi.RtnTradingNotice(tradeApi_OnRtnTradingNotice);
            //银期转帐
            //tradeApi.OnRspQryContractBank += new TradeApi.RspQryContractBank(tradeApi_OnRspQryContractBank);
            //tradeApi.OnRspQryTransferBank += new TradeApi.RspQryTransferBank(tradeApi_OnRspQryTransferBank);
            //tradeApi.OnRspFromFutureToBankByFuture += new TradeApi.RspFromFutureToBankByFuture(tradeApi_OnRspFromFutureToBankByFuture);
            //tradeApi.OnRtnFromFutureToBankByFuture += new TradeApi.RtnFromFutureToBankByFuture(tradeApi_OnRtnFromFutureToBankByFuture);
            //tradeApi.OnErrRtnFutureToBankByFuture += new TradeApi.ErrRtnFutureToBankByFuture(tradeApi_OnErrRtnFutureToBankByFuture);
            //tradeApi.OnRspFromBankToFutureByFuture += new TradeApi.RspFromBankToFutureByFuture(tradeApi_OnRspFromBankToFutureByFuture);
            //tradeApi.OnRtnFromBankToFutureByFuture += new TradeApi.RtnFromBankToFutureByFuture(tradeApi_OnRtnFromBankToFutureByFuture);
            //查银行余额
            //tradeApi.OnRspQueryBankAccountMoneyByFuture += new TradeApi.RspQueryBankAccountMoneyByFuture(tradeApi_OnRspQueryBankAccountMoneyByFuture);
            //tradeApi.OnRtnQueryBankBalanceByFuture += new TradeApi.RtnQueryBankBalanceByFuture(tradeApi_OnRtnQueryBankBalanceByFuture);
            //查转帐
            //tradeApi.OnRspQryTransferSerial += new TradeApi.RspQryTransferSerial(tradeApi_OnRspQryTransferSerial);
            //预埋单
            //tradeApi.OnRspQryParkedOrder += new TradeApi.RspQryParkedOrder(tradeApi_OnRspQryParkedOrder);
            //tradeApi.OnRspQryParkedOrderAction += new TradeApi.RspQryParkedOrderAction(tradeApi_OnRspQryParkedOrderAction);
            //tradeApi.OnRspParkedOrderInsert += new TradeApi.RspParkedOrderInsert(tradeApi_OnRspParkedOrderInsert);
            //tradeApi.OnRspParkedOrderAction += new TradeApi.RspParkedOrderAction(tradeApi_OnRspParkedOrderAction);
            //tradeApi.OnRspRemoveParkedOrder += new TradeApi.RspRemoveParkedOrder(tradeApi_OnRspRemoveParkedOrder);
            //tradeApi.OnRspRemoveParkedOrderAction += new TradeApi.RspRemoveParkedOrderAction(tradeApi_OnRspRemoveParkedOrderAction);
        }
        void ThreadFunc()
        {
            try
            {
                tradeApi.Join();
            }
            catch (Exception e)
            {
                //_LastError = e.Message;
                tradeApi.Release();
            }
        }

        // 开始启动线程进行市场数据监听
        public bool RunCTPTrader()
        {
            //DebugPrintFunction(new StackTrace(false));
            //MessageBox.Show("xxxx");
            // Called by RightEdge to initiate the data watch.
            debug("CTPTrader:开始监听数据");
            if (CTPTraderConnected == false)
                InitCTPTrader();

            if (!ThreadStarted)
            {
                ThreadStarted = true;
                // Start a new thread for our random data.
                CTPTraderThread = new Thread(new ThreadStart(ThreadFunc));
                CTPTraderThread.IsBackground = true;
                CTPTraderThread.Start();
                //Thread.Sleep(1000);
                //MessageBox.Show("线程工作状态：" + CTPMDThread.IsAlive.ToString());
            }
            return true;
        }
        public bool CTPTraderDispose()
        {
            //DebugPrintFunction(new StackTrace());
            debug("CTPAdapter断开服务器连接");
            if (tradeApi != null)
            {
                debug("CTPAdapter release");
                //mdAdapter.Dispose();
                //mdAdapter.
                tradeApi.Release();
                //MessageBox.Show(mdAdapter.ToString());
                //Process.GetCurrentProcess().Kill();
                CTPTraderConnected = false;
                tradeApi = null;
            }
            else
            {
                //isConnected = false;
            }

            return true;
        }

        public bool IsCTPTraderLive
        {
            // Return the state of the service.  If it is currently
            // listening/watching for ticks, return true.
            get { return CTPTraderLive; }
        }
        public bool IsLive { get { return IsCTPTraderLive; } }
        // 停止监听线程
        //注意当有数据请求的时候,我们无法正常关闭监听线程,在停止线程前我们需要正常关闭各个业务请求
        public bool ExitCTPTrader()
        {
            //DebugPrintFunction(new StackTrace(false));

            // Called by RightEdge to stop watching/listening for data.
            //log.Info("停止监听数据");
            if (ThreadStarted)
            {
                //unSubscribeMarketData(syms);
                //ReqUserLogout();

                //log.Info("停止正在进行中的监听......");
                ThreadStarted = false;
                // If running, abort the thread.

                CTPTraderDispose();
                //debug(CTPMDThread.ToString());
                //CTPMDThread.Abort();

                if (CTPTraderThread != null && !CTPTraderThread.Join(200))
                {   //log.Info("线程退出");
                    debug("结束工作线程");
                    CTPTraderThread.Abort();

                }

                //Thread.Sleep(1000);
                //MessageBox.Show("线程工作状态：" + CTPMDThread.IsAlive.ToString());
                CTPTraderThread = null;
                //log.Info("thread = null");
                //mdAdapter.Release();

            }
            return true;
        }


    
        bool IsErrorRspInfo(ThostFtdcRspInfoField pRspInfo)
        {
            // 如果ErrorID != 0, 说明收到了错误的响应
            bool bResult = ((pRspInfo != null) && (pRspInfo.ErrorID != 0));
            if (bResult)
                debug("--->>> ErrorID="+pRspInfo.ErrorID+",ErrorMsg="+pRspInfo.ErrorMsg);
            return bResult;
        }

        //检查是否是系统的Order orderref是动态变化的,我们需要有相应的管理机制来管理orderID
        bool IsMyOrder(ThostFtdcOrderField pOrder)
        {/*
            return ((pOrder.FrontID == FRONT_ID) &&
                    (pOrder.SessionID == SESSION_ID) &&
                    (pOrder.OrderRef == ORDER_REF));
          * **/
            return true;
        }

        //检查该order是否是有效委托
        bool IsTradingOrder(ThostFtdcOrderField pOrder)
        {/*
            return ((pOrder.OrderStatus != EnumOrderStatusType.PartTradedNotQueueing) &&
                    (pOrder.OrderStatus != EnumOrderStatusType.Canceled) &&
                    (pOrder.OrderStatus != EnumOrderStatusType.AllTraded));
          * */
            return true;
        }
        /*
        void DebugPrintFunc(StackTrace stkTrace)
        {
            string s = stkTrace.GetFrame(0).ToString();
            s = s.Split(new char[] { ' ' })[0];
            Debug.WriteLine("--->>> " + s);
            Console.WriteLine("--->>> " + s);
        }
         * */


        #region 接口回调处理
        #region 接口事件:连接/断开/登录/注销/查结算确认结果/查结算信息/确认结算/查合约
        //连接
        void tradeApi_OnFrontConnected()
        {
            debug("CTPTrader:前置连接回报");
            onNewState(EnumProgessState.OnConnected, "连接完成");
            onNewState(EnumProgessState.Login, "登入...");
            RunCTPTrader();//运行监听线程
            ReqUserLogin();//请求用户注册 用户注册后会自动接收到委托以及成交信息
        }
        //断开连接
        void tradeApi_OnFrontDisconnected(int nReason)
        {
            //DebugPrintFunc(new StackTrace());
            debug("--->>> Reason = " + nReason.ToString());
        }

        //登录响应:查询结算确认结果
        void tradeApi_OnRspUserLogin(ThostFtdcRspUserLoginField pRspUserLogin,ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (bIsLast)
            {
                debug("CTPTrader:用户登入回报");
                if (IsErrorRspInfo(pRspInfo))
                    onNewState(EnumProgessState.OnLogin, pRspInfo.ErrorMsg);
                else
                {
                    onNewState(EnumProgessState.OnLogin, "登入完成");
                    FRONT_ID = pRspUserLogin.FrontID;
                    SESSION_ID = pRspUserLogin.SessionID;
                    if (!string.IsNullOrEmpty(pRspUserLogin.MaxOrderRef))
                    MaxOrderRef = Convert.ToInt32(pRspUserLogin.MaxOrderRef);
                    debug("最大报单引用:" + pRspUserLogin.MaxOrderRef.ToString());
                    //时间设置
                    /*
                    try
                    {
                        tsSHFE = DateTime.Now.TimeOfDay - TimeSpan.Parse(pRspUserLogin.SHFETime);
                    }
                    catch
                    {
                        tsSHFE = new TimeSpan(0, 0, 0);
                    }
                    try
                    {
                        tsCZCE = DateTime.Now.TimeOfDay - TimeSpan.Parse(pRspUserLogin.CZCETime);
                    }
                    catch
                    {
                        tsCZCE = tsSHFE;
                    }
                    try
                    {
                        tsDCE = DateTime.Now.TimeOfDay - TimeSpan.Parse(pRspUserLogin.DCETime);
                    }
                    catch
                    {
                        tsDCE = tsSHFE;
                    }
                    try
                    {
                        tsCFFEX = DateTime.Now.TimeOfDay - TimeSpan.Parse(pRspUserLogin.FFEXTime);
                    }
                    catch
                    {
                        tsCFFEX = tsSHFE;
                    }
                    **/
                    if (ul.Visible)	//登录:首次连接才执行确认
                    {
                        debug("首次连接 执行确认");
                        onNewState(EnumProgessState.QrySettleConfirmInfo, "查结算确认结果...");
                        //Properties.Settings.Default.Servers[ul.cbServer.SelectedIndex] = Properties.Settings.Default.Servers[ul.cbServer.SelectedIndex].
                        //this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.QrySettleConfirmInfo, "查结算确认结果...");
                        //this.ReqSettlementInfoConfirm();//查询结算信息确认
                        this.ReqQrySettlementInfoConfirm();//查询结算信息确认
                    }

                    //tradeApi.QrySettlementInfoConfirm();//查询结算信息确认
                    //doAfterLogin();
                }
            }
        }

        //查询确认结算响应:没确认过,则查询结算信息予以确认
        void tradeApi_OnRspQrySettlementInfoConfirm(ThostFtdcSettlementInfoConfirmField pSettlementInfoConfirm, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            debug("CTPTrader:查询确认结果回报");
            if (bIsLast && !IsErrorRspInfo(pRspInfo))
            {
                //debug("确认结算信息回报");
                onNewState(EnumProgessState.OnQrySettleConfirmInfo, "查确认结果完成");
                //this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.OnQrySettleConfirmInfo, "查询确认结果完成.");
                Thread.Sleep(1000);
                //今天确认过:不再显示确认信息
                if (pSettlementInfoConfirm.BrokerID != "" && DateTime.ParseExact(pSettlementInfoConfirm.ConfirmDate, "yyyyMMdd", null) >= DateTime.Today)
                {
                    debug("今天已经确认过结算信息,我们查询合约");
                    onNewState(EnumProgessState.QryInstrument, "查合约...");
                    //    this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.QryInstrument, "查合约...");
                    this.ReqQryInstrument();
                }
                else//如果没有确认过则查询结算信息
                {
                    debug("没有结算过,我们查询结算信息");
                    onNewState(EnumProgessState.OnQrySettleInfo, "查结算信息...");
                    //  this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.OnQrySettleInfo, "查结算信息...");
                    this.ReqQrySettlementInfo();	//查结算信息
                    
                    
                }
            }
            if (IsErrorRspInfo(pRspInfo))
            {
                onNewState(EnumProgessState.OnQrySettleInfo, pRspInfo.ErrorMsg);
                //this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.OnQrySettleInfo, pRspInfo.ErrorMsg);
                //this.Close();
            }

        }
        //查结算信息响应:查合约
        void tradeApi_OnRspQrySettlementInfo(ThostFtdcSettlementInfoField pSettlementInfo,ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            debug("CTPTrader:查询结算信息回报");
            strInfo += pSettlementInfo.Content;
            if (bIsLast)
            {
                if (strInfo == string.Empty)	//无结算
                    onNewState(EnumProgessState.OnError, "无结算信息");
                //this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.OnError, "无结算信息.");
                else
                    onNewState(EnumProgessState.OnQrySettleInfo, "查询确认信息完成");
                //this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.OnQrySettleInfo, "查询确认信息完成.");
                if (!IsErrorRspInfo(pRspInfo))
                {
                    if (ul.Visible)
                    {

                        Thread.Sleep(1000);
                        onNewState(EnumProgessState.QryInstrument, "查合约...");
                        //this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.QryInstrument, "查合约...");
                        this.ReqQryInstrument();
                    }
                    else //登录后,查历史结算
                    {
                        //this.BeginInvoke(new Action(showHistorySettleInfo));
                    }
                }
                else
                {
                    onNewState(EnumProgessState.OnQrySettleInfo, pRspInfo.ErrorMsg);
                    //this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.OnQrySettleInfo, pRspInfo.ErrorMsg);
                    //this.Close();
                }
                this.apiIsBusy = false;
            }
        }
        //查手续费响应
        void tradeApi_OnRspQryInstrumentCommissionRate(ThostFtdcInstrumentCommissionRateField pInstrumentCommissionRate,ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (bIsLast)
            {
                //debug("手续费查询回报");
                if (!IsErrorRspInfo(pRspInfo) && (pInstrumentCommissionRate!=null) && this.instrument4QryRate.StartsWith(pInstrumentCommissionRate.InstrumentID))
                {
                    DataRow dr = this.dtInstruments.Rows.Find(this.instrument4QryRate);
                    if (dr == null) //无此合约,查下一个
                    {
                        this.apiIsBusy = false;	//查询完成
                    }
                    else
                    {
                        if (pInstrumentCommissionRate.OpenRatioByMoney == 0) //手续费率=0:手续费值
                        {
                            dr["手续费"] = pInstrumentCommissionRate.OpenRatioByVolume + pInstrumentCommissionRate.CloseTodayRatioByVolume;	//手续费
                            //this.BeginInvoke(new Action<string, string, string>(setInstrumentSelectedCellStyle), "手续费", (string)dr["合约"], "F2");
                            dr["手续费-平仓"] = pInstrumentCommissionRate.CloseRatioByVolume;
                            //this.BeginInvoke(new Action<string, string, string>(setInstrumentSelectedCellStyle), "手续费", (string)dr["合约"], "F2");
                        }
                        else
                        {
                            dr["手续费"] = pInstrumentCommissionRate.OpenRatioByMoney + pInstrumentCommissionRate.CloseTodayRatioByMoney;	//手续费率
                            //this.BeginInvoke(new Action<string, string, string>(setInstrumentSelectedCellStyle), "手续费", (string)dr["合约"], "P3");
                            dr["手续费-平仓"] = pInstrumentCommissionRate.CloseRatioByMoney;
                            //this.BeginInvoke(new Action<string, string, string>(setInstrumentSelectedCellStyle), "手续费-平仓", (string)dr["合约"], "P3");
                        }
                        Thread.Sleep(1000);
                        this.ReqQryInstrumentMarginRate((string)dr["合约"]);
                        //this.tradeApi.QryInstrumentMarginRate((string)dr["合约"]);
                    }
                }
                else
                    this.apiIsBusy = false;
            }
        }
        //查保证金响应
        void tradeApi_OnRspQryInstrumentMarginRate(ThostFtdcInstrumentMarginRateField pInstrumentMarginRate,ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            debug("保证金查询相应");
            DataRow dr = this.dtInstruments.Rows.Find(pInstrumentMarginRate.InstrumentID);
            if (dr != null)
            {
                if (pInstrumentMarginRate.IsRelative == EnumBoolType.No) //交易所收取总额度
                {
                    dr["保证金-多"] = pInstrumentMarginRate.LongMarginRatioByMoney;
                    dr["保证金-空"] = pInstrumentMarginRate.ShortMarginRatioByMoney;
                }
                else //相对交易所收取
                {
                    dr["保证金-多"] = (double)this.dtInstruments.Rows.Find(pInstrumentMarginRate.InstrumentID)["保证金-多"] + pInstrumentMarginRate.LongMarginRatioByMoney;
                    dr["保证金-空"] = (double)this.dtInstruments.Rows.Find(pInstrumentMarginRate.InstrumentID)["保证金-空"] + pInstrumentMarginRate.ShortMarginRatioByMoney;
                }
            }
            if (bIsLast)
            {
                this.apiIsBusy = false;	//查询完成
            }

        }
        //设置单元格样式
        void showHistorySettleInfo()
        { //this.richTextBoxSettleInfo.Text = strInfo; strInfo = string.Empty; 
        }//清空信息
        // 查合约响应
        void tradeApi_OnRspQryInstrument(ThostFtdcInstrumentField pInstrument,ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            debug("查合约回报");
            if (!IsErrorRspInfo(pRspInfo))
            {


                //合约/名称/交易所/合约数量乘数/最小波动/多头保证金率/空头保证金率/手续费/限价单下单最大量/最小量/自选
                DataRow drInstrument;
                //如果合约表中没有对应的合约,我们将合约插入进去，准备查询手续费
                if ((drInstrument = this.dtInstruments.Rows.Find(pInstrument.InstrumentID)) == null)
                    drInstrument = this.dtInstruments.Rows.Add(pInstrument.InstrumentID, pInstrument.InstrumentName, pInstrument.ExchangeID, pInstrument.VolumeMultiple, pInstrument.PriceTick
                        , pInstrument.LongMarginRatio, pInstrument.ShortMarginRatio, 0, pInstrument.MaxLimitOrderVolume, pInstrument.MinMarketOrderVolume, false);
                //"合约","名称","交易所"最新价""涨跌","涨幅","现手","总手","持仓","仓差","买价","买量","卖价","卖量","均价","最高","最低","涨停","跌停","开盘",
                //"昨结","时间","时间差,"自选",
                //DataRow drMartketData = this.dtMarketData.Rows.Add(pInstrument.InstrumentID, pInstrument.InstrumentName, pInstrument.ExchangeID, 0, 0, 0,
                //0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", 0, false, false);
                //是否在自选中
                //if (Properties.Settings.Default.Instruments.IndexOf(pInstrument.InstrumentID) >= 0)
                //{
                //    drInstrument["自选"] = true;
                //    drMartketData["自选"] = true;
                //    this.listQry.Insert(0, new QryOrder(EnumQryOrder.QryInstrumentCommissionRate, drInstrument["合约"].ToString())); //待查手续费
                //}
                //else
                this.listQry.Add(new QryOrder(EnumQryOrder.QryInstrumentCommissionRate, drInstrument["合约"].ToString()));	//非自选,放在后面
                //if (pInstrument.InstrumentID.StartsWith("SP"))
                //{
                //    drMartketData["套利"] = true;
                //    drInstrument["套利"] = true;
                // }
                // */
                //this.listQry.Insert(0, new QryOrder(EnumQryOrder.QryInstrumentCommissionRate,pInstrument.InstrumentID)); //待查手续费
                if (bIsLast)
                {
                    onNewState(EnumProgessState.OnQryInstrument, "合约查询完成");
                    //this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.OnQryInstrument, "合约查询完成.");
                    ul.DialogResult = System.Windows.Forms.DialogResult.OK;	//退出登入窗口
                    apiIsBusy = false;
                }

            }
        }

        //确认结算响应
        void tradeApi_OnRspSettlementInfoConfirm(ThostFtdcSettlementInfoConfirmField pSettlementInfoConfirm,ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            debug("确认结算响应");
            if (!IsErrorRspInfo(pRspInfo))
            {
                onNewState(EnumProgessState.OnSettleConfirm, "确认结算完成");
                //Thread.Sleep(1000);
                //this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.QryPosition, "查持仓...");
                //tradeApi.QryInvestorPosition();	//查持仓
            }
            else
            {
                onNewState(EnumProgessState.OnSettleConfirm, pRspInfo.ErrorMsg);
                //this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.OnSettleConfirm, pRspInfo.ErrorMsg); this.Close(); }
            }

        }
        //用户注销
        void tradeApi_OnRspUserLogout(ThostFtdcUserLogoutField pUserLogout,ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (bIsLast)
            {
                //if (pRspInfo.ErrorID != 0)

                //this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.OnLogout, pRspInfo.ErrorMsg);
                //IsErrorRspInfo(pRspInfo);
            }
        }
        //回报错误
        void tradeApi_OnRspError(ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            //IsErrorRspInfo(pRspInfo);
            if (IsErrorRspInfo(pRspInfo))
                onNewState(EnumProgessState.OnError, pRspInfo.ErrorMsg);
            //this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.OnError, pRspInfo.ErrorMsg);
        }

        void tradeApi_OnHeartBeatWarning(int pTimeLapes)
        {
            //showStructInListView();
        }

        void tradeApi_OnDisConnected(int reason)
        {
            debug("CTPTrader断开连接");
            //this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.OnDisConnect, "断开");
        }
        #endregion

        #region 响应: 查询委托/成交/持仓/资金
        // 查委托响应
        void tradeApi_OnRspQryOrder(ThostFtdcOrderField pOrder,ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            //debug("委托回报");
            if (!IsErrorRspInfo(pRspInfo))
            {
                if ((pOrder!=null)&&pOrder.BrokerID != "")
                    onNewObject(new ObjectAndKey(pOrder, pOrder.FrontID + "," + pOrder.SessionID + "," + pOrder.OrderRef));
                //_accMoniter.OnRtnOrder(ref pOrder);
                //this.BeginInvoke(new Action<ObjectAndKey>(showStructInListView), new ObjectAndKey(pOrder, pOrder.FrontID + "," + pOrder.SessionID + "," + pOrder.OrderRef));
                if (bIsLast)
                {
                    //debug("查询报单完成");
                    onNewState(EnumProgessState.OnQryOrder, "查报单完成");
                    //this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.OnQryOrder, "查报单完成");
                    //if (pOrder.BrokerID != "")	//返回空记录不再查成交
                    //{
                    //Thread.Sleep(1000);
                    //this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.QryTrade, "查成交...");
                    //tradeApi.QryTrade();	//查成交
                    //}
                }
            }
            else
                onNewState(EnumProgessState.OnQryOrder, pRspInfo.ErrorMsg);
            //IsErrorRspInfo(pRspInfo);
            //this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.OnQryOrder, pRspInfo.ErrorMsg);
            if (bIsLast)
                this.apiIsBusy = false;	//查询完成

        }

        // 查询成交响应
        void tradeApi_OnRspQryTrade(ThostFtdcTradeField pTrade,ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {

            if (!IsErrorRspInfo(pRspInfo))
            {
                if ((pTrade!=null)&&(pTrade.BrokerID != ""))
                    onNewObject(new ObjectAndKey(pTrade, pTrade.OrderSysID));
                //this.BeginInvoke(new Action<ObjectAndKey>(showStructInListView), new ObjectAndKey(pTrade, pTrade.OrderSysID));
                if (bIsLast)
                {
                    if ((pTrade!=null)&&(pTrade.BrokerID != ""))
                        onNewState(EnumProgessState.OnQryTrade, "查成交完成");
                    //this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.OnQryTrade, "查成交完成");
                    else
                        onNewState(EnumProgessState.OnError, "无成交");
                    //this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.OnError, "无成交");
                }
            }
            else
                onNewState(EnumProgessState.OnQryTrade, pRspInfo.ErrorMsg);
            //this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.OnQryTrade, pRspInfo.ErrorMsg);
            if (bIsLast)
            {
                this.apiIsBusy = false;	//查询完成
            }

        }

        // 查持仓汇总响应
        void tradeApi_OnRspQryInvestorPosition(ThostFtdcInvestorPositionField pInvestorPosition,ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {

            if (!IsErrorRspInfo(pRspInfo))
            {
                if (pInvestorPosition.BrokerID != "")
                    onNewObject(new ObjectAndKey(pInvestorPosition, pInvestorPosition.InstrumentID + pInvestorPosition.PosiDirection + pInvestorPosition.PositionDate));
                //this.BeginInvoke(new Action<ObjectAndKey>(showStructInListView), );
            }
            else
                onNewState(EnumProgessState.OnQryPosition, pRspInfo.ErrorMsg);
            //this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.OnQryPosition, pRspInfo.ErrorMsg);
            if (bIsLast)
            {
                onNewState(EnumProgessState.OnQryPosition, "查持仓完成");
                onNewState(EnumProgessState.OnQryPosition, "查资金...");
                //this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.OnQryPosition, "查持仓完成");
                //this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.OnQryPosition, "查资金...");
                this.listQry.Insert(0, new QryOrder(EnumQryOrder.QryTradingAccount, null));//查资金
                this.apiIsBusy = false;	//查询完成
            }

        }
        // 查持仓明细响应 == 暂时未用
        void tradeApi_OnRspQryInvestorPositionDetail(ThostFtdcInvestorPositionDetailField pInvestorPositionDetail,ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            /*
            if (pRspInfo.ErrorID == 0)
            {
                this.BeginInvoke(new Action<ObjectAndKey>(showStructInListView), new ObjectAndKey(pInvestorPositionDetail, pInvestorPositionDetail.InstrumentID + pInvestorPositionDetail.Direction + pInvestorPositionDetail.TradeID));
                if (bIsLast)
                {
                    this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.OnQryPositionDetail, "查持仓明细完成");
                }
            }
            else
                this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.OnQryPositionDetail, pRspInfo.ErrorMsg);
             * **/
        }

        // 查询帐户资金响应
        void tradeApi_OnRspQryTradingAccount(ThostFtdcTradingAccountField pTradingAccount,ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {

            if (!IsErrorRspInfo(pRspInfo))
            {
                if (bIsLast)
                {
                    onNewState(EnumProgessState.OnQryAccount, "查资金完成");
                    _accMoniter.FreshAccount(pTradingAccount);
                    //this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.OnQryAccount, "查资金完成");
                    //this.BeginInvoke(new Action<CThostFtdcTradingAccountField>(freshAccount), pTradingAccount);
                }
            }
            else
                onNewState(EnumProgessState.OnQryAccount, pRspInfo.ErrorMsg);
            //this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.OnQryAccount, pRspInfo.ErrorMsg);
            if (bIsLast)
            {
                onNewState(EnumProgessState.OnQryAccount, "查资金完成");
                //this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.OnQryPosition, "查资金完成");
                this.apiIsBusy = false;	//查询完成
            }

        }
        #endregion

        #region 响应:下单/撤单
        //CTP:下单有误
        void tradeApi_OnErrRtnOrderInsert(ThostFtdcInputOrderField pInputOrder,ThostFtdcRspInfoField pRspInfo)
        {
            //IsErrorRspInfo(pRspInfo);
            if (IsErrorRspInfo(pRspInfo))
                onNewState(EnumProgessState.OnErrOrderInsert, pRspInfo.ErrorMsg);
            //this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.OnErrOrderInsert, pRspInfo.ErrorMsg);
        }

        //Exchange:下单有误:使用CTP即可接收此回报
        void tradeApi_OnRspOrderInsert(ThostFtdcInputOrderField pInputOrder,ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            onNewState(EnumProgessState.OrderInsert, pRspInfo.ErrorMsg);
            //IsErrorRspInfo(pRspInfo);
            //	this.BeginInvoke(new Action<EnumProgessState, string>(progress),EnumProgessState, pRspInfo.ErrorMsg);
        }

        // 委托成功
        void tradeApi_OnRtnOrder(ThostFtdcOrderField pOrder)
        {
            debug("委托成功回报");
            string key = pOrder.FrontID + "," + pOrder.SessionID + "," + pOrder.OrderRef;
            debug(key+"|"+pOrder.OrderLocalID);
            //根据返回的Order来修正本地LocalID用于正确标识Order
            MaxOrderRef = Convert.ToInt32(pOrder.OrderLocalID);
            //触发委托回报事件
            if (GotOrderEvent != null)
            {
                Order o = getLocalOrderViaKey(key);
                if (o!=null)
                    GotOrderEvent(o);
            }
            //更新服务端信息
            onNewObject(new ObjectAndKey(pOrder, key));
            onNewState(EnumProgessState.OnRtnOrder, pOrder.StatusMsg);
        }

        // 报单成交响应
        void tradeApi_OnRtnTrade(ThostFtdcTradeField pTrade)
        {
            debug("成交回报:" + "orderlocalid:" + pTrade.OrderLocalID + "order ref:" + pTrade.OrderRef + "ordersysid:" + pTrade.OrderSysID);

           
            Order o = getLocalOrderViaRef(pTrade.OrderLocalID);
            if (o != null)
            {
                //反向生成fill正确传达给相应的操盘终端
                decimal xprice = (decimal)pTrade.Price;//成交价格
                int xsize = pTrade.Volume;//成交数量
                string xtime = pTrade.TradeDate + pTrade.TradeTime;
                DateTime dtime = DateTime.ParseExact(xtime, "yyyyMMddHH:mm:ss", new System.Globalization.CultureInfo("zh-CN", true));
                
                xsize = (pTrade.Direction == EnumDirectionType.Buy ? 1 : -1) * xsize;//local fill数量是带方向的
                Trade f = new TradeImpl(o.symbol, xprice, xsize, dtime);
                f.Account = o.Account;
                f.id = o.id;
                f.side = (pTrade.Direction == EnumDirectionType.Buy);
                

                if (GotFillEvent != null)
                    GotFillEvent(f);
            }
             //更新服务端信息
            onNewState(EnumProgessState.OnRtnOrder, "成交");
            onNewObject(new ObjectAndKey(pTrade, pTrade.OrderSysID));

            //this.BeginInvoke(new Action<ObjectAndKey>(showStructInListView), new ObjectAndKey(pTrade, pTrade.OrderSysID));
            /*
            if (this.lviCovert != null)			//反手
            {
                this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.QuickCover, "快速反手...");
                tradeApi.OrderInsert(pTrade.InstrumentID, EnumOffsetFlagType.Open, pTrade.Direction, pTrade.Price, pTrade.Volume);	//反手
                this.lviCovert = null;
            }
            else
                this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.OnRtnTrade, "成交");
             * **/
            //重新查询持仓
            //if (this.lviCovert == null) //快平/快锁/反手均已完成:查持仓
            {
                Thread.Sleep(1000);
                onNewState(EnumProgessState.QryPosition, "查持仓...");
                //this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.QryPosition, "查持仓...");
                //tradeApi.QryInvestorPosition(pTrade.InstrumentID);
                this.listQry.Insert(0, new QryOrder(EnumQryOrder.QryIntorverPosition, pTrade.InstrumentID));	//查持仓
            }

        }

        //CTP:撤单有误
        void tradeApi_OnErrRtnOrderAction(ThostFtdcOrderActionField pOrderAction,ThostFtdcRspInfoField pRspInfo)
        {
            //IsErrorRspInfo(pRspInfo);
            //if (pRspInfo.ErrorID != 0)
            //this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.OnErrOrderAction, "撤单失败:" + pRspInfo.ErrorMsg);
        }

        //Exchange:撤单成功
        void tradeApi_OnRspOrderAction(ThostFtdcInputOrderActionField pInputOrderAction,ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            debug("撤单回报");
            if (!IsErrorRspInfo(pRspInfo))
                onNewState(EnumProgessState.OnOrderAction, pInputOrderAction.OrderSysID);
            //this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.OnOrderAction, pInputOrderAction.OrderSysID);
            else
                onNewState(EnumProgessState.OnOrderAction, pRspInfo.ErrorMsg);
            //this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.OnOrderAction, pRspInfo.ErrorMsg);
        }
        #endregion
        #endregion

        #region 接口函数整理 本地向外请求操作
        //向对应接口发送一个Order 委托
        /// <summary>
        /// 下单:录入报单
        /// </summary>
        /// <param name="order">输入的报单</param>
        void OrderInsert(ThostFtdcInputOrderField order) {
            int iResult = tradeApi.ReqOrderInsert(order, ++iRequestID);
            debug("--->>> 发送用户登录请求: " + ((iResult == 0) ? "成功" : "失败"));
        
        }
        /// <summary>
        /// 开平仓:限价单
        /// </summary>
        /// <param name="InstrumentID">合约代码</param>
        /// <param name="OffsetFlag">平仓:仅上期所平今时使用CloseToday/其它情况均使用Close</param>
        /// <param name="Direction">买卖</param>
        /// <param name="Price">价格</param>
        /// <param name="Volume">手数</param>
        public string ReqOrderInsert(string InstrumentID, EnumOffsetFlagType OffsetFlag, EnumDirectionType Direction, double Price, int Volume)
        {
            ThostFtdcInputOrderField tmp = new ThostFtdcInputOrderField();
            tmp.BrokerID = _BROKER_ID;
            tmp.BusinessUnit = null;
            tmp.ContingentCondition = EnumContingentConditionType.Immediately;
            tmp.ForceCloseReason = EnumForceCloseReasonType.NotForceClose;
            tmp.InvestorID = _INVESTOR_ID;
            tmp.IsAutoSuspend = (int)EnumBoolType.No;
            tmp.MinVolume = 1;
            tmp.OrderPriceType = EnumOrderPriceTypeType.LimitPrice;
            tmp.OrderRef = (++this.MaxOrderRef).ToString();
            tmp.TimeCondition = EnumTimeConditionType.GFD;	//当日有效
            tmp.UserForceClose = (int)EnumBoolType.No;
            tmp.UserID = _INVESTOR_ID;
            tmp.VolumeCondition = EnumVolumeConditionType.AV;
            tmp.CombHedgeFlag_0 = EnumHedgeFlagType.Speculation;

            tmp.InstrumentID = InstrumentID;
            tmp.CombOffsetFlag_0 = OffsetFlag;
            tmp.Direction = Direction;
            tmp.LimitPrice = Price;
            tmp.VolumeTotalOriginal = Volume;
            int iResult = tradeApi.ReqOrderInsert(tmp, ++iRequestID);
            string key = FRONT_ID + "," + SESSION_ID + "," + this.MaxOrderRef;
            debug("--->>> 发送委托: " + ((iResult == 0) ? "成功" : "失败"));
            return key;
        }
        /// <summary>
        /// 开平仓:市价单
        /// </summary>
        /// <param name="InstrumentID"></param>
        /// <param name="OffsetFlag">平仓:仅上期所平今时使用CloseToday/其它情况均使用Close</param>
        /// <param name="Direction"></param>
        /// <param name="Volume"></param>
        public string ReqOrderInsert(string InstrumentID, EnumOffsetFlagType OffsetFlag, EnumDirectionType Direction, int Volume)
        {
            ThostFtdcInputOrderField tmp = new ThostFtdcInputOrderField();
            tmp.BrokerID = _BROKER_ID;
            tmp.BusinessUnit = null;
            tmp.ContingentCondition = EnumContingentConditionType.Immediately;
            tmp.ForceCloseReason = EnumForceCloseReasonType.NotForceClose;
            tmp.InvestorID = _INVESTOR_ID;
            tmp.IsAutoSuspend = (int)EnumBoolType.No;
            tmp.MinVolume = 1;
            tmp.OrderPriceType = EnumOrderPriceTypeType.AnyPrice;
            tmp.OrderRef = (++this.MaxOrderRef).ToString();
            tmp.TimeCondition = EnumTimeConditionType.IOC;	//立即完成,否则撤单
            tmp.UserForceClose = (int)EnumBoolType.No;
            tmp.UserID = _INVESTOR_ID;
            tmp.VolumeCondition = EnumVolumeConditionType.AV;
            tmp.CombHedgeFlag_0 = EnumHedgeFlagType.Speculation;

            tmp.InstrumentID = InstrumentID;
            tmp.CombOffsetFlag_0 = OffsetFlag;
            tmp.Direction = Direction;
            tmp.LimitPrice = 0;
            tmp.VolumeTotalOriginal = Volume;
            int iResult = tradeApi.ReqOrderInsert(tmp, ++iRequestID);
            string key = FRONT_ID + "," + SESSION_ID + "," + this.MaxOrderRef;
            debug("--->>> 发送委托: " + ((iResult == 0) ? "成功" : "失败"));
            return key;
        }
        /// <summary>
        /// 开平仓:触发单
        /// </summary>
        /// <param name="InstrumentID"></param>
        /// <param name="ConditionType">触发单类型</param>
        /// <param name="ConditionPrice">触发价格</param>
        /// <param name="OffsetFlag">平仓:仅上期所平今时使用CloseToday/其它情况均使用Close</param>
        /// <param name="Direction"></param>
        /// <param name="PriceType">下单类型</param>
        /// <param name="Price">下单价格:仅当下单类型为LimitPrice时有效</param>
        /// <param name="Volume"></param>
        public string ReqOrderInsert(string InstrumentID, EnumContingentConditionType ConditionType
            , double ConditionPrice, EnumOffsetFlagType OffsetFlag, EnumDirectionType Direction, EnumOrderPriceTypeType PriceType, double Price, int Volume)
        {
            ThostFtdcInputOrderField tmp = new ThostFtdcInputOrderField();
            tmp.BrokerID = _BROKER_ID;
            tmp.BusinessUnit = null;
            tmp.ForceCloseReason = EnumForceCloseReasonType.NotForceClose;
            tmp.InvestorID = _INVESTOR_ID;
            tmp.IsAutoSuspend = (int)EnumBoolType.No;
            tmp.MinVolume = 1;
            tmp.OrderRef = (++this.MaxOrderRef).ToString();
            tmp.TimeCondition = EnumTimeConditionType.GFD;
            tmp.UserForceClose = (int)EnumBoolType.No;
            tmp.UserID = _INVESTOR_ID;
            tmp.VolumeCondition = EnumVolumeConditionType.AV;
            tmp.CombHedgeFlag_0 = EnumHedgeFlagType.Speculation;

            tmp.InstrumentID = InstrumentID;
            tmp.CombOffsetFlag_0 = OffsetFlag;
            tmp.Direction = Direction;
            tmp.ContingentCondition = ConditionType;	//触发类型
            tmp.StopPrice = Price;						//触发价格
            tmp.OrderPriceType = PriceType;				//下单类型
            tmp.LimitPrice = Price;						//下单价格:Price = LimitPrice 时有效
            tmp.VolumeTotalOriginal = Volume;
            int iResult = tradeApi.ReqOrderInsert(tmp, ++iRequestID);
            string key = FRONT_ID + "," + SESSION_ID + "," + this.MaxOrderRef;
            debug("--->>> 发送委托: " + ((iResult == 0) ? "成功" : "失败"));
            return key;
        }
        //委托操作 删除 修改等
        void ReqOrderAction(ThostFtdcOrderField pOrder)
        {
            debug("删除委托");
            //if (ORDER_ACTION_SENT)
            //return;

            ThostFtdcInputOrderActionField req = new ThostFtdcInputOrderActionField();
            ///经纪公司代码
            req.BrokerID = pOrder.BrokerID;
            ///投资者代码
            req.InvestorID = pOrder.InvestorID;
            ///报单操作引用
            //	TThostFtdcOrderActionRefType	OrderActionRef;
            ///报单引用
            req.OrderRef = pOrder.OrderRef;
            ///请求编号
            //	TThostFtdcRequestIDType	RequestID;
            ///前置编号
            req.FrontID = pOrder.FrontID;
            ///会话编号
            req.SessionID = pOrder.SessionID;

            ///交易所代码
            //	TThostFtdcExchangeIDType	ExchangeID;
            ///报单编号
            //	TThostFtdcOrderSysIDType	OrderSysID;
            ///操作标志
            //req.ActionFlag = CTP.EnumActionFlagType.Delete;
            ///价格
            //	TThostFtdcPriceType	LimitPrice;
            ///数量变化
            //	TThostFtdcVolumeType	VolumeChange;
            ///用户代码
            //	TThostFtdcUserIDType	UserID;
            ///合约代码
            req.InstrumentID = pOrder.InstrumentID;

            int iResult = tradeApi.ReqOrderAction(req, ++iRequestID);
            debug("--->>> 报单操作请求(" + iRequestID.ToString() + ")+: " + ((iResult == 0) ? "成功" : "失败"));

            //ORDER_ACTION_SENT = true;
        }
        //请求注册
        void ReqUserLogin()
        {
            ThostFtdcReqUserLoginField req = new ThostFtdcReqUserLoginField();
            req.BrokerID = _BROKER_ID;
            req.UserID = _INVESTOR_ID;
            req.Password = _PASSWORD;
            int iResult = tradeApi.ReqUserLogin(req, ++iRequestID);
            debug("--->>> 发送用户登录请求: " + ((iResult == 0) ? "成功" : "失败"));
        }
        //请求结算结果确认
        void ReqSettlementInfoConfirm()
        {
            ThostFtdcSettlementInfoConfirmField req = new ThostFtdcSettlementInfoConfirmField();
            req.BrokerID = _BROKER_ID;
            req.InvestorID = _INVESTOR_ID;
            
            int iResult = tradeApi.ReqSettlementInfoConfirm(req, ++iRequestID);
            debug("--->>> 请求结算确认: " + ((iResult == 0) ? "成功" : "失败"));
        }
        //请求合约信息
        void ReqQryInstrument(string instrument = null)
        {
            ThostFtdcQryInstrumentField req = new ThostFtdcQryInstrumentField();
            req.InstrumentID = instrument;
            int iResult = tradeApi.ReqQryInstrument(req, ++iRequestID);
            debug("--->>> 请求查询合约: " + ((iResult == 0) ? "成功" : "失败"));
        }
        //请求结算信息
        void ReqQrySettlementInfo()
        { 
            ThostFtdcQrySettlementInfoField req = new ThostFtdcQrySettlementInfoField();
            req.BrokerID = _BROKER_ID;
            req.InvestorID = _INVESTOR_ID;
            int iResult = tradeApi.ReqQrySettlementInfo(req, ++iRequestID);
            debug("--->>> 请求查询结算信息: " + ((iResult == 0) ? "成功" : "失败"));
        }
        //请求查询结算确认信息
        void ReqQrySettlementInfoConfirm()
        {
            ThostFtdcQrySettlementInfoConfirmField req = new ThostFtdcQrySettlementInfoConfirmField();
            req.BrokerID = _BROKER_ID;
            req.InvestorID = _INVESTOR_ID;
            int iResult = tradeApi.ReqQrySettlementInfoConfirm(req, ++iRequestID);
            debug("--->>> 请求查询结算确认: " + ((iResult == 0) ? "成功" : "失败"));
        }
        //请求账户信息
        void ReqQryTradingAccount()
        {
            Thread.Sleep(1000);
            ThostFtdcQryTradingAccountField req = new ThostFtdcQryTradingAccountField();
            req.BrokerID = _BROKER_ID;
            req.InvestorID = _INVESTOR_ID;
            int iResult = tradeApi.ReqQryTradingAccount(req, ++iRequestID);
            debug("--->>> 请求查询资金账户: " + ((iResult == 0) ? "成功" : "失败"));
        }
        //请求仓位信息
        void ReqQryInvestorPosition(string instrument = null)
        {
            Thread.Sleep(1000);
            ThostFtdcQryInvestorPositionField req = new ThostFtdcQryInvestorPositionField();
            req.BrokerID = _BROKER_ID;
            req.InvestorID = _INVESTOR_ID;
            req.InstrumentID = instrument;
            int iResult = tradeApi.ReqQryInvestorPosition(req, ++iRequestID);
            debug("--->>> 请求查询投资者持仓: " + ((iResult == 0) ? "成功" : "失败"));
        }
        //请求合约手续费
        void ReqQryInstrumentCommissionRate(string instrument =null)
        {
            ThostFtdcQryInstrumentCommissionRateField req = new ThostFtdcQryInstrumentCommissionRateField();
            req.BrokerID = _BROKER_ID;
            req.InstrumentID = _INVESTOR_ID;
            req.InstrumentID = instrument;
            int iResult = tradeApi.ReqQryInstrumentCommissionRate(req, ++iRequestID);
            //debug("--->>> 请求查询合约手续费: " + ((iResult == 0) ? "成功" : "失败"));
        }
        //请求保证金
        void ReqQryInstrumentMarginRate(string instrument=null)
        {
            ThostFtdcQryInstrumentMarginRateField req = new ThostFtdcQryInstrumentMarginRateField();
            req.BrokerID = _BROKER_ID;
            req.InvestorID = _INVESTOR_ID;
            req.InstrumentID = instrument;
            int iResult = tradeApi.ReqQryInstrumentMarginRate(req, ++iRequestID);
            debug("--->>> 请求查询合约保证金: " + ((iResult == 0) ? "成功" : "失败"));
        }
        //请求委托信息
        void ReqQryOrder()
        {
            ThostFtdcQryOrderField req = new ThostFtdcQryOrderField();
            req.BrokerID = _BROKER_ID;
            req.InvestorID = _INVESTOR_ID;
            int iResult = tradeApi.ReqQryOrder(req, ++iRequestID);
            debug("--->>> 请求查询委托: " + ((iResult == 0) ? "成功" : "失败"));
        }
        //请求成交信息
        void ReqQryTrade(string instrument=null)
        {
            ThostFtdcQryTradeField req = new ThostFtdcQryTradeField();
            req.BrokerID = _BROKER_ID;
            req.InvestorID = _INVESTOR_ID;
            req.InstrumentID = instrument;
            int iResult = tradeApi.ReqQryTrade(req,++iRequestID);
            debug("--->>> 请求查询成交: " + ((iResult == 0) ? "成功" : "失败"));
        }
        //确认结算信息
        void req()
        { 
            ThostFtdcSettlementInfoConfirmField req = new ThostFtdcSettlementInfoConfirmField();
            req.BrokerID = _BROKER_ID;
            req.InvestorID = _INVESTOR_ID;

            int iResult = tradeApi.ReqSettlementInfoConfirm(req, ++iRequestID);
            debug("--->>> 请求确认结算: " + ((iResult == 0) ? "成功" : "失败"));
        }
        #endregion

        #region 实现IBroker接口 用于为系统提供下单 取消 获得成交等功能
        //当前CTP不支持市价单,我们必须变通的生成市价单,然后发送,条件单也需要自己再服务器内部进行解决,目前不支持条件单
        Dictionary<string, Order> _ctpKeyOrderMap = new Dictionary<string, TradeLink.API.Order>();//CTP报单Key与本地Order的映射
        Dictionary<string, Order> _localIDOrderMap = new Dictionary<string, TradeLink.API.Order>();//CTP报单Key与本地Order的映射
        Dictionary<long, string> _oIDKeyMap = new Dictionary<long, string>();//本地Order ID与 CTP报单Key映射

        //通过order ref来获得本地order
        Order getLocalOrderViaRef(string ordRef)
        {
            Order o = null;
            if (_localIDOrderMap.TryGetValue(ordRef, out o))
                return o;
            return o;
        }
        //order Id 到 key的映射
        string oid2CTPkey(long oid)
        {
            string key = string.Empty;
            if (_oIDKeyMap.TryGetValue(oid, out key))
                return key;
            return key;
        }
        //通过key 来得到本地order
        Order getLocalOrderViaKey(string key)
        {
            Order o = null;
            if (_ctpKeyOrderMap.TryGetValue(key, out o))
                return o;
            return o;
        }
        //记录本地报单与CTP报单key的对应关系
        void BookOrderAndKey(Order o, string key)
        {
            if (!_ctpKeyOrderMap.ContainsKey(key))
                _ctpKeyOrderMap.Add(key, o);
            else
                _ctpKeyOrderMap[key] = o;
            string[] p = key.Split(',');
            if (!_localIDOrderMap.ContainsKey(p[2]))
                _localIDOrderMap.Add(p[2], o);
            else
                _localIDOrderMap[p[2]] = o;

            if (!_oIDKeyMap.ContainsKey(o.id))
                _oIDKeyMap.Add(o.id, key);
            else
                _oIDKeyMap[o.id] = key;
        }

        public void SendOrder(Order o)
        {
            debug("CTPTrader Got Order From QSTrading Side:" + o.ToString());
            debug("side:" + o.side.ToString());
            EnumDirectionType direction = (o.side == true) ? EnumDirectionType.Buy : EnumDirectionType.Sell;
            debug("CTP order direction:" + direction.ToString());
            EnumOffsetFlagType offsetflag = EnumOffsetFlagType.Open;
            //如果是市价单,则我们买在涨停 卖在跌停 那么在这样的价格情况下就均可以成交
            string key = string.Empty;
            if (o.isMarket)
                key = this.ReqOrderInsert(o.symbol, offsetflag, direction, o.UnsignedSize);
            else if (o.isLimit)
                key = this.ReqOrderInsert(o.symbol, offsetflag, direction, (double)o.price, o.UnsignedSize);
            else if (o.isStop)
            {
                EnumContingentConditionType ctype = o.side == true ? EnumContingentConditionType.LastPriceGreaterThanStopPrice : EnumContingentConditionType.LastPriceLesserThanStopPrice;
                this.ReqOrderInsert(o.symbol, ctype, (double)o.stopp, offsetflag, direction, EnumOrderPriceTypeType.BestPrice, (double)o.stopp, o.UnsignedSize);
            }
            debug("Order inserted: id" + key);
            BookOrderAndKey(o, key);
        }

        public void CancelOrder(long oid)
        { 
        
        }

        /// <summary>
        /// 当CTP接口有成交回报时,我们通知客户端
        /// </summary>
        public event FillDelegate GotFillEvent;
        /// <summary>
        /// 当CTP接口有委托回报时,我们通知客户端
        /// </summary>
        public event OrderDelegate GotOrderEvent;
        /// <summary>
        /// 当CTP接口有取消交易回报时,我们通知客户端
        /// </summary>
        public event LongDelegate GotCancelEvent;

        #endregion

        #region 其他本地操作
        void QueryAccount()
        {
            onNewState(EnumProgessState.QryAccount, "查询账户...");
            this.listQry.Insert(0, new QryOrder(EnumQryOrder.QryTradingAccount, null));
        }
        #endregion


    }
}
