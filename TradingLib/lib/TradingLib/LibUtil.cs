using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using TradingLib.Data;
using TradeLink.API;
using TradingLib.Data;

namespace TradingLib
{
    public class LibUtil
    {

        public static List<Security> LoadFutFromXML()
        {

            XmlDocument xmlDoc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;//忽略文档里面的注释
            XmlReader reader = XmlReader.Create(@"config\future.xml", settings);
            xmlDoc.Load(reader);
            
            XmlNode xn = xmlDoc.SelectSingleNode("Ats");

            // 得到根节点的所有子节点
            XmlNodeList exchlist = xn.ChildNodes;//得到所有交易所节点
            List<Security> futlist = new List<Security>();
            foreach (XmlNode ex in exchlist)//遍历所有交易所节点
            {
                XmlElement exch = (XmlElement)ex;//将单个交易所节点转换成xmlelement
                //debug(xe.Attributes.ToString());
                //debug(exch.GetAttribute("name").ToString());//得到节点的属性
                XmlNodeList exchValues = exch.ChildNodes;//得到交易所节点的所有子节点
                //debug(exchValues[0].InnerText);
                for (int i = 3; i < exchValues.Count; i++)//遍历所有交易所得产品
                {
                    string exid = ((XmlElement)exchValues[i]).GetAttribute("id").ToString();
                    string exname = ((XmlElement)exchValues[i]).GetAttribute("name").ToString();
                    //debug(id + "|" + name);
                    XmlElement securitybook = (XmlElement)exchValues[i];//将product节点转换成子节点列表
                    XmlNodeList seclist = securitybook.ChildNodes;//

                    string family = securitybook.GetAttribute("id");//合约集名称

                    //debug("合约集:" + family);
                    foreach (XmlNode s in seclist)//遍历所有合约列表
                    {
                        XmlElement sec = (XmlElement)s;

                        //XmlNodeList symNods = sec.ChildNodes;
                        string sym = sec.GetAttribute("id");//合约代码
                        string fullName = sec.GetAttribute("name");//合约名称
                        string exchange = sec.SelectSingleNode("ExchangeID").InnerText.ToString();//交易所ID
                        int mutiple = Convert.ToInt16(sec.SelectSingleNode("VolumeMultiple").InnerText.ToString());//合约乘数
                        decimal priceTick = Convert.ToDecimal(sec.SelectSingleNode("PriceTick").InnerText.ToString());//最小价格变动单位
                        decimal margin = Convert.ToDecimal(sec.SelectSingleNode("LongMarginRatio").InnerText.ToString());//保证金比例

                        Security fut = new TradingLib.Data.SecurityBase(sym, exchange, family, mutiple, priceTick, margin);
                        //int mutiple = Convert.ToInt16(symNods[].int);
                        //debug(sec.GetAttribute("id"));
                        //debug(sec);
                        futlist.Add(fut);
                    }


                }
                //debug("futlist have:" + futlist.Count.ToString() + " items");
                
             
            }

            return futlist;
        
        }
    

   
          

        public   static   string   GetEnumDescription(object   e) 
        { 
                        //获取字段信息 
                        System.Reflection.FieldInfo[]   ms   =   e.GetType().GetFields();         
                        Type   t   =   e.GetType(); 
                        foreach(System.Reflection.FieldInfo   f   in     ms)         
                        { 
                                //判断名称是否相等 
                                if(f.Name   !=   e.ToString())continue; 
                                //反射出自定义属性 
                                foreach(Attribute   attr   in     f.GetCustomAttributes(true)) 
                                { 
                                        //类型转换找到一个Description，用Description作为成员名称 
                                        System.ComponentModel.DescriptionAttribute   dscript   =   attr   as   System.ComponentModel.DescriptionAttribute;                                                         
                                        if(dscript   !=   null)                                         
                                        return       dscript.Description; 
                                } 

                        } 
                        
                        //如果没有检测到合适的注释，则用默认名称 
                        return   e.ToString(); 
                }

        public static string GetDispdecpointformat(Security sec)
        { 
            string s="N1";
            if (sec.PriceTick == (decimal)0.2)
                return "N1";
            if (sec.PriceTick == (decimal)1)
                return "N0";
            return s;
                
        }
    
    }


}
