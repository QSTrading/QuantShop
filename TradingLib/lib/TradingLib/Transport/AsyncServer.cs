using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZMQ;
using System.Threading;
using TradeLink.API;
using TradingLib.API;
using TradeLink.Common;


namespace TradingLib.Transport
{
    /// <summary>
    /// 系统服务端的数据传输层,用于建立底层的信息交换业务
    /// </summary>
    public class AsyncServer
    {
        public event DebugDelegate SendDebugEvent;
        public event HandleTLMessageDel GotTLMessage;
        private void debug(string msg)
        {
            if (SendDebugEvent != null)
                SendDebugEvent(msg); ;
        }
        bool _noverb = false;
        public bool VerboseDebugging { get { return !_noverb; } set { _noverb = !value; } }
        private void v(string msg)
        {
            if (!_noverb)
                debug(msg);
        }
        
        //类似于TLServer_IP 我们这里需要指定消息类型,消息内容,消息来源
        private long handleMessage(MessageTypes type,string msg,string address)
        {
            if(GotTLMessage!=null)
            {
                return GotTLMessage(type, msg,address);
            }
            return -1;
        }
        private int _worknum = 5;
        public int WorkerNumber { get { return _worknum; }
            set
            {
                if (value < 2)
                    _worknum = 2;
                else
                    _worknum = value;
        } }
        private string _serverip=string.Empty;
        private int _port=-1;
        public int Port { get {
            if (_port == -1)
                return 5570;
            return _port;
        }
            set { _port = value; }
        }
        public AsyncServer(bool verb)
        {
            VerboseDebugging = verb;
        }
        Thread _srvThread;
        Thread _namesThread;
        List<Thread> workers;
        bool _srvgo = false;//路由线程运行标志
        bool _workergo = false;//worker线程运行标志
        bool _started = false;
        bool _mainthreadready = false;
        bool _serverDNSthreadready = false;
        //这里需要注意关闭 启动的细节。防止服务器崩溃
        public void Start()
        {
            if (_started)
                return;
            debug("Start Message Transport Server....");
            //启动主服务线程
            _workergo = true;
            _srvgo = true;
            _srvThread = new Thread(new ThreadStart(MessageRoute));
            _srvThread.IsBackground = true;
            _srvThread.Start();

            debug("Start server Names REQ Server....");
            //启动服务响应查询线程
            _namesgo = true;
            _namesThread = new Thread(new ThreadStart(ServerDNS));
            _namesThread.IsBackground = true;
            _namesThread.Start();
            //启动订阅服务线程
            int _wait = 0;
            //用于等待线程中的相关服务启动完毕 这样函数返回时候服务已经启动完毕 相当于阻塞了线程
            //防止过早返回 服务没有启动造成的程序混乱
            //这里是否可以用thread.join来解决？
            while((_mainthreadready!=true || _serverDNSthreadready !=true )&&(_wait++ < 5))
            {
                v("#:"+_wait.ToString()+ "AsyncServer is starting.....");
                Thread.Sleep(500);
            }
            //当主线程与名成查询线程全部启动时候我们认为服务启动完毕否则我们抛出异常
            if (_mainthreadready && _serverDNSthreadready)
                _started = true;
            else
                throw new QSAsyncServerError();
        }

       
        public void demoSend1(string address)
        { 
             Message msg = new Message(MessageTypes.EXECUTENOTIFY,"it is ok"+"@"+address);
             byte[] data = TradeLink.Common.Message.sendmessage(msg);

             ZMessage zmsg = new ZMessage(data);
             zmsg.Wrap(Encoding.Unicode.GetBytes(address), Encoding.Unicode.GetBytes(""));
             zmsg.Send(_outputChanel);
             debug("address is:" + zmsg.AddressToString());
             debug("output send the message out.....");
        }
        public bool isAlive { get { return _srvThread.IsAlive; } }
        public bool isMainServerAlive { get { return _srvThread.IsAlive; } }
        public bool isNameServerAlive { get { return _namesThread.IsAlive; } }
        public bool Runing { get { return false; } }

