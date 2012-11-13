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
    //单元格样式 单元格背景色,字体颜色,字体等信息
    public class CellStyle
    {
        public CellStyle(Color backcolor, Color fontcolor, Font font,Color gridColor)
        {
            BackColor = backcolor;
            FontColor = fontcolor;
            Font = font;
            LineColor = gridColor;
        }

        

        public CellStyle(CellStyle copythis)
        {
            BackColor = copythis.BackColor;
            FontColor = copythis.FontColor;
            Font = copythis.Font;
            LineColor = copythis.LineColor;

        }
        Color _lineColor;
        public Color LineColor
        {
            get { return _lineColor; }
            set
            {
                //设定线条颜色的同时 设定了LinePen
                _lineColor = value;
                _linepen = new Pen(_lineColor);
            }
        }
        Pen _linepen;
        public Pen LinePen { get { return _linepen; } }
        Color _backColor;
        public Color BackColor
        {
            get { return _backColor; }
            set
            {
                _backColor = value;
                
                _backbrush = new SolidBrush(_backColor);
            }
        }

        Brush _backbrush;
        public Brush BackBrush { get { return _backbrush; } }
        Color _FontColor;
        public Color FontColor
        {
            get { return _FontColor; }
            set
            {
                _FontColor = value;
                _fontbrush = new SolidBrush(_FontColor);
            }
        }

        Brush _fontbrush;
        public Brush FontBrush { get { return _fontbrush; } }
        Font _Font;
        public Font Font { get { return _Font; } set { _Font = value; } }

        //public int CellHight { get { return _} }

    }
    //报表样式
    public class QuoteStyle
    {
        public QuoteStyle(Color quoteback1,Color quoteback2,Font quotefont,Color linecolor, Color upcolor,Color dncolor,int headheight, int rowheight)
        {
            _quoteBackColor1 = quoteback1;
            _quoteBackColor2 = quoteback2;
            LineColor = linecolor;
            _headerheight = headheight;
            _rowheight = rowheight;
            _quoteFont = quotefont;
            _upcolor = upcolor;
            _dncolor = dncolor;

        }

        Color _upcolor;
        public Color UPColor { get { return _upcolor; } set { _upcolor = value; } }
        Color _dncolor;
        public Color DNColor { get { return _dncolor; } set { _dncolor = value; } }
        Font _quoteFont;
        public Font QuoteFont { get { return _quoteFont; } set { _quoteFont = value; } }
        Color _FontColor;
        public Color FontColor{get{return _FontColor;} set{_FontColor = value;}}


        Color _quoteBackColor1;
        public Color QuoteBackColor1 { get { return _quoteBackColor1; } set { _quoteBackColor1 = value; } }
        Color _quoteBackColor2;
        public Color QuoteBackColor2 { get { return _quoteBackColor2; } set { _quoteBackColor2 = value; } }

        Color _lineColor;
        public Color LineColor
        {
            get { return _lineColor; }
            set
            {
                //设定线条颜色的同时 设定了LinePen
                _lineColor = value;
                _linepen = new Pen(_lineColor);
            }
        }
        Pen _linepen;
        public Pen LinePen { get { return _linepen; } }

        int _rowheight;
        public int RowHeight { get { return _rowheight; } set { _rowheight = value; } }

        int _headerheight;
        public int HeaderHeight { get { return _headerheight; } set { _headerheight = value; } }

    }
}
