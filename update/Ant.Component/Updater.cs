using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace Ant.Component
{
    class TcpUtilInit
    {
        static TcpUtilInit()
        {
            
            Beetle.TcpUtils.Setup("beetle");
            
        }
        public static void Init()
        {
        }
    }
    public class Updater
    {

        public string Message
        {
            get;
            set;
        }
        private Beetle.TcpSyncChannel<Protocols.HeadSizePackage> mChannel;
        

        bool mDetectResult = false;

        public bool Detect()
        {
            //加载配置文件
            Utils.LoadINI();
            //MessageBox.Show(Utils.IPAddress + "|" + Utils.Port);
            return Detect(Utils.IPAddress, Convert.ToInt16(Utils.Port));
        }
        public  bool Detect(string host, int port)
        {
            try
            {

                //MessageBox.Show("run here 0");
                //TcpUtilInit.Init();
                Beetle.TcpUtils.Setup(2);
                //MessageBox.Show("run here 1");
                mChannel = new Beetle.TcpSyncChannel<Protocols.HeadSizePackage>();
                //MessageBox.Show("run here 2");
                mChannel.Connect(host, port);

                Protocols.GetUpdateInfo getinfo = new Protocols.GetUpdateInfo();
                Protocols.GetUpdateInfoResponse e = (Protocols.GetUpdateInfoResponse)mChannel.Send(getinfo);
                //MessageBox.Show("run here a");
                
                if (string.IsNullOrEmpty(e.Status))
                {

                    UpdateInfo local = GetLocalInfo();
                    UpdateInfo remot = new UpdateInfo();
                    remot.LoadXml(e.Info);
                    mDetectResult = local.Comparable(remot).Count > 0;
                }
                else
                {
                    Message = e.Status;
                }
                //MessageBox.Show("run here b");
            }
            catch (Exception e_)
            {
                Message = e_.Message;
                //MessageBox.Show(e_.ToString());
            }
            finally
            {
                if (mChannel != null)
                    mChannel.Dispose();
            }
            return mDetectResult;
        }

        public void Update(string appname, bool autoclose)
        {
            //MessageBox.Show("更新进行中:" + AntUpdateSection.Instance.Host.ToString() + "Port:" + AntUpdateSection.Instance.Port.ToString());
            Updating(Utils.IPAddress, Convert.ToInt16(Utils.Port), appname, autoclose);
        }
       
        private void Updating(string address,int port,string appname,bool autoclose)
        {
            //MessageBox.Show("更新进行中");
            System.Diagnostics.Process.Start("Ant.Update.exe", string.Format("{0} {1} {2} {3}",address,port,appname,autoclose));
        }
       
        private UpdateInfo GetLocalInfo()
        {
            UpdateInfo info = new UpdateInfo();
            string filename = System.IO.Path.GetDirectoryName( typeof(Updater).Assembly.Location) + "\\update_info.xml";
            info.Load(filename);
            return info;
        }
    }
}
