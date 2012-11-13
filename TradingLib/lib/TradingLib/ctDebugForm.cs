using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradeLink.AppKit;
using WeifenLuo.WinFormsUI.Docking;

namespace TradingLib
{
    public partial class ctDebugForm : DockContent
    {
        public ctDebugForm()
        {
            InitializeComponent();
        }

        //public DebugControl outputWindow

        public void GotDebug(TradeLink.API.Debug deb)
        {
            //debugOutPut
            debugControl1.GotDebug(deb);
        }

        public void GotDebug(string msg)
        {
            debugControl1.GotDebug(msg);
        }
    }
}
