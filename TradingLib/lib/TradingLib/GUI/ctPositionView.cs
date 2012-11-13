using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using TradingLib;
using TradeLink.Common;
using TradeLink.API;
using TradingLib.API;
using TradeLink.AppKit;
using TradingLib.Core;

namespace TradingLib.GUI
{
    public partial class PositionView : UserControl
    {
        public GUISide guiSide = GUISide.client;
        //用于显示Position数据 客户端是通过TradingTracker组件来记录order position记录的,服务端我们需要自己内建一个positiontracker用于记录相关信息
        PositionTracker pt = new PositionTracker();
        string _dispdecpointformat = "N" + ((int)2).ToString();

        const string SYMBOL = "合约";
        const string DIRECTION = "方向";
        const string SIZE = "持仓";
        const string AVGPRICE = "开仓均价";
        const string UNREALIZEDPL = "浮动盈亏";
        const string REALIZEDPL = "平仓盈亏";
        const string ACCOUNT = "账户";
        DataTable gt = new DataTable();
        Dictionary<string, Dictionary<string, bool>> exitStrategyMap = new Dictionary<string, Dictionary<string, bool>>();

        private PositionCheckCentre _poscheckCentre = null;
        public PositionCheckCentre PositionCheckCentre { get { return _poscheckCentre; } set { _poscheckCentre = value; } }

        public TradingTrackerCentre TradingTrackerCentre { get; set; }
        public PositionView()
        {
            InitializeComponent();
            initPositionGrid();
        }

        public event DebugDelegate SendDebugEvent;
        public event OrderDelegate SendOrderEvent;

        void SendOrder(Order o)
        {
            if (SendOrderEvent != null)
                SendOrderEvent(o);
        }

        public void initPositionGrid()
        {
            gt.Columns.Add(SYMBOL);
            gt.Columns.Add(DIRECTION);
            gt.Columns.Add(SIZE);
            gt.Columns.Add(AVGPRICE);
            gt.Columns.Add(UNREALIZEDPL);
            gt.Columns.Add(REALIZEDPL);
            gt.Columns.Add(ACCOUNT);

            positionGrid.DataSource = gt;
            //genConextMenu();

        }

        public void genConextMenu()
        {
            /*
            ContextMenuStrip _menu = new ContextMenuStrip();
            positionGrid.ContextMenuStrip = _menu;
            positionGrid.ContextMenuStrip.Items.Add("全平", null, new EventHandler(rightFlatPosition));
            _menu.Opening += new CancelEventHandler(_menu_Opening);**/

        }

        void _menu_Opening(object sender, CancelEventArgs e)
        {
            /*
            ContextMenuStrip _menu1 = new ContextMenuStrip();
            _menu1.Items.Add("打开", null, new EventHandler(rightFlatPosition));
            ContextMenuStrip _menu = sender as ContextMenuStrip;
            _menu.Items.Add("xxx",null,
            //throw new NotImplementedException();
             * **/

        }


        //QuoteView中的debug函数，通过触发sendDebugEvent来调用对应的函数
        void debug(string msg)
        {
            if (SendDebugEvent != null)
                SendDebugEvent(msg);

        }


        //position对tick的价格进行更新 
        //tick可以对多个账户的多个position进行驱动与更新
        public void GotTick(Tick t)
        {
            //数据列中如果是该symbol则必须全比更新
            //pt.NewTxt(t);
            pt.GotTick(t);
            for (int i = 0; i < gt.Rows.Count; i++)
            {
                if (gt.Rows[i][SYMBOL].ToString() == t.symbol)
                {
                    //记录该仓位所属账户
                    string acc = gt.Rows[i][ACCOUNT].ToString();
                    //通过symbol与Account找到该position,然后利用该symbol数据对其未平仓利润进行计算
                    Position pos;
                    if (guiSide == GUISide.client)
                        pos = TradingTrackerCentre.PositionTracker[t.symbol, acc];
                    else
                        pos = pt[t.symbol, acc];
                    //if (pos.isLong && t.hasBid)
                    gt.Rows[i][UNREALIZEDPL] = pos.UnRealizedPL;
                    //if (pt[t.symbol].isShort && t.hasAsk)
                    //    gt.Rows[i][UNREALIZEDPL] = pos.UnRealizedPL;
                    if (pos.isFlat)
                        gt.Rows[i][UNREALIZEDPL] = 0;

                }
                /*
                if (pt[t.symbol].isFlat)
                {
                    //positionGrid["unrealizedPL",i].Value = 0;
                        
                }**/
            }
        }

