using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeLink.API;
using TradeLink.Common;
using TradingLib.Data;
using Easychart.Finance.DataProvider;
namespace TradingLib.Core
{
    //封装了交易需要保存的数据包含 position order bar等基本数据
    public class TradingTrackerCentre
    {
        public event DebugDelegate SendDebugEvet;
        bool _verb = true;
        public bool VerboseDebugging { get { return _verb; } set { _verb = value; } }

        private PositionTracker _pt;
        public PositionTracker PositionTracker { get { return _pt; } }
        private OrderTracker _ordt;
        public OrderTracker OrderTracker { get { return _ordt; } }
        private BarListTracker _blt;
        public BarListTracker BarListTracker { get { return _blt; } }

        private QSMemoryDataManager _mdm = null;
        private QSFileDataManager _fileDM = null;
        public QSMemoryDataManager DataManager { get { return _mdm; } }
        

        private void debug(string msg)
        {
            if (SendDebugEvet != null)
                SendDebugEvet(msg);
        }
        public TradingTrackerCentre()
        {
            _pt = new PositionTracker();
            _ordt = new OrderTracker();
            _blt = new BarListTracker(new BarInterval[] {BarInterval.Minute,BarInterval.FiveMin,BarInterval.FifteenMin,BarInterval.ThirtyMin});
            
            //本地历史文件
            //读取本地历史数据并向服务器更新最新数据 形成有效的数据集
            _fileDM = new QSFileDataManager("d:\\data\\");
            _fileDM.SendDebugEvent +=new DebugDelegate(debug);
            _mdm = new QSMemoryDataManager(_fileDM);
            _mdm.SendDebugEvent +=new DebugDelegate(debug);
            //得到本地历史数据之后,我们需要向服务端发送更新命令。当有新的tick或者历史数据回补时,所有的数据均会发送到memoryDataManager
        }
        /*
        private void InitDataManager()
        {
            
            //读取本地历史数据并向服务器更新最新数据 形成有效的数据集
            _fileDM = new MStockDataManager("d:\\data\\");
            _mdm = new MemoryDataManager(_fileDM);
            //得到本地历史数据之后,我们需要向服务端发送更新命令。当有新的tick或者历史数据回补时,所有的数据均会发送到memoryDataManager
        }
        */


        public void setPositoinAccount(string acc)
        {
            _pt.DefaultAccount=acc;
            
        }
        public Position getPosition(string symbol)
        {
            debug("TradingTracker Got a position");
            return _pt[symbol];
        }
        public BarList getBarlist(string symbol)
        {
            return _blt[symbol];
        }

        public void Reset()
        {
            _pt.Clear();
            _ordt.Clear();
            _blt.Reset();
        }
        public void GotTick(Tick k)
        {
            _blt.GotTick(k);//更新bar数据
            _pt.GotTick(k);//更新position的最新价格 浮动盈亏等与行情相关的数据
            Bar b = BarListTracker[k.symbol].RecentBar;
            //将该bar封装成DataPackage发布到MemoryDataManager中去,这样Chart就会自动更新
            _mdm.AddNewPacket(new QSDataPacket(b, (double)_pt[k.symbol].Size, (double)_pt[k.symbol].AvgPrice));
        }

        public void GotOrder(Order o)
        {
            debug("TradingTracker Got an order");
            _ordt.GotOrder(o);//记录order
        }
        public void GotCancel(long id)
        {
            debug("TradingTracker Got an ordercancel");
            _ordt.GotCancel(id);//取消order
        }
        public void GotFill(Trade f)
        {
            debug("TradingTracker Got a trade");
            
            _ordt.GotFill(f);//orderbook记录成交
            _pt.GotFill(f);//仓位管理记录成交计算仓位变化
            //debug(getPosition("IF1210").ToString());
            _mdm.GotTrade(f);
        }
        public void GotPosition(Position pos)
        {
            debug("TradingTracker Got a position");
            _pt.GotPosition(pos);//系统启动时候对于服务器端保存的仓位数据进行初始化
        }

        public void GotBarResponse(Bar b)
        {
            _mdm.AddNewPacket(new QSDataPacket(b));
        }

        //关于历史数据的保存 这里我们需要深入研究K线生成以及数据有效保存
        //这里攻克完毕后软件便计入比较完善的状态
        public void Stop()
        {
            debug("保存历史数据,保存所有Symbol的K线数据");
            _mdm.SaveData();
            _fileDM.UpdateMaster();
            //_fileDM.SaveData("IF1211",_mdm.GetData("IF1211",int.MaxValue));

        }

    }
}
