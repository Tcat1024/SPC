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
