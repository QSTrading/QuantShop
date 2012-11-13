using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using Easychart.Finance;
using Easychart.Finance.DataProvider;
using TradeLink.API;
using TradeLink.Common;
namespace TradingLib.Data
{
    //用于向图标提供数据集封装了commondataprovider 并且集成了我们需要的其他数据
    //如何向我们的数据集集成我们本地的一些数据
    //1.QS特殊指标数据集成
    //2.交易数据集成 比如我们进行的交易如何输出到图表
    public class QSCommonDataProvider : CommonDataProvider
    {
        public event DebugDelegate SendDebugEvent;//发送日志信息
        void debug(string msg)
        {
            if (SendDebugEvent != null)
                SendDebugEvent(msg);
        }

        //method public hidebysig newslot specialname virtual
        //  instance float64[]  get_Item(string Name) cil managed

        public static QSCommonDataProvider EmptyCDP{
            get {
                QSCommonDataProvider provider = new QSCommonDataProvider(null);
                provider.LoadByteBinary(new byte[0]);
                return provider;

            }
        }
        //method public hidebysig newslot virtual   method public hidebysig newslot specialname virtual
        public QSCommonDataProvider(IDataManager dm)
            : base(dm)
        {
            DataKeys = new string[] { "DATE", "OPEN", "HIGH", "LOW", "CLOSE", "VOLUME", "ADJCLOSE" ,"OI","XPRICE","XSIZE","POS","AVGPRICE"};
            //DataKeys = new string[] { "DATE", "OPEN", "HIGH", "LOW", "CLOSE", "VOLUME", "OI" };//, "OI" };//,"XPRICE","XSIZE"};
        
        }
        //新增数据的方法:空CDP需要添加对应的列，从本地文件加载数据也需要添加对应的列

        //自定义数据集合 为图标提供交易信息 如何有效的获得自定义数据???
        public Hashtable htTradeData;

        public string[] tradeKeys = { "XSIZE", "XPRICE" };

        public string[] DataKeys;
        //provider向chart提供数据服务的入口
        public override double[] this[string Name]
        {
            get
            {
    
                return this.GetData(Name);
               
            }
        }

