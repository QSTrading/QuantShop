using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeLink.API;
using TradingLib.API;
using TradingLib.Data;
using TradeLink.Common;
using TicTacTec.TA.Library; // to use TA-lib indicators

namespace TradingLib.Indicator
{
    //策略体系中定义了数据驱动 由哪个个数据驱动了该策略 tick数据驱动了公式的计算
    public abstract class IndicatorBase : ISeries, IIndicator
    {
        public event DebugDelegate SendDebugEvent;
        protected void debug(string msg)
        {
            if (SendDebugEvent != null)
                SendDebugEvent(msg);
        }
        
      
        //数据集合本身是ISeries 指标本身也要实现ISeries 这样才可以利用公式进行嵌套计算
        //private DataSeries _resoult

        private bool _valid=false;
        //用于计算的源数据
        private ISeries _datalist;


        //需要将bar也封装成支持ISeries接口的数据 这样公式体系可以统一对数据进行计算

        //计算由indicator计算所生成的数据 indicator实现了ISeries接口 可以用于公式计算
        public IndicatorBase(ISeries series)
        {
            _datalist = series;
        }
        /*
        public IndicatorBase(BarList barlist)
        {
            _datalist = new BarList2Series(barlist);
        }
        public IndicatorBase(BarList barlist,BarDataType datatype)
        {
            _datalist = new BarList2Series(barlist, datatype);
           
        }
        public IndicatorBase(BarList barlist, BarDataType datatype, BarInterval interval)
        {
            _datalist = new BarList2Series(barlist, datatype, interval);
        }
        */

        //支持ISeries接口 用于放入其他函数中进行计算
        public abstract int Count { get; }
        public abstract void Reset();

        public abstract double LookBack(int n);
        public abstract double this[int index] { get; }
        public abstract double[] Data { get; }
        

        //支持indicator接口
        public abstract void Caculate();
        public abstract double First { get; }
        public abstract double Last { get; }
        


        protected ISeries OrigDataSeries {
            get
            {
                return _datalist;
            }
        
        }
        //指标是否有效
        public bool isValid
        {
            get
            {
                return _valid;
            }
            protected set
            {
                _valid= true;
            }
        }




        //指标的一些常用操作
        //指标与一个数列的相交 几个bar前的一个相交
        //ISeries 与 ISeries相交
        public virtual EnumCross Cross(ISeries series, int N)
        {
            if (((N >= 0) &&isValid) && ((series.Count >= (N + 2)) && (OrigDataSeries.Count >= (N + 2))))
            {
                double num = series.LookBack(N + 1) - LookBack(N + 1);
                double num2 = series.LookBack(N) - LookBack(N);
                if ((num < 0.0) && (num2 > 0.0))
                {
                    return EnumCross.Below;
                }
                if ((num > 0.0) && (num2 < 0.0))
                {
                    return EnumCross.Above;
                }
            }
            return EnumCross.None;
        }

        //ISeries与一个数值相交
        public virtual EnumCross Cross(double dvalue, int N)
        {
            if (((N >= 0) && isValid) && (OrigDataSeries.Count>= (N + 2)))
            {
                double num = dvalue - LookBack(N+1);
                double num2 = dvalue - LookBack(N);
                if ((num < 0.0) && (num2 > 0.0))
                {
                    return EnumCross.Below;
                }
                if ((num > 0.0) && (num2 < 0.0))
                {
                    return EnumCross.Above;
                }
            }
            return EnumCross.None;
        }

        //回溯0代表 当前数值进行了交叉,比如价格原本在2300下方，目前价格穿越了2300 但不代表当前bar就收在2300之上
        public virtual EnumCross Cross(ISeries series)
        {
            return Cross(series, 0);
        }
        public virtual EnumCross Cross(double dvalue)
        {
            return Cross(dvalue, 0);
        }
        //确认已经形成了穿越 指上个价格成功的形成了交叉
        public virtual EnumCross Crossed(ISeries series)
        {
            return Cross(series, 1);
        }
        public virtual EnumCross Crossed(double dvalue)
        {
            return Cross(dvalue, 1);
        }



    }
}
