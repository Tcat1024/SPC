using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SPC.Controls.ConfigControls
{
    public partial class KMeansConfigControl : ConfigControlBase
    {
        private string[] originalColumns;
        public string[] SelectedColumns { get; private set; }
        public int StartClustNum { get; private set; }
        public int EndClustNum { get; private set; }
        public int MaxCount { get; private set; }
        public double Avg { get; private set; }
        public double Stdev { get; private set; }
        public int MaxThread { get; private set; }
        public int InitialMode { get; private set; }
        public KMeansConfigControl()
        {
            InitializeComponent();
        }
        public void Init(string[] columns, int count)
        {
            this.originalColumns = columns;
            this.checkedListBoxControl1.Items.Clear();
            this.checkedListBoxControl1.Items.AddRange(columns);
            this.textEdit4.Text = ((int)Math.Pow(count, 0.5)).ToString();
        }
        public override event EventHandler OKEvent;
        public override event EventHandler CancelEvent;
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (this.checkedListBoxControl1.CheckedItems.Count < 2)
            {
                MessageBox.Show("选择聚类的字段不能小于两个");
                return;
            }
            int count = this.checkedListBoxControl1.CheckedItems.Count;
            SelectedColumns = new string[count];
            for (int i = 0; i < count; i++)
            {
                SelectedColumns[i] = this.checkedListBoxControl1.CheckedItems[i].ToString();
            }
            StartClustNum = textEdit3.Text.Trim()==""?-1:Convert.ToInt32(textEdit3.Text.Trim());
            EndClustNum = textEdit4.Text.Trim() == "" ? -1 : Convert.ToInt32(textEdit4.Text.Trim());
            MaxCount = textEdit5.Text.Trim() == "" ? -1 : Convert.ToInt32(textEdit5.Text.Trim());
            Avg = textEdit1.Text.Trim() == ""|!checkEdit1.Checked ? double.NaN : Convert.ToDouble(textEdit1.Text.Trim());
            Stdev = textEdit2.Text.Trim() == "" | !checkEdit2.Checked ? double.NaN : Convert.ToDouble(textEdit2.Text.Trim());
            MaxThread = textEdit6.Text.Trim() == "" ? 15 : Convert.ToInt32(textEdit6.Text.Trim());
            InitialMode = comboBoxEdit1.SelectedIndex;
            if (OKEvent != null)
                OKEvent(this, new EventArgs());
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (CancelEvent != null)
                CancelEvent(this, new EventArgs());
        }
    }
}