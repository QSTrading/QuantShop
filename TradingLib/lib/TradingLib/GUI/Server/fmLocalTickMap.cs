using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.GUI;

namespace TradingLib.GUI.Server
{
    //用于设置合约映射关系,从而产生本地Tick数据 形成对应合约的相关数据
    //如果新的tick symbol在映射关系表中有数据,那么系统就复制tick并变换对应的symbol进行数据重发
    //从而达到增加虚拟本地合约,维护连续数据的目的。
    public partial class fmLocalTickMap : Form
    {
        public fmLocalTickMap()
        {
            InitializeComponent();
            UIUtil.genComboBoxExpire(ref hot);
            UIUtil.genComboBoxExpire(ref thismonth);
            UIUtil.genComboBoxExpire(ref nextmonth);

        }


    }
}
