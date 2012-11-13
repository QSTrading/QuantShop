using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easychart.Finance.DataProvider;
using TradeLink.API;
using System.Collections;
using TradingLib.Data;

namespace TradingLib.Data
{
    public class QSMemoryDataManager : DataManagerBase
    {
        public event DebugDelegate SendDebugEvent;//发送日志信息
        // Fields
        private static Hashtable htHistorical = new Hashtable();//历史数据映射表
        private static Hashtable htStreaming = new Hashtable();//实时数据映射表
        private QSFileDataManager InnerDataManager;//本地历史数据管理器

        //构造 内存数据管理器的目的是与本地历史数据管理器相融合,动态的读取本地历史数据
        public QSMemoryDataManager(QSFileDataManager InnerDataManager)
        {
            this.InnerDataManager = InnerDataManager;
            //InnerDataManager.SendDebugEvent +=new DebugDelegate(debug);
        }
        void debug(string msg)
        {
            if (SendDebugEvent != null)
                SendDebugEvent(msg);
        }

        //状态类的数据通过datapackage推送到内存数据管理器,过程类的成交等信息则需要单独推送
        //同时我们默认成交信息标记在当前Bar 理论上存在成交在Bar1由于处理过程的时间延迟 当前为Bar2造成数据偏差(问题不严重)
        //
        public void GotTrade(Trade f)
        {
            debug("got a fill");
            string symbol = f.symbol;
            //我们先要得到内存管理器中的DataProvider然后进行对应的成交推送
            QSCommonDataProvider empty = GetData(symbol, int.MaxValue) as QSCommonDataProvider;
            /*
            QSCommonDataProvider empty = (QSCommonDataProvider)htHistorical[symbol];
            if (empty == null)
            {
                empty = QSCommonDataProvider.EmptyCDP;
                htStreaming[symbol] = empty;
                empty.SendDebugEvent += new DebugDelegate(debug);
            }
            //同时回补该条数据
             * */
            //得到当前数据集了我们进行更新 注意 我们需要保证没有2个线程同时访问memorydatamanager 从而线程安全
            empty.GotTrade(f);
        }
        //往数据集中新增一条数据 当有新的Tick产生时 更新MemoryDataManager
        public void AddNewPacket(QSDataPacket dp)
        {
            //debug("got new package");
            string symbol = dp.Symbol;
            //如果实时数据映射表中没有该实时数据provider则我们为该symbol新增一个
            QSCommonDataProvider empty = htStreaming[symbol] as QSCommonDataProvider;
            if (empty == null)
            {
                empty = QSCommonDataProvider.EmptyCDP;
                htStreaming[symbol] = empty;
                empty.SendDebugEvent +=new DebugDelegate(debug);
            }
            //同时合并该条数据
            empty.Merge(dp);
        }

        //获得数据
        //关于请求一段时间的历史数据 在历史数据回补中我们可以集中读取数据在request中比较开始与结束时间
        //然后决定是否发送该Bar 每次getData将清空实时数据 将实时数据与历史数据回补处理
        public override IDataProvider GetData(string Code, int Count)
        {
            //k线图每刷新一次,GetData就被调用一次 用于得到最新的数据
            //第一次加载数据的时候从映射表中获得历史数据,若数据集为null，则读取本地文件数据加载到内存
            //debug("IDataProvider GetData is called");
            QSCommonDataProvider empty = (QSCommonDataProvider)htHistorical[Code];
            if (empty == null)
            {
                if (this.InnerDataManager != null)
                {
                    this.InnerDataManager.StartTime = base.StartTime;
                    this.InnerDataManager.EndTime = base.EndTime;
                    empty = this.InnerDataManager[Code, Count] as QSCommonDataProvider;
                    empty.DataManager = this;
                    //debug("data loaded from files:" + ((double [])empty.htData["DATE"]).Length.ToString());
                    //empty.SendDebugEvent += new DebugDelegate(debug);
                }
                else
                {
                    empty = QSCommonDataProvider.EmptyCDP;
                }
                htHistorical[Code] = empty;
                //debug("data appened");
            }
            //同时从实时数据中获得数据与历史数据组合成完整的数据集合
            //每次图像更新的时候都要将实时数据与历史数据merge合成成最新的数据集用于显示 图像不刷新 数据时不会进行更新与合并操作的
            QSCommonDataProvider cdp = (QSCommonDataProvider)htStreaming[Code];
            if (cdp != null)
            {
                //debug("实时数据数目:" + ((double[])cdp.htData["DATE"]).Length.ToString());
                empty.Merge(cdp);
                cdp.ClearData();
            }
            //empty.dataCycle = new Easychart.Finance.DataCycle(Easychart.Finance.DataCycleBase.MINUTE, 1);//需要制定datacycle
            //debug("IDataprovider 数目:"+((double [])empty.htData["DATE"]).Length.ToString());
            //debug("IDataprovider 数目:" + (empty.Count.ToString()));
            return empty;
        }
        //删除某条数据集
        public void RemoveSymbol(string Symbol)
        {
            htHistorical.Remove(Symbol);
            htStreaming.Remove(Symbol);
        }
        /// <summary>
        /// 保存某个symbol的历史数据 目前保存历史数据需要将所有数据读取后然后再统一保存
        /// 以后我们需要设计成保存更新的数据,即程序启动会自动获得自上次保存数据以来的所有数据
        /// 在软件关闭的时候自动将本次动态更新得到的数据保存的本地历史数据文件中
        /// </summary>
        /// <param name="symbol"></param>
        public void SaveData(string symbol,bool updatemaster)
        {
            InnerDataManager.SaveData(symbol, this.GetData(symbol, int.MaxValue), true);
            // 近保存实时补充的数据
            //QSCommonDataProvider empty = htStreaming[symbol] as QSCommonDataProvider;
            /*
            if (empty != null)
            {
                empty = CommonDataProvider.Empty;
                htStreaming[symbol] = empty;
                InnerDataManager.SaveData(symbol,empty, true);
            }
            **/
            if (updatemaster)
                InnerDataManager.UpdateMaster();
        }

        /*
        public int StreamCount(string symbol)
        {
            CommonDataProvider empty = htStreaming[symbol] as CommonDataProvider;
            if (empty != null)
                return empty.Count;
            return 0;
        }
         * */
        //保存所有历史数据集到innerdatamanager 这里就是本地文件数据
        public void SaveData()
        { 
           //foreach(string symbol in  htStreaming.Keys)
            {
                SaveData("IF1211",false);
            }
            //统一更新master本地历史数据主表文件
            InnerDataManager.UpdateMaster();
        }
    }


}
