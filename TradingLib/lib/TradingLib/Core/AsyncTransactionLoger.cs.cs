using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using TradeLink.API;
using TradeLink.Common;

namespace TradingLib.Core
{
    
    /// <summary>
    ///用于将Order cancle trade交易信息安全的记录到数据库
    /// </summary>
    public class AsyncTransactionLoger
    {
        
        public event DebugDelegate SendDebugEvent;
        const int MAXLOG = 5000;
        //建立连接 注意这里的数据库读写是单线程的 是符合线程安全的
        Data.database.mysqlDB mysql = new Data.database.mysqlDB();
        RingBuffer<Order> _ocache;
        RingBuffer<Trade> _tcache;
        RingBuffer<long> _ccache;
        /// <summary>
        /// fired when barrequest is read asychronously from buffer
        /// </summary>
        public event BarRequestDel GotBarRequest;//有新的barRequest处理事件
        /// <summary>
        ///  fired when buffer is empty
        /// </summary>
        public event VoidDelegate GotBRQueueEmpty;
        /// <summary>
        /// fired when buffer is written
        /// </summary>
        public event VoidDelegate GotBRQueued;
        /// <summary>
        /// should be zero unless buffer too small
        /// </summary>
        private bool _noverb = true;
        public bool VerboseDebugging { get { return !_noverb; } set { _noverb = value; } }

        void debug(string msg)
        {
            if (SendDebugEvent != null)
                SendDebugEvent(msg);
        }

        void v(string msg)
        {
            if (!_noverb) return;
            debug(msg);
        }
        static ManualResetEvent _logwaiting = new ManualResetEvent(false);
        Thread _logthread = null;
        public bool isValid { get { return _loggo; } }

        int _nwt;
        int _nrt;

        bool _loggo=true;
        int _delay = 5;
        /// <summary>
        /// 异步交易信息记录系统拥有1000条的缓存数据,实验的时候发现插入数据错误,后来研究发现 插入数据的先后有关系
        /// 当我们在trade cache停留的时候 一致有新的交易被送进来，但是ordercache里面的order却没有发送到数据库 从而造成的问题就是 当trade里面出现对应的order时候 我们并没有插入到该order
        /// 因此我们在插入数据的时候要检查 Order是优先插入的 有了order我们才可以先交易。 因此Order被完全插入完毕后我们才开始插入交易数据
        /// </summary>
        void readedata()
        {
            try
            {
                while (_loggo)
                {
                    //插入交易
                    while (_ocache.hasItems)
                    {
                        Order o = _ocache.Read();
                        bool re = mysql.insertOrder(o);
                        debug("Order inserted:"+o.ToString());
                        if (!re)
                        {
                            _nrt++;
                            debug("some thing wrong");
                        }
                        Thread.Sleep(_delay);
                        
                    }
                    //插入成交
                    while (!_ocache.hasItems && _tcache.hasItems)
                    {
                        Trade f = _tcache.Read();
                        bool re = mysql.insertTrade(f);
                        debug("Trade inserted:"+f.ToString());
                        if (!re)
                        {
                            _nrt++;
                            debug("some thing wrong");
                        }
                        Thread.Sleep(_delay);
           
                    }
                    //插入取消
                    while (!_ocache.hasItems && _ccache.hasItems)
                    {
                        long oid = _ccache.Read();
                        bool re = mysql.insertCancel(Util.ToTLDate(DateTime.Now), Util.ToTLTime(DateTime.Now), oid);
                        debug("Cancle inserted:"+oid.ToString());
                        if (!re)
                        {
                            _nrt++;
                            debug("some thing wrong");
                        }
                        Thread.Sleep(_delay);
                    }
                    // clear current flag signal
                    _logwaiting.Reset();
                   
                    // wait for a new signal to continue reading
                    _logwaiting.WaitOne(SLEEP);
                }
            }
            catch (MissingMethodException ex)
            {

                System.Diagnostics.Process.Start(@"http://code.google.com/p/tradelink/wiki/MissingMethodException");
                Stop();

            }
            catch (MissingMemberException ex)
            {
                System.Diagnostics.Process.Start(@"http://code.google.com/p/tradelink/wiki/MissingMethodException");
                Stop();
            }
            catch (ThreadInterruptedException) { }
        }

