using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using TradeLink.API;
using TradeLink.AppKit;
using TradeLink.Common;
//using System.Windows.Forms;
//using System.ComponentModel;

namespace TradingLib.GDIControl
{

    public delegate int IntRetIntDel(int idx);
    public delegate int RetIntDel();

    public partial class ViewQuoteList : System.Windows.Forms.Control
    {
        //控件事件
        //public event SecurityDelegate EOpenChart;

        public event DebugDelegate SendDebugEvent;
        //public event SymDelegate SendRegisterSymbols;
        public event SecurityDelegate SymbolSelectedEvent;

        private event TickDelegate spillTick;//QuoateView中小的下单窗口更新tick的event

        public event OrderDelegate SendOrderEvent;//发送Order
        public event SecurityDelegate SendOpenTimeSalesEvent;
        public event SecurityDelegate SendOpenChartEvent;

        RingBuffer<int> cellLocations = new RingBuffer<int>(1000);
        System.Threading.Timer _timer;

        void debug(string msg)
        {
            if (SendDebugEvent != null)
                SendDebugEvent(msg);
        }
        
        //属性获得和设置
        [DefaultValue("Aqua")]
        Color _UPColor = Color.Red;
        public Color UPColor
        {
            get
            {
                return _UPColor;
            }
            set
            {
                _UPColor= value;
                Invalidate();
            }
        }
        [DefaultValue("Aqua")]
        Color _DNColor = Color.Green;
        public Color DNColor
        {
            get
            {
                return _DNColor;
            }
            set
            {
                _DNColor = value;
                Invalidate();
            }
        }

        //调用Invalidate()可以保证设置属性之后重绘控件
        [DefaultValue("Arial, 10.5pt, style=Bold")]
        Font _headFont = new Font("Arial,style=Bold",10);
        public Font HeaderFont
        {
            get
            {
                return _headFont;
            }
            set
            {
                _headFont = value;
                Invalidate();
            }
        }
        [DefaultValue("Aqua")]
        Color _headFontColor = Color.Turquoise;
        public Color HeaderFontColor
        {
            get
            {
                return _headFontColor;
            }
            set
            {
                _headFontColor = value;
                Invalidate();
            }
        }

        [DefaultValue("Aqua")]
        Color _headBackColor = Color.Turquoise;
        public Color HeaderBackColor
        {
            get
            {
                return _headBackColor;
            }
            set
            {
                _headBackColor = value;
                Invalidate();
            }
        }

        [DefaultValue("Arial, 10.5pt, style=Bold")]
        Font _quoteFont = new Font("Aria", 10,FontStyle.Bold);
        public Font QuoteFont
        {
            get
            {
                return _quoteFont;
            }
            set
            {
                _quoteFont = value;
                _cellstyle.Font = value;
                Invalidate();
            }
        }

        [DefaultValue("Arial, 10.5pt, style=Bold")]
        Font _symbolFont = new Font("Aria", 10, FontStyle.Bold);
        public Font SymbolFont
        {
            get
            {
                return _symbolFont;
            }
            set
            {
                _symbolFont = value;
                Invalidate();
            }
        }
        [DefaultValue("Aqua")]
        Color _symbolFontColor = Color.Green;
        public Color SymbolFontColor
        {
            get
            {
                return _symbolFontColor;
            }
            set
            {
                _symbolFontColor = value;
                Invalidate();
            }
        }



        [DefaultValue("SlateGray")]
        Color _quoteBackColor1 = Color.SlateGray;
        public Color QuoteBackColor1
        {
            get
            {
                return _quoteBackColor1;
            }
            set
            {
                _quoteBackColor1 = value;
                //_cellstyle.BackColor = value;
                DefaultQuoteStyle.QuoteBackColor1 = value;
                Invalidate();
            }
        }

        [DefaultValue("LightSlateGray")]
        Color _quoteBackColor2 = Color.LightSlateGray;
        public Color QuoteBackColor2
        {
            get
            {
                return _quoteBackColor2;
            }
            set
            {
                _quoteBackColor2 = value;
                //_cellstyle.BackColor = value;
                DefaultQuoteStyle.QuoteBackColor2 = value;
                Invalidate();
            }
        }

