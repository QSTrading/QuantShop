using System;
using System.Collections.Generic;
using TradeLink.API;
using TradeLink.Common;
using System.Threading;

namespace TradingLib.Core
{
    /// <summary>
    /// route your orders through this component to paper trade with a live data feed
    /// 模拟交易
    /// </summary>
    public class PapertradeEngine
    {
        IdTracker _idt;
        public PapertradeEngine() : this(new IdTracker(),false) { }

        public PapertradeEngine(IdTracker idt,bool verb)
        {
            VerboseDebugging = verb;
            _idt = idt;
        }
        bool _usebidask = true;
        /// <summary>
        /// whether to use bid ask for fills, otherwise last is used.
        /// </summary>
        public bool UseBidAskFills { get { return _usebidask; } set { _usebidask = value; } }
        /// <summary>
        /// fill acknowledged, order filled.
        /// </summary>
        public event FillDelegate GotFillEvent;
        /// <summary>
        /// order acknowledgement, order placed.
        /// </summary>
        public event OrderDelegate GotOrderEvent;
        /// <summary>
        /// cancel acknowledgement, order is canceled
        /// </summary>
        public event LongDelegate GotCancelEvent;
        /// <summary>
        /// copy of the cancel request
        /// </summary>
        public event LongDelegate SendCancelEvent;

        /// <summary>
        /// debug messages
        /// </summary>
        public event DebugDelegate SendDebugEvent;

        TickTracker _kt = new TickTracker();

        public const string DEFAULTBOOK = "DEFAULT";
        protected Account DEFAULT = new Account(DEFAULTBOOK, "Defacto account when account not provided");
        protected Dictionary<Account, List<Order>> MasterOrders = new Dictionary<Account, List<Order>>();
        protected Dictionary<string, List<Trade>> MasterTrades = new Dictionary<string, List<Trade>>();


        /// <summary>
        /// 产生新的tick用于引擎Fill Order
        /// </summary>
        /// <param name="k"></param>
        public void newTick(Tick k)
        {
            _kt.addindex(k.symbol);
            _kt.newTick(k);
            process();
        }

        bool _noverb = true;
        public bool VerboseDebugging { get { return !_noverb; } set { _noverb = !value; } }
        void v(string msg)
        {
            if (_noverb) return;
            debug(msg);
        }

        void debug(string msg)
        {
            if (SendDebugEvent != null)
                SendDebugEvent(msg);
        }

        //使用Queue后有些symbol会莫名其妙的丢失数据,这里是简易的交易引擎,我们使用List来代替
        //问题查出来了 是gotfillevent对外的调用太复杂 过程当中出现了一场 导致process循环被跳开
        //从而发现不是所有的order都被fill 因此主干过程我们的处理过程需要简单明了 其他非主干过程
        //我们用消息放到队列在其他线程中进行。主干线程就是任务明确 稳定 高效率
        Queue<Order> aq = new Queue<Order>(100);
        Queue<long> can = new Queue<long>(100);
        /// <summary>
        /// gets currently queued orders
        /// </summary>
        public Order[] QueuedOrders { get { lock (aq) { return aq.ToArray(); } } }

        /// <summary>
        /// gets count of queued cancels
        /// </summary>
        public int QueuedCancelCount { get { lock (can) { return can.Count; } } }

        

        /// <summary>
        /// send paper trade orders
        /// </summary>
        /// <param name="o"></param>
        public void sendorder(Order o)
        {
            if (o.id == 0)
                o.id = _idt.AssignId;
            lock (aq)
            {
                debug("PTT Server queueing order: " + o.ToString());
                //模拟交易对新提交上来的order进行复制后保留,原来的order形成事件触发.这样本地的order fill process对order信息的修改就不会影响到对外触发事件的order.
                //因为对外触发事件提交的order与本地按照tick fill order时使用的order是不同的副本
                Order oc = new OrderImpl(o);
                aq.Enqueue(oc);
            }
            // ack the order 对提交的order进行信息回报
            if (GotOrderEvent != null)
                GotOrderEvent(o);
            //有order进入时候我们不需要进行处理,order的fill均由tick引发
            //process();
        }
        void process()
        {
            Order[] orders;
            long[] cancels;
            lock (aq)
            {
                // get copy of current orders
                 orders = aq.ToArray();
                 aq.Clear();

                //get current cancels
                 cancels = can.ToArray();
                 can.Clear();
            }
            // get ready for unfilled orders
            List<Order> unfilled = new List<Order>();
            // try to fill every order
            for (int i = 0; i < orders.Length; i++)
            {
                //debug("we have orders now we deal with them");
                // get order
                Order o = orders[i];
                // check for a cancel, if so remove cancel, acknowledge and continue
                int cidx = gotcancel(o.id, cancels);
                if (cidx >= 0)
                {
                    debug("PTT Server canceling: " + o.id);
                    cancels[cidx] = 0;
                    if (GotCancelEvent != null)
                        GotCancelEvent(o.id);
                    continue;
                }

                // try to fill it
                bool filled = o.Fill(_kt[o.symbol], UseBidAskFills, false);
                // if it doesn't fill, add it back and continue
                if (!filled)
                {
                    unfilled.Add(o);
                    continue;
                }
                
                // check for partial fill
                Trade fill = (Trade)o;
                debug("PTT Server filled: " + fill.ToString());
                bool partial = fill.UnsignedSize != o.UnsignedSize;
                if (partial)
                {
                    o.size = (o.UnsignedSize - fill.UnsignedSize) * (o.side ? 1 : -1);
                    unfilled.Add(o);
                }
                // acknowledge the fill
                Trade nf = new TradeImpl(fill);
                //关于这里复制后再发出order，这里的process循环 fill order. fill是对order的一个引用，后面修改的数据会覆盖到前面的数据,而gotfillevent触发的时候可能是在另外一个线程中
                //运行的比如发送回client,或者记录信息等。这样就行程了多个线程对一个对象的访问可能会存在数据部同步的问题。
                if (GotFillEvent != null)
                    GotFillEvent(nf);
            }
            lock (aq)
            {

                // add orders back
                for (int i = 0; i < unfilled.Count; i++)
                {
                    aq.Enqueue(unfilled[i]);
                }
                // add cancels back
                for (int i = 0; i < cancels.Length; i++)
                    if (cancels[i] != 0)
                        can.Enqueue(cancels[i]);
            }
        }

        int gotcancel(long id, long[] ids)
        {
            if (id == 0) return -1;
            for (int i = 0; i < ids.Length; i++)
                if (id == ids[i]) return i;
            return -1;
        }

        /// <summary>
        /// cancel papertrade orders
        /// </summary>
        /// <param name="id"></param>
        public void sendcancel(long id)
        {
            if (id == 0) return;
            lock (aq)
            {
                can.Enqueue(id);
                debug(" PTT queueing cancel: " + id);
            }
            if (SendCancelEvent != null)
                SendCancelEvent(id);
            process();
        }
    }
}
