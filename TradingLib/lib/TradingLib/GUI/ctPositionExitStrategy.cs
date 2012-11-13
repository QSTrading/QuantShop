using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.Core;
using TradingLib.Data;
using TradingLib.API;
using System.Reflection;
using TradeLink.API;

namespace TradingLib.GUI
{
    public partial class ctPositionExitStrategy : UserControl
    {
        public event VoidDelegate SendApplyEvent;
        Dictionary<string, Type> poscheckTypeMap = new Dictionary<string, Type>();
        private Security _security = null;
        public Security Security {
            get { return _security; }
            set {
                _security = value;
                symbolExitStrategy.Enabled = true;
                btnAdd.Enabled = true;
                apply.Enabled = true;
                LoadSymbolPositionCheck();
                //LoadPositionCheckStrategy();
            }
        }
        private PositionCheckCentre _poscheckCentre;
        public PositionCheckCentre PositionCheckCentre
        {
            get { return _poscheckCentre; }
            set
            {
                _poscheckCentre = value;

            }
        }

        public ctPositionExitStrategy()
        {
            InitializeComponent();
            symbolExitStrategy.Enabled = false;
            btnAdd.Enabled = false;
            apply.Enabled = false;
            try
            {
                LoadPositionCheckStrategy();
            }
            catch (Exception ex)
            { }
            symbolExitStrategy.DisplayMember = "Name";
            symbolExitStrategy.ValueMember = "Value";
        }
        //往checklistbox中加载positioncheck接口的Response
        private void LoadPositionCheckStrategy()
        {
            Dictionary<string, Type> tmp = new Dictionary<string, Type>();
            foreach (Type t in StrategyHelper.GetResponseListViaType<IPositionCheck>())
            {
                
                //object[] attributes = t.GetCustomAttributes(typeof(Tablename), false);
                //debug(t.ToString());
                //得到RuleSet的名称与描述
                string rsname = (string)t.InvokeMember("Title",
                    BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetProperty,
                    null, null, null);
                string rsdescription = (string)t.InvokeMember("Description",
                    BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetProperty,
                    null, null, null);
                //debug(rsname+"|"+rsdescription);
                //dicRule.Add("RiskRuleSet" + "." + t.Name, t);
                tmp.Add(rsname, t);
                //MessageBox.Show("RiskRuleSet" + "." + t.Name);
                //if (t.GetInterface("IRuleCheck") != null)
                poscheckTypeMap.Add(t.FullName, t);
            }
            UIUtil.genCheckedListBoxPositoinCheck(ref posExitStratgy, tmp);
        }

        //通过模板增加一个positioncheck策略,弹出参数编辑对话框，设定完参数后由ToText保存到文件用于
        //下次程序从文本实例化该策略
        private void btnAdd_Click(object sender, EventArgs e)
        {

            object[] args;
            args = new object[] { };
            Type t = ((ValueObject<Type>)posExitStratgy.SelectedItem).Value;
            Response r = (Response)Activator.CreateInstance(t, args);
            r.Name = t.Name;
            r.FullName = t.FullName;

            fmPositionCheckParamPrompt fm = new fmPositionCheckParamPrompt(r);
            fm.SendAddResponseEvent += new AddResponseDel(fm_SendAddResponseEvent);
            fm.ShowDialog();

            //fmPositionCheckParamPrompt.Popup(r);

           
        }

        void fm_SendAddResponseEvent(Response r)
        {
            try
            {
                ValueObject<Response> vo = new ValueObject<Response>();
                IPositionCheck rc = r as IPositionCheck;
                string rsname = (string)rc.GetType().InvokeMember("Title",
                        BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetProperty,
                        null, null, null);
                vo.Name = rsname + "(" + rc.ToText() + ")";
                vo.Value = r;
                //更新选择列表控件
                symbolExitStrategy.Items.Add(vo);
                //启动一个response需要用positioncheckcentre中的switchresponse来启动
                r.isValid = false;
                //将response加入的系统cache中
                _poscheckCentre.addResponseIntoCache(_security.Symbol, r);
            }
            catch (Exception ex)
            { 
                
            }
        }
        //加载某个security所对应的position
        public void LoadSymbolPositionCheck()
        {
            symbolExitStrategy.Items.Clear();
            if (_security == null)
                return;
            List<string> l = PositionCheckTracker.getPositionCheckFromSymbol(_security.Symbol);
            for (int i = 0; i < l.Count; i++)
            {
                object[] args;
                args = new object[] { };
                string[] re = l[i].Split(':');
                string rname = re[0];
                string cfgtest = re[1];
                ValueObject<Response> vo = new ValueObject<Response>();
                 
                //Type t = 
                IPositionCheck rpc = ((IPositionCheck)Activator.CreateInstance(poscheckTypeMap[rname], args)).FromText(cfgtest);
                
                string rsname = (string)poscheckTypeMap[rname].InvokeMember("Title",
                    BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetProperty,
                    null, null, null);
                
                
                vo.Value = rpc as Response;
                vo.Name =  rsname+"(" + rpc.ToText() + ")";
                symbolExitStrategy.Items.Add(vo);
            }
        }

        //将策略保存到xml文档
        private void apply_Click(object sender, EventArgs e)
        {
            //List<Response> rlist = new List<Response>();
            foreach(object item in symbolExitStrategy.Items)
            {
                Response r = ((ValueObject<Response>)item).Value;
                //保存到xml文本
                PositionCheckTracker.addPositionCheckIntoSymbol(_security.Symbol, r.GetType().FullName, ((IPositionCheck)r).ToText());
                //rlist.Add(r);
            }
            
            //_poscheckCentre.updateSymbolPositionStrategy(_security.Symbol, rlist);
            if (SendApplyEvent != null)
                SendApplyEvent();
        }
        

        
        private void btnDel_Click(object sender, EventArgs e)
        {
            int i = symbolExitStrategy.SelectedIndex;
            if (i < 0)
                return;
            Response r = ((ValueObject<Response>)symbolExitStrategy.SelectedItem).Value;
            //将response从系统缓存中移除
            _poscheckCentre.delResponseFromCache(_security.Symbol, r);
            //更新列表控件
            symbolExitStrategy.Items.RemoveAt(i);
            //更新xml本地文件 
            //PositionCheckTracker.
            PositionCheckTracker.delPositionCheckFromSymbol(_security.Symbol, r.GetType().FullName, ((IPositionCheck)r).ToText());

        }

        //单选策略模板
        private void posExitStratgy_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (posExitStratgy.CheckedItems.Count > 0)
            {
                for (int i = 0; i < posExitStratgy.Items.Count; i++)
                {
                    if (i != e.Index)
                    {
                        posExitStratgy.SetItemChecked(i, false);
                    }
                }
            }
        }
        //启用或者禁用某条策略
        private void symbolExitStrategy_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int i =e.Index;
            bool c = symbolExitStrategy.GetItemChecked(i);
            ((ValueObject<Response>)symbolExitStrategy.Items[i]).Value.isValid=!c;
                    //symbolExitStrategy.SetItemChecked(i,)
    

        }
    }
}
