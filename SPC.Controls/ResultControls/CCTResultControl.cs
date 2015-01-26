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
    public partial class CCTResultControl : DevExpress.XtraEditors.XtraUserControl
    {
        List<DataType> data = new List<DataType>();
        public CCTResultControl()
        {
            InitializeComponent();        
        }
        public void Init(string[] columnnames, double[] ccts)
        {
            if (columnnames.Length != ccts.Length)
                throw new Exception("错误的输入数据,字段数与结果数量不符");
            int i = 0;
            foreach (var columnname in columnnames)
                data.Add(new DataType(columnname, ccts[i++]));
            this.gridControl1.DataSource = data;
            Draw();
        }
        private void Draw()
        {
            this.chartControl1.Series[0].Points.BeginUpdate();
            this.chartControl1.Series[0].Points.Clear();
            int count = this.gridView1.DataRowCount;
            for (int i = 0; i < count; i++)
            {
                this.chartControl1.Series[0].Points.Add(new DevExpress.XtraCharts.SeriesPoint(this.gridView1.GetRowCellValue(i, "ColumnName"),this.gridView1.GetRowCellValue(i, "CCT")));
            }
            this.chartControl1.Series[0].Points.EndUpdate();
        }
        private class DataType
        {
            public string ColumnName { get; set; }
            public double CCT { get; set; }
            public DataType(string colname,double cct)
            {
                this.ColumnName = colname;
                this.CCT = cct;
            }
        }

        private void gridView1_EndSorting(object sender, EventArgs e)
        {
            Draw();
        }

    }
}
