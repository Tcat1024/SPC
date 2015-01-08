using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevExpress.XtraEditors;

namespace SPC.Base.Control
{
    [Designer(typeof(LoadingControlDesigner))]
    public partial class WaitPanel : DevExpress.XtraEditors.XtraUserControl
    {
        public WaitPanel()
        {
            InitializeComponent();
        }
        public int Position
        {
            get
            {
                return this.progressBarControl1.Position;
            }
            set
            {
                this.progressBarControl1.Position = value;
            }
        }
        public event EventHandler CancelButtonClick;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (CancelButtonClick != null)
                CancelButtonClick(sender, e);
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.progressPanel1.Location = new Point((this.Width-progressPanel1.Width)/2,12);
            this.progressBarControl1.Width = (int)(this.Width * 0.95);
            this.progressBarControl1.Location = new Point((this.Width-progressBarControl1.Width)/2,progressPanel1.Height+progressPanel1.Location.Y+3);
            this.btnCancel.Location = new Point((this.Width - this.btnCancel.Width)/2,(this.Height + this.progressBarControl1.Location.Y+this.progressBarControl1.Height)/2 - this.btnCancel.Height/2);
        }
    }
    public class LoadingControlDesigner : System.Windows.Forms.Design.ControlDesigner
    {
        public LoadingControlDesigner()
        {
        }

        public override SelectionRules SelectionRules
        {
            get
            {
                SelectionRules rules = SelectionRules.Visible | SelectionRules.Moveable;
                return rules;
            }
        }
    }
}
