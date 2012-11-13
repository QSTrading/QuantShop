using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using TradeLink.API;
using System.Threading;


namespace TradingLib.Core
{
    //客户端动态的从服务器更新配置 策略文件
    public class UpdateHelper
    {
        public event DebugDelegate SendDebugEvent;
        string URLpre = "http://127.0.0.1/";


        string filelist = "config/update.txt";
        //string filename = "config/Security.xml";
        private WebClient wClient;

        private void debug(string msg)
        {
            if (SendDebugEvent != null)
                SendDebugEvent(msg);
        }
        public UpdateHelper(string ip)
        {
            if(ip != string.Empty)
                URLpre = "http://" + ip.ToString() + "/";
            wClient = new WebClient();
        }

        public void DownloadFile()
        {
            wClient.DownloadFile(URLpre + filelist, filelist);

            //读取得到的更新列表文件 形成需要更新的文件列表 
            //StreamReader txtStreamReader = new StreamReader(filelist);
            
            string[] strLine = File.ReadAllLines(filelist);
            foreach(string s in strLine)
            {
                //Thread.Sleep(200);
                DownloadFile(s);
            
            }
            //debug("")
            //txtStreamReader.ReadLine()

        }
        public void DownloadFile(string filename)
        {
            debug("更新:" + filename);
            wClient.DownloadFile(URLpre+filename,filename);
        }

    }
}
