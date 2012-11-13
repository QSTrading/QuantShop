using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.Data;


namespace TradingLib.GUI
{
    public partial class ctExchangeList : UserControl
    {
        private fmExchEdit editform = null;
        

        public ctExchangeList()
        {
            InitializeComponent();
            initMenu();
            //DataTable dt = ExchageTracker.getExchTable();
            //获得交易所列表
            //exchList.DataSource = ExchageTracker.getExchTable();
            //exchList.Columns[1] = dt.Columns[1];
             //ExchageTracker.getExchTable();

            //exchList.Rows.Add(new object[] { "11", "22", "33", "44" });
            //DataTable dt = ExchageTracker.getExchTable();

        }

        private void initMenu()
        {
            exchList.ContextMenuStrip = new ContextMenuStrip();
            exchList.ContextMenuStrip.Items.Add("增加交易所", null, new EventHandler(mAddExchange));

            exchList.ContextMenuStrip.Items.Add("删除交易所", null, new EventHandler(mDelExchange));
        }
        private void mAddExchange(object sender, EventArgs e)
        {

            editform.ShowDialog("Add");

        
        }
        private void mDelExchange(object sender, EventArgs e)
        {
            fmConfirm fm = new fmConfirm("确认删除交易所?");
            fm.SendConfimEvent +=new TradeLink.API.VoidDelegate(fm_SendConfimEvent);
            fm.ShowDialog();
            

        }
        private void fm_SendConfimEvent()
        {
            for (int i = 0; i < exchList.SelectedRows.Count; i++)
            {
                ExchangeTracker.delExchange(exchList[0, exchList.SelectedRows[i].Index].Value.ToString());
            }
            editform_ExchangChanged();
            
        }

        private void editform_ExchangChanged()
        {
            //label1.Text = "exchange !!";
            exchList.DataSource = ExchangeTracker.getExchTable();
            
        }

        private void ctExchangeListControl_Load(object sender, EventArgs e)
        {
            try
            {
                exchList.DataSource = ExchangeTracker.getExchTable();
                editform = new fmExchEdit();
                editform.ExchangChanged +=new TradeLink.API.VoidDelegate(editform_ExchangChanged);
            }
            catch (Exception ex)
            { 
                
            }
        }

        private void exchList_DoubleClick(object sender, EventArgs e)
        {

            //ExchageTracker.delExchange(exchList[0, exchList.SelectedRows[i].Index].Value.ToString());
            string code = exchList[1, exchList.SelectedRows[0].Index].Value.ToString();
            string name = exchList[2, exchList.SelectedRows[0].Index].Value.ToString();
            string s = exchList[3, exchList.SelectedRows[0].Index].Value.ToString();
            Exchange ex = new Exchange(code, name, (Country)Enum.Parse(typeof(Country), s, true));
            editform.ShowDialog(ex);
        }
    }
}
