using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using DevExpress.XtraEditors;

namespace SPC.Analysis.ResultControls
{
    public partial class KMeansResultControl : DevExpress.XtraEditors.XtraUserControl
    {
        private DataSet Result;
        private SPC.Base.Interface.IDataTable<DataRow> Data;
        public KMeansResultControl()
        {
            InitializeComponent();
        }
        public void Init(DataSet result,SPC.Base.Interface.IDataTable<DataRow> data)
        {
            this.Result = result;
            this.Data = data;
            this.gridControl2.DataSource = Result;
            this.gridControl2.DataMember = "Results";
            this.gridControl1.DataSource = Result;
            this.gridControl1.DataMember = "OverView";
            this.gridView2.Columns.ColumnByFieldName("序号").Visible = false;
            this.gridView2.GroupSummary.Clear();
            this.gridView2.GroupSummary.Add(new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Count, this.gridView2.Columns[0].FieldName, null, "Count:{0}"));
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow temp;
            if((temp =gridView1.GetDataRow(e.FocusedRowHandle))!=null)
            (this.gridView2.DataSource as DataView).RowFilter = "序号 = "+temp[0].ToString();
        }

        private void buttonEdit1_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit target;
            string column;
            if ((target = sender as DevExpress.XtraEditors.ButtonEdit) != null && (column = target.Text.Trim()) != "")
            {
                if (Data.ContainsColumn(column))
                {
                    if ((MessageBox.Show("原表中已有列" + column + "，是否要覆盖？", "注意", MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        int rowcount = (this.gridView2.DataSource as DataView).Count;
                        for (int i = 0; i < rowcount; i++)
                            Data[i, column] = (this.gridView2.DataSource as DataView)[i]["类标号"];
                        Data.SetColumnVisible(column);
                    }
                    else
                        return;
                }
                else
                {
                    Data.AddColumn(column, typeof(int));
                    int rowcount = (this.gridView2.DataSource as DataView).Count;
                    for (int i = 0; i < rowcount; i++)
                        Data[i, column] = (this.gridView2.DataSource as DataView)[i]["类标号"];
                }
                MessageBox.Show("添加成功");
            }
        }



    }
}