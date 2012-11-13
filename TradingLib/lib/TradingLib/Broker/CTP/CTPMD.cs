using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TradeLink.API;
using TradeLink.Common;
using CTP;
using System.Windows.Forms;


namespace TradingLib.Broker.CTP
{
    public class CTPMD
    {   

        /// <summary>
        /// 当有日志信息输出时调用
        /// </summary>
        public event DebugDelegate SendDebugEvent;
        /// <summary>
        /// 当数据服务器登入成功后调用
        /// </summary>
        public event VoidDelegate SendCTPMDReady;
        /// <summary>
        /// 当数据服务得到一个新的tick时调用
        /// </summary>
        public event TickDelegate GotTick;
        //储存CTP lastTick数据 用于得到上一个tick,用来计算size
        Dictionary<string, ThostFtdcDepthMarketDataField> _symTickSnapMap = new Dictionary<string, ThostFtdcDepthMarketDataField>();
        //CTP dll的封装,CTPAdapter用于DataFeed
        private CTPMDAdapter mdAdapter = null;

        #region CPTAdapter回调函数
        private bool CTPDataFeedLive = false;
        private bool CTPMDConnected = false;
        private bool CTPMDListened = false;
        private Thread CTPMDThread = null;

        
        string _FRONT_ADDR = "tcp://asp-sim2-md1.financial-trading-platform.com:26213";  // 前置地址
        string _BrokerID = "4070";                       // 经纪公司代码
        string _UserID = "00295";                       // 投资者代码
        string _Password = "123456";                     // 用户密码
        



        

        /*
        string _FRONT_ADDR = "tcp://180.168.212.52:41213";  // 前置地址
        string _BrokerID = "8888";                       // 经纪公司代码
        string _UserID = "8562000802";                       // 投资者代码
        string _Password = "194514";                     // 用户密码
        **/
        
        
        
        
        


        public string IPAddress { get { return _FRONT_ADDR; } set { _FRONT_ADDR = value; } }
        public string Broker { get { return _BrokerID; } set { _BrokerID = value; } }
        public string User { get { return _UserID; } set { _UserID = value; } }
        public string Pass { get { return _Password; } set { _Password = value; } }
        // 大连,上海代码为小写
        // 郑州,中金所代码为大写
        // 郑州品种年份为一位数
        string[] ppInstrumentID = { "IF1209" };	//{"ag1212", "cu1207", "ru1209","TA209", "SR301", "y1301", "IF1206"};	// 行情订阅列表
        int iRequestID = 0;


        private void debug(string s)
        {
            if (SendDebugEvent != null)
                SendDebugEvent(s);
        }

        public void InitCTPMD()
        {
            if (CTPMDConnected == false)
            {
                debug("Initing CTP MKTData Connection");
                debug("CTP MarketData Server Address:" + _FRONT_ADDR.ToString());

                try
                {
                    string path = System.IO.Directory.GetCurrentDirectory();
                    path = System.IO.Path.Combine(path, "Cache4Md\\");
                    System.IO.Directory.CreateDirectory(path);

                    mdAdapter = new CTPMDAdapter(path, false);
                    mdAdapter.OnFrontConnected += new FrontConnected(OnFrontConnected);
                    mdAdapter.OnFrontDisconnected += new FrontDisconnected(OnFrontDisconnected);

                    mdAdapter.OnRspError += new RspError(OnRspError);

                    mdAdapter.OnRspSubMarketData += new RspSubMarketData(OnRspSubMarketData);
                    mdAdapter.OnRspUnSubMarketData += new RspUnSubMarketData(OnRspUnSubMarketData);

                    mdAdapter.OnRspUserLogin += new RspUserLogin(OnRspUserLogin);
                    mdAdapter.OnRtnDepthMarketData += new RtnDepthMarketData(OnRtnDepthMarketData);

                    //初始化接口
                    mdAdapter.RegisterFront(_FRONT_ADDR);//->前置服务器连接成功后回调用户登入请求
                    mdAdapter.Init();
                    //mdAdapter.Join();
                    CTPMDConnected = true;
                  
                }
                catch (Exception e)
                {
                    //LastError = e.Message;
                    //return false;
                    MessageBox.Show(e.ToString());
                }
            }
          
            //return true;
        }

        public bool CTPMDDispose()
        {
            //DebugPrintFunction(new StackTrace());
            debug("CTPAdapter断开服务器连接");
            if (mdAdapter != null)
            {
                debug("CTPAdapter release");
                //mdAdapter.Dispose();
                //mdAdapter.
                mdAdapter.Release();
                //MessageBox.Show(mdAdapter.ToString());
                //Process.GetCurrentProcess().Kill();
                CTPMDConnected = false;
                mdAdapter = null;
            }
            else
            {
                //isConnected = false;
            }

            return true;
        }

