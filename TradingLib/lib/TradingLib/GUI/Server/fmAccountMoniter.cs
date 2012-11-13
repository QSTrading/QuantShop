using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using TradingLib.Broker.CTP;
using TradeLink.API;
using CTP;


namespace TradingLib.GUI.Server
{
    public partial class fmAccountMoniter : DockContent
    {
        public event VoidDelegate QueryAccountEvent;
        public DataTable dtInstruments;
        public fmAccountMoniter()
        {
            InitializeComponent();
            //#region 定义合约表/数据表
            //合约/名称/交易所/合约数量乘数/最小波动/多头保证金率/空头保证金率/限价单下单最大量/最小量
            this.dtInstruments = new DataTable("instruments");
            this.dtInstruments.Columns.Add("合约", string.Empty.GetType());
            this.dtInstruments.Columns.Add("名称", string.Empty.GetType());
            this.dtInstruments.Columns.Add("交易所", string.Empty.GetType());
            this.dtInstruments.Columns.Add("合约数量", int.MinValue.GetType());
            this.dtInstruments.Columns.Add("最小波动", double.NaN.GetType());
            this.dtInstruments.Columns.Add("保证金-多", double.NaN.GetType());
            this.dtInstruments.Columns.Add("保证金-空", double.NaN.GetType());
            this.dtInstruments.Columns.Add("手续费", double.NaN.GetType());	//单独查询获得
            this.dtInstruments.Columns.Add("手续费-平仓", double.NaN.GetType());	//
            this.dtInstruments.Columns.Add("最大下单量-限", int.MinValue.GetType());
            this.dtInstruments.Columns.Add("最小下单量-限", int.MinValue.GetType());
            //this.dtInstruments.Columns.Add("(经纪)保证金-多", double.NaN.GetType());	//单独查询获得:重新计算后覆盖上面的"保证金"
            //this.dtInstruments.Columns.Add("(经纪)保证金-空", double.NaN.GetType());	//
            //this.dtInstruments.Columns.Add("相对交易所", int.MinValue.GetType());
            this.dtInstruments.Columns.Add("自选", true.GetType());
            this.dtInstruments.Columns["自选"].DefaultValue = false;
            this.dtInstruments.Columns.Add("套利", true.GetType());
            this.dtInstruments.Columns["套利"].DefaultValue = false;
            this.dtInstruments.PrimaryKey = new DataColumn[] { this.dtInstruments.Columns["合约"] };	//主键
        }

