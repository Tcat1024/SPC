using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SPC.Analysis.ConfigControls
{
    public partial class CCTConfigControl : ConfigControlBase
    {
        public string TargetColumn = "";
        public string[] Columns;
        private string[] sourceColumns;
        public CCTConfigControl()
        {
            InitializeComponent();
        }
        public void Init(string[] columns)
        {
            this.sourceColumns = columns;
            this.checkedListBoxControl1.DataSource = sourceColumns;
            this.comboBoxEdit1.Properties.Items.Clear();
            this.comboBoxEdit1.Properties.Items.AddRange(sourceColumns);
        }
        public override event EventHandler OKEvent;
        public override event EventHandler CancelEvent;
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.checkedListBoxControl1.SelectedItems.Count == 0)
            {
                MessageBox.Show("未选择任何字段");
                return;
            }
            if (this.comboBoxEdit1.SelectedIndex == -1)
            {
                MessageBox.Show("未选择有效的目标字段");
                return;
            }
            this.TargetColumn = this.comboBoxEdit1.Text;
            this.Columns = new string[this.checkedListBoxControl1.CheckedItems.Count];
            int i = 0;
            foreach (var item in this.checkedListBoxControl1.CheckedItems)
                this.Columns[i++] = item.ToString();
            if (OKEvent != null)
                OKEvent(this, new EventArgs());
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (CancelEvent != null)
                CancelEvent(this, new EventArgs());
        }
    }
}
