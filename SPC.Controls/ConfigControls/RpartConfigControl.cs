using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SPC.Controls.ConfigControls
{
    public partial class RpartConfigControl : ConfigControlBase
    {
        public string TargetColumn = "";
        public string[] Columns;
        public string Method;
        private string[] sourceColumns;
        public int PicWidth;
        public int PicHeight;
        public double CP;
        public RpartConfigControl()
        {
            InitializeComponent();
        }
        public void Init(string[] columns)
        {
            bool same = false;
            int length;
            if (this.sourceColumns != null && (length = columns.Length) == this.sourceColumns.Length)
            {
                same = true;
                for (int i = 0; i < length; i++)
                {
                    if (this.sourceColumns[i] != columns[i])
                    {
                        same = false;
                        break;
                    }
                }
            }
            this.sourceColumns = columns;
            if (!same)
            {
                this.checkedListBoxControl1.DataSource = sourceColumns;
                this.comboBoxEdit1.Properties.Items.Clear();
                this.comboBoxEdit1.Properties.Items.AddRange(sourceColumns);
            }
        }
        public override event EventHandler OKEvent;
        public override event EventHandler CancelEvent;
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.comboBoxEdit1.SelectedIndex == -1)
            {
                MessageBox.Show("未选择有效的目标字段");
                return;
            }
            this.TargetColumn = this.comboBoxEdit1.Text;
            this.checkedListBoxControl1.SetItemChecked(this.comboBoxEdit1.SelectedIndex, false);
            if (this.checkedListBoxControl1.CheckedItems.Count == 0)
            {
                MessageBox.Show("未选择任何字段");
                return;
            }
            if (this.comboBoxEdit4.SelectedIndex == 3)
            {
                if (this.textEdit1.Text.Trim() == "" || this.textEdit2.Text.Trim() == "")
                {
                    MessageBox.Show("未指定正确的大小");
                    return;
                }
                else
                {
                    this.PicWidth = Convert.ToInt32(textEdit1.Text.Trim());
                    this.PicHeight = Convert.ToInt32(textEdit2.Text.Trim());
                }
            }
            else
            {
                var re = comboBoxEdit4.Text.Split('*');
                this.PicWidth = Convert.ToInt32(re[0]);
                this.PicHeight = Convert.ToInt32(re[1]);
            }
            this.Columns = new string[this.checkedListBoxControl1.CheckedItems.Count];
            switch(this.comboBoxEdit2.SelectedIndex)
            {
                case 0: this.Method = "anova"; break;
                case 1: this.Method = "poisson"; break;
                case 2: this.Method = "class"; break;
                case 3: this.Method = "exp"; break;
            }
            int i = 0;
            foreach (var item in this.checkedListBoxControl1.CheckedItems)
                this.Columns[i++] = item.ToString();
            if (this.checkEdit1.Checked)
                CP = Convert.ToDouble(this.textEdit3.Text);
            else
                CP = -1;
            if (OKEvent != null)
                OKEvent(this, new EventArgs());
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (CancelEvent != null)
                CancelEvent(this, new EventArgs());
        }

        private void comboBoxEdit4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxEdit4.SelectedIndex == 3)
                this.panelControl1.Enabled = true;
            else
                this.panelControl1.Enabled = false;
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit1.Checked)
                this.textEdit3.Enabled = true;
            else
                this.textEdit3.Enabled = false;
        }
    }
}
