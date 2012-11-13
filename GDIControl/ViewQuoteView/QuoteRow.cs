using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using TradeLink.API;
using TradeLink.Common;

namespace TradingLib.GDIControl
{
    public class QuoteRow
    {
        public event IntDelegate SendRowLastPriceChangedEvent;
        public event IntRetIntDel getColumnStartX;
        public event IntRetIntDel getColumnWidth;
        public event RetIntDel getRowWidth;
        public event RetIntDel getBeginIndex;
        void FlashLoaction()
        {
            if (SendRowLastPriceChangedEvent != null)
                SendRowLastPriceChangedEvent(RowID);
        }
        /*
        public QuoteRow(string symbol, decimal last, int size, decimal ask, decimal bid, int asksize, int bidsize, decimal open, decimal high, decimal low, int preoi, int oi)
        {
            _symbol = symbol;
            _last = last;
            _size = size;
            _ask = ask;
            _bid = bid;
            _asksize = asksize;
            _bidsize = bidsize;
            _open = open;
            _high = high;
            _low = low;
            _preoi = preoi;
            _oi = oi;
        }
        public QuoteRow(Tick k)
        {
            _symbol = k.symbol;
            _last = k.trade;
            _size = k.size;
            _ask = k.ask;
            _bid = k.bid;
            _asksize = k.AskSize;
            _bidsize = k.BidSize;
        }**/
        //列名称,默认单元格样式
        //行编号,列名,列宽,默认quote样式,默认cell样式
        public ViewQuoteList _quotelist;
        public ViewQuoteList QuoteTable { get { return _quotelist; }}
       
        /// <summary>
        /// 初始化一个QuoteRow
        /// </summary>
        /// <param name="quotelist">用于传递quotelist控件引用,调用invaildate</param>
        /// <param name="rid">行序号</param>
        /// <param name="columes">列名</param>
        /// <param name="columnWidths">列起点/列宽</param>
        /// <param name="defaultQuoteStyle">默认的绘图配置</param>
        /// <param name="defaultCellStyle"></param>
        /// 
        //int[] _columnStartX;
        //int[] _columnWidth;
        //int _totalWidth;
        public QuoteRow(ViewQuoteList quotelist,int rid, ref string[] columes,IntRetIntDel startfun,IntRetIntDel widthfun,RetIntDel rowwidthfun,RetIntDel beginIdxfun, QuoteStyle defaultQuoteStyle)//, CellStyle defaultCellStyle)
        {
            _quotelist = quotelist;
            RowID = rid;
            _columesname = columes;
            _defaultQuoteStyle = defaultQuoteStyle;
            //_defaultCellStyle = defaultCellStyle;
            //_columnWidth = columnWidth;
            //_columnStartX = columnStartX;
            //_totalWidth = totalwidht;
            getColumnStartX = startfun;
            getColumnWidth = widthfun;
            getRowWidth = rowwidthfun;
            getBeginIndex = beginIdxfun;

            cellRectsArray = new Rectangle[columes.Length];

            //初始化行
            InitRow();

        }
        QuoteStyle _defaultQuoteStyle;
        //CellStyle _defaultCellStyle;
        //列名
        string[] _columesname;
        private string[] ColumnNames { get { return _columesname; } }
        string _pricedispformat = "N1";
        public string PriceDispFormat { get { return _pricedispformat; } set { _pricedispformat = value; } }
        //{ SYMBOL, LAST, LASTSIZE, ASK, BID, ASKSIZE, BIDSIZE, VOL, CHANGE, OI, OICHANGE, SETTLEMENT, OPEN, HIGH, LOW, LASTSETTLEMENT };

