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

    //保存具体的合约代码并组成symbol 列表 合约列表使用basketimpl来实现
    /// <summary>
    /// symbol:symbol全称
    /// MasterFN:对应证券全名用于索引security
    /// 其他相关信息
    /// </summary>
    /// 
    //用于读取 保存 basket列表
    public class BasketTracker
    {
        private static XmlDocument getXMLDoc()
        {
            XmlDocument xmlDoc = new XmlDocument();
            //XmlReaderSettings settings = new XmlReaderSettings();
            //settings.IgnoreComments = true;//忽略文档里面的注释
            // XmlReader reader = XmlReader.Create(@"config\exchange.xml", settings);
            xmlDoc.Load(@"config\SecList.xml");
            return xmlDoc;

        }

        //更新一个节点
        public static void updateBasket(string  b)
        {
            if (!HaveBasket(b))
            {   //如果没有该sec记录则直接插入
                addBasket(b);
                return;
            }
            //若有该sec记录，则先删除然后再插入
            delBasket(b);
            addBasket(b);
        }

        public static void delBasket(string fn)
        {
            //debug("更新数据");
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("SecList");
            XmlNodeList exlist = xn.ChildNodes;
            foreach (XmlNode x in exlist)
            {
                XmlElement xe = (XmlElement)x;
                if (xe.GetAttribute("Name") == fn)
                {
                    xn.RemoveChild(x);//删除该节点下所有数据包含子节点本身
                }
            }
            xmlDoc.Save(@"config\SecList.xml");


        }

        public static void addBasket(string b)
        {
            if (HaveBasket(b))
            {
                //updateBasket(b);
                return;
            }
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("SecList");

            XmlElement e = xmlDoc.CreateElement("Basket");
            e.SetAttribute("Name",b);
            
            //XmlElement symbol = xmlDoc.CreateElement("Symbol");//代码
            //symbol.InnerText = "IF1210";
            /*
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
            */

            //e.AppendChild(symbol);
            //e.AppendChild(exchange);
            //e.AppendChild(des);
            //e.AppendChild(secType);
            //e.AppendChild(multiple);
            //e.AppendChild(priceTick);
            //e.AppendChild(marginRation);

            xn.AppendChild(e);
            xmlDoc.Save(@"config\SecList.xml");

        }


        //将某个合约从basket中删除
        public static void delSecFromBasket(string sym, string basket)
        {
            //如果不存在某个basket则我们添加该basket
            if (!HaveBasket(basket))
                return;
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("SecList");
            XmlNodeList exlist = xn.ChildNodes;
            foreach (XmlNode x in exlist)
            {
                XmlElement xe = (XmlElement)x;
                if (xe.GetAttribute("Name") == basket)
                {
                    foreach (XmlNode s in x.ChildNodes)
                    {
                        XmlElement xs = (XmlElement)s;
                        if (xs.GetAttribute("Symbol") == sym)
                        {
                            x.RemoveChild(xs);//删除该节点下所有数据包含子节点本身
                        }
                    }
                }
            }
            xmlDoc.Save(@"config\SecList.xml");
        }
        //将某个合约插入到某个Basket中
        public static string addSecIntoBasket(Security sec,string basket,string expire)
        {
            //如果不存在某个basket则我们添加该basket
            string s = string.Empty;
            if (!HaveBasket(basket))
                addBasket(basket);
            //获得basket的node
            XmlNode bnode = null;
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("SecList");
            XmlNodeList exlist = xn.ChildNodes;
            foreach (XmlNode x in exlist)
            {
                XmlElement xe = (XmlElement)x;
                if (xe.GetAttribute("Name") == basket)
                    bnode = x;
            }

            //增加一个Security结点
            switch (sec.Type)
            {
                //Fut类型的security需要制定到期日期以及保证金
                case SecurityType.FUT:
                    {
                        XmlElement e = xmlDoc.CreateElement("Sec");
                        string sym = sec.Symbol + expire;
                        s = sym;
                        e.SetAttribute("Symbol", sym);
                        //检查是否有该symbol如果有则我们返回
                        if (HaveSecInBasket(sym, basket))
                            break ;
                        XmlElement symbol = xmlDoc.CreateElement("MasterSec");//代码从该代码可以从security数据中获取主Security
                        symbol.InnerText = sec.FullName;
                        XmlElement secType = xmlDoc.CreateElement("SecType");//类别
                        secType.InnerText = sec.Type.ToString();
                        XmlElement margin = xmlDoc.CreateElement("Margin");//保证金
                        margin.InnerText = sec.Margin.ToString();
                        XmlElement exdate = xmlDoc.CreateElement("Expire");//保证金
                        exdate.InnerText = expire;
                        e.AppendChild(symbol);
                        e.AppendChild(secType);
                        e.AppendChild(margin);
                        e.AppendChild(exdate);
                        bnode.AppendChild(e);
                        //return sym;
                        break;
                    }
                case SecurityType.STK:
                    {
                        XmlElement e = xmlDoc.CreateElement("Sec");
                        e.SetAttribute("Symbol", sec.Symbol);
                        XmlElement symbol = xmlDoc.CreateElement("MasterSec");//代码从该代码可以从security数据中获取主Security
                        symbol.InnerText = sec.FullName;
                        XmlElement secType = xmlDoc.CreateElement("SecType");//类别
                        secType.InnerText = sec.Type.ToString();
                        e.AppendChild(symbol);
                        e.AppendChild(secType);
                        bnode.AppendChild(e);
                        s = sec.Symbol;
                        //return sec.Symbol;
                        break;
                    }
                default :
                    
                    break;
           }
            xmlDoc.Save(@"config\SecList.xml");
            return s;
        }

        //获得对应的basket name列表
        public static string[] getBaskets()
        {
            ArrayList blist = new ArrayList();
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("SecList");
            XmlNodeList exlist = xn.ChildNodes;
            foreach (XmlNode x in exlist)
            {
                XmlElement xe = (XmlElement)x;
                blist.Add(xe.GetAttribute("Name"));
            }
            string[] ret = new string[blist.Count];
            for (int i = 0; i < blist.Count; i++)
            {
                ret[i] = (string)blist[i];
            }
            return ret;     
        }

        //获得所有交易所得代码
        public static Basket getBasket()
        {
            Basket b = new BasketImpl();
            List<Exchange> exlist = ExchangeTracker.getExchList();
            foreach (Exchange ex in exlist)
            {
                string exname = ex.EXCode;
                b.Add(getBasket(exname));
            }
            return b;
        
        }
        //通过列表名获得security的basket
        public static Basket getBasket(string basket)
        {
            Basket b = new BasketImpl();
            ArrayList seclist = new ArrayList();
            b.Name = basket;
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("SecList");
            XmlNodeList exlist = xn.ChildNodes;
            foreach (XmlNode x in exlist)
            {
                XmlElement xe = (XmlElement)x;
                if (xe.GetAttribute("Name") == basket)
                {
                    foreach (XmlNode secnode in xe.ChildNodes)
                    {
                        XmlElement sec = (XmlElement)secnode;
                        string sym = sec.GetAttribute("Symbol");//获得合约symbol
                        string mastSec = sec.SelectSingleNode("MasterSec").InnerText.ToString();//获得Security
                        //SecurityType secType = Enum.Parse(SecurityType,sec.SelectSingleNode("MasterSec").InnerText.ToString())
                        Security s = SecurityImpl.Parse(mastSec);
                        s.MasterSecFN = mastSec;//记录主合约全名
                        Security mastersec = SecurityTracker.getSecurity(mastSec);
                        s.Symbol = sym;
                        //利用masterSecurity 来填充 合约priceTick Margin mutiple等参数
                        switch (s.Type)
                        {
                            case SecurityType.FUT:
                                {
                                    s.Margin = Convert.ToDecimal(sec.SelectSingleNode("Margin").InnerText.ToString());
                                    s.Date = Convert.ToInt16(sec.SelectSingleNode("Expire").InnerText.ToString());
                                    //if (mastersec != null)
                                        //s.PriceTick = mastersec.PriceTick; s.Multiple = mastersec.Multiple;
                                    //s.PriceTick = mastersec.PriceTick;
                                    //s.Multiple = mastersec.Multiple;

                                    //s.Multiple = 
                                    break;
                                }
                            default:
                                break;
                        }

                        b.Add(s);//将从xml文档实例化得到的security加入basket
                    }
                }
            }
            return b;
        }

        //通过列表名获得security的basket
        public static Security getSecFromBasket(string symbol,string basket)
        {
            Security s = null;
            //Basket b = new BasketImpl();
            //ArrayList seclist = new ArrayList();
            //b.Name = basket;
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("SecList");
            XmlNodeList exlist = xn.ChildNodes;
            foreach (XmlNode x in exlist)
            {
                XmlElement xe = (XmlElement)x;
                if (xe.GetAttribute("Name") == basket)
                {
                    foreach (XmlNode secnode in xe.ChildNodes)
                    {
                        XmlElement sec = (XmlElement)secnode;
                        string sym = sec.GetAttribute("Symbol");//获得合约symbol
                        if (sym == symbol)
                        {
                            string mastSec = sec.SelectSingleNode("MasterSec").InnerText.ToString();//获得Security
                            //SecurityType secType = Enum.Parse(SecurityType,sec.SelectSingleNode("MasterSec").InnerText.ToString())
                            s = SecurityImpl.Parse(mastSec);
                            s.MasterSecFN = mastSec;//记录主合约全名
                            s.Symbol = sym;
                            //利用masterSecurity 来填充 合约priceTick Margin mutiple等参数
                            switch (s.Type)
                            {
                                case SecurityType.FUT:
                                    {
                                        s.Margin = Convert.ToDecimal(sec.SelectSingleNode("Margin").InnerText.ToString());
                                        s.Date = Convert.ToInt16(sec.SelectSingleNode("Expire").InnerText.ToString());
                                        break;
                                    }
                                default:
                                    break;
                            }
                        }

                        //b.Add(s);//将从xml文档实例化得到的security加入basket
                    }
                }
            }
            return s;
        }

        //获得某个basket的symbol列表
        public static string[] getSymbolList(string basket)
        {
            ArrayList seclist = new ArrayList();
            //if (!HaveBasket(basket))
            //    return seclist;
            
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("SecList");
            XmlNodeList exlist = xn.ChildNodes;
            foreach (XmlNode x in exlist)
            {
                XmlElement xe = (XmlElement)x;
                if (xe.GetAttribute("Name") == basket)
                {
                    foreach (XmlNode secnode in xe.ChildNodes)
                    {
                        XmlElement sec = (XmlElement)secnode;
                        seclist.Add(sec.GetAttribute("Symbol"));
                    }
                }      
            }
            string[] ret = new string[seclist.Count];
            for(int i =0 ; i < seclist.Count; i ++)
            {
                ret[i] = (string)seclist[i];
            }
            return ret;     
        }


        public static bool HaveSecInBasket(string symbol,string basket)
        { 
            if(!HaveBasket(basket))
                return false;
            //获得basket的node
            //XmlNode bnode = null;
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("SecList");
            XmlNodeList exlist = xn.ChildNodes;
            foreach (XmlNode x in exlist)
            {
                XmlElement xe = (XmlElement)x;
                if (xe.GetAttribute("Name") == basket)
                {
                    foreach (XmlNode secnode in xe.ChildNodes)
                    {
                        XmlElement sec = (XmlElement)secnode;
                        if (sec.GetAttribute("Symbol") == symbol)
                            return true;
                    }
                }
            }

            return false;
        }
        public static bool HaveBasket(Basket b)
        {
            return HaveBasket(b.Name);
        }

        public static bool HaveBasket(string bn)
        {
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("SecList");
            XmlNodeList exlist = xn.ChildNodes;
            foreach (XmlNode x in exlist)
            {
                XmlElement xe = (XmlElement)x;
                if (xe.GetAttribute("Name") == bn)
                    return true;
            }

            return false;

        }
    

    
    }
    public class SecurityList
    {


    }
}
