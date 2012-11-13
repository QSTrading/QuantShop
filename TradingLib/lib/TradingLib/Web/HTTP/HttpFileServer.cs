using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.IO;
using TradeLink.API;


namespace TradingLib.Web.HTTP
{
    public class HttpFileServer
    {
        public event DebugDelegate SendDebugEvent;
        static string filelist = "";


        private HttpListener listener;
        private string _serverip;
        public HttpFileServer(string server)
        {
            _serverip = server;

        }

        private void debug(string msg)
        {
            if (SendDebugEvent != null)
                SendDebugEvent(msg);
        }
        public void Start()
        {
            try
            {
                filelist = GetFilelist();
                listener = new HttpListener();
                //从配置文件获取监听前缀  
                string prePath = "." + Path.DirectorySeparatorChar + "config\\httpsrv.config";
                debug(prePath);
                if (!File.Exists(prePath))
                {
                    debug("config  not found.");
                    return;
                }
                string[] pre = File.ReadAllLines(prePath);
                foreach (var p in pre)
                    listener.Prefixes.Add("http://"+_serverip+"/"+p);
                listener.Start();
                debug("Service Ready! Listening for clients...@" + "http://" + _serverip + "/");
                new Thread(Request).Start(listener);
            }
            catch (Exception ec)
            {
                debug(ec.ToString());
            }
        
        }

        public void Stop()
        {
            listener.Stop();
        }

        private  void Request(object lis)
        {
            try
            {
                HttpListener listener = lis as HttpListener;
                if (listener.IsListening)
                {
                    while (true)
                    {
                        new Thread(Processing).Start(listener.GetContext());
                    }
                }
            }
            catch (Exception ex)
            {
                debug("处理请求时发生异常：\r\n" + ex.Message);
            }  
        }

        //处理客户端请求  
        private  void Processing(object state)
        {

            HttpListenerContext context = state as HttpListenerContext;
            try
            {
                HttpListenerRequest request = context.Request;

                debug(request.HttpMethod + " " + request.Url + "  From:" + context.Request.RemoteEndPoint);
                //获取客户端请求文件的路径  
                string uri = System.Web.HttpUtility.UrlDecode(request.RawUrl);
                uri = uri.Substring(uri.IndexOf("/") + 1);
                uri = uri.Replace("/", Path.DirectorySeparatorChar.ToString());
                uri = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, uri);
                debug("Request File:" + uri.ToString());
                if (!File.Exists(uri))
                {
                    //若请求的文件不存在，则返回可用的文件列表  
                    HttpListenerResponse respon = context.Response;
                    Encoding encoding = Encoding.GetEncoding("GBK");
                    byte[] buff = encoding.GetBytes(filelist);
                    respon.ContentLength64 = buff.Length;
                    respon.ContentEncoding = encoding;
                    respon.OutputStream.Write(buff, 0, buff.Length);
                    respon.OutputStream.Close();
                    return;
                }
                //开始传输客户端请求的文件  
                FileStream stream = new FileStream(uri, FileMode.Open, FileAccess.Read);
                try
                {
                    byte[] buff2 = new byte[1024];
                    int count = 0;
                    while ((count = stream.Read(buff2, 0, 1024)) != 0)
                    {
                        context.Response.OutputStream.Write(buff2, 0, count);
                    }
                    context.Response.OutputStream.Close();
                    stream.Close();
                }
                catch (Exception e2)
                {
                    debug(string.Format("停止传输文件：'{0}'", uri));
                    stream.Close();
                    context.Response.Close();
                }
            }
            catch (Exception ex)
            {
                debug("异常：" + ex.Message);
                //  context.Response.OutputStream.Close();  
                context.Response.Close();
            }
        }


        //获取程序启动路径下所有文件的列表。（包括所有子文件夹下的文件）  
        private  string GetFilelist()
        {
            try
            {
                debug("Available Files:");
                string[] files = Directory.GetFiles(Environment.CurrentDirectory, "*.*", SearchOption.AllDirectories);
                StringBuilder sb = new StringBuilder();
                sb.Append("<html><body><h3>Files List:<h3><ul>");
                foreach (var f in files)
                {
                    Console.WriteLine(f);
                    string fName = Path.GetFileName(f);
                    sb.Append("<li><a href=\"" + System.Web.HttpUtility.UrlEncode(f.Substring(AppDomain.CurrentDomain.BaseDirectory.Length)) + "\" >" + fName + "</a></li>");
                }
                sb.Append("</ul></body></html>");
                return sb.ToString();
            }
            catch (Exception ex)
            {
                debug("读取文件列表发生异常：" + ex.Message.ToString());
                return "";
            }
        }  


    }
}
