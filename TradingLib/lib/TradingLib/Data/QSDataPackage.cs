using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeLink.Common;
using TradeLink.API;
using Easychart.Finance;
namespace TradingLib.Data
{
    //DataPacket用于封装数据用于向MemoryDataManager推送数据用于ChartControl显示
    public class QSDataPacket
    {
        // Fields
        public double AdjClose;
        public double Close;
        public DateTime Date;
        public string Exchange;
        public double High;
        public double Last;
        public double Low;
        public static int MaxValue = 0x591c8;
        public double Open;
        public double OpenInterest;//持仓
        public double XSize;//成交数量
        public double XPrice;//成交价格
        public double Position;
        public double AVGPrice;
        public static int PacketByteSize = 9*4;//(PacketSize * 4);
        public static int PacketSize = 9;
        public string StockName;
        public string Symbol;
        public double TimeZone;
        public double Volume;

        //构造datapackage

        public QSDataPacket(Bar b)
            :this(b.Symbol, Util.ToDateTime(b.Bardate, b.Bartime), (double)b.Open, (double)b.High, (double)b.Low, (double)b.Close, (double)b.Volume,0,0,0)
        { 
        
        }
        public QSDataPacket(Bar b,double position,double avgprice)
            : this(b.Symbol, Util.ToDateTime(b.Bardate, b.Bartime), (double)b.Open, (double)b.High, (double)b.Low, (double)b.Close, (double)b.Volume, 0,position,avgprice)
        {

        }
        
        public QSDataPacket(string Symbol, DateTime Date, double Open, double High, double Low, double Close, double Volume,double OpenInterest,double position,double avgprice)
            : this(Symbol, Date, Open, High, Low, Close, Volume,0,OpenInterest,0,0,position,avgprice)
        {
        }

        public QSDataPacket(string Symbol, DateTime Date, double Open, double High, double Low, double Close, double Volume, double AdjClose, double OpenInterest, double xSize, double xPrice,double position,double avgprice)
        {
            this.Symbol = Symbol;
            this.Date = Date;
            this.Close = Close;
            if (AdjClose == 0.0)
            {
                this.AdjClose = Close;
            }
            else
            {
                this.AdjClose = AdjClose;
            }
            if (Open == 0.0)
            {
                this.Open = Close;
            }
            else
            {
                this.Open = Open;
            }
            if (High == 0.0)
            {
                this.High = Close;
            }
            else
            {
                this.High = High;
            }
            if (Low == 0.0)
            {
                this.Low = Close;
            }
            else
            {
                this.Low = Low;
            }
            this.Volume = Volume;
            this.OpenInterest = Open;
            this.XSize = xSize;
            this.XPrice = xPrice;
            this.Position = position;
            this.AVGPrice = avgprice;


        }
       

        public static DateTime GetDateTime(float[] fs)
        {
            return GetDateTime(fs, 0);
        }

        public static DateTime GetDateTime(float[] fs, int i)
        {
            DateTime time;
            double[] numArray = new double[1];
            try
            {
                Buffer.BlockCopy(fs, i * PacketByteSize, numArray, 0, 8);
                time = DateTime.FromOADate(numArray[0]);
            }
            catch (Exception exception)
            {
                throw new Exception(string.Concat(new object[] { exception.Message, ";", numArray[0], ";i=", i, ";Length=", fs.Length / PacketByteSize }));
            }
            return time;
        }
        /*
        public DateTime GetExchangeTime(ExchangeIntraday ei)
        {
            if (this.TimeZone == double.MaxValue)
            {
                return this.Date;
            }
            return this.Date.AddHours(ei.TimeZone);
        }
        **/



        public bool Merge(QSDataPacket dp, DataCycle dc)
        {
            if (!dc.SameSequence(dp.Date, this.Date))
            {
                this.Open = dp.Close;
                this.High = dp.Close;
                this.Low = dp.Close;
                this.Close = dp.Close;
                this.AdjClose = dp.Close;
                this.Volume = dp.Volume;
                this.Date = dp.Date;
                return false;
            }
            this.Close = dp.Close;
            this.AdjClose = this.Close;
            this.High = Math.Max(this.High, this.Close);
            this.Low = Math.Min(this.Low, this.Close);
            this.Volume = dp.Volume;
            this.Date = dp.Date;
            return true;
        }

       

   
        /*
        public byte[] ToByte()
        {
            byte[] buffer = new byte[PacketByteSize];
            Buffer.BlockCopy(this.GetFloat(), 0, buffer, 0, buffer.Length);
            return buffer;
        }
        **/

        // Properties
        public double DoubleDate
        {
            get
            {
                return this.Date.ToOADate();
            }
        }

        public bool IsZeroValue
        {
            get
            {
                return ((((this.Open == 0.0) || (this.High == 0.0)) || (this.Low == 0.0)) || (this.Close == 0.0));
            }
        }

        public double this[string Type]
        {
            get
            {
                switch (Type.ToUpper())
                {
                    case "OPEN":
                        return this.Open;

                    case "DATE":
                        return this.DoubleDate;

                    case "HIGH":
                        return this.High;

                    case "LOW":
                        return this.Low;

                    case "CLOSE":
                        return this.Close;

                    case "VOLUME":
                        return this.Volume;

                    case "ADJCLOSE":
                        return this.AdjClose;
                    case "OI":
                        return this.OpenInterest;
                    case "XPRICE":
                        return this.XPrice;
                    case "XSIZE":
                        return this.XSize;
                    case "POS":
                        return this.Position;
                    case "AVGPRICE":
                        return this.AVGPrice;
                }
                return double.NaN;
            }
        }
    }
 

}
