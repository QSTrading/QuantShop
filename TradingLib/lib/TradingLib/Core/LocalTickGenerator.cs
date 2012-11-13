using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeLink.API;
using TradeLink.Common;
namespace TradingLib.Core
{
    //用于本地发生虚拟的Tick数据(本地虚拟合约代码,为了数据和本地储存 我们需要安排一些虚拟代码)
    //IF999代表是当月连续等等
    public class LocalTickGenerator
    {
        public event TickDelegate GotTick;

        //定义了本地代码映射在当前IF1211为主力合约,IF1211->IF999形成映射
        //关键是要形成一个智能规则来进行代码转换
        //在编辑List的时候得到一个主力合约代码list,该列表中的合约均要将其转换成999进行转发
        Dictionary<string, string> _LocalSymbolMap = new Dictionary<string, string>();

        void newTick(Tick k)
        {
            string sym = k.symbol;
            string osy;
            //如果找到本地代码映射 则对外输出Tick
            if (_LocalSymbolMap.TryGetValue(sym, out osy))
            {
                //copy该tick
                TickImpl ntick = new TickImpl(osy);
                ntick.trade = k.trade;
                ntick.size = k.size;
                ntick.ask = k.ask;
                ntick.bid = k.bid;
                ntick.AskSize = k.AskSize;
                ntick.BidSize = k.BidSize;

                ntick.Open = k.Open;
                ntick.High = k.High;
                ntick.Low = k.Low;
                ntick.OpenInterest = k.OpenInterest;
                ntick.PreOpenInterest = k.PreOpenInterest;
                ntick.PreSettlement = k.PreSettlement;

                //对外触发该Tick
                if (GotTick != null)
                    GotTick(ntick);
            }

        }
    }
}
