﻿using System;
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

namespace TradingLib
{
    public partial class ctSecurityEditForm : DockContent
    {
        List<FutureSec> futlist = null;
        public ctSecurityEditForm()
        {
            InitializeComponent();

            futlist = LibUtil.LoadFutFromXML();
            initSecGrid();
        }

        void initSecGrid()
        {
            futlist = LibUtil.LoadFutFromXML();
            foreach(FutureSec fut in futlist)
            {
                secGrid.Rows.Add(fut.Symbol,fut.FullName,fut.DestEx,fut.SecFamily,fut.PriceTick,fut.Multiple,fut.MarginRatio);
            }
        }
    }
}