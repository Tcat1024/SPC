using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EAS.Services;

namespace TestForm
{
    public partial class Form1 : Form
    {
        QtDataTrace.UI.CPKtool.DebugDataSelectForm DataForm;
        DataTable Data;
        public Form1()
        {
            InitializeComponent();
            DataForm = new QtDataTrace.UI.CPKtool.DebugDataSelectForm();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.DataForm.ShowDialog() == DialogResult.Yes)
            {
                try
                {
                    this.Data = ServiceContainer.GetService<SPC.Web.Interface.ICPKtool>().DebugGetTable(this.DataForm.command, this.DataForm.conStr);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                //this.cpKtoolControl1.DataSource = this.Data;
                this.monitorControl1.DataSource = this.Data;

            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.cpKtoolControl1.DataSource = this.Data;
            (this.cpKtoolControl1.DataView as SPC.Controls.Base.CanChooseDataGridView).Synchronize(this.monitorControl1.DataView as SPC.Controls.Base.CanChooseDataGridView, DevExpress.XtraGrid.Views.Base.SynchronizationMode.Full);
            //this.xyRelationControl1.DataSource = this.Data;
            //(this.xyRelationControl1.DataView as SPC.Controls.Base.CanChooseDataGridView).Synchronize(this.monitorControl1.DataView as SPC.Controls.Base.CanChooseDataGridView, DevExpress.XtraGrid.Views.Base.SynchronizationMode.Full);
            this.determineControl1.DataSource = this.Data;
            (this.determineControl1.DataView as SPC.Controls.Base.CanChooseDataGridView).Synchronize(this.monitorControl1.DataView as SPC.Controls.Base.CanChooseDataGridView, DevExpress.XtraGrid.Views.Base.SynchronizationMode.Full);
        }
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var data = new SPC.Base.Interface.ChoosedData(this.Data,(this.monitorControl1.DataView as SPC.Controls.Base.CanChooseDataGridView).GetChoosedRowIndexs());
            var columns =data.GetColumnsList(false,typeof(DateTime),typeof(string),typeof(bool));
            Form configform = new Form();
            var con = new SPC.Controls.ConfigControls.CCTConfigControl(){Dock = DockStyle.Fill};
            configform.Controls.Add(con);
            con.OKEvent += (ss, ee) => { configform.DialogResult = System.Windows.Forms.DialogResult.OK; };
            con.CancelEvent += (ss, ee) => { configform.DialogResult = System.Windows.Forms.DialogResult.Cancel; };
            con.Init(columns);
            if(configform.ShowDialog()==DialogResult.OK)
            {
                Form resultform = new Form();
                var res = new SPC.Controls.ResultControls.CCTResultControl() { Dock = DockStyle.Fill };
                resultform.Controls.Add(res);
                res.Init(con.Columns, SPC.Algorithm.Relations.GetCCTs(data, con.TargetColumn, con.Columns, new SPC.Base.Interface.WaitObject()));
                resultform.Show();
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var data = new SPC.Base.Interface.ChoosedData(this.Data, (this.monitorControl1.DataView as SPC.Controls.Base.CanChooseDataGridView).GetChoosedRowIndexs());
            var columns = data.GetColumnsList(false, typeof(DateTime), typeof(string), typeof(bool));
            Form configform = new Form();
            var con = new SPC.Controls.ConfigControls.ContourPlotConfigControl() { Dock = DockStyle.Fill };
            configform.Controls.Add(con);
            con.OKEvent += (ss, ee) => { configform.DialogResult = System.Windows.Forms.DialogResult.OK; };
            con.CancelEvent += (ss, ee) => { configform.DialogResult = System.Windows.Forms.DialogResult.Cancel; };
            con.Init(columns);
            if (configform.ShowDialog() == DialogResult.OK)
            {
                Form resultform = new Form();
                var res = new SPC.Controls.ResultControls.ContourPlotResultControl() { Dock = DockStyle.Fill };
                resultform.Controls.Add(res);
                res.Init(SPC.Rnet.Methods.DrawContourPlot(data,con.X,con.Y,con.Z,con.PicWidth,con.PicHeight,con.Levels,con.IsDrawLine),con.X,con.Y,con.Z);
                resultform.Show();
            }
        }
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var re = SPC.Rnet.Methods.RunScript("111.txt", new List<object>() {2,10 });
            MessageBox.Show(re[0].ToString());
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
                        var data = new SPC.Base.Interface.ChoosedData(this.Data,(this.monitorControl1.DataView as SPC.Controls.Base.CanChooseDataGridView).GetChoosedRowIndexs());
            var columns =data.GetColumnsList(false,typeof(DateTime),typeof(string),typeof(bool));
            var con = new SPC.Controls.ConfigControls.ColumnCalculateConfigControl();
            Form configform  = new Form();
configform.Controls.Add(con);
            con.OKEvent += (ss, ee) => { configform.DialogResult = System.Windows.Forms.DialogResult.OK; };
            con.CancelEvent += (ss, ee) => { configform.DialogResult = System.Windows.Forms.DialogResult.Cancel; };
            con.Init(columns);
            double[] result = null;
            if (configform.ShowDialog() == DialogResult.OK)
            {
                switch (con.MethodIndex)
                {
                    case 0:
                        result = SPC.Base.Operation.ColumnCalculate.Sum(data, con.SourceColumns); break;
                    case 1:
                        result = SPC.Base.Operation.ColumnCalculate.Avg(data, con.SourceColumns); break;
                    case 2:
                        result = SPC.Base.Operation.ColumnCalculate.Stdev(data, con.SourceColumns); break;
                    case 3:
                        result = SPC.Base.Operation.ColumnCalculate.Min(data, con.SourceColumns); break;
                    case 4:
                        result = SPC.Base.Operation.ColumnCalculate.Max(data, con.SourceColumns); break;
                    case 5:
                        result = SPC.Base.Operation.ColumnCalculate.Range(data, con.SourceColumns); break;
                    case 6:
                        result = SPC.Base.Operation.ColumnCalculate.Mid(data, con.SourceColumns); break;
                    case 7:
                        result = SPC.Base.Operation.ColumnCalculate.QuadraticSum(data, con.SourceColumns); break;
                    case 8:
                        result = SPC.Base.Operation.ColumnCalculate.Count(data, con.SourceColumns); break;
                    case 9:
                        result = SPC.Base.Operation.ColumnCalculate.IsNotNull(data, con.SourceColumns); break;
                    case 10:
                        result = SPC.Base.Operation.ColumnCalculate.IsNull(data, con.SourceColumns); break;
                    default:
                        MessageBox.Show("不支持的方法");
                        return;
                }
                DataTable resulttable = new DataTable();
                resulttable.Columns.Add("列名", typeof(string));
                resulttable.Columns.Add(con.MethodString, typeof(double));
                int count = result.Length;
                for (int i = 0; i < count; i++)
                {
                    resulttable.Rows.Add(con.SourceColumns[i], result[i]);
                }
                Form resultform = new Form();
                var res = new SPC.Controls.ResultControls.CalculateResultControl() { Dock = DockStyle.Fill };
                resultform.Controls.Add(res);
                res.Init(resulttable);
                resultform.Show();
            }
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var data = new SPC.Base.Interface.ChoosedData(this.Data, (this.monitorControl1.DataView as SPC.Controls.Base.CanChooseDataGridView).GetChoosedRowIndexs());
            var columns = data.GetColumnsList(false, typeof(DateTime));
            Form configform = new Form();
            var con = new SPC.Controls.ConfigControls.RpartConfigControl() { Dock = DockStyle.Fill };
            configform.Controls.Add(con);
            con.OKEvent += (ss, ee) => { configform.DialogResult = System.Windows.Forms.DialogResult.OK; };
            con.CancelEvent += (ss, ee) => { configform.DialogResult = System.Windows.Forms.DialogResult.Cancel; };
            con.Init(columns);
            if (configform.ShowDialog() == DialogResult.OK)
            {
                Form resultform = new Form();
                var res = new SPC.Controls.ResultControls.RpartResultControl() { Dock = DockStyle.Fill };
                resultform.Controls.Add(res);
                var result = SPC.Rnet.Methods.Rpart(data,con.PicWidth,con.PicHeight, con.TargetColumn, con.Columns,con.Method,con.CP);
                res.Init(result.Item1,result.Item2,result.Item3);
                resultform.Show();
            }
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var data = new SPC.Base.Interface.ChoosedData(this.Data, (this.monitorControl1.DataView as SPC.Controls.Base.CanChooseDataGridView).GetChoosedRowIndexs());
            var columns = data.GetColumnsList(false, typeof(DateTime),typeof(bool),typeof(string));
            Form configform = new Form();
            var con = new SPC.Controls.ConfigControls.LmregressConfigControl() { Dock = DockStyle.Fill };
            configform.Controls.Add(con);
            con.OKEvent += (ss, ee) => { configform.DialogResult = System.Windows.Forms.DialogResult.OK; };
            con.CancelEvent += (ss, ee) => { configform.DialogResult = System.Windows.Forms.DialogResult.Cancel; };
            con.Init(columns);
            if (configform.ShowDialog() == DialogResult.OK)
            {
                Form resultform = new Form();
                var res = new SPC.Controls.ResultControls.LmregressResultControl() { Dock = DockStyle.Fill };
                resultform.Controls.Add(res);
                var result = SPC.Rnet.Methods.LmGress(data, con.PicWidth, con.PicHeight, con.TargetColumn, con.Columns);
                res.Init(result.Item1, result.Item2, result.Item3,con.Columns);
                resultform.Show();
            }
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var data = new SPC.Base.Interface.ChoosedData(this.Data, (this.monitorControl1.DataView as SPC.Controls.Base.CanChooseDataGridView).GetChoosedRowIndexs());
            Form configform = new Form();
            var con = new SPC.Controls.ConfigControls.EngineConfigControl() { Dock = DockStyle.Fill };
            configform.Controls.Add(con);
            con.OKEvent += (ss, ee) => { configform.DialogResult = System.Windows.Forms.DialogResult.OK; };
            con.CancelEvent += (ss, ee) => { configform.DialogResult = System.Windows.Forms.DialogResult.Cancel; };
            con.Init(data);
            if (configform.ShowDialog() == DialogResult.OK)
            {
                con.doMethod();
            }
        }
    }
}
