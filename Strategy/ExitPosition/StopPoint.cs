using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Text;
//using TradeLink.Common;
using TradingLib.API;
using TradeLink.API;
using System.ComponentModel;

namespace Strategy.ExitPosition
{
    /// <summary>
    /// 固定点数止损,当价格反向达到我们设定的止损值时,我们平仓出局
    /// </summary>
    class TwoStepPercent:TradingLib.Core.PositionCheckTemplate
    {
        //该position在配置窗口中显示的标题名称
        public static string Title
        {
            get { return "固定点数止损"; }
        }
        //positioncheck的中文解释
        public static string Description
        {
            get { return "固定点数止损"; }
        }

        [Description("固定点数止损值"), Category("运行参数")]
        public decimal Loss { get; set; }

        private TradingLib.Indicator.SMA _sma_1min;
        private TradingLib.Indicator.SMA _sma2;
        private TradingLib.Indicator.Bolling _bolling;
        public override void Reset()
        {
            
            base.Reset();
            ISeries close_1min = getBarSeries(BarDataType.Close,BarInterval.Minute);

            //_sma_1min = new TradingLib.Indicator.SMA(close_1min, 2);
            //_sma_1min.SendDebugEvent +=new DebugDelegate(D);
            //_sma2 = new TradingLib.Indicator.SMA(_sma_1min, 2);
            //_sma2.SendDebugEvent +=new DebugDelegate(D);

            //AddIndicator(_sma_1min);
            //AddIndicator(_sma2);
            //AddIndicator(_sma2);
            //_sma_1min.SendDebugEvent +=new DebugDelegate(D);
            
            //AddIndicator(_sma_1min);
            _bolling = new TradingLib.Indicator.Bolling(getBarSeries());
            _bolling.SendDebugEvent +=new DebugDelegate(D);
            AddIndicator(_bolling);
        }

        public override void checkPosition(out string msg)
        {
            //D("we are here");
            //foreach (decimal i in _sma_1min[myPosition.symbol])
            //{
                //D(i.ToString());
            //}
            msg = "";
            
            //D(myPosition.ToString());
            if(myPosition.isLong)
            {
                if ((myPosition.AvgPrice - myPosition.LastPrice) > Loss)
                {
                    D("止损触发 平掉所有仓位");
                    FlatPosition();
                }
            }
            if (myPosition.isShort)
            {
                if ((myPosition.LastPrice - myPosition.AvgPrice) > Loss)
                {
                    D("止损触发 平掉所有仓位");
                    FlatPosition();
                }
            }
            D("Send indicators");
            //D(myBarList[myBarList.Last].Bardate.ToString());
            //回报计算结果或者相关信息
            I(new object[] { _bolling.Mid.LookBack(0), _bolling.UNBand.LookBack(0), _bolling.DNBand.LookBack(0) });

        }

        //定义如何保存该策略
        public override string ToText()
        {
            string s = string.Empty;
            string[] r = new string[] { DataDriver.ToString(),Loss.ToString() };
            return string.Join(",", r);
        }

        //定义如何从保存的文本生成该策略
        public override IPositionCheck FromText(string str)
        {
            string[] rec = str.Split(',');
            //if (rec.Length < 5) throw new InvalidResponse();

            PositionDataDriver driver = (PositionDataDriver)Enum.Parse(typeof(PositionDataDriver), rec[0]);
            decimal loss = Convert.ToDecimal(rec[1]);
            

            DataDriver = driver;
            Loss = loss;
            return this;
        }

        //public override string PositionCheckDescription()
        //{
        //    return base.PositionCheckDescription();
        //}


    }
}
