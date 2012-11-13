using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeLink.API;
using TradeLink.Common;
using TradingLib.Data;
using TradingLib.API;
using TradingLib;
using System.Windows.Forms;
namespace RiskRuleSet
{
    public class RSTime:IRuleCheck
    {
        private AccountBase _acc;
        public AccountBase Account { get { return _acc; } set { _acc = value; } }

        private bool _enable=true;
        public bool Enable { get{return _enable;} set{_enable=value;}}

        public string ValueName { get { return "开仓时间"; } }
        private int _time;
        //public DateTime Time { get { return _time; } set { _time = value; } }

        //用于验证客户端的输入值是否正确
        public bool ValidValue(string value)
        {
            try
            {
                //DateTime dt = Util.ToDateTime(Util.ToTLDate(DateTime.Now), Convert.ToInt32(value));
                return true;
            }
            catch (Exception ex)
            {
                return true;
            }
            
        }

        //private object _val;
        public string Value { get { return _time.ToString(); } set { _time = Convert.ToInt32(value); } }
        
        public CompareType _comparetype;
        public CompareType Compare { get { return _comparetype; } set { _comparetype = value; } }

        //规则检查函数
        public bool checkOrder(Order o,out string msg)
        {

            msg = string.Empty;
            if (!_enable)
                return true;
            //DateTime dt = Util.ToDateTime(o.date, o.time);
            //int time = _time;
            bool ret = false;
            switch (_comparetype)
            {
                case CompareType.Equals:
                    ret = (o.time == _time);
                    break;
                case CompareType.Greater:
                    ret =  (o.time > _time);
                    break;
                case CompareType.GreaterEqual:
                    ret =  (o.time >= _time);
                    break;
                case CompareType.Less:
                    ret = (o.time < _time);
                    break;
                case CompareType.LessEqual:
                    ret = (o.time <= _time);
                    break;
                default:
                    break;

            }
            if (!ret)
                msg = RuleDescription + " 不满足,委托被拒绝";
            return ret;
        }
        
        public string ToText()
        {
            string s = (_enable ?1 :0).ToString()+"|RSTime:" + _comparetype.ToString() + ":" + _time.ToString();
            return s;
        }
        public string RuleDescription
        {
            get
            {
                return "开仓条件:开仓时间 " + LibUtil.GetEnumDescription(_comparetype) + " " + _time.ToString();
                //return "";
            }
        }

        //从配置文件得到对应的规则实例,用于进行检查
        public  IRuleCheck FromText(string rule)
        {
            //MessageBox.Show(rule);
            
            string[] p = rule.Split('|');
            
            string[] p2 = p[1].Split(':');
            
            Enable = p[0] == "1" ? true : false;
            //MessageBox.Show(p2[1]);
            Value = p2[2];
            //MessageBox.Show("come to here");

            Compare = (CompareType)Enum.Parse(typeof(CompareType), p2[1], true);
            return this;
        }

        public static string Name
        {
            get { return "RSTime:时间检查"; }
        }
        public static string Description
        {
            get { return "当委托时间[Compare]设定时间,该委托可用接收"; }
        }

    }
}
