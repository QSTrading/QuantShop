using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using TradingLib.API;
using TradingLib.Data;
using System.Windows.Forms;
namespace TradingLib.Data
{
    public class RiskRuleTracker
    {

        private static XmlDocument getXMLDoc()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(@"config\RiskRule.xml");
            return xmlDoc;
        }


        public static void delRuleFromAccount(string acc, IRuleCheck rc)
        {
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("RuleSet");
            XmlNodeList exlist = xn.ChildNodes;
            foreach (XmlNode x in exlist)
            {
                XmlElement xe = (XmlElement)x;
                if (xe.GetAttribute("ID") == acc)
                {
                    foreach (XmlNode rsnode in xe.ChildNodes)
                    {
                        XmlElement rs = (XmlElement)rsnode;
                         string s = rs.GetAttribute("Text");
                        if(s!=string.Empty || s!=null)
                        {
                            //MessageBox.Show((s.Split('|')[1]==rc.ToText().Split('|')[1]).ToString());
                            if (s.Split('|')[1] == rc.ToText().Split('|')[1])
                                x.RemoveChild(rs);
                        }
                    }
                }
            }
            xmlDoc.Save(@"config\RiskRule.xml");

        }
        public static List<string> getRuleTextFromAccount(string acc)
        {
            List<string> r = new List<string>();
            if (!HaveAccount(acc))
                return  r;
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("RuleSet");
            XmlNodeList exlist = xn.ChildNodes;
            foreach (XmlNode x in exlist)
            {

                XmlElement xe = (XmlElement)x;
                if (xe.GetAttribute("ID") == acc)
                {
                    foreach (XmlNode rsnode in xe.ChildNodes)
                    {
                        XmlElement rs = (XmlElement)rsnode;
                        string rsset = rs.SelectSingleNode("RSName").InnerText.ToString();
                        r.Add(rsset + "#" + rs.GetAttribute("Text"));
                    }

                }
            }
            return r;
        }
        public static bool addRuleIntoAccount(string acc, IRuleCheck rc)
        {
            if (!HaveAccount(acc))
                addAccount(acc);
            if (AccountHaveRule(acc, rc))
            {   //MessageBox.Show("del the rule");
                delRuleFromAccount(acc, rc);
            }
            XmlNode bnode = null;
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("RuleSet");
            XmlNodeList exlist = xn.ChildNodes;
            foreach (XmlNode x in exlist)
            {
                
                XmlElement xe = (XmlElement)x;
                if (xe.GetAttribute("ID") == acc)
                {
                    bnode = x;
               

                XmlElement e = xmlDoc.CreateElement("RS");
                e.SetAttribute("Text", rc.ToText());
                XmlElement rsname = xmlDoc.CreateElement("RSName");
                rsname.InnerText = rc.GetType().ToString();
                XmlElement enable = xmlDoc.CreateElement("Enable");
                enable.InnerText = rc.Enable.ToString();
                XmlElement value = xmlDoc.CreateElement("Value");
                value.InnerText = rc.Value.ToString();
                XmlElement compare = xmlDoc.CreateElement("Compare");
                compare.InnerText = rc.Compare.ToString();
                e.AppendChild(rsname);
                e.AppendChild(enable);
                e.AppendChild(value);
                e.AppendChild(compare);
                bnode.AppendChild(e);
                }
            }

            xmlDoc.Save(@"config\RiskRule.xml");
            return true;
        }

        public static void addAccount(string acc)
        {
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("RuleSet");

            XmlElement e = xmlDoc.CreateElement("Account");
            e.SetAttribute("ID",acc);
            xn.AppendChild(e);
            xmlDoc.Save(@"config\RiskRule.xml");
        
        
        }
        public static bool HaveAccount(AccountBase a)
        {
            return HaveAccount(a.ID);
        }
        public static bool HaveAccount(string acc)
        {
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("RuleSet");
            XmlNodeList exlist = xn.ChildNodes;
            foreach (XmlNode x in exlist)
            {
                XmlElement xe = (XmlElement)x;
                if (xe.GetAttribute("ID") == acc)
                    return true;
            }
            return false;
        }

        public static bool AccountHaveRule(string acc,IRuleCheck rc)
        {
            XmlDocument xmlDoc = getXMLDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("RuleSet");
            XmlNodeList exlist = xn.ChildNodes;
            foreach (XmlNode x in exlist)
            {
                XmlElement xe = (XmlElement)x;
                if (xe.GetAttribute("ID") == acc)
                {
                    foreach (XmlNode rsnode in xe.ChildNodes)
                    {
                        XmlElement rs = (XmlElement)rsnode;
                        //.
                        string s = rs.GetAttribute("Text");
                        if(s!=string.Empty || s!=null)
                        {
                            //MessageBox.Show((s.Split('|')[1]==rc.ToText().Split('|')[1]).ToString());
                            if (s.Split('|')[1] == rc.ToText().Split('|')[1])
                                return true;
                        }
                        else
                            return false;
                    }
                }
            }
            return false;
            
        }
    }
}
