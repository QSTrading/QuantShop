using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradeLink.API;
using TradingLib.API;
using TradingLib.Data;

namespace TradingLib.GUI
{



    public partial class ctTimeSales : UserControl,GotTickIndicator
    {
        private Security _sec;
        public event VoidDelegate SendStyleChangeEvent;
        private bool _long=true;
        private TimeSalesType _type=TimeSalesType.Window;
        public TimeSalesType Type { get { return _type; } set { _type = value; } }
        public ctTimeSales()
        {
            InitializeComponent();
        }


        public void SetSeurity(Security sec)
        {
            _sec = sec;
            ctQuotePanel1.SetSecurity(sec);
            
            
        }

        public void GotTick(Tick k)
        {
            ctQuotePanel1.GotTick(k);

            //将trade加入TimeSales列表;
            /*
            if (k.isTrade)
                tsgd.Rows.Add(k.time, k.trade.ToString("N1"), k.size.ToString(), "test");
            if(tsgd.Rows.Count>1)
                tsgd.CurrentCell = tsgd.Rows[tsgd.Rows.Count - 1].Cells[0];
             * */

            if (k.isTrade)
                timeAndSales1.GotTick(k);
        
        }

        public bool IsSmall
        {
            get
            {
                return false;
            }
            //get{return !tsgd.Visible;}
            /*
            set {
                tsgd.Visible = value;
                if (tsgd.Visible)
                    Height = 600;
                else
                    Height = 200;
            }*/
        }

        private void ctQuotePanel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            /*
            if(_type == TimeSalesType.Window)
                tsgd.Visible = !tsgd.Visible;
            if (_type == TimeSalesType.ChartRight)
            {
                if (_long == true)
                {
                    Height = 400;
                    _long = false;
                }
                else
                {
                    Height = 100;
                    _long = true;
                }
                
                
            }
             * */

            /*
            if (tsgd.Visible)
            {
                //_long = Height;
                tsgd.Visible = false;
                //Height = 200;

            }
            else
            {   
                tsgd.Visible = true;
                //Height = _long;
            }
             * */
            if (SendStyleChangeEvent != null)
                SendStyleChangeEvent();
        }
    }
}
