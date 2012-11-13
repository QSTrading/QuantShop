using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using TradeLink.API;
using System.Data;

namespace TradingLib.Data
{
    //定义了交易所,交易所由ExchangeTracker管理 并从本地文件负责加载
    public class ExchangeTracker
    {
        const string EXIndex = "编码";
        const string EXCode = "代码";
        const string EXName = "名称";
        const string EXCountry = "国家";

        public event DebugDelegate SendDebugEvent;
        public ExchangeTracker()
        {
       
        }

        public static DataTable getExchTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(EXIndex);
            table.Columns.Add(EXCode);
            table.Columns.Add(EXName);
            table.Columns.Add(EXCountry);
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("Exchange");
            XmlNodeList exlist = xn.ChildNodes;
            foreach (XmlNode x in exlist)
            {
                XmlElement xe = (XmlElement)x;
                table.Rows.Add(new object[] { xe.GetAttribute("Index"), xe.SelectSingleNode("EXCode").InnerText, xe.SelectSingleNode("Name").InnerText, xe.SelectSingleNode("Country").InnerText });

            }
            return table;            
        }


        public static List<Exchange> getExchList()
        {
            List<Exchange> tmp = new List<Exchange>();

            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("Exchange");
            XmlNodeList exlist = xn.ChildNodes;
            foreach (XmlNode x in exlist)
            {
                XmlElement xe = (XmlElement)x;
                tmp.Add(new Exchange(xe.SelectSingleNode("EXCode").InnerText, xe.SelectSingleNode("Name").InnerText, (Country)Enum.Parse(typeof(Country), xe.SelectSingleNode("Country").InnerText, true)));
                //table.Rows.Add(new object[] { xe.GetAttribute("Index"), xe.SelectSingleNode("EXCode").InnerText, xe.SelectSingleNode("Name").InnerText, xe.SelectSingleNode("Country").InnerText });

            }
            return tmp;
            
        }


        private static XmlDocument getXMLDoc()
        {
            XmlDocument xmlDoc = new XmlDocument();
            //XmlReaderSettings settings = new XmlReaderSettings();
            //settings.IgnoreComments = true;//忽略文档里面的注释
           // XmlReader reader = XmlReader.Create(@"config\exchange.xml", settings);
            xmlDoc.Load(@"config\exchange.xml");
            return xmlDoc;
            
        }

        public static Exchange Exchange(string exindex)
        {
             //debug("更新数据");
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("Exchange");
            XmlNodeList exlist = xn.ChildNodes;
            Exchange ex = null;
            foreach (XmlNode x in exlist)
            {
                
                XmlElement xe = (XmlElement)x;
                if (xe.GetAttribute("Index") == exindex)
                {
                    ex = new Exchange();
                    ex.EXCode = xe.SelectSingleNode("EXCode").InnerText;
                    ex.Name = xe.SelectSingleNode("Name").InnerText;
                    ex.Country = (Country)Enum.Parse(typeof(Country), xe.SelectSingleNode("Country").InnerText, true);
                    return ex;
                    //xe.RemoveAll();
                    //x.RemoveAll();
                    //xn.RemoveChild(x);//删除该节点下所有数据包含子节点本身
                }
                

            }
            return ex;
           
    
        }
        private void debug(string s)
        {
            if (SendDebugEvent != null)
            {
                SendDebugEvent(s);
            }
        }
        
        //增加一个节点
        public static void addExchange(Exchange ex)
        {
            if (HaveExchange(ex))
            {
                updateExchange(ex);
                return;
            }
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("Exchange");

            XmlElement e = xmlDoc.CreateElement("EXCH");
            e.SetAttribute("Index", ex.Index);

            XmlElement excode = xmlDoc.CreateElement("EXCode");
            excode.InnerText = ex.EXCode;
            XmlElement name = xmlDoc.CreateElement("Name");
            name.InnerText = ex.Name;
            XmlElement country = xmlDoc.CreateElement("Country");
            country.InnerText = ex.Country.ToString();
            e.AppendChild(excode);
            e.AppendChild(name);
            e.AppendChild(country);
            xn.AppendChild(e);
            xmlDoc.Save(@"config\exchange.xml");
        
        }

        //删除一个节点
        public static void delExchange(Exchange ex)
        {
            //debug("更新数据");
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("Exchange");
            XmlNodeList exlist = xn.ChildNodes;
            foreach (XmlNode x in exlist)
            {
                XmlElement xe = (XmlElement)x;
                if (xe.GetAttribute("Index") == ex.Index)
                {
                    //xe.RemoveAll();
                    //x.RemoveAll();
                    xn.RemoveChild(x);//删除该节点下所有数据包含子节点本身
                }

            }
            xmlDoc.Save(@"config\exchange.xml");
        
            
        }

        public static void delExchange(string index)
        {
            //debug("更新数据");
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("Exchange");
            XmlNodeList exlist = xn.ChildNodes;
            foreach (XmlNode x in exlist)
            {
                XmlElement xe = (XmlElement)x;
                if (xe.GetAttribute("Index") == index)
                {
                    //xe.RemoveAll();
                    //x.RemoveAll();
                    xn.RemoveChild(x);//删除该节点下所有数据包含子节点本身
                }

            }
            xmlDoc.Save(@"config\exchange.xml");


        }

        //更新一个节点
        public static void updateExchange(Exchange ex)
        {
            if (!HaveExchange(ex))
            {
                addExchange(ex);
                return;
            }
            //debug("更新数据");
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("Exchange");
            XmlNodeList exlist = xn.ChildNodes;
            foreach (XmlNode x in exlist)
            {
                XmlElement xe = (XmlElement)x;
                if (xe.GetAttribute("Index") == ex.Index)
                {
                    //debug("it is here");
                    xe.SelectSingleNode("EXCode").InnerText = ex.EXCode;
                    xe.SelectSingleNode("Name").InnerText = ex.Name;
                    //debug(ex.Name);
                    xe.SelectSingleNode("Country").InnerText = ex.Country.ToString();
                }
            }
            xmlDoc.Save(@"config\exchange.xml");



            
        }

        //查询一个结点
        public static bool HaveExchange(Exchange ex)
        {
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("Exchange");
            XmlNodeList exlist = xn.ChildNodes;
            foreach (XmlNode x in exlist)
            {
                XmlElement xe = (XmlElement)x;
                if (xe.GetAttribute("Index") == ex.Index)
                    return true;
            }

            return false;
            
        }
    }

    

    public class Exchange
    {
        private string _ex;//交易所代码
        private string _name;//交易所名称
        private Country _country;//交易所所处国家
        //private Session _session;//交易所交易时间

        public string EXCode { get { return _ex; } set { _ex = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public Country Country { get { return _country; } set { _country = value; } }

        //获得交易所本地编码由国家与交易所代码组成
        public string Index { get { return _country.ToString() + "_" + _ex.ToString(); } }

        public Exchange()
        { }

        public Exchange(string ex, string name, Country country)
        {
            _ex = ex;
            _name = name;
            _country = country;

        }
    }
}
