using System;
using System.Collections.Generic;
using System.Text;
using Beetle;
namespace Ant.Component.Protocols
{
    /// <summary>
    /// 上传文件请求
    /// 4 byte int32 消息总长度
    /// 4 byte int32 消息名称长度
    ///    byte[] 消息名称 "Post" utf8编码
    /// 4 byte int32 消息ID
    /// 4 byte int32 文件名长度
    ///    byte[] string 文件件名utf8编码
    /// 8 byte int64 文件大小
    /// 4 byte int32 传送文件块大小
    /// 4 byte int32 文件块数量
    /// </summary>
    public class Post:MessageBase
    {

        public string FileName;
        public long Size;
        public int PackageSize;
        public int Packages;
        public Post()
        {
           
        }
        public override void Load(BufferReader reader)
        {
            base.Load(reader);   
            FileName = reader.ReadString();
            Size = reader.ReadInt64();
            PackageSize = reader.ReadInt32();
            Packages = reader.ReadInt32();
        }
        public override void Save(BufferWriter writer)
        {
            base.Save(writer);
            writer.Write(FileName);
            writer.Write(Size);
            writer.Write(PackageSize);
            writer.Write(Packages);
        }
    }
    /// <summary>
    /// 上传文件应答
    /// 4 byte int32 消息总长度
    /// 4 byte int32 消息名称长度
    ///    byte[] 消息名称 "PostResponse" utf8编码
    /// 4 byte int32 消息ID
    /// 4 byte int32 应答状态信息长度
    ///    byte[] string 状态值的utf8编码，些值为空的时候表示正常处理，否则是相关错误信息
    /// </summary>
    public class PostResponse : MessageBase
    {
        public string Status;

        public override void Load(BufferReader reader)
        {
            base.Load(reader);
            Status = reader.ReadString();
        }
        public override void Save(BufferWriter writer)
        {
            base.Save(writer);
            writer.Write(Status);
        }

    }
  
}
