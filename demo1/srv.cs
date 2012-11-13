using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradeLink.Common;
using TradeLink.API;
using TradeLink.AppKit;
using TradingLib;
using TradingLib.GUI;
using TradingLib.Data;
using TradingLib.Core;
using TradingLib.Data.database;
using TradingLib.Transport;

using Easychart.Finance.DataProvider;
using Easychart.Finance;
using System.Drawing;
using System.Threading;
using TradingLib.Web.HTTP;
using TradeLink.Common;
using System.IO;

using ZMQ;

namespace demo1
{
    
        


    public partial class srv : Form
    {

        TradingLib.Core.BrokerFeed _bf;
        public srv()
        {
            InitializeComponent();
            //initfeeds();
            //Thread.Sleep(100);
            //MessageBox.Show("try to init brokerfeed!");
            //Load += new EventHandler(Form1_Load);
            //quoteView1.SendDebugEvent +=new DebugDelegate(debug);
            //test();
            //ctsqlOrderView1.SendDebugEvent +=new DebugDelegate(debug);

            //用事后点击事件调用initfeeds与后台直接加载initfeeds会出现时间不同程度的延迟,造成函数
            //initfeeds();



            








            string[] servers = { "127.0.0.1" };
            int serverport = 4500;

            //_bf = new BrokerFeed(Providers.eSignal, Providers.eSignal, true, false, "QSTrading", servers, serverport);
            //_bf.VerboseDebugging = true;


            //_bf.SendDebugEvent += new DebugDelegate(debug);
            //debug("Initfeeds Called:Init BrokerFeed");
            

            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
        }

        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _bf.Stop();
        }

        void Form1_Load(object sender, EventArgs e)
        {
            test();
        }


        public void test()
        {
          
            //f.Subscribe(quoteView1);
        
        
        }

        //本窗口的debugcontrol,用于显示系统的debug输出信息
        public void debug(string s)
        {
            //_dmsg.AppendLine(s);
            //debugControl1.GotDebug(s);
            //_log.GotDebug(s);
            // m_DebugForm.GotDebug(s);
            debugControl1.GotDebug(s);
        }

        string str = "111";
        private void demo(string s)
        { 
            s="222";
            MessageBox.Show(str + "|" + s);
        }

        private void demo(ref string s)
        {
            s = "222";
            MessageBox.Show(str + "|" + s);
        }
        public void initfeeds()
        {
            string[] servers = { "127.0.0.1" };
            int serverport = 4500;

            //_bf = new BrokerFeed(Providers.eSignal, Providers.eSignal, true, false, "QSTrading", servers, serverport);
            //_bf.VerboseDebugging = true;


            //_bf.SendDebugEvent += new DebugDelegate(debug);
            //debug("Initfeeds Called:Init BrokerFeed");


            //_bf.Reset();
        }

        private Thread t;
        private void button1_Click(object sender, EventArgs e)
        {
            //需要将这些初始化工作放入线程中进行，不能用UI主线程去初始化这些事物
             t = new Thread(new ThreadStart(initfeeds));
            t.Start();
            
            //demo(str);
            //demo(ref str);

            /*
            MStockDataManager ms = new MStockDataManager("D:\\data\\");
            ms.SendDebugEvent += new DebugDelegate(debug);

            DataManagerBase base2 = new YahooCSVDataManager(Environment.CurrentDirectory, "CSV");
            IDataProvider cdp2 = base2.GetData("MSFT", 200);
            debug("raw count:"+cdp2.Count.ToString());
            //debug(cdp2["DATE"][1].ToString());
            //ms.SaveData("MSFT",base2.GetData("MSFT", 200),true);

            string[] s = { "ACF", "ACI", "MSFT" };
            Basket ds = new BasketImpl(s);

            ctChartForm f = new ctChartForm(new MemoryDataManager(ms), ds, "MSFT");
            f.SendDebugEvent += new DebugDelegate(debug);
            
            //ctChartForm f = new ctChartForm();
            f.Show();
             * /
             */
            //test();
            //Security sec = SecurityTracker.getSecurity("IF CN_CFFEX FUT");
            //debug(sec.PriceTick.ToString());
            //mysqlDB sql = new mysqlDB();
            //DataSet ds = sql.getOrderSet();
           // ctsqlOrderView1.DataSource = ds;

            //ctsqlTradeView1.DataSource = sql.getTradeSet();
            
            
        }


