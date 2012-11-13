using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.GUI;
using TradingLib.GUI.Server;
using TradingLib.API;
using TradingLib.Data;

using System.Reflection;

namespace TradingLib.GUI.Server
{
    public partial class ctRuleSet : UserControl
    {
        //private string _acc="admin";
        private AccountBase _a;
        Dictionary<string, Type> dicRule;
        public AccountBase Account { get { return _a; } set { _a = value; } }
        public ctRuleSet()
        {
            InitializeComponent();
            checkedListBoxAccountRule.DisplayMember = "Name";
            checkedListBoxAccountRule.ValueMember = "Value";

            try
            {
                loadRuleSet();
                //LoadAccountRule();
            }
            catch (Exception ex)
            { 
            }
        }

        public void LoadAccountRule()
        {
            if (_a == null || _a.ID == string.Empty)
                return;
            List<string> l = RiskRuleTracker.getRuleTextFromAccount(_a.ID);
            for (int i = 0; i < l.Count;i++ )
            {
                string[] re = l[i].Split('#');
                string rsname = re[0];
                string rstext = re[1];
                //MessageBox.Show(l[i]);
                ValueObject<IRuleCheck> vo =new ValueObject<IRuleCheck>();
                //MessageBox.Show(dicRule[rsname].ToString());
                vo.Value = ((IRuleCheck)Activator.CreateInstance(dicRule[rsname])).FromText(rstext);
                    //
                //MessageBox.Show(vo.Value.ToString());
                vo.Name = vo.Value.RuleDescription;
                checkedListBoxAccountRule.Items.Add(vo,vo.Value.Enable);
                //MessageBox.Show(vo.Value.RuleDescription);
            }
        }

        //加载风控规则
        private void loadRuleSet()
        {
             dicRule = new Dictionary<string, Type>();
             Dictionary<string, Type> tmp = new Dictionary<string, Type>();
            foreach (Type t in Assembly.Load("RiskRuleSet").GetTypes())
            {
                //object[] attributes = t.GetCustomAttributes(typeof(Tablename), false);
                //debug(t.ToString());
                //得到RuleSet的名称与描述
                string rsname = (string)t.InvokeMember("Name",
                    BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic |BindingFlags.Static | BindingFlags.GetProperty,
                    null,null,null);
                string rsdescription = (string)t.InvokeMember("Description",
                    BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetProperty,
                    null, null, null);
                //debug(rsname+"|"+rsdescription);
                dicRule.Add("RiskRuleSet"+"."+t.Name, t);
                tmp.Add(rsname, t);
                //MessageBox.Show("RiskRuleSet" + "." + t.Name);
                //if (t.GetInterface("IRuleCheck") != null)
            }
            UIUtil.genCheckedListBoxRuleSet(ref checkedListBoxRuleSet,tmp);
            
        }

        //为某账户增加风控规则
        private void btnAdd_Click(object sender, EventArgs e)
        {
            fmRSconfig fm = new fmRSconfig(((ValueObject<Type>)checkedListBoxRuleSet.SelectedItem).Value);
            fm.SendRuleAddEvent += new RuleAddedDel(RSconfig_SendRuleAddEvent);
            fm.ShowDialog();
            
        }
        void RSconfig_SendRuleAddEvent(API.IRuleCheck rs)
        {
            ValueObject<IRuleCheck> ro = new ValueObject<IRuleCheck>();
            ro.Name = rs.RuleDescription;
            ro.Value = rs;

            checkedListBoxAccountRule.Items.Add(ro, rs.Enable);
            
        }
        //启用或者禁用某条规则
        private void checkedListBoxAccountRule_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            for (int i = 0; i < checkedListBoxAccountRule.Items.Count; i++)
            {
                if (i == e.Index)
                {
                    bool c = checkedListBoxAccountRule.GetItemChecked(i);
                    ((ValueObject<IRuleCheck>)checkedListBoxAccountRule.Items[i]).Value.Enable = !c;
                    //MessageBox.Show(c.ToString());
                }
            }
        }


        //单选规则
        private void checkedListBoxRuleSet_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (checkedListBoxRuleSet.CheckedItems.Count > 0)
            {
                for (int i = 0; i < checkedListBoxRuleSet.Items.Count; i++)
                {
                    if (i != e.Index)
                    {
                        checkedListBoxRuleSet.SetItemChecked(i, false);
                    }
                }
            }
        }

        //应用新的风控规则
        //1.将原有account的所有规则清除
        //2.给现在的规则绑定Account然后将它加入到对应的Account中去
        //3.用锁保护对Account的操作。这样就可以达到实时生效我们的设定
        private void btnOk_Click(object sender, EventArgs e)
        {
            lock (_a)
            {
                _a.clearOrderCheck();
                foreach (object item in checkedListBoxAccountRule.Items)
                {
                    IRuleCheck rc = ((ValueObject<IRuleCheck>)item).Value;
                    RiskRuleTracker.addRuleIntoAccount(_a.ID, rc);
                    rc.Account = _a;
                    _a.addOrderCheck(rc);

                }
            }
            
            //_a.addOrderCheck(rc)
            /*
            IRuleCheck rc = ((ValueObject<IRuleCheck>)checkedListBoxAccountRule.SelectedItem).Value;
            MessageBox.Show(rc.RuleDescription);
            RiskRuleTracker.addRuleIntoAccount(_acc,rc);
             * */
        }

        
    }
}
