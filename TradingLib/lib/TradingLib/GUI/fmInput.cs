using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TradingLib.GUI
{
    public partial class fmInput : Form
    {
        public string Value 
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }
        public fmInput()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
