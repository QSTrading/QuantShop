using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeLink.Common;
//using TradingLib.Core;
using TradeLink.API;
using TradingLib.API;
using System.ComponentModel;

namespace Strategy.ExitPosition
{
    /// <summary>
    /// 止损,BreakEven,两级回调止盈
    /// 当价格触及止损价格时,止损平仓
    /// 当价格触及BreakEven触发价时,将止损价提高到BreakEven价格
    /// 当价格触及一段止盈价格时,将止损价格提高到一级止盈价格
    /// 当价格触及二段止盈价格时,将止损价格提高到二级止盈价格
    /// </summary>
    public class TwoStepPoint : TradingLib.Core.PositionCheckTemplate
    {
        //该position在配置窗口中显示的标题名称
        public static string Title
        {
            get { return "两阶段跟踪止盈"; }
        }
        //positioncheck的中文解释
        public static string Description
        {
            get { return "两阶段跟踪止盈"; }
        }


        [Description("止损值"), Category("运行参数")]
        public decimal StopLoss { get; set; }
        [Description("BreakEven"), Category("运行参数")]
        public decimal BreakEven { get; set; }
        [Description("一级止盈起步值"),Category("运行参数")]
        public decimal Start1 { get; set; }
        [Description("一级止盈值"), Category("运行参数")]
        public decimal Loss1 { get; set; }
        [Description("二级止盈起步值"), Category("运行参数")]
        public decimal Start2 { get; set; }
        [Description("二级止盈值"), Category("运行参数")]
        public decimal Loss2 { get; set; }
              
        //策略内部调用参数
        private decimal stoplossprice = 0;
        private decimal breakeventriger = 0;
        private decimal profittakeprice1 = 0;
        private decimal profittakeprice2 = 0;
        private GUI.fmStopTrailing fm;

        //按照给定的参数 我们预先判断逻辑状态
        private bool stopEnable = false;
        private bool breakevenEnable = false;
       // private bool step1Enable = false;
       // private bool step2Enable = false;
        //出场策略触发所锁 如果已经触发就不要重复发单 防止由于保单的延迟 造成误发单
        private bool trigerlock = false;
        //激活策略
        public override void Reset()
        {
            base.Reset();
            //D("打开交易提示窗口");
            if (fm == null)
            {
                fm = new GUI.fmStopTrailing();
                //显示策略标题
                fm.Text = Title + " " + ToText() + ":" + myPosition.symbol;
                //绑定输出窗体的数据源
                fm.bindDataSource(DataManager, myPosition.symbol);
                //绑定事件
                fm.SendFlatPosition += new VoidDelegate(fm_SendFlatPosition);
                fm.SendBuyAction += new VoidDelegate(fm_SendBuyAction);
                fm.SendSellAction += new VoidDelegate(fm_SendSellAction);
                fm.Show();
            }
            fm.Visible = true;
            if (StopLoss > 0)
                stopEnable = true;
            if (BreakEven > 0)
                breakevenEnable = true;
            fm.setFunction(stopEnable, breakevenEnable);            
        }
        //界面操作函数
        void fm_SendSellAction()
        {
            SellMarket(fm.OrdSize);
            fm.message("手动卖出");
        }

        void fm_SendBuyAction()
        {
            BuyMarket(fm.OrdSize);
            fm.message("手动买入");
        }

        void fm_SendFlatPosition()
        {
            if (!trigerlock)
            {
                FlatPosition();
                fm.message("手动干预平仓");
                trigerlock = true;
            }
        }
        //关闭策略
        public override void Shutdown()
        {
            base.Shutdown();
            fm.Visible = false;
        }


