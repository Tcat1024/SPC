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
    public partial class ContourPlotConfigControl : ConfigControlBase
    {
        public string X;
        public string Y;
        public string Z;
        public int PicWidth;
        public int PicHeight;
        public ContourPlotConfigControl()
        {
            InitializeComponent();
        }
        public void Init(string[] columns)
        {
            this.comboBoxEdit1.Properties.Items.Clear();
            this.comboBoxEdit1.Properties.Items.AddRange(columns);
            this.comboBoxEdit2.Properties.Items.Clear();
            this.comboBoxEdit2.Properties.Items.AddRange(columns);
            this.comboBoxEdit3.Properties.Items.Clear();
            this.comboBoxEdit3.Properties.Items.AddRange(columns);
        }
        public override event EventHandler OKEvent;
        public override event EventHandler CancelEvent;
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.comboBoxEdit1.SelectedIndex == -1)
            {
                MessageBox.Show("未选择有效的X轴字段");
                return;
            }
            if (this.comboBoxEdit2.SelectedIndex == -1)
            {
                MessageBox.Show("未选择有效的Y轴字段");
                return;
            }
            if (this.comboBoxEdit3.SelectedIndex == -1)
            {
                MessageBox.Show("未选择有效的Z轴字段");
                return;
            }
            if(this.comboBoxEdit4.SelectedIndex==3)
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
            this.X = this.comboBoxEdit1.Text;
            this.Y = this.comboBoxEdit2.Text;
            this.Z = this.comboBoxEdit3.Text;
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
    }
}
