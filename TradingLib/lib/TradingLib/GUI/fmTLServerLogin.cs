using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradeLink.API;

namespace TradingLib.GUI
{
    public partial class fmTLServerLogin : Form
    {
        public event AccountRequestDel SendTLServerLogin;
        public fmTLServerLogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (SendTLServerLogin != null)
            { 
                SendTLServerLogin(accName.Text,Convert.ToInt32(accPass.Text));
            }
            Close();
        }


    }
}
