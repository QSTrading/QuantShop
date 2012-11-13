using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TradingLib.API
{
    public enum TimeSalesType
    {
        Window,
        ChartRight,
    }

    public enum ExecutionType
    { 
        Sim,
        Real,
    }

    public enum CompareType
    {
        [Description("大于")]
        Greater,
        [Description("大于等于")]
        GreaterEqual,
        [Description("小于")]
        Less,
        [Description("小于等于")]
        LessEqual,
        [Description("等于")]
        Equals,
    }

    public enum PositionDataDriver
    {
        [Description("Tick")]
        Tick,
        [Description("1分钟")]
        min_1,
        [Description("3分钟")]
        min_3,
        [Description("5分钟")]
        min_5,
        [Description("15分钟")]
        min_15,
        [Description("30分钟")]
        min_30,
    }

    //指标分析中的交汇方式
    public enum EnumCross
    {
        [Description("上穿")]
        Above,
        [Description("下穿")]
        Below,
        [Description("无动作")]
        None
    }

 

 


    public enum BarDataType
    {
        [Description("开盘价")]
        Open,
        [Description("最高价")]
        High,
        [Description("最低价")]
        Low,
        [Description("收盘价")]
        Close,
        [Description("成交量")]
        Volumne,
       
        
    }

    public enum GUISide
    { 
        server,
        client,
        
    }

}
