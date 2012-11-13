using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easychart.Finance;
using Easychart.Finance.DataProvider;
using Easychart.Finance.DataClient;
using TradeLink.Common;
using TradeLink.API;


namespace TradingLib
{
    public class ChartDataClient:DataClientBase
    {

            // Fields
    //private EventHandler OnFinished;
    //private DataProgress OnProgress;
    //private StreamingDataChanged OnStreamingData;
    //private EventHandler OnStreamingStopped;

    // Events
        public event DebugDelegate SendDebugEvent;
        public event EventHandler OnFinished;
        public event DataProgress OnProgress;
        public event StreamingDataChanged OnStreamingData;//当有实时数据到达时,我们进行的处理
        public event BarBackFilled OnBarBackFilled;
        public event EventHandler OnStreamingStopped;
    

    // Methods
    public ChartDataClient()
    {
        
    }

    private void debug(string s)
    {
        if (SendDebugEvent != null)
        {
            SendDebugEvent(s);
        }
    }
    //获得实时数据
    public override void DownloadStreaming()
    {
        try
        {
            /*
            HttpWebRequest request = WebRequest.Create("http://data.easychart.net/streaming.aspx?Symbols=" + base.GetStreamingSymbol(";"));
            if ((base.Proxy != null) && (base.Proxy != ""))
            {
                request.set_Proxy(new WebProxy(base.Proxy));
            }
            request.set_UserAgent("Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; .NET CLR 1.0.3705; .NET CLR 1.1.4322)");
            HttpWebResponse response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.get_ASCII());
            while (true)
            {
                string[] strArray;
                do
                {
                    string str2 = reader.ReadLine();
                    if (str2 == null)
                    {
                        return;
                    }
                    strArray = str2.Split(new char[] { ';' });
                }
                while (this.OnStreamingData == null);
                IFormatProvider uSFormat = FormulaHelper.USFormat;
                DataPacket dp = new DataPacket(strArray[0], DateTime.Parse(strArray[1], DateTimeFormatInfo.get_InvariantInfo()), (double) float.Parse(strArray[2], uSFormat), double.Parse(strArray[3], uSFormat)) {
                    TimeZone = double.MaxValue
                };
                this.OnStreamingData(this, dp);
             */

            
        }
        catch (Exception exception)
        {
            base.LastError = exception;
        }
        finally
        {
            if (this.OnStreamingStopped != null)
            {
                this.OnStreamingStopped.Invoke(this, new EventArgs());
            }
        }
    }


    //当BrokerFeed得到新的Tick时,我们调用该函数进行处理tick,
    //DataClient则通过与之绑定的onStreamingData来更新对应的Chart
    public void GotTick(Tick t)
    {
        //当tick含有交易数据以及onStreamingData有对应的回调函数时,我们才整理DataPacket用于更新
        if (this.OnStreamingData != null&&t.isTrade)
        { 
            //DataPacket dp = null;
            DateTime dt = Util.ToDateTime(t.date, t.time);
            DataPacket dp = new DataPacket(t.symbol, dt, Convert.ToDouble(t.trade), Convert.ToDouble(t.size));
            //触发onStreamingData事件 调用相应的函数操作
            OnStreamingData(this, dp);
        }
    }

    public void GotRealTimeBar(Bar b)
    {
        //当tick含有交易数据以及onStreamingData有对应的回调函数时,我们才整理DataPacket用于更新
        if (this.OnStreamingData != null)
        {
            //DataPacket dp = null;
            //DataPacket dp = new DataPacket(t.symbol, DateTime.Now, Convert.ToDouble(t.trade), Convert.ToDouble(t.size));
            //触发onStreamingData事件 调用相应的函数操作
           // OnStreamingData(this, dp);
        }
    }


