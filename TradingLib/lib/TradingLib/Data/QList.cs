using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Data
{
    public class QList<T>:List<T>
    {
        //往后回溯n值
        public T LookBack(int n)
        {
            if (n < 0)
                return Last;
            if (n > (base.Count - 1))
                return First;
            return base[(base.Count - 1) - n];
        }

        public T First
        {
            get {
                if (base.Count > 0)
                {
                    return base[0];
                }
                return default(T);
            }
        }

        public T Last
        {
            get {
                if (base.Count > 0)
                {
                    return base[base.Count - 1];
                }
                return default(T);
            
            }
            
        }

        public int LastIndex
        {
            get {
                return (base.Count - 1);
            }
        }
    }
}
