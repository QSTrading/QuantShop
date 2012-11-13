using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.Data;
using TradingLib.GUI;
using TradeLink.API;
using TradingLib.Core;
using TradeLink.Common;
using TradeLink.AppKit;
using Easychart.Finance.DataProvider;
using WeifenLuo.WinFormsUI.Docking;
using TradingLib.API;
using System.Threading;

namespace QSTrading
{
    public partial class mainForm : AppTracker
    {
        public const string PROGRAM = "QSClient";

        public event DebugDelegate SendLoadingInfo;
        private bool _bSaveLayout = true;//是否从config加载上次程序的窗体
        private DeserializeDockContent _deserializeDockContent;
        private event TickDelegate spillTick;
        //交易数据记录核心
        private TradingTrackerCentre _tradingtrackerCentre = null;
        //数据通讯核心
        private CoreCentre _coreCentre = null;
        //仓位检查策略核心
        private PositionCheckCentre _poscheckCentre = null;
        //系统默认证券列表
        Basket _defaultBasket;
        //用于维护Symbol列表中的bar数据 根据tick生成新的bar数据 同时读取本地bar数据或与服务器同步
        //private BarListTracker _blt = new BarListTracker();
        //用于形成内存数据管理,为ChartControl提供实时数据,图表控件或窗口需要有MemoryDataManager才可以有效工作
        //private MemoryDataManager _mdm = null;
        //private BLTDataManager _bltdm = null;

        //窗体
        private fmDebug _fmDebug = null;
        private fmOrdEntry _fmOrdEntry = null;
        private fmQuoteList _fmQuoteList = null;

        public mainForm(DebugDelegate info)
        {
            SendLoadingInfo = info;
            TrackEnabled = Util.TrackUsage();
            Program = PROGRAM;
            InitializeComponent();
            _deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
            //Thread t = new Thread(new ThreadStart(Init));
            Init();
        }
        public mainForm()
        {
           
            TrackEnabled = Util.TrackUsage();
            Program = PROGRAM; ;
            InitializeComponent();
            _deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
            //Thread t = new Thread(new ThreadStart(Init));
            Init();
        }

        #region 程序加载
        public void Init()
        {
           
                //从服务器更新软件
                UpdateHelper uh = new UpdateHelper(Properties.Settings.Default.ServerIpAddresses);
                uh.SendDebugEvent += new DebugDelegate(loadingInfo);
                uh.DownloadFile();
                //日志输出窗体 
                loadingInfo("准备交易记录组件");
                _fmDebug = new fmDebug();
               
                    //准备数据记录中心
                    initTradingTrackerCentre();
                    //初始化corecentre核心
                    loadingInfo("加载交易通讯组件");
                    initCoreCentre();
                    //初始化仓位策略中心
                    loadingInfo("加载仓位管理策略组件");
                    initPositionCheckCentre();

                    //初始化窗口
                    loadingInfo("初始化窗口");
                    initForms();
                    //_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
                
        
        }
        private void initTradingTrackerCentre()
        {
            _tradingtrackerCentre = new TradingTrackerCentre();
            _tradingtrackerCentre.SendDebugEvet += new DebugDelegate(debug);

        }

        private void initCoreCentre()
        {
            //加载默认证券列表
            _defaultBasket = BasketTracker.getBasket("Default");
            _coreCentre = new CoreCentre("QSTrading");
            _coreCentre.SendDebugEvent += new DebugDelegate(debug);
            _coreCentre.EExecutionDisConnect += new VoidDelegate(_coreCentre_EExecutionDisConnect);
            _coreCentre.EExecutionConnect += new VoidDelegate(_coreCentre_EExecutionConnect);
            _coreCentre.EDataFeedConnect += new VoidDelegate(_coreCentre_EDataFeedConnect);
            _coreCentre.EDataFeedDisConnect += new VoidDelegate(_coreCentre_EDataFeedDisConnect);
            _coreCentre.EventAccountAvabile += new StringParamDelegate(_coreCentre_EventAccountAvabile);
            _coreCentre.EventAccountDisable += new VoidDelegate(_coreCentre_EventAccountDisable);
            _coreCentre.Init(_tradingtrackerCentre);
            _coreCentre.GotTick += new TickDelegate(this.GotTick);
            
            
        }

