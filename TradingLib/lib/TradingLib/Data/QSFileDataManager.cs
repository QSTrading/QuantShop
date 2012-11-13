using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Collections;
using System.Threading;
using Easychart.Finance.DataProvider;
using TradeLink.API;
using Easychart.Finance;

namespace TradingLib.Data
{
    public class QSFileDataManager : DataManagerBase
    {
        //数据管理组件用于从本地文件读取或者存储历史数据,当用memory封装时,memory会缓存历史数据,只有第一次加载该数据的时候才会读取本地文件加载历史数据
        // Fields
        //所有的K线历史数据均保存为1min数据,其他频率的数据利用电脑实时生成
        public event DebugDelegate SendDebugEvent;
        private int Fields = 7;
        private string FilePath;//记录历史数据文件夹目录
        private SymbolMaster[] Masters;
        private MetaStockTimeFrame TimeFrame = MetaStockTimeFrame.Intraday;//历史数据时间频率

        // 初始化datamanager加载master 读取历史数据文件与symbol的映射关系
        public QSFileDataManager(string FilePath)
        {
            FilePath = Path.GetDirectoryName(FilePath);
            if (!FilePath.EndsWith(@"\"))
            {
                FilePath = FilePath + @"\";
            }
            this.FilePath = FilePath;

            if (!File.Exists(FilePath + "MASTER"))
            {
                throw new Exception("MetaStock Path Not Found!" + FilePath + "MASTER");
            }
            this.LoadMaster();
        }

        private void debug(string s)
        {
            if (SendDebugEvent != null)
            {
                SendDebugEvent(s);
            }
        }





        //通过number或者symbol查询对应的master对象
        public SymbolMaster FindByNumber(string Number)
        {
            foreach (SymbolMaster master in this.Masters)
            {
                if (("F" + master.FN) == Number)
                {
                    return master;
                }
            }
            return null;
        }

        public SymbolMaster FindBySymbol(string Symbol)
        {

            if (this.Masters != null)
            {
                foreach (SymbolMaster master in this.Masters)
                {
                    if (master.Symbol == Symbol)
                    {
                        return master;
                    }
                }
            }
            return null;
        }


        #region 历史数据读写
        //从本地文件读取历史数据
        public override IDataProvider GetData(string Code, int Count)
        {
            debug("MStockDataManager load data:"+Count.ToString());
            this.Fields = 8;
            QSCommonDataProvider cdp = new QSCommonDataProvider(this);
            cdp.SendDebugEvent +=new DebugDelegate(debug);
            string path = this.LookupDataFile(Code);
            if ((path != "") && File.Exists(path))
            {
                using (FileStream stream = this.ReadData(path))
                {
                    byte[] buffer = new byte[this.Fields * 4];
                    byte[] buffer2 = new byte[stream.Length - buffer.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    stream.Read(buffer2, 0, buffer2.Length);
                    float[] dst = new float[buffer2.Length / 4];
                    Buffer.BlockCopy(buffer2, 0, dst, 0, buffer2.Length);
                    this.fmsbin2ieee(dst);
                    int num = dst.Length / this.Fields;
                    double[] numArray2 = new double[num];
                    double[] numArray3 = new double[num];
                    double[] numArray4 = new double[num];
                    double[] numArray5 = new double[num];
                    double[] numArray6 = new double[num];
                    double[] numArray7 = new double[num];
                    double[] numArray8 = new double[num];
                    if (this.Fields == 5)
                    {
                        numArray3 = numArray6;
                        numArray8 = numArray6;
                    }
                    for (int i = 0; i < num; i++)
                    {
                        int num3 = (int)dst[i * this.Fields];
                        //debug("num3 is:" + num3.ToString());
                        DateTime time = new DateTime((num3 / 0x2710) + 0x76c, (num3 / 100) % 100, num3 % 100);
                        int num4 = 0;
                        if ((this.Fields == 8) || (this.TimeFrame == MetaStockTimeFrame.Intraday))
                        {
                            int num5 = (int)dst[(i * this.Fields) + 1];
                            time += new TimeSpan(num5 / 0x2710, (num5 / 100) % 100, num5 % 100);
                            num4 = 1;
                        }
                        numArray2[i] = time.ToOADate();
                        if (this.Fields >= 6)
                        {
                            numArray3[i] = dst[((i * this.Fields) + 1) + num4];
                            numArray4[i] = dst[((i * this.Fields) + 2) + num4];
                            numArray5[i] = dst[((i * this.Fields) + 3) + num4];
                            numArray6[i] = dst[((i * this.Fields) + 4) + num4];
                            numArray7[i] = dst[((i * this.Fields) + 5) + num4];
                            numArray8[i] = numArray6[i];
                        }
                        else
                        {
                            numArray4[i] = dst[(i * this.Fields) + 1];
                            numArray5[i] = dst[(i * this.Fields) + 2];
                            numArray6[i] = dst[(i * this.Fields) + 3];
                            numArray7[i] = dst[(i * this.Fields) + 4];
                        }
                    }
                    //debug("data in files:"+numArray3.Length.ToString());
                    cdp.LoadBinary(new double[][] { numArray3, numArray4, numArray5, numArray6, numArray7, numArray2, numArray8 });
                    return cdp;
                }
            }
            cdp.LoadByteBinary(new byte[0]);
            return cdp;
        }
        //数据读写完毕需要更新Master表 用于记录历史数据的更新截至时间
        public void UpdateMaster()
        {
            this.SaveMaster();
        }
        //将CDP中的历史数据保存
        public void SaveData(string symbol, IDataProvider idp)
        {
            this.SaveData(symbol, idp,null,DateTime.Now,DateTime.Now ,true);
        }
        public override void SaveData(string Symbol, IDataProvider idp, Stream OutStream, DateTime Start, DateTime End, bool Intraday)
        {
            debug("Saving Data");
            QSCommonDataProvider cdp = (QSCommonDataProvider)idp;
            if ((Symbol != null) && (Symbol != ""))
            {

                int count = cdp.Count;
                SymbolMaster master2 = this.FindBySymbol(Symbol);
                bool flag = false;

                this.Fields = 8;// (byte)(7 + (Intraday ? 1 : 0));
                if ((master2 == null))
                {
                    int num2 = this.MaxNum + 1;
                    master2 = new SymbolMaster();
                    master2.FN = (byte)num2;
                    ArrayList list2 = new ArrayList(this.Masters);
                    master2.Symbol = Symbol;
                    master2.Fields = (byte)this.Fields;
                    master2.IssueName = cdp.GetStringData("Name");
                    master2.Country = "";
                    master2.Exchange = "";
                    if (master2.IssueName == null)
                    {
                        master2.IssueName = Symbol;
                    }
                    list2.Add(master2);
                    this.Masters = (SymbolMaster[])list2.ToArray(typeof(SymbolMaster));
                    flag = true;

                }
                double[] numArray = cdp["DATE"];
                double[] numArray2 = cdp["OPEN"];
                double[] numArray3 = cdp["HIGH"];
                double[] numArray4 = cdp["LOW"];
                double[] numArray5 = cdp["CLOSE"];
                double[] numArray6 = cdp["VOLUME"];
                double[] numArray7 = cdp["ADJCLOSE"];
                float[] ff = new float[count * this.Fields];
                for (int i = 0; i < count; i++)
                {
                    int num4 = 0;
                    DateTime time = DateTime.FromOADate(numArray[i]);
                    ff[(i * this.Fields) + num4] = (((time.Year - 0x76c) * 0x2710) + (time.Month * 100)) + time.Day;
                    if (this.Fields == 8)
                    {
                        num4 = 1;
                        ff[(i * this.Fields) + num4] = ((time.Hour * 0x2710) + (time.Minute * 100)) + time.Second;
                    }
                    //debug("Field num:"+this.Fields.ToString()+" Save time:" + ff[(i * this.Fields) + 0].ToString());
                    ff[((i * this.Fields) + 1) + num4] = (float)numArray2[i];
                    ff[((i * this.Fields) + 2) + num4] = (float)numArray3[i];
                    ff[((i * this.Fields) + 3) + num4] = (float)numArray4[i];
                    ff[((i * this.Fields) + 4) + num4] = (float)numArray5[i];
                    ff[((i * this.Fields) + 5) + num4] = (float)numArray6[i];
                    ff[((i * this.Fields) + 6) + num4] = (float)numArray5[i];
                }
                this.fieee2msbin(ff);
                byte[] dst = new byte[ff.Length * 4];
                Buffer.BlockCopy(ff, 0, dst, 0, dst.Length);
                using (FileStream stream = File.Create(this.LookupDataFile(Symbol)))
                {
                    stream.Write(dst, 0, dst.Length);
                    master2.LastDate = (float)numArray[0];
                }
                if (flag)
                {
                    this.SaveMaster();
                }

            }
        }


        #endregion
        private byte[] GetEmptyByteArray(int Count, byte Fill)
        {
            byte[] buffer = new byte[Count];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Fill;
            }
            return buffer;
        }



        #region symbol操作
        public override int DeleteSymbols(string Exchange, string[] Symbols, bool Remain, bool DeleteRealtime, bool DeleteHistorical)
        {
            ArrayList list = new ArrayList();
            int num = 0;
            if (this.Masters != null)
            {
                list.AddRange(this.Masters);
                int index = 0;
                while (index < list.Count)
                {
                    //查询master中是否有对应的symbols[]中的symbols
                    if (Array.IndexOf(Symbols, (list[index] as Master).symbol) >= 0)
                    {
                        list.RemoveAt(index);
                        num++;
                    }
                    else
                    {
                        index++;
                    }
                }
                if (num > 0)
                {
                    this.Masters = (SymbolMaster[])list.ToArray(typeof(SymbolMaster));
                    this.SaveMaster();
                }
            }
            return num;
        }

        // Properties
        public override DataTable Exchanges
        {
            get
            {
                DataTable table = new DataTable();
                table.Columns.Add("Value");
                table.Columns.Add("Text");
                table.Rows.Add(new object[] { "", "ALL" });
                return table;
            }
        }

        public override DataTable GetStockList(string Exchange, string ConditionId, string Key, string Sort, int StartRecords, int MaxRecords)
        {
            return base.RecordRange(this.GetTable(), StartRecords, MaxRecords);
        }

        public string GetSymbol(string Number)
        {
            SymbolMaster master = this.FindByNumber(Number);
            if (master == null)
            {
                return "";
            }
            return master.Symbol.Trim();
        }

        public override int SymbolCount(string Exchange, string ConditionId, string Key)
        {
            return this.GetTable().Rows.Count;
        }
        public override DataTable GetSymbolList(string Exchange, string ConditionId, string Key, string Sort, int StartRecords, int MaxRecords)
        {
            return base.RecordRange(this.GetTable(), StartRecords, MaxRecords);
        }
        //通过master映射文件形成SymbolList Table
        
        public DataTable GetTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Symbol");
            table.Columns.Add("Name");
            table.Columns.Add("Country");
            table.Columns.Add("Exchange");
            table.Columns.Add("Start");
            table.Columns.Add("End");
            table.Columns.Add("FN");

            if (this.Masters != null)
            {
                foreach (SymbolMaster master in this.Masters)
                {
                    table.Rows.Add(new object[] { master.Symbol, master.IssueName, master.Country, master.Exchange, master.FirstDate,DateTime.FromOADate(master.LastDate),master.FN });
                }
            }
            return table;
        }
        #endregion


        //master的读写操作
        public void LoadMaster()
        {
            using (FileStream stream = this.ReadData(this.FilePath + "MASTER"))
            {
                //stream长度除以 53 得到最后的数值 为我们需要建立list的长度
                this.Masters = new SymbolMaster[(stream.Length / 76)];
                debug("master length:" + stream.Length.ToString());
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    //reader.ReadBytes(0x35);
                    //默认先读取53byte的数据
                    int index = 0;
                    do
                    {
                        Masters[index] = new SymbolMaster();
                        Masters[index].Symbol = Encoding.ASCII.GetString(reader.ReadBytes(15)).Trim();
                        Masters[index].IssueName = Encoding.ASCII.GetString(reader.ReadBytes(30)).Trim();
                        Masters[index].FirstDate = reader.ReadSingle();
                        Masters[index].LastDate = reader.ReadSingle();
                        Masters[index].Country = Encoding.ASCII.GetString(reader.ReadBytes(10)).Trim();
                        Masters[index].Exchange = Encoding.ASCII.GetString(reader.ReadBytes(10)).Trim();
                        Masters[index].Fields = reader.ReadByte();
                        Masters[index].FN = reader.ReadInt16();

                        index++;
                    }
                    while (index < this.Masters.Length);
                }
            }
        }



