using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeLink.API;
using TradingLib.API;
using TradeLink.Common;
using TicTacTec.TA.Library; // to use TA-lib indicators
using TradingLib.Data;

namespace TradingLib.Indicator
{
    //策略体系中定义了数据驱动 由哪个个数据驱动了该策略
    public class SMA:IndicatorBase
    {

        //最新的源数据值
        private double _latestvalue;
        //指标计算的当前序号
        private int _curIdx=-1;
        //简单算术平均计算周期
        private int _period;
        //计算结果
        private DataSeries _result;
        //
        private double _sum;
        private BarDataType _datatype;

        //计算由indicator计算所生成的数据 indicator实现了ISeries接口 可以用于公式计算
        public SMA(ISeries series,int N):base(series)
        {
            _result = new DataSeries();
            _period = 1;
            _curIdx = -1;
            if (N < 1)
                N = 1;
            _period = N;
            //_curIdx = N - 1;
            
            
        }
        /*
        public SMA(BarList barlist, int N):base(barlist)
        {
            _result = new DataSeries();
            _period = 1;
            _curIdx = -1;
            if (N < 1)
                N = 1;
            _period = N;
            //_curIdx = N - 1;
        }
        public SMA(BarList barlist,BarDataType datatype, int N):base(barlist,datatype)
        {
            _result = new DataSeries();
            _period = 1;
            _curIdx = -1;
            if (N < 1)
                N = 1;
            _period = N;
            //_curIdx = N - 1;
            _datatype = datatype;
        }

        public SMA(BarList barlist, BarDataType datatype, BarInterval interval, int N)
            : base(barlist, datatype, interval)
        {
            _result = new DataSeries();
            _period = 1;
            _curIdx = -1;
            if (N < 1)
                N = 1;
            _period = N;
            //_curIdx = N - 1;
            _datatype = datatype;
        
        }
        */
        /*
        private string _defaultsymbol;
        //
        //public BarListTracker BarListTracker { get { return _blt; } 
            set { _blt = value;
                //_blt.GotNewBar +=new SymBarIntervalDelegate(bar_GotNewBar);
            } }

        private BarInterval _interval;
        public BarInterval BarInterval { get { return _interval; } set { _interval = value; } }

        
        //private BarList _bl;
        
        private Dictionary<string, List<decimal>> indicatorValues = new Dictionary<string, List<decimal>>();
        
        */

        //barlisttracker,symbol,interval + 指标计算的参数
       // public SMA(BarListTracker blt,string symbol, BarInterval interval, int daysback)
        //{
            //_bl = blt[symbol,(int)interval];
            //_period = daysback;
            //_values = Calc.Decimal2Double(_bl.Close());
            //_bl.GotNewBar += new SymBarIntervalDelegate(bar_GotNewBar);
        //}

        //public SMA(BarList bar,BarInterval interval,int daysback)
        //{
            //_bl = bar;
            //_period = daysback;
            //_values = Calc.Decimal2Double(_bl.Close());
            //_bl.GotNewBar += new SymBarIntervalDelegate(bar_GotNewBar);
        //}



        //得到新的bar时候 增加新的指标位
        /*
        public void GotNewBar(string symbol, int interval)
        {
            debug("we got a new bar");
            if (interval != (int)_interval)
                return;
            Caculation(symbol, true);

        }*/

        /*
        //得到tick价格更新时指标更新
        public void GotTick(Tick k)
        {
            if (!k.isTrade)
                return;
            Caculation(k.symbol, false);

        }
        */
       
        //计算哪个symbol的指标数据
        public override void Caculate()
        {
            
            debug("SMA do caculation....");
            debug("OrigDataSeries Count:" + OrigDataSeries.Count.ToString());
            if (_curIdx < 0)
            {
                for (int i = 0; i < OrigDataSeries.Count; i++)
                {
                    debug(OrigDataSeries[i].ToString());
                }
            }

            
            //准备指标运算所需要用到的数据
            int begin = 0;
            int length = 0;
            int start = 0;
            double[] output;
            
            //默认从0位开始计算
            if (_curIdx < 0)
                _curIdx = 0;
            //我们默认开始计算的位置为当前current位置
            start = _curIdx;
            output = new double[OrigDataSeries.Count];
            //开始计算位/结束计算位/计算数据/参数/数据开始位/总结过长度/输出数组
            TicTacTec.TA.Library.Core.RetCode retCode = TicTacTec.TA.Library.Core.Sma(start, OrigDataSeries.Count - 1,base.OrigDataSeries.Data, _period, out begin, out length, output);
            //设定当前计算数值
            try
            {
                //debug("caculation into here");
                //debug("start:" + start.ToString() + " begin:" + begin.ToString() + " lengeth:" + length.ToString());
                //debug("current:"+_curIdx.ToString());
                //debug("tmp array count:" + _result.Count.ToString());
                //1.当前index小于起始计算位 补0
                for (int i=_curIdx;i< begin;i++)
                {
                    //debug("#:"+i.ToString()+"当前序号小于开始计算序号,将前面的数值赋0");
                    //如果在tmp数组范围内我们直接赋值,如果需要在原有基础上新增加一个数值
                    if ((i + 1) < _result.Count)
                        _result[i] = 0;
                    else
                        _result.Add(0);
                    _curIdx++;
                }
                //2.更新当前数值
                //debug("当前序号:" + _curIdx.ToString());
                //在begin之前的数值均被设为0,当前curidx为begin开始赋值,第一次计算时curIdx至begin时

                //当结果数组的长度小于计算数组的长度,我们需要将新的数值增加进去
                //更新当前index的数值(_curIdx+1)为结果数组中需含有的位数(实际计算中)
                if (_curIdx + 1 <= _result.Count)
                    _result[_curIdx] = output[_curIdx - begin];
                else
                    _result.Add(output[_curIdx - begin]);
                base.isValid = true;
                //3.如果有新的bar进来则增加一个结果位
                //确保公式计算结果与源数据同步。100位的源数据 ->100位的计算结果
                for (int i = _curIdx+1; i < OrigDataSeries.Count; i++)
                {
                    //if (i < tmp.Count)//索引操作在数组范文内 直接赋值
                    //    tmp[i-1] = (decimal)output[i - begin];
                    //else //索引操作在数组范外 增加位数
                    _result.Add(output[i - begin]);
                    //_curIdx++;
                }
                //更新当前的index,下次计算数据的时候就可以不需要计算原来计算过的数值
                _curIdx = OrigDataSeries.Count - 1;
                //debug("current index is:" + _curIdx.ToString());
                //debug("origdata count is:" +OrigDataSeries.Count.ToString()+"result count:"+ _result.Count.ToString());
                
                //for (int i = 0; i < OrigDataSeries.Count - 1;i++ )
                //{
                //    debug("#:" + i.ToString() + " data:" + OrigDataSeries[i] + " value:" + _result[i].ToString());
                //}
                

            }
            catch (Exception ex)
            {
                debug(ex.ToString());
            }
        }

        //
        public override double  LookBack(int n)
        {
            return _result.LookBack(n);
        }

        public override double[] Data
        {
            get { return _result.Data; }
        }
        public override double First
        {
            get { return _result.First; }
        }

        public override double this[int index]
        {
            get { return _result[index]; }
        }
        public override double Last
        {
            get { return _result.Last; }
        }

        public override void Reset()
        {
            _result.Clear();
            _latestvalue = double.MinValue;
            _curIdx = -1;
        }
        public override int Count
        {
            get { return _result.Count; }
        }

    }
}
