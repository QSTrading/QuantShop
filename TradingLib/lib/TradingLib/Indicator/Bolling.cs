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
    public class Bolling : IndicatorBase
    {

        //最新的源数据值
        private double _latestvalue = double.MinValue;
        //指标计算的当前序号
        private int _curIdx = -1;
        //简单算术平均计算周期
        private int _N{get;set;}
        private double _K{get;set;}
        //计算结果
        private SMA _mid;
        private DataSeries _uplist;
        private DataSeries _dnlist;
        private DataSeries _stdlist;
        //
        private double _multiplier;
        //private BarDataType _datatype;

        //计算由indicator计算所生成的数据 indicator实现了ISeries接口 可以用于公式计算
        public Bolling(ISeries series, int N,double K)
            : base(series)
        {
            _N = N;
            _K = K;
            _mid = new SMA(series, N);
            _uplist = new DataSeries();
            _dnlist = new DataSeries();
            _stdlist = new DataSeries();
            _latestvalue = double.MinValue;
            _curIdx = -1;
            Init();
        }
        public Bolling(ISeries series)
            : base(series)
        {
            _N = 20;
            _K = 2.0;
            _mid = new SMA(series, _N);
            _uplist = new DataSeries();
            _dnlist = new DataSeries();
            _stdlist = new DataSeries();
            _latestvalue = double.MinValue;
            _curIdx = -1;
            Init();
        }
        //小于SMA周期N的 数值均填0
        private void Init()
        {
            this._curIdx = this._N - 1;
            for (int i = 0; i < this._N; i++)
            {
                _uplist.Add(0.0);
                _dnlist.Add(0.0);
                _stdlist.Add(0.0);
            }
        }



        //计算指标数据 EMA算法属于快速算法
        public override void Caculate()
        {
            if (OrigDataSeries.Count >= _N)
            {
                _mid.Caculate();
                base.isValid = true;
                if (OrigDataSeries[_curIdx] != _latestvalue)
                {
                    double avg = _mid[_curIdx];
                    double std = Math.Round(MathHelper.Standard(OrigDataSeries,_curIdx -_N+1,_N,avg),6);
                    _stdlist[_curIdx] = std;
                    _uplist[_curIdx] = Math.Round((avg + _K * std), 6);
                    _dnlist[_curIdx] = Math.Round((avg - _K * std), 6);

                }
                for (int i = _curIdx + 1; i < OrigDataSeries.Count; i++)
                {
                    double avg1 = _mid[i];
                    double std1 = Math.Round(MathHelper.Standard(OrigDataSeries, i - _N + 1, _N, avg1), 6);
                    _stdlist.Add(std1);
                    _uplist.Add(Math.Round((avg1 + _K * std1), 6));
                    _dnlist.Add(Math.Round((avg1 - _K * std1), 6));
                }
                _curIdx = OrigDataSeries.Count - 1;
                _latestvalue = OrigDataSeries[_curIdx];
            }
            /*
            for (int i = 0; i < OrigDataSeries.Count - 1; i++)
            {
                debug("#:" + i.ToString() + " data:" + OrigDataSeries[i] + " value:" + _mid[i].ToString()+" up:"+_uplist[i].ToString()+" dn:"+_dnlist[i].ToString());//_.ToString());
            }
             * */


        }

        public ISeries Mid { get { return (ISeries)_mid; } }
        public ISeries UNBand { get { return _uplist; } }
        public ISeries DNBand { get { return _dnlist; } }
        //
        public override double LookBack(int n)
        {   
            return _mid.LookBack(n);
        }

        public override double[] Data
        {
            get { return _mid.Data; }
        }
        public override double First
        {
            get { return _mid.First; }
        }

        public override double this[int index]
        {
            get { return _mid[index]; }
        }
        public override double Last
        {
            get { return _mid.Last; }
        }

        public override void Reset()
        {
            _mid.Reset();
            _uplist.Clear();
            _dnlist.Clear();
            _stdlist.Clear();
            _latestvalue = double.MinValue;
            Init();
        }

        public override int Count
        {
            get { return _mid.Count; }
        }

    }
}
