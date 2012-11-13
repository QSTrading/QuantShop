using System;
using System.Collections.Generic;
using System.Text;

namespace Ant.Component.Protocols
{
    public class HeadSizePackage:Beetle.HeadSizeOfPackage
    {
        public HeadSizePackage()
        {

        }
        public HeadSizePackage(Beetle.TcpChannel channel)
            : base(channel)
        {
        }
        protected override void WriteMessageType(Beetle.IMessage msg, Beetle.BufferWriter writer)
        {
            writer.Write(msg.GetType().Name);
        }
        protected override Beetle.IMessage ReadMessageByType(Beetle.BufferReader reader, out object typeTag)
        {
            typeTag = reader.ReadString();
            switch ((string)typeTag)
            {
                case "Error":
                    return new Error();
                case "Get":
                    return new Get();
                case "GetResponse":
                    return new GetResponse();
                case "GetPackage":
                    return new GetPackage();
                case "GetPackageResponse":
                    return new GetPackageResponse();
                case "Post":
                    return new Post();
                case "PostResponse":
                    return new PostResponse();
                case "PostPackage":
                    return new PostPackage();
                case "PostPackageResponse":
                    return new PostPackageResponse();
                case "Sign":
                    return new Sign();
                case "SingResponse":
                    return new SingResponse();
                case "GetUpdateInfo":
                    return new GetUpdateInfo();
                case "GetUpdateInfoResponse":
                    return new GetUpdateInfoResponse();
                default:
                    return null;
            }
        }
    }
}
