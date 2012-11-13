using System;
using System.Drawing;
using System.Windows.Forms;
using TradeLink.API;

namespace TradingLib.GUI
{
	/// <summary>
	/// Param Prompt quickly creates a GUI to edit properties of your Response
	/// </summary>
	public partial class fmPositionCheckParamPrompt : Form
	{

        public event AddResponseDel SendAddResponseEvent;
        private Button applyoptionsbut;
        private PropertyGrid propertyGrid1;

        /// <summary>
        /// You create a param prompt to display a gui for your Response
        /// </summary>
        /// <param name="DisplayParamsOfObject">The Response for which you want the user to specify or alter the properties for.</param>
		public fmPositionCheckParamPrompt(Response DisplayParamsOfObject)
		{

			InitializeComponent();
			this.Text = DisplayParamsOfObject.Name + " ²ÎÊý";
            propertyGrid1.PropertySort = PropertySort.Categorized;
			propertyGrid1.SelectedObject = (object)DisplayParamsOfObject;
            DialogResult = DialogResult.Cancel;
            Invalidate();

		}

        static Response r;
        /// <summary>
        /// popup the parameters for selected response
        /// </summary>
        public static void Popup(Response displayParamsAvail) { Popup(displayParamsAvail, false,false); }
        public static void Popup(Response displayParamsAvail, bool skip) { Popup(displayParamsAvail, false, skip); }
        /// <summary>
        /// pop up the parameters, allows you to pausing the application to do so (also can skip prompt)
        /// </summary>
        /// <param name="pauseapp"></param>
        public static void Popup(Response displayParamsAvail, bool pauseapp, bool skip)
        {
            if (skip) return;
            fmPositionCheckParamPrompt p = new fmPositionCheckParamPrompt(displayParamsAvail);
            r = displayParamsAvail;
            p.Invalidate(true);
            if (pauseapp)
            {
                if (p.ShowDialog() == DialogResult.OK)
                {
                }
                else
                    displayParamsAvail.isValid = false;
            }
            else
            {
                p.FormClosing += new FormClosingEventHandler(p_FormClosing);
                p.Show();
            }
            p.Invalidate(true);
        }

        static void p_FormClosing(object sender, FormClosingEventArgs e)
        {
            fmPositionCheckParamPrompt p = (fmPositionCheckParamPrompt)sender;
            if (p.DialogResult != DialogResult.OK)
                r.isValid = false;
        }

		void ApplybutClick(object sender, EventArgs e)
		{
            DialogResult = DialogResult.OK;
            //propertyGrid1
            if (SendAddResponseEvent != null)
                SendAddResponseEvent((Response)propertyGrid1.SelectedObject);
			this.Close();
		}

        private void InitializeComponent()
        {
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.applyoptionsbut = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.CommandsVisibleIfAvailable = false;
            this.propertyGrid1.LineColor = System.Drawing.SystemColors.ControlLight;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(207, 381);
            this.propertyGrid1.TabIndex = 0;
            // 
            // applyoptionsbut
            // 
            this.applyoptionsbut.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.applyoptionsbut.Location = new System.Drawing.Point(0, 380);
            this.applyoptionsbut.Name = "applyoptionsbut";
            this.applyoptionsbut.Size = new System.Drawing.Size(207, 32);
            this.applyoptionsbut.TabIndex = 1;
            this.applyoptionsbut.Text = "Accept";
            this.applyoptionsbut.UseVisualStyleBackColor = true;
            this.applyoptionsbut.Click += new System.EventHandler(this.ApplybutClick);
            // 
            // fmPositionCheckParamPrompt
            // 
            this.ClientSize = new System.Drawing.Size(207, 411);
            this.Controls.Add(this.applyoptionsbut);
            this.Controls.Add(this.propertyGrid1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fmPositionCheckParamPrompt";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ParamPrompt_Load);
            this.SizeChanged += new System.EventHandler(this.ParamPrompt_SizeChanged);
            this.ResumeLayout(false);

        }

        void ParamPrompt_Load(object sender, EventArgs e)
        {
            if (ClientRectangle.Height != 0)
                propertyGrid1.Height = (int)ClientRectangle.Height - applyoptionsbut.Height -3 ;
        }

        void ParamPrompt_SizeChanged(object sender, EventArgs e)
        {
            int neww = (int)(ClientRectangle.Width);
            if (neww != 0)
                propertyGrid1.Width = neww;
            if (ClientRectangle.Height != 0)
                propertyGrid1.Height = (int)ClientRectangle.Height - applyoptionsbut.Height -3;
            Invalidate(true);
        }
	}
}
