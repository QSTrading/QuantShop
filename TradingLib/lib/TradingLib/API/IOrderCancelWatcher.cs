﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeLink.API;

namespace TradingLib.API
{
    public interface IOrderCancelWatcher
    {
        void GotOrderCancel(long oid);
    }
}