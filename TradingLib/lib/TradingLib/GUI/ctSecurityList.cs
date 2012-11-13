using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.Data;
using TradeLink.API;
using System.Windows.Forms;
using TradeLink.API;
using TradeLink.Common;
namespace TradingLib.GUI
{
    public partial class ctSecurityList : UserControl
    {
        const string FULLNAME = "全称";
        const string SYMBOL = "代码";
        const string EXCHANGE = "交易所";
        const string DESCRIPTION = "名称";
        const string TYPE = "类型";
        const string MULTIPLE = "乘数";
        const string PRICETICK = "跳";
        const string MARGIN = "保证金";


        //private fmSecEdit editform = null;
        private SecurityBase _secbase = null;
        public event SecurityBaseDel SendSecurityBaseEvent = null;
        public ctSecurityList()
        {
            InitializeComponent();
            initMenu();

            //secList.DataSource = SecurityTracker.getSecuityTable();
            try
            {
                UIUtil.genComboBoxExchange(ref exch,true);
                UIUtil.genComboBoxList<SecurityType>(ref secType,true);
                
                
            }
            catch (Exception ex)
            { 
                
            }
        }

        public SecurityBase SelectedMasterSec { get { return _secbase; } }
        private void initMenu()
        {
            secList.ContextMenuStrip = new ContextMenuStrip();
            secList.ContextMenuStrip.Items.Add("增加证券", null, new EventHandler(mAddSecurity));
            secList.ContextMenuStrip.Items.Add("删除证券", null, new EventHandler(mDelSecurity));
        }

        //增加security
        private void mAddSecurity(object sender,EventArgs e)
        {
            fmSecEdit fm = new fmSecEdit();
            fm.SecurityChangedEvent += new VoidDelegate(editform_SecurityChanged);
            fm.ShowDialog("Add");
        }

        private void editform_SecurityChanged()
        {
            //secList.DataSource = SecurityTracker.getSecuityTable();
            RefreshSecList();
        }

        //删除security
        private void mDelSecurity(object sender, EventArgs e)
        {
            fmConfirm fm = new fmConfirm("确认删除证券?");
            fm.SendConfimEvent +=new VoidDelegate(fm_SendConfimEvent);
            fm.ShowDialog();

        }

        private void fm_SendConfimEvent()
        {
            for (int i = 0; i < secList.SelectedRows.Count; i++)
            {
                SecurityTracker.delSecurity(secList[0, secList.SelectedRows[i].Index].Value.ToString());
                //ExchangeTracker.delExchange(_editIndex);
            }
            editform_SecurityChanged();
        }


        private void ctSecurityListControl_Load(object sender, EventArgs e)
        {
            try
            {
                //label1.Text = "loading";
                //label1.Text = SecurityTracker.getSecuityTable().Rows.Count.ToString();
                secList.DataSource = SecurityTracker.getSecuityTable();

                //editform = new fmSecEdit();
                //editform.SecurityChangedEvent +=new TradeLink.API.VoidDelegate(editform_SecurityChanged);
            }
            catch (Exception ex)
            { 
            
            }
        }



        private void secList_DoubleClick(object sender, EventArgs e)
        {
            string sym = secList[SYMBOL, secList.SelectedRows[0].Index].Value.ToString();
            string exchange = secList[EXCHANGE, secList.SelectedRows[0].Index].Value.ToString();
            string des = secList[DESCRIPTION, secList.SelectedRows[0].Index].Value.ToString();
            SecurityType sectype = (SecurityType)Enum.Parse(typeof(SecurityType), secList[TYPE, secList.SelectedRows[0].Index].Value.ToString(), true);
            int multiple = Convert.ToInt16(secList[MULTIPLE, secList.SelectedRows[0].Index].Value.ToString());
            decimal tick = Convert.ToDecimal(secList[PRICETICK, secList.SelectedRows[0].Index].Value.ToString());
            decimal margin = Convert.ToDecimal(secList[MARGIN, secList.SelectedRows[0].Index].Value.ToString());

            Security sec = new SecurityBase(sym,exchange,sectype,des,multiple,tick,margin);

            fmSecEdit fm = new fmSecEdit();
            fm.SecurityChangedEvent += new VoidDelegate(editform_SecurityChanged);
            fm.ShowDialog(sec);
            //editform.ShowDialog(sec);

        }

        //对外发送所选masterSecurity变化时间
        private void SelectedSecurityBaseChanged()
        {
            if (SendSecurityBaseEvent != null)
            {
                SendSecurityBaseEvent(SelectedMasterSec);
            }
        }

        //security list选择项发生变化时的的操作
        private void secList_SelectionChanged(object sender, EventArgs e)
        {
            //当我们选中了list中的某个条目时,我们才进行判断
            if (secList.SelectedRows.Count != 0)
            {
                string sym = secList[SYMBOL, secList.SelectedRows[0].Index].Value.ToString();
                string exchange = secList[EXCHANGE, secList.SelectedRows[0].Index].Value.ToString();
                string des = secList[DESCRIPTION, secList.SelectedRows[0].Index].Value.ToString();
                SecurityType sectype = (SecurityType)Enum.Parse(typeof(SecurityType), secList[TYPE, secList.SelectedRows[0].Index].Value.ToString(), true);
                int multiple = Convert.ToInt16(secList[MULTIPLE, secList.SelectedRows[0].Index].Value.ToString());
                decimal tick = Convert.ToDecimal(secList[PRICETICK, secList.SelectedRows[0].Index].Value.ToString());
                decimal margin = Convert.ToDecimal(secList[MARGIN, secList.SelectedRows[0].Index].Value.ToString());
                //更新所选证券数据
                _secbase = new SecurityBase(sym, exchange, sectype, des, multiple, tick, margin);
                //当所选security发生变化时调用委托
                SelectedSecurityBaseChanged();
            }
            else
            {
                _secbase = null;
            }
            
        }

        //过滤条件发生变化时更新列表
        private void secType_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshSecList();
        }

        private void exch_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshSecList();
        }

        private void RefreshSecList()
        {
            string sectype = string.Empty;
            string exchange = string.Empty;

            if (((ValueObject<Exchange>)exch.SelectedItem).Name.ToString() == UIUtil.ANY)
            {
                exchange = string.Empty;
            }
            else
            {
                exchange = ((ValueObject<Exchange>)exch.SelectedItem).Value.Index;
            }

            if (((ValueObject<SecurityType>)secType.SelectedItem).Name.ToString() == UIUtil.ANY)
            {
                sectype = string.Empty;
                // MessageBox.Show(((ValueObject<SecurityType>)secType.SelectedItem).Value.ToString());
            }
            else
            {
                sectype = ((ValueObject<SecurityType>)secType.SelectedItem).Value.ToString();
            }

            secList.DataSource = SecurityTracker.getSecuityTable(sectype, exchange);
        }
    }
}
