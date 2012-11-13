using System;
using System.Collections.Generic;
using System.Text;

namespace Ant.Component.Protocols
{
    /// <summary>
    /// 签名验证请求
    /// 4 byte int32 消息总长度
    /// 4 byte int32 消息名称长度
    ///   byte[] 消息名称 "Sign" utf8编码
    /// 4 byte int32 消息ID
    /// 4 byte int32 签名名称长度
    ///   byte[] string 名称 utf8编码
    /// 4 byte int32 签名数据长度
    ///   byte[] string 签名数据 utf8编码，获取方式是 Data=ToBase64String(Sign(ASCII.GetBytes(Name)));Sing(byte[] data){rsaProvider.SignData(data, "MD5")}
    /// </summary>
    public class Sign:MessageBase
    {
        public string Name;
        public string Data;
        public override void Load(Beetle.BufferReader reader)
        {
            base.Load(reader);
            Name = reader.ReadString();
            Data = reader.ReadString();
        }
        public override void Save(Beetle.BufferWriter writer)
        {
            base.Save(writer);
            writer.Write(Name);
            writer.Write(Data);
        }
    }
    /// <summary>
    /// 签名验证应答
    /// 4 byte int32 消息总长度
    /// 4 byte int32 消息名称长度
    ///   byte[] 消息名称 "SingResponse" utf8编码
    /// 4 byte int32 消息ID
    /// 4 byte int32 状态信息长度
    ///   byte[] string 状态值的utf8编码，些值为空的时候表示正常处理，否则是相关错误信息
    /// </summary>
    public class SingResponse : MessageBase
    {
        public string Status;
        
        public override void Load(Beetle.BufferReader reader)
        {
            base.Load(reader);
            Status = reader.ReadString();
        }
        public override void Save(Beetle.BufferWriter writer)
        {
            base.Save(writer);
            writer.Write(Status);
        }
    }
}
