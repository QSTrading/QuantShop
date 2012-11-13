using System;
using System.Collections.Generic;
using System.Text;

namespace Ant.Server.Codes
{
    class Actions:Beetle.IMessageHandler
    {
        public Smark.Core.AsyncDelegate<Smark.Core.Log.LogType, string> Output
        {
            get;
            set;
        }
        public void Sing(Beetle.TcpChannel channel, Component.Protocols.Sign e)
        {
            Component.Protocols.SingResponse response = new Component.Protocols.SingResponse();
            if (Utils.Ras.Verify(e.Name, e.Data))
            {
                channel.Status = Beetle.ChannelStatus.Security;
                Output(Smark.Core.Log.LogType.None, string.Format("{0} Sing ok!", channel.EndPoint));
            }
            else
            {

                response.Status = "Sing error!";
                Output(Smark.Core.Log.LogType.Warning, string.Format("{0} Sing error!", channel.EndPoint));
            }
            channel.Send(response);
            if (!string.IsNullOrEmpty(response.Status))
            {
                System.Threading.Thread.Sleep(2000);
                channel.Dispose();
            }
        }
        public const string CURRENT_POSTFILE="_CURRENTPOSTFILE";
        public const string CURRENT_GETFILE = "_CURRENTGETFILE";
        public const string CURRENT_GETFILENAME = "_CURRENTGETFILENAME";
        public void Post(Beetle.TcpChannel channel, Component.Protocols.Post e)
        {
            Component.Protocols.PostResponse response = new Component.Protocols.PostResponse();
            try
            {
                if (channel.Status == Beetle.ChannelStatus.Security)
                {
                    channel[CURRENT_POSTFILE] = e;
                    string dir = System.IO.Path.GetDirectoryName(e.FileName);
                    if (!string.IsNullOrEmpty(dir))
                    {
                        if (!System.IO.Directory.Exists(Utils.GetFileFullName("data\\"+dir)))
                        {
                            System.IO.Directory.CreateDirectory(Utils.GetFileFullName("data\\" + dir));
                        }
                    }
                    e.FileName = Utils.GetFileFullName("data\\" + e.FileName);
                    Component.FileUtils.CreateFile(e.FileName+".up", e.Size);
                    Output(Smark.Core.Log.LogType.None, string.Format("{0} upload {1}", channel.EndPoint, System.IO.Path.GetFileName(e.FileName)));


                }
                else
                {
                    response.Status = "无效签名!";
                    Output(Smark.Core.Log.LogType.Warning, string.Format("{0} upload error {1}", channel.EndPoint, response.Status));
                }
            }
            catch (Exception e_)
            {
                response.Status = e_.Message;
            }
            channel.Send(response);
        }
        public void PostPackage(Beetle.TcpChannel channel, Component.Protocols.PostPackage e)
        {
            Component.Protocols.PostPackageResponse response = new Component.Protocols.PostPackageResponse();
            response.Index = e.Index;
            Component.Protocols.Post info = (Component.Protocols.Post)channel[CURRENT_POSTFILE];
            try
            {
                if (info != null)
                {
                    if (e.Check())
                    {
                        Component.FileUtils.FileWrite(info.FileName+".up", e.Index, info.PackageSize, e.Data);
                        if (e.Index + 1 == info.Packages)
                        {
                            if (System.IO.File.Exists(info.FileName))
                                System.IO.File.Delete(info.FileName);
                            System.IO.File.Move(info.FileName + ".up", info.FileName);
                            Output(Smark.Core.Log.LogType.None, string.Format("{0} upload {1} Completed", channel.EndPoint, System.IO.Path.GetFileName(info.FileName)));
                        }
                    }
                    else
                    {
                        response.Status = "文件校验错误!";
                    }
                }
                else
                {
                    response.Status = "不存在文件信息!";
                }
            }
            catch (Exception e_)
            {
                response.Status = e_.Message;
            }
            if(!string.IsNullOrEmpty(response.Status))
                Output(Smark.Core.Log.LogType.Warning, string.Format("{0} upload error {1}", channel.EndPoint, response.Status));
            channel.Send(response);
        }
        public void GetUpdateInfo(Beetle.TcpChannel channel, Component.Protocols.GetUpdateInfo e)
        {
            Component.Protocols.GetUpdateInfoResponse response = new Component.Protocols.GetUpdateInfoResponse();
            string file = Utils.GetFileFullName("data\\"+Utils.UPDATE_FILE);
            if(System.IO.File.Exists(file))
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(file, Encoding.UTF8))
                {
                    response.Info = reader.ReadToEnd();
                }
                Output(Smark.Core.Log.LogType.None, string.Format("{0} get update info!", channel.EndPoint));
            }
            else{
               response.Status="不存在更新信息!";
               Output(Smark.Core.Log.LogType.Warning, string.Format("{0} get update info error {1}!", channel.EndPoint, response.Status));
            }
            channel.Send(response);
        }
        public void Get(Beetle.TcpChannel channel, Component.Protocols.Get e)
        {
            Component.Protocols.GetResponse response = new Component.Protocols.GetResponse();
            string filename = Utils.GetFileFullName("data\\" + e.FileName);
            if (System.IO.File.Exists(filename))
            {
                System.IO.FileInfo info = new System.IO.FileInfo(filename);
                response.Size = info.Length;
                response.Packages = Component.FileUtils.GetFilePackages(response.Size);
                response.PackageSize = Component.FileUtils.PackageSize;
                channel[CURRENT_GETFILE] = response;
                channel[CURRENT_GETFILENAME] = e.FileName;
                Output(Smark.Core.Log.LogType.None, string.Format("{0} get file {1}!", channel.EndPoint, e.FileName));
            }
            else
            {
                response.Status = "文件不存在!";
                Output(Smark.Core.Log.LogType.Warning, string.Format("{0} get file {1} not found!", channel.EndPoint, e.FileName));
            }
            channel.Send(response);

        }
        public void GetPackage(Beetle.TcpChannel channel, Component.Protocols.GetPackage e)
        {
            Component.Protocols.GetPackageResponse response = new Component.Protocols.GetPackageResponse();
            response.Index = e.Index;
            Component.Protocols.GetResponse getfile = (Component.Protocols.GetResponse)channel[CURRENT_GETFILE];
            string name = (string)channel[CURRENT_GETFILENAME];
            if (getfile != null)
            {
                try
                {
                    string filename = Utils.GetFileFullName("data\\" + name);
                    response.Data = Component.FileUtils.FileRead(filename, e.Index, getfile.PackageSize);
                    
                }
                catch (Exception e_)
                {
                    response.Status = e_.Message;

                }
            }
            else
            {
                response.Status = "文件不存在!";
            }
            if(!string.IsNullOrEmpty(response.Status))
                Output(Smark.Core.Log.LogType.Warning, string.Format("{0} get file package error {1}!", channel.EndPoint, response.Status));
            channel.Send(response);
        }
        public const string LASTTIME_TAG = "_LASTTIME";
        public void ProcessMessage(Beetle.TcpChannel channel, Beetle.MessageHandlerArgs message)
        {
            channel[LASTTIME_TAG] = DateTime.Now;
        }
        public DateTime LastTime(Beetle.TcpChannel channel)
        {
            return (DateTime)channel[LASTTIME_TAG];
        }
    }
}
