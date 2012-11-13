using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using TradeLink.API;
using TradingLib.API;

namespace TradingLib.Data
{
    public class PositionCheckTracker
    {
        private static XmlDocument getXMLDoc()
        {
            XmlDocument xmlDoc = new XmlDocument();
            //XmlReaderSettings settings = new XmlReaderSettings();
            //settings.IgnoreComments = true;//忽略文档里面的注释
            // XmlReader reader = XmlReader.Create(@"config\exchange.xml", settings);
            xmlDoc.Load(@"config\PositionCheck.xml");
            return xmlDoc;

        }
        public static List<string> getPositionCheckFromSymbol(string sym)
        {
            List<string> l = new List<string>();
            //if (!HaveSymbol(sym))
            //    return l;
            //获得basket的node
            XmlNode bnode = null;
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("PositionCheck");
            XmlNodeList exlist = xn.ChildNodes;
            foreach (XmlNode x in exlist)
            {
                XmlElement xe = (XmlElement)x;
                if (xe.GetAttribute("Symbol") == sym)
                    bnode = x;
            }
            if (bnode == null)
                return l;

            foreach (XmlNode x in bnode.ChildNodes)
            {
                XmlElement xe = (XmlElement)x;
                l.Add(xe.GetAttribute("Name") + ":" + xe.GetAttribute("Text"));
            }
            return l;
        }
        //将某个positioncheck添加到对应的symbol中
        public static void addPositionCheckIntoSymbol(string sym, string name, string positioncfg)
        {
            //如果某个symbol已经有该position了,则直接返回
            if (HavePositionCheckInSymbol(sym,name,positioncfg))
                return;
            if (!HaveSymbol(sym))
                addSymbol(sym);
            //获得basket的node
            XmlNode bnode = null;
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("PositionCheck");
            XmlNodeList exlist = xn.ChildNodes;
            foreach (XmlNode x in exlist)
            {
                XmlElement xe = (XmlElement)x;
                if (xe.GetAttribute("Symbol") == sym)
                    bnode = x;
            }

            XmlElement e = xmlDoc.CreateElement("PosCheck");
            e.SetAttribute("Name", name);
            e.SetAttribute("Text", positioncfg);
            bnode.AppendChild(e);
            xmlDoc.Save(@"config\PositionCheck.xml");

        }

        public static void addSymbol(string sym)
        {

            if (HaveSymbol(sym))
            {
                //updateBasket(b);
                return;
            }
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("PositionCheck");

            XmlElement e = xmlDoc.CreateElement("SymbolSection");
            e.SetAttribute("Symbol", sym);
            xn.AppendChild(e);
            xmlDoc.Save(@"config\PositionCheck.xml");
        }
        //检查positioncheck配置中是否存在某个Symbol的Section
        public static bool HaveSymbol(string sym)
        {
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("PositionCheck");
            XmlNodeList exlist = xn.ChildNodes;
            foreach (XmlNode x in exlist)
            {
                XmlElement xe = (XmlElement)x;
                if (xe.GetAttribute("Symbol") == sym)
                    return true;
            }

            return false;
        }

        //检查某个symbol是否有某个positioncheck
        private static bool HavePositionCheckInSymbol(string symbol, string name, string config)
        {

            XmlNode bnode = null;
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("PositionCheck");
            XmlNodeList exlist = xn.ChildNodes;
            foreach (XmlNode x in exlist)
            {
                XmlElement xe = (XmlElement)x;
                if (xe.GetAttribute("Symbol") == symbol)
                    bnode = x;
            }
            if (bnode == null)
                return false;
            foreach (XmlNode x in bnode.ChildNodes)
            {
                XmlElement xe = (XmlElement)x;
                if ((xe.GetAttribute("Name") == name) && (xe.GetAttribute("Text") == config))
                    return true;
            }
            return false;  
        }

        public static void delPositionCheckFromSymbol(string symbol, string name, string config)
        {

            XmlNode bnode = null;
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("PositionCheck");
            XmlNodeList exlist = xn.ChildNodes;
            foreach (XmlNode x in exlist)
            {
                XmlElement xe = (XmlElement)x;
                if (xe.GetAttribute("Symbol") == symbol)
                    bnode = x;
            }
            if (bnode == null)
                return;
            foreach (XmlNode x in bnode.ChildNodes)
            {
                XmlElement xe = (XmlElement)x;
                if ((xe.GetAttribute("Name") == name) && (xe.GetAttribute("Text") == config))
                    bnode.RemoveChild(x);
            }
            xmlDoc.Save(@"config\PositionCheck.xml");
        }


    }
}
