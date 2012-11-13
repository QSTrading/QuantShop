using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Broker.CTP
{
    public class ObjectAndKey
    {
        /// <summary>
        /// 添加到listview的实例
        /// </summary>
        public object Object { get; set; }
        /// <summary>
        /// 此listviewitem的Key
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 被添加listviewitem的HFListView
        /// </summary>
        //public HFListView LV { get; set; }
        public ObjectAndKey(object obj, string key)
        {
            this.Object = obj;
            this.Key = key;
            //this.LV = lv;
        }
    }
}
