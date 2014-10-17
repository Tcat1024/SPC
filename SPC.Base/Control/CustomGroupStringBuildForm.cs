using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SPC.Base.Control
{
    public partial class CustomGroupStringBuildForm : DevExpress.XtraEditors.XtraForm
    {
        public string result = "";
        public CustomGroupStringBuildForm()
        {
            InitializeComponent();
        }

        private void customGroupStringBuilder1_GroupStringCanceled(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }

        private void customGroupStringBuilder1_GroupStringDetermined(object sender, CustomGroupStringBuilder.GroupStringDeterminedEventArgs e)
        {
            this.result = e.result;
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
        }
    }
}