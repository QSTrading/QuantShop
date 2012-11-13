using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeLink.API;
using TradeLink.Common;
using TradingLib;
using TradingLib.Data;
using TradingLib.API;
using System.Data;
using System.Windows.Forms;
namespace RiskRuleSet
{
    //[Tablesname（"tb2"）]
    public class RSMargin : IRuleCheck
    {
        private AccountBase _acc;
        public AccountBase Account { get { return _acc; } set { _acc = value; } }

        //是否激活
        private bool _enable = true;
        public bool Enable { get { return _enable; } set { _enable = value; } }
        //比较值的名称
        public string ValueName { get { return "可用资金比例";} }
        private decimal _percent;//用于内部使用的值

        public string Value { get { return _percent.ToString(); } set { _percent = Convert.ToDecimal(value); } }
        //用于验证客户端的输入值是否正确
        public bool ValidValue(string value)
        {
            return true;
        }
        //比较方式
        public CompareType _comparetype;
        public CompareType Compare { get { return _comparetype; } set { _comparetype = value; } }

        //规则检查函数
        //Margin检查用于检查保证金占用
        public bool checkOrder(Order o,out string msg)
        {
            //MessageBox.Show("check time");
            msg = string.Empty;
            if (!_enable)
                return true;
            //可用资金-该Order需要占用的保证金为该Order成交后所剩余可用资金，该资金比例是我们监控的主要项目
            decimal avabilCash = _acc.Cash - 0;
            //decimal avabilCash = _acc.Cash-o.side*(该合约的最新价格)*合约乘数*保证金比例 这项项目应该有Security提供
            //主合约列表也要GotTick用于维护合约的最新成交价格,一次可用计算对应的保证金数目
            decimal per = avabilCash / _acc.NowEquity;
            bool ret = false;
            switch (_comparetype)
            {
                case CompareType.Equals:
                    ret = (per == _percent);
                    break;
                case CompareType.Greater:
                    ret =(per > _percent);
                    break;
                case CompareType.GreaterEqual:
                    ret = (per >= _percent);
                    break;
                case CompareType.Less:
                    ret= (per < _percent);
                    break;
                case CompareType.LessEqual:
                    ret = (per <= _percent);
                    break;
                default :
                    break;
            }
            if (!ret)
                msg = RuleDescription +" 不满足,委托被拒绝";
            return ret;
            
        }

        public string ToText()
        { 
            string s = (_enable ?1 :0).ToString()+"|RSMargin:"+_comparetype.ToString()+":"+_percent.ToString();
            return s;
        }
        public string RuleDescription { 
            get 
            {
                return "开仓条件:当可用资金比例 " + LibUtil.GetEnumDescription(_comparetype)+" " + _percent.ToString("N4");
            } 
        }
        //从配置文件得到对应的规则实例,用于进行检查
        //RSMargin:1:0.1
        public IRuleCheck FromText(string rule)
        {
            //MessageBox.Show("gen rs form:"+rule);
            string[] p = rule.Split('|');
            string[] p2 = p[1].Split(':');
            Enable = p[0] == "1" ? true : false;
            Value = p2[2];
            Compare = (CompareType)Enum.Parse(typeof(CompareType), p2[1], true);
            return this;
        }
        public static string Name
        {
            get { return "RSMargin:风险比例检查"; }  
        }
        public static string Description
        {
            get { return "当可用资金[Compare]设定百分比时,该委托可用接收(可用资金是当前可用资金扣除该委托成交所占用保证金)"; }
        }



    }
}
