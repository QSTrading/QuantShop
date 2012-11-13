using System;
using System.Collections.Generic;
using System.Text;

namespace Ant.Component.Protocols
{
    public class Error:MessageBase
    {
        public string Message
        {
            get;
            set;
        }
        public override void Load(Beetle.BufferReader reader)
        {
            base.Load(reader);
            Message = reader.ReadString();
        }
        public override void Save(Beetle.BufferWriter writer)
        {
            base.Save(writer);
            writer.Write(Message);
        }
    }
}
