using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeLink.API;

namespace TradingLib.API
{
    public interface  IBroker
    {

        /// <summary>
        /// 向Broker发送Order
        /// </summary>
        /// <param name="o"></param>
        void SendOrder(Order o);
        /// <summary>
        /// 向broker取消一个order
        /// </summary>
        /// <param name="oid"></param>
        void CancelOrder(long oid);
        /// <summary>
        /// 查询broker连接状况
        /// </summary>
        bool IsLive { get; }
        
    }
}
