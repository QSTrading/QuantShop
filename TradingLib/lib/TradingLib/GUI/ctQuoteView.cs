using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradeLink.API;
using TradeLink.Common;
using TradeLink.AppKit;
using TradingLib.API;
using TradingLib.Core;


namespace TradingLib.GUI
{
    
    public partial class QuoteView : UserControl, ITickWatcher
    {
        public event SecurityDelegate EOpenChart;

        public event DebugDelegate SendDebugEvent;
        public event SymDelegate SendRegisterSymbols;
        public event SecurityDelegate SymbolSelectedEvent;

        private event TickDelegate spillTick;//QuoateView中小的下单窗口更新tick的event

        public event OrderDelegate SendOrderEvent;//发送Order
        public event SecurityDelegate SendOpenTimeSalesEvent;
        public event SecurityDelegate SendOpenChartEvent;



        private IBroker _broker = null;
        DataTable qt = new DataTable();
        //private Basket _defaultBasket = null;
        string _dispdecpointformat = "N" + ((int)2).ToString();
        string _sharepercontract = "100";

        //储存用于QuoteView显示订阅数据的合约集合
        Basket mb = new BasketImpl();

        const string SYMBOL = "合约";
        const string LAST = "最新";
        const string LASTSIZE = "现手";
        const string BID = "买价";
        const string BIDSIZE = "买量";
        const string ASK = "卖价";
        const string ASKSIZE = "卖量";

        const string LASTCLOSE = "昨收";
        const string OPEN = "开盘";
        const string HIGH = "最高";
        const string LOW = "最低";
        const string EXCH = "交易所";


        const string AVGPRICE = "AvgPrice";
        const string POSSIZE = "PosSize";
        const string CLOSEDPL = "ClosedPL";
        const string DATE = "Date";
        const string TIME = "Time";

        System.Threading.Timer _timer;

        //RingBuffer<cellLocation> cellLocations = new RingBuffer<cellLocation>(1000);
        //记录刚变色的单元格位置
        RingBuffer<int> cellLocations = new RingBuffer<int>(1000);
        public QuoteView()
        {
            InitializeComponent();
            QuoteGridSetup();
            //启动时间线程用于定期更改单元格颜色
            _timer = new System.Threading.Timer(changecolor, null, 800, 1500);

        }
        //QuoteView中的debug函数，通过触发sendDebugEvent来调用对应的函数
        void debug(string msg)
        {
            if (SendDebugEvent != null)
                SendDebugEvent(msg);

        }


        void QuoteGridSetup()
        {
            //qg.Columns

            qt.Columns.Add(SYMBOL);
            qt.Columns.Add(LAST);
            qt.Columns.Add(LASTSIZE);
            qt.Columns.Add(BID);
            qt.Columns.Add(BIDSIZE);
            qt.Columns.Add(ASK);
            qt.Columns.Add(ASKSIZE);
            qt.Columns.Add(LASTCLOSE);
            qt.Columns.Add(OPEN);
            qt.Columns.Add(HIGH);
            qt.Columns.Add(LOW);


            qg.RowHeadersVisible = false;
            qg.ColumnHeadersVisible = true;
            qg.Capture = true;
            qg.ContextMenuStrip = new ContextMenuStrip();
            qg.ContextMenuStrip.Items.Add("K线图", null, new EventHandler(rightchart));
            qg.ContextMenuStrip.Items.Add("下单板", null, new EventHandler(rightticket));
            qg.ContextMenuStrip.Items.Add("分时数据", null, new EventHandler

(rightTimeSales));
            qg.DataSource = qt; //设定需要显示的数据源

            qg.Dock = DockStyle.Fill;
            //qg.DoubleClick += new EventHandler(qg_DoubleClick);
            //quoteTab.KeyUp += new KeyEventHandler(qg_KeyUp);
            //this.KeyUp += new KeyEventHandler(qg_KeyUp);
            //qg.MouseUp += new MouseEventHandler(qg_MouseUp);
            //SetColumnContext();
        }

