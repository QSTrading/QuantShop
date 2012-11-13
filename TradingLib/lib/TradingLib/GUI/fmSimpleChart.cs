using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradeLink.API;
using WeifenLuo.WinFormsUI.Docking;

namespace TradingLib.GUI
{
    public partial class fmSimpleChart : DockContent, GotTickIndicator
    {
        public fmSimpleChart()
        {
            InitializeComponent();
        }


        public void GotTick(Tick k)
        { 
        
        }
    }

}
