using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.Data;
using TradeLink.API;
using TradeLink.Common;
using Easychart.Finance.DataProvider;
using TradingLib.Broker.CTP;
using System.Threading;
namespace demo3
{
    public partial class Form1 : Form
    {
        private QSMemoryDataManager _mdm = null;
        private QSFileDataManager _fileDM = null;

        public Form1()
        {
            InitializeComponent();

            _fileDM = new QSFileDataManager("d:\\data\\");
            _fileDM.SendDebugEvent += new DebugDelegate(debug);
            _mdm = new QSMemoryDataManager(_fileDM);
            _mdm.SendDebugEvent += new DebugDelegate(debug);

        }

        void debug(string msg)
        {
            debugControl1.GotDebug(msg);
        }
        TradeApi tradeApi;
        string _FRONT_ADDR = "tcp://asp-sim2-front1.financial-trading-platform.com:26205";  // 前置地址
        string _BROKER_ID = "4070";                       // 经纪公司代码
        string _INVESTOR_ID = "00295";                    // 投资者代码
        string _PASSWORD = "123456";                     // 用户密码
        private void button1_Click(object sender, EventArgs e)
        {

            tradeApi = new TradeApi(_INVESTOR_ID, _PASSWORD, _BROKER_ID, _FRONT_ADDR);
            tradeApi.OnFrontConnect += new TradeApi.FrontConnect(tradeApi_OnFrontConnect);
            tradeApi.OnRspUserLogin += new TradeApi.RspUserLogin(tradeApi_OnRspUserLogin);
            tradeApi.OnRspQrySettlementInfo += new TradeApi.RspQrySettlementInfo(tradeApi_OnRspQrySettlementInfo);
            tradeApi.OnRspQrySettlementInfoConfirm += new TradeApi.RspQrySettlementInfoConfirm(tradeApi_OnRspQrySettlementInfoConfirm);
            tradeApi.OnRspQryInstrument += new TradeApi.RspQryInstrument(tradeApi_OnRspQryInstrument);
            tradeApi.OnRspQryOrder += new TradeApi.RspQryOrder(tradeApi_OnRspQryOrder);
            tradeApi.Connect();
        }

        void tradeApi_OnRspQryOrder(ref CThostFtdcOrderField pOrder, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            debug("委托查询回报");
        }

        void tradeApi_OnRspQryInstrument(ref CThostFtdcInstrumentField pInstrument, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            debug("合约查询回报");
        }

        void tradeApi_OnRspQrySettlementInfoConfirm(ref CThostFtdcSettlementInfoConfirmField pSettlementInfoConfirm, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (pSettlementInfoConfirm.BrokerID != "" && DateTime.ParseExact(pSettlementInfoConfirm.ConfirmDate, "yyyyMMdd", null) >= DateTime.Today)
            {
                //    this.BeginInvoke(new Action<EnumProgessState, string>(progress), EnumProgessState.QryInstrument, "查合约...");
                //tradeApi.QryInstrument();
                debug("已经确认过结算信息");
                Thread.Sleep(1000);
                //tradeApi.QryInstrument();
                tradeApi.QryOrder();
                
            }
            else
            { 
                
            }
        }

        void tradeApi_OnRspQrySettlementInfo(ref CThostFtdcSettlementInfoField pSettlementInfo, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            throw new NotImplementedException();
        }

        void tradeApi_OnRspUserLogin(ref CThostFtdcRspUserLoginField pRspUserLogin, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            debug("用户登入回报:" + pRspInfo.ErrorMsg);

            tradeApi.QrySettlementInfoConfirm();
        }

        void tradeApi_OnFrontConnect()
        {
            debug("前置连接回报 准备登入");
            
            tradeApi.UserLogin();
        }

        private void button2_Click(object sender, EventArgs e)
        {
         
        }
    }
}