        public const int SLEEPDEFAULTMS = 10;
        int _sleep = SLEEPDEFAULTMS;
        /// <summary>
        /// sleep time in milliseconds between checking read buffer
        /// </summary>
        public int SLEEP { get { return _sleep; } set { _sleep = value; } }

        /// <summary>
        /// 将新的需要记录的数据记录下来 从而实现异步处理防止阻塞通讯主线程
        /// 数据记录需要copy模式,否则引用对象得其他线程访问时候会出现数据错误 比如成交数目与实际成交数目无法对应等问题。
        /// </summary>
        /// <param name="k"></param>
        public void newOrder(Order o)
        {
            Order oc = new OrderImpl(o);
            _ocache.Write(oc);
            newlog();
        }
        public void newTrade(Trade f)
        {
            Trade nf = new TradeImpl(f);
            _tcache.Write(nf);
            newlog();
        }
        public void newCancle(long id)
        {
            _ccache.Write(id);
            newlog();
        }

        private void newlog()
        {
             if ((_logthread!= null) && (_logthread.ThreadState == ThreadState.Unstarted))
            {
                _loggo = true;
                _logthread.Start();
            }
            else if ((_logthread != null) && (_logthread.ThreadState == ThreadState.WaitSleepJoin))
            {
                _logwaiting.Set(); // signal ReadIt thread to read now
            }
            
        }
        /// <summary>
        /// called if bad barrequest is written or read.
        /// check bad counters to see if written or read.
        /// </summary>
        public event VoidDelegate GotBadBR;
        /// <summary>
        /// called if buffer set is too small
        /// </summary>
        public event VoidDelegate GotBarRequestOverrun;
        /// <summary>
        /// # of null barrequest ignored at write
        /// </summary>
        public int BadBRWritten { get { return _nwt; } }
        /// <summary>
        /// # of null barrequest ignored at read
        /// </summary>
        public int BadBRRead { get { return _nrt; } }



        /// <summary>
        /// create an asynchronous responder
        /// </summary>
        public AsyncTransactionLoger() : this(MAXLOG) { }
        /// <summary>
        /// creates asynchronous responder with specified buffer sizes
        /// </summary>
        /// <param name="maxticks"></param>
        /// <param name="maximb"></param>
        public AsyncTransactionLoger(int maxbr)
        {
            _ocache = new RingBuffer<Order>(maxbr);
            _tcache = new RingBuffer<Trade>(maxbr);
            _ccache = new RingBuffer<long>(maxbr);

            _logthread = new Thread(this.readedata);
            //_brcache = new RingBuffer<BarRequest>(maxbr);
            //_brcache.BufferOverrunEvent += new VoidDelegate(_brcache_BufferOverrunEvent);
            //_readbarrequestthread = new Thread(this.ReadBarRequest);

        }

        void _brcache_BufferOverrunEvent()
        {
            if (GotBarRequestOverrun != null)
                GotBarRequestOverrun();
        }
        /// <summary>
        /// stop the read threads and shutdown (call on exit)
        /// </summary>
        public void Stop()
        {
            _loggo = false;
            try
            {
                if ((_logthread != null) && ((_logthread.ThreadState != ThreadState.Stopped) && (_logthread.ThreadState != ThreadState.StopRequested)))
                    _logthread.Interrupt();
            }
            catch { }
            try
            {
                //_brcache = new RingBuffer<BarRequest>(MAXLOG);
                _logwaiting.Reset();
            }
            catch { }
        }

    }
}