        #region 报价表格右键菜单项目
        /*
        void rightadd(object sender, EventArgs e)
        {
            debug("rightadd is called");
            string syms = TextPrompt.Prompt("Symbols to add", "Enter symbols seperated by 

commas: ");
            //addbasket(BasketImpl.FromString(syms));
        }
        
        void rightremove(object sender, EventArgs e)
        {
            
            Security sec = GetVisibleSecurity(CurrentRow);
            if (!sec.isValid)
                return;
            string sym = sec.Symbol;
            if (MessageBox.Show("Are you sure you want to remove " + sym + "?", "Confirm 

remove", MessageBoxButtons.YesNo) == 

DialogResult.Yes)
            {
                // remove symbol from basket
                mb.Remove(sym);
                //remove the row
                qt.Rows.RemoveAt(CurrentRow);
                // accept changes
                qt.AcceptChanges();
                // clear current index
                symidx.Clear();
                // reget index
                symindex();
            }
             
        }
         * */

        void rightTimeSales(object sender, EventArgs e)
        {
            Security sec = GetVisibleSecurity(CurrentRow);
            if (!sec.isValid)
                return;
            if (SendOpenTimeSalesEvent != null)
                SendOpenTimeSalesEvent(sec);

        }
        void rightchart(object sender, EventArgs e)
        {

            Security sec = GetVisibleSecurity(CurrentRow);
            if (!sec.isValid)
                return;
            if (EOpenChart != null)
                EOpenChart(sec);

        }

        //简易下单
        void rightticket(object sender, EventArgs e)
        {
            Security s = GetVisibleSecurity(CurrentRow);
            if (s.Type == SecurityType.IDX) return;
            string sym = s.Symbol;
            if ((s.FullName == string.Empty) || (sym == string.Empty))
            {
                return;
            }
            Order o = new OrderImpl(s.Symbol, 0);
            o.ex = s.DestEx;
            o.Security = s.Type;
            o.LocalSymbol = sym;
            //简易下单面板
            Ticket t = new Ticket(o);
            //考虑用什么方式去调用对应的函数
            t.SendOrder += new OrderDelegate(SendOrder);
            spillTick += new TickDelegate(t.newTick);
            //orderStatus += new OrderStatusDel(t.orderStatus);

            System.Drawing.Point p = new System.Drawing.Point(MousePosition.X,

MousePosition.Y);
            p.Offset(-315, 20);
            t.SetDesktopLocation(p.X, p.Y);
            t.Show();
        }

        void SendOrder(Order o)
        {
            if (SendOrderEvent != null)
                SendOrderEvent(o);
        }

        #endregion
        /*
         * QuoteView控件维护了一组Symbol集合,控件用于显示这些Symbol集合的价格表
         * symbdx 维护symbol代码--->与行号映射关系
         * mb   维护了QuoteView显示的Symbol集合
         
         */

        //symbol - >  行号的映射列表
        //Dictionary<string, int[]> symidx = new Dictionary<string, int[]>();
        Dictionary<string, int> symRowMap = new Dictionary<string, int>();
        //用于记录最高,最低价格
        HighTracker _ht = new HighTracker();
        LowTracker _lt = new LowTracker();


