using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ant.Component.Protocols;
namespace Ant.Update
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }
        private Beetle.TcpSyncChannel<Ant.Component.Protocols.HeadSizePackage> mChannel;
       
      
        private Queue<Component.FileItem> mDownloads = null;
        private string tmpFolder = "";
        private int mUpateCount = 0;

        public string[] Arges
        {
            get;
            set;
        }
        private Component.UpdateInfo mLocalInfo = new Component.UpdateInfo();
        private Component.UpdateInfo mUpdateInfo = new Component.UpdateInfo();
        private IList<Component.FileItem> mCopyFiles = new List<Component.FileItem>(100);
        
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                
                Beetle.TcpUtils.Setup("beetle");

                tmpFolder = "tmp" + DateTime.Now.ToString("yyyyMMdd"); ;
                Utils.FileProgress.Draw("", 0, 100);
                Utils.TotalProgress.Draw("下载总进度 ", 0, 10);
                imgFile.Image = Utils.FileProgress.Image;
                imtTotal.Image = Utils.TotalProgress.Image;
                Utils.LoadINI();
                mLocalInfo.Load(Utils.GetFileFullName(Utils.UPDATE_FILE));
                //利用线程池进行工作项目的安排
                System.Threading.ThreadPool.QueueUserWorkItem(Connect);
            }
            catch (Exception e_)
            {
                MessageBox.Show(this, e_.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
                
           
        }
        private void ChangeStatus(Color color, string text)
        {
            Invoke(new Smark.Core.AsyncDelegate<Color,string>((c,s) =>
            {
                txtStatus.ForeColor = c;
                txtStatus.Text = s;
            }),color, text);
        }
        protected void Connect(object state)
        {
            try
            {
                if (Arges.Length > 0)
                    Utils.IPAddress = Arges[0];
                if (Arges.Length > 1)
                    Utils.Port = Arges[1];
                if (Arges.Length > 2)
                    Utils.AppName = Arges[2];
                if (Arges.Length > 3)
                    Utils.AutoClose = Arges[3];
                if (string.IsNullOrEmpty(Utils.IPAddress))
                {
                    throw new Exception("不存在服务器地址信息!");
                }
                if (string.IsNullOrEmpty(Utils.Port))
                {
                    throw new Exception("不存在服务器地址信息!");
                }
                mChannel = new Beetle.TcpSyncChannel<HeadSizePackage>();
                mChannel.Connect(Utils.IPAddress, int.Parse(Utils.Port));
                Component.Protocols.GetUpdateInfo update = new Component.Protocols.GetUpdateInfo();
                GetUpdateInfoResponse getinfo = (GetUpdateInfoResponse)mChannel.Send(update);
                ChangeStatus(Color.Black, "获取文件更新信息...");
                if (string.IsNullOrEmpty(getinfo.Status))
                {
                    mUpdateInfo.LoadXml(getinfo.Info);
                    mDownloads = mLocalInfo.Comparable(mUpdateInfo);
                    if (mDownloads.Count == 0)
                    {
                        ChangeStatus(Color.Black, "当前是最新版本!");
                    }
                    else
                    {
                        ChangeStatus(Color.Black, "下载文件到临时文件夹...");
                        mUpateCount = mDownloads.Count;
                        Utils.TotalProgress.Draw(string.Format("更新文件总进度 {0}/{1}", 0, mUpateCount), 0, mUpateCount);
                        string tmpf = Utils.GetFileFullName(tmpFolder);
                        if (!System.IO.Directory.Exists(tmpf))
                            System.IO.Directory.CreateDirectory(tmpf);
                        while (mDownloads.Count > 0)
                        {
                            Component.FileItem item = mDownloads.Dequeue();
                            Component.Protocols.Get get = new Component.Protocols.Get();
                            get.FileName = item.File;
                            GetResponse getresponse = (GetResponse)mChannel.Send(get);
                            if (string.IsNullOrEmpty(getresponse.Status))
                            {
                                
                                string path = System.IO.Path.GetDirectoryName(item.File);
                                if (!string.IsNullOrEmpty(path))
                                {
                                    string createFolder = Utils.GetFileFullName(tmpFolder + "\\" + path);
                                    if (!System.IO.Directory.Exists(createFolder))
                                        System.IO.Directory.CreateDirectory(createFolder);

                                }
                                Component.FileUtils.CreateFile(Utils.GetFileFullName(tmpFolder + "\\" + item.File + ".update"), getresponse.Size);
                                Component.Protocols.GetPackage getpackage = new Component.Protocols.GetPackage();
                                int page = 0;
                                getpackage.Index = 0;
                                GetPackageResponse gpr = (GetPackageResponse)mChannel.Send(getpackage);
                                page = gpr.Index + 1;
                                Utils.FileProgress.Draw(item.File, page, getresponse.Packages);
                                Component.FileUtils.FileWrite(Utils.GetFileFullName(tmpFolder + "\\" + item.File + ".update"), gpr.Index, getresponse.PackageSize, gpr.Data);
                               
                                while (page < getresponse.Packages)
                                {
                                    if (string.IsNullOrEmpty(gpr.Status))
                                    {
                                    
                                        getpackage = new Component.Protocols.GetPackage();
                                        getpackage.Index = page;
                                        gpr = (GetPackageResponse)mChannel.Send(getpackage);
                                        page = gpr.Index + 1;
                                        Utils.FileProgress.Draw(item.File, page, getresponse.Packages);
                                        Component.FileUtils.FileWrite(Utils.GetFileFullName(tmpFolder + "\\" + item.File + ".update"), gpr.Index, getresponse.PackageSize, gpr.Data);
                                    }
                                    else
                                    {
                                        ChangeStatus(Color.Red, getresponse.Status);
                                        return;
                                    }
                                }
                                mCopyFiles.Add(item);
                            }
                            else
                            {
                                ChangeStatus(Color.Red, getresponse.Status);
                                return;;
                            }
                            Utils.TotalProgress.Draw(string.Format("下载总进度 {0}/{1}", mUpateCount - mDownloads.Count, mUpateCount), mUpateCount - mDownloads.Count, mUpateCount);
                        }
                        CopyTempFiles();
                    }
                }
                else
                {
                    ChangeStatus(Color.Red, getinfo.Status);
                    return;
                }

            }
            catch (Exception e_)
            {
                ChangeStatus(Color.Red,e_.Message);
            }
        }
        private void OnError(object sender, Beetle.ChannelErrorEventArgs e_)
        {
            ChangeStatus(Color.Red, "程序处理错误:" + e_.Exception.Message);
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer", "http://www.ikende.net");
        }

        
        private void CopyTempFiles()
        {
            ChangeStatus(Color.Black, "下载完成，更新本地文件...");
            foreach (Component.FileItem item in mCopyFiles)
            {
                string filename = Utils.GetFileFullName(item.File);
                if (System.IO.File.Exists(filename))
                    System.IO.File.Delete(filename);
                string path = System.IO.Path.GetDirectoryName(item.File);
                if (!string.IsNullOrEmpty(path))
                {
                    string createFolder = Utils.GetFileFullName(path);
                    if (!System.IO.Directory.Exists(createFolder))
                        System.IO.Directory.CreateDirectory(createFolder);

                }
                System.IO.File.Move(Utils.GetFileFullName(tmpFolder + "\\" + item.File + ".update"),
                    filename);
            }
            mLocalInfo.Save(Utils.GetFileFullName(Utils.UPDATE_FILE));
            System.IO.Directory.Delete(Utils.GetFileFullName(tmpFolder), true);
            ChangeStatus(Color.Black, "更新完成!");
            bool autoclose;
            System.Threading.Thread.Sleep(1000);
            if (bool.TryParse(Utils.AutoClose, out autoclose))
            {
                if (autoclose)
                {
                    Invoke(new Action<FrmMain>(f =>
                    {
                        f.Close();
                    }), this);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lock (Utils.FileProgress)
            {
                imgFile.Refresh();
            }
            lock (Utils.TotalProgress)
            {
                imtTotal.Refresh();
            }
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(!string.IsNullOrEmpty(Utils.AppName))
                System.Diagnostics.Process.Start(Utils.AppName);
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer", "http://www.ikende.com/");
        }
    }
}
