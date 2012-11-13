using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.Broker.CTP;
using CTP;
namespace TradingLib.GUI
{
	public partial class ctTradeAccount: UserControl
	{
        public ctTradeAccount()
		{
			InitializeComponent();
		}
		/// <summary>
		/// 是否在使用
		/// </summary>
		public bool IsActive { get; set; }
		/// <summary>
		/// 数据
		/// </summary>
		public ThostFtdcTradingAccountField Account
		{
			set
			{
				IsActive = true;
				this.labelAvailable.Text = value.Available.ToString("F2");		//可用资金
				this.labelCloseProfit.Text = value.CloseProfit.ToString("F2");	//平仓盈亏
				this.labelCommission.Text = value.Commission.ToString("F2");		//手续费
				this.labelCurrMargin.Text = value.CurrMargin.ToString("F2");		//当前保证金总额
				this.labelDeposit.Text = value.Deposit.ToString("F2");			//入金
				this.labelFrozenCommission.Text = value.FrozenCommission.ToString("F2");	//冻结手续费
				this.labelFrozenMargin.Text = value.FrozenMargin.ToString("F2");	//冻结保证金
				this.labelPositionProfit.Text = value.PositionProfit.ToString("F2");	//持仓盈亏
				this.labelPreBalance.Text = value.PreBalance.ToString("F2");		//上次结算
				this.labelReserve.Text = value.Reserve.ToString("F2");			//基本准备金
				this.labelWithdraw.Text = value.Withdraw.ToString("F2");			//出金
				this.labelWithdrawQuota.Text = value.WithdrawQuota.ToString("F2");	//可取资金
				this.label1Static.Text = (value.PreBalance + value.Deposit - value.Withdraw).ToString("F2");	//静态权益
				this.labelCurrent.Text = (value.PreBalance + value.Deposit - value.Withdraw + value.PositionProfit + value.CloseProfit - value.Commission).ToString("F2");//动态权益
			}
		}
	}
}