        //增加一个合约集合,通过该函数将一个basket与一个view进行绑定
        public void SetBasket(Basket b)
        {
            //debug("addbasket is called");
            foreach (Security s in b)
            {
                //debug("symbol's Full Name:" + s.FullName);
                addSecurity(s);
            }
            //subscribe();
        }
        //增加security
        public bool addSecurity(Security sec) { return addSecurity(sec, false); }
        public bool addSecurity(Security sec, bool dosubscribe)
        {
            string sym = sec.Symbol;//得到该security的symbol代号
            //1.检查是否存在该symbol,如果存在则直接返回
            if (mb.HaveSymbol(sym)) return true;
            //如果baskect中没有该symbol,我们将其加入
            mb.Add(sec);//basket.add默认有完备性检查
            //2.如果没有该symbol则进行数据项目的增加
            try
            {
                //1.在DataGrid中增加新的一行数据
                // SYM,LAST,TSIZE,BID,ASK,BSIZE,ASIZE,SIZES,OHLC(YEST),CHANGE
                DataRow r = qt.Rows.Add(sym);
                int i = qt.Rows.Count - 1;
                debug("row id:" + i.ToString());
                //2.建立symbol与行号的索引
                if (!symRowMap.ContainsKey(sym))
                    symRowMap.Add(sym, i);
            }
            catch
            {
                debug("qtTable数据行增加错误");
            }
            //重建symindex索引
            //symindex();
            //如果需要订阅数据,请求订阅数据
            if (dosubscribe)
                subscribe(sym);
            return true;
        }
        //删除一个symbol
        public bool delSecurity(Security sec)
        {
            //debug("delSecurity:"+sec.Symbol);
            string sym = sec.Symbol;//得到该security的symbol代号
            //1.检查是否存在该symbol,如果存在则直接返回
            if (!mb.HaveSymbol(sym)) return true;
            //如果baskect中没有该symbol,我们将其加入
            mb.Remove(sec);//basket.add默认有完备性检查
            int rid = GetSymbolRow(sym);
            //debug("row id is:" + rid.ToString());
            if (rid >= 0)
                qt.Rows.RemoveAt(rid);
            symindex();
            return true;

        }


        //注册订阅该symbol的数据,发送SendRegisterSymbols事件
        void subscribe(string sym)
        {
            debug("QuoteView 发送sendRegisterSymbols事件,调用主窗口的相关函数");

            if (SendRegisterSymbols != null)
            {
                SendRegisterSymbols(sym);
            }
        }

        //更新symbol->行号索引
        void symindex()
        {
            symRowMap.Clear();
            for (int i = 0; i < qt.Rows.Count; i++)
            {
                if (qt.Rows[i].RowState == DataRowState.Deleted) continue;
                //Security sec = SecurityImpl.Parse();
                string sym = qt.Rows[i][SYMBOL].ToString();
                //debug(i.ToString() + "_" + sym);
                symRowMap.Add(sym, i);
            }
        }

        //得到当前选择的行号
        private int CurrentRow
        {
            get
            {
                return (qg.SelectedRows.Count > 0 ? qg.SelectedRows

                    [0].Index : -1);
            }
        }

        //通过行号得该行的Security
        Security GetVisibleSecurity(int row)
        {
            if ((row < 0) || (row >= qg.Rows.Count)) return new SecurityImpl();
            string sym = qt.Rows[row][SYMBOL].ToString();
            Security s = mb[sym];
            return s;
        }

        //通过symbol代码返回对应的行号
        int GetSymbolRow(string sym)
        {
            if (symRowMap.ContainsKey(sym))
                return symRowMap[sym];
            return -1;
        }

        Dictionary<string, decimal> _lastpriceMap = new Dictionary<string, decimal>();

