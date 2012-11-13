using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeLink.Common;
using TradingLib.API;
using TradeLink.API;
using TradingLib.Data;
using TradingLib.Indicator;
using Easychart.Finance.DataProvider;
using System.ComponentModel;

namespace TradingLib.Core
{
    //PositionCheckTemplate在ResponseTemplate基础上定义了 positioncheck的基础工作流程
    //positionCheck实现的操作有平仓,加减仓,反手等操作 positioncheck并不作数据管理
    //positioncheck可以在template的基础上用简单的逻辑实现较为复杂工作
    //positioncheck不需要进行数据管理 order管理 以及 position管理 这些工作统一由tradingtracker完成
    //positioncheck可以tick驱动,或者bar驱动此外的数据position并不处理与关心
    public abstract class PositionCheckTemplate:ResponseTemplate,IPositionCheck
    {
        //仓位检查一般需要用到的数据

        //记录positionCheck所跟踪的Position
        private Position _pos;
        [Description("Positon数据"), Category("PositionCheck基本属性")]
        public Position myPosition { get { return _pos; } set { _pos = value; } }
        //private BarList myBarList { get;}

        //记录了与position.symbol所对应的barlist 这样在positioncheck的逻辑中就可以得到
        //与之对应的barlist用于计算操作
        //每个positioncheck在初始化的时候初始化私有barlist然后从服务器得到基本的数据
        //形成本position所需的周期的Bars用于positioncheck的计算服务
        //private BarList _bl;
        //[Description("Bar数据"), Category("PositionCheck基本属性")]
        //protected BarList myBarList { get { return _blt[_pos.symbol]; }}

        private BarListTracker _blt;
        [Description("Bar数据管理容器"), Category("PositionCheck基本属性")]
        public BarListTracker BarListTracker { get { return _blt; } set { _blt = value; } }

        private QSMemoryDataManager _mdm;
        [Description("绘图数据管理器"), Category("PositionCheck基本属性")]
        public QSMemoryDataManager DataManager { get { return _mdm; } set { _mdm = value; } }


        //数据驱动类型标识该position检查用什么数据去驱动tick,minute,等
        private PositionDataDriver _pdatadriver=PositionDataDriver.Tick;
        [Description("策略驱动数据的类型"), Category("数据")]
        public PositionDataDriver DataDriver { get {return _pdatadriver;} set{_pdatadriver = value;} }
        //标记是否生效
        //private _enable; 
        //public bool Enable { get; set; }
        //position的初始化

        /*
        public PositionCheckTemplate() : this(new PositionImpl()) { }
        public PositionCheckTemplate(Position pos)
        {
            //初始化设定position
            //_pos = pos;
            //根据post初始化bl;默认初始化1min的Bar数据
            //_bl = new BarListImpl(pos.symbol,(int)BarInterval.Minute);
            //_bl.GotNewBar += new SymBarIntervalDelegate(_bl_GotNewBar);
            
        }
         * */
        #region 指标函数部分

        private List<IIndicator> _indicatorCal = new List<IIndicator>();

        protected void AddIndicator(IndicatorBase ind)
        {
            //ind.BarListTracker = _blt;
            //_blt.GotNewBar +=new SymBarIntervalDelegate(ind.GotNewBar);
            _indicatorCal.Add(ind);
        }
        #endregion


        //
        /*
        private int _barsSinceEntry=0;
        private bool _barsSinceEntryFlag = false;
        private int _barsCountEntry = 0;

        private void updateBarsSinceEntry()
        {
            if (myPosition.isFlat)
            {
                _barsCountEntry = 0;
                _barsSinceEntryFlag = false;
            }
            else { 
                _barsCountEntry = 
            
            }
        }
         * */


        #region response input 的函数
        //当有新的tick数据进来后执行的操作 注意positioncheck通过上层tick检查与symbol的配对，到本positioncheck
        //的tick都是myposition的tick
        //注意这里被子类覆盖我们需要用virtual 否则子类对其进行覆写后会产生运行时nullreference exception的问题
        //策略的数据驱动入口就是有这里进去,所有策略通过对应Tick来驱动
        public  override void GotTick(Tick k)
        {
            //GotTick函数当策略有效的时候我们进行指标计算 无效的时候我们直接返回
            //D("we got here position check");
            if (!base.isValid)
                return;
            //D("##进行计算指标");

            //1.指标计算
            //只对激活的指标进行计算,不激活的response内的指标统一不计算，当response被关闭时候,系统要正常关闭指标
            //计算指标数据驱动程序 自行对指标进行计算 当指标计算完毕后 再进入position阶段
            foreach (IIndicator ind in _indicatorCal)
            {
                if (!k.isTrade)
                    return;
                //D("we are here");
                ind.Caculate();
            }

            //2.检查position
            //如果检查是基于tick驱动的,那么我们每个tick过来就进行position检查
            if(DataDriver == PositionDataDriver.Tick)
            {
                string msg;
                checkPosition(out msg);
                //onTick(k);
            }

            //3.子类所实现的用tick驱动的操作
            /*
            if(_bl.Symbol == k.symbol)
                _bl.newTick(k);
             * */

            //
            //_blt.GotNewBar += new SymBarIntervalDelegate(_blt_GotNewBar);
        }

