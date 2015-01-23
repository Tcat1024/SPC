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
    public partial class ColumnCalculateConfigControl : ConfigControlBase
    {
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
        public ColumnCalculateConfigControl()
        {
            InitializeComponent();
        }
        public void Init(string[] columns)
        {
            this.checkedListBoxControl1.Items.Clear();
            this.checkedListBoxControl1.Items.AddRange(columns);
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            int count = this.checkedListBoxControl1.CheckedItems.Count;
            if (count < 1)
            {
                MessageBox.Show("未选择任何列");
                return;
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
            if (CancelEvent != null)
                CancelEvent(this, new EventArgs());
        }
    }
}
