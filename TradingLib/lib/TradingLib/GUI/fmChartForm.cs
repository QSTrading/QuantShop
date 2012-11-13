using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Easychart.Finance;
using Easychart.Finance.Win;
using Easychart.Finance.Objects;
using Easychart.Finance.DataProvider;
using WeifenLuo.WinFormsUI.Docking;
using TradeLink.API;
using TradingLib.API;
using TradeLink.Common;
using TradingLib.Data;


namespace TradingLib.GUI
{
    //k线图窗体,用来绘制K线
    //问题:窗体用到了外界传递进来的数据组件，数据组件会自动更新数据，当窗体初始化与数据更新同时调用某个函数的时候，会产生错误。
    //应该需要加锁来解决
    //GotTickIndicator 用于相应Tick进行价格更新
    public partial class fmChartForm : DockContent,GotTickIndicator
    {
        public event DebugDelegate SendDebugEvent;//发送日志信息
        public event StringParamDelegate SendHistDataRequest;
        
        //ChartControl Designer
        //ObjectToolPanel ToolPanel
        private ObjectManager Manager = null;
        private PropertyGrid pg = null;
        private Basket defaultSymbols = null;

        //private bool _loaded = false;
        private Security _sec;
        private QSMemoryDataManager _mdm;
        private bool _verb = true;
        private bool VerboseDebugging { get { return _verb; } set { _verb = value; } }
        
