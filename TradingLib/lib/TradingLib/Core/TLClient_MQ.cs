using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TradeLink.API;
using TradingLib.API;
using TradingLib.Transport;

namespace TradeLink.Common
{
    /// <summary>
    /// TradeLink clients can connect to any supported TradeLink broker.
    /// version of the client that supports the tradelink protocol via windows messaging transport.
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    public class TLClient_MQ : TLClient
    {
        public ProviderType ProviderType { get; set; }
        Socket server;
        System.ComponentModel.BackgroundWorker _bw;
        System.ComponentModel.BackgroundWorker _bw2;
        int port = IPUtil.TLDEFAULTBASEPORT;
        int _wait = 50;
        public bool IsConnected { get { return _connect; } }
        

        public event Int32Delegate GotConnectEvent;
        public event Int32Delegate GotDisconnectEvent;

        void updateheartbeat()
        {
            _lastheartbeat = DateTime.Now.Ticks;
        }

        //断开连接后我们需要进行标识并输出事件
        void markdisconnect()
        {
            _connect = false;
            if (GotDisconnectEvent != null)
                GotDisconnectEvent(Util.ToTLTime());
        }
    

        bool _connect = false;

        Basket _b = new BasketImpl();

        int bufferoffset = 0;
        byte[] buffer;

        int _sendheartbeat = IPUtil.SENDHEARTBEATMS;
        long _lastheartbeat = 0;
        bool _requestheartbeat = false;
        bool _recvheartbeat = false;
        int _heartbeatdeadat = IPUtil.HEARTBEATDEADMS;
        bool _reconnectreq = false;

        public bool isHeartbeatOk { get { return _connect && (_requestheartbeat == _recvheartbeat); } }

        

        void _bw2_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            int p = (int)e.Argument;
            while (_started)
            {
                if (!e.Cancel)
                    Thread.Sleep(_wait * 10);
                else
                {
                    _started = false;
                }
                // get current timestamp
                long now = DateTime.Now.Ticks;
                // get time since last heartbeat in MS
                long diff = (now - _lastheartbeat) * 10000;
                // if we're not waiting for reconnect and we're due for heartbeat
                if (false && _connect && !_reconnectreq && (diff > _sendheartbeat))
                {
                    // our heartbeat is presently ok but it shouldn't be
                    if (isHeartbeatOk)
                    {
                        // notify 
                        v("heartbeat request at: " + DateTime.Parse(now.ToString()));
                        // mark heartbeat as bad
                        _requestheartbeat = !_recvheartbeat;
                        // try to jumpstart by requesting heartbeat
                        TLSend(MessageTypes.HEARTBEATREQUEST);
                    }
                        // if we're waiting for response but never get one, reconnect
                    else if (diff > _heartbeatdeadat)
                    {
                        _reconnectreq = true;
                        debug("heartbeat is dead, reconnecting at: " + DateTime.Parse(now.ToString()));
                        connect(p,false);
                    }
                }
            }
        }

