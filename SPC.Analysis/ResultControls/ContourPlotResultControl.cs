using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SPC.Analysis.ResultControls
{
    public partial class ContourPlotResultControl : DevExpress.XtraEditors.XtraUserControl
    {
        public ContourPlotResultControl()
        {
            InitializeComponent();
        }
        public void Init(Image pic,string xn,string yn,string zn)
        {
            this.pictureEdit1.Image = pic;
            this.labelControl1.Text = string.Format("X轴: {0}    Y轴: {1}    Z轴: {2}", xn, yn,zn);
            this.splitContainerControl2.Panel1.MinSize = pic.Size.Height+panelControl1.Height;
            this.splitContainerControl1.Panel1.MinSize = Math.Max(pic.Size.Width,labelControl1.Width);
            this.MinimumSize = new Size(this.splitContainerControl1.Panel1.MinSize,this.splitContainerControl2.Panel1.MinSize);
            this.labelControl1.Location = new Point((this.panelControl1.Width - this.labelControl1.Width) / 2, this.labelControl1.Location.Y);
        }

        private void pictureEdit1_Enter(object sender, EventArgs e)
        {
            this.splitContainerControl2.Panel1.Focus();
        }

        private void panelControl1_Enter(object sender, EventArgs e)
        {
            this.splitContainerControl2.Panel1.Focus();
        }

        private void panelControl1_Resize(object sender, EventArgs e)
        {
            this.labelControl1.Location = new Point((this.panelControl1.Width - this.labelControl1.Width) / 2, this.labelControl1.Location.Y);
        }
    }
}