        //仓位管理策略
        private void initPositionCheckCentre()
        {
           
            _poscheckCentre = new PositionCheckCentre(_coreCentre, _tradingtrackerCentre);
            _poscheckCentre.SendDebugEvent += new DebugDelegate(debug);
            _coreCentre.GotTick += new TickDelegate(_poscheckCentre.GotTick);
            


        }

        private void initForms()
        {
            //简易下单面板窗体
            _fmOrdEntry = new fmOrdEntry(_coreCentre.DefaultBasket);
            //绑定positioncheck 用于OrdeEntry里面动态生成菜单
            _fmOrdEntry.PositionCheckCentre = _poscheckCentre;
            //绑定tradingtrackercentre用于窗体动态显示当前的position order等信息
            _fmOrdEntry.TradingTrackerCentre = _tradingtrackerCentre;
            _fmOrdEntry.SendDebugEvent += new DebugDelegate(debug);
            _fmOrdEntry.EOpenChart += new SecurityDelegate(openChart);
            _fmOrdEntry.SendOrderEvent += new OrderDelegate(_coreCentre.SendOrder);
            _fmOrdEntry.SendOpenTimeSalesEvent += new SecurityDelegate(openTimeSales);
            _fmOrdEntry.SymbolSelectedEvent += new SecurityDelegate(symbolSelectedChanged);

            //将orderEntry注册到coreCentre观察者列表
            _coreCentre.Subscribe(_fmOrdEntry);
            //报价列表窗体
            _fmQuoteList = new fmQuoteList();
            _fmQuoteList.SendDebugEvent +=new DebugDelegate(debug);
            _coreCentre.GotTick += new TickDelegate(_fmQuoteList.GotTick);
        }
        #endregion




        #region coreCentre服务器连接,账户变动等信息
        void _coreCentre_EDataFeedDisConnect()
        {
            this.toolStripStatusLabeQuote.BackColor = Color.Red;
        }

        void _coreCentre_EDataFeedConnect()
        {
            this.toolStripStatusLabeQuote.BackColor = Color.Green;
        }

        void _coreCentre_EventAccountDisable()
        {
            this.toolStripStatusLabelAcc.BackColor = Color.Red;
        }

        void _coreCentre_EventAccountAvabile(string param)
        {
            this.toolStripStatusLabelAcc.BackColor = Color.Green;
            _tradingtrackerCentre.setPositoinAccount(param);
            Text = "QStrading:" + param;

        }

        void _coreCentre_EExecutionConnect()
        {
            debug("窗口:交易链接启动,现在可以进行交易");
            this.reqAccountToolStripMenuItem.Enabled = true;
            this.toolStripStatusLabelExec.BackColor = Color.Green;
            this.toolStripStatusProvider.Text = _coreCentre.Provider.ToString();//.PadLeft(10, ' ');
            _connecting = false;
        }

        void _coreCentre_EExecutionDisConnect()
        {
            debug("窗口:交易链接断开,现在无法进行交易");
            this.reqAccountToolStripMenuItem.Enabled = false;
            this.toolStripStatusLabelExec.BackColor = Color.Red;
            this.toolStripStatusProvider.Text = "无连接";//.PadLeft(10,' ')
        }
        #endregion

        //即时向启动窗口发送启动信息
        private void loadingInfo(string msg)
        {
            if (SendLoadingInfo != null)
            {
                SendLoadingInfo(msg);
            }
            
        }
        private void debug(string s)
        {
            _fmDebug.GotDebug(s);
        }


        #region 窗体加载与关闭
        private void Centre_Load(object sender, System.EventArgs e)
        {
            //从文件加载窗体
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "dp.cfg");
            if (File.Exists(configFile))
                dockPanel.LoadFromXml(configFile, _deserializeDockContent);
            this.toolStripStatusProvider.Text = _coreCentre.Provider.ToString().PadLeft(10, ' ');
            Thread.Sleep(500);
            _coreCentre_EDataFeedConnect();
            _coreCentre_EExecutionConnect();

            //_coreCentre.Stop();
            //_coreCentre.Start();
            
        }

        private void Centre_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _tradingtrackerCentre.Stop();//关闭数据采集中心 用于保存历史K线数据
            _coreCentre.Stop(true);
            
