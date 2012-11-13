using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeLink.API;

namespace TradingLib.API
{
    public interface IOrderWatcher
    {
        void GotOrder(Order o);
    }
}
