using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradeLink.API;
using TradingLib.Data;

namespace TradingLib.GUI
{
    public partial class fmSecListEdit_NewList : Form
    {
        public event StringParamDelegate ConfirmBasketName;
        private string _bname = string.Empty;
        //public string BasketName { get { return _bname; } set { _bname = value; } }

        public fmSecListEdit_NewList()
        {
            InitializeComponent();
            output.Text = string.Empty;
            FormClosed +=new FormClosedEventHandler(fmSecListEdit_NewList_FormClosed);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(basketName.Text == string.Empty)
            {
                output.Text = "请输入列表名!";
                return;
            }
            if(BasketTracker.HaveBasket(basketName.Text))
            {
                output.Text = "已存在该列表名!";
                return;
            }
            if(ConfirmBasketName != null)
            {
                ConfirmBasketName(basketName.Text);
            }
            Close();

        }

        private void baseketName_TextChanged(object sender, EventArgs e)
        {
            output.Text = string.Empty;
        }

        private void fmSecListEdit_NewList_FormClosed(object sender, EventArgs e)
        {
            output.Text = string.Empty;
            basketName.Text = string.Empty;
        }
    }
}