        public fmChartForm()
        {
            // 加载plugins
            PluginManager.Load(FormulaHelper.Root + @"Plugins\");

            InitializeComponent();
            //从本地k线文件加载数据源
            LoadCSVFile("MSFT");
            this.Load += new EventHandler(Form_Load);

        }

        private void v(string s)
        {
            if (_verb)
                debug(s);

        }
        private void debug(string s)
        {
            if (SendDebugEvent != null)
                SendDebugEvent(s);
        }
        //统一ChartForm加载方式,传递图表显示所需数据
        //窗体加载时提供参数:数据源,默认symbol,默认Symbol List
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mdm"></param>
        /// <param name="symbol"></param>
        /// 
        //public fmChartForm(MemoryDataManager mdm, Security sec) : this(mdm, new BasketImpl(sec as SecurityImpl), sec) { }
        private QSFileDataManager _fileDM;

        public fmChartForm(Basket ds, Security sec)
        {
            //设定默认security

            //_mdm = mdm;
            _sec = sec;
            defaultSymbols = ds;

           // _fileDM = new MStockDataManager("d:\\data\\");
            //_bltdm = new BLTDataManager(_blt);
            //_bltdm.SendDebugEvent += new DebugDelegate(debug);
            //_mdm = new MemoryDataManager(_fileDM);
            // 加载plugins
            PluginManager.Load(FormulaHelper.Root + @"Plugins\");
            InitializeComponent();
            UIUtil.genComboBoxHistory(ref toolStripComboBoxHistoryDate);
            ctTimeSales1.SetSeurity(_sec);
            LoadSymbolMenu();
            this.Load += new EventHandler(Form_Load);
            //this.Shown += new EventHandler(fmChartForm_Shown);
            //PluginManager.OnPluginChanged += new FileSystemEventHandler(OnPluginChange);
        }

        private void Form_Load(object sender, EventArgs e)
        {
            //Designer:ChartControl pg:PropertyGrid ToolPanel:ObjectToolPanel
            pg = new PropertyGrid();
            Manager = new ObjectManager(this.Designer, this.pg, this.ToolPanel);
            Manager.AfterCreateStart += new ObjectEventHandler(this.Manager_AfterCreateStart);
            //Designer.ScaleType = ScaleType.Log;
            KeyMessageFilter.AddMessageFilter(this.Designer);
            Designer.MouseDown += new MouseEventHandler(Designer_MouseDown);
            Designer.NativePaint += new NativePaintHandler(Designer_NativePaint);

            ToolPanel.AfterButtonCreated += new ObjectButtonEventHandler(ToolPanel_AfterButtonCreated);
            ToolPanel.Visible = false;

            Designer.StatisticWindow.Visible = false;
            ctTimeSales1.Type = TimeSalesType.ChartRight;
            
            LoadDataManager(_mdm, _sec.Symbol, DataCycle.Minute, 200);
            Text = _sec.Symbol;
        }
        public fmChartForm(QSMemoryDataManager mdm, Basket ds, Security sec)
        {
            //设定默认security
            
            _mdm = mdm;
            _sec = sec;
            defaultSymbols = ds;
            //_fileDM = new MStockDataManager("d:\\data\\");
            //_bltdm = new BLTDataManager(_blt);
            //_bltdm.SendDebugEvent += new DebugDelegate(debug);
            //_mdm = new MemoryDataManager(_fileDM);
            // 加载plugins
            PluginManager.Load(FormulaHelper.Root + @"Plugins\");
            InitializeComponent();
            UIUtil.genComboBoxHistory(ref toolStripComboBoxHistoryDate);
            ctTimeSales1.SetSeurity(_sec);
            LoadSymbolMenu();
            //设定初始化参数,从内存MemeryDataManager加载数据源
            this.Load += new EventHandler(Form_Load);
            //this.Shown += new EventHandler(fmChartForm_Shown);
            //PluginManager.OnPluginChanged += new FileSystemEventHandler(OnPluginChange);
            
        }
        /*
        void fmChartForm_Shown(object sender, EventArgs e)
        {
            debug("!!!!!!!!!!!!!!!it is showing ");
        }*/


        //当有新的tick数据收到时,我们需要更新Chart.具体数据有Barlisttracker提供
        //得到Tick数据我们就向服务器发送补充数据的请求
        public void GotTick(Tick k)
        {
            //debug("refesh chart:" + k.trade.ToString());
            //当tick属于chart显示的symbol并且是trade我们才进行更新,如果所有tick均进行刷新 会造成CPU资源浪费
            if (k.symbol == Designer.Symbol && k.isTrade)
            {
                ctTimeSales1.GotTick(k);
                Designer.NeedRefresh();
                //debug("实时数据:" + _mdm.StreamCount(_sec.Symbol));
            } 
        }
        //加载symbol列表
        private void LoadSymbolMenu()
        {
            tbnSymbolsMenu.DropDownItems.Clear();
            if (defaultSymbols == null || defaultSymbols.Count == 0) return;
            foreach (Security sec in defaultSymbols.ToArray())
            {
                tbnSymbolsMenu.DropDownItems.Add(sec.Symbol, null, new EventHandler(SelectSymbol));
            }

        }
        private void SelectSymbol(object sender, EventArgs e)
        {
            
            ToolStripDropDownItem m = sender as ToolStripDropDownItem;
            Text = m.Text;
            //MessageBox.Show(m.Text);
            Designer.Symbol = m.Text;
            tbnSymbolsMenu.Text = m.Text;
            Designer.NeedRebind();
            toolStripAdjustSize_Click(null, null);//调整Chart默认大小

        }
        //ChartContorl加载数据源
        private void LoadDataManager(DataManagerBase dmb, string DefaultSymbol, DataCycle DefaultCycle, int DefaultBars)
        {
            //为加载数据源提供默认参数
            if (DefaultSymbol == null) DefaultSymbol = "IF999";
            if (DefaultBars == 0) DefaultBars = 200;
            if (DefaultCycle == null) DefaultCycle = DataCycle.Minute;

            if (DefaultSymbol != "")
            {
                Designer.DataManager = dmb;
                Designer.Symbol = DefaultSymbol;
                tbnSymbolsMenu.Text = DefaultSymbol;
                Designer.StockBars = DefaultBars;
                Designer.CurrentDataCycle = DefaultCycle;
                //ChartControl.
            }

        }
        //从本地文件加载数据源
        private void LoadCSVFile(string Symbol)
        {
            DataManagerBase base2 = new YahooCSVDataManager(Environment.CurrentDirectory, "CSV");
            this.Designer.DataManager = base2;
            this.Designer.Symbol = Symbol;
        }

        

        #region 控件回调函数
        private void Manager_AfterCreateStart(object sender, BaseObject Object)
        {
            if (Object is FillPolygonObject)
            {
                (Object as FillPolygonObject).Brush = new BrushMapper(Color.Red);
                (Object as FillPolygonObject).Brush.Alpha = 10;
            }
            else if (Object is FibonacciLineObject)
            {
                FibonacciLineObject obj2 = Object as FibonacciLineObject;
                obj2.ObjectFont.Alignment = 0;
                if (obj2.ObjectType.InitMethod == "InitPercent")
                {
                    obj2.Split = new float[] { 0f, 0.33f, 0.5f, 0.66f, 1f };
                }
            }
            else if ((!this.Manager.InPlaceTextEdit && (Object is LabelObject)) && (Object.ObjectType.InitMethod == "InitLabel"))
            {
                string str = InputBox.ShowInputBox("Input the label", "Label");
                if (str != "")
                {
                    (Object as LabelObject).Text = str;
                    this.Manager.PostMouseUp();
                }
                else
                {
                    this.Manager.CancelCreate(Object);
                }
            }
        }

        private void ToolPanel_AfterButtonCreated(object sender, ToolBarButton tbb)
        {
        }

        private void Designer_MouseDown(object sender, MouseEventArgs e)
        {
            //this.label1.Text = "hh";
            /*
            if (e.Button == 0)
            {
                BaseObject objectAt = this.Manager.GetObjectAt(e.X, e.Y);
                if (objectAt != null)
                {
                    this.Manager.SelectedObject = objectAt;
                }
            }
             * */
        }

        private void Designer_NativePaint(object sender, NativePaintArgs e)
        {
        }

        #endregion

        #region 调整时间频率

        private void mINToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Designer.CurrentDataCycle = new DataCycle(DataCycleBase.MINUTE, 1);
            toolStripDropDownTimeFrame.Text = "1分钟";
        }

        private void mINToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Designer.CurrentDataCycle = new DataCycle(DataCycleBase.MINUTE, 3);
            toolStripDropDownTimeFrame.Text = "3分钟";
        }

        private void mINToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Designer.CurrentDataCycle = new DataCycle(DataCycleBase.MINUTE, 5);
            toolStripDropDownTimeFrame.Text = "5分钟";

        }

