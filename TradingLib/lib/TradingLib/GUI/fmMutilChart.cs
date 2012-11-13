using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
namespace TradingLib.GUI
{
    public partial class fmMutilChart : DockContent
    {
        public fmMutilChart()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            /*
            fmChartForm fm = new fmChartForm();
            fm.Show(dockPanel, DockState.Document);
             * */

            demo fm = new demo();
            fm.Show(dockPanel, DockState.Document);
        }
    }
}
