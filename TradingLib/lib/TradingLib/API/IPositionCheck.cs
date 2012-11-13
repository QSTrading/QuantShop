using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeLink.API;
using TradingLib.Data;
using TradeLink.Common;

namespace TradingLib.API
{
    public interface  IPositionCheck
    {
        string Name { get; set; }
        //记录positionCheck所跟踪的Position
        Position myPosition { get; set; }
        BarListTracker BarListTracker { get; set; }

        //标记是否生效
        //bool Enable{get;set;}
        //position的检查逻辑
        void checkPosition(out string msg);
        //配置文本化
        string ToText();
        //从文本读取配置信息,通过给positioncheck配置不同的参数实现不同的效果
        IPositionCheck FromText(string msg);
        //得到容易理解的positioncheck的中文描述
        string PositionCheckDescription();


    }
}
