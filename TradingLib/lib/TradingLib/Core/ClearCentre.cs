using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TradeLink.API;
using TradeLink.Common;
using TradingLib.Data;
using TradingLib.Data.database;

namespace TradingLib.Core
{
    //清算中心，为服务器维护了一批交易账户,以及每个交易账户的实时Order,trades,position的相关信息。
    public class ClearCentre:GotOrderIndicator,GotCancelIndicator,GotFillIndicator,GotTickIndicator
    {
        public event DebugDelegate SendDebugEvet;
        bool _verb = true;
        public bool VerboseDebugging { get { return _verb; } set { _verb = value; } }


        //Account索引
        protected Dictionary<string, AccountBase> AcctList = new Dictionary<string, AccountBase>();
        //为每个账户映射一个OrderTracker用于跟踪该账户的Order
        protected Dictionary<string, OrderTracker> OrdBook = new Dictionary<string, OrderTracker>();
        //为每个账户映射一个PositionTracker用户维护该Account的Position
        protected Dictionary<string, PositionTracker> PosBook = new Dictionary<string, PositionTracker>();
        //为每个账户映射一个TradeList用于记录实时的成交记录
        protected Dictionary<string, List<Trade>> TradeBook = new Dictionary<string, List<Trade>>();


        public ClearCentre()
        {
            LoadAccounts();
        }

        private void debug(string msg)
        {
            if (SendDebugEvet != null)
                SendDebugEvet(msg);
        }
        private void v(string msg)
        {
            if (_verb)
                debug(msg);
        }
        //获得Account数组
        public AccountBase[] getAccounts()
        {
            return AcctList.Values.ToArray();
        }
        //加载账户信息并初始化账户对应的清算信息
        private void LoadAccounts()
        {
            mysqlDB m = new mysqlDB();
            DataSet ds = m.getAccounts();
            DataTable dt = ds.Tables["accounts"];
            for (int i = 0; i < dt.Rows.Count;i++ )
            {
                DataRow dr = dt.Rows[i];
                AccountBase a = new AccountBase(dr["account"].ToString(), Convert.ToDecimal(dr["lastequity"]));
                if(!HaveAccount(a))
                    addAccount(a);
            }   
        }
        public PositionTracker getPositionTracker(AccountBase a)
        {
            return getPositionTracker(a.ID);
        }
        public PositionTracker getPositionTracker(string AccountID)
        {
            if(HaveAccount(AccountID))
                return PosBook[AccountID];
            else
                addAccount(new AccountBase(AccountID));
                return PosBook[AccountID];
        }



        public bool HaveAccount(Account a)
        {
            return HaveAccount(a.ID);
        }
        public bool HaveAccount(string a)
        {
            if (AcctList.ContainsKey(a))
                return true;
            else
                return false;
        }

        public void addAccount(AccountBase a)
        { 
            AcctList.Add(a.ID,a);
            if(!OrdBook.ContainsKey(a.ID))
                OrdBook.Add(a.ID, new OrderTracker());
            if (!PosBook.ContainsKey(a.ID))
            {
                PositionTracker pt = new PositionTracker();
                pt.DefaultAccount = a.ID;
                PosBook.Add(a.ID, pt);
            }
            if(!TradeBook.ContainsKey(a.ID))
                TradeBook.Add(a.ID,new List<Trade>());
            //将clearcentre传递给Account方便Account通过clearcentre调用函数计算Account状态
            a.ClearnCentre = this;
            
        }


        public void GotOrder(Order o)
        {
            v("Clear Centre Got an Order");
            if (HaveAccount(o.Account))
            {
                OrdBook[o.Account].GotOrder(o);
                return;
            }
            else
            {
                addAccount(new AccountBase(o.Account));
            }
            
        }

        public void GotCancel(long oid)
        {
            v("Clear Centre Got cancle");
            foreach (string aid in AcctList.Keys)
            {
                if (AcctList[aid].Execute)
                    continue;
                if (OrdBook.ContainsKey(aid))
                {
                    OrdBook.Add(aid,new OrderTracker());
                }
                OrdBook[aid].GotCancel(oid);
            }
            
        }

        public void GotFill(Trade f)
        {
            v("Clear Centre Got Fill");
            if (HaveAccount(f.Account))
            {
                OrdBook[f.Account].GotFill(f);
                PosBook[f.Account].GotFill(f);
                TradeBook[f.Account].Add(f);
            }
            else
            {
                addAccount(new AccountBase(f.Account));
            }
            
        }
    
    
        public void GotTick(Tick k)
        {
            //v("Clear Centre Got Tick");
            //循环所有的账户,给每个账户的positiontrakceker调用GotTick用于更新所有positon的信息
            foreach(string aid in AcctList.Keys)
            {
                //if (AcctList[aid].Execute)
                //    continue;
                //v("send tick to positiontracker");
                if(!PosBook.ContainsKey(aid))
                {
                    PositionTracker pt = new PositionTracker();
                    pt.DefaultAccount = aid;
                    PosBook.Add(aid,pt);
                }
                PosBook[aid].GotTick(k);
            }
           
        }
    }
}
