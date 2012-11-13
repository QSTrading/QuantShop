using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using TradeLink.Common;
using TradeLink.API;
using TradeLink.AppKit;


namespace TradingLib.GUI
{
    public partial class TradeView : UserControl
    {
        //tradeView 用于显示账户的成交记录
        string _dispdecpointformat = "N" + ((int)2).ToString();

        public TradeView()
        {
            InitializeComponent();
        }

        public void GotFill(Trade t)
        {
            if (InvokeRequired)
                Invoke(new FillDelegate(GotFill), new object[] { t });
            else
            {
                tradeGrid.Rows.Add(t.id, t.symbol, (t.side ? "买" : "卖"), t.xsize, t.xprice.ToString(_dispdecpointformat), t.xdate, t.Account); // if we accept trade, add it to list

            }
        }
    }
}