        private void mINToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Designer.CurrentDataCycle = new DataCycle(DataCycleBase.MINUTE, 15);
            toolStripDropDownTimeFrame.Text = "15分钟";
        }

        private void mINToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Designer.CurrentDataCycle = new DataCycle(DataCycleBase.MINUTE, 30);
            toolStripDropDownTimeFrame.Text = "30分钟";
        }

        private void hOURToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Designer.CurrentDataCycle = new DataCycle(DataCycleBase.HOUR, 1);
            toolStripDropDownTimeFrame.Text = "1小时";
        }

        private void dAYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Designer.CurrentDataCycle = new DataCycle(DataCycleBase.DAY, 1);
            toolStripDropDownTimeFrame.Text = "1天";
        }

        private void wEEKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Designer.CurrentDataCycle = new DataCycle(DataCycleBase.WEEK, 1);
            toolStripDropDownTimeFrame.Text = "1周";

        }
        private void mONTHToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Designer.CurrentDataCycle = new DataCycle(DataCycleBase.MONTH, 1);
            toolStripDropDownTimeFrame.Text = "1月";

        }

        private void yEARToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Designer.CurrentDataCycle = new DataCycle(DataCycleBase.YEAR, 1);
            toolStripDropDownTimeFrame.Text = "1年";

        }
        #endregion


        #region 绘图菜单
        //打开盘口数据
        private void toolStripButtonTimeSales_Click(object sender, EventArgs e)
        {
            if (ctTimeSales1.Visible == true)
            {
                ctTimeSales1.Visible = false;
                Designer.Width = Designer.Width + ctTimeSales1.Width;
            }
            else
            {
                Designer.Width = Designer.Width - ctTimeSales1.Width;
                ctTimeSales1.Visible = true;

            }
        }
        //打开统计窗口
        private void toolStripButtonStatisWin_Click(object sender, EventArgs e)
        {
            Designer.StatisticWindow.Visible = !Designer.StatisticWindow.Visible;
        }


        //刷新历史数据
        private void tsbRequestHistData_Click(object sender, EventArgs e)
        {
            try
            {
                v("ChartForm更新历史数据");
                int startdate = ((ValueObject<int>)toolStripComboBoxHistoryDate.SelectedItem).Value;
                //v("Start date:"+startdate.ToString());
                if(SendHistDataRequest != null)
                    SendHistDataRequest(BarImpl.BuildBarRequest(_sec.Symbol,BarInterval.Minute,startdate));
            }
            catch (Exception ex)
            { }

        }

        private void toolStripDraw_Click(object sender, EventArgs e)
        {
            //debug("xxx");
            ToolPanel.Visible = !ToolPanel.Visible;
        }

        private void lineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Designer.Designing = true;
            Manager.ObjectType = new ObjectInit("Line", typeof(LineObject), "InitLine");

        }

        private void rayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Designer.Designing = true;
            Manager.ObjectType = new ObjectInit("Line", typeof(LineObject), "InitLine3");
        }

        private void eToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Designer.Designing = true;
            Manager.ObjectType = new ObjectInit("Line", typeof(LineObject), "InitLine");

        }

        private void hLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Designer.Designing = true;
            Manager.ObjectType = new ObjectInit("HLine", typeof(SingleLineObject), "Horizontal");

        }

        private void vLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Designer.Designing = true;
            Manager.ObjectType = new ObjectInit("VLine", typeof(SingleLineObject), "Vertical");

        }
        #endregion

        //调整Chart为默认大小
        private void toolStripAdjustSize_Click(object sender, EventArgs e)
        {
            Designer.Reset(7);
        }
        /*
        #region 定义热键
        private void ctChartForm_Activated(object sender, EventArgs e)
        {
            //注册热键Shift+Q 显示绘图工具栏
            HotKey.RegisterHotKey(Handle, 100, HotKey.KeyModifiers.Shift, Keys.Q);
            //注册热键Shift+A 重置图表大小
            HotKey.RegisterHotKey(Handle, 101, HotKey.KeyModifiers.Shift, Keys.A);
            //注册热键Ctrl+B，Id号为101。HotKey.KeyModifiers.Ctrl也可以直接使用数字2来表示。

            //注册热键Alt+D，Id号为102。HotKey.KeyModifiers.Alt也可以直接使用数字1来表示。
            HotKey.RegisterHotKey(Handle, 102, HotKey.KeyModifiers.Alt, Keys.D);
        }

        private void ctChartForm_Leave(object sender, EventArgs e)
        {
            //注销Id号为100的热键设定
            HotKey.UnregisterHotKey(Handle, 100);
            //注销Id号为101的热键设定
            HotKey.UnregisterHotKey(Handle, 101);
            //注销Id号为102的热键设定
            HotKey.UnregisterHotKey(Handle, 102);
        }

        /// 
        /// 监视Windows消息
        /// 重载WndProc方法，用于实现热键响应
        /// 
        /// 
        //Shift+Q:显示划线工具栏
        //Shift+A:重置图表大小
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            const int WM_HOTKEY = 0x0312;
            //按快捷键 
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        case 100:    //按下的是Shift+Q
                            //此处填写快捷键响应代码       
                            ToolPanel.Visible = !ToolPanel.Visible;
                            break;
                        case 101:    //按下的是Ctrl+A
                            //此处填写快捷键响应代码
                            Designer.Reset(5);
                            break;
                        case 102:    //按下的是Alt+D
                            //此处填写快捷键响应代码
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }
        #endregion
        **/

        int _keynum = 0;//解决keydown触发2次的问题
        private void Designer_KeyDown(object sender, KeyEventArgs e)
        {
            //debug("key is down:" + e.KeyCode.ToString());
           
            switch (e.KeyCode)
            { 
                case Keys.F5:
                    if (_keynum == 1)
                    {
                        _keynum = 0;
                        return;
                    }
                    _keynum = 1;
                    if (this.Designer.StockRenderType == StockRenderType.Default)
                    {
                        //debug("it is F5 we open times lines");
                        this.Designer.StockRenderType = StockRenderType.Line;

                    }
                    else
                    {
                        //debug("it is F5 we open ohlc");
                        this.Designer.StockRenderType = StockRenderType.Default;
                    }
                    this.Designer.NeedRebind();
                    break;
                default:
                    break;
            
            }
        }



       

        

        

       






    }
}
