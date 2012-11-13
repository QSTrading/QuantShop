using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradeLink.API;
using Easychart.Finance;
using Easychart.Finance.Win;
using Easychart.Finance.DataProvider;
using System.IO;
using TradingLib.Data;

namespace TradingLib.GUI
{
    public partial class ctTimesLineChart : UserControl, GotTickIndicator
    {
        public event DebugDelegate SendDebugEvent;
        private void debug(string msg)
        {
            if (SendDebugEvent != null)
                SendDebugEvent(msg);
        }
        private string _defalutlsymbol;

        //相应数据tick用于更新绘图界面
        public void GotTick(Tick k)
        {
            if (k.isTrade && k.symbol == _defalutlsymbol)
                chartWinControl1.NeedRefresh();
        }

        //向绘图控件绑定数据
        public void setDataSource(object o,string symbol)
        {
            _defalutlsymbol = symbol;
            chartWinControl1.DataManager = o as DataManagerBase;
            chartWinControl1.Symbol = symbol;
            chartWinControl1.NeedRebind();
            
        }
        public ctTimesLineChart()
        {
            PluginManager.Load(Environment.CurrentDirectory + "\\Plugins\\");
            PluginManager.OnPluginChanged += new FileSystemEventHandler(OnPluginChange);
            InitializeComponent();
            //PluginManager.Load(FormulaHelper.Root + @"Plugins\");

            //InitializeComponent();
            //从本地k线文件加载数据源
            //LoadCSVFile("MSFT");
            //KeyMessageFilter.AddMessageFilter(chartWinControl1);
            //this.Load += new EventHandler(control_Load);
        }
        /*
        void control_Load(object sender, EventArgs e)
        {
            LoadCSVFile(Environment.CurrentDirectory + "\\MSFT.CSV");
            chartWinControl1.ContextMenu = null;
            chartWinControl1.Focus();
        }
         * */


        private void OnPluginChange(object source, FileSystemEventArgs e)
        {
            chartWinControl1.NeedRefresh();
        }


        /*

        /// <summary>
        /// Load CSV data from file
        /// </summary>
        /// <param name="FileName"></param>
        private void LoadCSVFile(string FileName)
        {
            DataManagerBase dmb = new YahooCSVDataManager(Path.GetDirectoryName(FileName), Path.GetExtension(FileName));
            chartWinControl1.DataManager = dmb;
            chartWinControl1.Symbol = Path.GetFileNameWithoutExtension(FileName);
            chartWinControl1.EndTime = DateTime.MinValue;
        }
         * */


    }
}