        //模拟更新某单元格数据
        public void update(int col, decimal value)
        {
            if (value != this[col].Value)
            {
                this[col].Value = value;
                Rectangle c = getChangedCellRect(col);
                QuoteTable.Invalidate(c);
                debug("请求更新");
                debug("x:" + c.X.ToString() + " y:" + c.Y.ToString() + " w:" + c.Width.ToString() + " h:" + c.Height.ToString());
            }
        }
        //关于gottick后界面的更新
        //1.单条更新的,
        Tick _lastTick;//保存前一条Tick用于对比后进行数据显示
        Tick _nowtick;//用于更新的Tick
        public void GotTick(Tick k)
        {
            if (_lastTick == null)
            {
                _lastTick = k;
                _nowtick = k;
            }
            _lastTick = _nowtick;
            _nowtick = k;
            //debug("got a tick");
            //this[QuoteListConst.SYMBOL].symbol = k.symbol;
            //将数据更新到cell value中去
            if (k.isTrade)
            {
                if (k.trade != this[QuoteListConst.LAST].Value)
                {
                    if (RowID != QuoteTable.SelectedQuoteRow)
                    {
                        this[QuoteListConst.LAST].CellStyle.FontColor = k.trade < k.PreSettlement ? _defaultQuoteStyle.DNColor : _defaultQuoteStyle.UPColor;
                        cellChanged(QuoteListConst.LAST, k.trade > this[QuoteListConst.LAST].Value ? Color.Tomato : Color.SpringGreen);
                        FlashLoaction();
                    }
                    this[QuoteListConst.LAST].Value = k.trade;//更新单元各value
                    //Rectangle cellRectChanged = getChangedCellRect(QuoteListConst.LAST);//获得单元各更新区域
                    //QuoteTable.Invalidate(cellRectChanged);//请求quotelist更新该区域
                    //cellLocations.Write(RowID);
                }     
            }
            if (k.size != 0)
            {
                this[QuoteListConst.LASTSIZE].Value = k.size;
                //Rectangle cellRectChanged = getChangedCellRect(QuoteListConst.LASTSIZE);//获得单元各更新区域
               // QuoteTable.Invalidate(cellRectChanged);//请求quotelist更新该区域
            }

            
            //更新当前的Tick数据

            if (k.ask != this[QuoteListConst.ASK].Value)
            {
                this[QuoteListConst.ASK].Value = k.ask;
                this[QuoteListConst.ASK].CellStyle.FontColor = k.ask < k.PreSettlement ? _defaultQuoteStyle.DNColor : _defaultQuoteStyle.UPColor;
            }
            if (k.bid != this[QuoteListConst.BID].Value)
            {
                this[QuoteListConst.BID].Value = k.bid;
                this[QuoteListConst.BID].CellStyle.FontColor = k.bid < k.PreSettlement ? _defaultQuoteStyle.DNColor : _defaultQuoteStyle.UPColor;
            }

            this[QuoteListConst.BIDSIZE].Value = k.bs;
            this[QuoteListConst.ASKSIZE].Value = k.os;


            this[QuoteListConst.VOL].Value = k.Vol;
            if ((k.trade - k.PreSettlement) != this[QuoteListConst.CHANGE].Value)
            {
                this[QuoteListConst.CHANGE].Value = k.trade - k.PreSettlement;
                this[QuoteListConst.CHANGE].CellStyle.FontColor = (k.trade - k.PreSettlement) < 0 ? _defaultQuoteStyle.DNColor : _defaultQuoteStyle.UPColor;
            }

            this[QuoteListConst.OI].Value = k.OpenInterest;

            this[QuoteListConst.OICHANGE].Value = k.OpenInterest - k.PreOpenInterest;

            this[QuoteListConst.SETTLEMENT].Value = k.Settlement;

            if (k.Open != this[QuoteListConst.OPEN].Value)
            {
                this[QuoteListConst.OPEN].Value = k.Open;
                this[QuoteListConst.OPEN].CellStyle.FontColor = k.Open < k.PreSettlement ? _defaultQuoteStyle.DNColor : _defaultQuoteStyle.UPColor;
            }
            if (k.Low != this[QuoteListConst.HIGH].Value)
            {
                this[QuoteListConst.HIGH].Value = k.High;
                this[QuoteListConst.HIGH].CellStyle.FontColor = k.High < k.PreSettlement ? _defaultQuoteStyle.DNColor : _defaultQuoteStyle.UPColor;
            }

            if (k.Low != this[QuoteListConst.LOW].Value)
            {
                this[QuoteListConst.LOW].Value = k.Low;
                this[QuoteListConst.LOW].CellStyle.FontColor = k.Low < k.PreSettlement ? _defaultQuoteStyle.DNColor : _defaultQuoteStyle.UPColor;
            }
            
            this[QuoteListConst.LASTSETTLEMENT].Value =k.PreSettlement;
            QuoteTable.Invalidate(this.Rect);
        }
        //RingBuffer<int> cellLocations = new RingBuffer<int>(1000);
        //改变某个单元格的背景颜色
        private void cellChanged(string col, Color c)
        {
                this[col].CellStyle.BackColor = c;
        }
        /*
        string _symbol;
        public string Symbol { get { return _symbol; } set { _symbol = value; } }
        decimal _last;
        public decimal Last { get { return _last; } set { _last = value; } }
        int _size;
        public int Size { get { return _size; } set { _size = value; } }
        decimal _ask;
        public decimal Ask { get { return _ask; } set { _ask = value; } }
        decimal _bid;
        public decimal Bid { get { return _bid; } set { _bid = value; } }
        int _asksize;
        public int AskSize { get { return _asksize; } set { _asksize = value; } }
        int _bidsize;
        public int BidSize { get { return _bidsize; } set { _bidsize = value; } }
        decimal _open;
        public decimal Open { get { return _open; } set { _open = value; } }
        decimal _high;
        public decimal High { get { return _high; } set { _high = value; } }
        decimal _low;
        public decimal Low { get { return _low; } set { _low = value; } }
        int _preoi;
        public int PreOpenInterest { get { return _preoi; } set { _preoi = value; } }
        int _oi;
        public int OpenInterest { get { return _oi; } set { _oi = value; } }
        **/
        //序号对应的单元格
        public Dictionary<int, QuoteCell> _columeCellMap = new Dictionary<int, QuoteCell>();
        //colume名称对应的序号
        public Dictionary<string, int> _columeString2idx = new Dictionary<string, int>();
        /*
        //序号对应的单元格值
        public Dictionary<int,int> _columeCellValueMap = new Dictionary<int,int>();
         * */