        public bool IsCTPMDLive
        {
            // Return the state of the service.  If it is currently
            // listening/watching for ticks, return true.
            get { return CTPDataFeedLive; }
        }
        // 开始启动线程进行市场数据监听
        public bool RunCTPMD()
        {
            //DebugPrintFunction(new StackTrace(false));
            //MessageBox.Show("xxxx");
            // Called by RightEdge to initiate the data watch.
            debug("CTPMD:开始监听数据");
            if (CTPMDConnected == false)
                InitCTPMD();

            if (!CTPMDListened)
            {
                CTPMDListened = true;
                // Start a new thread for our random data.
                CTPMDThread = new Thread(new ThreadStart(ThreadFunc));
                CTPMDThread.IsBackground = true;
                CTPMDThread.Start();
                //Thread.Sleep(1000);
                //MessageBox.Show("线程工作状态：" + CTPMDThread.IsAlive.ToString());
            }
            return true;
        }
        // 停止监听线程
        //注意当有数据请求的时候,我们无法正常关闭监听线程,在停止线程前我们需要正常关闭各个业务请求
        public bool ExitCTPMD()
        {
            //DebugPrintFunction(new StackTrace(false));

            // Called by RightEdge to stop watching/listening for data.
            //log.Info("停止监听数据");
            if (CTPMDListened)
            {
                //unSubscribeMarketData(syms);
                //ReqUserLogout();

                //log.Info("停止正在进行中的监听......");
                CTPMDListened = false;
                // If running, abort the thread.

                CTPMDDispose();
                debug(CTPMDThread.ToString());
                //CTPMDThread.Abort();

                if (CTPMDThread != null && !CTPMDThread.Join(200))
                {   //log.Info("线程退出");
                    debug("结束工作线程");
                    CTPMDThread.Abort();

                }

                //Thread.Sleep(1000);
                //MessageBox.Show("线程工作状态：" + CTPMDThread.IsAlive.ToString());
                CTPMDThread = null;
                //log.Info("thread = null");
                //mdAdapter.Release();

            }
            return true;
        }

        //获得tick数据的函数，用于线程中调用
        // Thread function to generate random tick data.
        void ThreadFunc()
        {
            try
            {
                mdAdapter.Join();
            }
            catch (Exception e)
            {
                //_LastError = e.Message;
                mdAdapter.Release();
            }
        }

        bool IsErrorRspInfo(ThostFtdcRspInfoField pRspInfo)
        {
            // 如果ErrorID != 0, 说明收到了错误的响应
            bool bResult = ((pRspInfo != null) && (pRspInfo.ErrorID != 0));
            if (bResult)
                debug("--->>> ErrorID=" + pRspInfo.ErrorID.ToString() + "ErrorMsg=" + pRspInfo.ErrorMsg.ToString());
            return bResult;
        }

        //用户登入服务器
        public void ReqUserLogin()
        {
            //请求用户登入
            debug("CTPMD:请求用户登入");

            ThostFtdcReqUserLoginField req = new ThostFtdcReqUserLoginField();
            req.BrokerID = _BrokerID;
            req.UserID = _UserID;
            req.Password = _Password;
            int iResult = mdAdapter.ReqUserLogin(req, ++iRequestID);

            debug("CTPMD:--->>> 发送用户登录请求: " + ((iResult == 0) ? "成功" : "失败"));
        }

        //用户登出
        public void ReqUserLogout()
        {
            //请求用户登入
            debug("CTPMD:请求用户注销");

            //ThostFtdcReqUserLoginField req = new ThostFtdcReqUserLoginField();
            ThostFtdcUserLogoutField req = new ThostFtdcUserLogoutField();
            req.BrokerID = _BrokerID;
            req.UserID = _UserID;
            //req.Password = _Password;
            int iResult = mdAdapter.ReqUserLogout(req, ++iRequestID);

            debug("CTPMD:--->>> 发送用户登出请求: " + ((iResult == 0) ? "成功" : "失败"));
        }


        //请求市场数据
        public void SubscribeMarketData(string[] symbols)
        {
            //请求市场数据
            debug("CTPMD:订阅市场数据");
            int iResult = mdAdapter.SubscribeMarketData(symbols);
            debug("CTPMD:--->>> 发送行情订阅请求: " + ((iResult == 0) ? "成功" : "失败"));
        }

        //请求市场数据
        public void unSubscribeMarketData(string[] symbols)
        {
            //请求市场数据
            debug("CTPMD:退订市场数据");
            int iResult = mdAdapter.UnSubscribeMarketData(symbols);
            debug("CTPMD:--->>> 发送行情退订请求: " + ((iResult == 0) ? "成功" : "失败"));
        }


        void OnRspUserLogout(ThostFtdcUserLogoutField pUserLogout, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            debug("CTPMD:用户注销回报");
            //DebugPrintFunc(new StackTrace());
        }