        //获得某个symbol的上次成交价并更新最近的成交价格
        private decimal getLastTradePrice(string symbol, decimal price)
        {
            decimal i = 0;
            if (!_lastpriceMap.TryGetValue(symbol, out i))
            {
                _lastpriceMap.Add(symbol, price);
            }
            _lastpriceMap[symbol] = price;
            return i;
        }
        //更新Quote数据
        public void GotTick(Tick t)
        {
            if (qg.InvokeRequired)
            {
                qg.Invoke(new TickDelegate(GotTick), new object[] { t });

            }
            else
            {
                //根据symbol获得该symbol所对应的数据行
                int r = GetSymbolRow(t.symbol);
                if ((r == -1) || (r >= qt.Rows.Count)) return;
                //更新下单板的报价
                if (spillTick != null)
                    spillTick(t);
                //跟新最高最低价
                _ht.newTick(t);
                _lt.newTick(t);
                decimal lastprice = 0;
                //更新最新成交价格
                if (t.isTrade)
                {
                    lastprice = getLastTradePrice(t.symbol, t.trade);
                    //改变最新价格列的背景色(如果价格发生了变化 上涨用一种颜色 下跌用一种颜色)
                    if (lastprice != t.trade)
                    {
                        qt.Rows[r][LAST] = formatdisp(t.trade);
                        //更新最新价格的颜色并记录位置 这样Timer线程可以将其颜色恢复
                        cellChanged(r, LAST, t.trade > lastprice ? Color.Red : Color.Green);
                        cellLocations.Write(r);
                    }
                    if (t.size > 0) // make sure TSize is reported
                        qt.Rows[r][LASTSIZE] = t.size;
                }
                //更新ask bid数据
                //if (t.isFullQuote)
                {
                    qt.Rows[r][BID] = formatdisp(t.bid);
                    qt.Rows[r][ASK] = formatdisp(t.ask);
                    qt.Rows[r][BIDSIZE] = t.bs;
                    qt.Rows[r][ASKSIZE] = t.os;

                }
                //更新颜色
                updateColor(r, t.trade);

                //else if (t.hasBid)
                //{
                //    qt.Rows[r][BID] = formatdisp(t.bid);
                //     qt.Rows[r][BIDSIZE] = t.bs;
                //     updateColor(r, t.trade);
                // }
                // else if (t.hasAsk)
                // {
                //     qt.Rows[r][ASK] = formatdisp(t.ask);
                //      
                //      qt.Rows[r][ASKSIZE] = t.os;
                //  }
                try
                {
                    qt.Rows[r][HIGH] = formatdisp(_ht[t.symbol]);
                    qt.Rows[r][LOW] = formatdisp(_lt[t.symbol]);
                }
                catch { }

            }
        }
        //将闪烁过的单元格颜色改变成原来的颜色
        private void changecolor(object obj)
        {
            try
            {
                while (cellLocations.hasItems)
                {
                    int r = cellLocations.Read();
                    if (r % 2 == 0)
                        qg[LAST, r].Style.BackColor = Color.SlateGray;
                    else
                        qg[LAST, r].Style.BackColor = Color.LightSlateGray;
                }
            }
            catch (Exception ex)
            { debug(ex.ToString()); }
        }
        //格式化数字输出
        private string formatdisp(decimal d)
        {
            return string.Format("{0:F1}", d);
        }
        //改变某个单元格的背景颜色
        private void cellChanged(int r, string col, Color c)
        {
            qg[col, r].Style.BackColor = c;

        }
        //更新某行的相关数字单元格的数字颜色
        private void updateColor(int r, decimal price)
        {
            //debug("update color!!!!!!!!!!");
            Color fcolor = price > 2300 ? Color.DarkRed : Color.DarkGreen;
            updateASKBIDColor(r, fcolor);
            updateLastPriceColor(r, fcolor);
        }
        //改变最新价格颜色
        private void updateLastPriceColor(int r, Color c)
        {
            qg[LAST, r].Style.ForeColor = c;
        }
        //改变买卖价格颜色
        private void updateASKBIDColor(int r, Color c)
        {
            qg[ASK, r].Style.ForeColor = c;
            qg[BID, r].Style.ForeColor = c;
        }
        //选定某个合约
        private void qg_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs

e)
        {
            Security s = GetVisibleSecurity(CurrentRow);
            debug("选定合约:" + s.Symbol);
            if (SymbolSelectedEvent != null)
                SymbolSelectedEvent(s);
        }


    }

    struct cellLocation
    {
        public int rowId;
        public string column;

    }
}
