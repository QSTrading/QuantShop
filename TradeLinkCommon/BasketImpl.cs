using System;
using System.Collections.Generic;
using System.Collections;
using TradeLink.API;
using System.IO;

namespace TradeLink.Common
{
    /// <summary>
    /// Holds collections of securities.
    /// </summary>
    [Serializable]
    public class BasketImpl : TradeLink.API.Basket
    {
        /// <summary>
        /// gets symbols removed from new list of symbols, given original list
        /// </summary>
        /// <param name="old"></param>
        /// <param name="newb"></param>
        /// <returns></returns>
        public static Basket Subtract(string[] old, string[] newb)
        {
            return Subtract(new BasketImpl(old), new BasketImpl(newb));
        }
        /// <summary>
        /// gets symbols removed from newbasket, given original basket
        /// </summary>
        /// <param name="old"></param>
        /// <param name="newb"></param>
        /// <returns></returns>
        public static Basket Subtract(Basket old, Basket newb)
        {
            if (old.Count == 0) return new BasketImpl();
            Basket rem = new BasketImpl();
            foreach (Security sec in old)
            {
                if (!newb.ToString().Contains(sec.Symbol))
                    rem.Add(sec);
            }
            return rem;
        }

        public static string[] TrimSymbols(string[] syms)
        {
            List<string> trimmed = new List<string>();
            for (int i = 0; i < syms.Length; i++)
            {
                syms[i] = syms[i].TrimStart(' ', ',');
                syms[i] = syms[i].TrimEnd(' ', ',');
                // ensure we still have a symbol
                if (string.IsNullOrWhiteSpace(syms[i]))
                    continue;
                trimmed.Add(syms[i]);
            }
            return trimmed.ToArray();
        }

        /// <summary>
        /// Create a basket of securities
        /// </summary>
        /// <param name="onesymbol">first symbol</param>
        public BasketImpl(string onesymbol) : this(new string[] { onesymbol }) { }
        /// <summary>
        /// Create a basket of securities
        /// </summary>
        /// <param name="symbolist">symbols</param>
        public BasketImpl(string[] symbolist)
        {
            foreach (string s in symbolist)
                Add(new SecurityImpl(s));
        }
        /// <summary>
        /// clone a basket
        /// </summary>
        /// <param name="copy"></param>
        public BasketImpl(Basket copy)
        {
            foreach (Security s in copy)
                Add(new SecurityImpl(s));
            Name = copy.Name;
        }
        /// <summary>
        /// Create a basket of securities
        /// </summary>
        /// <param name="firstsec">security</param>
        public BasketImpl(SecurityImpl firstsec)
        {
            Add(firstsec);
        }
        /// <summary>
        /// Create a basket of securities
        /// </summary>
        /// <param name="securities"></param>
        public BasketImpl(SecurityImpl[] securities)
        {
            foreach (SecurityImpl s in securities)
                Add(s);
        }
        public BasketImpl() { }
        public Security this [int index] { get { return symbols[index]; } set { symbols[index] = value; } }
        public Security this[string sym] { 
            get {
            foreach (Security s in symbols)
                if (s.Symbol == sym)
                    return s;
            return null;           
                 }
            set {
            }
        
        }
        List<Security> symbols = new List<Security>();//����security���б�
        string _name = "";
        public string Name { get { return _name; } set { _name = value; } }
        public int Count { get { return symbols.Count; } }
        public bool hasStock { get { return symbols.Count >0; } }
        /// <summary>
        /// adds a security if not already present
        /// </summary>
        /// <param name="sym"></param>
        public void Add(string sym) { Add(new SecurityImpl(sym)); }
        /// <summary>
        /// adds a security if not already present
        /// </summary>
        /// <param name="s"></param>
        public void Add(Security s) { if (s.isValid && !contains(s)) symbols.Add(s); }
        bool contains(string sym) { foreach (Security s in symbols) if (s.Symbol == sym) return true; return false; }
        bool contains(Security sec) { return symbols.Contains(sec); }

