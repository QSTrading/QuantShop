using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradeLink.API;

namespace Strategy.GUI
{
    public partial class fmStopTrailing : Form
    {
        public event VoidDelegate SendFlatPosition;
        public event VoidDelegate SendBuyAction;
        public event VoidDelegate SendSellAction;
        private bool stoplossEnable = false;
        private bool breakevenEnable = false;

        public int OrdSize { get { return (int)_osize.Value; } }
        public fmStopTrailing()
        {
            InitializeComponent();

        }

        //设定功能
        public void setFunction(bool stoploss, bool breakeven)
        {
            if (!stoploss)
                _stoploss.Text = "无止损!";
            if (!breakeven)
                _breakEven.Text = "无BE!";
            stoplossEnable = stoploss;
            breakevenEnable = breakeven;
        
        }
        //绑定数据源
        public void bindDataSource(object o,string symbol)
        {
            ctTimesLineChart1.setDataSource(o, symbol);
        }

        //发送信息
        public void message(string msg)
        {
            debugControl1.GotDebug(msg);
        }
        //更新窗体界面
        public void updateForm(Tick k,Position p,decimal stoplossprice,decimal breakevenprice,decimal profitprice1,decimal profitprice2)
        {

             Color c =  p.UnRealizedPL >= 0 ? System.Drawing.Color.Red : System.Drawing.Color.Green;
            _last.Text = k.isTrade ? formatdisp(k.trade) : _last.Text;
            _last.ForeColor = c;
            _bid.Text = formatdisp(k.bid);
            _bid.ForeColor = c;
            _ask.Text = formatdisp(k.ask);
            _ask.ForeColor = c;
            _pos.Text = p.Size.ToString("N0");

            _avgcost.Text = formatdisp(p.AvgPrice);
            _favor.Text = p.isLong ? formatdisp(p.Highest) : formatdisp(p.Lowest);
            _adverse.Text = p.isLong ? formatdisp(p.Lowest) : formatdisp(p.Highest);
            _unrealizedpl.Text = formatdisp(p.UnRealizedPL);
            _unrealizedpl.ForeColor = c;
            _closedpl.Text = formatdisp(p.ClosedPL);
            _closedpl.ForeColor = p.ClosedPL >0 ? System.Drawing.Color.Red : System.Drawing.Color.Green;
            if(stoplossEnable)
                _stoploss.Text = formatdisp(stoplossprice);

            if (breakevenEnable)
            {
                string s1 = "未触发";
                if (breakevenprice > 0)
                    s1 = formatdisp(breakevenprice);
                _breakEven.Text = s1;
            }

            string s ="未触发动态止盈";
            if(profitprice1>0)
                s = "一级:" + formatdisp(profitprice1);
            if(profitprice2>0)
                s = "二级:" + formatdisp(profitprice2);
            _profitTakestop.Text = s;
            button1.Enabled = p.isFlat?false:true;

            //刷新绘图
            ctTimesLineChart1.GotTick(k);

        }

        private void fmStopTrailing_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private string formatdisp(decimal d)
        {
            return string.Format("{0:F1}", d);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (SendFlatPosition != null)
                SendFlatPosition();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (SendBuyAction != null)
                SendBuyAction();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (SendSellAction != null)
                SendSellAction();
        }

        private void fmStopTrailing_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        bool _fullwin = true;
        private void panel1_DoubleClick(object sender, EventArgs e)
        {
            if (_fullwin == true)
            {
                Height = Height - 300;
                _fullwin = false;
                tabControl1.Visible = false;
            }
            else
            {
                Height = Height + 300;
                _fullwin = true;
                tabControl1.Visible = true;
            }
        }

        //public Const  HTCAPTION = 2; 
        const int WM_NCLBUTTONDBLCLK = 0x00A3;
        //private bool _winhide = false;
        private int _winheight = 0;
        protected override void WndProc(ref Message m)
        {
            
            if (m.WParam.ToInt32() == 2 && m.Msg == WM_NCLBUTTONDBLCLK)
            {
                if (_winheight<=0)
                {
                    _winheight = Height;
                    ClientSize = new Size(ClientSize.Width, 0);
                   
                   
                }
                else
                {
                    ClientSize = new Size(ClientSize.Width, 440);
                    _winheight = 0;

                }
                
            }

            base.WndProc(ref m);
                

        }
    }
}