        private void button2_Click(object sender, EventArgs e)
        {
            debug("mai thread:"+_srv.isAlive.ToString()+"name thread:"+_srv.isNameServerAlive.ToString()+"| server status:"+_srv.Runing.ToString());
        }

        private void runasyncserver()
        {
            _srv = new AsyncServer(true);
            _srv.SendDebugEvent += new DebugDelegate(debug);
            _srv.Start();
        }

        private void runtlmq()
        { 
            
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(runasyncserver));
            t.Start();
           
            //f.Reset();
            //_bf.Stop();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        Socket frontend ,backend;
        private  void ServerTask()
        {
            var workers = new List<Thread>(5);
            using (var context = new Context(1))
            {
                using (Socket frontend = context.Socket(SocketType.ROUTER), backend = context.Socket(SocketType.DEALER))
                {
                    frontend.Bind("tcp://*:5570");
                    backend.Bind("inproc://backend");

                    for (int workerNumber = 0; workerNumber < 5; workerNumber++)
                    {
                        workers.Add(new Thread(ServerWorker));
                        workers[workerNumber].Start(context);
                    }

                    //  Switch messages between frontend and backend
                    frontend.PollInHandler += (socket, revents) =>
                    {
                        debug("frontend->backent");
                        var zmsg = new ZMessage(socket);
                        zmsg.Send(backend);
                    };

                    backend.PollInHandler += (socket, revents) =>
                    {
                        debug("backend->frontend");
                        var zmsg = new ZMessage(socket);
                        zmsg.Send(frontend);
                    };

                    var sockets = new List<Socket> { frontend, backend };

                    while (true)
                    {
                        Context.Poller(sockets);
                    }
                }
            }
        }

        //  Accept a request and reply with the same text a random number of
        //  times, with random delays between replies.
        private  void ServerWorker(object context)
        {
            var randomizer = new Random(DateTime.Now.Millisecond);
            using (Socket worker = ((Context)context).Socket(SocketType.DEALER))
            {
                worker.Connect("inproc://backend");

                while (true)
                {
                    debug("worker got task from backend");
                    //服务端从这里得到客户端过来的消息
                    //  The DEALER socket gives us the address envelope and message
                    var zmsg = new ZMessage(worker);
                    //Server收到信息-->TradeLink Message(将消息转换成Tradelink消息)
                    TradeLink.Common.Message msg = TradeLink.Common.Message.gotmessage(zmsg.Body);
                    debug(msg.Type.ToString() + "|" + msg.Content.ToString());
                    debug("msg:" + zmsg.BodyToString());
                    debug(zmsg.Address.ToString());
                    Thread.Sleep(5000);
                    //  Send 0..4 replies back
                    //int replies = randomizer.Next(5);
                    //for (int reply = 0; reply < replies; reply++)
                    {
                        //Thread.Sleep(randomizer.Next(1, 1000));
                        if (textBox1.Text != string.Empty)
                            zmsg.AddressFromString(textBox1.Text);
                        zmsg.Send(worker);
                        debug("信息来自于:"+zmsg.AddressToString());

                    }
                }
            }
        }
        //服务端返回信息
        private void sendbackmessage_Click(object sender, EventArgs e)
        {

        }

        //Context context = new Context(1);
        //Socket replyer;
        private void startServer()
        {
                debug("Start server....");

                using (var context = new Context(1))
                {
                    using (Socket replyer = context.Socket(SocketType.REP))
                    {
                        replyer.Bind("tcp://*:5555");

                        const string replyMessage = "World";

                        //这里实现一个收发循环，如果没有消息进来则repley.recv就会停止在那里
                        //同步的信息发送1对1
                        while (true)
                        {
                            debug("listening");
                            string message = replyer.Recv(Encoding.Unicode);
                            debug("Received request:");
                            //Console.WriteLine("Received request: {0}", message);

                            // Simulate work, by sleeping
                            //Thread.Sleep(1000);

                            // Send reply back to client
                            replyer.Send(replyMessage, Encoding.Unicode);
                        }
                    }
                }
                
            
        }
        

