using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeLink.API;
using TradingLib.Data;


namespace TradingLib.API
{
    public interface IRuleCheck
    {
        AccountBase Account { get; set; }
        //标记该规则是否生效
        bool Enable { get; set; }
        //变量名 委托时间,可用资金比率等
        string ValueName { get; }
        //验证输入值是否有效
        bool ValidValue(string value);
        //变量值
        string Value { get; set; }
        //关系 大于,小于,等于
        CompareType Compare { get; set; }
        //检查某个委托是否有效
        bool checkOrder(Order o,out string msg);
        //将规则序列化为文本内部储存
        string ToText();
        //从文本中加载参数
        IRuleCheck FromText(string rule);
        //规则的描述,人可以阅读并理解
        string RuleDescription { get; }
        
    }
}
