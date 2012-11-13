using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeLink.API;
using TradeLink.Common;

namespace TradingLib.API
{
    public interface  IIndicator
    {
        //void GotNewBar(string symbol, int interval);
        //指标计算周期
        //BarInterval BarInterval{get;set;}
        //
        //BarListTracker BarListTracker { get; set; }
        //void GotTick(Tick k);
        void Caculate();
        void Reset();
    }
}