        //策略的GotTick调用 当有新的tick进来时 我们进行的调用
        public  override void GotTick(Tick k)
        {
            base.GotTick(k);
            //根据tick更新界面数据
            try
            {
                //只有在valid情况下 positioncheckcentre 才通过switchrepsonse激活了某个策略，这个时候信息输出窗口也必然已经实例化了。
                fm.updateForm(k, myPosition, stoplossprice,breakeventriger, profittakeprice1, profittakeprice2);
            }
            catch (Exception ex)
            {
                D(ex.ToString());
            }
        }
        /// <summary>
        /// 改进:注意平仓后的重复平仓发单问题,这个问题最好由positionchecktemplate统一解决
        /// positiontemplate 记录本地发单。比较本地发单和当前仓位 避免oversell
        /// 关注oversell tracker 组件 看看是否有构件可以很好的解决这个问题。
        /// </summary>
        /// <param name="msg"></param>
        public override void checkPosition(out string msg)
        {
            msg = "";
            //空仓情况下我们将止损价格与保本价格等相关价格止空
            if(myPosition.isFlat)
            {
                resetPriceTriger();
                trigerlock = false;
                return;
            }
            //计算止损价
            //多头
            if (myPosition.isLong && !trigerlock)
            {
                //二级止盈
                if ((myPosition.Highest - myPosition.AvgPrice) > Start2)
                {
                    profittakeprice2 = myPosition.Highest - Loss2;
                    if ((myPosition.Highest - myPosition.LastPrice) > Loss2)
                    {
                        //平掉所有仓位
                        FlatPosition();
                        //SellMarket(myPosition.Size / 2);
                        fm.message("多头触发二级止盈 平仓");
                        trigerlock = true;
                        
                    }
                }
                //一级止盈
                else if ((myPosition.Highest - myPosition.AvgPrice) > Start1)
                {
                    profittakeprice1 = myPosition.Highest - Loss1;
                    if ((myPosition.Highest - myPosition.LastPrice) > Loss1)
                    {
                        FlatPosition();
                        fm.message("多头触发一级止盈 平仓");
                        trigerlock = true;
                        
                    }
                }
                if (breakevenEnable)
                {
                    //最高价>成本+BreakEven触发点位
                    if (myPosition.Highest - myPosition.AvgPrice > BreakEven)
                    {   //breakeventriger为保本触发价
                        breakeventriger = myPosition.AvgPrice + 1;
                        if (myPosition.LastPrice <= breakeventriger)
                        {
                            FlatPosition();
                            fm.message("多头触发保本 平仓");
                            trigerlock = true;
                            
                        }

                    }
                    //D("breakeven price:"+breakeventriger.ToString());
                }
                if (stopEnable)
                {
                    stoplossprice = myPosition.AvgPrice - StopLoss;
                    if (myPosition.LastPrice <= stoplossprice)
                    {
                        FlatPosition();
                        fm.message("多头触发止损 平仓");
                        trigerlock = true;
                        
                    }
                }
            }
            //空头
            if (myPosition.isShort && !trigerlock)
            {
                //二级止盈
                if ((myPosition.AvgPrice - myPosition.Lowest) > Start2)
                {
                    profittakeprice2 = myPosition.Lowest + Loss2;
                    if ((myPosition.LastPrice - myPosition.Lowest) > Loss2)
                    {
                        FlatPosition();
                        fm.message("空头触发二级止盈 平仓");
                        trigerlock = true;
                        
                    }
                }
                //一级止盈
                else if ((myPosition.AvgPrice - myPosition.Lowest) > Start1)
                {
                    profittakeprice1 = myPosition.Lowest + Loss1;
                    if ((myPosition.LastPrice - myPosition.Lowest) > Loss1)
                    {
                        FlatPosition();
                        fm.message("空头触发一级止盈 平仓");
                        trigerlock = true;
                        
                    }
                }
                if (breakevenEnable)
                {
                    if (myPosition.AvgPrice -myPosition.Lowest >BreakEven)
                    {
                        breakeventriger = myPosition.AvgPrice -1;
                        if (myPosition.LastPrice >= breakeventriger)
                        {
                            FlatPosition();
                            fm.message("空头触发保本 平仓");
                            trigerlock = true;
                            
                        }
                    }
                
                }
                if (stopEnable)
                {
                    stoplossprice = myPosition.AvgPrice + StopLoss;
                    if (myPosition.LastPrice >= stoplossprice)
                    {
                        FlatPosition();
                        fm.message("空头触发止损 平仓");
                        trigerlock = true;
                        
                    }
                }


            }

        }
        //仓位归0后 重置内部参数
        private void resetPriceTriger()
        {
            stoplossprice = 0;
            breakeventriger = 0;
            profittakeprice1 = 0;
            profittakeprice2 = 0;
        }
        

        //定义如何保存该策略
        public override string ToText()
        { 
            string s= string.Empty;
            string[] r = new string[] {DataDriver.ToString(), Start1.ToString(),Loss1.ToString(),Start2.ToString(),Loss2.ToString(),StopLoss.ToString(),BreakEven.ToString()};
            return string.Join(",", r);
        }

        //定义如何从保存的文本生成该策略
        public override IPositionCheck FromText(string str)
        {
            string[] rec = str.Split(',');
            //if (rec.Length < 5) throw new InvalidResponse();
            
            PositionDataDriver driver = (PositionDataDriver)Enum.Parse(typeof(PositionDataDriver), rec[0]);
            decimal start1 = Convert.ToDecimal(rec[1]);
            decimal loss1 = Convert.ToDecimal(rec[2]);
            decimal start2 = Convert.ToDecimal(rec[3]);
            decimal loss2 = Convert.ToDecimal(rec[4]);
            decimal stoploss = Convert.ToDecimal(rec[5]);
            decimal breakeven = Convert.ToDecimal(rec[6]);

            DataDriver = driver;
            Start1 = start1;
            Loss1 = loss1;
            Start2 = start2;
            Loss2 = loss2;
            StopLoss = stoploss;
            BreakEven = breakeven;
            return this;
        }
    }

}
