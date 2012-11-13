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
    public class EMA : IndicatorBase
    {

        //最新的源数据值
        private double _latestvalue=double.MinValue;
        //指标计算的当前序号
        private int _curIdx = -1;
        //简单算术平均计算周期
        private int _period;
        //计算结果
        private DataSeries _result;
        //
        private double _multiplier;
        //private BarDataType _datatype;

        //计算由indicator计算所生成的数据 indicator实现了ISeries接口 可以用于公式计算
        public EMA(ISeries series, int N)
            : base(series)
        {
            _result = new DataSeries();
            _period = 1;
            _curIdx = -1;
            _multiplier = -1;
            if (N < 1)
                N = 1;
            _period = N;
            //_curIdx = N - 1;
            _multiplier = 2.0 / ((double)(N + 1));


        }
     

        //计算指标数据 EMA算法属于快速算法
        public override void Caculate()
        {
            if (_curIdx < 0)
                _curIdx = 0;
            
            if (OrigDataSeries.Count > 0)
            {
                base.isValid = true;
                if (OrigDataSeries[_curIdx] != _latestvalue)
                {
                    if (_curIdx == 0)
                    {
                        if (_curIdx + 1 < _result.Count)
                            _result[_curIdx] = OrigDataSeries[_curIdx];
                        else
                            _result.Add(OrigDataSeries[_curIdx]);
                    }
                    else
                    {
                        _result[_curIdx] = Math.Round((double)((OrigDataSeries[_curIdx] - _result[_curIdx - 1]) * _multiplier + _result[_curIdx - 1]),6);
                    }
                }
                //debug("EMA 2 ");
                for (int i = _curIdx + 1; i < OrigDataSeries.Count; i++)
                {
                    _result.Add(Math.Round((double)((OrigDataSeries[i] - _result[i-1]) * _multiplier + _result[i-1]), 6));
                }
                _curIdx = OrigDataSeries.Count - 1;
                _latestvalue = OrigDataSeries[_curIdx];
            }
            
                //for (int i = 0; i < OrigDataSeries.Count - 1; i++)
                //{
                //    debug("#:" + i.ToString() + " data:" + OrigDataSeries[i] + " value:" + _result[i].ToString());
                //}


        }

        //
        public override double LookBack(int n)
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
            _curIdx = -1;
            _latestvalue = double.MinValue;

        }

        public override int Count
        {
            get { return _result.Count; }
        }


    }
}
