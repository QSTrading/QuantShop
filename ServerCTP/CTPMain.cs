using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradeLink.API;
using TradeLink.Common;
using TradeLink.AppKit;
//using IESignal;

using WeifenLuo.WinFormsUI.Docking;
using TradingLib;
using TradingLib.Data;
using TradingLib.GUI;
using TradingLib.GUI.Server;
using TradingLib.Core;
using TradingLib.Web.HTTP;
using System.Diagnostics;//记得加入此引用
using System.Threading;
using TradingLib.Broker.CTP;

namespace ServerCTP
{
    public partial class CTPMain : AppTracker
    {
     
        //Dock窗体的设置
        bool _bSaveLayout = true;
        private DeserializeDockContent _deserializeDockContent;

        public const string PROGRAM = "CTPServer";

        //是否记录日志 用于查询信息
        private bool _logenable = true;
        public bool LogEnable { get{return _logenable;} set{_logenable =value;} }
        //是否显示调试信息
        bool _verb = true;
        public bool VerboseDebugging { get { return _verb; } set { _verb = value; } }

        Log _log = new Log(PROGRAM);//日志组件
        DebugWindow _dw = new DebugWindow();//日志显示窗口组件需要最先实例化,后面初始化的组件用于日志输出
        

        //private DeserializeDockContent m_deserializeDockContent;
        //服务组件
        private TradingLib.Web.HTTP.HttpFileServer _httpsrv;
        private TradingLib.Core.CTPServer _srv;
        private ClearCentre _clearCentre;//清算中心
        private RiskCentre _riskCentre;//风控中心


        //窗口
        private fmDebug m_DebugForm = new fmDebug();
        private fmSrvClearCentre m_fmSrvClearCentre;

        



        public CTPMain()
        {

            TrackEnabled = Util.TrackUsage();
            Program = PROGRAM;
            InitializeComponent();
            //从配置文件加载对应的窗体
            _deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
           
            // send debug messages to log file
            debug("程序启动 " + PROGRAM + Util.TLVersion());

            
            //初始化CTPSrv
            InitCTPSrv();
            //初始化结算中心 初始化账户信息
            InitClearCentre();
            //初始化风控中心 初始化账户风控规则
            InitRiskCentre(); 
            //初始化必要的View组件
            InitForm();
            //初始化httpserver
            InitHttpSrv();
            debug("等待系统建立 Feed Execution Server Connection");
            
            FormClosing += new FormClosingEventHandler(CTP_FormClosing);
        }

        #region 服务组件及窗口初始化
        //初始化HttpServer服务组件
        private void InitHttpSrv()
        {
            //TradingLib.Web.HTTP.HttpFileServer
            debug("初始化Http File Server");
            _httpsrv = new HttpFileServer(Properties.Settings.Default.TLServerIP);
            _httpsrv.SendDebugEvent +=new DebugDelegate(debug);
            _httpsrv.Start();
        }
        //初始化CTPServer
        private void InitCTPSrv()
        {
            debug("初始化TLServer 服务组件");
            //根据不同的配置生成具体的TLServer
            TLServer tlserver;
            if (Properties.Settings.Default.TLServerIP == string.Empty)
            {
                tlserver = new TLServer_WM();
            }
            else
            {
                tlserver = new TLServer_MQ(Properties.Settings.Default.TLServerIP, Properties.Settings.Default.TLServerPort, 50, 100000, debug, false);
                //debug("IPServer");
            }

            //实例化CTPServer用于与Broker进行通讯
            _srv = new TradingLib.Core.CTPServer(tlserver,true);
            //_srv.VerboseDebugging = true;
            _srv.SendDebugEvent += new DebugDelegate(debug);
            _srv.SendAccountUpdateEvent += new AccountUpdateDel(_srv_SendAccountUpdateEvent);
            _srv.ServerConnectedEvent += new VoidDelegate(tl_ServerConnected);


            // set defaults
            _srv.isPaperTradeEnabled = true;//Properties.Settings.Default.PaperTrade;
            _srv.isPaperTradeUsingBidAsk = true;////Properties.Settings.Default.PaperTradeUseBidAsk;

            _srv.AllowSendInvalidBars = Properties.Settings.Default.AllowSendOfInvalidBars;
            _srv.DefaultBarsBack = Properties.Settings.Default.DefaultBarsBack;
            _srv.VerboseDebugging = Properties.Settings.Default.VerboseDebugging;
            _srv.ReleaseBarHistoryAfteRequest = Properties.Settings.Default.ReleaseBarHistoryAfterSending;
            _srv.ReleaseDeadSymbols = Properties.Settings.Default.ReleaseDeadSymbols;
            _srv.WaitBetweenEvents = Properties.Settings.Default.WaitBetweenEvents;
            _srv.BarRequestsGetAllData = Properties.Settings.Default.BarRequestsuseAllData;
            
        
        }