        //public void GotPosition(Position )
        //position对Fill的更新
        //fill是针对有特定账户的,
        public void GotFill(Trade t)
        {

            debug("positionView得到一个新的成交回报");
            pt.Adjust(t);
            Position pos;
            if (guiSide == GUISide.client)
                pos = TradingTrackerCentre.PositionTracker[t.symbol, t.Account];
            else
                pos = pt[t.symbol, t.Account];

            //通过account_symbol键对找到对应的行
            int posidx = positionidx(t.Account + "_" + t.symbol);

            //如果positionGrid存在该symbol的position信息
            if ((posidx > -1) && (posidx < gt.Rows.Count))
            {
                int size = pos.Size;
                gt.Rows[posidx][DIRECTION] = size == 0 ? "空仓" : (size > 0 ? "多头" : "空头");
                gt.Rows[posidx][SIZE] = size.ToString();
                gt.Rows[posidx][AVGPRICE] = pos.AvgPrice.ToString(_dispdecpointformat);
                gt.Rows[posidx][REALIZEDPL] = pos.ClosedPL.ToString(_dispdecpointformat);
                //浮动利润由tick来驱动  
            }
            else
            {   //插入新的position列
                DataRow r = gt.Rows.Add(t.symbol);
                int i = gt.Rows.Count - 1;//得到新建的Row号
                //如果不存在,则我们将该account-symbol对插入映射列表我们的键用的是account_symbol配对
                if (!symRowMap.ContainsKey(t.Account + "_" + t.symbol))
                    symRowMap.Add(t.Account + "_" + t.symbol, i);
                int size = pos.Size;
                gt.Rows[i][DIRECTION] = size == 0 ? "空仓" : (size > 0 ? "Long" : "Short");
                gt.Rows[i][SIZE] = size.ToString();
                gt.Rows[i][AVGPRICE] = pos.AvgPrice.ToString(_dispdecpointformat);
                gt.Rows[i][REALIZEDPL] = pos.ClosedPL.ToString(_dispdecpointformat);
                gt.Rows[i][ACCOUNT] = t.Account.ToString();
            }
        }

        Dictionary<string, int> symRowMap = new Dictionary<string, int>();
        int positionidx(string acc_sym)
        {
            if (symRowMap.ContainsKey(acc_sym))
                return symRowMap[acc_sym];
            return -1;
        }

        //得到当前选择的行号
        private int CurrentRow { get { return (positionGrid.SelectedRows.Count > 0 ? positionGrid.SelectedRows[0].Index : -1); } }

        //通过行号得该行的Position
        private Position GetVisiblePosition(int row)
        {
            if ((row < 0) || (row >= positionGrid.Rows.Count)) return new PositionImpl();
            string sym = gt.Rows[row][SYMBOL].ToString();
            string acc = gt.Rows[row][ACCOUNT].ToString();
            return pt[sym, acc];
        }
        private string GetVisibleSymbol(int row)
        {
            if ((row < 0) || (row >= positionGrid.Rows.Count)) return string.Empty;
            return gt.Rows[row][SYMBOL].ToString();

        }