        //当系统有成交时候 我们将成交信息记录到我们对应的图标上去
        public void GotTrade(Trade f)
        {
            /*
            ArrayList[] listArray = new ArrayList[DataKeys.Length];//新建一个list将本地对应的关键字数据集复制到该list
            for (int i = 0; i < listArray.Length; i++)
            {
                listArray[i] = new ArrayList();
                listArray[i].AddRange((double[])htData[DataKeys[i]]);
                debug("listArray:" + i.ToString() + "  num:" + listArray[i].Count.ToString());
            }
            double ftime =  Util.ToDateTime(f.xdate,f.xtime).ToOADate();
             //debug("数据集数目:"+listArray[0].Count.ToString());
                for (int j = 0; j <= listArray[0].Count; j++)//循环list
                {
                    //debug("循环:"+j.ToString());
                    if (j < listArray[0].Count) //
                    {
                        if ((double)listArray[0][j] <  ftime) //如果进来的数据集的时间大于本地数据时间,则直接数据从0-N 递增,直到时间大于dp时间插入数据 等于dp时间 更新数据 或者达到数据最末端(直接添加)
                        {
                            debug("时间大于");
                            goto Label_0130;
                        }
                        //进来的数据集时间小于记录时间 则我们插入该dbpackage 即回补以前的历史数据,当回补时候找到某个时间点大于dp中的事件点我们就插入并且break出循环
                        if ((double)listArray[0][j] > ftime)
                        {
                            debug("时间小于："+((double)listArray[0][j]).ToString() +"___tradetime:"+ftime.ToString() );
                            //时间
                            listArray[0].Insert(j, ftime);
                            //1-8k线数据
                            for (int m = 1; m < 8; m++)
                            {
                                listArray[m].Insert(j,0.0D);
                            }
                            //XPRICE XSIZE数据
                            listArray[8].Insert(j, (double)f.xsize);
                            listArray[9].Insert(j, (double)f.xprice);
                            
                        }
                        else
                        {
                            debug("时间等于");
                            double _size = (double)listArray[8][j];
                            double _price = (double)listArray[9][j];
                            listArray[8][j] = (double)(_size+(double)f.xsize);
                            listArray[9][j] = (double)((_size*_price + (double)(f.xsize*f.xprice))/((double)listArray[8][j]));
                          
                        }
                    }
                    else
                    {
                        debug("插入数据");
                        listArray[0].Add(ftime);
                        //1-8k线数据
                        for (int m = 1; m < 8; m++)
                        {
                            listArray[m].Add(0.0D);
                        }
                        //XPRICE XSIZE数据
                        listArray[8].Add((double)f.xsize);
                        listArray[9].Add((double)f.xprice);
                    }
                    break;
                Label_0130: ;
                }
                this.htData.Clear();
                for (int j = 0; j < DataKeys.Length; j++)
                {
                    debug("listArray:" + j.ToString() + "  num:"+listArray[j].Count.ToString());
                    this.htData.Add(DataKeys[j], (double[])listArray[j].ToArray(typeof(double)));
                }
                debug(((double[])htData["XPRICE"])[1].ToString());
                this.htAllCycle.Clear();
             * */
            //成交类过程信息我们无法用通常megre(dp/cdp)方式来处理.当有tick数据进来 主动推送Bar数据我们可以同时
            //获得仓位状态信息。当时成交的生成并不是与tick数据同步的。有成交了我们就需要将它记录到当前Bar.不能通过内存adddatapacket的方式推送。
            //因此 仓位信息与成交信息的推送机理是不同的。
            int i = ((double[])htData["XSIZE"]).Length;
            debug("数据长度:" + ((double[])htData["XSIZE"]).Length.ToString()+" i:"+i.ToString()+" Count:"+Count.ToString());
           
            double _size = ((double[])htData["XSIZE"])[i - 1];
            double _price = ((double[])htData["XPRICE"])[Count - 1];
            ((double[])htData["XSIZE"])[i - 1] = (double)(_size + (double)f.xsize);
            ((double[])htData["XPRICE"])[i - 1] = (double)((_size * _price + (double)(f.xsize * f.xprice)) / (((double[])htData["XSIZE"])[i - 1]));
            //((double[])htData["POS"])[i - 1] = ((double[])htData["XSIZE"])[i - 1];
            debug("XSIZE:"+((double[])htData["XSIZE"])[i - 1].ToString());

            debug("访问得到的数据:"+this.GetData("XSIZE")[Count-1].ToString());
        }
        //通过key得到我们所需要的数据集
        public override double[] GetData(string DataType)
        {
            if (DataType == "XSIZE")
            { 
                debug("XSIZE in memory:"+((double [])htData["XSIZE"])[Count-1].ToString());
            }
            Hashtable cycleData = htData;//this.GetCycleData(this.DataCycle);
            if (cycleData == null)
            {
                throw new Exception(string.Concat(new object[] { "Quote data ", DataType, " ", this.DataCycle, " not found" }));
            }
            double[] dd = (double[])cycleData[DataType.ToUpper()];
            if (dd == null)
            {
                throw new Exception("The name " + DataType + " does not exist.");
            }
            if ((this.BaseDataProvider != null) && (this.BaseDataProvider != this))
            {
                dd = this.AdjustByBase((double[])cycleData["DATE"], dd);
            }
            if ((this.MaxCount == -1) || (dd.Length <= this.MaxCount))
            {
                return dd;
            }
            double[] numArray2 = new double[this.MaxCount];
            Array.Copy(dd, dd.Length - this.MaxCount, numArray2, 0, this.MaxCount);
            return numArray2;

        }
        //得到不同周期的数据集
        public override Hashtable GetCycleData(DataCycle dc)
        {
            if (((dc.CycleBase == DataCycleBase.DAY) && (dc.Repeat == 1)) && !this.Adjusted)
            {
                return this.htData;
            }
            dc.WeekAdjust = this.weekAdjust;
            Hashtable hashtable = (Hashtable)this.htAllCycle[dc.ToString()];
            if (hashtable == null)
            {
                if (this.htData == null)
                {
                    return this.htData;
                }
                Hashtable htData = this.htData;
                if (this.intradayInfo != null)
                {
                    htData = this.DoExpandMinute(htData);
                }
                if (this.futureBars != 0)
                {
                    htData = this.ExpandFutureBars(htData);
                }
                if (htData["CLOSE"] != null)
                {
                    if (htData["OPEN"] == null)
                    {
                        htData["OPEN"]=  htData["CLOSE"];
                    }
                    if (htData["HIGH"] == null)
                    {
                        htData["HIGH"]= htData["CLOSE"];
                    }
                    if (htData["LOW"] == null)
                    {
                        htData["LOW"] =  htData["CLOSE"];
                    }
                }
                double[] oDATE = (double[])htData["DATE"];
                if (oDATE == null)
                {
                    return null;
                }
                int[] nEWDATE = new int[oDATE.Length];
                int num = -2147483648;
                int num2 = -1;
                for (int i = 0; i < oDATE.Length; i++)
                {
                    int sequence;
                    if (this.DataCycle.CycleBase == DataCycleBase.TICK)
                    {
                        sequence = i;
                    }
                    else
                    {
                        sequence = this.DataCycle.GetSequence(oDATE[i]);
                    }
                    if (sequence > num)
                    {
                        num2++;
                    }
                    nEWDATE[i] = num2;
                    num = sequence;
                }
                hashtable = new Hashtable();
                foreach (string str in DataKeys)
                {
                    hashtable[str]= new double[num2 + 1];
                }
                bool flag = (this.Adjusted && (htData["ADJCLOSE"] != null)) && (htData["CLOSE"] != null);
                double[] cLOSE = (double[])htData["CLOSE"];
                double[] aDJCLOSE = (double[])htData["ADJCLOSE"];
                //将所有的数据集进行周期化转换
                //不同的数据类型有不同的周期转换方式
                foreach (string str2 in DataKeys)
                {
                    MergeCycleType dateMergeType;
                    bool doAdjust = flag;
                    doAdjust = false;
                    switch (str2)
                    {
                        case "DATE":
                            dateMergeType = this.dateMergeType;
                            break;

                        case "VOLUME":
                        case "AMOUNT":
                            dateMergeType = MergeCycleType.SUM;
                            break;
                        case "OI":
                            dateMergeType = MergeCycleType.CLOSE;
                            break;
                        case "XSIZE":
                            dateMergeType = MergeCycleType.CLOSE;
                            break;
                        default:
                            try
                            {
                                dateMergeType = (MergeCycleType)Enum.Parse(typeof(MergeCycleType), str2);
                                doAdjust = true;
                            }
                            catch
                            {
                                dateMergeType = MergeCycleType.CLOSE;
                            }
                            break;
                    }
                    this.MergeCycle(oDATE, nEWDATE, cLOSE, aDJCLOSE, (double[])htData[str2], (double[])hashtable[str2], dateMergeType, doAdjust);
                }
                this.htAllCycle[dc.ToString()] = hashtable;
            }
            return hashtable;

        }