        /// <summary>
        /// ���basket���Ƿ���ĳ���ض���symbol
        /// </summary>
        /// <param name="s"></param>
        public bool HaveSymbol(string sym) { return contains(sym); }
        /// <summary>
        /// adds contents of another basket to this one.
        /// will not result in duplicate symbols
        /// </summary>
        /// <param name="mb"></param>
        public void Add(Basket mb)
        {
            for (int i = 0; i < mb.Count; i++)
                this.Add(mb[i]);
        }
        public void Add(string[] syms)
        {
            for (int i = 0; i < syms.Length; i++)
                this.Add(syms[i]);
        }
        /// <summary>
        /// removes all elements of baskets that match.
        /// unmatching elements are ignored
        /// </summary>
        /// <param name="mb"></param>
        public void Remove(Basket mb)
        {
            List<int> remove = new List<int>();
            for (int i = 0; i < symbols.Count; i++)
                for (int j = 0; j < mb.Count; j++)
                    if (symbols[i].Symbol == mb[j].Symbol)
                        remove.Add(i);
            for (int i = remove.Count - 1; i >= 0; i--)
                symbols.RemoveAt(remove[i]);
        }
        /// <summary>
        /// remove single symbol from basket
        /// </summary>
        /// <param name="symbol"></param>
        public void Remove(string symbol) { int i = -1; for (int j = 0; j < symbols.Count; j++) if (symbols[j].Symbol == symbol) i = j; if (i != -1) symbols.RemoveAt(i); }
        /// <summary>
        /// remove index of a particular symbol
        /// </summary>
        /// <param name="i"></param>
        public void Remove(int i) { symbols.RemoveAt(i); }
        /// <summary>
        /// remove security from basket
        /// </summary>
        /// <param name="s"></param>
        public void Remove(Security s) { symbols.Remove(s); }
        /// <summary>
        /// empty basket
        /// </summary>
        public void Clear() { symbols.Clear(); }
        //���л�basket
        public static string Serialize(Basket b)
        {
            List<string> s = new List<string>();
            for (int i = 0; i < b.Count; i++) s.Add(b[i].FullName);
            return string.Join(",", s.ToArray());
        }
        //�����л�basket
        public static BasketImpl Deserialize(string serialBasket)
        {
            BasketImpl mb = new BasketImpl();
            if ((serialBasket == null) || (serialBasket == "")) return mb;
            string[] r = serialBasket.Split(',');
            for (int i = 0; i < r.Length; i++)
            {
                if (r[i] == "") continue;
                SecurityImpl sec = SecurityImpl.Parse(r[i]);
                if (sec.isValid)
                    mb.Add(sec);
            }
            return mb;
        }
        public static Basket FromFile(string filename)
        {
            try
            {
                StreamReader sr = new StreamReader(filename);
                string file = sr.ReadToEnd();
                sr.Close();
                string[] syms = file.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                BasketImpl b = new BasketImpl(syms);
                b.Name = Path.GetFileNameWithoutExtension(filename);
                return b;
            }
            catch { }
            return new BasketImpl();
        }

        public static void ToFile(Basket b, string filename) { ToFile(b, filename, false); }
        public static void ToFile(Basket b, string filename, bool append)
        {
            StreamWriter sw = new StreamWriter(filename, append);
            for (int i = 0; i < b.Count; i++)
                sw.WriteLine(b[i].Symbol);
            sw.Close();
        }
        public override string ToString() { return Serialize(this); }
        public static BasketImpl FromString(string serialbasket) { return Deserialize(serialbasket); }
        public IEnumerator GetEnumerator() { foreach (SecurityImpl s in symbols) yield return s; }

        //���Security����
        public Security[] ToArray()
        {
            return symbols.ToArray();
        }
        //���symbol����
        public string[] ToSymArray()
        {
            string[] syms = new string[symbols.Count];
            for (int i = 0; i < syms.Length; i++)
                syms[i] = symbols[i].Symbol;
            return syms;
        }

    }
}
