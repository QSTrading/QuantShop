using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradeLink.API;
using TradingLib;

namespace Responses
{
    public partial class ctQuotePanel : UserControl, GotTickIndicator
    {
        private Security _sec;

        private string _dispdecpointformat = "{0:F1}";
        public ctQuotePanel()
        {
            InitializeComponent();
            openinterest.ForeColor = Color.Yellow;
            size.ForeColor = Color.Yellow;
            volume.ForeColor = Color.Yellow;
            interestdiff.ForeColor = Color.Yellow;
        }

        public Security Security
        {
            get { return _sec; }
            set
            {
                _sec = value;
                //sym.Text = .FullName;
            }
        }
        public void SetSecurity(Security sec)
        {
            _sec = sec;
            sym.Text = sec.Symbol;//sec.FullName+"|"+sec.PriceTick.ToString();
            _dispdecpointformat = LibUtil.GetDispdecpointformat(sec);


        }
        public void GotTick(Tick k)
        {
            if (k.isTrade)
            {
                last.Text = string.Format("{0:F1}", k.trade); ; size.Text = k.size.ToString();
                last.ForeColor = k.trade > k.PreSettlement ? Color.Red : Color.Green;
            }

            if (k.hasAsk)
                askprice.Text = string.Format("{0:F1}", k.ask); ; asksize.Text = k.os.ToString();
            if (k.hasBid)
                bidprice.Text = string.Format("{0:F1}", k.bid); ; bidsize.Text = k.bs.ToString();

            open.Text = string.Format("{0:F1}", k.Open);
            open.ForeColor = k.Open > k.PreSettlement ? Color.Red : Color.Green;

            high.Text = string.Format("{0:F1}", k.High);
            high.ForeColor = k.High > k.PreSettlement ? Color.Red : Color.Green;

            low.Text = string.Format("{0:F1}", k.Low);
            low.ForeColor = k.Low > k.PreSettlement ? Color.Red : Color.Green;

            change.Text = string.Format("{0:F1}", k.trade - k.PreSettlement);
            change.ForeColor = k.trade - k.PreSettlement > 0 ? Color.Red : Color.Green;
            openinterest.Text = k.OpenInterest.ToString();

            interestdiff.Text = (k.OpenInterest - k.PreOpenInterest).ToString();
            volume.Text = k.Vol.ToString();
            decimal ch = (k.trade - k.PreSettlement) / k.PreSettlement*100;
            chpercent.Text = string.Format("{0:F2}", ch);
            chpercent.ForeColor = k.trade - k.PreSettlement > 0 ? Color.Red : Color.Green;

        }
    }
}
            