using System;
using System.Collections.Generic;
using System.Text;
using Beetle;
namespace Ant.Component.Protocols
{
    public class MessageBase : IMessage
    {
        public MessageBase()
        {
            ID = GetSeed();

        }
        public int ID;
        public virtual void Save(BufferWriter writer)
        {
            writer.Write(ID);
          
        }
        public virtual void Load(BufferReader reader)
        {
            ID = reader.ReadInt32();
        }
        static int mSeed = 1;
        static int GetSeed()
        {
            lock (typeof(MessageBase))
            {
                mSeed++;
                if (mSeed >= int.MaxValue)
                    mSeed = 0;
                return mSeed;
            }
        }
   
    }
}
