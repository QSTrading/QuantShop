using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradeLink;
using TradingLib;
using TradeLink.API;
using TradingLib.API;
using TradingLib.GUI.Server;
using TradingLib.GUI;
using TradingLib.Core;
using TradeLink.Common;
using System.Reflection;
using TradingLib.Misc;
using System.IO;
using System.Reflection;
using TicTacTec.TA.Library;
using TradingLib.Web.HTTP;
using System.Web;
using System.Net;
using ZMQ;
using System.Threading;
using TradingLib.Transport;
namespace demo2
{
    public partial class cli : Form
    {
        public cli()
        {
            InitializeComponent();
        }

        Type _t = null;
        private void loadClass()
        {
            //Assembly.
            Dictionary<string, Type> dicRule = new Dictionary<string, Type>();
            foreach (Type t in Assembly.Load("RiskRuleSet").GetTypes())
            {
                //object[] attributes = t.GetCustomAttributes(typeof(Tablename), false);
                //debug(t.ToString());

                //得到RuleSet的名称与描述
                string rsname = (string)t.InvokeMember("Name",
                    BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic |BindingFlags.Static | BindingFlags.GetProperty,
                    null,null,null);
                string rsdescription = (string)t.InvokeMember("Description",
                    BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetProperty,
                    null, null, null);
                debug(rsname+"|"+rsdescription);
                dicRule.Add(rsname, t);
                //if (t.GetInterface("IRuleCheck") != null)
                //生成该ruleset实例
                //    debug(((IRuleCheck)Activator.CreateInstance(t)).ToString());

                _t = t;
            }
            //ctRuleSet1.loadRuleSetList(dicRule); 
        }


        private void debug(string msg)
        {
            debugControl1.GotDebug(msg);

            
        }


        /*
        private void startClient2()
        {
            ZmqRequestSocket request_socket;
            request_socket = new ZmqRequestSocket();
            request_socket.Connect("tcp://127.0.0.1:5555");

            request_socket.OnSend += (msg, mp) =>
            {

                debug("Sent {0} byte request:"+msg.Length.ToString());
            };

            request_socket.OnReceive += () =>
            {

                Byte[] msg;
                request_socket.Receive(out msg, true);
                var message = Encoding.ASCII.GetString(msg);

                if (message.StartsWith("ack") == false)
                {

                    throw new Exception();
                }

                debug("Got {0} byte reply."+msg.Length.ToString());
            };

            request_socket.Send(Encoding.ASCII.GetBytes("syn"));

        }

        void requester_OnReceive()
        {
            throw new NotImplementedException();
        }*/
        Socket client;
        public  void ClientTask()
        {
            using (var context = new Context(1))
            {
                using (client = context.Socket(SocketType.DEALER))
                {
                    //  Generate printable identity for the client
                    ZHelpers.SetID(client, Encoding.Unicode);
                    string identity = client.IdentityToString(Encoding.Unicode);
                    client.Connect("tcp://localhost:5570");
                    client.Send(" ", Encoding.Unicode);
                }
            }
        }


        Socket requester;
        //Context context = new Context(1);
        //const string requestMessage = "Hello";
        private void startClient()
        {
            debug("start client ");
            using (var context = new Context(1))
            {
                using ( requester = context.Socket(SocketType.REQ))
                {
                    requester.Connect("tcp://localhost:5570");

                    const string requestMessage = "Hello";
                    const int requestsToSend = 10;
                    //这里只是工作
                    /*
                    for (int requestNumber = 0; requestNumber < requestsToSend; requestNumber++)
                    {
                        //Console.WriteLine("Sending request {0}...", requestNumber);
                        debug("Sending request");
                        requester.Send(requestMessage, Encoding.Unicode);
                        //造成线程等待
                        string reply = requester.Recv(Encoding.Unicode);
                        debug("Received reply {");
                        //Console.WriteLine("Received reply {0}: {1}", requestNumber, reply);
                    }
                    */
                    
                    requester.PollInHandler += (socket, revents) =>
                    {
                        var zmsg = new ZMessage(socket);
                        debug(zmsg.Body.ToString());
                    };
                    while (true)
                    {
                        //string reply = requester.Recv(Encoding.Unicode);
                        //debug("Received reply {");
                        //debug("waiting for working...");
                        Thread.Sleep(1000);
                    }
                    

                }
            }
            
        }


        

