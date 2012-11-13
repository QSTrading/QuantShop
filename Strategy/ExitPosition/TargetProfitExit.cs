using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeLink.API;
using TradingLib.API;
using System.ComponentModel;


namespace Strategy.ExitPosition
{
    /// <summary>
    /// 当价格达到我们的目标位置时平仓
    /// </summary>
    class TargetProfitExit:TradingLib.Core.PositionCheckTemplate
    {
        //该position在配置窗口中显示的标题名称
        public static string Title
        {
            get { return "目标盈利止盈"; }
        }
        //positioncheck的中文解释
        public static string Description
        {
            get { return "目标盈利止盈"; }
        }

        [Description("止损值"), Category("运行参数")]
        public decimal TargetProfit { get; set; }
        [Description("平仓数量"), Category("运行参数")]
        public int CloseSize { get; set; }

        bool trigerlock = false;
        GUI.fmTargetProfit fm;
        public override void Reset()
        {
            base.Reset();

            if (fm == null)
            {
                fm = new GUI.fmTargetProfit(myPosition.symbol,TargetProfit,CloseSize);
                fm.Show(); 
            }
            fm.Visible = true;

        }
        public override void Shutdown()
        {
            base.Shutdown();
            fm.Visible = false;
        }

        public  override void GotTick(Tick k)
        {
            base.GotTick(k);
            //D("XXXXXXXXXXX got tick");
            if (k.isTrade)
                fm.updateForm(k.trade, myPosition.AvgPrice, myPosition.isLong);
        }

        public override void checkPosition(out string msg)
        {
            msg = "";

            if (myPosition.isFlat)
            {
                trigerlock = false;
                return;
            }

            //当价格达到我们预期利润时 平仓
            if (myPosition.isLong && !trigerlock)
            {
                if (myPosition.LastPrice >= myPosition.AvgPrice + fm.TargetProfit)
                {
                    FlatPosition();
                    trigerlock = true;

                }

            }

            if (myPosition.isShort && !trigerlock)
            {
                if (myPosition.LastPrice <= myPosition.AvgPrice - fm.TargetProfit)
                {
                    FlatPosition();
                    trigerlock = true;
                }
            }


        }

        public override string ToText()
        {
            string s = string.Empty;
            string[] r = new string[] { DataDriver.ToString(),TargetProfit.ToString(),CloseSize.ToString()};
            return string.Join(",", r);
        }

        public override IPositionCheck FromText(string msg)
        {
            string[] rec = msg.Split(',');
            //if (rec.Length < 5) throw new InvalidResponse();

            PositionDataDriver driver = (PositionDataDriver)Enum.Parse(typeof(PositionDataDriver), rec[0]);
            decimal target = Convert.ToDecimal(rec[1]);
            int size = Convert.ToInt16(rec[2]);

            TargetProfit = target;
            CloseSize = size;

            return this;
        }
    }
}
