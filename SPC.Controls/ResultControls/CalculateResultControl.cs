using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SPC.Controls.ResultControls
{
    public partial class CalculateResultControl : DevExpress.XtraEditors.XtraUserControl
    {
        public CalculateResultControl()
        {
            InitializeComponent();
        }
        public void Init(DataTable data)
        {
            this.gridControl1.DataSource = null;
            this.gridView1.Columns.Clear();
            this.gridControl1.DataSource = data;
        }
    }
}
