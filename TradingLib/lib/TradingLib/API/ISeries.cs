using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    public interface ISeries
    {
        //计数
        int Count { get; }
        //最近一个数值
        double Last { get; }
        double First { get; }
        //回溯
        double LookBack(int n);
        //获得index对位位置的数值
        double this[int index] { get; }
        //返回double类型的数组 供计算器计算使用
        double[] Data { get; }
    }


}