        public void onNewState(EnumProgessState _state, string _msg)
        {
            this.comboBoxErrMsg.Items.Insert(0, DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss.fff") + "|" + _state.ToString() + ":" + _msg);
            this.comboBoxErrMsg.SelectedIndex = 0;
            if (_state == EnumProgessState.OnError || _state == EnumProgessState.OnDisConnect || _state == EnumProgessState.OnErrOrderInsert ||
                _state == EnumProgessState.OnErrOrderAction || _state == EnumProgessState.OnMdDisConnected)
                this.toolTipInfo.ToolTipIcon = ToolTipIcon.Warning;
            else
                this.toolTipInfo.ToolTipIcon = ToolTipIcon.Info;
            //this.toolTipInfo.Show(_msg, this, 100,100, 5000);	//冒泡
        }
        /// 将获得的响应struct填充到HFListView中
        public void onNewObject(ObjectAndKey oak)
        {
            //报单返回
            if (oak.Object.GetType() == typeof(ThostFtdcOrderField))
            {
                ThostFtdcOrderField field = (ThostFtdcOrderField)oak.Object;
                ListViewItem lvi = this.listViewOrder.Items.Find(oak.Key, false).FirstOrDefault();
                if (lvi == null)
                {
                    lvi = new ListViewItem(field.InstrumentID);	//1:frondid+sessionid,2:ordersysid
                    lvi.Name = oak.Key;
                    lvi.SubItems.Add(field.Direction == EnumDirectionType.Buy ? "买" : "卖");
                    lvi.SubItems.Add("");//field.CombOffsetFlag == EnumOffsetFlagType.Open ? "开仓" : field.CombOffsetFlag == EnumOffsetFlagType.Close ? "平仓" : "平今");
                    lvi.SubItems.Add(field.LimitPrice.ToString());				//报单价格
                    lvi.SubItems.Add("0");										//成交均价
                    lvi.SubItems.Add(field.VolumeTotalOriginal.ToString());		//委托数量
                    lvi.SubItems.Add(field.VolumeTraded.ToString());			//成交量
                    lvi.SubItems.Add(field.InsertTime);							//报单时间
                    lvi.SubItems.Add(field.InsertTime);							//成交时间
                    lvi.SubItems.Add(field.OrderLocalID);
                    lvi.SubItems.Add(field.OrderSysID.Trim());							//报单编号:有成交时对应
                    lvi.SubItems.Add(field.StatusMsg);							//报单状态
                    //建组
                    lvi.Group = this.listViewOrder.Groups[field.OrderStatus.ToString()];
                    this.listViewOrder.Items.Add(lvi);
                }
                else
                {
                    lvi.SubItems[6].Text = field.VolumeTraded.ToString();			//成交量
                    lvi.SubItems[8].Text = field.UpdateTime;						//成交时间:最后更新时间
                    lvi.SubItems[10].Text = field.OrderSysID.Trim();						//报单编号:有成交时对应
                    lvi.SubItems[11].Text = field.StatusMsg;						//报单状态
                    lvi.Group = this.listViewOrder.Groups[field.OrderStatus.ToString()];
                }
            } //成交响应
            /*
            else if (oak.Object.GetType() == typeof(CThostFtdcTradeField))
            {
                CThostFtdcTradeField field = (CThostFtdcTradeField)oak.Object;
                if (this.isQryHistoryTrade)	//历史成交
                {
                    ListViewItem lvi = new ListViewItem(field.InstrumentID);
                    //lvi.SubItems.Add(string.Format("{0}.{1}.{2}", field.TradeDate.Substring(0, 4), field.TradeDate.Substring(4, 2), field.TradeDate.Substring(6, 2)));//日期
                    lvi.SubItems.Add(field.TradeTime);				//时间
                    lvi.SubItems.Add(field.ExchangeID);
                    lvi.SubItems.Add(field.OrderSysID.Trim());
                    lvi.SubItems.Add(field.Direction == EnumDirectionType.Buy ? "买" : "卖");
                    lvi.SubItems.Add(field.OffsetFlag == EnumOffsetFlagType.Open ? "开" : "平");
                    lvi.SubItems.Add(field.Price.ToString("F2"));	//价格
                    lvi.SubItems.Add(field.Volume.ToString());		//数量
                    this.hfListViewTrade.Items.Add(lvi);
                    this.hfListViewTrade.SortColumn = 1;
                    this.hfListViewTrade.Sorting = SortOrder.Descending;
                    this.hfListViewTrade.Sort();
                }
                else
                {
                    ListViewItem lvi = this.listViewOrder.FindItemWithText(oak.Key.Trim(), true, 0);
                    if (lvi != null)
                    {
                        lvi.SubItems[4].Text = field.Price.ToString();					//成交均价
                        //lvi.SubItems[6].Text = field.Volume.ToString();					//成交量
                        lvi.SubItems[8].Text = field.TradeTime;							//成交时间
                    }
                }
            } 
             * */
            //持仓
            else if (oak.Object.GetType() == typeof(ThostFtdcInvestorPositionField))
            {
                ThostFtdcInvestorPositionField field = (ThostFtdcInvestorPositionField)oak.Object;
                ListViewItem lvi = this.listViewPosition.Items.Find(oak.Key, false).FirstOrDefault();
                int unit = (int)this.dtInstruments.Rows.Find(field.InstrumentID)["合约数量"];	//数量乘积,用于计算价格
                if (lvi == null)
                {
                    lvi = new ListViewItem(field.InstrumentID);
                    lvi.Name = oak.Key;
                    lvi.SubItems.Add(field.PosiDirection == EnumPosiDirectionType.Long ? "多" : "空");
                    lvi.SubItems.Add(field.YdPosition.ToString());		//昨日持仓
                    lvi.SubItems.Add(field.Position.ToString());		//今日持仓
                    lvi.SubItems.Add((field.PositionCost / unit / field.Position).ToString("F2"));	//持仓成本
                    lvi.SubItems.Add(field.PositionProfit.ToString("F2"));	//持仓盈亏
                    lvi.SubItems.Add(field.OpenVolume == 0 ? "0" : (field.OpenAmount / unit / field.OpenVolume).ToString("F2"));		//开仓成本
                    lvi.SubItems.Add(field.OpenVolume.ToString());		//开仓手数
                    lvi.SubItems.Add(field.CloseVolume == 0 ? "0" : (field.CloseAmount / unit / field.CloseVolume).ToString("F2"));		//平仓价格
                    lvi.SubItems.Add(field.CloseVolume.ToString());		//平仓手数
                    lvi.SubItems.Add(field.CloseProfit.ToString("F2"));		//平仓盈亏
                    lvi.SubItems.Add(field.Commission.ToString("F2"));		//手续费
                    //分组
                    lvi.Group = this.listViewPosition.Groups[field.PositionDate.ToString()];
                    this.listViewPosition.Items.Add(lvi);
                }
                else //更新
                {
                    lvi.SubItems[3].Text = field.Position.ToString();	//今日持仓
                    lvi.SubItems[4].Text = (field.PositionCost / unit / field.Position).ToString("F2");	//持仓成本
                    lvi.SubItems[5].Text = field.PositionProfit.ToString("F2");	//持仓盈亏
                    lvi.SubItems[6].Text = field.OpenVolume == 0 ? "0" : (field.OpenAmount / unit / field.OpenVolume).ToString("F2");
                    lvi.SubItems[7].Text = field.OpenVolume.ToString();
                    lvi.SubItems[8].Text = field.CloseVolume == 0 ? "0" : (field.CloseAmount / unit / field.CloseVolume).ToString("F2");	//平仓价格
                    lvi.SubItems[9].Text = field.CloseVolume.ToString();	//平仓手数
                    lvi.SubItems[10].Text = field.CloseProfit.ToString("F2");	//平仓盈亏
                    lvi.SubItems[11].Text = field.Commission.ToString("F2");	//手续费
                } //无持仓删除
                if (lvi.SubItems[3].Text == "0")		//无持仓:删除
                    this.listViewPosition.Items.Remove(lvi);
            }
            /*
             * //预埋报单
        else if (oak.Object.GetType() == typeof(CThostFtdcParkedOrderField))
        {
            CThostFtdcParkedOrderField field = (CThostFtdcParkedOrderField)oak.Object;
            ListViewItem lvi = this.hfListViewParkedOrder.Items.Find(oak.Key, false).FirstOrDefault();
            if (lvi == null)
            {
                lvi = new ListViewItem(field.InstrumentID);
                lvi.Name = oak.Key;
                lvi.SubItems.Add(field.StopPrice.ToString());			//触发价格
                lvi.SubItems.Add(field.Direction == EnumDirectionType.Buy ? "买" : "卖");
                lvi.SubItems.Add(field.CombOffsetFlag_0 == EnumOffsetFlagType.Open ? "开" : "平");
                lvi.SubItems.Add(field.OrderPriceType.ToString());		//价格条件
                lvi.SubItems.Add(field.LimitPrice.ToString());			//报单价格
                lvi.SubItems.Add(field.VolumeTotalOriginal.ToString());	//报单数量
                lvi.SubItems.Add(field.Status.ToString());				//埋单状态
                lvi.SubItems.Add(field.ErrorMsg);						//错误信息
                this.hfListViewParkedOrder.Items.Add(lvi);
            }
            else //更新
            {
                lvi.SubItems[7].Text = field.Status.ToString();				//埋单状态
                lvi.SubItems[8].Text = field.ErrorMsg;						//错误信息
            }
        } //合约
        **/
        }

        public void FreshAccount(ThostFtdcTradingAccountField pTradingAccount)
        {
            this.ctTradeAccount1.Account = pTradingAccount;

            //this.userControlTradeAccount1.Account = pTradingAccount;
            //this.textBoxFutureFetchAmount.Text = pTradingAccount.WithdrawQuota.ToString();	//可用资金:银转
             //* */
        }
        private void fmAccountMoniter_Load(object sender, EventArgs e)
        {

        }

        private void btn_freshAccount_Click(object sender, EventArgs e)
        {
            if (QueryAccountEvent != null)
                QueryAccountEvent();
        }
    }
}
