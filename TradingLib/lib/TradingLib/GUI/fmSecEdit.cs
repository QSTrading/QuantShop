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
namespace TradingLib.GUI
{
    public partial class fmSecEdit : Form
    {
        private string _editfullname = null;
        public event VoidDelegate SecurityChangedEvent;
        public fmSecEdit()
        {
            InitializeComponent();
            UIUtil.genComboBoxList<TradeLink.API.SecurityType>(ref SECType);
            UIUtil.genComboBoxList<Currency>(ref SECCurrency);
            UIUtil.genTickList(ref SECPriceTick);
            UIUtil.genExchangeList(ref SECExchange);

           
            FormClosed +=new FormClosedEventHandler(SecEditForm_FormClosed);
            StartPosition = FormStartPosition.CenterParent;
        }

        //打开窗口用于增加新的symbol
        public  DialogResult ShowDialog(string s)
        {
            if (s == "Add")
            {
                SECSymbol.Text = "";
                SECName.Text = "";
                //foreach(//SECExchange.SelectedItems
            }
            else
            {
                Security sec = SecurityImpl.Parse(s);
                _editfullname = s;
            
            }
        
            return base.ShowDialog();
        }

        public DialogResult ShowDialog(Security sec)
        {
            _editfullname = sec.FullName;
            SECSymbol.Text = sec.Symbol;
            //ValueObject<SecurityType> t = new ValueObject<SecurityType>();
            //t.Name = LibUtil.GetEnumDescription(t);
            //t.Value = ;
            SECType.SelectedValue = sec.Type;
            SECType.Enabled = false;
            SECCurrency.Enabled = false;
            SECPriceTick.Text = sec.PriceTick.ToString();
            SECMultiple.Value = sec.Multiple;
            
            SECMargin.Text = sec.Margin.ToString();
            SECName.Text = sec.Description;
            //sec.Date = 20121220;
            //MessageBox.Show(sec.ToString());
            //MessageBox.Show(SECExchange.Items[0].ToString());
            for (int i = 0; i < SECExchange.Items.Count; i++)
            {
                //利用datasource绑定的list item对象为valueobject我们需要转换成valueobject然后取得对应的value才考验得到数值
                if (((ValueObject<Exchange>)SECExchange.Items[i]).Value.Index != sec.DestEx)
                {
                    SECExchange.SetItemChecked(i, false);
                }
                else 
                {
                    SECExchange.SetItemChecked(i, true);
                }
            }

            return base.ShowDialog();
        
        
        }

        private void SecurityChanged()
        {
            if (SecurityChangedEvent != null)
            {
                SecurityChangedEvent();
            }
        }

        //单选交易所列表
        private void SECExchange_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (SECExchange.CheckedItems.Count > 0)
            {
                for (int i = 0; i < SECExchange.Items.Count; i++)
                {
                    if (i != e.Index)
                    {
                        SECExchange.SetItemChecked(i, false);
                    }
                }
            
            }
        }


        private void SecEditForm_FormClosed(object sender, EventArgs e)
        {
            _editfullname = null;
               
        
        }

        private void ok_Click(object sender, EventArgs e)
        {
            //编辑某个security
            SecurityBase sec = new SecurityBase(SECSymbol.Text, ((Exchange)SECExchange.SelectedValue).Index, (SecurityType)SECType.SelectedValue, SECName.Text, (int)SECMultiple.Value, (decimal)SECPriceTick.SelectedValue, Convert.ToDecimal(SECMargin.Text));
            if (_editfullname != null)
            {
                if (SecurityTracker.HaveSecurity(_editfullname))
                {
                    SecurityTracker.delSecurity(_editfullname);
                }
                
            
            }

            //MessageBox.Show(SecurityTracker.HaveSecurity(sec).ToString());
            SecurityTracker.updateSecurity(sec);
            SecurityChanged();
            Close();
        }

        private void cancle_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
