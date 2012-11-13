using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.Data;
namespace TradingLib.GUI
{
    public partial class ctSecTreeView : UserControl
    {
        public ctSecTreeView()
        {
            InitializeComponent();
            try
            {
                UIUtil.genSecTreeView(ref SecLists);
            }
            catch (Exception ex)
            { 
            }

           /* ArrayList cl = new ArrayList();

            TreeNode ct0 = new TreeNode("IF1210");
            //cl.Add(ct0);

            //string[] seclist = BasketTracker.getSecList("Default");
            //TreeNode[] a = new TreeNode[2];
            //a[0] = new TreeNode(seclist[0]);
            //a[1] = new TreeNode(seclist[1]);
            //MessageBox.Show(seclist.Length.ToString());
            //TreeNode tn = new TreeNode("Default",a);
            //SecLists.Nodes.Add(tn);

            string[] seclist = BasketTracker.getSecList("Default");
            TreeNode[] a = new TreeNode[seclist.Length];
            for (int i = 0; i < seclist.Length; i++)
            {
                a[i] = new TreeNode(seclist[i]);
            }

            SecLists.Nodes.Add(new TreeNode("Default", a));
            * */
        }
    }
}