        //CTP触发账户连接更新事件
        void _srv_SendAccountUpdateEvent(string acc, bool w, string socketname)
        {
            //throw new NotImplementedException();
            m_fmSrvClearCentre.updateAccountConnection(acc, w, socketname);
        }
        //初始化清算中心
        private void InitClearCentre()
        {
            debug("初始化清算中心");
            _clearCentre = new ClearCentre();
            _clearCentre.SendDebugEvet +=new DebugDelegate(debug);
            //将order,fill,cancle,tick与清算中心的回调函数绑定，清算中心会根据接收到的消息进行账户结算
            _srv.GotTick += new TickDelegate(_clearCentre.GotTick);
            _srv.GotOrder += new OrderDelegate(_clearCentre.GotOrder);
            _srv.GotFill += new FillDelegate(_clearCentre.GotFill);
            _srv.GotCancel += new LongDelegate(_clearCentre.GotCancel);
            //将清算中心传递给ctpserver
            _srv.ClearCentre = _clearCentre;
            //tl.GotOrder += new OrderDelegate(_clearCentre.GotOrder); 
        }

        //初始化分控中心
        public void InitRiskCentre()
        {
            debug("初始化风控中心");
            _riskCentre = new RiskCentre(_clearCentre.getAccounts());
            _riskCentre.SendDebugEvent += new DebugDelegate(debug);
            _srv.SendOrderRiskCheckEvent += new RiskCheckOrderDel(_riskCentre.CheckOrder);
            
        
        }
        //初始化窗体
        private void InitForm()
        {
            debug("初始化服务端窗口显示组件");
            m_fmSrvClearCentre = new fmSrvClearCentre(_clearCentre);
            m_fmSrvClearCentre.SendDebugEvent +=new DebugDelegate(debug);
            _srv.GotTick += new TickDelegate(m_fmSrvClearCentre.GotTick);
            _srv.GotOrder += new OrderDelegate(m_fmSrvClearCentre.GotOrder);
            _srv.GotFill += new FillDelegate(m_fmSrvClearCentre.GotFill);
            _srv.GotCancel += new LongDelegate(m_fmSrvClearCentre.GotCancel);
            //m_fmSrvClearCentre.gotAccount();
            m_fmAccountMoniter = new fmAccountMoniter();
            _srv._CTPTrader.AccountMoniter = m_fmAccountMoniter;//将账户监控窗口与对应的Trader进行绑定用于显示账户交易信息

        }
        #endregion



        #region 窗体加载与关闭
        private void CTP_Load(object sender, System.EventArgs e)
        {
            //从文件加载窗体
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "dp.cfg");
            if (File.Exists(configFile))
                dockPanel.LoadFromXml(configFile, _deserializeDockContent);
        }