        public void Stop()
        {
            debug("Stop Message Transport Server....");
            if (!_started)
                return;
            _workergo = false;
            foreach(Thread w in workers)
            {
                w.Abort();
            }
            _srvgo = false;
            _srvThread.Abort();
            _mainthreadready = false;

            debug("Stop Message Transport Server....");
            _namesgo = false;
            _namesThread.Abort();
            _serverDNSthreadready = false;
            //selfStopMessage();
            //stopMainserver();
            new Thread(stopDNSserver).Start();
            new Thread(stopMainserver).Start();
            //stopDNSserver();
            int _wait = 0;
            while((isMainServerAlive == true || isNameServerAlive == true) && (_wait++ < 5))
            {
                v("#:" + _wait.ToString() +"  #mainthread:"+isMainServerAlive.ToString()+" #dnsthread:"+isNameServerAlive.ToString()+ "  AsyncServer is stoping.....");
                //_maincontext.Dispose();
                //_namecontext.Dispose();
                Thread.Sleep(1000);
            }
            //当主线程与名成查询线程全部启动时候我们认为服务启动完毕否则我们抛出异常
            if (!isMainServerAlive && !isNameServerAlive)
                _started = false;
            else
                throw new QSAsyncServerError();
            v("AsyncServer shutdown clearly...");
         
        }

        private void stopMainserver()
        {
            using (var context = new Context(1))
            {
                using (Socket client = context.Socket(SocketType.DEALER))
                {
                    //  Generate printable identity for the client
                    ZHelpers.SetID(client, Encoding.Unicode);
                    string identity = client.IdentityToString(Encoding.Unicode);
                    client.Connect("tcp://localhost:5570");
                    client.Send(" ", Encoding.Unicode);
                }
            }
        }
        private void stopDNSserver()
        {
            using (var context = new Context(1))
            {
                using (Socket requester = context.Socket(SocketType.REQ))
                {
                    requester.Connect("tcp://localhost:5571");
                    //byte[] nrequest = Message.sendmessage(MessageTypes.BROKERNAME, string.Empty);
                    TradeLink.Common.Message msg = new TradeLink.Common.Message(MessageTypes.BROKERNAME, " ");
                    byte[] data = TradeLink.Common.Message.sendmessage(msg);
                    ZMessage zmsg = new ZMessage("SHUT");
                    zmsg.Send(requester);
                    Thread.Sleep(20);
                    //debug("send message to self");
                    string reply = requester.Recv(Encoding.Unicode);
                    //debug("==>send message to self" + reply);
                    return;
                }
            }
        }

        ~AsyncServer()
        {
            try
            {
                Stop();

            }
            catch { }
        }

        //服务端名称查询 用于客户端检测是否存在我们系统内的服务器
        private bool _namesgo;
        Providers _pn = Providers.Unknown;
        public Providers newProviderName { get { return _pn; } set { _pn = value; } }
        Context _namecontext;
        private void ServerDNS()
        {
            using (var context = new Context(1))
            {
                using (Socket replyer = context.Socket(SocketType.REP))
                {
                    replyer.Bind("tcp://*:" + (Port + 1).ToString());
                    //这里实现一个收发循环，如果没有消息进来则repley.recv就会停止在那里
                    //同步的信息发送1对1
                    _serverDNSthreadready = true;
                    _namecontext = context;
                    ZMessage zmsg;
                    while (_namesgo && getNameLookup(replyer,out zmsg))
                    {
                        Thread.Sleep(10);
                        
                    }
                    //_serverDNSthreadready = false;
                }
            }
        }

        private bool getNameLookup(Socket socket, out ZMessage zmsg)
        {
            zmsg = new ZMessage(socket);
            if (zmsg.BodyToString().Equals("SHUT"))
            {
                socket.Send("shutdowing", Encoding.Unicode);
                return false;
            }
            v("waiting new Name Lookup....");
            //var zmsg = new ZMessage(replyer);
            TradeLink.Common.Message msg = TradeLink.Common.Message.gotmessage(zmsg.Body);
            v("ServerDNS Got Message:" + msg.Type.ToString() + "|" + msg.Content.ToString());
            if (msg.Type == MessageTypes.BROKERNAME)
            {   //如果客户端请求名字查询则我们返回brokername 客户端根据这个返回 确认服务器具备服务能力 具体具有哪些功能
                //由FeatureList来决定
                int id = (int)newProviderName;
                socket.Send(id.ToString(), Encoding.Unicode);
                //debug("cccccccccc");
                //Thread.Sleep(5000);
            }
            return true;
        }
       
        //服务器发送产生了一定的问题 是不是需要从worker下手发送返回消息？
        //服务端向客户端发送消息需要附带地址
        public void Send(string msg,string address)
        {
            ZMessage zmsg = new ZMessage(msg);
            zmsg.Wrap(Encoding.Unicode.GetBytes(address), Encoding.Unicode.GetBytes(""));
            zmsg.Send(_outputChanel);
            
        }

