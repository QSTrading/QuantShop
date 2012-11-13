using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QSTrading
{
    public partial class startForm : Form
    {
        public startForm()
        {
            InitializeComponent();
        }

        public void setInfo(string msg)
        {
            _info.Text = msg;
        }
        private void startForm_Load(object sender, EventArgs e)
        {

        }

        public void showError(string msg)
        {
            label2.Text = msg;
            label2.ForeColor = Color.Red;
        }
    }
}
