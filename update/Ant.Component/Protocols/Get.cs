using System;
using System.Collections.Generic;
using System.Text;
using Beetle;
namespace Ant.Component.Protocols
{
    /// <summary>
    /// 获取指定文件名信息请求
    /// 4 byte int32 消息总长度
    /// 4 byte int32 消息名称长度
    ///   byte[] 消息名称 "Get" utf8编码
    /// 4 byte int32 消息ID
    /// 4 byte int32 文件名长度
    ///   byte[] string 文件名 utf8编码
    /// </summary>
    public class Get:MessageBase
    {
        public Get()
        {
           
        }

        public string FileName;
        public override void Load(BufferReader reader)
        {
            base.Load(reader);
            FileName = reader.ReadString();
            
        }
        public override void Save(BufferWriter writer)
        {
            base.Save(writer);
            writer.Write(FileName);
        }
        
    }
    /// <summary>
    ///  获取指定文件名信息应答
    /// 4 byte int32 消息总长度
    /// 4 byte int32 消息名称长度
    ///   byte[] 消息名称 "GetResponse" utf8编码
    /// 4 byte int32 消息ID
    /// 8 byte int64 文件的总大小
    /// 4 byte int32 文件块大小
    /// 4 byte int32 文件块数量
    /// 4 byte int32 状态值长度
    ///   byte[] string 状态值的utf8编码，些值为空的时候表示正常处理，否则是相关错误信息
    /// </summary>
    public class GetResponse : MessageBase
    {
        public long Size;
        public int PackageSize;
        public int Packages;
        public string Status;
        public override void Load(BufferReader reader)
        {
            base.Load(reader);
            Size = reader.ReadInt64();
            PackageSize = reader.ReadInt32();
            Packages = reader.ReadInt32();
            Status = reader.ReadString();
            
        }
        public override void Save(BufferWriter writer)
        {
            base.Save(writer);
            writer.Write(Size);
            writer.Write(PackageSize);
            writer.Write(Packages);
            writer.Write(Status);
        }
    }
}
