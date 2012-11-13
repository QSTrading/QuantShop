using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.Data;

namespace TradingLib.GUI.Server
{
    public partial class fmAccountEdit : Form
    {
        private AccountBase _acc=null;
        public fmAccountEdit(AccountBase a)
        {
            _acc = a;
            InitializeComponent();
            //设定显示的Account
            ctRuleSet1.Account = a;
            //加载对应的数据
            ctRuleSet1.LoadAccountRule();
            Text = "编辑账户:" + _acc.ID;
            
        }

    }
}
