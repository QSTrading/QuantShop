using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.Core;
using TradingLib.API;
using TradeLink.API;
using System.Windows.Forms;
namespace TradingLib.Data
{
    public delegate void AccountRefreshDel(AccountBase a);

    public class AccountBase:TradeLink.Common.Account
    {
        //指示账户是否已经分配清算中心资源,这样账户就可以进行交易并进行清算
        private bool _inClearCentre;
        public bool Ready { get { return _inClearCentre; } set { _inClearCentre = value; } }

        //Account对应的清算中心
        private ClearCentre _cc;
        public ClearCentre ClearnCentre { get { return _cc; } set { _cc = value; } }

        public AccountBase(string AccountID, decimal lastequity):base(AccountID)
        {
            _lastequity = lastequity;

        }
        public AccountBase(string AccountID):base(AccountID)
        {

        }
        //返回账户对应的交易socket连接
        private string _sockname;
        public string SocketName { get { return _sockname; } set { _sockname = value; } }
        //账户当前是在线还是离线状态
        private string _status;
        public string Status {get{return _status;} set{_status=value;} }
        //昨日权益
        public decimal _lastequity;
        public decimal LastEquity { get { return _lastequity; } set { _lastequity = value; } }
        //当前动态权益
        //账户当前权益 = 上期权益+账户内仓位未平仓利润 + 平仓利润
        public decimal NowEquity {
            get {
                return  LastEquity + RealizedPL + UnRealizedPL;
            }
        }
        //当前占用保证金
        public decimal Margin { get { return 0; } }
        //当前可用资金
        public decimal Cash { get { return 0; } }
        //当日平仓盈亏
        public decimal RealizedPL { get { return ClearnCentre.getPositionTracker(this).TotalClosedPL; } }
        //当日浮动盈亏
        public decimal UnRealizedPL { get {
            //MessageBox.Show("call here");
            //return ClearnCentre.getPositionTracker(this).Count.ToString();
            return ClearnCentre.getPositionTracker(this).TotalUnRealizedPL;
        } }
        //当日手续费
        public decimal Commision { get { return 0; } }
        //当日净利
        public decimal Profit
        {
            get
            {
                return  RealizedPL + UnRealizedPL + Commision; 
             } 
        }

        //返回账户IRuleCheck规则集
        private Dictionary<string,IRuleCheck> _ordchekMap = new Dictionary<string,IRuleCheck>();

        public bool checkOrder(Order o,out string msg)
        {
            //string msg;
            msg = "";
            foreach (IRuleCheck rc in _ordchekMap.Values)
            {
                if (!rc.checkOrder(o, out msg))
                    return false;
            }
            return true;
        }
        public void clearOrderCheck()
        {
            _ordchekMap.Clear();
        }
        public void addOrderCheck(IRuleCheck rc)
        {
            //rulecheck用totext来统一标识
            if(!haveOrderCheck(rc))
                _ordchekMap.Add(rc.ToText(),rc);
        }

        public bool haveOrderCheck(IRuleCheck rc)
        {
            return _ordchekMap.ContainsKey(rc.ToText());
        }
        public void delOrderCheck(IRuleCheck rc)
        {
            _ordchekMap.Remove(rc.ToText());
        }



    }
}