        //当用户注册成功后的回报
        void OnRspUserLogin(ThostFtdcRspUserLoginField pRspUserLogin, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            debug("CTPMD:用户登入回报");
            if (bIsLast && !IsErrorRspInfo(pRspInfo))
            {
                ///获取当前交易日
                debug("CTPMD: 获取当前交易日 = " + mdAdapter.GetTradingDay());
                CTPDataFeedLive = true;
                debug("CTP数据服务组件启动成功");
                // 请求订阅行情
                //demoRequestData();
                //当CTP服务器连接成功后触发服务器连接成功事件
                //ServerConnectedHandler();
                if (SendCTPMDReady != null)
                    SendCTPMDReady();
            }
        }

        void OnRspUnSubMarketData(ThostFtdcSpecificInstrumentField pSpecificInstrument, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            debug("CTPMD:退订市场数据回报");
            //DebugPrintFunc(new StackTrace());
            //ReqUserLogout();
        }

        void OnRspSubMarketData(ThostFtdcSpecificInstrumentField pSpecificInstrument, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            //debug("CTPMD:订阅市场数据回报");
            //DebugPrintFunc(new StackTrace());
        }

        void OnRspError(ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            debug("CTPMD:错误回报");
            //debug(new StackTrace());
            IsErrorRspInfo(pRspInfo);
        }

        void OnHeartBeatWarning(int nTimeLapse)
        {
            debug("CTPMD:心跳回报");
            //DebugPrintFunc(new StackTrace());
            debug("CTPMD:--->>> nTimerLapse = " + nTimeLapse);
        }

        void OnFrontDisconnected(int nReason)
        {
            debug("CTPMD:前置断开回报");
            //DebugPrintFunc(new StackTrace());
            Console.WriteLine("--->>> Reason = {0}", nReason);
        }

        void OnFrontConnected()
        {
            debug("CTPMD:前置连接回报-准备启动监听线程");
            RunCTPMD();//前段连接注册成功后启动线程
            ReqUserLogin();//启动线程后登入用户
            //DebugPrintFunc(new StackTrace());
            //ReqUserLogin();
        }

        //行情数据解析
        void OnRtnDepthMarketData(ThostFtdcDepthMarketDataField pDepthMarketData)
        {
            try
            {
                //debug("市场数据到达");
                string sym = pDepthMarketData.InstrumentID;
                //形成本地的tick数据
                Tick k = new TickImpl(sym);

                k.ask = pDepthMarketData.AskPrice1 != null ? (decimal)pDepthMarketData.AskPrice1 : 0;
                k.bid = pDepthMarketData.BidPrice1 != null ? (decimal)pDepthMarketData.BidPrice1 : 0;
                k.bs = pDepthMarketData.BidVolume1;
                k.os = pDepthMarketData.AskVolume1;
                //k.AskSize = pDepthMarketData.AskVolume1;
                //k.BidSize = pDepthMarketData.BidVolume1;

                k.trade = pDepthMarketData.LastPrice != null ? (decimal)pDepthMarketData.LastPrice : 0;
                DateTime t = Convert.ToDateTime(pDepthMarketData.UpdateTime);
                k.time = Util.ToTLTime(t);
                k.date = Util.ToTLDate(t.Date);
                //tick extend field
                k.Vol = pDepthMarketData.Volume;
                k.Open = (decimal)pDepthMarketData.OpenPrice;
                k.High = (decimal)pDepthMarketData.HighestPrice;
                k.Low = (decimal)pDepthMarketData.LowestPrice;

                k.PreOpenInterest = Convert.ToInt32(pDepthMarketData.PreOpenInterest);
                k.OpenInterest = Convert.ToInt32(pDepthMarketData.OpenInterest);
                k.PreSettlement = (decimal)pDepthMarketData.PreSettlementPrice;
                //k.Settlement = (decimal)pDepthMarketData.SettlementPrice;


                ThostFtdcDepthMarketDataField mktData = null;
                //如果存在该symbol的上次ticksnapshot
                if (_symTickSnapMap.TryGetValue(sym, out mktData))
                {
                    k.size = pDepthMarketData.Volume - mktData.Volume;
                }
                else
                {
                    _symTickSnapMap.Add(sym, pDepthMarketData);//插入新的键值
                    k.size = pDepthMarketData.Volume;
                }
                _symTickSnapMap[sym] = pDepthMarketData;
                //k.size = pDepthMarketData.Volume;

                //if (isPaperTradeEnabled)
                //{
                //ptt.newTick(k);
                // }
                // send it
                //debug("got tick");
                if (GotTick != null)
                    GotTick(k);
                //tl.newTick(k);//tlserver给client发送tick
                //_asynTickData.newTick(k);//异步处理实时储存tick数据
                //debug(k.ToString());

                //debug("last" + tt["IF1210"].ask.ToString());
                //tt.newTick(k);//tickTracker维护最新的tick数据
                //debug("new" + k.size.ToString());
                //debug(blt["IF1210"].RecentBar.ToString());
                //debug(k.time.ToString()+"|"+k.date.ToString());
                //debug(s);
                //Debug.WriteLine(s);
                //Console.WriteLine(s);
            }
            catch (Exception ex)
            {
                //debug(ex.ToString());
            }
        }



        #endregion
    }
}