        //序对应的行宽
        //public Dictionary<int, int> _columnWidth;//= new Dictionary<int, float>();
        //对应的行号
        private int _rowid;
        public int RowID { get { return _rowid; } set { _rowid = value; } }
        
        /*
        //对应的行高
        private int _rowHeight;
        public int RowHeight { get { return _rowHeight; } set { _rowHeight = value; } }
        //标题高度
        private int _headerHeight;
        public int HeaderHeight { get { return _headerHeight; } set { _headerHeight = value; } }
        */

        public event DebugDelegate SendDebutEvent;
        void debug(string msg)
        {
            if (SendDebutEvent != null)
                SendDebutEvent(msg);
        }

        int column2Idx(string column)
        {
            int idx = -1;
            if (_columeString2idx.TryGetValue(column, out idx))
                return idx;
            return idx;
        }

        //通过序号返回对应的cell
        public QuoteCell this[int index]
        {
            get { return _columeCellMap[index]; }
        }
        //通过列名返回对应的cell
        public QuoteCell this[string columnname]
        {

            get { return _columeCellMap[column2Idx(columnname)]; }
        }

        public void AddCell(int index, QuoteCell cell)
        {
            if (!_columeCellMap.ContainsKey(index))
                _columeCellMap.Add(index, cell);
            else
                _columeCellMap[index] = cell;
        }
        //初始化行
        public void InitRow()
        {
            //根据行号得到列底色基本配置
            CellStyle cellstyle = new CellStyle(RowID % 2 == 0 ? _defaultQuoteStyle.QuoteBackColor1 : _defaultQuoteStyle.QuoteBackColor2,Color.DarkRed, _defaultQuoteStyle.QuoteFont,_defaultQuoteStyle.LineColor);
            //遍历所有的行名 并初始化单元格
            for (int i = 0; i < ColumnNames.Length; i++)
            {
                QuoteCell c = new QuoteCell(ColumnNames[i], cellstyle, 0M, PriceDispFormat);
                //AddCell(i,c);
                c.SendDebutEvent += new DebugDelegate(debug);
                _columeCellMap.Add(i, c);
                _columeString2idx.Add(ColumnNames[i], i);
                //_columeCellValueMap.Add(i,0);
            }
        }
       

        Rectangle  getChangedCellRect(string column)
        {
            return getChangedCellRect(_columeString2idx[column]);
        }
        //获得该行中某个单元格的区域
        private Dictionary<int, Rectangle> cellRectsMap = new Dictionary<int, Rectangle>();
        Rectangle[] cellRectsArray;
        //通过将rect计算后放入映射列表 可以避免每次更新都进行运算,但是当列宽改变的时候我们需要将
        //映射重置
        public void ResetCellRect()
        {
            lock (cellRectsMap)
            {
                cellRectsMap.Clear();
            }
            lock (cellRectsArray)
            {
                cellRectsArray = new Rectangle[_columesname.Length];
            }
        }
        Rectangle getChangedCellRect(int colindex)
        {
            int i = colindex;
            Rectangle cellRect;
            
            if (cellRectsMap.TryGetValue(i, out cellRect))
                return cellRect;

            //if (cellRectsArray[i] != null)
            //    return cellRectsArray[i];
            Point cellLocation = new Point(getColumnStartX(i), (RowID-getBeginIndex()) * _defaultQuoteStyle.RowHeight + _defaultQuoteStyle.HeaderHeight);
            cellRect = new Rectangle(cellLocation.X, cellLocation.Y, getColumnWidth(i), _defaultQuoteStyle.RowHeight);
            cellRectsMap.Add(i, cellRect);
            //cellRectsArray[i] = cellRect;
            return cellRect;
        }
        //返回本row所在区域 每行起点就是从0-整个控件宽度
        private Rectangle _rowrect;
        private bool _rowrectsetted = false;
        public Rectangle Rect { get {
            if (_rowrectsetted)
                return _rowrect;
            Point cellLocation = new Point(0, (RowID-getBeginIndex()) * _defaultQuoteStyle.RowHeight + _defaultQuoteStyle.HeaderHeight);
            Rectangle cellRect = new Rectangle(cellLocation.X, cellLocation.Y,getRowWidth(), _defaultQuoteStyle.RowHeight);
            _rowrect = cellRect;
            return cellRect;
        } }
        public void ResetRowRect()
        {
            _rowrectsetted = false;
        }