        void _bw_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            int p = (int)e.Argument;
            // run until client stopped
            while (_started)
            {
                // quit thread if requested
                if (e.Cancel)
                {
                    _started = false;
                    break;
                }
                bool connected = server.Connected;

                try
                {

                    int ret = server.Receive(buffer, bufferoffset, buffer.Length - bufferoffset, SocketFlags.None);
                    if (ret > 0)
                    {
#if DEBUG
                        // notify
                        //v("client received bytes: " + ret+" raw data: "+HexToString(buffer,ret));
#endif
                        // get messages from data
                        Message[] msgs = Message.gotmessages(ref buffer, ref bufferoffset);
                        // handle messages
                        for (int i = 0; i < msgs.Length; i++)
                        {
#if DEBUG
                            //v("client message#"+i+" type: "+msgs[i].Type+" content: "+msgs[i].Content);
#endif
                            handle(msgs[i].Type, msgs[i].Content);
                        }

                    }
                    else if (ret == 0) // socket was shutdown
                    {
                        connected = issocketconnected(server);
                    }
                    
                }

                catch (SocketException ex)
                {
                    v("socket exception: " + ex.SocketErrorCode + ex.Message + ex.StackTrace);

                }
                catch (Exception ex)
                {
                    debug(ex.Message + ex.StackTrace);
                }
                
                
                if (_connect && ((server == null) || !connected))
                {
                    if ((p >= 0) && (p < serverip.Count))
                    {
                        markdisconnect();
                        debug("client lost connection to server: " + serverip[p]);
                    }
                    if (connect(p, false))
                        debug("recovered connection to: " + serverip[p]);
                    else
                        debug("unable to recover connection to: " + serverip[p]);
                }
            }
        }

        bool issocketconnected(Socket client) { int err; return issocketconnected(client, out err); }
        bool issocketconnected(Socket client, out int errorcode)
        {
            bool blockingState = client.Blocking;
            errorcode = 0;
            try
            {
                byte[] tmp = new byte[1];

                client.Blocking = false;
                client.Send(tmp, 0, 0);
            }
            catch (SocketException e)
            {
                // 10035 == WSAEWOULDBLOCK
                if (e.NativeErrorCode.Equals(10035))
                {
                    v("connected but send blocked.");
                    return true;
                }
                else
                {

                    errorcode = e.NativeErrorCode;
                    v("disconnected, error: " + errorcode);
                    return false;
                }
            }
            finally
            {
                client.Blocking = blockingState;
            }
            return client.Connected;
        }



        bool _started = false;

        private void StartRecieve()
        {
            
        }

        #region Start Stop Section
        //启动服务
        public void Start()
        {
            debug("TLClient Starting,Try to connect to server.");
            Mode(_curprovider, false);
            //当去的服务器连接后
        }
        //当有服务特性返回如果对应的服务端支持tick则我们需要单独启动tick数据服务
        //我们使用不同的连接来处理数据以及请求当一个Provider同时满足数据和交易的要求时,我们的交易连接也会根据Featuresupport自动注册到服务端的Tick分发接口.在这里我们需要
        //对provider的类型进行验证.该TLClient所对应的连接是DataFeed还是Execution进行区分。这样数据就不会应为多次注册 造成Tick数据的重复
        private void checkTickSupport()
        {
            
            if (_rfl.Contains(MessageTypes.TICKNOTIFY) && ProviderType == ProviderType.DataFeed)
            {
                debug("     Spuuort Tick we subscribde tick data server");
                _mqcli.StartTickReciver();
                //_mqcli.GotTick += (msg) =>
               // {
                        //dealtick(msg);
               // };
            }
        }
        /*
        private void dealtick(string msg)
        {
            Tick t=new TickImpl();
            try
            {
                _lastheartbeat = DateTime.Now.Ticks;
                t = TickImpl.Deserialize(msg);

            }
            catch (Exception ex)
            {
                _tickerrors++;
                debug("Error processing tick: " + msg);
                debug("TickErrors: " + _tickerrors);
                debug("Error: " + ex.Message + ex.StackTrace);
            }
            if (gotTick != null)
                gotTick(t);
        }*/

        /// <summary>
        /// stop the client
        /// </summary>
        public void Stop()
        {
           //_started=false
            try
            {
                if (_bw.IsBusy)
                    _bw.CancelAsync();
                if (_bw2.IsBusy)
                    _bw2.CancelAsync();
                //return;
                if (_mqcli.isConnected&& _started) //如果实现已经stop了brokerfeed 会造成服务器循环相应。应该将_stated放在这里进行相应
                {
                    _started = false;
                    try
                    {
                        _mqcli.Disconnect();
                        //debug("disconnected : "  +_mqcli.ID);
                    }
                    catch { }
                }



            }
            catch (Exception ex)
            {
                debug("Error stopping TLClient_IP " + ex.Message + ex.StackTrace);
            }
            debug("TLClient Stopped: " + Name);
        }

        #endregion


        // clients that want notifications for subscribed stocks can override these methods

        public event TickDelegate gotTick;
        public event FillDelegate gotFill;
        public event OrderDelegate gotOrder;
        public event DebugDelegate gotAccounts;
        public event LongDelegate gotOrderCancel;
        public event MessageTypesMsgDelegate gotFeatures;
        public event PositionDelegate gotPosition;
        public event ImbalanceDelegate gotImbalance;
        public event MessageDelegate gotUnknownMessage;
        public event DebugDelegate SendDebugEvent;
        public event DebugDelegate gotServerUp;
        public event DebugDelegate gotServerDown;

        // member fields

        List<MessageTypes> _rfl = new List<MessageTypes>();
        public List<MessageTypes> RequestFeatureList { get { return _rfl; } }
        Dictionary<string, PositionImpl> cpos = new Dictionary<string, PositionImpl>();
        List<Providers> servers = new List<Providers>();

        const int MAXSERVER = 10;
        //List<IPEndPoint> serverip = new List<IPEndPoint>();
        List<string> serverip = new List<string>();

        int _curprovider = -1;
        string _name = string.Empty;
        public string Name
        {
            get
            {
                return _name;
            }
            set { }
        }

        int _disconnectretry = 3;
        public int DisconnectRetries { get { return _disconnectretry; } set { _disconnectretry = value; } }

        public Providers[] ProvidersAvailable { get { return servers.ToArray(); } }
        public int ProviderSelected { get { return _curprovider; } }


        Providers _bn = Providers.Unknown;
        public Providers BrokerName
        {
            get
            {
                return _bn;
            }
        }

        private int _serverversion;
        public int ServerVersion { get { return _serverversion;  } }

        public static List<IPEndPoint> GetEndpoints(int port, params string[] servers)
        {
            List<IPEndPoint> ip = new List<IPEndPoint>();
            foreach (string server in servers)
                if (IPUtil.isValidAddress(server))
                    ip.Add(new IPEndPoint(IPAddress.Parse(server),port));
            return ip;
        }
        public static List<IPEndPoint> GetEndpoints(params IPEndPoint[] eps) 
        { 
            List<IPEndPoint> ip = new List<IPEndPoint>();
            foreach(IPEndPoint ep in eps)
                    ip.Add(ep);
            return ip;
        }

        #region TLClient_IP 构造函数
        //构造生成search client
        //在某个特定IP上构造成我们需要的client
        public TLClient_MQ(string server, int srvport, DebugDelegate deb, bool verbose)
            : this(new string[] {server},0, srvport,"tlclient", DEFAULTRETRIES, DEFAULTWAIT, deb, verbose)
        {
          
        }
        //用于利用一组servers构造search client 用于检查服务器是否可用
        public TLClient_MQ(string[] servers, int srvport, DebugDelegate deb, bool verbose)
            : this(servers, 0, srvport, "tlclient", DEFAULTRETRIES, DEFAULTWAIT, deb, verbose)
        {

        }

        //通过provider index选择我们需要的某个server连接
        public TLClient_MQ(string[] server, int ProviderIndex, int srvport, DebugDelegate deb, bool verbose)
            : this(server, 0,srvport, "tlclient", DEFAULTRETRIES, DEFAULTWAIT, deb, verbose)
        {
          
        }

        public const int DEFAULTWAIT = 100;
        public const int DEFAULTRETRIES = 3;
        public TLClient_MQ(string[] servers, int ProviderIndex, int srvport, string Clientname) : this(servers,ProviderIndex, srvport,Clientname, DEFAULTRETRIES, DEFAULTWAIT, null, false) { }
        
        //构造函数
        public TLClient_MQ(string[] servers, int ProviderIndex, int srvport, string Clientname, int disconnectretries, int wait, DebugDelegate deb, bool verbose)
        {
            debug("Start TLClient...");
            VerboseDebugging = verbose;
            _wait = wait;
            port = srvport;
            SendDebugEvent = deb;
            foreach (string s in servers)
                serverip.Add(s);
            //serverip = servers;
            Mode(ProviderIndex, false);
        }

        #endregion
        

        #region Mode 用于寻找可用服务 并进行连接
        delegate bool ModeDel(int pi, bool warn);
        /// <summary>
        /// 通过pidx选择我们需要的Provider
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <returns></returns>
        public bool Mode() { return Mode(0, true); }
        public bool Mode(int ProviderIndex, bool showwarning)
        {
            // search our provider list
            //这里会像服务端尝试发起一个连接并请求broker name.
            debug("[TLClient_MQ]Mode to Provider");
            try
            {
                TLFound();
            }
            catch (QSNoServerException ex)
            {
                throw ex;
            }
            // see if called from start
            if (ProviderIndex < 0)
            {
                debug("provider index cannot be less than zero, using first provider.");
                ProviderIndex = 0;
            }
            // attempt to connect to preferred
            //正式与服务器建立连接,这里会发出一个新的会话连接 connect函数会较正式的建立一个连接 TLFound中时通过 new socket连打开一个通道
            bool ok = connect(ProviderIndex, false);
            //return false;
            if (!_started && ok)
            {
                // restart if we connected
                _started = true;
                // background thread to receive messages
                v("client starting background thread.");
                _bw = new System.ComponentModel.BackgroundWorker();
                _bw.WorkerSupportsCancellation = true;
                _bw.DoWork += new System.ComponentModel.DoWorkEventHandler(_bw_DoWork);
                _bw.RunWorkerAsync(ProviderIndex);
                
                
                _bw2 = new System.ComponentModel.BackgroundWorker();
                _bw2.WorkerSupportsCancellation = true;
                _bw2.DoWork+=new System.ComponentModel.DoWorkEventHandler(_bw2_DoWork);
                _bw2.RunWorkerAsync(ProviderIndex);
                
            }
            if (!ok)
            {
                debug("Unable to connect to provider: " + ProviderIndex);
                return false;
            }
            
            try
            {
               
                // register ourselves with provider
                Register();
                // request list of features from provider
                RequestFeatures();
                // assuming we got this far, mark selected provider current
                //request server version;
                ReqServerVersion();

                _curprovider = ProviderIndex;
                _bn = servers[_curprovider];
                //这里对外触发的事件我们需要等待所有的连接工作均顺利进行完毕后才可以进行,否则会出现连接无效 发送消息错误
                //debug("we are here..........................................................3333333333333333333...");
                if (GotConnectEvent != null)
                {
                    GotConnectEvent(Util.ToTLTime());
                    //debug("connect event is called");
                }
                return true;
            }
            catch (SocketException ex)
            {

                debug("socket exception: " + ex.SocketErrorCode + ex.Message + ex.StackTrace);
                    
            }
            
            catch (Exception ex) 
            { 
                debug(ex.Message+ex.StackTrace);
                
            }
            debug("Server not found at index: " + ProviderIndex);
            return false; 
        }
        #endregion


        #region TLSend

        delegate long TLSendDelegate(MessageTypes type, string msg, IntPtr d);
        public long TLSend(MessageTypes type, long source, long dest, long msgid, string message, ref string result)
        {
            return TLSend(type, message);
        }
        /// <summary>
        /// sends a message to server.  
        /// synchronous responses are returned immediately as a long
        /// asynchronous responses come via their message type, or UnknownMessage event otherwise
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public long TLSend(string msg)
        {
            if (_mqcli.isConnected)
            { 
                _mqcli.Send(msg);
                return 0;
            }
            return 0;
        }
        public long TLSend(MessageTypes type) { return TLSend(type, string.Empty); }
        public long TLSend(MessageTypes type, string m)
        {
            
            // encode
            //if (type == MessageTypes.ACCOUNTREQUEST)
            //    debug("@@"+m);
            byte[] data = Message.sendmessage(type, m);
#if DEBUG
            //v("client sending message type: " + type + " contents: " + m);
            //v("client sending raw data size: " + data.Length + " data: " + HexToString(data, data.Length));
#endif
            //int len = 0;
            try
            {
                if (_mqcli.isConnected)
                {
                    // send request
                    //debug("sending data to serverside");
                    _mqcli.Send(data);
                    //debug("send information....................");
                    return 0;
                }
                else
                {
                    retryconnect();
                    return 0;
                }
            }
            catch (SocketException ex)
            {
                debug("exception: " + ex.SocketErrorCode + ex.Message + ex.StackTrace);
                retryconnect();


            }
            catch (Exception ex)
            {
                debug("error sending: " + type + " " + m);
                debug(ex.Message + ex.StackTrace);

            }
            return (long)MessageTypes.UNKNOWN_ERROR;
        }

        #endregion

        public static string HexToString(byte[] buf, int len)
        {
            string Data1 = "";
            string sData = "";
            int i = 0;
            while (i < len)
            {
                //Data1 = String.Format(攞0:X}? buf[i++]); //no joy, doesn抰 pad
                Data1 = buf[i++].ToString("X").PadLeft(2, '0'); //same as ?02X?in C
                sData += Data1;
            }
            return sData;
        }



        #region 其他程序---> TLClient 用于向TLServer发送请求的操作

        public void Register()
        {
            TLSend(MessageTypes.REGISTERCLIENT, Name);
        }

        public void ReqServerVersion()
        {
            TLSend(MessageTypes.VERSION,Name);
        }

        public void ReqBrokerName()
        { 
            
        }

        public void Subscribe(TradeLink.API.Basket mb)
        {
            // save last basket
            _b = mb;
            TLSend(MessageTypes.REGISTERSTOCK, Name + "+" + mb.ToString());
        }

        public void Unsubscribe()
        {
            TLSend(MessageTypes.CLEARSTOCKS, Name);
        }

        public int HeartBeat()
        {
            return (int)TLSend(MessageTypes.HEARTBEATREQUEST, Name);
        }

        public void RequestDOM()
        {
            int depth = 4; //default depth
            TLSend(MessageTypes.DOMREQUEST, Name + "+" + depth);
        }

        public void RequestDOM(int depth)
        {
            TLSend(MessageTypes.DOMREQUEST, Name + "+" + depth);
        }
        /// <summary>
        /// Sends the order.
        /// </summary>
        /// <param name="o">The oorder</param>
        /// <returns>Zero if succeeded</returns>
        public int SendOrder(Order o)
        {
            if (o == null) return (int)MessageTypes.EMPTY_ORDER;
            if (!o.isValid) return (int)MessageTypes.EMPTY_ORDER;
            string m = OrderImpl.Serialize(o);
            try
            {
                TLSend(MessageTypes.SENDORDER, m);
                return 0;
            }
            catch (SocketException ex)
            {
                debug("Exception sending order: " + o.ToString() + " " + ex.SocketErrorCode + ex.Message + ex.StackTrace);
                return (int)MessageTypes.UNKNOWN_ERROR;
            }
        }
        /// <summary>
        /// request a list of features, result will be returned to gotFeatureResponse and RequestFeaturesList
        /// </summary>
        public void RequestFeatures() 
        {
            _rfl.Clear();
            TLSend(MessageTypes.FEATUREREQUEST,Name); 
        }


        /// <summary>
        /// Request an order be canceled
        /// </summary>
        /// <param name="orderid">the id of the order being canceled</param>
        public void CancelOrder(Int64 orderid) { TLSend(MessageTypes.ORDERCANCELREQUEST, orderid.ToString()); }

        /// <summary>
        /// Send an account request, response is returned via the gotAccounts event.
        /// </summary>
        /// <returns>error code, and list of accounts via the gotAccounts event.</returns>
        /// 
        public int RequestAccounts() { return (int)TLSend(MessageTypes.ACCOUNTREQUEST, Name); }
        /// <summary>
        /// send a request so that imbalances are sent when received (via gotImbalance)
        /// </summary>
        /// <returns></returns>
        public int RequestImbalances() { return (int)TLSend(MessageTypes.IMBALANCEREQUEST, Name); }
        /// <summary>
        /// Sends a request for current positions.  gotPosition event will fire for each position record held by the broker.
        /// </summary>
        /// <param name="account">account to obtain position list for (required)</param>
        /// <returns>number of positions to expect</returns>
        public int RequestPositions(string account) { if (account == "") return 0; return (int)TLSend(MessageTypes.POSITIONREQUEST, Name + "+" + account); }


        #endregion



        #region 连接与断开连接
        public void Disconnect() { Disconnect(true); }

        AsyncClient _mqcli = null;
        //断开连接 我们需要用stop来停止本地的一些服务进程，在这之前我们需要向服务器发送一个clearclient的消息
        public void Disconnect(bool nice)
        {
            if (nice)
            {
                //debug("we are stop here..........");
                //v("Client disconnected here");
                TLSend(MessageTypes.CLEARCLIENT, Name);//向服务器发送clearClient消息
                _connect = false;
               
                //stop 用于断开连接 因此消息需要在连接断开前发送
                //return;
                //Thread.Sleep(1000);
                Stop();//停止本地服务器
                if (GotDisconnectEvent != null)
                    GotDisconnectEvent(Util.ToTLTime());
            }
            else
            {
                server.Close();
            }
        }

        bool connect() { return connect(_curprovider != -1 ? _curprovider : 0); }
        bool connect(int providerindex) { return connect(providerindex, false); }
        bool connect(int providerindex, bool showwarn)
        {
            debug("[TLClient_MQ]Connect to prvider....");
            if ((providerindex >= servers.Count) || (providerindex < 0))
            {
                v("     Ensure provider is running and Mode() is called with correct provider number.   invalid provider: " + providerindex);
                return false;
            }

            try
            {
                v("     Attempting connection to server: " + serverip[providerindex]);
                if ((_mqcli != null) && (_mqcli.isConnected))
                {
                    _mqcli.Disconnect();
                    markdisconnect();
                }
                //实例化asyncClient并绑定对已的函数
                _mqcli = new AsyncClient(serverip[providerindex],port,VerboseDebugging);
                _mqcli.SendDebugEvent +=new DebugDelegate(debug);
                _mqcli.SendTLMessage += new TradingLib.HandleTLMessageClient(handle);
                //开始启动连接
                _mqcli.Start();
                _lastheartbeat = DateTime.Now.Ticks;
                if (_mqcli.isConnected)
                {
                    // set our name 获得连接的唯一标识序号
                    _name = _mqcli.ID;
                    // notify
                    debug("     connected to server: " + serverip[providerindex]+":"+this.port + " via:" + Name);
                    _reconnectreq = false;
                    _recvheartbeat = true;
                    _requestheartbeat = true;
                    _connect = true;
                }
                else
                {
                    _connect = false;
                    v("     unable to connect to server at: " + serverip[providerindex].ToString());
                }

            }
            catch (Exception ex)
            {
                v("     exception creating connection to: " + serverip[providerindex].ToString()+ex.ToString());
                v(ex.Message + ex.StackTrace);
                _connect = false;
            }
            return _connect;
        }

        bool retryconnect()
        {
            v("     disconnected from server: " + serverip[_curprovider] + ", attempting reconnect...");
            bool rok = false;
            int count = 0;
            while (count++ < _disconnectretry)
            {
                rok = connect(_curprovider, false);
                if (rok)
                    break;
            }
            v(      rok ? "reconnect suceeded." : "reconnect failed.");
            return rok;
        }

        #endregion




        /// <summary>
        /// 用于通过ip地址来获得对应的provider名称
        /// </summary>
        /// <returns></returns>
        public Providers [] TLFound()
        {
            v("[TLClient_MQ]Searching provider list...");
            v("     clearing existing list of available providers");
            servers.Clear();
            // get name for every server provided by client
            foreach (string ip in serverip)
            {
                try
                {
                    v("     Attempting to connect to: " + ip + ":" + port.ToString());
                    //只需要让asynclient向某个特定的ip地址发送个寻名消息即可
                    DebugDelegate f;
                    if (VerboseDebugging)
                        f = debug;
                    else
                        f = v;
                    int pcode = Convert.ToInt16(AsyncClient.HelloServer(ip, port,f));
                    try
                    {
                        Providers p = (Providers)pcode;
                        if (p != Providers.Unknown)
                        {
                            debug("     Found provider: " + p.ToString() + " at: " + ip+":"+port.ToString());
                            servers.Add(p);//将有用的brokername保存到server中
                        }
                        else
                        {
                            debug("     skipping unknown provider at: " + ip + ":" + port.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        debug("     error adding providing at: " + ip + ":" + port.ToString() + " pcode: " + pcode);
                        debug(ex.Message + ex.StackTrace);
                    }
                }
                catch (Exception ex)
                {
                    debug("     exception connecting to server: " + ip + ":" + port.ToString());
                    debug(ex.Message + ex.StackTrace);
                    throw new QSNoServerException();
                }
            }
            v("     found " + servers.Count + " providers.");
            return servers.ToArray();
        }


        void debug(string msg)
        {
            if (SendDebugEvent != null)
                SendDebugEvent(msg);
        }

        int _tickerrors = 0;
        bool _noverb = false;
        public bool VerboseDebugging { get { return !_noverb; } set { _noverb = !value; } }

        void v(string msg)
        {
            if (_noverb) return;
            debug(msg);
        }

        //消息处理逻辑
        void handle(MessageTypes type, string msg)
        {
            long result = 0;
            switch (type)
            {
                case MessageTypes.TICKNOTIFY:
                    //debug("got tick");
                    Tick t;
                    try
                    {
                        _lastheartbeat = DateTime.Now.Ticks;
                        t = TickImpl.Deserialize(msg);

                    }
                    catch (Exception ex)
                    {
                        _tickerrors++;
                        debug("Error processing tick: " + msg);
                        debug("TickErrors: " + _tickerrors);
                        debug("Error: " + ex.Message + ex.StackTrace);
                        break;
                    }
                    //debug("got a tick:"+t.ToString());
                    if (gotTick != null) 
                        gotTick(t);
                    break;
                case MessageTypes.IMBALANCERESPONSE:
                    Imbalance i = ImbalanceImpl.Deserialize(msg);
                    _lastheartbeat = DateTime.Now.Ticks;
                    if (gotImbalance != null)
                        gotImbalance(i);
                    break;
                case MessageTypes.ORDERCANCELRESPONSE:
                    {
                        long id = 0;
                        _lastheartbeat = DateTime.Now.Ticks;
                        if (gotOrderCancel != null)
                            if (long.TryParse(msg, out id))
                                gotOrderCancel(id);
                            else if (SendDebugEvent!=null)
                                SendDebugEvent("Count not parse order cancel: " + msg);
                    }
                    break;
                case MessageTypes.EXECUTENOTIFY:
                    _lastheartbeat = DateTime.Now.Ticks;
                    // date,time,symbol,side,size,price,comment
                    Trade tr = TradeImpl.Deserialize(msg);
                    debug("prased symbol:"+tr.symbol);
                    if (gotFill != null) gotFill(tr);
                    debug("execution is prased:->" + tr.ToString());
                    break;
                case MessageTypes.ORDERNOTIFY:
                    _lastheartbeat = DateTime.Now.Ticks;
                    Order o = OrderImpl.Deserialize(msg);
                    if (gotOrder != null) gotOrder(o);
                    break;
                case MessageTypes.HEARTBEATRESPONSE:
                    {
                        _lastheartbeat = DateTime.Now.Ticks;
                        v("got heartbeat response at: " + _lastheartbeat);
                        _recvheartbeat = !_recvheartbeat;
                    }
                    break;
                case MessageTypes.POSITIONRESPONSE:
                    try
                    {
                        _lastheartbeat = DateTime.Now.Ticks;
                        Position pos = PositionImpl.Deserialize(msg);
                        if (gotPosition != null) gotPosition(pos);
                    }
                    catch (Exception ex)
                    {
                        if (SendDebugEvent!=null)
                            SendDebugEvent(msg+" "+ex.Message + ex.StackTrace);
                    }
                    break;

                case MessageTypes.ACCOUNTRESPONSE:
                    _lastheartbeat = DateTime.Now.Ticks;
                    if (gotAccounts != null) gotAccounts(msg);
                    break;
                case MessageTypes.FEATURERESPONSE:
                    debug("#Feature Response Arrived ....");
                    _lastheartbeat = DateTime.Now.Ticks;
                    string[] p = msg.Split(',');
                    List<MessageTypes> f = new List<MessageTypes>();
                    _rfl.Clear();
                    foreach (string s in p)
                    {
                        try
                        {
                            MessageTypes mt = (MessageTypes)Convert.ToInt32(s);
                            f.Add(mt);
                            _rfl.Add(mt);
                            //debug(mt.ToString());
                        }
                        catch (Exception) { }
                    }
                    //检查是否支持tick然后我们就可以启动tickreceive
                    checkTickSupport();
                    if (gotFeatures != null) 
                        gotFeatures(f.ToArray());
                    break;
                case MessageTypes.VERSIONRESPONSE:
                    debug("#Version Response Arrived ...."+msg.ToString());
                    _serverversion = Convert.ToInt16(msg);
                    break;
                case MessageTypes.SERVERDOWN:
                    if (gotServerDown != null)
                        gotServerDown(msg);
                    break;
                case MessageTypes.SERVERUP:
                    if (gotServerUp != null)
                        gotServerUp(msg);
                    break;
                default:
                    //debug("Unkonw Message Arrived.............");
                    _lastheartbeat = DateTime.Now.Ticks;
                    if (gotUnknownMessage != null)
                    {
                        gotUnknownMessage(type, 0, 0, 0, string.Empty, ref msg);
                    }
                    break;
            }
            result = 0;
        
        }

            
            
    }

    // credit : http://www.codeproject.com/KB/IP/realtimeapp.aspx


}