        private void CTP_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            _httpsrv.Stop();
            _srv.Stop();
            //将dockPanel内的窗体序列化到xml文件
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "dp.cfg");
            if (_bSaveLayout)
                dockPanel.SaveAsXml(configFile);
            else if (File.Exists(configFile))
                File.Delete(configFile);
            _debug = false;
            //m_fmSrvClearCentre.Dispose();
            //m_DebugForm.Dispose();
            
            //Thread.Sleep(2000);
            

            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(TradingLib.GUI.fmDebug).ToString())
                return m_DebugForm;
            if (persistString == typeof(TradingLib.GUI.Server.fmSrvClearCentre).ToString())
                return m_fmSrvClearCentre;
            if (persistString == typeof(TradingLib.GUI.Server.fmAccountMoniter).ToString())
                return m_fmAccountMoniter;
            {
                // DummyDoc overrides GetPersistString to add extra information into persistString.
                // Any DockContent may override this value to add any needed information for deserialization.
                /*
                string[] parsedStrings = persistString.Split(new char[] { ',' });
                if (parsedStrings.Length != 3)
                    return null;
                if (parsedStrings[0] != typeof(DummyDoc).ToString())
                    return null;
                DummyDoc dummyDoc = new DummyDoc();
                if (parsedStrings[1] != string.Empty)
                    dummyDoc.FileName = parsedStrings[1];
                if (parsedStrings[2] != string.Empty)
                    dummyDoc.Text = parsedStrings[2];
                return dummyDoc;
                 * */
            }
            return null;
        }
        #endregion

        //将debug信息输出到debugForm
        bool _debug = true;
        void debug(string msg)
        {
            //_dw.GotDebug(msg);
            //_log.GotDebug(msg);
            if(_debug)
                m_DebugForm.GotDebug(msg);
        }
        void v(string msg)
        {
            if (_verb)
                debug(msg);
        }

        // attempt to connect to esignal
        private void _ok_Click(object sender, EventArgs e)
        {
            //if (_acctapp.Text == string.Empty) return;
            debug("Starting CTPServer");
            _srv.Start();
            if (_srv.isValid) { BackColor = Color.Green; }
            else
                BackColor = Color.Red;
            Invalidate(true); 
        }
        

        //登入CTP服务器
        private void cTPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //loginform = new ctCTPLoginForm();
            fmCTPLogin fm = new fmCTPLogin();
            fm.SendLoginEvent += new CTPLoginDel(loginform_SendLoginEvent);
            fm.SendLogoutEvent += new VoidDelegate(loginform_SendLogoutEvent);
            fm.ShowDialog();
        }
        

        //登入服务器的操作
        private void loginform_SendLoginEvent(string ip, string broker, string name, string pass)
        {
            serverStatus.Text = "CTP连接中....";
            //tl.IPAddress = ip;
            //tl.Broker = broker;
            //tl.User = name;
            //tl.Pass = pass;
            //启动服务
            _srv.Start();    
        }


        //注销服务器的操作
        private void loginform_SendLogoutEvent()
        {
            //关闭服务
            _srv.Stop();
            serverStatus.Text = "CTP断开连接....";
        }
        private void tl_ServerConnected()
        {
            //loginform.Visible = false;
            serverStatus.Text = "CTP已连接";
            //tl.demoRequestData();//请求模拟数据 数据请求应该有另外的部分来完成
        }

        //启动CTP连接
        /*
        TradeApi tradeApi;
        string _FRONT_ADDR = "tcp://asp-sim2-front1.financial-trading-platform.com:26205";  // 前置地址
        string _BROKER_ID = "4070";                       // 经纪公司代码
        string _INVESTOR_ID = "00295";                    // 投资者代码
        string _PASSWORD = "123456";                     // 用户密码
         * **/
        /*void tradeApi_OnRspUserLogin(ref CThostFtdcRspUserLoginField pRspUserLogin, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            debug("用户登入回报:" + pRspInfo.ErrorMsg);
        }

        void tradeApi_OnFrontConnect()
        {
            debug("前置连接回报 准备登入");

            tradeApi.UserLogin();
        }
        **/
        private void connectServer_Click(object sender, EventArgs e)
        {
           

            //_srv.Start();
        }
        //停止CTP连接
        private void disconnect_Click(object sender, EventArgs e)
        {
            _srv.Stop();
        }

        //打开账户监控中心
        private void toolStripButtonClearCentre_Click(object sender, EventArgs e)
        {
            m_fmSrvClearCentre.Show(dockPanel, DockState.Document);
        }

        //合约列表编辑


        //显示日志窗口
        private void debugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //m_OrdersForm.Show(dockPanel, DockState.DockBottomAutoHide);
            m_DebugForm.Show(dockPanel, DockState.DockBottomAutoHide);
        }

        //发送模拟Tick
        private void tsbDemoTick_Click(object sender, EventArgs e)
        {
            TradingLib.GUI.Server.fmSrvDemoTick fm = new TradingLib.GUI.Server.fmSrvDemoTick();
            fm.SendTickEvent += new TickDelegate(_srv.demoTick);
            fm.SendDebugEvent +=new DebugDelegate(debug);
            fm.Show();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            _riskCentre.InitRuleSetDll();
        }
        //擦除日志
        private void toolStripButtonClearLog_Click(object sender, EventArgs e)
        {
            m_DebugForm.Clear();
        }

        private void debugSwitchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _srv.VerboseDebugging = !_srv.VerboseDebugging;
        }

        
        //将启动与关闭放在线程中运行就不会卡死界面
        private void toolStripBtnShutDown_Click(object sender, EventArgs e)
        {
            new Thread(_srv.Stop).Start();
           
        }

        private void toolStripBtnstart_Click(object sender, EventArgs e)
        {
            new Thread(_srv.Start).Start();
            //tradeApi = new TradeApi(_INVESTOR_ID, _PASSWORD, _BROKER_ID, _FRONT_ADDR);
           // tradeApi.OnFrontConnect += new TradeApi.FrontConnect(tradeApi_OnFrontConnect);
           // tradeApi.OnRspUserLogin += new TradeApi.RspUserLogin(tradeApi_OnRspUserLogin);
           // tradeApi.Connect();
        }

        private void symbolEditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fmSecListEdit fm = new fmSecListEdit();
            fm.Show();
        }

        private void localSymbolGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fmLocalTickMap fm = new fmLocalTickMap();
            fm.Show();
        }

        private void exchSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fmSessionEdit fm = new fmSessionEdit();
            fm.Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Security s = new SecurityImpl("IF1211");
            if (s.Type == SecurityType.IDX) return;
            string sym = s.Symbol;
            if ((s.FullName == string.Empty) || (sym == string.Empty))
            {
                return;
            }
            Order o = new OrderImpl(s.Symbol, 0);
            o.ex = s.DestEx;
            o.Security = s.Type;
            o.LocalSymbol = sym;
            //简易下单面板
            Ticket t = new Ticket(o);
            //考虑用什么方式去调用对应的函数
            t.SendOrder += new OrderDelegate(_srv._CTPTrader.SendOrder);
            //spillTick += new TickDelegate(t.newTick);
            //orderStatus += new OrderStatusDel(t.orderStatus);

            System.Drawing.Point p = new System.Drawing.Point(MousePosition.X,

MousePosition.Y);
            p.Offset(-315, 20);
            t.SetDesktopLocation(p.X, p.Y);
            t.Show();
        }

        fmAccountMoniter m_fmAccountMoniter;
        private void tbnAccountMoniter_Click(object sender, EventArgs e)
        {
            m_fmAccountMoniter.Show(dockPanel,DockState.Document);
        }

       

       
    }
}