        //数据转换周期
        public override void MergeCycle(double[] ODATE, int[] NEWDATE, double[] CLOSE, double[] ADJCLOSE, double[] ht, double[] htCycle, MergeCycleType mct, bool DoAdjust)
        {
            int num = -1;
            int index = -1;
            for (int i = 0; i < ODATE.Length; i++)
            {
                double num4 = 1.0;
                if (DoAdjust && (ADJCLOSE != null))
                {
                    num4 = ADJCLOSE[i] / CLOSE[i];
                }
                double num5 = ht[i] * num4;
                if (num4 != 1.0)
                {
                    num5 = Math.Round(num5, 2);
                }
                if (num != NEWDATE[i])
                {
                    index++;
                    htCycle[index] = num5;
                }
                else if (!double.IsNaN(num5))
                {
                    if (mct == MergeCycleType.HIGH)
                    {
                        htCycle[index] = this.Max(htCycle[index], num5);
                    }
                    else if (mct == MergeCycleType.LOW)
                    {
                        htCycle[index] = this.Min(htCycle[index], num5);
                    }
                    else if (mct == MergeCycleType.CLOSE)
                    {
                        htCycle[index] = num5;
                    }
                    else if (mct == MergeCycleType.ADJCLOSE)
                    {
                        htCycle[index] = ht[i];
                    }
                    else if (mct == MergeCycleType.OPEN)
                    {
                        htCycle[index] = this.First(htCycle[index], num5);
                    }
                    else
                    {
                        htCycle[index] = this.Sum(htCycle[index], num5);
                    }
                }
                num = NEWDATE[i];
            }

        }


        //本地文件历史数据->原始二维数组->加载数据集
        public override void LoadBinary(double[][] ds)
        {
            if (ds.Length > 4)
            {
                this.htData.Clear();
                this.htData.Add("OPEN", ds[0]);
                this.htData.Add("HIGH", ds[1]);
                this.htData.Add("LOW", ds[2]);
                this.htData.Add("CLOSE", ds[3]);
                this.htData.Add("VOLUME", ds[4]);
                this.htData.Add("DATE", ds[5]);
                if (ds.Length > 6)
                {
                    this.htData.Add("OI", ds[0]);
                    this.htData.Add("XPRICE",new double[ds[5].Length]);
                    this.htData.Add("XSIZE", new double[ds[5].Length]);
                    this.htData.Add("POS", new double[ds[5].Length]);
                    this.htData.Add("AVGPRICE", new double[ds[5].Length]);
                    this.htData.Add("ADJCLOSE", ds[6]);
                }
                else
                {
                    double[] numArray = new double[ds[0].Length];
                    Buffer.BlockCopy(ds[3], 0, numArray, 0, ds[0].Length * 8);
                    this.htData.Add("ADJCLOSE", numArray);
                }
            }
            this.htAllCycle.Clear();

        }