        //行的绘制函数
        //paint过程调用的函数要尽量减少运算,这样可以降低系统资源的消耗。
        //可以将一些运算通过一次运算下次取值的方式放入映射列表。这样可以有效的降低运算CPU消耗
        public void Paint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            /*
            //单条记录更新,表明目前我们在进行接收Tick并进行更新
            if (e.ClipRectangle.Height == _defaultQuoteStyle.RowHeight)
            {
                if (_lastTick != null && _nowtick != null)
                    return;
                debug("update here");
                if (_lastTick.trade != _nowtick.trade)
                    this[QuoteListConst.LAST].updateCellValue(e,getChangedCellRect(QuoteListConst.LAST),_defaultQuoteStyle);
                if (_lastTick.size != _nowtick.size)
                    this[QuoteListConst.LASTSIZE].updateCellValue(e, getChangedCellRect(QuoteListConst.LASTSIZE), _defaultQuoteStyle);
                if (_lastTick.ask != _nowtick.ask)
                    this[QuoteListConst.ASK].updateCellValue(e, getChangedCellRect(QuoteListConst.ASK), _defaultQuoteStyle);
                if (_lastTick.bid != _nowtick.bid)
                    this[QuoteListConst.BID].updateCellValue(e, getChangedCellRect(QuoteListConst.BID), _defaultQuoteStyle);
                if (_lastTick.AskSize != _nowtick.AskSize)
                    this[QuoteListConst.ASKSIZE].updateCellValue(e, getChangedCellRect(QuoteListConst.ASKSIZE), _defaultQuoteStyle);
                if (_lastTick.BidSize != _nowtick.BidSize)
                    this[QuoteListConst.BIDSIZE].updateCellValue(e, getChangedCellRect(QuoteListConst.BIDSIZE), _defaultQuoteStyle);
                return;
            }
            **/

            //检查需要更新的矩形区域与本单元格的矩形区域是否相交,如果相交则我们进行更新
            if (e.ClipRectangle.IntersectsWith(this.Rect))
            {
                //debug("更新Row: "+RowID.ToString());
                //Rectangle c = this.Rect;
                //debug("本行区域 x:" + c.X.ToString() + " y:" + c.Y.ToString() + " w:" + c.Width.ToString() + " h:" + c.Height.ToString());
                //c = e.ClipRectangle;
                //debug("更新区域：x:" + c.X.ToString() + " y:" + c.Y.ToString() + " w:" + c.Width.ToString() + " h:" + c.Height.ToString());

                //遍历每一个单元格并绘制0:Symbol不需要更新
                for (int i = 0; i < _columesname.Length; i++)
                {
                    //数值发生变化重绘
                    
                    //debug("cells:"+_columeCellMap.Count.ToString());
                    //PointF cellLocation = new PointF(getColumnStart(i), RowID * _defaultQuoteStyle.RowHeight + _defaultQuoteStyle.HeaderHeight);
                    //RectangleF cellRect = new RectangleF(cellLocation.X, cellLocation.Y, getColumnWidth(i), _defaultQuoteStyle.RowHeight);
                    //debug("key:"+i.ToString()+_columeCellMap[i].ToString());
                    //debug(_columeCellMap[i].Value.ToString());
                    //debug("X:" + cellRect.X.ToString() + " Y:" + cellRect.Y.ToString());
                    Rectangle cellRect = getChangedCellRect(i);

                    _columeCellMap[i].Paint(e,cellRect , _defaultQuoteStyle);
                    //c.Paint(g,cellRect,null,

                }
            }


        }
    }

}
