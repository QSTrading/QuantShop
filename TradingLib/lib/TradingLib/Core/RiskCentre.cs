using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeLink.API;
using TradeLink.Common;
using TradingLib.Data;
using TradingLib.API;
using System.Reflection;
namespace TradingLib.Core
{
    //服务端风险控制模块,根据每个账户的设定，实时的检查Order是否符合审查要求予以确认或者决绝
    public class RiskCentre
    {
        public event DebugDelegate SendDebugEvent;
        bool _verb = true;
        public bool VerboseDebugging { get { return _verb; } set { _verb = value; } }


        //每个Account有一个rule规则列表,用于防止我们需要检测的规则
        Dictionary<string, AccountBase> accMap = new Dictionary<string,AccountBase>();
        //分控类 名称与 Type的对应,用于从XML配置文件中生成分控检测插件
        Dictionary<string, Type> dicRule = new Dictionary<string, Type>();
        
        public RiskCentre(AccountBase[] accts)
        {
            foreach (AccountBase a in accts)
            {
                loadRuleSet();
                LoadAccountRule(a);
                accMap.Add(a.ID, a);
            }
        }

        //加载风控规则
        private void loadRuleSet()
        {
            dicRule = new Dictionary<string, Type>();
            foreach (Type t in Assembly.Load("RiskRuleSet").GetTypes())
                dicRule.Add("RiskRuleSet" + "." + t.Name, t);
        }
        public void LoadAccountRule(AccountBase a)
        {
            if (a == null || a.ID == string.Empty)
                return;
            List<string> l = RiskRuleTracker.getRuleTextFromAccount(a.ID);
            for (int i = 0; i < l.Count; i++)
            {
                string[] re = l[i].Split('#');
                string rsname = re[0];
                string rstext = re[1];
                IRuleCheck rc = ((IRuleCheck)Activator.CreateInstance(dicRule[rsname])).FromText(rstext);
                rc.Account = a;
                a.addOrderCheck(rc);
                
            }
        }

        private void debug(string msg)
        {
            if (SendDebugEvent != null)
                SendDebugEvent(msg);
        }
        private void v(string msg)
        {
            if (_verb)
                debug(msg);
        }

        //加载分控规则Dll 引入该程序集后,可以利用里面的分控组件为Account建立分控策略
        public void InitRuleSetDll()
        {
            dicRule.Clear();
            foreach (Type t in Assembly.Load("RiskRuleSet").GetTypes())
            {
                //得到RuleSet的名称与描述
                string rsname = (string)t.InvokeMember("Name",
                    BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetProperty,
                    null, null, null);
                string rsdescription = (string)t.InvokeMember("Description",
                    BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetProperty,
                    null, null, null);
                //debug(rsname+"|"+rsdescription);
                dicRule.Add(t.Name, t);
                v("加载风控插件:"+rsname);
                v(t.Name);
                //if (t.GetInterface("IRuleCheck") != null)
            }
        }


        //检查某个Order是否通过
        public bool CheckOrder(Order o,out string msg)
        {
            msg = "";
            AccountBase a ;
            if(!accMap.TryGetValue(o.Account,out a))
            {   //风控中心找不到该Account则我们拒绝该Order
                msg = "风控中心无该帐号信息";
                return false;
            }
            
            bool re = a.checkOrder(o, out msg);
            v(msg);
            return re;
        }

    }


    
    


}