        public override void LoadByteBinary(byte[] bs)
        {

            //base.LoadByteBinary(bs);
            //debug("Size:" + QSDataPacket.PacketByteSize.ToString());
            LoadBinary(bs, bs.Length /QSDataPacket.PacketByteSize);

        }
        
        public override void LoadBinary(byte[] bs, int N)
        {
            this.htData.Clear();
            double[] numArray = new double[N];
            double[] numArray2 = new double[N];
            double[] numArray3 = new double[N];
            double[] numArray4 = new double[N];
            double[] numArray5 = new double[N];
            double[] numArray6 = new double[N];
            double[] numArray7 = new double[N];

            //自定义数据信息
            double[] numArray10 = new double[N];
            double[] numArray11 = new double[N];
            double[] numArray12 = new double[N];
            double[] numArray13 = new double[N];
            double[] numArray14 = new double[N];

            float[] numArray8 = new float[N * DataPacket.PacketSize];
            Buffer.BlockCopy(bs, 0, numArray8, 0, bs.Length);
            for (int i = 0; i < N; i++)
            {
                Buffer.BlockCopy(numArray8, i * DataPacket.PacketByteSize, numArray6, i * 8, 8);
                numArray[i] = numArray8[(i * DataPacket.PacketSize) + 5];
                numArray2[i] = numArray8[(i * DataPacket.PacketSize) + 2];
                if (numArray2[i] == 0.0)
                {
                    numArray2[i] = numArray[i];
                }
                numArray3[i] = numArray8[(i * DataPacket.PacketSize) + 3];
                if (numArray3[i] == 0.0)
                {
                    numArray3[i] = numArray[i];
                }
                numArray4[i] = numArray8[(i * DataPacket.PacketSize) + 4];
                if (numArray4[i] == 0.0)
                {
                    numArray4[i] = numArray[i];
                }
                Buffer.BlockCopy(numArray8, ((i * DataPacket.PacketSize) + 6) * 4, numArray5, i * 8, 8);
                numArray7[i] = numArray8[(i * DataPacket.PacketSize) + 8];
            }
            this.htData.Add("CLOSE", numArray);
            this.htData.Add("OPEN", numArray2);
            this.htData.Add("HIGH", numArray3);
            this.htData.Add("LOW", numArray4);
            this.htData.Add("VOLUME", numArray5);
            this.htData.Add("DATE", numArray6);
            this.htData.Add("ADJCLOSE", numArray7);

            //加载自定义数据信息
            this.htData.Add("OI", numArray10);
            this.htData.Add("XPRICE", numArray11);
            this.htData.Add("XSIZE", numArray12);
            this.htData.Add("POS", numArray12);
            this.htData.Add("AVGPRICE", numArray13);
            this.htAllCycle.Clear();

        }