        //public abstract void onTick(Tick k);
        public override void GotOrder(Order o)
        {
            base.GotOrder(o);
        }

        public override void GotFill(Trade f)
        {
            base.GotFill(f);
        }

        #endregion

        //当Position对应的barlist获得一个新Bar的时候触发这个事件
        void _bl_GotNewBar(string symbol, int interval)
        {
            if (!isValid)
                return;
            string msg=string.Empty;
            if (symbol != myPosition.symbol)
                return;
            switch (DataDriver)
            { 
                case PositionDataDriver.min_1:
                    if (interval == 60)
                        checkPosition(out msg);
                    break;
                case PositionDataDriver.min_3:
                    if (interval == 180)
                        checkPosition(out msg);
                    break;
                case PositionDataDriver.min_5:
                    if(interval == 300)
                        checkPosition(out msg);
                    break;
                case PositionDataDriver.min_15:
                    if(interval == 900)
                        checkPosition(out msg);
                    break;
                case PositionDataDriver.min_30:
                    if(interval == 1800)
                        checkPosition(out msg);
                    break;
                default:
                    break;
            }
        }

        public abstract void checkPosition(out string msg);
        public override void Reset()
        {
            base.Reset();
            //_bl.GotNewBar +=new SymBarIntervalDelegate(_bl_GotNewBar);
        }

        public override void Shutdown()
        {
            //_bl.GotNewBar -= new SymBarIntervalDelegate(_bl_GotNewBar);
            //base.shutdown(_pt, gt);
            //将所有注册的公式清0，避免不必要的计算
            foreach (IndicatorBase ind in _indicatorCal)
            {
                ind.Reset();
            }
        }
        //配置文本化
        public abstract string ToText();
        //从文本读取配置信息,通过给positioncheck配置不同的参数实现不同的效果
        public abstract IPositionCheck FromText(string msg);
        //得到容易理解的positioncheck的中文描述
        public virtual string PositionCheckDescription(){return "";}


        #region position基础操作
        //需要用到的常规仓位操作，方便具体的position操作调用
        protected void SellMarket(int size)
        { 
            sendorder(new MarketOrder(myPosition.symbol,false,size));  
        }
        protected void SellLimit(int size,decimal price)
        { 
            sendorder(new LimitOrder(myPosition.symbol,false,size,price));
        }
        private void SellStop(int size, decimal price)
        {
            sendorder(new SellStop(myPosition.symbol, size, price));
        }

        protected void BuyMarket(int size)
        {
            sendorder(new MarketOrder(myPosition.symbol, true, size));
        }
        protected void BuyLimit(int size, decimal price)
        {
            sendorder(new LimitOrder(myPosition.symbol, true, size, price));
        }
        protected void BuyStop(int size, decimal price)
        { 
            sendorder(new SellStop(myPosition.symbol,size,price));
        }
        protected void FlatPosition()
        {
            sendorder(new MarketOrderFlat(myPosition));
        }

        protected void ReversePosition()
        {
            sendorder(new MarketOrderFlat(myPosition));
        }
        #endregion

        #region positioncheck基础数据


        
        //获得本策略监控的position的默认symbol的barlist
        //返回某个symbol的bar数据
        protected ISeries getBarSeries(string symbol) { return new BarList2Series(_blt[symbol]);}
        protected ISeries getBarSeries(string symbol, BarDataType datatype) { return new BarList2Series(_blt[symbol], datatype); }
        protected ISeries getBarSeries(string symbol,BarDataType datatype,BarInterval interval){return new BarList2Series(_blt[symbol],datatype,interval);}
        protected ISeries getBarSeries() { return getBarSeries(myPosition.symbol); }
        protected ISeries getBarSeries(BarDataType datatype) { return getBarSeries(myPosition.symbol, datatype); }
        protected ISeries getBarSeries(BarDataType datatype, BarInterval interval) { return getBarSeries(myPosition.symbol, datatype, interval); }

        protected void demo()
        { 
            
        }
        //protected ISeries Close{get{return getBarSeries();}}
        //protected ISeries Open { get { return getBarSeries(BarDataType.Open); } }
        //protected ISeries High { get { return getBarSeries(BarDataType.High); } }
        //protected ISeries Low { get { return getBarSeries(BarDataType.Low); } }

        #endregion

    }

}
