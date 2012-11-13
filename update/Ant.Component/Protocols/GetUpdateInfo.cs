using System;
using System.Collections.Generic;
using System.Text;

namespace Ant.Component.Protocols
{
    /// <summary>
    /// 获取更新文件信息请求
    /// 4 byte int32 消息总长度
    /// 4 byte int32 消息名称长度
    ///   byte[] 消息名称 "GetUpdateInfo" utf8编码
    /// 4 byte int32 消息ID
    /// </summary>
    public class GetUpdateInfo:MessageBase
    {
    }
    /// <summary>
    /// 获取更新文件信息应答
    /// 4 byte int32 消息总长度
    /// 4 byte int32 消息名称长度
    ///   byte[] 消息名称 "GetUpdateInfoResponse" utf8编码
    /// 4 byte int32 消息ID
    /// 4 byte int32 更新描述长度
    ///   byte[] string 更新信息utf8编码
    /// 4 byte int32 状态信息长度
    ///   byte[] string 状态值的utf8编码，些值为空的时候表示正常处理，否则是相关错误信息
    /// </summary>
    public class GetUpdateInfoResponse:MessageBase
    {
        public string Info;

        public string Status;
       
        public override void Load(Beetle.BufferReader reader)
        {
            base.Load(reader);
            Info = reader.ReadString();
            Status = reader.ReadString();
        }
        public override void Save(Beetle.BufferWriter writer)
        {
            base.Save(writer);
            writer.Write(Info);
            writer.Write(Status);
        } 
    }
}
