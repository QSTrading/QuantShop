﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradeLink.API
{
    public interface Position
    {

        string Symbol { get; }
        string symbol { get; }
        decimal AvgPrice { get; }
        int Size { get; }
        int UnsignedSize { get; }
        bool isLong { get; }
        bool isShort { get; }
        bool isFlat { get; }
        decimal ClosedPL { get; }
        int FlatSize { get; }
        string Account { get; }
        bool isValid { get; }
        decimal Adjust(Position newPosition);
        decimal Adjust(Trade newFill);
        decimal UnRealizedPL { get; }
        decimal LastPrice { get; }
        void GotTick(Tick k);
        //position建立后的一些数据，仓位建立后出现的最高价与最低价
        decimal Highest { get; }
        decimal Lowest { get; }
        Trade ToTrade();
    }

    public class InvalidPosition : Exception {}
}
