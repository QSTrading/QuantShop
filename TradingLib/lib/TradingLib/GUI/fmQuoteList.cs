using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using TradeLink.Common;
using TradeLink.API;
using TradeLink.AppKit;
using TradingLib;
using TradingLib.Data;
using TradingLib.GDIControl;
namespace TradingLib.GUI
{
    public partial class fmQuoteList : DockContent
    {
        List<Security> futlist = null;
        public event DebugDelegate SendDebugEvent;
        //缓存symbol代码到对应的QuoteView的映射,用于加快tick的处理与现实速度
        Dictionary<string, ViewQuoteList> symViewMap = new Dictionary<string, ViewQuoteList>();

        List<string> shfe_list = new List<string>();
        List<string> dce_list = new List<string>();
        List<string> czce_list = new List<string>();
        List<string> cffex_list = new List<string>();
        List<string> _currentlist;
        ViewQuoteList _currentview;

        Dictionary<string, ViewQuoteList> shfe_symViewMap = new Dictionary<string, ViewQuoteList>();
        Dictionary<string, ViewQuoteList> dce_symViewMap = new Dictionary<string, ViewQuoteList>();
        Dictionary<string, ViewQuoteList> czce_symViewMap = new Dictionary<string, ViewQuoteList>();
        Dictionary<string, ViewQuoteList> cffex_symViewMap = new Dictionary<string, ViewQuoteList>();

        Basket mb_SHFE = null;
        Basket mb_CFFEX = null;
        Basket mb_DCE = null;
        Basket mb_CZCE = null;
        Basket mb_all = null;

        public event BasketDel SendRegisterBasket;

        void debug(string msg)
        {
            if (SendDebugEvent != null)
                SendDebugEvent(msg);
        }
        public fmQuoteList()
        {
            InitializeComponent();
            initGrids();
            
        }

        //初始化QuoteList窗体的QuoteView
        void initGrids()
        {
            //1.加载所有的future symbol代码列表
            //futlist = LibUtil.LoadFutFromXML();
            //2.遍历所有的代码列表将他们按照交易所归类到相应的basket中
            mb_SHFE = BasketTracker.getBasket("SHFE");
            mb_DCE = BasketTracker.getBasket("DCE");
            mb_CZCE = BasketTracker.getBasket("CZCE");
            mb_CFFEX = BasketTracker.getBasket("CFFEX");
            mb_all = BasketTracker.getBasket();
            foreach (Security sec in mb_all.ToArray())
            {
                switch (sec.DestEx)
                {
                    case "CN_SHFE":
                        symViewMap.Add(sec.Symbol, quote_shfe);
                        shfe_list.Add(sec.Symbol);
                        break;
                    case "CN_DCE":
                        symViewMap.Add(sec.Symbol, quote_dce);
                        dce_list.Add(sec.Symbol);
                        break;
                    case "CN_CFFEX":
                        symViewMap.Add(sec.Symbol, quote_cffex);
                        cffex_list.Add(sec.Symbol);
                        break;
                    case "CN_CZCE":
                        symViewMap.Add(sec.Symbol, quote_czce);
                        czce_list.Add(sec.Symbol);
                        break;
                    default:
                        break;
                }
            
            }
            //初始化显示页面
            _currentlist = shfe_list;
            _currentview = quote_shfe;
            
            /*
            foreach (Security f in futlist)
            {
                mb_all.Add(f);
                if (f.DestEx == "SHFE")
                {
                    mb_SHFE.Add(f);
                    symViewMap.Add(f.Symbol, quote_shfe);
                }
                if (f.DestEx == "CFFEX")
                {
                    mb_CFFEX.Add(f);
                    symViewMap.Add(f.Symbol, quote_cffex);
                }
                if (f.DestEx == "DCE")
                {
                    mb_DCE.Add(f);
                    symViewMap.Add(f.Symbol, quote_dce);
                }
                if (f.DestEx == "CZCE")
                {
                    mb_CZCE.Add(f);
                    symViewMap.Add(f.Symbol, quote_czce);
                }

            }
             * */
            //3.将每个交易所的合约列表加载到对应的QuoteView控件中去
            quote_shfe.SetBasket(mb_SHFE);
            quote_cffex.SetBasket(mb_CFFEX);
            quote_dce.SetBasket(mb_DCE);
            quote_czce.SetBasket(mb_CZCE);
        }
        //注册订阅symbol数据
        public void RegisterSymbols()
        {
            RegisterSymbols(mb_all);

            //RegisterSymbols(mb_DCE);
            //RegisterSymbols(mb_CZCE);
            //RegisterSymbols(mb_DCE);
            //RegisterSymbols(mb_SHFE);
            //RegisterSymbols(mb_CFFEX);
        }

        //注册订阅symbol数据
        void RegisterSymbols(Basket b)
        {
            if (SendRegisterBasket != null)
            {
                SendRegisterBasket(b);
            }
        }
        //当有新的tick数据过来时,我们需要调用该函数用于更新QuoteList界面的Symbol数据
        public void GotTick(Tick t)
        {
            //只有当前页面显示的证券我们才进行更新 这样可以节省资源
            if(_currentlist.Contains(t.symbol))
                _currentview.GotTick(t);
            /*
            if (symViewMap.ContainsKey(t.symbol))
                symViewMap[t.symbol].GotTick(t);

            if()
             * */
        }

        private void fmQuoteList_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
        //单击选项卡 切换显示页面
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lock (_currentlist)
            {
                switch (tabControl1.SelectedIndex)
                {
                    case 0:
                        _currentlist = shfe_list;
                        _currentview = quote_shfe;
                        debug("选择:上海期货交易所页面");
                        break;
                    case 1:
                        _currentlist = dce_list;
                        _currentview = quote_dce;
                        debug("选择:大连期货交易所页面");
                        break;
                    case 2:
                        _currentlist = czce_list;
                        _currentview = quote_czce;
                        debug("选择:郑州期货交易所页面");
                        break;
                    case 3:
                        _currentlist = cffex_list;
                        _currentview = quote_cffex;
                        debug("选择:上海金融期货交易所页面");
                        break;
                }
            }
            
        }

    }
}
