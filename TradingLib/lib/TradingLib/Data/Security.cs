using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeLink.Common;
using System.Collections;
using System.Data;
using System.Xml;
using TradeLink.API;
namespace TradingLib.Data
{
        
    public class SecurityTracker
    {
        const string FULLNAME = "全称";
        const string SYMBOL = "代码";
        const string EXCHANGE = "交易所";
        const string DESCRIPTION = "名称";
        const string TYPE = "类型";
        const string MULTIPLE = "乘数";
        const string PRICETICK = "跳";
        const string MARGIN = "保证金";
        public event DebugDelegate SendDebugEvent;

        public static DataTable getSecuityTable()
        {
            return getSecuityTable(string.Empty, string.Empty);
        }
        public static DataTable getSecuityTable(string secType,string exchange)

        {
            DataTable table = new DataTable();
            table.Columns.Add(FULLNAME);
            table.Columns.Add(SYMBOL);
            table.Columns.Add(EXCHANGE);
            table.Columns.Add(DESCRIPTION);
            table.Columns.Add(TYPE);
            table.Columns.Add(MULTIPLE);
            table.Columns.Add(PRICETICK);
            table.Columns.Add(MARGIN);
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("Security");
            XmlNodeList exlist = xn.ChildNodes;
            foreach (XmlNode x in exlist)
            {
                XmlElement xe = (XmlElement)x;
                string e = x.SelectSingleNode("Exchange").InnerText.ToString();
                string s = x.SelectSingleNode("SecType").InnerText.ToString();
                if (((secType == string.Empty) || (secType == s)) && ((exchange == string.Empty) || (exchange == e)))
                {
                    table.Rows.Add(new object[] { xe.GetAttribute("FN"), xe.SelectSingleNode("Symbol").InnerText, xe.SelectSingleNode("Exchange").InnerText, xe.SelectSingleNode("Description").InnerText, xe.SelectSingleNode("SecType").InnerText, xe.SelectSingleNode("Multiple").InnerText, xe.SelectSingleNode("PriceTick").InnerText, xe.SelectSingleNode("Margin").InnerText });

                }
            }
            return table;
        }

        

        private static XmlDocument getXMLDoc()
        {
            XmlDocument xmlDoc = new XmlDocument();
            //XmlReaderSettings settings = new XmlReaderSettings();
            //settings.IgnoreComments = true;//忽略文档里面的注释
            // XmlReader reader = XmlReader.Create(@"config\exchange.xml", settings);
            xmlDoc.Load(@"config\security.xml");
            return xmlDoc;

        }

        //删除一个节点
        public static void delSecurity(SecurityBase sec)
        {
            delSecurity(sec.FullName);
        }

        public static void delSecurity(string fn)
        {
            //debug("更新数据");
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("Security");
            XmlNodeList exlist = xn.ChildNodes;
            foreach (XmlNode x in exlist)
            {
                XmlElement xe = (XmlElement)x;
                if (xe.GetAttribute("FN") == fn)
                {
                    xn.RemoveChild(x);//删除该节点下所有数据包含子节点本身
                }
            }
            xmlDoc.Save(@"config\Security.xml");


        }

        public static Security getSecurity(string fn)
        {
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("Security");
            XmlNodeList exlist = xn.ChildNodes;
            foreach (XmlNode x in exlist)
            {
                XmlElement xe = (XmlElement)x;
                if (xe.GetAttribute("FN") == fn)
                {   
                    //SecurityImpl.Parse(fn)
                    string sym = xe.SelectSingleNode("Symbol").InnerText.ToString();
                    string ex = xe.SelectSingleNode("Exchange").InnerText.ToString();
                    string desp = xe.SelectSingleNode("Description").InnerText.ToString();
                    SecurityType type = (SecurityType)Enum.Parse(typeof(SecurityType), xe.SelectSingleNode("SecType").InnerText.ToString(), true);
                    int multiple = Convert.ToInt16(xe.SelectSingleNode("Multiple").InnerText.ToString());
                    decimal pricetick = Convert.ToDecimal(xe.SelectSingleNode("PriceTick").InnerText.ToString());
                    decimal margin = Convert.ToDecimal(xe.SelectSingleNode("Margin").InnerText.ToString());

                    return new SecurityBase(sym, ex, multiple, pricetick, margin);
                      //SecurityBase        
                }  
            }
            return null;
        }

