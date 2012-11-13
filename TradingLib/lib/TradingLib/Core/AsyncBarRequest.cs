using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using TradeLink.API;
using TradeLink.Common;

namespace TradingLib.Core
{
    /// <summary>
    /// used to provide ultra-fast tick processing on machines with multiple cores.
    /// takes ticks immediately on main thread, processes them on a seperate thread.
    /// </summary>
    public class AsyncBarRequest
    {
        const int MAXBR = 1000;
        RingBuffer<BarRequest> _brcache;//history bar data request buffer

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
        public int BROverrun { get { return _brcache.BufferOverrun; } }
        static ManualResetEvent _barrequestswaiting = new ManualResetEvent(false);
        Thread _readbarrequestthread = null;
        volatile bool _readbarrequest = true;

        int _nrt = 0;
        int _nwt = 0;

        public bool isValid { get { return _readbarrequest; } }

        void ReadBarRequest()
        {
            try
            {
                while (_readbarrequest)
                {
                    if(_brcache.hasItems && (GotBRQueued != null))
                        GotBRQueued();
                    while (_brcache.hasItems)
                    {
                        if (!_readbarrequest)
                            break;
                        //TickImpl k = _tickcache.Read();
                        BarRequest br = _brcache.Read();
                        //if (br.is)
                        //{
                        //    _nrt++;
                        //    if (GotBadBR != null)
                        //        GotBadBR();
                        //    continue;
                       // }
                        if (GotBarRequest != null)
                            GotBarRequest(br);
                    }
                    // send event that queue is presently empty
                    if (_brcache.isEmpty && (GotBRQueueEmpty != null))
                        GotBRQueueEmpty();
                    // clear current flag signal
                    _barrequestswaiting.Reset();
                    // wait for a new signal to continue reading
                    _barrequestswaiting.WaitOne(SLEEP);

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
        /// 将新的BarRequest传递如AsyncBarRequest 从而实现异步处理防止阻塞通讯主线程
        /// </summary>
        /// <param name="k"></param>
        public void newBarRequest(BarRequest br)
        {
            //if (br == null)
            //{
            //    _nwt++;
            //    if (GotBadTick != null)
            //        GotBadTick();
           //     return;
           // }
            _brcache.Write(br);
            if ((_readbarrequestthread != null) && (_readbarrequestthread.ThreadState == ThreadState.Unstarted))
            {
                _readbarrequest = true;
                _readbarrequestthread.Start();
            }
            else if ((_readbarrequestthread != null) && (_readbarrequestthread.ThreadState == ThreadState.WaitSleepJoin))
            {
                _barrequestswaiting.Set(); // signal ReadIt thread to read now
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
        public AsyncBarRequest() : this(MAXBR) { }
        /// <summary>
        /// creates asynchronous responder with specified buffer sizes
        /// </summary>
        /// <param name="maxticks"></param>
        /// <param name="maximb"></param>
        public AsyncBarRequest(int maxbr)
        {
            _brcache = new RingBuffer<BarRequest>(maxbr);
            _brcache.BufferOverrunEvent += new VoidDelegate(_brcache_BufferOverrunEvent);
            _readbarrequestthread = new Thread(this.ReadBarRequest);

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
            _readbarrequest = false;
            try
            {
                if ((_readbarrequestthread != null) && ((_readbarrequestthread.ThreadState != ThreadState.Stopped) && (_readbarrequestthread.ThreadState != ThreadState.StopRequested)))
                    _readbarrequestthread.Interrupt();
            }
            catch { }
            try
            {
                _brcache = new RingBuffer<BarRequest>(MAXBR);
                _barrequestswaiting.Reset();
            }
            catch { }
        }

    }
}
