using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeLink.Common;
using TradeLink.API;
using Easychart.Finance.DataProvider;
using TradingLib.Data;
using TradingLib.API;

namespace TradingLib
{
    /*
     * 定义lib中所使用的委托类型
     */
    //由消息中继调用的TLServer handlemessge事件
    public delegate long HandleTLMessageDel(MessageTypes type,string msg,string address);
    public delegate void HandleTLMessageClient(MessageTypes type,string msg);
    public delegate void AddResponseDel(Response r);
    //public delegate void AccountUpdateDel(AccountBase a);
    public delegate bool RiskCheckOrderDel(Order o,out string msg);
    public delegate void RuleAddedDel(IRuleCheck rs);
    public delegate void BasketDel(Basket b);//参数为basket的事件
    public delegate void BarRequestDel(BarRequest b);//参数为barrequest的事件
    public delegate void CTPLoginDel(string address,string brokerid,string name,string pass);
    public delegate void BarBackFilled(object sender, DataPacket dp);

    public delegate void SecurityBaseDel(SecurityBase sec);
    
    //public delegate void SymbolDel(string s);



}