        public static void addSecurity(SecurityBase sec)
        {
            if (HaveSecurity(sec))
            {
                updateSecurity(sec);
                return;
            }
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("Security");

            XmlElement e = xmlDoc.CreateElement("Sec");
            e.SetAttribute("FN", sec.FullName);

            XmlElement symbol = xmlDoc.CreateElement("Symbol");//代码
            symbol.InnerText = sec.Symbol;
            XmlElement exchange = xmlDoc.CreateElement("Exchange");//交易所
            exchange.InnerText = sec.DestEx;
            XmlElement des = xmlDoc.CreateElement("Description");//交易所
            des.InnerText = sec.Description;
            XmlElement secType = xmlDoc.CreateElement("SecType");//类别
            secType.InnerText = sec.Type.ToString();
            XmlElement multiple = xmlDoc.CreateElement("Multiple");//乘数
            multiple.InnerText = sec.Multiple.ToString();
            XmlElement priceTick = xmlDoc.CreateElement("PriceTick");//最小价格变动
            priceTick.InnerText = sec.PriceTick.ToString();
            XmlElement marginRation = xmlDoc.CreateElement("Margin");//保证金
            marginRation.InnerText = sec.Margin.ToString();


            e.AppendChild(symbol);
            e.AppendChild(exchange);
            e.AppendChild(des);
            e.AppendChild(secType);
            e.AppendChild(multiple);
            e.AppendChild(priceTick);
            e.AppendChild(marginRation);



            xn.AppendChild(e);
            xmlDoc.Save(@"config\security.xml");

        }

        //更新一个节点
        public static void updateSecurity(SecurityBase sec)
        {
            if (!HaveSecurity(sec))
            {   //如果没有该sec记录则直接插入
                addSecurity(sec);
                return;
            }
            //若有该sec记录，则先删除然后再插入
            delSecurity(sec.FullName);
            addSecurity(sec);
        }

        public static bool HaveSecurity(Security sec)
        {
            return HaveSecurity(sec.FullName);
        }
        public static bool HaveSecurity(string fn)
        {
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("Security");
            XmlNodeList exlist = xn.ChildNodes;
            foreach (XmlNode x in exlist)
            {
                XmlElement xe = (XmlElement)x;
                if (xe.GetAttribute("FN") == fn)
                    return true;
            }

            return false;

        }




    }
    public class SecurityBase : SecurityImpl,Security
    {
        //private decimal _priceTick;//最小价格变动单位
        //private int _multiple;//该期货合约乘数
        //private decimal _marginRation;//合约保证金系数
        private string _family;//合约类别
        //private TradingLib.Data.Exchange _exchange;

        //public decimal PriceTick { get { return _priceTick; } set { _priceTick = value; } }
        //public int Multiple { get { return _multiple; } set { _multiple = value; } }
        //public decimal MarginRatio { get { return _marginRation; } set { _marginRation = value; } }
        public string SecFamily { get { return _family; } set { _family = value; } }



        //实例化Future合约
        public SecurityBase(string sym, string exchange,string family ,int mutiple, decimal priceTick, decimal margin)
            : base(sym, exchange, TradeLink.API.SecurityType.FUT)
        {
            Margin = margin;
            PriceTick = priceTick;
            Multiple = mutiple;

        }

        public SecurityBase(string sym, string exchange, int mutiple, decimal priceTick, decimal margin)
            : base(sym, exchange, TradeLink.API.SecurityType.FUT)
        {
            Margin = margin;
            PriceTick = priceTick;
            Multiple = mutiple;

        }
        public SecurityBase(string sym, string exchange,SecurityType type,string des, int mutiple, decimal priceTick, decimal margin)
            : base(sym, exchange, type)
        {
            Description = des;
            Margin = margin;
            PriceTick = priceTick;
            Multiple = mutiple;

        }
        //期货合约定义的时候需要制定最小价格变动,与乘数
        /*
        public SecurityBase(string sym, Exchange exch, int mutiple, decimal priceTick)
            : base(sym, exch.Index, TradeLink.API.SecurityType.FUT)
        {



        }*/










    }
}
