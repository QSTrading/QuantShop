using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradeLink.API;

namespace TradingLib.TimeAndSales
{
    public partial class TimeAndSales : Control
    {
        public TimeAndSales()
        {
            DoubleBuffered = true;
            InitializeComponent();
            columnWidthChanged();
        }

        //调用Invalidate()可以保证设置属性之后重绘控件
        [DefaultValue("Aqua")]
        Color _backColor = Color.Black;
        public Color TableBackColor
        {
            get
            {
                return _backColor;
            }
            set
            {
                _backColor = value;
                Invalidate();
            }
        }
        [DefaultValue("Arial, 10.5pt, style=Bold")]
        Font _headFont = new Font("Arial,style=Bold", 10);
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
        Color _headFontColor = Color.Yellow;
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

        [DefaultValue("Arial, 10.5pt, style=Bold")]
        Font _tickFont = new Font("Aria", 10, FontStyle.Bold);
        public Font TickFont
        {
            get
            {
                return _tickFont;
            }
            set
            {
                _tickFont = value;
                Invalidate();
            }
        }
        [DefaultValue("Aqua")]
        Color _tickFontColor = Color.Green;
        public Color TickFontColor
        {
            get
            {
                return _tickFontColor;
            }
            set
            {
                _tickFontColor = value;
                Invalidate();
            }
        }

        void TimeAndSales_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            //绘制表头
            paintHeader(e);
            int rows = calRowsCanShow();
            if (rows > _tslist.Count)
            {
                int start = 0;
                for (int i = start; i < _tslist.Count; i++)
                {
                    paintRow(i,start, e);
                }
            }
            else
            {
                int start = _tslist.Count - rows;
                for (int i = start; i < _tslist.Count; i++)
                {
                    paintRow(i,start, e);
                }
            }
        }

        const string TIME = "时间";
        const string PRICE = "价格";
        const string SIZE = "手数";
        const string TYPE = "类型";

        string[] columns = { TIME, PRICE, SIZE, TYPE };
        int[] columnWidth = { 60,60,35, 45 };
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
        //当列宽发生变化时候,我们需要重新计算更新列的起点 以及 总宽等数据编译QuoteRow cell进行调用
        void columnWidthChanged()
        {
            genColunmStartX();
            genColunmTotalWidth();
            
        }
        //计算当前控件高度可以显示的行数
        int calRowsCanShow()
        {
            int i = ClientSize.Height;
            //得到可以显示的行数
            int rnum = Convert.ToInt16((i - HeaderHeight) / RowHeight);
            //debug("可以显示:"+rnum.ToString());
            return rnum;
        }

        //标题高度
        private int HeaderHeight { get { return _headFont.Height + 4; } }
        //行高度
        private int RowHeight { get { return _tickFont.Height + 4; } }

        void paintHeader(System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (int i = 0; i < columns.Length; i++)
            {
                PointF cellLocation = new PointF(getColumnStarX(i), 0);
                RectangleF cellRect = new RectangleF(cellLocation.X, cellLocation.Y, getColumnWidth(i),HeaderHeight);
                g.FillRectangle(new SolidBrush(TableBackColor), cellRect);
                //绘制方形区域边界
                //绘制单元格
                //g.DrawRectangle(DefaultQuoteStyle.LinePen, getColumnStarX(i), 0, getColumnWidth(i), DefaultQuoteStyle.HeaderHeight);
                //矩形区域的定义是由左上角的坐标进行定义的,当要输出文字的时候从左上角坐标 + 本行高度度 - 实际输出文字的高度 + 文字距离下界具体
                g.DrawString(columns[i], HeaderFont, new SolidBrush(HeaderFontColor), cellRect.X, cellRect.Y + HeaderHeight - HeaderFont.Height);//-DefaultQuoteStyle.HeaderHeightHeaderHeight);
            }
        }
        void paintRow(int id,int start,System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int i=0;
            //Point cellLocation = new Point(getColumnStarX(i), (id-start)*RowHeight + HeaderHeight);
            //Rectangle cellRect = new Rectangle(cellLocation.X, cellLocation.Y, getColumnWidth(i), RowHeight);
            int cellY = (id - start) * RowHeight + HeaderHeight;
            int ylocation = cellY + RowHeight - TickFont.Height;
            //g.FillRectangle(new SolidBrush(TableBackColor), cellRect);
           //绘制方形区域边界
                //绘制单元格
                //g.DrawRectangle(DefaultQuoteStyle.LinePen, getColumnStarX(i), 0, getColumnWidth(i), DefaultQuoteStyle.HeaderHeight);
                //矩形区域的定义是由左上角的坐标进行定义的,当要输出文字的时候从左上角坐标 + 本行高度度 - 实际输出文字的高度 + 文字距离下界具体
            g.DrawString(_tslist[id][columns[i]], TickFont, TimeBrush, getColumnStarX(i), ylocation);//-DefaultQuoteStyle.HeaderHeightHeaderHeight);
            i=1;
            //cellLocation = new PointF(getColumnStarX(i), (id-start)*RowHeight + HeaderHeight);
            //cellRect = new Rectangle(getColumnStarX(i), cellLocation.Y, getColumnWidth(i), RowHeight);
            //g.FillRectangle(new SolidBrush(TableBackColor), cellRect);
            g.DrawString(_tslist[id][columns[i]], TickFont, getTypeBrush(_tslist[id].tsType), getColumnStarX(i), ylocation);//-DefaultQuoteStyle.HeaderHeightHeaderHeight);

            i = 2;
            //cellLocation = new PointF(getColumnStarX(i), (id - start) * RowHeight + HeaderHeight);
            //cellRect = new Rectangle(getColumnStarX(i), cellLocation.Y, getColumnWidth(i), RowHeight);
            //g.FillRectangle(new SolidBrush(TableBackColor), cellRect);
            g.DrawString(_tslist[id][columns[i]], TickFont, SizeBrush, getColumnStarX(i), ylocation);//-DefaultQuoteStyle.HeaderHeightHeaderHeight);
            i = 3;
            //cellLocation = new PointF(getColumnStarX(i), (id - start) * RowHeight + HeaderHeight);
            //cellRect = new Rectangle(getColumnStarX(i), cellLocation.Y, getColumnWidth(i), RowHeight);
            //g.FillRectangle(new SolidBrush(TableBackColor), cellRect);
            g.DrawString(_tslist[id][columns[i]], TickFont, getTypeBrush(_tslist[id].tsType), getColumnStarX(i), ylocation);//-DefaultQuoteStyle.HeaderHeightHeaderHeight);
        
        
        }
        Brush TimeBrush = new SolidBrush(Color.Silver);
        Brush SizeBrush = new SolidBrush(Color.Yellow);
        Brush LongBrush = new SolidBrush(Color.Red);
        Brush ShortBrush = new SolidBrush(Color.Green);
        Brush getTypeBrush(TSType type)
        {
            if (type == TSType.EntryLong || type == TSType.ExitShort)
                return LongBrush;
            if (type == TSType.EntryShort || type == TSType.ExitLong)
                return ShortBrush;
            return LongBrush;
        }
        List<TimeAndSalesE> _tslist = new List<TimeAndSalesE>();

