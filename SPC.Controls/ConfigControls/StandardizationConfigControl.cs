using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SPC.Controls.ConfigControls
{
    public partial class StandardizationConfigControl : ConfigControlBase
    {
        public int MethodType
        {
            get
            {
                return currentCheckEdit == null ? -1 : Convert.ToInt32(currentCheckEdit.Tag);
            }
        }
        public double Arg_1 = double.NaN;
        public double Arg_2 = double.NaN;
        public string[] SourceColumns;
        public string[] TargetColumns;
        public StandardizationConfigControl()
        {
            InitializeComponent();
            currentCheckEdit = this.checkEdit1;
        }
        DevExpress.XtraEditors.CheckEdit currentCheckEdit;
        private void checkEdit_CheckedChanged(object sender, EventArgs e)
        {
            var target = (sender as DevExpress.XtraEditors.CheckEdit);
            if (target.Checked)
            {
                if (currentCheckEdit != null)
                    currentCheckEdit.Checked = false;
                currentCheckEdit = target;
            }
        }
        private void checkPanel1_CheckedChanged(object sender, EventArgs e)
        {
            var target = (sender as DevExpress.XtraEditors.CheckEdit);
            if (target.Checked)
                this.panelControl1.Enabled = true;
            else
                this.panelControl1.Enabled = false;
        }
        private void checkPanel2_CheckedChanged(object sender, EventArgs e)
        {
            var target = (sender as DevExpress.XtraEditors.CheckEdit);
            if (target.Checked)
                this.panelControl2.Enabled = true;
            else
                this.panelControl2.Enabled = false;
        }
        public void Init(string[] columns)
        {
            this.listBoxControl1.Items.Clear();
            this.listBoxControl1.Items.AddRange(columns);
        }
        public override event EventHandler OKEvent;
        public override event EventHandler CancelEvent;

        private void btnOK_Click(object sender, EventArgs e)
        {
            int incount = this.listBoxControl2.Items.Count;
            int outcount = this.listBoxControl3.Items.Count;
            if (incount < 1)
            {
                MessageBox.Show("未选择要标准化的列");
                return;
            }
            if (incount != outcount)
            {
                MessageBox.Show("输出列数量与输入列数量不相等");
                return;
            }
            Arg_1 = double.NaN;
            Arg_2 = double.NaN;
            if (checkEdit5.Checked) 
            {
                if (Convert.ToDouble(textEdit3.Text) >= Convert.ToDouble(textEdit4.Text))
                {
                    MessageBox.Show("起始值必须小于结束值");
                    return;
                }
                Arg_1 = Convert.ToDouble(textEdit3.Text);
                Arg_2 = Convert.ToDouble(textEdit4.Text);
            }
            else if(checkEdit4.Checked)
            {
                Arg_1 = Convert.ToDouble(textEdit1.Text);
                Arg_2 = Convert.ToDouble(textEdit2.Text);
            }
            SourceColumns = new string[outcount];
            TargetColumns = new string[outcount];
            for(int i = 0;i<outcount;i++)
            {
                SourceColumns[i] = this.listBoxControl2.Items[i].ToString();
                TargetColumns[i] = this.listBoxControl3.Items[i].ToString();
            }
            if (this.OKEvent != null)
                OKEvent(this, new EventArgs());
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (this.CancelEvent != null)
                CancelEvent(this, new EventArgs());
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            foreach(var item in this.listBoxControl1.SelectedItems)
            {
                if (!this.listBoxControl2.Items.Contains(item))
                    this.listBoxControl2.Items.Add(item);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            int count = this.listBoxControl2.SelectedItems.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                this.listBoxControl2.Items.Remove(listBoxControl2.SelectedItems[i]);
            }
        }
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            foreach (var item in this.listBoxControl1.SelectedItems)
            {
                if (!this.listBoxControl3.Items.Contains(item))
                    this.listBoxControl3.Items.Add(item);
            }
        }
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            int count = this.listBoxControl3.SelectedItems.Count;
            for (int i = count-1; i >= 0;i--)
            {
                this.listBoxControl3.Items.Remove(listBoxControl3.SelectedItems[i]);
            }
        }

        private void buttonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            string target = this.buttonEdit1.Text.Trim();
            if(target!="")
            {
                if(this.listBoxControl3.Items.Contains(target))
                {
                    MessageBox.Show("指定列已存在");
                    return;
                }
                this.listBoxControl3.Items.Add(target);
            }
        }

        private void textEdit1_Validating(object sender, CancelEventArgs e)
        {
            if ((sender as DevExpress.XtraEditors.TextEdit).Text.Trim() == "")
            {
                MessageBox.Show("不能为空");
                e.Cancel = true;
            }
        }

        private void textEdit2_Validating(object sender, CancelEventArgs e)
        {
            string text = (sender as DevExpress.XtraEditors.TextEdit).Text.Trim();
            if (text == "")
            {
                MessageBox.Show("不能为空");
                e.Cancel = true;
            }
            else if (Convert.ToDouble(text) == 0)
            {
                MessageBox.Show("不能为0");
                e.Cancel = true;
            }
        }

    }
}