        public void Send(byte[] body, string address)
        {
            ZMessage zmsg = new ZMessage(body);
            zmsg.Wrap(Encoding.Unicode.GetBytes(address), Encoding.Unicode.GetBytes(""));
            zmsg.Send(_outputChanel);
        }

        /*
        public void SendTick(Tick k)
        {
            //debug("发送tick数据");
            KVMessage tickmsg = new KVMessage(0, k.symbol,TickImpl.Serialize(k));
            tickmsg.Send(_tickpub);
        }*/


        public void SendTick(byte [] tick)
        {
            _tickpub.Send(tick);
        }

        //传输层前端
        Socket _outputChanel;//用于服务端主动向客户端发送消息
        Socket _tickpub;//用于转发Tick数据
        Context _maincontext;
        private void MessageRoute()
        {
            workers = new List<Thread>(WorkerNumber);
            using (var context = new Context(1))
            {   //当server端返回信息时,我们同样需要借助一定的设备完成
                using (Socket frontend = context.Socket(SocketType.ROUTER), backend = context.Socket(SocketType.DEALER), output = context.Socket(SocketType.DEALER), outClient = context.Socket(SocketType.DEALER), publisher = context.Socket(SocketType.PUB))
                {
                    frontend.Bind("tcp://*:"+Port.ToString());
                    backend.Bind("inproc://backend");
                    //outClient.Connect("tcp://127.0.0.1:);//注册到frontend就可以向front router发送消息
                    //outClient.Connect("inproc://backend");//连接到backend就会发生该socket就成为了一个worker会接收backend转发过来的消息
                    output.Bind("inproc://output");
                    _outputChanel = outClient;
                    outClient.Connect("inproc://output");
                    //tick数据转发
                    publisher.Bind("tcp://*:"+(Port+2).ToString());
                    _tickpub = publisher;
                    _maincontext = context;

                    for (int workerNumber = 0; workerNumber < WorkerNumber; workerNumber++)
                    {
                        workers.Add(new Thread(MessageTranslate));
                        workers[workerNumber].IsBackground = true;
                        workers[workerNumber].Start(context);
                    }

                    //在Frontend与Backend之间进行信息交换
                    //fronted过来的信息我们路由到backend上去
                    frontend.PollInHandler += (socket, revents) =>
                    {
#if DEBUG
                        v("frontend->backent");
#endif
                        var zmsg = new ZMessage(socket);
                        zmsg.Send(backend);
                    };
                    //backend过来的信息我们路由到frontend上去
                    backend.PollInHandler += (socket, revents) =>
                    {
#if DEBUG
                        v("backend->frontend");
#endif
                        var zmsg = new ZMessage(socket);
                        zmsg.Send(frontend);
                    };

                    output.PollInHandler += (socket, revents) =>
                        {
#if DEBUG
                            v("server side send the message outside");
                            v("output->frontend");
#endif
                            var zmsg = new ZMessage(socket);
#if DEBUG
                            v("address is:" + zmsg.AddressToString());
#endif
                            zmsg.Send(frontend);
                        };
                    var sockets = new List<Socket> { frontend, backend ,output};
                    //让线程一直获取由socket发报过来的信息
                    _mainthreadready = true;
                    while (_srvgo)
                    {
                        //debug("polling.......");
                        Context.Poller(sockets);
                    }
                    //_mainthreadready = false;
                }
            }
        }
        //传输层消息翻译与分发
        private void MessageTranslate(object context)
        {
            var randomizer = new Random(DateTime.Now.Millisecond);
            using (Socket worker = ((Context)context).Socket(SocketType.DEALER))
            {
                //将worker连接到backend用于接收由backend中继转发过来的信息
                worker.Connect("inproc://backend");
                
                while (_workergo)
                {
#if DEBUG
                    v("worker is waiting new message for translation");
#endif
                    //服务端从这里得到客户端过来的消息
                    //Server收到信息-->TradeLink Message(将消息转换成Tradelink消息)
                    var zmsg = new ZMessage(worker);
                    TradeLink.Common.Message msg = TradeLink.Common.Message.gotmessage(zmsg.Body);
                    string address= zmsg.AddressToString();
#if DEBUG
                    debug("MQ Got a message from :" + address.ToString() + "$ " + msg.Type.ToString() + " | " + msg.Content.ToString());
                    v("handled this message");
#endif
                    handleMessage(msg.Type, msg.Content, address);

                }
            }
        }
    }
}
