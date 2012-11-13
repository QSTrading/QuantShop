using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.Core;
namespace TradingLib.GUI
{
    public partial class fmPositionExitStrategy : Form
    {
        private PositionCheckCentre _poscheckCentre;
        public PositionCheckCentre PositionCheckCentre { get { return _poscheckCentre; }
            set {
                _poscheckCentre = value;
                ctPositionExitStrategy1.PositionCheckCentre = _poscheckCentre;
            }
        }
        public fmPositionExitStrategy()
        {
            InitializeComponent();
            Text = "仓位策略编辑:" + "未选择合约";
            ctDefaultSymbolList1.SendSecuritySelectedEvent += new TradeLink.API.SecurityDelegate(ctDefaultSymbolList1_SendSecuritySelectedEvent);
            ctPositionExitStrategy1.SendApplyEvent += () => 
            {
                this.Close();
            };
        }

        void ctDefaultSymbolList1_SendSecuritySelectedEvent(TradeLink.API.Security sec)
        {
            //throw new NotImplementedException();
            //设定ctPositionExitStrategy组件所对应的组件
            ctPositionExitStrategy1.Security = sec;
            Text = "仓位策略编辑:" + sec.Symbol;
        }
    }
}
