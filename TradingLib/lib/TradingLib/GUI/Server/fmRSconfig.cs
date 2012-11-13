using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using TradingLib.API;
using TradingLib.Data;
using TradingLib.GUI;
namespace TradingLib.GUI.Server
{
    public partial class fmRSconfig : Form
    {
        public event RuleAddedDel SendRuleAddEvent;
        Type _t;
        IRuleCheck _o;
        public fmRSconfig(Type t)
        {
            InitializeComponent();
            _t = t;
            _o = (IRuleCheck)Activator.CreateInstance(_t);

            UIUtil.genComboBoxList<CompareType>(ref comboBox1, false);
            label1.Text = _o.ValueName;
            initForm();
        }

        private void initForm()
        {
            //得到RuleSet的名称与描述
            
            string rsname = (string)_t.InvokeMember("Name",
                BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetProperty,
                null, null, null);
            string rsdescription = (string)_t.InvokeMember("Description",
                BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetProperty,
                null, null, null);
            Text = rsname;
            textBox1.Text = rsdescription;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ValueObject<CompareType> xx = new ValueObject<CompareType>();
            _o.Compare = ((ValueObject<CompareType>)comboBox1.SelectedItem).Value;
            if (_o.ValidValue(cfgvalue.Text))
            {
                //MessageBox.Show("wrong value");
                _o.Value = cfgvalue.Text;
            }
            if (SendRuleAddEvent != null)
            {
                SendRuleAddEvent(_o);
            }
            Close();
        }

    
    }
}
