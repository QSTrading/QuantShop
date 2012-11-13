using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using TradingLib.Data;
using TradeLink.API;
using TradeLink.Common;

namespace TradingLib.GUI
{
    public class UIUtil
    {
        public  const string ANY = "<Any>";
        public static void genSecTreeView(ref TreeView tv)
        {
            string[] baskets = BasketTracker.getBaskets();
            for (int i = 0; i < baskets.Length; i++)
            {
                string[] secs = BasketTracker.getSymbolList(baskets[i]);
                TreeNode[] t = new TreeNode[secs.Length];
                for (int j = 0; j < secs.Length; j++)
                {
                    t[j] = new TreeNode(secs[j]);
                }
                tv.Nodes.Add(new TreeNode(baskets[i], t));
            }
        }

        //将枚举类型形成下拉惨淡栏输出，用于用户UI
        public static void genComboBoxList<T>(ref ComboBox cb)
        {
            genComboBoxList<T>(ref cb,false);
        }
        
        public static void genComboBoxList<T>(ref ComboBox cb,bool isany)
        {
            ArrayList list = new ArrayList();
            if (isany)
            {
                ValueObject<T> vo = new ValueObject<T>();
                vo.Name = ANY;
                vo.Value = (T)(Enum.GetValues(typeof(T)).GetValue(0));
                list.Add(vo);
            }
           
            foreach (T c in Enum.GetValues(typeof(T)))
            {
                ValueObject<T> vo = new ValueObject<T>();
                vo.Name = LibUtil.GetEnumDescription(c);
                vo.Value = c;
                list.Add(vo);
            }
            cb.DataSource = list;
            cb.DisplayMember = "Name";
            cb.ValueMember = "Value";
            
            //cb.Items.AddRange((ComboBox.ObjectCollection[])list.ToArray());
        }
        //获得positioncheck列表选择框
        public static void genCheckedListBoxPositoinCheck(ref CheckedListBox ck, Dictionary<string, Type> data)
        {
            ArrayList list = new ArrayList();
            foreach (string name in data.Keys)
            {
                ValueObject<Type> vo = new ValueObject<Type>();
                vo.Name = name;
                vo.Value = data[name];
                list.Add(vo);
            }
            ck.DataSource = list;
            ck.DisplayMember = "Name";
            ck.ValueMember = "Value";

        }

        public static void genCheckedListBoxRuleSet(ref CheckedListBox ck, Dictionary<string, Type> data)
        {
            ArrayList list = new ArrayList();
            foreach (string name in data.Keys)
            {
                ValueObject<Type> vo = new ValueObject<Type>();
                vo.Name = name;
                vo.Value = data[name];
                list.Add(vo);
            }
            ck.DataSource = list;
            ck.DisplayMember = "Name";
            ck.ValueMember = "Value";
        
        }
        public static void genListBoxBasket(ref ListBox lb, string basket)
        {
            ArrayList list = new ArrayList();
            //string[] secs = BasketTracker.getSymbolList(basket);
            Basket b = BasketTracker.getBasket(basket);
            foreach(Security sec in b.ToArray())
            { 
                ValueObject<Security> vo = new ValueObject<Security>();
                vo.Name = sec.Symbol;
                vo.Value = sec;
                list.Add(vo);
            }
            /*for (int i = 0; i < secs.Length; i++)
            {
                ValueObject<string> vo = new ValueObject<string>();
                vo.Name = secs[i];
                vo.Value = secs[i];
                list.Add(vo); 
            }
             * */
            lb.DataSource = list;
            lb.DisplayMember = "Name";
            lb.ValueMember = "Value";
            
        }
        //生成basket选择列表
        public static void genComboBoxBasekt(ref ComboBox cb)
        {
            genComboBoxBasekt(ref cb, false);
        }
        public static void genComboBoxBasekt(ref ComboBox cb,bool isany)
        {
            
            ArrayList list = new ArrayList();
            if (isany)
            {
                ValueObject<string> vo = new ValueObject<string>();
                vo.Name = ANY;
                vo.Value = ANY;
                list.Add(vo); 
            }
            string[] baskets = BasketTracker.getBaskets();
            for (int i = 0; i < baskets.Length; i++)
            {
                ValueObject<string> vo = new ValueObject<string>();
                vo.Name = baskets[i];
                vo.Value = baskets[i];
                list.Add(vo); 
            }
            cb.DataSource = list;
            cb.DisplayMember = "Name";
            cb.ValueMember = "Value";
            
        }

