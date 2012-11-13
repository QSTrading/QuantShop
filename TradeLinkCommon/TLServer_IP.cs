using System;
using System.Collections.Generic;
using System.Text;
using TradeLink.API;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TradeLink.Common
{
    /// <summary>
    /// tradelink servers allow tradelink clients to talk to any supported broker with common interface.
    /// this version of server supports communication with clients via windows messaging.
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    public class TLServer_IP : TLServer
    {
        /// <summary>
        /// ���ĳ��symbol�Ƿ��Ѿ�ע��
        /// </summary>
        /// <param name="sym"></param>
        /// <returns></returns>
        public bool SymbolSubscribed(string sym)
        {
            for (int i = 0; i < client.Count; i++)
            {
                if (client[i] == string.Empty) continue;
                else if (stocks[i].Contains(sym))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// ��������ע��symbol��basket
        /// </summary>
        public Basket AllClientBasket
        {
            get
            {

                Basket b = new BasketImpl();
                for (int i = 0; i < stocks.Count; i++)
                    b.Add(BasketImpl.FromString(stocks[i]));
                return b;
            }
        }
        /// <summary>
        /// �������ӵ�client����
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public string ClientName(int num) { return client[num]; }

        /// <summary>
        /// ����ĳ��client��ע���symbol
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public string ClientSymbols(string client)
        {
            int cid = client.IndexOf(client);
            if (cid < 0) return string.Empty;
            return stocks[cid];
        }

        /// <summary>
        /// �����˻�������Ϣ
        /// </summary>
        /// 
        private void updateAccountConnect(string acc,bool w,string socketname)
        {
            if (SendAccountUpdateEvent != null)
                SendAccountUpdateEvent(acc, w, socketname);
    
        }
        

        ~TLServer_IP()
        {
            try
            {
                Stop();

            }
            catch { }
        }



        


        IPAddress _addr;
        System.ComponentModel.BackgroundWorker _at = new System.ComponentModel.BackgroundWorker();
        Socket _list;
        RingBuffer<Tick> tickq;
        IAsyncResult _myresult = null;
        bool _startedthread = false;
        long _lastheartbeat = 0;
        
        protected int MinorVer = 0;
        string _name = string.Empty;
        
        int _wait = 5;
        public int WaitDelayMS { get { return _wait; } set { _wait = value; } }
        
        int _port = 0;
        public int Port { get { return _port; } }
        
        bool _started = false;
        public bool isStarted { get { return _started; } }
        
        int _maxoutstandingreq = 50;
        public int MaxOustandingRequests { get { return _maxoutstandingreq; } set { _maxoutstandingreq = value; } }
        
        bool _queueb4send = false;
        public bool QueueTickBeforeSend { get { return _queueb4send; } set { _queueb4send = value; } }
        
        bool _doe = true;
        public bool DisconnectOnError { get { return _doe; } set { _doe = value; } }
        
        public int NumClients { get { return client.Count; } }

        Providers _pn = Providers.Unknown;
        public Providers newProviderName { get { return _pn; } set { _pn = value; } }

        public string Version() { return Util.TLSIdentity(); }

        

       

        void checkheartbeat()
        {
            long now = DateTime.Now.Ticks;
            long diff = (now-_lastheartbeat)*10000;
            // don't send heartbeat if not needed
            if (diff < IPUtil.SENDHEARTBEATMS)
                return;
            // otherwise attempt to send heartbeat
            for (int i = 0; i < client.Count; i++)
                if (client[i] != string.Empty)
                {
                    v("sending heartbeat to: " + client[i]+" at "+now);
                    TLSend(string.Empty, MessageTypes.HEARTBEATRESPONSE, i);
                }
            _lastheartbeat = now;
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
        public static string HexToString(byte[] buf, int len)
        {
            string Data1 = "";
            string sData = "";
            int i = 0;
            while (i < len)
            {
                //Data1 = String.Format(�{0:X}? buf[i++]); //no joy, doesn�t pad
                Data1 = buf[i++].ToString("X").PadLeft(2, '0'); //same as ?02X?in C
                sData += Data1;
            }
            return sData;
        }

        
        


        #region Start Stop Section
        /// <summary>
        /// Start Server
        /// </summary>
        public virtual void Start()
        {
            Start(3, 100, false);
        }
        public virtual void Start(int retries, int delayms, bool allowchangeport)
        {
            try
            {
                if (_started) return;
                Stop();
                debug("Starting server...");
                int attempts = 0;
                while (!_started && (attempts++ < retries))
                {
                    debug("Starting server at: " + _addr.ToString() + ":" + _port.ToString());
                    IPEndPoint end = new IPEndPoint(_addr, _port);
                    try
                    {

                        _list = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        _list.Bind(end);
                        _list.Listen(MaxOustandingRequests);
                        _myresult = _list.BeginAccept(new AsyncCallback(ReadSocket), new socketinfo(_list));
                    }
                    catch (SocketException ex)
                    {
                        Stop();
                        v("start attempt #" + attempts + " failed: " + ex.Message + ex.StackTrace);
                        Thread.Sleep(delayms);
                        if (allowchangeport)
                        {
                            Random r = new Random();
                            _port += r.Next(1, 50);
                        }
                    }
                    debug("Server can handle pending requests: " + MaxOustandingRequests);
                    debug("Starting background threads to process requests and ticks.");
                    _started = _list.IsBound;
                }

                debug("Server started.");

            }
            catch (Exception ex)
            {
                debug(ex.Message + ex.StackTrace);
                return;
            }


        }
        //����client������������ʱ��readsocket����
        void ReadSocket(IAsyncResult ir)
        {
            // get state
            socketinfo si = (socketinfo)ir.AsyncState;
            // get listener
            Socket list = si.sock;
            // get data from client
            Socket client = (Socket)list.EndAccept(ir);
            // get socket info for client
            socketinfo csi = new socketinfo(client, si.buffer, si.startidx);
            // get client name
            string name = client.RemoteEndPoint.ToString();
            // notify
            debug("Connection from: " + name);
            try
            {
                // receive data that arrives
                client.BeginReceive(csi.buffer, csi.startidx, csi.freebuffersize, SocketFlags.None, new AsyncCallback(ReadData), csi);
            }
            catch (SocketException ex)
            {
                v(client.RemoteEndPoint + " " + ex.SocketErrorCode + ex.Message + ex.StackTrace);
            }
            // wait for new connection
            list.BeginAccept(new AsyncCallback(ReadSocket), si);

        }
        //��readSocket����readData��ȡ�ͻ����͵���Ϣ��
        //readdata ���߳����յ��������sleep��ѭ�� һ�´��ڼ���״̬��ÿ�ζ�����һ�Σ�����������������û��������pass�ڽ���׼��������״̬��
        //
        void ReadData(IAsyncResult ir)
        {
            //get our state
            socketinfo csi = (socketinfo)ir.AsyncState;
            v("??????:"+csi.sock.RemoteEndPoint.ToString());
            bool connected = csi.sock.Connected;
            try
            {
                SocketError se = SocketError.Success;
                // see how much data was read for this call
                int len = csi.sock.EndReceive(ir, out se);
                // receive any data
                if (len > 0)
                {
                    v("good luck we got data");
                    try
                    {
                        // get messages from data
                        v("srv data received: socket_status: " + se.ToString() + " data size: " + csi.freebuffersize + " data: " + HexToString(csi.buffer, len));
                        Message[] msgs = Message.gotmessages(ref csi.buffer, ref csi.startidx);
                        v("srv messages received: " + msgs.Length + " messages.  ");
                        // handle messages
                        for (int i = 0; i < msgs.Length; i++)
                        {
                            Message m = msgs[i];
                            v("srv message# " + i + " size: " + m.ByteLength + " type: " + m.Type + " tag: " + m.Tag + " data: " + m.Content);
                            handle(m.Type, m.Content, csi.sock);
                        }
                        v("srv handled " + msgs.Length + " messages.");
                    }
                    catch (Exception ex)
                    {
                        debug(ex.Message + ex.StackTrace);
                    }

                }
                else
                {   
                    // implies possible disconnect, verify
                    connected = issocketconnected(csi.sock);

                }
            }
            catch (SocketException ex)
            {
                debug(ex.SocketErrorCode + ex.Message + ex.StackTrace);
            }
            catch (Exception ex)
            {
                debug(ex.Message + ex.StackTrace);
            }

            try
            {
                // wait for more data to arrive ֻ���յ���Ϣ�������,�������Ż���뵽��һ�׶�
                if (connected)
                {
                    //v("try to wait for more data to arrive");
                    Thread.Sleep(10);
                    //
                    csi.sock.BeginReceive(csi.buffer, csi.startidx, csi.freebuffersize, SocketFlags.None, new AsyncCallback(ReadData), csi);
                }
                else
                {
                    csi.sock.Close();
                }
            }
            catch (SocketException ex)
            {
                // host likely disconnected
                //����ͻ��˶Ͽ��������������ڶ�ȡ���ݵ�ʱ�������쳣 ������Ҫ�ر����������ͷ���Դ
                v("disconnected.  msg: " + ex.SocketErrorCode);//+ ex.Message + ex.StackTrace);
                csi.sock.Close();
            }

        }
       
        /// <summary>
        /// stop the server
        /// </summary>
        public virtual void Stop()
        {
            _started = false;
            _startedthread = false;
            try
            {
                for (int i = 0; i < _sock.Count; i++)
                {
                    if (_sock[i] == null)
                        continue;
                    if (!_sock[i].Connected)
                        continue;
                    _sock[i].Shutdown(SocketShutdown.Both);
                    _sock[i].Close();

                }

                if (_list.Connected)
                {
                    _list.Shutdown(SocketShutdown.Both);
                    _list.Close();
                }
                if (_at.IsBusy)
                    _at.CancelAsync();


            }
            catch (Exception ex)
            {
                debug(ex.Message + ex.StackTrace);
            }
            debug("Stopped: " + newProviderName);
        }
        #endregion


        //public event StringDelegate newAcctRequest;
        public event AccountRequestDel newAcctRequest;
        public event OrderDelegateStatus newSendOrderRequest;
        public event LongDelegate newOrderCancelRequest;
        public event PositionArrayDelegate newPosList;
        public event SymbolRegisterDel newRegisterSymbols;
        public event MessageArrayDelegate newFeatureRequest;
        public event UnknownMessageDelegate newUnknownRequest;
        public event UnknownMessageDelegateSource newUnknownRequestSource;
        public event VoidDelegate newImbalanceRequest;
        public event Int64Delegate DOMRequest;
        public event AccountUpdateDel SendAccountUpdateEvent;

        


        


        #region TLServer_IP ���캯��
        public TLServer_IP() : this(IPAddress.Loopback.ToString(), IPUtil.TLDEFAULTBASEPORT, 50,100000) { }

        public TLServer_IP(string ipaddr, int port) : this(ipaddr, port, 25, 100000) { }
        /// <summary>
        /// create an ip server
        /// </summary>
        /// <param name="ipaddr"></param>
        /// <param name="port"></param>
        /// <param name="wait"></param>
        /// <param name="TickBufferSize">set to zero to send ticks immediately</param>
        public TLServer_IP(string ipaddr, int port, int wait, int TickBufferSize) : this(ipaddr, port, wait, TickBufferSize, null) { }
        public TLServer_IP(string ipaddr, int port, int wait, int TickBufferSize, DebugDelegate deb)
        {
            SendDebugEvent = deb;
            if (TickBufferSize == 0)
                _queueb4send = false;
            else
                tickq = new RingBuffer<Tick>(TickBufferSize);
            MinorVer = Util.ProgramBuild(Util.PROGRAM,debug);
            _wait = wait;
            if (!IPUtil.isValidAddress(ipaddr))
                debug("Not valid ip address: " + ipaddr + ", using localhost.");
            _addr = IPUtil.isValidAddress(ipaddr) ? IPAddress.Parse(ipaddr) : IPAddress.Loopback;
            _port = port;
            v("tlserver_ip wait: " + _wait);
            Start();
        }

        #endregion






        #region TLSend ��Ϣ����
        //����+Ŀ��sock���
        public void TLSend(byte[] data, int dest)
        {
            if ((dest < 0) || (dest >= _sock.Count))
                return;

            try
            {
                Socket s = _sock[dest];
                if (s == null)
                    return;
                if (s.Connected)
                {
                    try
                    {
                        s.Send(data);
                    }
                    catch (SocketException ex)
                    {
                        debug("socket exception: " + ex.SocketErrorCode + ex.Message + ex.StackTrace);
                        handleerror(dest);
                    }
                    catch (Exception ex)
                    {
                        debug("exception sending data: " + ex.Message + ex.StackTrace);
                    }
                }
            }
            catch (SocketException ex)
            {
                debug(ex.SocketErrorCode + ex.Message + ex.StackTrace);
                handleerror(dest);
            }


        }

        void handleerror(int dest)
        {
            if (DisconnectOnError)
            {
                debug("Disconnecting errored client: " + client[dest]);
                SrvClearClient(dest);
            }
        }
        //��Ϣ+��Ϣ����+clientname
        public void TLSend(string message, MessageTypes type, string clientname)
        {
            int id = client.IndexOf(clientname);
            if (id == -1) return;
            TLSend(message,type,id);
        }
        delegate void tlsenddel(string m, MessageTypes t, int dest);
        //��Ϣ+��Ϣ����+Ŀ��sock���
        public void TLSend(string message, MessageTypes type, int  dest)
        {
            Socket s = _sock[dest];
            if (s == null) return;
            TLSend(message, type, s);
        }
        public void TLSend(string msg, MessageTypes type, Socket s)
        {
            if (s.Connected)
            {
                byte[] data = Message.sendmessage(type, msg);
#if DEBUG
                v("srv sending message type: " + type + " contents: " + msg);
                v("srv sending raw data size: " + data.Length + " data: " + HexToString(data, data.Length));
#endif
                try
                {
                    s.Send(data);
                }
                catch (Exception ex)
                {
                    debug("error sending: " + type.ToString() + " " + msg);
                    debug("exception: " + ex.Message + ex.StackTrace);
                    if (DisconnectOnError)
                    {
                        debug("disconnecting from: " + s.RemoteEndPoint.ToString());
                        s.Shutdown(SocketShutdown.Both);
                        s.Disconnect(true);
                    }
                }
            }
        }
        #endregion




        // server structures ���������ݽṹ
        protected List<string> client = new List<string>();//��¼client name 192.168.1.14.11
        protected List<DateTime> heart = new List<DateTime>();//��¼����ʱ��
        protected List<string> stocks = new List<string>();//client������symbol�б�
        protected List<string> index = new List<string>();//��¼index��Ϣ
        protected List<string> accts = new List<string>();//��¼client��������Acct��Ϣ
        protected List<Socket> _sock = new List<Socket>();//socket ��¼socks��Ϣ

        #region client-->TLServer��Ϣ�������ĸ������
        //���ĳ��client��ע���symbol
        private string SrvStocks(string him)
        {
            int cid = client.IndexOf(him);
            if (cid == -1) return ""; // client not registered
            return stocks[cid];
        }
        //�������ע��ͻ���
        private string[] SrvGetClients() { return client.ToArray(); }
        //ע��index
        void SrvRegIndex(string cname, string idxlist)
        {
            int cid = client.IndexOf(cname);
            if (cid == -1) return;
            else index[cid] = idxlist;
            SrvBeatHeart(cname);
        }


        //����Order
        private void SrvDoExecute(string msg) // handle an order (= execute request)
        {
            Order o = OrderImpl.Deserialize(msg);
            if (newSendOrderRequest != null)
                newSendOrderRequest(o); //request fill

        }

        //ע��client��Ҫ��¼��clientͨ�ŵ�Socket
        void SrvRegClient(string cname, Socket s)
        {
            int idx = client.IndexOf(cname) ;
            // check for already registered
            if (idx!= -1)
            {
                // update socket
                _sock[idx] = s;
                // we're done
                return; 
            }
            client.Add(cname);
            heart.Add(DateTime.Now);
            stocks.Add("");
            index.Add("");
            accts.Add("");
            _sock.Add(s);
            SrvBeatHeart(cname);
        }
        //ע��symbols
        void SrvRegStocks(string cname, string stklist)
        {
            int cid = client.IndexOf(cname);
            if (cid == -1) return;
            v("got registration request: " + stklist + " from: " + cname);
            stocks[cid] = stklist;
            SrvBeatHeart(cname);
            if (newRegisterSymbols != null)
            {
                newRegisterSymbols(cname,stklist);
            }
        }
        //����Ѿ�ע��symbol
        void SrvClearStocks(string cname)
        {
            int cid = client.IndexOf(cname);
            if (cid == -1) return;
            stocks[cid] = "";
            SrvBeatHeart(cname);
        }
        void SrvClearIdx(string cname)
        {
            int cid = client.IndexOf(cname);
            if (cid == -1) return;
            index[cid] = "";
            SrvBeatHeart(cname);
        }
        //ɾ��client
        public void SrvClearClient(string client, bool niceclose)
        {
            int cid = client.IndexOf(client);
            if (cid == -1) return; // don't have this client to clear him
            SrvClearClient(cid,niceclose);
        }
        void SrvClearClient(string him)
        {
            int cid = client.IndexOf(him);
            if (cid == -1) return; // don't have this client to clear him
            updateAccountConnect(accts[cid], false, "");//�����˻�������Ϣ
            SrvClearClient(cid);
            
        }
        void SrvClearClient(int cid) { SrvClearClient(cid, true); }
        void SrvClearClient(int cid, bool niceclose)
        {
            if ((cid >= client.Count) || (cid < 0)) return;
            client.RemoveAt(cid);
            stocks.RemoveAt(cid);
            heart.RemoveAt(cid);
            index.RemoveAt(cid);
            accts.RemoveAt(cid);
            
            try
            {
                if (niceclose)
                {
                    //debug("close client socket1");
                    _sock[cid].Shutdown(SocketShutdown.Both);
                    //debug("close client socket2");
                    _sock[cid].Disconnect(true);
                }
                else
                {
                    _sock[cid].Close();
                }
            }
            catch { }
            _sock.RemoveAt(cid);
        }

        //client����Account
        void SrvAcctReq(string msg)
        {
            debug(msg);
            string[] p1 = msg.Split('+');
            if (p1.Length < 2) return;

            string[] p2 = p1[1].Split(':');
            if (p2.Length < 2) return;

            string cname = p1[0];
            string ac = p2[0];
            int pass = Convert.ToInt32(p2[1]);


            int cid = client.IndexOf(cname);
            if (cid == -1) return;
            if (newAcctRequest == null) return;
            
            string acname = newAcctRequest(ac,pass);//�޸�����Ϊ�ض���Client�����˻�,��ͬ�ķ��˻�
            //������Լ����˻���֤ ����޷���֤ͨ�����˻� ���������ɾ�����ӡ�
            if (acname == string.Empty)
            {
                debug("@�˻�: " + msg + "��֤ʧ��");
                TLSend("�˻���֤ʧ��,��ȷ���������ϵ����Ա",MessageTypes.POPMESSAGE,cid);
                return;
            }
            debug("@�˻�: " + msg + "��֤�ɹ�");
            accts[cid] = acname;
            TLSend(ac, MessageTypes.ACCOUNTRESPONSE,cid);
            TLSend("�˻���֤�ɹ�,ף�㽻����죡", MessageTypes.POPMESSAGE, cid);
            updateAccountConnect(acname, true, client[cid]);
            
        }
        //��¼����������ʱ������ʱ��
        void SrvBeatHeart(string cname)
        {
            int cid = client.IndexOf(cname);
            if (cid == -1) return; // this client isn't registered, ignore
            TimeSpan since = DateTime.Now.Subtract(heart[cid]);
            heart[cid] = DateTime.Now;
        }

        void SrvDOMReq(string cname, int depth)
        {
            int cid = client.IndexOf(cname);
            if (cid == -1) return;
            SrvBeatHeart(cname);
            if (DOMRequest!=null)
                DOMRequest(depth);
        }

        #endregion


        #region TLServer -->client������Ӧ��Ϣ
        delegate void tlneworderdelegate(OrderImpl o, bool allclients);
        //public void newOrder(Order o) { newOrder(o, true); }
        public void newOrder(Order o) { newOrderViaAcct(o); }

        //��Order�����ض���Account���͵���Ӧ��Client
        public void newOrderViaAcct(Order o)
        {
            debug("TLServer send ordernotify");
            if (o.Account == null || o.Account == string.Empty) return;
            for (int i = 0; i < client.Count; i++)
                if ((client[i] != null) && accts[i].Contains(o.Account))
                    TLSend(OrderImpl.Serialize(o), MessageTypes.ORDERNOTIFY, i);
        }

        public void newOrderReject(Order o,string msg)
        {
            debug("TLServer send order rejection");
            if (o.Account == null || o.Account == string.Empty) return;
            for (int i = 0; i < client.Count; i++)
                if ((client[i] != null) && accts[i].Contains(o.Account))
                    TLSend(msg, MessageTypes.POPMESSAGE, i);
        }
        public void newOrderViaAcct(Order o, Account a)
        {
            if (!a.isValid) return;
            for (int i = 0; i < client.Count; i++)
                if ((client[i] != null) && accts[i].Contains(a.ID))
                    TLSend(OrderImpl.Serialize(o), MessageTypes.ORDERNOTIFY, i);
        }

        public void newOrder(Order o, bool allclients)
        {
            for (int i = 0; i < client.Count; i++) // send tick to each client that has subscribed to tick's stock
                if ((client[i] != null) && (allclients || (stocks[i].Contains(o.symbol))))
                    TLSend(OrderImpl.Serialize(o), MessageTypes.ORDERNOTIFY, i);

        }

        

        // server to clients
        /// <summary>
        /// Notifies subscribed clients of a new tick.
        /// </summary>
        /// <param name="tick">The tick to include in the notification.</param>
        public void newTick(Tick tick)
        {
            if (_queueb4send)
            {
                starttickthread();
                tickq.Write(tick);
            }
            else
                sendtick(tick);

        }
        void sendtick(Tick k)
        {

            byte[] data = Message.sendmessage(MessageTypes.TICKNOTIFY, TickImpl.Serialize(k));
            for (int i = 0; i < client.Count; i++) // send tick to each client that has subscribed to tick's stock
            {
                if ((client[i] != null) && stocks[i].Contains(k.symbol))
                    TLSend(data, i);
            }
        }

        //��ʼworker�������ڴ���tick
        void starttickthread()
        {
            if (_startedthread)
                return;

            _at = new System.ComponentModel.BackgroundWorker();
            _at.DoWork += new System.ComponentModel.DoWorkEventHandler(_at_DoWork);
            _at.WorkerSupportsCancellation = true;
            _at.RunWorkerAsync();
            _startedthread = true;
        }
        void _at_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (_started)
            {
                int count = 0;
                while (tickq.hasItems)
                {
                    if (e.Cancel)
                    {
                        _started = false;
                        break;
                    }
                    Tick tick = tickq.Read();
                    sendtick(tick);
                    count++;
                    if (count % 1000 == 0)
                        checkheartbeat();
                }

                if (tickq.isEmpty)
                {
                    Thread.Sleep(_wait);
                    checkheartbeat();
                }
            }
        }


        delegate void tlnewfilldelegate(TradeImpl t, bool allclients);
        /// <summary>
        /// Notifies subscribed clients of a new execution.
        /// </summary>
        /// <param name="trade">The trade to include in the notification.</param>
        public void newFill(Trade trade) { newFill(trade, true); }
        public void newFill(Trade trade, bool allclients)
        {
            debug("TLServer send Filld to client");
            // make sure our trade is filled and initialized properly
            if (!trade.isValid)
            {
                debug("invalid trade: " + trade.ToString());
                return;
            }
            for (int i = 0; i < client.Count; i++)
                if ((client[i] != null) && accts[i].Contains(trade.Account))
                    TLSend(TradeImpl.Serialize(trade), MessageTypes.EXECUTENOTIFY, i);
            /*
            for (int i = 0; i < client.Count; i++) // send tick to each client that has subscribed to tick's stock
                if ((client[i] != null) && (allclients || (stocks[i].Contains(trade.symbol))))
                    TLSend(TradeImpl.Serialize(trade), MessageTypes.EXECUTENOTIFY, i);*/
        }

        public void newCancel(long id)
        {
            newOrderCancel(id);
        }

        public void newOrderCancel(long orderid_being_cancled)
        {
                foreach (string c in client) // send order cancel notifcation to clients
                    TLSend(orderid_being_cancled.ToString(), MessageTypes.ORDERCANCELRESPONSE, c);
        }

        public void newImbalance(Imbalance imb)
        {
                for (int i = 0; i < client.Count; i++)
                    TLSend(ImbalanceImpl.Serialize(imb), MessageTypes.IMBALANCERESPONSE, i);
        }
        #endregion


        #region debug
        bool _noverb = true;
        public bool VerboseDebugging { get { return !_noverb; } set { _noverb = !value; } }
        public event DebugDelegate SendDebugEvent;

        void debug(string msg)
        {
            if (SendDebugEvent != null)
                SendDebugEvent(msg);

        }
        void v(string msg)
        {
            if (_noverb) return;
            debug(msg);
        }
        #endregion


        #region Message��Ϣ�����߼�
        const long NORETURNRESULT = long.MaxValue;
        long handle(Message m)
        {
            if (m.Tag == string.Empty) return 0;
            int cid = client.IndexOf(m.Tag);
            return handle(m.Type, m.Content, cid==-1 ? null : _sock[cid]);
        }

        //����Ϣ������ͨ��ί�����ǽ�ҵ�񲿷ְ���,���䲿��������õ��ַ�,sock��ʶ�˸���Ϣ���͸��ĸ�client
        //������ί�а����ȥ���߼����� ������Ҫ���߼����ֽ���ʵ�ֲ�����
        long handle(MessageTypes type, string msg, Socket sock)
        {
            long result = NORETURNRESULT;
            v((sock!=null  ? sock.RemoteEndPoint.ToString() : string.Empty) + " " + type.ToString() + " " + msg);
            switch (type)
            {
                case MessageTypes.ACCOUNTREQUEST:
                    debug("#Got ACCOUNTREQUEST From"+msg.ToString());
                    SrvAcctReq(msg);
                    break;
                case MessageTypes.POSITIONREQUEST:
                    debug("#Got POSITIONREQUEST From"+msg.ToString());
                    if (newPosList == null) break;
                    string[] pm = msg.Split('+');
                    if (pm.Length < 2) break;
                    string client = pm[0];
                    string acct = pm[1];
                    Position[] list = newPosList(acct);
                    foreach (Position pos in list)
                    {
                        debug("@send position to client:" + client.ToString());
                        TLSend(PositionImpl.Serialize(pos), MessageTypes.POSITIONRESPONSE, client);
                    }
                    break;
                case MessageTypes.ORDERCANCELREQUEST:
                    {
                        debug("#Got ORDERCANCELREQUEST From" + msg.ToString());
                        long id = 0;
                        if (long.TryParse(msg, out id) && (newOrderCancelRequest != null))
                            newOrderCancelRequest(id);
                    }
                    break;
                case MessageTypes.SENDORDER:
                    debug("#Got SENDORDER From" + msg.ToString());
                    SrvDoExecute(msg);
                    break;
                case MessageTypes.REGISTERCLIENT:
                    debug("#Got REGISTERCLIENT From" + msg.ToString());
                    SrvRegClient(msg,sock);
                    break;
                case MessageTypes.REGISTERSTOCK:
                    debug("#Got REGISTERSTOCK From" + msg.ToString());
                    string[] m2 = msg.Split('+');
                    SrvRegStocks(m2[0], m2[1]);
                    break;
                case MessageTypes.CLEARCLIENT:
                    debug("#Got CLEARCLIENT From" + msg.ToString());
                    SrvClearClient(msg);
                    break;
                case MessageTypes.CLEARSTOCKS:
                    debug("#Got CLEARSTOCKS From" + msg.ToString());
                    SrvClearStocks(msg);
                    break;
                case MessageTypes.HEARTBEATREQUEST:
                    debug("#Got HEARTBEATREQUEST From" + msg.ToString());
                    SrvBeatHeart(msg);
                    break;
                case MessageTypes.BROKERNAME:
                    {   //ע����TLClientFund�����ǻὨ��һ���µ�socket����ֱ�ӷ�����Ϣ����brokername,�ھ����TLclient������������и�Socket���ڲ����״̬
                        //����ڵ�һ���յ�BrokerName�����ʱ��������Ҫ�����رո�socket
                        debug("#Got BROKERNAME From" + msg.ToString());
                        result = (long)newProviderName;
                        sock.Send(BitConverter.GetBytes(result));
                        Thread.Sleep(10);
                        //debug("close client socket1");
                        sock.Shutdown(SocketShutdown.Both);
                        //debug("close client socket2");
                        sock.Disconnect(true);
                        //sock.Close();
                    }
                    break;
                case MessageTypes.IMBALANCEREQUEST:
                    if (newImbalanceRequest != null) newImbalanceRequest();
                    break;
                case MessageTypes.FEATUREREQUEST:
                    debug("#Got FEATUREREQUEST From" + msg.ToString());
                    string msf = "";
                    List<MessageTypes> f = new List<MessageTypes>();
                    f.Add(MessageTypes.HEARTBEATREQUEST);
                    f.Add(MessageTypes.HEARTBEATRESPONSE);
                    f.Add(MessageTypes.CLEARCLIENT);
                    f.Add(MessageTypes.CLEARSTOCKS);
                    f.Add(MessageTypes.REGISTERCLIENT);
                    f.Add(MessageTypes.REGISTERSTOCK);
                    f.Add(MessageTypes.FEATUREREQUEST);
                    f.Add(MessageTypes.FEATURERESPONSE);
                    f.Add(MessageTypes.VERSION);
                    f.Add(MessageTypes.BROKERNAME);
                    List<string> mf = new List<string>();
                    foreach (MessageTypes t in f)
                    {
                        int ti = (int)t;
                        mf.Add(ti.ToString());
                    }
                    if (newFeatureRequest != null)
                    {
                        MessageTypes[] f2 = newFeatureRequest();
                        foreach (MessageTypes t in f2)
                        {
                            int ti = (int)t;
                            mf.Add(ti.ToString());
                        }
                    }
                    msf = string.Join(",", mf.ToArray());
                    TLSend(msf, MessageTypes.FEATURERESPONSE, msg);
                    break;
                case MessageTypes.VERSION:
                    debug("#Got VERSIONREQUEST From" + msg.ToString());
                    result = (long)2900;
                    //ֱ��ͨ��socket�������ݷ���
                    //TLSend��ͨѶ�첽��ͬ�������ؽ�� ��Ҫ��¼����Ϣ�Ĺ��� Ȼ��׼ȷ�Ľ�����Ϣ���ͻؿͻ���
                    TLSend(result.ToString(), MessageTypes.VERSIONRESPONSE, sock);
                    break;
                case MessageTypes.DOMREQUEST:
                    string[] dom = msg.Split('+');
                    SrvDOMReq(dom[0], int.Parse(dom[1]));
                    break;
                default:
                    //��Ĭ��serverû��ʵ�ֵĹ���ͨ��default·�ɵ�����event handler��������ȥ
                    if (newUnknownRequestSource != null)
                        result = newUnknownRequestSource(type, msg, sock.RemoteEndPoint.ToString());
                    else if (newUnknownRequest != null)
                        result = newUnknownRequest(type, msg);
                    else
                        result = (long)MessageTypes.FEATURE_NOT_IMPLEMENTED;
                    break;
            }

            return result;
            
        }
        #endregion


        internal struct socketinfo
        {
            internal socketinfo(socketinfo si)
            {
                sock = si.sock;
                startidx = si.startidx;
                buffer = si.buffer;
            }
            internal Socket sock;
            internal byte[] buffer;
            internal bool haspartial { get { return startidx != 0; } }
            internal socketinfo(Socket s)
            {
                sock = s;
                buffer = new byte[s.ReceiveBufferSize];
                startidx = 0;
            }
            internal int startidx;
            internal socketinfo(Socket s, byte[] data)
            {
                startidx = 0;
                sock = s;
                buffer = data;
            }
            internal int freebuffersize { get { return buffer.Length - startidx; } }
            internal socketinfo(Socket s, byte[] data,int offset)
            {
                startidx = offset;
                sock = s;
                buffer = data;
            }


        }





    }

    // credit : http://www.codeproject.com/KB/IP/realtimeapp.aspx
}
