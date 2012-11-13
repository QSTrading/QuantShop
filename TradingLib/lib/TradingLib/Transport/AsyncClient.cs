using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZMQ;
using TradeLink.API;
using TradeLink.Common;
using System.Threading;
using System.Net;
using TradingLib.API;

namespace TradingLib.Transport
{
    /// <summary>
    /// 用于发起向服务器的底层传输连接,实现消息的收发
    /// </summary>
    public class AsyncClient
    {
        public event DebugDelegate SendDebugEvent;
        public event HandleTLMessageClient SendTLMessage;
        public event StringParamDelegate GotTick;

        private void handleMessage(MessageTypes type,string msg)
        {
            if (SendTLMessage != null)
               SendTLMessage(type, msg);
        }

        bool _noverb = false;
        public bool VerboseDebugging { get { return !_noverb; } set { _noverb = !value; } }
        private void v(string msg)
        {
            if (!_noverb)
                debug(msg);
        }
        
        private void debug(string msg)
        {
            if (SendDebugEvent != null)
                SendDebugEvent(msg);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverip">服务器地址</param>
        /// <param name="port">服务器端口</param>
        /// <param name="verbos">是否日志输出</param>
        public AsyncClient(string serverip,int port,bool verbos)
        {
            _serverip = serverip;
            _serverport = port;
            VerboseDebugging = verbos;
            
        
        }

        /// <summary>
        /// 断开与服务器的连接
        /// </summary>
        public void Disconnect()
        {
            Stop();
        }

        public bool isConnected { get { return _started; } }
        /// <summary>
        /// 启动客户端连接
        /// </summary>
        public void Start()
        {
            if (_started)
                return;
            v("[AsyncClient] start thread ....");
            try
            {
                _cligo = true;
                _cliThread = new Thread(new ThreadStart(MessageTranslate));
                _cliThread.IsBackground = true;
                _cliThread.Start();
            }
            catch (System.Exception ex)
            {
                throw new QSAsyncClientError();
            }

            int _wait = 0;
            while (!isConnected && (_wait++ < 5))
            {
                //等待1秒,当后台正式启动完毕后方可有进入下面的程序端运行
                Thread.Sleep(500);
                debug(      "#:" + _wait.ToString() + "  AsynClient is starting....");
            }
            //注意这里是通过启动线程来运行底层传输的，Start返回后 后台的传输线程并没有完全启动完毕
            //这里我们需要有一个循环用于等待启动完毕 _started = true;放在启动函数里面
            if (!isConnected)
                throw new QSAsyncClientError();
        }

        public void Stop()
        {
            if (!_started)
                return;
            
            _cliThread.Abort();
            _started = false;

           
            int _wait = 0;
            while (_cliThread.IsAlive&& (_wait++ < 5))
            {
                //等待1秒,当后台正式启动完毕后方可有进入下面的程序端运行
                Thread.Sleep(500);
                debug("#:" + _wait.ToString() + "  AsynClient is stoping....");
                _cligo = false;
            }

            StopTickReciver();

        
        }
        /// <summary>
        /// 与服务器连接时候 获得的唯一的ID标示 用于区分客户端
        /// </summary>
        public string ID { get { return _identity; } }
        public string Name { get { return ID; } }

        Thread _cliThread=null;
        Socket client=null;
        bool _cligo=false;
        bool _started=false;
        string _identity = "";
        //string _server = "tcp://localhost:5570";
        string _serverip = "192.168.1.144";
        public string ServerAddress { get { return _serverip; } set { _serverip = value; } }
        int _serverport = 5570;

        public int Port { get { return _serverport; } set {

            if (value < 1000)
                _serverport = 5570;
            else
                _serverport = value;
        } }
        //消息翻译线程,当socket有新的数据进来时候,我们将数据转换成TL交易协议的内部信息,并触发SendTLMessage事件,从而TLClient可以用于调用对应的处理逻辑对信息进行处理
        private void MessageTranslate()
        {
            using (var context = new Context(1))
            {
                using (client = context.Socket(SocketType.DEALER))
                {
                    //  Generate printable identity for the client
                    ZHelpers.SetID(client, Encoding.Unicode);
                    _identity = client.IdentityToString(Encoding.Unicode);
                    client.Connect("tcp://"+_serverip.ToString()+":"+Port.ToString());

                    client.PollInHandler += (socket, revents) =>
                    {
                        var zmsg = new ZMessage(socket);
                        TradeLink.Common.Message msg = TradeLink.Common.Message.gotmessage(zmsg.Body);
#if DEBUG  
                        v("AsyncClient:"+msg.Type.ToString() + "|" + msg.Content.ToString());
#endif
                        handleMessage(msg.Type, msg.Content);
                    };
                    //当我们运行到这里的时候才可以认为服务启动完毕
                    _started = true;
                    while (_cligo)
                    {
                        Context.Poller(new List<Socket>(new[] { client }), 10000);
                    }
                    _started = false;
                    //当我们运行到这里的时候 才可以认为
                }
            }
        }

        /// <summary>
        /// 启动Tick数据接收,如果TLClient所连接的服务器支持Tick数据,则我们可以启动单独的Tick对话流程,用于接收数据
        /// </summary>
        public void StartTickReciver()
        {
            if (_tickreceiveruning)
                return;
            _tickgo = true;
            v("Start Client Tick Reciving Thread....");
            _tickthread = new Thread(new ThreadStart(TickHandler));
            _tickthread.Start();
        }

        public void StopTickReciver()
        { 
            if(!_tickreceiveruning)
                return;
            
            v("Stop Client Tick Reciving Thread....");
            _tickthread.Abort();

            int _wait = 0;
            while (_tickthread.IsAlive && (_wait++ < 5))
            {
                
                //等待1秒,当后台正式启动完毕后方可有进入下面的程序端运行
                Thread.Sleep(500);
                debug("#:" + _wait.ToString() + "  AsynClient[Tick Reciver] is stoping....");
                _tickgo = false;
            }

        }

        Socket subscriber;
        bool _tickgo;
        Thread _tickthread;
        bool _tickreceiveruning = false;
        private void TickHandler()
        {
            using (var context = new Context(1))
            {
                using (subscriber = context.Socket(SocketType.SUB))
                {
                    //  Generate printable identity for the client
                    debug("AsynClient Connect to TickServer");
                    subscriber.Connect("tcp://"+_serverip.ToString()+":"+(Port+2).ToString());
                    subscriber.Subscribe("", Encoding.Unicode);
                    
                    _tickreceiveruning = true;
                    while (_tickgo)
                    {
                        /* 方式一
                        KVMessage msg = KVMessage.Recv(subscriber);
                        //debug("got a tick:"+msg.Body);
                        if (GotTick != null)
                            GotTick(msg.Body);
                        */
                       
                        //方式二
                        var zmsg = new ZMessage(subscriber);
                        TradeLink.Common.Message msg = TradeLink.Common.Message.gotmessage(zmsg.Body);
                        handleMessage(msg.Type, msg.Content);
                    }
                    _tickreceiveruning = false;
                }
            }
        }

        //用于检查服务器的响应,如果有数据或交易服务区 则服务器会返回一个Provider名称 用于标识服务器
        public static string HelloServer(string ip,int port,DebugDelegate debug)
        { 
           debug("[AsyncClient]Start say hello to server...");
           string rep = string.Empty;
           using (var context = new Context(1))
           {
               using (Socket requester = context.Socket(SocketType.REQ))
               {
                   string srv = "tcp://" + ip + ":" + (port + 1).ToString();
                   requester.Connect(srv);
                   //byte[] nrequest = Message.sendmessage(MessageTypes.BROKERNAME, string.Empty);
                   Message msg = new Message(MessageTypes.BROKERNAME, " ");
                   byte[] data = TradeLink.Common.Message.sendmessage(msg);
                   ZMessage zmsg = new ZMessage(data);
                   zmsg.Send(requester);
                   //requester.Send();
                   rep = requester.Recv(Encoding.Unicode);
               }
           }
           return rep;
        }

        //发送TL内部消息
        public void Send(TradeLink.Common.Message msg)
        {
            byte[] data = TradeLink.Common.Message.sendmessage(msg);
            ZMessage zmsg = new ZMessage(data);
            zmsg.Send(client);
        }
        //发送文字消息
        public void Send(string msg)
        {
            ZMessage zmsg = new ZMessage(msg);
            zmsg.Send(client);
        }
        //发送byte信息
        public void Send(byte[] msg)
        {
            ZMessage zmsg = new ZMessage(msg);
            zmsg.Send(client);
        }
    }
}
