using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.GUI;
using TradeLink.API;

namespace TradingLib.GUI
{
    public partial class ctSecListBox : UserControl
    {
        //增加
        private string _selectBasket = null;
        private Security _selectSecurity = null;
        public ctSecListBox()
        {
            InitializeComponent();
            Load +=new EventHandler(ctSecListBox_Load);
            try
            {
                UIUtil.genComboBoxBasekt(ref basket);
                
                //MessageBox.Show(SelectedBasket);
            }
            catch (Exception ex)
            { }
        }
        private void ctSecListBox_Load(object sender, EventArgs e)
        {
            try
            {
                //MessageBox.Show("加载结束:" + SelectedBasket);
                basket.SelectedItem = basket.Items[0];
                _selectBasket = ((ValueObject<string>)basket.SelectedItem).Value;
                //MessageBox.Show("加载结束:" + SelectedBasket);
            }
            catch (Exception ex)
            { 
            }
            
        }
        public string SelectedBasket
        {   get { return _selectBasket; }
            set {
                _selectBasket = value;
            
            }
        }

        public Security SelectedSecurity { get { return _selectSecurity; } }

        //当增加新的basket时,我们需要将选择的basket锁定在新增加的basket上
        public void onAddDelBasket(string s)
        {
            //重新加载basket列表
            UIUtil.genComboBoxBasekt(ref basket);
            //如果增加list则重新加载list后 停留在新增加的list上面
            if (s != string.Empty)
            {
                for (int i = 0; i < basket.Items.Count; i++)
                {
                    if (((ValueObject<string>)basket.Items[i]).Value == s)
                    {
                        basket.SelectedItem = basket.Items[i];
                        //_selectBasket = ((ValueObject<string>)basket.SelectedItem).Value;
                        //UIUtil.genListBoxBasket(ref lb, s);
                    }
                }
            }
        }
        //当某个Basket的Symbol列表发生变化时候，我们刷新该列表
        public void onSecListChanged()
        {
            if ((SelectedBasket != string.Empty) || SelectedBasket != null)
                UIUtil.genListBoxBasket(ref lb,SelectedBasket);
            
        }

        //我们改变basket选择所发生的操作
        private void basket_SelectedValueChanged(object sender, EventArgs e)
        {
            //获得选择的basket的名称
            //MessageBox.Show(((ValueObject<string>)basket.SelectedItem).Value);
            string s = ((ValueObject<string>)basket.SelectedItem).Value;
            _selectBasket = s;
            //重新加载列表中的security
            UIUtil.genListBoxBasket(ref lb, s);
        }

        private void lb_SelectedValueChanged(object sender, EventArgs e)
        {
            //lb.SelectedItem
            //MessageBox.Show(((ValueObject<Security>)lb.SelectedItem).Value.FullName);
            _selectSecurity = ((ValueObject<Security>)lb.SelectedItem).Value;

        }
        

    }
}