        //回补合成cdp
        public override void Merge(CommonDataProvider cdp)
        {
            debug("crack run here Merge dataprovider");
            ArrayList[] listArray = new ArrayList[DataKeys.Length];
            ArrayList[] listArray2 = new ArrayList[DataKeys.Length];
            for (int i = 0; i < listArray.Length; i++)
            {
                listArray[i] = new ArrayList();
                listArray[i].AddRange((double[])this.htData[DataKeys[i]]);
                listArray2[i] = new ArrayList();
                listArray2[i].AddRange((double[])cdp.htData[DataKeys[i]]);
            }
            int index = 0;
            int num3 = 0;
            while (num3 < listArray2[0].Count)
            {
                if (index < listArray[0].Count)
                {
                    if (((double)listArray[0][index]) < ((double)listArray2[0][num3]))//新数据事件大于 内存数据事件 序号递增
                    {
                        index++;
                    }
                    else if (((double)listArray[0][index]) >= ((double)listArray2[0][num3]))
                    {
                        if (((double)listArray[0][index]) > ((double)listArray2[0][num3]))//新数据事件小于 内存数据事件 回补该数据
                        {
                            for (int k = 0; k < DataKeys.Length; k++)
                            {
                                listArray[k].Insert(index, listArray2[k][num3]);
                            }
                        }
                        else
                        {
                            //数据赋值操作要区分过程类与状态类,状态类数据可以更新赋值,过程类数据不能进行赋值覆盖
                            for (int m = 1; m < 8; m++)//新数据事件等于 内存数据事件 赋值 
                            {
                                listArray[m][index] = listArray2[m][num3];
                            }
                            //8 XSIZE
                            //9 XPRICE
                            listArray[10][index] = listArray2[10][num3];
                            listArray[11][index] = listArray2[11][num3];
                        }
                        index++;
                        num3++;
                    }
                }
                else
                {
                    for (int n = num3; n < listArray2[0].Count; n++)
                    {
                        for (int num7 = 0; num7 < DataKeys.Length; num7++)
                        {
                            listArray[num7].Add(listArray2[num7][n]);
                        }
                    }
                    break;
                }
            }
            this.htData.Clear();
            for (int j = 0; j < DataKeys.Length; j++)
            {
                this.htData.Add(DataKeys[j], (double[])listArray[j].ToArray(typeof(double)));
            }
            this.htAllCycle.Clear();


            //debug("Merge data number:" + (listArray[0].ToArray(typeof(double))).Length.ToString());
            //base.Merge(cdp);

        }
        //实时K线数据
        //从服务器TLTracker回补数据的时候是先得到最近的数据然后再得到以前的数据
        //数据时按照时间排列 从0-N来进行排列 listArray[0]是最早的数据
        public  void Merge(QSDataPacket dp)
        {
            //debug("htdata column:"+htData.Keys.Count.ToString());
            //debug("CDP得到新数据："+dp.DoubleDate.ToString()+","+" open"+dp.Open.ToString()+" high:"+dp.High.ToString()+" low:"+dp.Low.ToString()+" close:"+dp.Close.ToString());
            if ((dp != null) && !dp.IsZeroValue)
            {
                ArrayList[] listArray = new ArrayList[DataKeys.Length];//新建一个list将本地对应的关键字数据集复制到该list
                for (int i = 0; i < listArray.Length; i++)
                {
                    listArray[i] = new ArrayList();
                    listArray[i].AddRange((double[])this.htData[DataKeys[i]]);
                }
                //debug("数据集数目:"+listArray[0].Count.ToString());
                for (int j = 0; j <= listArray[0].Count; j++)//循环list
                {
                    //debug("循环:"+j.ToString());
                    if (j < listArray[0].Count) //
                    {
                        if ((double)listArray[0][j] < dp.DoubleDate) //如果进来的数据集的时间大于本地数据时间,则直接数据从0-N 递增,直到时间大于dp时间插入数据 等于dp时间 更新数据 或者达到数据最末端(直接添加)
                        {
                            //debug("时间大于：" + listArray[0][j].ToString() +" dp time"+dp.DoubleDate.ToString());
                            goto Label_0130;
                        }
                        //进来的数据集时间小于记录时间 则我们插入该dbpackage 即回补以前的历史数据,当回补时候找到某个时间点大于dp中的事件点我们就插入并且break出循环
                        if ((double)listArray[0][j] > dp.DoubleDate)
                        {
                            //debug("时间小于" + listArray[0][j].ToString() + " dp time" + dp.DoubleDate.ToString());
                            for (int m = 0; m < DataKeys.Length; m++)
                            {
                                listArray[m].Insert(j, dp[DataKeys[m]]);
                            }
                        }
                        else
                        {
                            //debug("时间等于" + listArray[0][j].ToString() + " dp time" + dp.DoubleDate.ToString());
                            for (int n = 1; n < 8; n++)//如果时间相等则我们直接赋值,如果是赋值我们不能对XPRICE XSIZE进行赋值 datapacket对size price是无状态的
                            {
                                listArray[n][j] = dp[DataKeys[n]];
                            }
                            listArray[10][j] = dp["POS"];
                            listArray[11][j] = dp["AVGPRICE"];

                        }
                    }
                    else
                    {
                        //debug("新增数据集");
                        for (int num5 = 0; num5 < DataKeys.Length; num5++)
                        {
                            listArray[num5].Add(dp[DataKeys[num5]]);
                        }
                    }
                    break;
                Label_0130: ;
                }
                this.htData.Clear();
                for (int k = 0; k < DataKeys.Length; k++)
                {
                    this.htData.Add(DataKeys[k], (double[])listArray[k].ToArray(typeof(double)));
                }
                this.htAllCycle.Clear();
            }
            
            //base.Merge(dp);
        }



        //清楚数据并重置
        public override void ClearData()
        {
            this.htData.Clear();
            for (int i = 0; i < DataKeys.Length; i++)
            {
                this.htData.Add(DataKeys[i], new double[0]);
            }
            this.htAllCycle.Clear();

        }


 

    }

}
