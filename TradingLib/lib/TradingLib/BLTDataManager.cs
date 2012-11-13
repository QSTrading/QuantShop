using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeLink.API;
using TradeLink.Common;
using Easychart.Finance.DataProvider;

namespace TradingLib
{
    //利用tradelink的 barlisttracker 包装成ChartControl的DataManager
    public class BLTDataManager : DataManagerBase
    {
        private BarListTracker blt = null;
        public event DebugDelegate SendDebugEvent;

        public BLTDataManager(BarListTracker b)
        {
            blt = b;
        }
        private void debug(string s)
        {
            if (SendDebugEvent != null)
            {
                SendDebugEvent(s);
            }
        }
        //获得某个symbol的一定数目的Bars
        public override IDataProvider GetData(string Code, int Count)
        {
            debug("dataprovider GetData is called");
            CommonDataProvider cdp = new CommonDataProvider(this);
            cdp.SetStringData("Code", Code);
            //注意bl.OpenDouble() 是否包含最新的k线数据
            if (blt !=null)
            {

                BarList bl = blt[Code,60];
                int N = bl.Count;
                double[] dt = new double[N];

                int[] d = bl.Date();
                int[] t = bl.Time();
                for(int i=0;i<N;i++)
                {
                    dt[i] =Util.ToDateTime(d[i],t[i]).ToOADate();
                    
                }
                //double[] open = Array.ConvertAll(bl.Open(), new Converter<decimal, double>(DecimalToDouble));

                debug("price num:" + bl.OpenDouble().Length.ToString() + "date num:" + dt.Length.ToString());
                
                cdp.LoadBinary(new double[][] {bl.OpenDouble(),bl.HighDouble(),bl.LowDouble(),bl.CloseDouble(),bl.VolDouble(),dt});
                return cdp;
                //return base.GetData(Code, Count);
                
            }
            return base.GetData(Code, Count);
        }


    }
}
