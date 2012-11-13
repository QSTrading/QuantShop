using System;
using System.Collections.Generic;
using System.Text;
namespace TradeLink.API
{
    /// <summary>
    /// For providing execution and data subscription services to tradelink clients.
    /// </summary>
    public interface TLServer
    {
        /// <summary>
        /// provider name of the server
        /// </summary>
        Providers newProviderName { get; set; }

        /// <summary>
        /// enable extended debugging
        /// </summary>
        bool VerboseDebugging { get; set; }
        /// <summary>
        /// send subscribed clients new tick
        /// </summary>
        /// <param name="tick"></param>
        void newTick(Tick tick);
        /// <summary>
        /// send clients new fill
        /// </summary>
        /// <param name="trade"></param>
        void newFill(Trade trade);
        /// <summary>
        /// number of client connected
        /// </summary>
        int NumClients { get; }
        /// <summary>
        /// send clients new order
        /// </summary>
        /// <param name="o"></param>
        void newOrder(Order o);

        /// <summary>
        /// 向Client发送Order风控拒绝信息
        /// </summary>
        /// <param name="o"></param>
        void newOrderReject(Order o, string msg);

        /// <summary>
        /// send clients new cancel ack
        /// </summary>
        /// <param name="id"></param>
        void newCancel(long id);
        /// <summary>
        /// start server
        /// </summary>
        void Start();
        /// <summary>
        /// stop server
        /// </summary>
        void Stop();
        /// <summary>
        /// notify of debug events
        /// </summary>
        event DebugDelegate SendDebugEvent;

        /// <summary>
        /// send message to a client
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="type"></param>
        /// <param name="client"></param>
        void TLSend(string msg, MessageTypes type, string client);

        /// <summary>
        /// send message to a client
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="type"></param>
        /// <param name="client"></param>
        void TLSend(string msg, MessageTypes type, int client);

        event AccountRequestDel newAcctRequest;
        event OrderDelegateStatus newSendOrderRequest;
        event LongDelegate newOrderCancelRequest;
        event PositionArrayDelegate newPosList;
        event SymbolRegisterDel newRegisterSymbols;
        event MessageArrayDelegate newFeatureRequest;
        event UnknownMessageDelegate newUnknownRequest;
        event UnknownMessageDelegateSource newUnknownRequestSource;
        event VoidDelegate newImbalanceRequest;
        //当账户连接发生变化时候我们触发该函数
        event AccountUpdateDel SendAccountUpdateEvent;
        /// <summary>
        /// gets current list of symbols for a given client
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        string ClientSymbols(string client);
        /// <summary>
        /// gets a name of given client
        /// </summary>
        /// <param name="clientnum"></param>
        /// <returns></returns>
        string ClientName(int clientnum);

        /// <summary>
        /// notify clients of a new imbalance
        /// </summary>
        /// <param name="imb"></param>
        void newImbalance(Imbalance imb);

        /// <summary>
        /// returns true if any client is subscribed to a given symbol
        /// </summary>
        /// <param name="sym"></param>
        /// <returns></returns>
        bool SymbolSubscribed(string sym);

        Basket AllClientBasket { get; }
    }



}
