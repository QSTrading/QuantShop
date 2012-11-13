using System;
using System.Collections.Generic;
using TradeLink.API;
using System.Windows.Forms;
namespace TradeLink.Common
{
    /// <summary>
    /// A position type used to describe the position in a stock or instrument.
    /// </summary>
    public class PositionImpl : TradeLink.API.Position, IConvertible
    {
        public decimal ToDecimal(IFormatProvider provider)
        {
            return AvgPrice;
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return (double)AvgPrice;
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return (Int16)Size;
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return Size;
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return Size;
        }

        public TypeCode GetTypeCode()
        {
            return TypeCode.Object;
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return !isFlat;
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            return ToString();
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ChangeType(Size, conversionType);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        /// <summary>
        /// convert from position to decimal (price)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static implicit operator decimal(PositionImpl p)
        {
            return p.AvgPrice;
        }
        /// <summary>
        /// convert from position to integer (size)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static implicit operator int(PositionImpl p)
        {
            return p.Size;
        }
        /// <summary>
        /// convert from
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static implicit operator bool(PositionImpl p)
        {
            return !p.isFlat;
        }
    
        public PositionImpl() : this("") { }
        public PositionImpl(Position p) : this(p.Symbol, p.AvgPrice, p.Size, p.ClosedPL,p.Account) { }
        public PositionImpl(string symbol) { _sym = symbol; }
        public PositionImpl(string symbol, decimal price, int size) : this(symbol, price, size, 0,"") { }
        public PositionImpl(string symbol, decimal price, int size, decimal closedpl) : this(symbol, price, size, closedpl, "") { }
        public PositionImpl(string symbol, decimal price, int size, decimal closedpl, string account) { _sym = symbol; if (size == 0) price = 0; _price = price; _size = size; _closedpl = closedpl; _acct = account; if (!this.isValid) throw new Exception("Can't construct invalid position!"); }
        public PositionImpl(Trade t) 
        {
            if (!t.isValid) throw new Exception("Can't construct a position object from invalid trade.");
            _sym = t.symbol; _price = t.xprice; _size = t.xsize; _date = t.xdate; _time = t.xtime;  _acct = t.Account;
            if (_size>0) _size *= t.side ? 1 : -1;
        }
        string _acct = "";
        string _sym = "";
        int _size = 0;
        decimal _price = 0;
        int _date = 0;
        int _time = 0;
        decimal _closedpl = 0;
        decimal _unrealizedpl = 0;
        decimal _last = 0;
        public bool isValid
        {
            get { return (_sym!="") && (((AvgPrice == 0) && (Size == 0)) || ((AvgPrice != 0) && (Size != 0))); }
        }

        public decimal LastPrice
        {
            get { return _last; }
        }

        public decimal UnRealizedPL
        {
            
            get {
                //MessageBox.Show("last price:" + _last.ToString());
                return _size * (_last - AvgPrice); }
        }

        public void GotTick(Tick k)
        {
            //MessageBox.Show(k.symbol + "|" + symbol);
            //MessageBox.Show(isLong.ToString() + "|" + k.hasBid.ToString() + "|" + k.bid.ToString());
            //�������µļ۸���Ϣ
            if (k.symbol != symbol)
                return;
            decimal nprice=0;
            if(isLong && k.hasBid)
                nprice = k.bid;
            if(isShort && k.hasAsk)
                nprice = k.ask;
            if (nprice != 0)
                _last = nprice;
            //���������ͼ�
            if (isFlat)
            {
                _highest = decimal.MinValue;
                _lowest = decimal.MaxValue;
            }
            else
            {
                _highest = _highest >= _last ? _highest : _last;
                _lowest = _lowest <= _last ? _lowest : _last;
            }

        }
        private decimal _highest=decimal.MinValue;
        public decimal Highest { get {return _highest; }  }
        private decimal _lowest=decimal.MaxValue;
        public decimal Lowest { get { return _lowest; } }
        
        public decimal ClosedPL { get { return _closedpl; } }
        public string Symbol { get { return _sym; } }
        public string symbol { get { return _sym; } }
        public decimal Price { get { return _price; } }
        public decimal AvgPrice { get { return _price; } }
        public int Size { get { return _size; } }
        public int UnsignedSize { get { return Math.Abs(_size); } }
        public bool isLong { get { return _size > 0; } }
        public bool isFlat { get { return _size==0; } }
        public bool isShort { get { return _size < 0; } }
        public int FlatSize { get { return _size * -1; } }
        public string Account { get { return _acct; } }
        // returns any closed PL calculated on position basis (not per share)
        /// <summary>
        /// Adjusts the position by applying a new position.
        /// </summary>
        /// <param name="pos">The position adjustment to apply.</param>
        /// <returns></returns>
        public decimal Adjust(Position pos)
        {
            if ((_sym!="") && (this.Symbol != pos.Symbol)) throw new Exception("Failed because adjustment symbol did not match position symbol");
            if (_acct == "") _acct = pos.Account;
            if (_acct != pos.Account) throw new Exception("Failed because adjustment account did not match position account.");
            if ((_sym=="") && pos.isValid) _sym = pos.Symbol;
            if (!pos.isValid) throw new Exception("Invalid position adjustment, existing:" + this.ToString() + " adjustment:" + pos.ToString());
            if (pos.isFlat) return 0; // nothing to do
            bool oldside = isLong;
            decimal pl = Calc.ClosePL(this,pos.ToTrade());
            if (this.isFlat) this._price = pos.AvgPrice; // if we're leaving flat just copy price
            else if ((pos.isLong && this.isLong) || (!pos.isLong && !this.isLong)) // sides match, adding so adjust price
                this._price = ((this._price * this._size) + (pos.AvgPrice * pos.Size)) / (pos.Size+ this.Size);
            this._size += pos.Size; // adjust the size
            if (oldside != isLong) _price = pos.AvgPrice; // this is for when broker allows flipping sides in one trade
            if (this.isFlat) _price = 0; // if we're flat after adjusting, size price back to zero
            _closedpl += pl; // update running closed pl
            return pl;
        }
        /// <summary>
        /// Adjusts the position by applying a new trade or fill.
        /// </summary>
        /// <param name="t">The new fill you want this position to reflect.</param>
        /// <returns></returns>
        public decimal Adjust(Trade t) { return Adjust(new PositionImpl(t)); }

        public override string ToString()
        {
            return Symbol+" "+Size+"@"+AvgPrice.ToString("F2")+" ["+Account+"]";
        }
        public Trade ToTrade()
        {
            DateTime dt = (_date*_time!=0) ? Util.ToDateTime(_date, _time) : DateTime.Now;
            return (TradeLink.API.Trade)new TradeImpl(Symbol, AvgPrice, Size,dt );
        }

        public static Position Deserialize(string msg)
        {
            
            string[] r = msg.Split(',');
            string sym = r[(int)PositionField.symbol];
            decimal price = Convert.ToDecimal(r[(int)PositionField.price], System.Globalization.CultureInfo.InvariantCulture);
            decimal cpl = Convert.ToDecimal(r[(int)PositionField.closedpl], System.Globalization.CultureInfo.InvariantCulture);
            int size = Convert.ToInt32(r[(int)PositionField.size]);
            string act = r[(int)PositionField.account];
            Position p = new PositionImpl(sym,price,size,cpl,act);
            return p;
        }

        public static string Serialize(Position p)
        {
            string[] r = new string[] { p.Symbol, p.AvgPrice.ToString("F2", System.Globalization.CultureInfo.InvariantCulture), p.Size.ToString("F0", System.Globalization.CultureInfo.InvariantCulture), p.ClosedPL.ToString("F2", System.Globalization.CultureInfo.InvariantCulture), p.Account };
            return string.Join(",", r);
        }


    }
}