        //动态建立Position菜单,
        private void positionGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                int rid = e.RowIndex;
                genRightMenu(rid).Show(Control.MousePosition);
            }
        }
        //根据选择的symbol动态生成需要的菜单
        private ContextMenuStrip genRightMenu(int rid)
        {
            ContextMenuStrip _menu1 = new ContextMenuStrip();

            //1.快捷操作部分全平,反手,平所有
            _menu1.Items.Add("全平", null, new EventHandler(rightFlatPosition));
            _menu1.Items.Add("反手", null, new EventHandler(rightReversePosition));

            //如果是客户端使用控件,则我们需要生成策略菜单
            if (guiSide == GUISide.client)
            {
                //分隔一
                _menu1.Items.Add(new System.Windows.Forms.ToolStripSeparator());

                //2.每个symbol预设的仓位策略(快捷启用)
                List<Response> tm = this.PositionCheckCentre.getPositionStrategy(GetVisibleSymbol(CurrentRow));
                //debug(tm.Count.ToString());
                foreach (Response r in tm)
                {
                    string sname = this.PositionCheckCentre.getPositionStrategyTitle(r) + "(" + ((IPositionCheck)r).ToText() + ")";


                    _menu1.Items.Add(sname, r.isValid ? (System.Drawing.Image)(guiresource.response_enable) : (System.Drawing.Image)(guiresource.response_disable), new EventHandler(strategyClick));

                }

                //分隔二
                _menu1.Items.Add(new System.Windows.Forms.ToolStripSeparator());
                //3.预设的仓位操作策略
                List<Type> types = this.PositionCheckCentre.getPositionStrategyTemple();
                foreach (Type t in types)
                {

                    string sname = this.PositionCheckCentre.getPositionStrategyTitle(t);

                    _menu1.Items.Add(sname, null, new EventHandler(newPositionStratey));

                }

                //GetVisiblePosition(CurrentRow)
            }

            return _menu1;
        }
        //即时增加出场策略
        public void newPositionStratey(object sender, EventArgs e)
        {
            ToolStripMenuItem mi = sender as ToolStripMenuItem;
            string cnname = mi.ToString();
            Response r = this.PositionCheckCentre.getPositionStrategyInstanceByTitle(cnname);
            fmPositionCheckParamPrompt fm = new fmPositionCheckParamPrompt(r);
            fm.SendAddResponseEvent += new AddResponseDel(ParamPrompt_SendAddResponseEvent);
            fm.ShowDialog();
        }

        void ParamPrompt_SendAddResponseEvent(Response r)
        {
            //throw new NotImplementedException();
            r.isValid = false;
            _poscheckCentre.addResponseIntoCache(gt.Rows[CurrentRow][SYMBOL].ToString(), r,true);
        }
        //单击某个策略会打开或者关闭该策略
        public void strategyClick(object sender, EventArgs e)
        {
            string smenutitle = (sender as ToolStripMenuItem).ToString();
            //debug("StrategyEnable:"+sender.ToString());
            this.PositionCheckCentre.switchResponse(smenutitle);

        }

        //菜单全平
        private void rightFlatPosition(object sender, EventArgs e)
        {

            fmConfirm fm = new fmConfirm("确认平掉所有合约" + gt.Rows[CurrentRow][SYMBOL].ToString() + "仓位？");
            fm.SendConfimEvent += new VoidDelegate(flatposition_SendConfimEvent);
            System.Drawing.Point p = new System.Drawing.Point(MousePosition.X, MousePosition.Y);
            p.Offset(-20, -20);
            fm.SetDesktopLocation(p.X, p.Y);
            fm.ShowDialog();

        }

        void flatposition_SendConfimEvent()
        {
            int row = CurrentRow;
            string sym = gt.Rows[row][SYMBOL].ToString();
            string acc = gt.Rows[row][ACCOUNT].ToString();
            Position pos;
            if (guiSide == GUISide.client)
                pos = TradingTrackerCentre.PositionTracker[sym, acc];
            else
                pos = pt[sym, acc];
            if (pos.isFlat) return;
            SendOrder(new MarketOrderFlat(pos));
        }
        //菜单 反手
        private void rightReversePosition(object sender, EventArgs e)
        {
            fmConfirm fm = new fmConfirm("确认平掉所有合约" + gt.Rows[CurrentRow][SYMBOL].ToString() + "仓位 然后反手？");
            fm.SendConfimEvent += new VoidDelegate(reverseposition_SendConfimEvent);
            System.Drawing.Point p = new System.Drawing.Point(MousePosition.X, MousePosition.Y);
            p.Offset(-20, -20);
            fm.SetDesktopLocation(p.X, p.Y);
            fm.ShowDialog();
        }
        void reverseposition_SendConfimEvent()
        {
            int row = CurrentRow;
            string sym = gt.Rows[row][SYMBOL].ToString();
            string acc = gt.Rows[row][ACCOUNT].ToString();
            Position pos;
            if (guiSide == GUISide.client)
                pos = TradingTrackerCentre.PositionTracker[sym, acc];
            else
                pos = pt[sym, acc];
            if (pos.isFlat) return;
            int size = pos.FlatSize;
            SendOrder(new MarketOrderFlat(pos));
            SendOrder(new MarketOrder(sym, size));
        }


        //出场策略模板 Type 名字与对应的 类型

        //针对本地配置文件,生成 出场策略实例(不同的配置文件,起不同的作用,同时标记是否生效)
        //有效 无效决定了position是否需要去做一个positoinCheck
        //Symbol通过MasterSec来绑定一组出场策略,该主合约下的合约通过这个来初始化出场策略实例
    }
}