        [DefaultValue("Silver")]
        Color _tableLineColor = Color.Silver;
        public Color TableLineColor
        {
            get
            {
                
                return _tableLineColor;
            }
            set
            {
                _tableLineColor = value;
                
                DefaultQuoteStyle.LineColor = value;
                Invalidate();
            }
        }
        //初始
        CellStyle _cellstyle;
        public CellStyle DefaultCellStyle { get { return _cellstyle; } }
        QuoteStyle _quotestyle;
        public QuoteStyle DefaultQuoteStyle { get { return _quotestyle; } }

        
        public ViewQuoteList()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            //设置单元格样式
            _cellstyle = new CellStyle(QuoteBackColor1, Color.Red, QuoteFont,TableLineColor);
            _quotestyle = new QuoteStyle(QuoteBackColor1, QuoteBackColor2, QuoteFont,TableLineColor,UPColor,DNColor,HeaderHeight, RowHeight);

            _timer = new System.Threading.Timer(changecolor, null, 800, 1500);
           
            this.MouseMove +=new MouseEventHandler(ViewQuoteList_MouseMove);
            this.MouseDown += new MouseEventHandler(ViewQuoteList_MouseDown);
            this.MouseUp += new MouseEventHandler(ViewQuoteList_MouseUp);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(ViewQuoteList_KeyUp);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(ViewQuoteList_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ViewQuoteList_KeyPress);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(ViewQuoteList_MouseDoubleClick);
            //初始化右键菜单
            initMenu();
            //计算列起点 总宽等参数
            columnWidthChanged();
        }
        //将最新成交价闪亮的单元格改回原来的颜色
        private void changecolor(object obj)
        {
            try
            {
                //debug("改变颜色");
                while (cellLocations.hasItems)
                {
                    //debug("改回原来颜色");
                    int r = cellLocations.Read();

                    if (r % 2 == 0)
                    {
                        this[r][QuoteListConst.LAST].CellStyle.BackColor = QuoteBackColor1;
                        Invalidate(this[r].Rect);
                    }
                    else
                    {
                        this[r][QuoteListConst.LAST].CellStyle.BackColor = QuoteBackColor1;
                        Invalidate(this[r].Rect);
                    }
                }
            }
            catch (Exception ex)
            { debug(ex.ToString()); }
            
        }

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

