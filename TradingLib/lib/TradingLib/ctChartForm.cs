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
namespace TradingLib
{
    public partial class ctChartForm : DockContent
    {
        private ObjectManager Manager=null;
        private PropertyGrid pg = null;



        public ctChartForm()
        {
            PluginManager.Load(FormulaHelper.Root + @"Plugins\");

            InitializeComponent();
            //objectToolPanel1.LoadObjectTool();
            pg = new PropertyGrid();
            

            this.Load+=new EventHandler(Form1_Load);
           
        }

        private void LoadCSVFile(string Symbol)
        {
            DataManagerBase base2 = new YahooCSVDataManager(Environment.CurrentDirectory, "CSV");
            this.Designer.DataManager = base2;
            this.Designer.Symbol = Symbol;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.LoadCSVFile("MSFT");
            this.Manager = new ObjectManager(this.Designer, this.pg, this.ToolPanel);
            //this.Manager = new ObjectManager();
            this.Manager.AfterCreateStart += new ObjectEventHandler(this.Manager_AfterCreateStart);
            this.Designer.ScaleType = ScaleType.Log;
            KeyMessageFilter.AddMessageFilter(this.Designer);
            Designer.MouseDown += new MouseEventHandler(Designer_MouseDown);
            Designer.NativePaint += new NativePaintHandler(Designer_NativePaint);

            ToolPanel.AfterButtonCreated += new ObjectButtonEventHandler(ToolPanel_AfterButtonCreated);
        
        }

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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Designer.Designing = true;
            this.Manager.ObjectType = new ObjectInit("Fill Ellpise", typeof(CircleObject), "Fill");

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
            Manager.ObjectType = new ObjectInit("HLine",typeof(SingleLineObject), "Horizontal");

        }

        private void vLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Designer.Designing = true;
            Manager.ObjectType = new ObjectInit("VLine", typeof(SingleLineObject), "Vertical");

        }
        #endregion

        private void toolStripAdjustSize_Click(object sender, EventArgs e)
        {
            Designer.Reset(5);
        }







    }
}