    //处理回补的bar数据
    public void GotBarBackFilled(Bar b)
    {
        if (this.OnBarBackFilled != null)
        { 
            //Util.f
            DateTime dt = Util.ToDateTime(b.Bardate, b.Bartime);
            DataPacket dp = new DataPacket(b.Symbol,dt,(double)b.Open,(double)b.High,(double)b.Low,(double)b.Close,(double)b.Volume,(double)b.Close);
            
            OnBarBackFilled(this, dp);
        }
    
    }
    //获得symbols某个时间段的某个频率的数据
    public override CommonDataProvider GetData(string symbols, DataCycle dataCycle, DateTime startTime, DateTime endTime)
    {
        
        byte[] bs = base.DownloadBinary(symbols, "http://subscribe.easychart.net/member/datafeed.aspx?f=BinaryHistory&AddWhenNoSymbol=1&Symbol=" + symbols);
        if (bs != null)
        {
            CommonDataProvider provider = new CommonDataProvider(null);
            provider.LoadByteBinary(bs);
            provider.SetStringData("Code", symbols.ToUpper());
            return provider;
        }
        return null;
    }

    //补充EOD数据
    public override DataPacket[] GetEodData(string Exchanges, string[] symbols, DateTime Time)
    {
        string str = base.DownloadString("http://subscribe.easychart.net/member/datafeed.aspx?f=EndOfDay&Exchanges=" + Exchanges + "&Date=" + Time.ToString("yyyy-MM-dd"));
        if ((str != null) && (str != ""))
        {
            string[] strArray = str.Trim().Split(new char[] { '\r' });
            DataPacket[] packetArray = new DataPacket[strArray.Length];
            for (int i = 0; i < packetArray.Length; i++)
            {
                packetArray[i] = DataPacket.ParseEODData(strArray[i].Trim());
            }
            return packetArray;
        }
        return base.GetEodData(Exchanges, symbols, Time);
    }
    //获得交易所列表
    public override string[] GetExchanges()
    {
        return new string[] { "AMEX;Nasdaq;Nyse;^=All US & Index", "AMEX;Nasdaq;Nyse=All US", "AMEX", "Nasdaq", "Nyse", "OTC+BB=OTC BB", "Shanghai", "Shenzhen", "TOR;CDNX=Canada", "TOR=Toronto", "CDNX=Vancouver", "ASX", "^=Global Indices" };
    }
    //获得industry
    public override string[] GetIndustry()
    {
        string str = base.DownloadString("http://data.easychart.net/Industry.aspx");
        if (str != null)
        {
            return str.Split(new char[] { '\r' });
        }
        return base.GetIndustry();
    }

    public override bool Login(string Username, string Password)
    {
        base.Ticket = base.DownloadString("http://subscribe.easychart.net/GetTicket.aspx?UserId=" + Username + "&Password=" + Password);
        return ((base.Ticket != null) && (base.Ticket != ""));
    }

    public override string[] LookupSymbols(string Key, string Exchanges, string StockType, string Market)
    {
        if (base.Logined)
        {
            string str = base.DownloadString("http://subscribe.easychart.net/member/datafeed.aspx?f=SymbolList&Exchanges=" + Exchanges);
            if (str != null)
            {
                return str.Trim().Split(new char[] { '\r' });
            }
        }
        return null;
    }

    public override void StopDownload()
    {
        base.Canceled = true;
    }

    // Properties
    public override string DataFeedName
    {
        get
        {
            return "TradeLink ChartDataClient";
        }
    }




    public override bool IsFree
    {
        get
        {
            return true;
        }
    }

    public override bool SupportEod
    {
        get
        {
            return true;
        }
    }

    public override bool SupportIndustry
    {
        get
        {
            return true;
        }
    }

    public override bool SupportIntraday
    {
        get
        {
            return false;
        }
    }

    public override bool SupportStreaming
    {
        get
        {
            return true;
        }
    }

    public override bool SupportSymbolList
    {
        get
        {
            return true;
        }
    }

    }
}
