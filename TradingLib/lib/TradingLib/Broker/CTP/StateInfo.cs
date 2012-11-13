using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Broker.CTP
{
    public enum EnumProgessState
    {
        OnMdConnected, OnMdDisConnected,	//行情连接/断开
        Connect, OnConnected, DisConnect, OnDisConnect, Login, OnLogin, Logout, OnLogout,	//连接/登录/注销
        QrySettleConfirmInfo, OnQrySettleConfirmInfo, QrySettleInfo, OnQrySettleInfo, SettleConfirm, OnSettleConfirm,	//结算
        QryInstrument, OnQryInstrument, QryOrder, OnQryOrder, QryTrade, OnQryTrade, QryPosition, OnQryPosition, QryAccount, OnQryAccount,	//查询
        QryParkedOrder, OnQryParkedOrder, QryParkedOrderAction, OnQryParkedOrderAction,	//预埋
        QryDepthMarketData, OnQryDepthMarketData, //深度行情
        QryPositionDetail, OnQryPositionDetail,	//查持仓明细
        Other, //其它
        OnError, //有错误响应
        OrderInsert, OnErrOrderInsert, OnRtnOrder, OnRtnTrade,	//下单
        OrderAction, OnErrOrderAction, OnOrderAction,		//撤单
        OnRtnTradingNotice, //交易信息
        OnRtnInstrumentStatus, //合约状态
        RemovePackedOrder, OnRemovePackedOrder, RemovePackedOrderAction, OnRemovePackedOrderAction, ParkedOrder, OnParkedOrder, ParkedOrderAction, OnParkedOrderAction, //预埋
        QuickClose,//快速平仓
        QuickLock,  //锁仓
        UpdateUserPassword, OnUpdateUserPassword, UpdateAccountPassword, OnUpdateAccountPassword,	//修改密码
        FutureToBank, OnFutureToBank, BankToFuture, OnBankToFuture, QryBankAccountMoney, OnQryBankAccountMoney,//银期
        QuickCover,
        OnQryInstrumentMarginRate, QryInstrumentMarginRate
    }

    public struct StateInfo
    {
        public StateInfo(EnumProgessState state, string info)
        {
            State = state;
            Info = info;
        }
        public EnumProgessState State;
        public string Info;
    }
}
