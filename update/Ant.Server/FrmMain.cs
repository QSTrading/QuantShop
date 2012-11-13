using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ant.Server
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_MinimumSizeChanged(object sender, EventArgs e)
        {

        }
        Queue<Smark.Core.Log.Item> mLogs = new Queue<Smark.Core.Log.Item>(1024);
        private void Log(Smark.Core.Log.LogType type, string text)
        {
            lock (mLogs)
            {
                Smark.Core.Log.Item item = new Smark.Core.Log.Item();
                item.LogType = type;
                item.Message = text;
               
                mLogs.Enqueue(item);
            }
        }
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Show();
        }

        private void 退出xToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer", "http://www.ikende.com/");
        }

        private void 显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }

        private void cmdAbout_Click(object sender, EventArgs e)
        {
            关于ToolStripMenuItem_Click(null, null);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            关于ToolStripMenuItem_Click(null, null);
        }

        private void cmdExportCA_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "是否创建新的证书?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                Utils.CreateKeyFiles(new Smark.Core.RasCrypto());
                Utils.LoadPublicKey();
            }
        }
        private Codes.Actions mAction = new Codes.Actions();
        private Beetle.TcpServer mServer;
        private void FrmMain_Load(object sender, EventArgs e)
        {
            Log(Smark.Core.Log.LogType.Track, Beetle.LICENSE.GetLICENSE().ToString());

            Beetle.TcpUtils.Setup("beetle");
            Utils.LoadINI();
            Utils.LoadPublicKey();
            Utils.CreateFolder();
            mAction.Output = Log;
            //txtLog.Text = "xyz";
            //txtPort.Text = Utils.Port;
            if (string.IsNullOrEmpty(txtPort.Text))
                txtPort.Text = "9560";
            Beetle.ChannelController.RegisterHandler(mAction);

        }

        private void cmdStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Utils.CheckPublicKey())
                    throw new Exception("证书不存在不能启动服务，请创建证书！");
                mServer = new Beetle.TcpServer();
                mServer.ChannelConnected += OnConnected;
                mServer.ChannelDisposed += OnDisposed;
                mServer.Open(int.Parse(txtPort.Text));
                cmdStart.Enabled = false;
                cmdStop.Enabled = true;
                Utils.Port = txtPort.Text;

            }
            catch (Exception e_)
            {
                MessageBox.Show(this, e_.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void OnConnected(object sender, Beetle.ChannelEventArgs e)
        {
            Log(Smark.Core.Log.LogType.None, string.Format("{0} Connected!", e.Channel.EndPoint));
            e.Channel.SetPackage<Component.Protocols.HeadSizePackage>(Beetle.TcpUtils.DefaultController);
            Beetle.TcpServer.SetKeepAliveValues(e.Channel.Socket, 30000, 10000);
            e.Channel.ChannelError += OnError;
            e.Channel[Codes.Actions.LASTTIME_TAG] = DateTime.Now;
            e.Channel.BeginReceive();
        }
        private void OnDisposed(object sender, Beetle.ChannelEventArgs e)
        {
            Log(Smark.Core.Log.LogType.None,string.Format("{0} Disposed!", e.Channel.EndPoint));
        }
        private void OnError(object sender, Beetle.ChannelErrorEventArgs e)
        {
            Log(Smark.Core.Log.LogType.Error,string.Format("{0} Error:{1}", e.Channel.EndPoint, e.Exception.Message));
            if(e.Exception.InnerException !=null)
                Log(Smark.Core.Log.LogType.Error,string.Format("\t\t{0} Inner Error:{1}", e.Channel.EndPoint, e.Exception.InnerException.Message));
        }

        private void cmdStop_Click(object sender, EventArgs e)
        {
            if (mServer != null)
                mServer.Dispose();
            cmdStart.Enabled = true;
            cmdStop.Enabled = false;
        }
        private void CheckChannel(object state)
        {
            try
            {
                if(mServer !=null)
                foreach (Beetle.TcpChannel channel in mServer.GetOnlines())
                {
                    DateTime dt = mAction.LastTime(channel);
                    TimeSpan ts = DateTime.Now - dt;
                    if (ts.TotalSeconds > 120)
                        channel.Dispose();
                }
            }
            catch
            {
            }
        }
        long mCount;
        private void timer1_Tick(object sender, EventArgs e)
        {
            lock (mLogs)
            {
                mCount++;
                if(mServer !=null)
                    txtConnections.Text = mServer.Clients.Count.ToString();
                txtReceive.Text = Beetle.TcpUtils.ReceiveBytes.ToString();
                txtSend.Text = Beetle.TcpUtils.SendBytes.ToString();
                if (mCount % 120 == 0)
                    System.Threading.ThreadPool.QueueUserWorkItem(CheckChannel);
                if (txtLog.TextLength > 100000)
                    txtLog.Text = "";
                while (mLogs.Count > 0)
                {
                    txtLog.SelectionStart = txtLog.Text.Length;
                    txtLog.ScrollToCaret();
                    Smark.Core.Log.Item item = mLogs.Dequeue();
                    switch (item.LogType)
                    {
                        case Smark.Core.Log.LogType.None:
                            txtLog.SelectionColor = Color.LawnGreen;
                            break;
                        case Smark.Core.Log.LogType.Warning:
                            txtLog.SelectionColor = Color.Yellow;
                            break;
                        case Smark.Core.Log.LogType.Error:
                            txtLog.SelectionColor = Color.Red;
                            break;
                    }
                    txtLog.SelectedText = string.Format("{0}\t{1}",item.Date,item.Message) + Environment.NewLine;
                }
            }
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            关于ToolStripMenuItem_Click(null, null);
        }
    }
}
