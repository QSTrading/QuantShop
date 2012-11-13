using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using WeifenLuo.WinFormsUI.Docking;
using TradingLib;
using TradeLink.API;

namespace TradingLib.GUI
{
    public partial class fmSecurityEdit : DockContent
    {
        List<Security> futlist = null;
        public fmSecurityEdit()
        {
            InitializeComponent();

            futlist = LibUtil.LoadFutFromXML();
            initSecGrid();
        }

        void initSecGrid()
        {
            futlist = LibUtil.LoadFutFromXML();
            //foreach ( fut in futlist)
            {
                //secGrid.Rows.Add(fut.Symbol,fut.FullName,fut.DestEx,fut.SecFamily,fut.PriceTick,fut.Multiple,fut.MarginRatio);
            }
        }
    }
}
