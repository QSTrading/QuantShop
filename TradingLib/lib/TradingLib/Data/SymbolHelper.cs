using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeLink.Common;

namespace TradingLib.Data
{
    public class SymbolHelper
    {
        //针对不同的交易所返回月份代码 每个交易所的月份代码有所不同
        public static string genExpireCode(SecurityBase sec, int monthcode)
        {
            //DateTime dt = Util.FT2DT(fastdate);

            switch (sec.DestEx)
            { 
                case "CN_CZCE":
                    return (monthcode-1000).ToString();
                default:
                    return monthcode.ToString();

            }

        }
    }
}
