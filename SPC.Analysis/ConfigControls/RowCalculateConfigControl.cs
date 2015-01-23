using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SPC.Analysis.ConfigControls
{
    public partial class RowCalculateConfigControl : ConfigControlBase
    {
        public bool SaveNewColumn
        {
            get
            {
                return this.checkEdit1.Checked;
            }
        }
        public string TargetColumn
        {
            get
            {
                return this.textEdit1.Text.Trim();
            }
        }
        public int MethodIndex
        {
            get
            {
                return this.radioGroup1.SelectedIndex;
            }
        }
        public string MethodString
        {
            get
            {
                return this.radioGroup1.Properties.Items[MethodIndex].Description;
            }
        }
        public string[] SourceColumns;
        public RowCalculateConfigControl()
        {
            InitializeComponent();
        }
        public void Init(string[] columns)
        {
            this.checkedListBoxControl1.Items.Clear();
            this.checkedListBoxControl1.Items.AddRange(columns);
        }
        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkEdit1.Checked)
                this.textEdit1.Enabled = true;
            else
                this.textEdit1.Enabled = false;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            int count = this.checkedListBoxControl1.CheckedItems.Count;
            if (count < 1)
            {
                MessageBox.Show("未选择任何列");
                return;
            }
            if (this.SaveNewColumn)
            {
                if (this.TargetColumn == "")
                {
                    MessageBox.Show("请输入正确的列名");
                    return;
                }
                else if (this.checkedListBoxControl1.Items.Contains(this.TargetColumn) && MessageBox.Show("指定列已存在，是否覆盖原有列？", "Warning", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    return;
                }
            }
            this.SourceColumns = new string[count];
            int i = 0;
            foreach (var col in this.checkedListBoxControl1.CheckedItems)
                this.SourceColumns[i++] = col.ToString();
            if (OKEvent != null)
                OKEvent(this, new EventArgs());
        }
        public override event EventHandler OKEvent;
        public override event EventHandler CancelEvent;
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if(CancelEvent!=null)
                CancelEvent(this,new EventArgs());
        }
    }
}
