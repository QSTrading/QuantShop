using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradeLink.API;
using TradingLib.Data;
using System.Collections;

namespace TradingLib.GUI
{
    public partial class fmExchEdit : Form
    {
        public event VoidDelegate ExchangChanged;
        private string _editIndex = null;
        public fmExchEdit()
        {
            InitializeComponent();
            UIUtil.genComboBoxList<Country>(ref EXCountry);
            FormClosed +=new FormClosedEventHandler(ExchEditForm_FormClosed);
            StartPosition = FormStartPosition.CenterParent;
            
        }
        private void ExchEditForm_FormClosed(object sender ,EventArgs e)
        {
            _editIndex = null;

        }

        //重载Form窗口的showdialog函数,用于实现增加,修改等不同功能
        //增加某个Exchange
        public DialogResult ShowDialog(string s)
        {
            if (s == "Add")
            {
                EXIndex.Text = "";
                EXCode.Text = "";
                EXCountry.SelectedValue = Country.CN;
                EXName.Text = "";
                
                return base.ShowDialog();
            }
            return base.ShowDialog();
            
        }
        //修改某个Exchange
        public DialogResult ShowDialog(Exchange ex)
        {
            _editIndex = ex.Index;
            EXIndex.Text = ex.Index;
            EXCode.Text = ex.EXCode;
            EXCountry.SelectedValue = ex.Country;
            EXName.Text = ex.Name;
            //EXCode.Enabled = false;
            return base.ShowDialog();
        
        }
        //出发exchange变动事件
        private void ExchangChangedHandler()
        {

            if (ExchangChanged != null)
            {
                ExchangChanged();
            }
        }

        //增加或者修改某个ex
        private void button1_Click(object sender, EventArgs e)
        {
            Exchange ex = new Exchange();

            ex.Country = (Country)EXCountry.SelectedValue;
            ex.EXCode = EXCode.Text;
            ex.Name = EXName.Text;
            //检查编辑标记
            //oo.Text = "editindex" + _editIndex.ToString();
            if (_editIndex != null)
            {
                //只是修改除了代码与国家之外的信息
                if (_editIndex == ex.Index)
                {
                    ExchangeTracker.updateExchange(ex);
                    //oo.Text = "修改信息" + _editIndex.ToString();
                }
                else 
                {
                //由于改变了EXIndex我们需要删除原有记录 然后重新增加新的数据
                    //oo.Text = "修改代码" + _editIndex.ToString();
                    ExchangeTracker.delExchange(_editIndex);
                    ExchangeTracker.addExchange(ex);
                }
            }
            ExchangeTracker.addExchange(ex); 
            ExchangChangedHandler();
            Close();
        }
        //删除某个ex
        private void button2_Click(object sender, EventArgs e)
        {
            if (_editIndex != null)
            {
                ExchangeTracker.delExchange(_editIndex);
                ExchangChangedHandler();
                //_editIndex = null;     
               
            }
            Close();
        }
    }

    





}
