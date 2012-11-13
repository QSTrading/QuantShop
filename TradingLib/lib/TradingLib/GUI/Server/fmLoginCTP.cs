using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TradingLib.GUI.Server
{
	public partial class FormLoginTrade : Form
	{
		public FormLoginTrade()
		{
			InitializeComponent();
		}

		private void UserLogin_Load(object sender, EventArgs e)
		{
            /*
			//从设置里读取服务器列表
			for (int i = 0; i < Properties.Settings.Default.Servers.Count; i++)
			{
				string server = Properties.Settings.Default.Servers[i];
				this.cbServer.Items.Add(server.Split('|')[0]);
				for (int j = server.Split('|').Length; j < 8; j++)
					Properties.Settings.Default.Servers[i] = Properties.Settings.Default.Servers[i] + "|";
			}
			if (this.cbServer.Items.Count > 0)
				this.cbServer.SelectedIndex = Properties.Settings.Default.ServerIndex;	//默认设置
             * **/
		}

		private void FormLoginTrade_FormClosed(object sender, FormClosedEventArgs e)
		{
			//Properties.Settings.Default.Save();
		}

		private void buttonSetServer_Click(object sender, EventArgs e)
		{
            /*
			using (FormServers fs = new FormServers())
			{
				fs.dataGridViewServers.Columns.Add("Broker", "经纪公司");
				fs.dataGridViewServers.Columns.Add("BrokerID", "经纪代码");
				fs.dataGridViewServers.Columns.Add("MDAddr", "行情服务器地址");
				fs.dataGridViewServers.Columns.Add("MDPort", "端口");
				fs.dataGridViewServers.Columns.Add("TradeAddr", "交易服务器地址");
				fs.dataGridViewServers.Columns.Add("TradePort", "端口");
				fs.dataGridViewServers.Columns.Add("Invorter", "用户");
				fs.dataGridViewServers.Columns["Invorter"].Visible = false;
				fs.dataGridViewServers.Columns.Add("PWD", "密码");
				fs.dataGridViewServers.Columns["PWD"].Visible = false;
				fs.dataGridViewServers.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				fs.dataGridViewServers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
				fs.dataGridViewServers.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				fs.dataGridViewServers.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;	//不换行
				//fs.dataGridViewServers.Columns["Broker"].Width = 60;
				//fs.dataGridViewServers.Columns["BrokerID"].Width = 60;
				//fs.dataGridViewServers.Columns["MDAddr"].Width = 60;
				//fs.dataGridViewServers.Columns["MDPort"].Width = 60;
				//fs.dataGridViewServers.Columns["TradeAddr"].Width = 60;
				//fs.dataGridViewServers.Columns["TradePort"].Width = 60;
				//从设置里读取服务器列表
				for (int i = 0; i < Properties.Settings.Default.Servers.Count; i++)
				{
					try
					{
						fs.dataGridViewServers.Rows.Add(Properties.Settings.Default.Servers[i].Split('|'));//
					}
					catch { }
				}

				if (fs.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
				{
					//添加到设置里
					Properties.Settings.Default.Servers.Clear();
					this.cbServer.Items.Clear();
					for (int i = 0; i < fs.dataGridViewServers.Rows.Count; i++)
					{
						string server = string.Empty;	//保存服务器信息
						for(int j = 0 ;j<fs.dataGridViewServers.Columns.Count;j++)
						{
							if (fs.dataGridViewServers.Rows[i].Cells[j].Value != null)
							{
								server += fs.dataGridViewServers.Rows[i].Cells[j].Value + "|";
							}
						}
						if (!string.IsNullOrWhiteSpace(server))
						{
							server += "|";
							//server.Remove(server.Length - 1);	//去掉尾"|"
							this.cbServer.Items.Add((string)fs.dataGridViewServers.Rows[i].Cells[0].Value);	//加到列表中
							Properties.Settings.Default.Servers.Add(server);
						}
					}
					if (this.cbServer.Items.Count > 0)
						this.cbServer.SelectedIndex = 0;
				}
			}
             * **/
		}

		private void cbServer_SelectedIndexChanged(object sender, EventArgs e)
		{
            /*
			Properties.Settings.Default.ServerIndex = this.cbServer.SelectedIndex;	//设置
			this.tbUserID.Text = Properties.Settings.Default.Servers[this.cbServer.SelectedIndex].Split('|')[6];
			if(Properties.Settings.Default.SavePWD)
				this.tbPassword.Text = Properties.Settings.Default.Servers[this.cbServer.SelectedIndex].Split('|')[7];
             * */
		}

		private void button1_Click(object sender, EventArgs e)
		{
            /*
			Properties.Settings.Default.Servers.Clear();
			Properties.Settings.Default.ServerIndex = 0;
             * */
		}

      
	}
}