        #region 计算我们需要显示的行
        //关于绘图:如果不指定更新区域 Invalidate 会更新所有区域,这样会造成更新某个单元各 却需要更新所有的单元格 使得运行起来很不经济
        //默认模式中我们循环所有的列然后对呗该列区域与更新区域是否相交 若不相交则直接返回不进行更新,若相交则据需遍历所有列进行比较若有相交则更新该单元格。
        //这种模式是正常工作模式下动态更新价格信息所采用的方式。
        //我们需要找到效率相对最高的方式来进行工作。
        private void GDIControl_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            //绘制表头
            paintHeader(e);
            //绘制我们需要显示的数据行
            //debug("begin:" + _beginIdx.ToString() + " end:" + _endIdx.ToString());
            try
            {
                for (int i = _beginIdx; i <= _endIdx; i++)
                {
                    //可以实现行的排列,当排列后我们将_idxQuoteRowMap重新映射到新的QuoteRow队列即可
                    _idxQuoteRowMap[i].Paint(e);
                }
            }
            catch (Exception ex)
            { 
            
            }
        }
        //绘制标题行
        void paintHeader(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (e.ClipRectangle.IntersectsWith(new Rectangle(0, 0, ClientSize.Width, DefaultQuoteStyle.HeaderHeight)))
            {
                for (int i = 0; i < columns.Length; i++)
                {
                    PointF cellLocation = new PointF(getColumnStarX(i), 0);
                    RectangleF cellRect = new RectangleF(cellLocation.X, cellLocation.Y, getColumnWidth(i), DefaultQuoteStyle.HeaderHeight);
                    g.FillRectangle(new SolidBrush(HeaderBackColor), cellRect);
                    //绘制方形区域边界
                    //绘制单元格
                    g.DrawRectangle(DefaultQuoteStyle.LinePen, getColumnStarX(i), 0, getColumnWidth(i), DefaultQuoteStyle.HeaderHeight);
                    //矩形区域的定义是由左上角的坐标进行定义的,当要输出文字的时候从左上角坐标 + 本行高度度 - 实际输出文字的高度 + 文字距离下界具体
                    g.DrawString(columns[i], HeaderFont, new SolidBrush(HeaderFontColor), cellRect.X, cellRect.Y + DefaultQuoteStyle.HeaderHeight - HeaderFont.Height);//-DefaultQuoteStyle.HeaderHeightHeaderHeight);
                }
            }
        }
        int _beginIdx = 0;
        int _endIdx = 0;
        //更新我们需要显示的起点idx与终点idx 这个运算不需要每次都调用当移动光标使得显示的行改变的时候才需要进行
        void updateBeginEndIdx()
        {
            int[] ids = getRowsStartEndToShow();
            //debug("caculate:" + _beginIdx.ToString() + "|" + _endIdx.ToString());
            int oldbegin = _beginIdx;
            _beginIdx = ids[0];
            _endIdx = ids[1];
            //如果更新后的起点与终点发生了变化,我们需要重新计算每个单元格的rect
            if (oldbegin != _beginIdx)
                this.ResetAllRect();
        }
        //计算当前控件高度可以显示的行数
        int calRowsCanShow()
        {
            int i = ClientSize.Height;
            //得到可以显示的行数
            int rnum = Convert.ToInt16((i - DefaultQuoteStyle.HeaderHeight) / DefaultQuoteStyle.RowHeight);
            //debug("可以显示:"+rnum.ToString());
            return rnum;
        }
        //根据我们选择的row来去定我们需要显示哪些行
        int[] getRowsStartEndToShow()
        {   
            int rows = calRowsCanShow();
            //当选择的行小于我们可以显示的行 则我们显示可以显示的行数与总行数中最小的一个数字
            if (SelectedQuoteRow + 1 <= rows)
                return new int[] { 0, Math.Min(rows-1, Count - 1)};
            else
            {   //如果大于可以显示的行了 则我们需要移动表格,把起点与终点都同步向上移动
                //selectedrows会对行数进行变化,当移动到没有足够的行的时候它会自己跳到首行进行显示
                return new int[] { 0 + (SelectedQuoteRow - rows+1), rows + (SelectedQuoteRow - rows+1)-1 };

            }
        }
        #endregion

        //记录rowid与对应的quoteRow的映射
        //行号与idx的映射 当进行排列时候 我们就是需要动态的去更改idxQuoteRowMap
        Dictionary<int, QuoteRow> _idxQuoteRowMap = new Dictionary<int, QuoteRow>();
        //记录symbol代码与对应ID的映射
        Dictionary<string, int> _symbolIdxMap = new Dictionary<string, int>();
        //symbol到本地idx的转换
        int symbol2idx(string symbol)
        {
            int idx;
            if (_symbolIdxMap.TryGetValue(symbol, out idx))
                return idx;
            return -1;
        }
        //通过本地idx得到quoterow
        public QuoteRow this[int idx]{
                get{
                QuoteRow qr;
                if(_idxQuoteRowMap.TryGetValue(idx,out qr))
                    return qr;
                return null;
                }
        }
        //通过symbol得到quoterow
        public QuoteRow this[string symbol] { get { return this[symbol2idx(symbol)]; } }

        //记录quoteview所监听的证券列表
        Basket mb = new BasketImpl();
        //加入一个证券

        //记录改变颜色的最新价格的行号
        void bookCellLocation(int val)
        {
            cellLocations.Write(val);
        }

        //增加security
        public void addSecurity(Security sec)
        {
            string sym = sec.Symbol;//得到该security的symbol代号
            //1.检查是否存在该symbol,如果存在则直接返回
            if (mb.HaveSymbol(sym)) return;
            //如果baskect中没有该symbol,我们将其加入
            mb.Add(sec);//basket.add默认有完备性检查
            //2.如果没有该symbol则进行数据项目的增加
            try
            {
                //1.在DataGrid中增加新的一行数据
                // SYM,LAST,TSIZE,BID,ASK,BSIZE,ASIZE,SIZES,OHLC(YEST),CHANGE
                int i = _idxQuoteRowMap.Count;
                //新建一行 并插入到数据结构中
                QuoteRow qr = new QuoteRow(this,i,ref columns,getColumnStarX,getColumnWidth,getRowWidth,getBeginIndex, DefaultQuoteStyle);

                qr.SendRowLastPriceChangedEvent += new IntDelegate(bookCellLocation);
                qr.SendDebutEvent +=new DebugDelegate(debug);
                qr[QuoteListConst.SYMBOL].symbol = sym;
                _symbolIdxMap.Add(sym, i);
                _idxQuoteRowMap.Add(i, qr);
                //更新当前的序号
                _endIdx = Count - 1;
                //重新绘制窗口的某个特定部分
                Invalidate(qr.Rect);   
            }
            catch(Exception ex)
            {
                debug(ex.ToString());
            }
        }

        //报价表总行数
        public int Count { get { return _idxQuoteRowMap.Count; } }
        //得到新的Tick通过索引直接调用QuoteRow进行gottick更新单元格,然后按行进行数据更新
        public void GotTick(Tick k)
        {
            int idx = symbol2idx(k.symbol);
            if ((idx == -1) || (idx > Count)) return;
            if (spillTick != null)
                spillTick(k);
            this[idx].GotTick(k);
            //Invalidate();   
        }

        #region 表宽,表名
        //设定列的表头名称
        string[] columns = new string[] { QuoteListConst.SYMBOL, QuoteListConst.LAST, QuoteListConst.LASTSIZE, QuoteListConst.ASK, QuoteListConst.BID, QuoteListConst.ASKSIZE, QuoteListConst.BIDSIZE, QuoteListConst.VOL, QuoteListConst.CHANGE, QuoteListConst.OI, QuoteListConst.OICHANGE, QuoteListConst.SETTLEMENT, QuoteListConst.OPEN, QuoteListConst.HIGH, QuoteListConst.LOW, QuoteListConst.LASTSETTLEMENT };
        //设定每列的宽度
        int[] columnWidth = new int[] { 60,70,40,70,80,40,40,80,40,80,70,70,70,70,70,70};
        //记录每行的起点X值
        int[] columnStartX;
        
        //报价列表总宽
        int totalWidth;
        public int QuoteViewWidth { get { return totalWidth; } set { totalWidth = value; } }

        //获得某个序号列的起点
        private int getColumnStarX(int i)
        {
            return columnStartX[i];
        }
        //获得某个序号列的宽度
        private int getColumnWidth(int i)
        {
            return columnWidth[i];
        }
        //获得行总宽
        private int getRowWidth()
        {
            return totalWidth;
        }
        //开始显示的序号
        private int getBeginIndex()
        {
            return _beginIdx;
        }

        //计算行总宽
        void genColunmTotalWidth()
        {
            int w = 0;
            for (int i = 0; i < columnWidth.Length; i++)
            {
                w += columnWidth[i];
            }
            totalWidth = w;
        }
        //根据列宽重新计算列的起点
        void genColunmStartX()
        {
            columnStartX = new int[columnWidth.Length];
            for (int i = 0; i < columnWidth.Length; i++)
            {
                columnStartX[i] = 0;
                //循环计算起点
                for (int j = 0; j < i; j++)
                {
                    columnStartX[i] += columnWidth[j];
                }
            }
        }
        //用于将单元格所缓存的Rect清楚 重新计算 主要用于当列宽差生变化后进行更新,
        //平时更新数据的时候进行这些计算是没有意义 浪费CPU资源
        void ResetAllRect()
        {
            foreach (QuoteRow qr in _idxQuoteRowMap.Values)
            {
                qr.ResetRowRect();
                qr.ResetCellRect();
            }
        }
        //当列宽发生变化时候,我们需要重新计算更新列的起点 以及 总宽等数据编译QuoteRow cell进行调用
        void columnWidthChanged()
        {
            genColunmStartX();
            genColunmTotalWidth();
            ResetAllRect();
        }
        
        //标题高度
        private int HeaderHeight { get { return _headFont.Height + 4; } }
        //行高度
        private int RowHeight { get { return _symbolFont.Height + 4; } }

        #endregion



        #region 鼠标修改列宽

        bool CanChangeMoveState = true;
        bool CanMoveColumnWidth = false;
        /// 正在拖动的ColLine已拖动距离
        /// </summary>
        int CurrentYLineMoveWidth = 0;
        /// <summary>
        /// 当前正在移动的ColLine
        /// </summary>
        int CurrentMoveYLIneID = -1;
        /// <summary>
        /// 鼠标移动事件的处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void ViewQuoteList_MouseMove(object sender, MouseEventArgs e)
        {
            //int xlineID=-1;
            int ylineID;
            //if (xtable != null)
            {
                //debug("x:" + e.X.ToString() + " y:" + e.Y.ToString());
                //判断鼠标在x y的那条线
                ylineID = MouseIInYLineIdentity(e);
                //debug(ylineID.ToString());
                //如果鼠标在表格区域内
                /*
                if (!true)
                {
                    //如果可以拖动
                    if (CanChangeMoveState)
                    {
                        //鼠标变成箭头
                        this.Cursor = Cursors.Arrow;
                    }
                    else
                    {
                        if (!CanChangeMoveState && CanMoveColumnWidth && CurrentMoveYLIneID != -1)
                        {
                           // DrawChangeColWidthLine(e, ylineID);
                        }
                    }
                }
                else
                {*/
                    if (CanChangeMoveState)
                    {
                        if (ylineID != -1)
                        {
                            //记录当前列序号
                            CurrentMoveYLIneID = ylineID;
                            this.Cursor = Cursors.SizeWE;//更改鼠标
                            //CanMoveRowHight = false;
                            CanMoveColumnWidth = true;//打开移动开关 可以移动列
                        }
                        else
                        {
                            this.Cursor = Cursors.Arrow;
                            CanMoveColumnWidth = false;
                            //CanMoveRowHight = false;
                        }
                    }
                    if (!CanChangeMoveState && CanMoveColumnWidth && CurrentMoveYLIneID != -1)
                    {
                        //debug("start moving");
                        MoveChangeColWidthLine(e, ylineID);
                    }
                //}
            }
        }

        int _selectedRow=-1;
        public int SelectedQuoteRow { get { return _selectedRow; } set { _selectedRow = value; } }
        void ViewQuoteList_MouseDown(object sender, MouseEventArgs e)
        {
            this.Focus();
            
            if (e.Button == MouseButtons.Left)
            {
                if (CanMoveColumnWidth)
                {
                    CanChangeMoveState = false;
                }
                else
                {
                    //debug("click row:" + mouseX2RowID(e).ToString());
                    int i = mouseX2RowID(e);
                    SelectRow(i);
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                
                _cmenu.Show(new Point(MousePosition.X, MousePosition.Y));
            }
        }
        //选中某一行 会高亮该行
        void SelectRow(int i)
        {
            if (i < 0) return;
            if (i > (Count - 1)) i = 0;
            int old = SelectedQuoteRow;
            if (i != old)
            {
                ChangeRowBackColor(i);
                //debug("改回原来的颜色");
                ResetRowBackColor(old);
                SelectedQuoteRow = i;
            }
            
        }
        //触发选择某个symbol的事件
        void ViewQuoteList_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            
            //debug("selected :" + SelectedQuoteRow.ToString());
            Security sec = GetVisibleSecurity(SelectedQuoteRow);
            if (SymbolSelectedEvent != null)
                SymbolSelectedEvent(sec);
            debug("Symbol:" + sec.ToString() + " Selected");
        }

        //获得某个行的security
        Security GetVisibleSecurity(int row)
        {
            if ((row < 0) || (row >= Count)) return new SecurityImpl();
            string sym = this[row][QuoteListConst.SYMBOL].symbol;
            Security s = mb[sym];
            return s;
        }

        //将某行颜色重置
        void ResetRowBackColor(int rid)
        {
            if (rid >= 0)
            {
                for (int i = 0; i < columns.Length; i++)
                {
                    Color c = rid % 2 == 0 ? QuoteBackColor1 : QuoteBackColor2;
                    this[rid][i].CellStyle.BackColor = c;
                    this[rid][i].CellStyle.LineColor = TableLineColor;
                }
                Invalidate(this[rid].Rect);
            }
        }
        //更改某行颜色
        void ChangeRowBackColor(int rid)
        {
            //if (rid == SelectedQuoteRow) return;
            for (int i = 0; i < columns.Length; i++)
            {
                this[rid][i].CellStyle.BackColor = Color.Blue;
                this[rid][i].CellStyle.LineColor = Color.Blue;
            }
            Invalidate(this[rid].Rect);
        }
        void ViewQuoteList_MouseUp(object sender, MouseEventArgs e)
        {

            if (!CanChangeMoveState)
            {
                if (CanMoveColumnWidth)
                {
                    ChangeColWidth();

                }
            }
            CanChangeMoveState = true;
            //Invalidate();
            //this.Refresh();
        }

        /// <summary>
        /// 拖拽停止后改变列宽
        /// </summary>
        private void ChangeColWidth()
        {
            //debug("改变列宽");
            columnWidth[CurrentMoveYLIneID-1] = columnWidth[CurrentMoveYLIneID-1]+CurrentYLineMoveWidth;
            columnWidthChanged();
            this.Refresh();
            //Invalidate();
        }
        private bool MouseIsInTableArea(MouseEventArgs e)
        {
            //return true;
            return e.X >= 20 && e.X <= (totalWidth + 20) && e.Y >= 50 && e.Y <= (DefaultQuoteStyle.HeaderHeight + 10);
        }

        private int mouseX2RowID(MouseEventArgs e)
        {
            return Convert.ToInt16((e.Y - DefaultQuoteStyle.HeaderHeight) / DefaultQuoteStyle.RowHeight);
        }
        private int MouseIInYLineIdentity(MouseEventArgs e)
        {
            
            for(int i=0;i<columnStartX.Length;i++)
            {

                if (e.X > columnStartX[i] - 1 && e.X < columnStartX[i] + 1)
                {
                    return i;
                }
            }
            return -1;
        }
        /// <summary>
        /// 通过拖动改变列宽时显示的虚线
        /// </summary>
        /// <param name="e"></param>
        private void MoveChangeColWidthLine(MouseEventArgs e, int ylineID)
        {
            //debug("moving column");
            //输出当前鼠标坐标
            _mouseX = e.X;
            _mouseY = e.Y;
            
            CurrentYLineMoveWidth = (e.X - columnStartX[CurrentMoveYLIneID]);//计算移动值
            ChangeColWidth();//计算新的列宽
            columnWidthChanged();//重新计算绘制表格需要的列宽 列起点 总宽数据
            this.Refresh();//刷新
        }
        private int _mouseX;
        private int _mouseY;

        #endregion

        #region 右键菜单

        ContextMenuStrip _cmenu;
        private void initMenu()
        {
            _cmenu =new ContextMenuStrip();
            _cmenu.Items.Add("K线图", null, new EventHandler(rightchart));
            _cmenu.Items.Add("分时数据", null, new EventHandler(rightTimeSales));
            _cmenu.Items.Add("下单板", null, new EventHandler(rightticket));

        }

        void rightTimeSales(object sender, EventArgs e)
        {
            openTimeSales();
        }
        void openTimeSales()
        {
            Security sec = GetVisibleSecurity(SelectedQuoteRow);
            if (!sec.isValid)
                return;
            if (SendOpenTimeSalesEvent != null)
                SendOpenTimeSalesEvent(sec);
        }
        void rightchart(object sender, EventArgs e)
        {
            openChart();
        }
        void openChart()
        {
            Security sec = GetVisibleSecurity(SelectedQuoteRow);
            if (!sec.isValid)
                return;
            if (SendOpenChartEvent != null)
                SendOpenChartEvent(sec);
        }

        //简易下单
        void rightticket(object sender, EventArgs e)
        {
            openTicket();
        }
        void openTicket()
        {
            Security s = GetVisibleSecurity(SelectedQuoteRow);
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

            System.Drawing.Point p = new System.Drawing.Point(MousePosition.X, MousePosition.Y);
            p.Offset(-200,5);
            t.SetDesktopLocation(p.X, p.Y);
            t.Show();
        }
        void SendOrder(Order o)
        {
            if (SendOrderEvent != null)
                SendOrderEvent(o);
        }


        #endregion

        #region 键盘响应

        void ViewQuoteList_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            debug("key is down:"+e.KeyCode.ToString());
            if ((e.KeyCode == Keys.Q) || (e.KeyCode == Keys.Return))//Q打开K线图//回车打开k线
                openChart();
            if (e.KeyCode == Keys.W)//W打开盘口
                openTimeSales();
            if (e.KeyCode == Keys.E)//E打开小下单面板
                openTicket();
            
            //throw new NotImplementedException();
            if (e.KeyCode == Keys.Up)
                debug("upkey");
            if (e.KeyCode == Keys.Down)
                debug("dnkey");
        }

        void ViewQuoteList_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
           
        }


        void ViewQuoteList_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                SelectRow(SelectedQuoteRow - 1);
                updateBeginEndIdx();
                Invalidate();
            }
            if (e.KeyCode == Keys.Down)
            {
                SelectRow(SelectedQuoteRow + 1);
                updateBeginEndIdx();
                Invalidate();
            }
            
        }

        #endregion

    }
}