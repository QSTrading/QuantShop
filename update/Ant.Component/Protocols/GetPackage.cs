using System;
using System.Collections.Generic;
using System.Text;
using Beetle;
using ICSharpCode.SharpZipLib.Checksums;
namespace Ant.Component.Protocols
{
    /// <summary>
    ///  获取某个文件块请求，些消息是在PostResponse成功后执行
    /// 4 byte int32 消息总长度
    /// 4 byte int32 消息名称长度
    ///   byte[] 消息名称 "GetPackage" utf8编码
    /// 4 byte int32 消息ID
    /// 4 byte int32 获取文件块索引，从零开始
    /// </summary>
    public class GetPackage:MessageBase
    {
        public int Index;
        public override void Load(BufferReader reader)
        {
            base.Load(reader);
            Index = reader.ReadInt32();
        }
        public override void Save(BufferWriter writer)
        {
            base.Save(writer);
            writer.Write(Index);
        }
    }
    /// <summary>
    ///  获取某个文件块应答，可以根据完成情况是否再进行GetPackage请求
    /// 4 byte int32 消息总长度
    /// 4 byte int32 消息名称长度
    ///   byte[] 消息名称 "GetPackageResponse" utf8编码
    /// 4 byte int32 消息ID
    /// 4 byte int32 获取文件块索引，从零开始
    /// 4 byte int32 文件块内容长度
    ///   byte[] 文件块内容
    /// 8 byte int64 文件块内容的校验码
    /// 4 byte int32 状态信息长度
    ///   byte[] string 状态值的utf8编码，些值为空的时候表示正常处理，否则是相关错误信息
    /// </summary>
    public class GetPackageResponse : MessageBase
    {
        public int Index;
        public byte[] Data;
        public long Checksums;
        public string Status;
        public override void Load(BufferReader reader)
        {
            base.Load(reader);
            Index = reader.ReadInt32();
            Data = reader.ReadByteArray();
            Checksums = reader.ReadInt64();
            Status = reader.ReadString();
        }
        public override void Save(BufferWriter writer)
        {
            base.Save(writer);
            writer.Write(Index);
            writer.Write(Data);
            Adler32 adler = new Adler32();
            adler.Update(Data);
            writer.Write(adler.Value);
            writer.Write(Status);
        }
        public bool Check()
        {
            Adler32 adler = new Adler32();
            adler.Update(Data);
            return adler.Value == Checksums;
        }
    }
}