        private void button4_Click(object sender, EventArgs e)
        {
            //_tlmq.TLSend(textBox1.Text);
            //var zmsg = new ZMessage("");
            //string s = String.Format("send message: {0}", textBox1.Text);
            //zmsg.StringToBody(s);
            //zmsg.Send(client);
            //debug("we are send message:" + s);
            _mqCli.Send(textBox1.Text);
            //byte[] data = TradeLink.Common.Message.sendmessage(MessageTypes.REGISTERCLIENT,"192.168.1.11");
            //ZMessage zmsg = new ZMessage(data);
            //zmsg.Send(client);

            /*
            //客户端每发一个消息都要取回1个回报,这样才完成一次Req,然后才可以进行下一次Req
            debug("Sending request:" + textBox1.Text);
            debug("Sending request");
            requester.Send(textBox1.Text, Encoding.Unicode);
            //造成线程等待
            string reply = requester.Recv(Encoding.Unicode);
            debug("Received reply {");
             * */

        }


        
        TradingLib.Core.BrokerFeed _bf;
        private void button1_Click(object sender, EventArgs e)
        {
            

            

            //Thread t = new Thread(new ThreadStart(intclient));
            //t.Start();
           
            
            
        }
        public const int TOTAL_PERIODS = 100;
        public const int PERIODS_AVERAGE = 30;

        private void button2_Click(object sender, EventArgs e)
        {
            debug("########Start Broker Feed....");
            if (!_bf.FeedClient.IsConnected)
                _bf.FeedClient.Start();//用于启动服务
            if (!_bf.BrokerClient.IsConnected)
                _bf.BrokerClient.Start();

            /*
            int start = Convert.ToInt32(textBox1.Text);

            double[] closePrice = new double[TOTAL_PERIODS];
            double[] output = new double[TOTAL_PERIODS];
            int begin;//开始计算的序号
            int length;
            for (int i = 0; i < closePrice.Length; i++)
            {
                closePrice[i] = (double)i;
            }
            //output是从第一位置开始填充结果的
            //begin是指从数组的哪个位置开始计算
            TicTacTec.TA.Library.Core.RetCode retCode = Core.Sma(start, closePrice.Length - 1, closePrice, PERIODS_AVERAGE, out begin, out length, output);

            if (retCode == TicTacTec.TA.Library.Core.RetCode.Success)
            {
                debug("Output Begin:" + begin);
                debug("Output lengeth:" + length);
                debug("Latest value:" + output[0].ToString());
                for (int i = begin; i < output.Length; i++)
                {
                    StringBuilder line = new StringBuilder();
                    line.Append("Period #");
                    line.Append(i + 1);
                    line.Append(" close= ");
                    line.Append(closePrice[i]);
                    line.Append(" mov avg=");
                    line.Append(output[i-begin]);
                    debug(line.ToString());
                }
            }
             * */

        }

        private void button3_Click(object sender, EventArgs e)
        {
            _tlmq.Disconnect();
            /*
             debug("########Stop Broker Feed....");
            if(_bf.FeedClient.IsConnected)
                _bf.FeedClient.Disconnect();//停止服务的同时我们需要向服务器发送cliearClient消息
            if (_bf.BrokerClient.IsConnected)
            {
                //acc = null;
                _bf.BrokerClient.Disconnect();
            }*/
        }

        private void button5_Click(object sender, EventArgs e)
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
                    debug("send message to self");
                    string reply = requester.Recv(Encoding.Unicode);
                    debug("==>send message to self"+reply);
                    
                    return;
                }
            }
        }

        void initbf()
        {
            string[] servers = { "192.168.1.144" };
            int serverport =5570;
            _bf = new TradingLib.Core.BrokerFeed(Providers.eSignal, Providers.eSignal, true, false, "QSTrading", servers, serverport);
            _bf.VerboseDebugging = true;


            _bf.SendDebugEvent += new DebugDelegate(debug);
            debug("Initfeeds Called:Init BrokerFeed");
            _bf.Reset();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(initbf));
            t.Start();
        }


        AsyncClient _mqCli;
        TLClient_MQ _tlmq;

        void intasyclient()
        {
            _mqCli = new AsyncClient("192.168.1.168", 5570, true);
            _mqCli.SendDebugEvent +=new DebugDelegate(debug);
            _mqCli.Start();
            
        }

        void intclient()
        {
            _tlmq = new TLClient_MQ(new string [] {"192.168.1.168"}, 5570, 0,debug, true);
        }


        private void button7_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(intasyclient));
            t.Start();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(intclient));
            t.Start();

        }

        public void initsub()
        {
            using (var context = new Context(1))
            {
                debug("start sub");
                using (Socket subscriber = context.Socket(SocketType.SUB), syncClient = context.Socket(SocketType.REQ))
                {
                    subscriber.Connect("tcp://localhost:5572");
                    subscriber.Subscribe("", Encoding.Unicode);

                    //syncClient.Connect("tcp://localhost:5562");

                    //  - send a synchronization request
                    //syncClient.Send("", Encoding.Unicode);
                    //  - wait for synchronization reply
                   // syncClient.Recv();

                    int receivedUpdates = 0;
                    
                    while (true)
                    {
                        string msg = subscriber.Recv(Encoding.Unicode);
                        debug(msg);
                        receivedUpdates++;
                    }

                    Console.WriteLine("Received {0} updates.", receivedUpdates);
                }
            }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(initsub));
            t.Start();
        }


    }
}