        const string replyMessage = "World";
        
        public void startServer2()
        {
            /*
                ZmqReplySocket reply_socket = new ZmqReplySocket();

                reply_socket.Bind("tcp://127.0.0.1:12929");
            
                reply_socket.OnSend += (msg, mp) =>
                {

                    debug("Sent {0} byte reply." + msg.Length.ToString());
                };

                reply_socket.OnReceive += () =>
                {

                    Byte[] msg;
                    reply_socket.Receive(out msg, true);
                    var message = Encoding.ASCII.GetString(msg);

                    if (message.StartsWith("syn"))
                    {

                        debug("Got {0} byte request." + msg.Length.ToString());
                        reply_socket.Send(Encoding.ASCII.GetBytes("ack"));
                    }
                };
             

                while (!_interrpet)
                {
                    debug("waitting new data");
                    Thread.Sleep(500);
                }
              * */

            
            
            
        }
        bool _interrpet = false;
        AsyncServer _srv;
        TLServer_MQ TLmq;
        TLClient_MQ tlmq;
        private void runserver()
        {
            //tlmq = new TLClient_MQ("127.0.0.1", 5570, debug, true);

            TLmq = new TLServer_MQ("127.0.0.1",4500, 25, 100000,debug,false);
            TLmq.newProviderName = Providers.CTP;
             //_srv = new AsyncServer();
            //_srv.SendDebugEvent +=new DebugDelegate(debug);
            //_srv.Start();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            
            Thread t = new Thread(new ThreadStart(runserver));
            t.Start();
            
            
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //_interrpet = true;
            Thread t = new Thread(new ThreadStart(_srv.Stop));
            t.Start();
            //_srv.Stop();
            

            
        }
        
        

        private void button7_Click(object sender, EventArgs e)
        {
            //f.newdemoTick();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //f.Subscribe(quoteView1);
            MessageBox.Show(_srv.isAlive.ToString());
        }

        private void button9_Click(object sender, EventArgs e)
        {
            TradingLib.Data.database.mysqlDB db = new mysqlDB();
            bool s= db.validAccount(textBox1.Text, textBox2.Text);
            debug(s.ToString());

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //quoteView1.Visible = !quoteView1.Visible;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            debugControl1.Clear();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            //_srv.demoSend(textBox1.Text);
            _srv.demoSend1(textBox1.Text);
        }

        private Socket _pub;
        private void initpub()
        {

          
               using (var context = new Context(1))
            {
                using (Socket publisher = context.Socket(SocketType.PUB), syncService = context.Socket(SocketType.REP))
                {
                    debug("start server");
                    publisher.Bind("tcp://*:5561");
                    _pub = publisher;
                    //syncService.Bind("tcp://*:5562");

                    //  Get synchronization from subscribers
                    /*
                    const int subscribersToWaitFor = 10;
                    for (int count = 0; count < subscribersToWaitFor; count++)
                    {
                        debug("it is here");
                        syncService.Recv();
                        syncService.Send("", Encoding.Unicode);
                    }*/

                    while (true)
                    {
                        Thread.Sleep(1000);
                    }
                    //  Now broadcast exactly 1M updates followed by END
                    /*
                    const int updatesToSend = 1000000;
                    for (int updateId = 0; updateId < updatesToSend; updateId++)
                    {
                        publisher.Send("Rhubard", Encoding.Unicode);
                        Thread.Sleep(1000);
                    }

                    publisher.Send("END", Encoding.Unicode);*/
                }
            }
        
        }
    
        private void button12_Click(object sender, EventArgs e)
        {

            Thread t = new Thread(new ThreadStart(initpub));
               t.Start();
            
        }

        private void button13_Click(object sender, EventArgs e)
        {
            _pub.Send("xyz",Encoding.Unicode);
        }

       

    }
}