        private void SaveMaster()
        {
            using (FileStream stream = File.Create(this.FilePath + "MASTER"))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    foreach (SymbolMaster master in this.Masters)
                    {
                        writer.Write(StringToBytes(master.Symbol, 15, 0x20));
                        if ((master.IssueName == null) || (master.IssueName == ""))
                            master.IssueName = master.Symbol;
                        writer.Write(StringToBytes(master.IssueName, 30, 0x20));
                        writer.Write(master.FirstDate);
                        writer.Write(master.LastDate);
                        writer.Write(StringToBytes(master.Country, 10, 0x20));
                        writer.Write(StringToBytes(master.Exchange, 10, 0x20));
                        writer.Write((byte)master.Fields);
                        writer.Write(master.FN);
                    }
                }
            }
        }



        //获得对应的历史数据文件名

        public string LookupDataFile(string Code)
        {
            foreach (SymbolMaster master in this.Masters)
            {
                if (string.Compare(master.Symbol, Code, true) == 0)
                {

                    return string.Concat(new object[] { this.FilePath, "F", master.FN, ".DAT" });
                }
            }
            return "";

        }


        //内部私有函数定义
        private FileStream ReadData(string Filename)
        {
            for (int i = 5; i >= 0; i--)
            {
                try
                {
                    return new FileStream(Filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                }
                catch
                {
                    if (i == 0)
                    {
                        throw;
                    }
                    Thread.Sleep(100);
                }
            }
            return null;
        }

        private byte[] StringToBytes(string s, int Count, byte Fill)
        {
            byte[] emptyByteArray = this.GetEmptyByteArray(Count, Fill);
            Encoding.ASCII.GetBytes(s, 0, s.Length, emptyByteArray, 0);
            return emptyByteArray;
        }

        private string TrimToZero(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '\0')
                {
                    return s.Substring(0, i);
                }
            }
            return s;
        }

        private int MaxNum
        {
            get
            {
                int fn = 1;
                if (this.Masters != null)
                {
                    foreach (SymbolMaster master in this.Masters)
                    {
                        if (fn < master.FN)
                        {
                            fn = master.FN;
                        }
                    }
                }

                return fn;
            }
        }

        private void fmsbin2ieee(float[] ff)
        {
            uint[] dst = new uint[ff.Length];
            Buffer.BlockCopy(ff, 0, dst, 0, ff.Length * 4);
            for (int i = 0; i < ff.Length; i++)
            {
                if (dst[i] != 0)
                {
                    uint num = dst[i] >> 0x10;
                    uint num2 = (num & 0xff00) - 0x200;
                    num = (num & 0x7f) | ((num << 8) & 0x8000);
                    num |= num2 >> 1;
                    dst[i] = (dst[i] & 0xffff) | (num << 0x10);
                }
            }
            Buffer.BlockCopy(dst, 0, ff, 0, ff.Length * 4);
        }
        private void fieee2msbin(float[] ff)
        {
            uint[] dst = new uint[ff.Length];
            Buffer.BlockCopy(ff, 0, dst, 0, ff.Length * 4);
            for (int i = 0; i < ff.Length; i++)
            {
                if (dst[i] != 0)
                {
                    uint num = dst[i] >> 0x10;
                    uint num2 = ((num << 1) & 0xff00) + 0x200;
                    if ((num2 & 0x8000) == ((num << 1) & 0x8000))
                    {
                        num = (num & 0x7f) | ((num >> 8) & 0x80);
                        num |= num2;
                        dst[i] = (dst[i] & 0xffff) | (num << 0x10);
                    }
                }
            }
            Buffer.BlockCopy(dst, 0, ff, 0, ff.Length * 4);
        }



       


    }






}