        public static void genComboBoxHistory(ref ToolStripComboBox cb)
        {
            ArrayList list = new ArrayList();
            DateTime today = DateTime.Now;//Convert.ToDateTime(DateTime.Now.AddMonths(1).ToString("yyyy-MM-01")).AddDays(-1);
            //1周
            ValueObject<int> vo1 = new ValueObject<int>();
            vo1.Name = "1周";
            vo1.Value = Util.ToTLDate(today.AddDays(-7));
            list.Add(vo1);
            //1月
            ValueObject<int> vo2 = new ValueObject<int>();
            vo2.Name = "1月";
            vo2.Value = Util.ToTLDate(today.AddDays(-30));
            list.Add(vo2);
            //半年
            ValueObject<int> vo3 = new ValueObject<int>();
            vo3.Name = "半年";
            vo3.Value = Util.ToTLDate(today.AddDays(-180));
            list.Add(vo3);
            //1年
            ValueObject<int> vo4 = new ValueObject<int>();
            vo4.Name = "1年";
            vo4.Value = Util.ToTLDate(today.AddYears(-1));
            list.Add(vo4);
            //2年
            ValueObject<int> vo5 = new ValueObject<int>();
            vo5.Name = "2年";
            vo5.Value = Util.ToTLDate(today.AddYears(-2));
            list.Add(vo4);
            //5年


            //cb.Items.Add
            cb.ComboBox.DataSource = list;
            //cb.DataSource = list;
            cb.ComboBox.DisplayMember = "Name";
            cb.ComboBox.ValueMember = "Value";
        }

        public static void genComboBoxExpire(ref ComboBox cb)
        {
            ArrayList list = new ArrayList();
            DateTime lastDay = Convert.ToDateTime(DateTime.Now.AddMonths(1).ToString("yyyy-MM-01")).AddDays(-1);
            for (int i = 0; i < 12; i++)
            {
                ValueObject<int> vo = new ValueObject<int>();

                vo.Name = lastDay.AddMonths(i).ToString("yyMM");
                vo.Value = Convert.ToInt16(vo.Name);//Util.DT2FT(lastDay.AddMonths(i));
                list.Add(vo);
            }
            cb.DataSource = list;
            cb.DisplayMember = "Name";
            cb.ValueMember = "Value";
        }

        public static void genComboBoxExchange(ref ComboBox cb)
        {
            genComboBoxExchange(ref cb, false);
        }
        public static void genComboBoxExchange(ref ComboBox cb,bool isany)
        {
            ArrayList list = new ArrayList();
            if (isany)
            {
                ValueObject<Exchange> vo = new ValueObject<Exchange>();
                vo.Name = ANY;
                vo.Value = new Exchange(); ;
                list.Add(vo);
                
            }
            foreach (Exchange e in ExchangeTracker.getExchList())
            {
                ValueObject<Exchange> vo = new ValueObject<Exchange>();
                vo.Name = e.Name;
                vo.Value = e;
                list.Add(vo);
            }
            cb.DataSource = list;
            cb.DisplayMember = "Name";
            cb.ValueMember = "Value";
        }

        public static void genExchangeList(ref CheckedListBox ck)
        {
            ArrayList list = new ArrayList();
             foreach (Exchange e in ExchangeTracker.getExchList())
            {
                ValueObject<Exchange> vo = new ValueObject<Exchange>();
                vo.Name = e.Name;
                vo.Value = e;
                list.Add(vo);
            }
            ck.DataSource = list;
            ck.DisplayMember = "Name";
            ck.ValueMember = "Value";
        }


        public static void genTickList(ref ComboBox cb)
        {
            decimal[] ticklist = { (decimal)0.0000001, (decimal)0.000001, (decimal)0.00001,(decimal)000025,(decimal)0.00005,(decimal)0.0001,(decimal)0.00025,
                                     (decimal)0.0005,(decimal)0.001,(decimal)0.00125,(decimal)0.0020,(decimal)0.0025,(decimal)0.005,(decimal)0.01,(decimal)0.0125,
                                 (decimal)0.020,(decimal)0.025,(decimal)0.05,(decimal)0.1,(decimal)0.125,(decimal)0.20,(decimal)0.25,(decimal)0.5,(decimal)1,
                                 (decimal)5,(decimal)10,(decimal)25,(decimal)100,(decimal)1000,(decimal)10000,(decimal)100000};
            ArrayList list = new ArrayList();
            foreach (decimal t in ticklist)
            {
                ValueObject<decimal> vo = new ValueObject<decimal>();
                vo.Name = t.ToString();
                vo.Value = t;
                list.Add(vo);
            }
            cb.DataSource = list;
            cb.DisplayMember = "Name";
            cb.ValueMember = "Value";
        }

        private static ValueObject<int> anyItem()
        {
            ValueObject<int> a = new ValueObject<int>();
            a.Name = "<Any>";
            a.Value = -1;
            return a;
        }
    }

    //下拉菜单所使用的vlaueobject,用于形成名称与对应的value
    
    public class ValueObject<T>
    {
        
        private string _name;
        private T _value;
        public T Value { get { return _value; } set { _value = value; } }
        public string Name { get { return _name; } set { _name = value; } }

    }
}
