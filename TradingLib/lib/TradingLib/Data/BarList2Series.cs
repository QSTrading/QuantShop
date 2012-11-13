using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradeLink.API;
using TradeLink.Common;

namespace TradingLib.Data
{
    //将barlist wrapper成 ISeries便于指标系统进行运算与调用
    public class BarList2Series:ISeries
    {
        //barlist 已经包含了对应的symbol barlisttracker则是对不同symbol的bar数据的封装
        private BarListImpl _barlist;
        //默认为分钟数据
        private BarInterval _defaultinterval=BarInterval.Minute;
        //默认为收盘价
        private BarDataType _datatype = BarDataType.Close;

        public BarList2Series(BarList bl)
        {
            _barlist = bl as BarListImpl;
        }

        public BarList2Series(BarList bl, BarDataType datatype)
        {
            _barlist = bl as BarListImpl;
            _datatype = datatype;
        }
        public BarList2Series(BarList bl, BarDataType datatype,BarInterval interval)
        {
            _barlist = bl as BarListImpl;
            _datatype = datatype;
            _defaultinterval = interval;
        }

        public double LookBack(int n)
        { 
            //return _barlist
            //int count = 
            return this[Count - n - 1];
        }

        public int Count
        {
            get { return _barlist.Close(_defaultinterval).Length; }
        }

        public double[] Data {
            get {

                switch (_datatype)
                {
                    case BarDataType.Close:
                        return Calc.Decimal2Double(_barlist.Close(_defaultinterval));

                    case BarDataType.High:
                        return Calc.Decimal2Double(_barlist.High(_defaultinterval));

                    case BarDataType.Low:
                        return Calc.Decimal2Double(_barlist.Low(_defaultinterval));

                    case BarDataType.Open:
                        return Calc.Decimal2Double(_barlist.Open(_defaultinterval));

                    //case BarDataType.Volumne:
                    //    return (_barlist.Vol(_defaultinterval));

                    default:
                        return new double[] {0};
                    //break;
                }
            }
        
        }
        public double this[int index]
        {
            get
            {
                switch (_datatype)
                { 
                    case BarDataType.Close:
                        return (double)_barlist[index, _defaultinterval].Close;
                        
                    case BarDataType.High:
                        return (double)_barlist[index, _defaultinterval].High;
                       
                    case BarDataType.Low:
                        return (double)_barlist[index, _defaultinterval].Low;
                        
                    case BarDataType.Open:
                        return (double)_barlist[index, _defaultinterval].Open;
                       
                    case BarDataType.Volumne:
                        return (double)_barlist[index, _defaultinterval].Volume;
                        
                    default:
                        return 0.0;
                        //break;
                }
            }
        }

        public double Last {
            get { 
                if (Count >0)
                    return this[Count - 1];
                return 0;
            }
        }

        public double First { get { 
            if (Count>0)
                return this[0];
            return 0;

        }
        }
    }
}
