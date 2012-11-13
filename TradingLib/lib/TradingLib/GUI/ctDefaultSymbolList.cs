using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.API;
using TradeLink.API;

namespace TradingLib.GUI
{
    public partial class ctDefaultSymbolList : UserControl
    {
        public event SecurityDelegate SendSecuritySelectedEvent;
        private Security _selectSecurity;
        public ctDefaultSymbolList()
        {
            InitializeComponent();
            try
            {
                UIUtil.genListBoxBasket(ref listBox1, "Default");
            }
            catch (Exception ex)
            { }
        }
        //触发所选security改变事件,用于触发其他组件作出相应变化
        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            _selectSecurity = ((ValueObject<Security>)listBox1.SelectedItem).Value;
            if (SendSecuritySelectedEvent != null)
                SendSecuritySelectedEvent(_selectSecurity);
        }


    }
}
