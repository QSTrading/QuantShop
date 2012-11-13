using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradeLink.API;
using TradeLink.Common;
using TradingLib.Data;
using TradingLib.Core;
using WeifenLuo.WinFormsUI.Docking;

namespace TradingLib.GUI.Server
{
    public partial class fmSrvClearCentre : DockContent,GotOrderIndicator,GotFillIndicator,GotCancelIndicator,GotTickIndicator
    {

        public event DebugDelegate SendDebugEvent;

        //定时器用于定时更新Account信息
        private System.Threading.Timer _timer = null;

        const string ACCOUNT = "账户";
        const string ADDRESS = "地址";
        const string STATUS = "状态";
        const string LASTEQUITY="昨日权益";
        const string NOWEQUITY = "当前权益";
        const string MARGIN = "保证金";
        const string CASH = "可用资金";
        const string REALIZEDPL = "平仓盈亏";
        const string UNREALIZEDPL = "浮动盈亏";
        const string COMMISSION = "手续费";
        const string PROFIT = "净利";
        bool _verb = true;
        public bool VerboseDebugging { get { return _verb; } set { _verb = value; } }


        DataTable gt = new DataTable();
        private ClearCentre _clearCentre;
        private Dictionary<string, AccountBase> accMap = new Dictionary<string, AccountBase>();
        public fmSrvClearCentre(ClearCentre c)
        {
            InitializeComponent();

            InitAccountGrid();
            _clearCentre = c;
            positionView1.guiSide = API.GUISide.server;

            foreach (AccountBase a in _clearCentre.getAccounts())
            {
                accMap.Add(a.ID, a);
                gotAccount(a);
            }
            StartRefresh();
        }

        private void InitAccount()
        {
            
        }


        private void debug(string msg)
        {
            if (SendDebugEvent != null)
                SendDebugEvent(msg);
        }
        private void v(string msg)
        {
            if (_verb)
                debug(msg);
        }

        private void StartRefresh()
        {
            _timer = new System.Threading.Timer(refreshAccount, null, 1000, 2000);
        }

        //更新账户连接信息
        public void updateAccountConnection(string acc, bool w, string sotcketname)
        {
            int rid = accountIdx(acc);
            if (rid == -1)
                return;
            else
            {
                gt.Rows[rid][STATUS] = w?"连接":"断开";                
                gt.Rows[rid][ADDRESS] = w?sotcketname:"";
                
            }
        }
        private void refreshAccount(object obj)
        {
            foreach (AccountBase a in accMap.Values)
            {
                gotAccount(a);
            }
        }
        private void gotAccount(AccountBase a)
        {
            if (InvokeRequired)
                Invoke(new AccountRefreshDel(gotAccount), new object[] { a });
            else
            {
                int r = accountIdx(a.ID);
                //datatable不存在该行，我们则增加该行
                if (r == -1)
                {
                    gt.Rows.Add(new object[] { a.ID, "", "断开", decDisp(a.LastEquity), decDisp(a.NowEquity), decDisp(a.Cash), decDisp(a.Margin), decDisp(a.RealizedPL), decDisp(a.UnRealizedPL), decDisp(a.Commision), decDisp(a.Profit) });
                    if(!HaveAccount(a))
                        accMap.Add(a.ID, a);
                }
                else
                {
                    decimal realized = a.RealizedPL;
                    decimal unrealized = a.UnRealizedPL;
                    decimal commission = a.Commision;
                    decimal lastequity = a.LastEquity;
                    decimal profit = commission + realized + unrealized;
                    decimal nowequity = profit + lastequity;
                    decimal margin = a.Margin;
                    //更新数据
                    gt.Rows[r][LASTEQUITY] = decDisp(lastequity);
                    gt.Rows[r][NOWEQUITY] = decDisp(nowequity);
                    gt.Rows[r][CASH] = decDisp(nowequity - margin);
                    gt.Rows[r][REALIZEDPL] = decDisp(realized);
                    gt.Rows[r][UNREALIZEDPL] = decDisp(unrealized);
                    gt.Rows[r][COMMISSION] = decDisp(commission);
                    gt.Rows[r][PROFIT] = decDisp(profit);
                    gt.Rows[r][MARGIN] = decDisp(margin);
                }
            }
        }

        private string decDisp(decimal d)
        {
            return d.ToString("N2");
        }

       

        //初始化Account显示空格
        private void InitAccountGrid()
        {
            gt.Columns.Add(ACCOUNT);
            gt.Columns.Add(ADDRESS);
            gt.Columns.Add(STATUS);
            gt.Columns.Add(LASTEQUITY);
            gt.Columns.Add(NOWEQUITY);
            gt.Columns.Add(MARGIN);
            gt.Columns.Add(CASH);
            gt.Columns.Add(REALIZEDPL);
            gt.Columns.Add(UNREALIZEDPL);
            gt.Columns.Add(COMMISSION);
            gt.Columns.Add(PROFIT);
            accountlist.ContextMenuStrip = new ContextMenuStrip();
            accountlist.ContextMenuStrip.Items.Add("添加账户", null, new EventHandler(addAccount));
            accountlist.ContextMenuStrip.Items.Add("编辑账户", null, new EventHandler(editAccount));
           
            accountlist.DataSource = gt;
        }

        private void editAccount(object sender, EventArgs e)
        {
            AccountBase a = GetVisibleAccount(CurrentRow);
            debug(a.ID);
            fmAccountEdit fm = new fmAccountEdit(a);
            fm.Show();
        }
        private void addAccount(object sender,EventArgs e)
        { 

            
        }


        //如果列表中存在Account则返回某个账户的rowid
        private int accountIdx(string account)
        {
            for (int i = 0; i < accountlist.Rows.Count; i++)
            {
                if (accountlist[0, i].Value.ToString() == account)
                    return i;
            }
            return -1;

        }

        //检查控件是否已经显示了某账户
        private bool HaveAccount(AccountBase a)
        {
            return HaveAccount(a.ID);
        }
        private bool HaveAccount(string id)
        {
            return (accMap.ContainsKey(id));
        }

        //得到当前选择的行号
        private int CurrentRow { get { return (accountlist.SelectedRows.Count > 0 ? accountlist.SelectedRows[0].Index : -1); } }

        //通过行号得该行的Security
        AccountBase GetVisibleAccount(int row)
        {
            if ((row < 0) || (row >= accountlist.Rows.Count)) return new AccountBase("");
            string acct = gt.Rows[row][ACCOUNT].ToString();
            v("Account:" + acct + "Selected");
            if (HaveAccount(acct))
                return accMap[acct];
            return new AccountBase("");
        }



        //数据接收接口
        public void GotTick(Tick k)
        {
            positionView1.GotTick(k);
        
        }
        public void GotOrder(Order o)
        {

            orderView1.GotOrder(o);
        }

        public void GotFill(Trade f)
        {
            orderView1.GotFill(f);
            tradeView1.GotFill(f);
            positionView1.GotFill(f);
        }

        public void GotCancel(long oid)
        {
            orderView1.GotOrderCancel(oid);
        }


      

        private void accountlist_DoubleClick(object sender, EventArgs e)
        {
            AccountBase a = GetVisibleAccount(CurrentRow);
            gotAccount(a);
        }
    }
}