        Tick _oldTick = null;
        public void GotTick(Tick k)
        {
            if (k.isTrade)
            {
                if (_oldTick == null)
                {

                    _tslist.Add(new TimeAndSalesE(k, TSType.other));
                    _oldTick = k;
                }
                else
                {
                    _tslist.Add(new TimeAndSalesE(k, getTSType(_oldTick, k)));
                    _oldTick = k;
                }
                Invalidate();
            } 
        }

        TSType getTSType(Tick old, Tick k)
        { 
            //仓位上升
            if (k.OpenInterest > old.OpenInterest)
            {
                if (k.trade == k.ask)
                    return TSType.EntryLong;//多头主动买入
                if (k.trade == k.bid)
                    return TSType.EntryShort;//空头主动卖出
            }
            //仓位下降
            if (k.OpenInterest < old.OpenInterest)
            {
                if (k.trade == k.ask)
                    return TSType.EntryLong;//空头主动买入 平仓
                if (k.trade == k.bid)
                    return TSType.EntryShort;//多头主动卖出 平仓
            }
            //仓位不变
            //if (k.OpenInterest == old.OpenInterest)
            { 
            

            }
            return TSType.other;
        }


        
    }

    internal class TimeAndSalesE
    {

        public TimeAndSalesE(Tick k, TSType type)
        {

            _time = k.time;
            _price = k.trade;
            _size = k.size;
            _type = type;

        }

        const string TIME = "时间";
        const string PRICE = "价格";
        const string SIZE = "手数";
        const string TYPE = "类型";

        public string GetEnumDescription(object e)
        {
            //获取字段信息 
            System.Reflection.FieldInfo[] ms = e.GetType().GetFields();
            Type t = e.GetType();
            foreach (System.Reflection.FieldInfo f in ms)
            {
                //判断名称是否相等 
                if (f.Name != e.ToString()) continue;
                //反射出自定义属性 
                foreach (Attribute attr in f.GetCustomAttributes(true))
                {
                    //类型转换找到一个Description，用Description作为成员名称 
                    System.ComponentModel.DescriptionAttribute dscript = attr as System.ComponentModel.DescriptionAttribute;
                    if (dscript != null)
                        return dscript.Description;
                }

            }

            //如果没有检测到合适的注释，则用默认名称 
            return e.ToString();
        }
        public string this[string col]
        {
            get
            {

                switch (col)
                { 
                    case TIME:
                        return Time.ToString();
                        
                    case SIZE:
                        return Size.ToString();
                       
                    case PRICE:
                        return Price.ToString("F1");
                      
                    case TYPE:
                        return GetEnumDescription(tsType);
                       
                    default:
                        return "";


                }
            }
        }

        int _time;
        public int Time { get { return _time; } set { _time = value; } }
        int _size;
        public int Size { get { return _size; } set { _size = value; } }
        decimal _price;
        public decimal Price { get { return _price; } set { _price = value; } }
        TSType _type;
        public TSType tsType { get { return _type; } set { _type = value; } }


    }

    enum TSType
    {
        [Description("多开")]
        EntryLong,//多开 仓位增加 成交价格是ask价格 多头主动买入  
        [Description("空开")]
        EntryShort,//空开 仓位增加 成交价格是bid价格 空头主动卖出 
        [Description("多平")]
        ExitLong,//多平 仓位下降 成交价格是bid价格  多头被动卖出 
        [Description("空平")]
        ExitShort,//空平 仓位下降 成交价格是 ask价格 空头被动买入
        other,
    }
}