            //将dockPanel内的窗体序列化到xml文件
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "dp.cfg");
            if (_bSaveLayout)
                dockPanel.SaveAsXml(configFile);
            else if (File.Exists(configFile))
                File.Delete(configFile);
            
            System.Diagnostics.Process.GetCurrentProcess().Kill();
            //窗体关闭时候 关闭CoreCentre
        }
        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(TradingLib.GUI.fmDebug).ToString())
                return _fmDebug;
            else if(persistString == typeof(TradingLib.GUI.fmOrdEntry).ToString())
                return _fmOrdEntry;
            else if(persistString == typeof(TradingLib.GUI.fmQuoteList).ToString())
                return _fmQuoteList;
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

        #region 转发symbol编辑的增加与删除symbol
        private void AddSecToDefault(Security s)
        {
            debug(s.Symbol + " is added into default basket list");
            _defaultBasket.Add(s);
            _fmOrdEntry.AddSecToDefault(s);
        }
        private void DelSecFromDefault(Security s)
        {
            debug(s.Symbol + " is deleted default basket list");
            _defaultBasket.Remove(s);
            _fmOrdEntry.DelSecFromDefault(s);
        }
        #endregion


        #region 按钮调用方法
        //出场策略编辑
        private void exitStrategyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fmPositionExitStrategy fm = new fmPositionExitStrategy();
            fm.PositionCheckCentre = _poscheckCentre;
            fm.Show();

        }
        //工具栏 K线图
        private void tbnChart_Click(object sender, EventArgs e)
        {
            fmChartForm m_ctChartForm = new fmChartForm(_tradingtrackerCentre.DataManager, _defaultBasket, _defaultBasket[0]);
            _coreCentre.GotTick +=new TickDelegate(m_ctChartForm.GotTick);
            m_ctChartForm.Show(dockPanel, DockState.Document);

        }
        //代码编辑
        private void symbolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fmSecListEdit fm = new fmSecListEdit();
            fm.DefaultBasketSymAdd += new SecurityDelegate(AddSecToDefault);
            fm.DefaultBasketSymDel += new SecurityDelegate(DelSecFromDefault);
            fm.Show();
        }
        //菜单 k线图
        private void chartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Basket b = new BasketImpl();
            string[] s = { "ACF", "ACI", "MSFT" };
            Basket ds = new BasketImpl(s);
            b.Add(ds);
            fmChartForm m_ctChartForm = new fmChartForm(_tradingtrackerCentre.DataManager,b, new SecurityImpl("MSFT"));
            m_ctChartForm.Show(dockPanel, DockState.Document);
        }
        //菜单 日志输出
        private void debugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _fmDebug.Show(dockPanel, DockState.DockBottom);
        }
        //菜单 简易下单
        private void orderEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _fmOrdEntry.Show(dockPanel, DockState.DockBottom);

        }
        //菜单 报价列表
        private void quoteListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _fmQuoteList.Show(dockPanel, DockState.Document);
        }

        //菜单 历史数据
        private void histDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fmHistData fm = new fmHistData();
            fm.Show();
        }

        //状态栏provider单击连接
        private bool _connecting=false;
        private void toolStripStatusProvider_Click(object sender, EventArgs e)
        {
            //if (!_coreCentre.IsBrokerConnected && !_connecting && this.toolStripStatusProvider.Text=="无连接")
            if (this.toolStripStatusProvider.Text == "无连接")
            {
                connectServer();
            }
            if (_coreCentre.IsBrokerConnected && !_connecting)
            {
                fmConfirm fm = new fmConfirm("确认断开数据连接？");
                fm.SendConfimEvent += new VoidDelegate(stopCoreCentre_SendConfimEvent);
                fm.ShowDialog();
            }
        }

        private void stopCoreCentre_SendConfimEvent()
        {
            _coreCentre.Stop();
            //Thread.Sleep(1000);
            //_connecting = false;
        }
        #endregion


        #region 通用操作
        void GotTick(Tick k)
        {
            if (spillTick != null)
                spillTick(k);
        }
        private void connectServer()
        {
            _coreCentre.Connect();
            this.toolStripStatusProvider.Text = "连接中";//.ToString().PadLeft(10, ' ');
            _connecting = true;
        }
        //关于新增程序对coreCentre的调用,使用subscribe适合于长驻内存的服务,如果动态的生成的窗体或者服务通过这个方式绑定方法，会产生程序崩溃
        private void symbolSelectedChanged(Security sec)
        { 
            debug("Selected Security Changed we request hist data");
            //_coreCentre.RequestInterDayBar(sec);
            
        }

        //打开某个security的盘口数据
        private void openTimeSales(Security sec)
        {
            fmTimeSales fm = new fmTimeSales(sec);
            fm.Security = sec;
            spillTick += new TickDelegate(fm.GotTick);
            //_coreCentre.GotTick +=new TickDelegate(fm.GotTick);
            //_coreCentre.Subscribe(fm);
            //debug(_coreCentre.GotTick)
            //fm.FormClosing += new FormClosingEventHandler(timesales_FormClosing);
            fm.Show();
        }
        //当窗口关闭时 如果有方法绑定到CoreCentre我们需要将它干净的去除
        void timesales_FormClosing(object sender, FormClosingEventArgs e)
        {
            lock (sender)
            {
                //fmTimeSales fm = sender as fmTimeSales;
                //fmTimeSales fm = sender as fmTimeSales;
                //debug("xxxxxxxxxxxx sender:" + sender.ToString());
                //_coreCentre.GotTick -= new TickDelegate(fm.GotTick);
                //_coreCentre.unSubscribe(fm);
            }
        }
        

        //打开某个Security的ｋ线图
        private void openChart(Security sec)
        {
            //检查数据源是否需要更新，如果需要更新则我们更新数据
            _coreCentre.RequestInterDayBar(sec);
            Basket b = new BasketImpl(sec as SecurityImpl);
            fmChartForm fm = new fmChartForm(_tradingtrackerCentre.DataManager,b,sec);

            fm.SendDebugEvent += new DebugDelegate(debug);
            fm.SendHistDataRequest +=new StringParamDelegate(_coreCentre.RequestHistBar);
            fm.FormClosing += new FormClosingEventHandler(chart_FormClosing);
            _coreCentre.GotTick +=new TickDelegate(fm.GotTick);
            fm.Show(dockPanel, DockState.Document);
                //_coreCentre.Subscribe(fm);
        }
        //K线图关闭
        void chart_FormClosing(object sender, FormClosingEventArgs e)
        {
            lock (sender)
            {
                fmChartForm fm = sender as fmChartForm;
                //fmTimeSales fm = sender as fmTimeSales;
                debug("xxxxxxxxxxxx sender:" + sender.ToString());
                _coreCentre.GotTick -= new TickDelegate(fm.GotTick);
                
            }
        }


        #endregion

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            connectServer();
        }

        
        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _coreCentre.Stop();
        }

        private void reqAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fmTLServerLogin fm = new fmTLServerLogin();
            fm.Text = "请求交易帐号";
            fm.SendTLServerLogin += new AccountRequestDel(fm_SendTLServerLogin);
            fm.ShowDialog();
            
        }

        //交易终端请求交易账户
        //在请求交易账户时,我们需要将所有的数据归零,切换至一个新的交易账户
        string fm_SendTLServerLogin(string ac, int pass)
        {
            //debug("QSTrading request trading account");
            _coreCentre.RequestAccount(ac, pass);
            return ac;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //Chart fm = new Chart(_coreCentre.getBarlist("IF1211"));
           // fm.Show();
            /*
            demo fm = new demo();
            fm.SendDebugEvent +=new DebugDelegate(debug);
            fm.Show();
             * */
            fmMutilChart fm = new fmMutilChart();
            fm.Show(dockPanel, DockState.Document);
            /*
            string ac = _coreCentre.Account !=null ?_coreCentre.Account.ID.ToString():"null";
            string s = (_coreCentre.IsBrokerConnected.ToString() + "|" + _coreCentre.IsFeedConnected.ToString())+"|"+ac;
            fmMessage fm = new fmMessage(s);
            fm.ShowDialog();
             * */
            //_fmOrdEntry.se
            //_coreCentre.RequestHistBar()
        }

        private void toolStripButtonClearLog_Click(object sender, EventArgs e)
        {
            _fmDebug.Clear();
        }





        

       



 



    }
}
       
