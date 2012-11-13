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
using WeifenLuo.WinFormsUI.Docking;
namespace TradingLib.GUI
{
    public partial class fmSecListEdit : Form
    {
        public event SecurityDelegate DefaultBasketSymAdd;
        public event SecurityDelegate DefaultBasketSymDel;
        
        public fmSecListEdit()
        {
            InitializeComponent();
            ctSecurityList1.SendSecurityBaseEvent +=new SecurityBaseDel(ctSecurityList1_SendSecurityBaseEvent);
            //
        }

        //securitylist选择发生变化时我们进行的操作
        private void ctSecurityList1_SendSecurityBaseEvent(SecurityBase sec)
        {
            //MessageBox.Show(sec.Type.ToString());
            //根据选择的MasterSecurity来生成对应的expire列表
            UIUtil.genComboBoxExpire(ref expire);
            if (sec.Type == SecurityType.FUT)
            {
                margin.Enabled = true;
                expire.Enabled = true;
            }
            else
            {

                margin.Enabled = false;
                expire.Enabled = false;
            }
        }
        private void addBasket_Click(object sender, EventArgs e)
        {
            fmSecListEdit_NewList fm = new fmSecListEdit_NewList();
            fm.ConfirmBasketName += new TradeLink.API.StringParamDelegate(fm_ConfirmBasketName);
            fm.ShowDialog();
            //BasketTracker.addBasket();
        }

        private void fm_ConfirmBasketName(string s)
        {
            BasketTracker.addBasket(s);
            ctSecListBox.onAddDelBasket(s);
            //ctSecListBox.
        }

        private void dellBasket_Click(object sender, EventArgs e)
        {
            fmConfirm fm = new fmConfirm("确认删除列表?");
            fm.SendConfimEvent +=new TradeLink.API.VoidDelegate(fm_SendConfimEvent);
            fm.ShowDialog();
            
        }

        private void fm_SendConfimEvent()
        {
            BasketTracker.delBasket(ctSecListBox.SelectedBasket);
            ctSecListBox.onAddDelBasket(string.Empty);
        }

        //向某个basket中增加symbol
        private void btnAdd_Click(object sender, EventArgs e)
        {
            //string s = ctSecListBox.SelectedBasket;
            //MessageBox.Show(s);
            //MessageBox.Show("basket:" + ctSecListBox.SelectedBasket + " sec:" + ctSecurityList1.SelectedMasterSec.FullName);
            string monthcode = SymbolHelper.genExpireCode(ctSecurityList1.SelectedMasterSec, ((ValueObject<int>)expire.SelectedItem).Value);
            
            string sym = BasketTracker.addSecIntoBasket(ctSecurityList1.SelectedMasterSec, ctSecListBox.SelectedBasket,monthcode);
            //ctSecListBox.onBasketChanged();
            
            ctSecListBox.onSecListChanged();
            if (DefaultBasketSymAdd != null && ctSecListBox.SelectedBasket == "Default")
            {
                Security s = BasketTracker.getSecFromBasket(sym, "Default");
                DefaultBasketSymAdd(s);
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            Security sec = ctSecListBox.SelectedSecurity;
            BasketTracker.delSecFromBasket(ctSecListBox.SelectedSecurity.Symbol, ctSecListBox.SelectedBasket);
            ctSecListBox.onSecListChanged();
            if (DefaultBasketSymDel != null && ctSecListBox.SelectedBasket == "Default")
            {
                DefaultBasketSymDel(sec);
            }
        }
    }
}
