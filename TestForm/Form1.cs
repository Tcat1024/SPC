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

        private void cpKtoolControl1_Load(object sender, EventArgs e)
        {

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
            (this.cpKtoolControl1.DataView as SPC.Base.Control.CanChooseDataGridView).Synchronize(this.monitorControl1.DataView as SPC.Base.Control.CanChooseDataGridView, DevExpress.XtraGrid.Views.Base.SynchronizationMode.Full);
            this.xyRelationControl1.DataSource = this.Data;
            (this.xyRelationControl1.DataView as SPC.Base.Control.CanChooseDataGridView).Synchronize(this.monitorControl1.DataView as SPC.Base.Control.CanChooseDataGridView, DevExpress.XtraGrid.Views.Base.SynchronizationMode.Full);
            this.determineControl1.DataSource = this.Data;
            (this.determineControl1.DataView as SPC.Base.Control.CanChooseDataGridView).Synchronize(this.monitorControl1.DataView as SPC.Base.Control.CanChooseDataGridView, DevExpress.XtraGrid.Views.Base.SynchronizationMode.Full);
        }
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var data = new SPC.Base.Interface.ChoosedData(this.Data,(this.monitorControl1.DataView as SPC.Base.Control.CanChooseDataGridView).GetChoosedRowIndexs());
            var columns =data.GetColumnsList(false,typeof(DateTime),typeof(string),typeof(bool));
            Form configform = new Form();
            var con = new SPC.Analysis.ConfigControls.CCTConfigControl(){Dock = DockStyle.Fill};
            configform.Controls.Add(con);
            con.OKEvent += (ss, ee) => { configform.DialogResult = System.Windows.Forms.DialogResult.OK; };
            con.CancelEvent += (ss, ee) => { configform.DialogResult = System.Windows.Forms.DialogResult.Cancel; };
            con.Init(columns);
            if(configform.ShowDialog()==DialogResult.OK)
            {
                Form resultform = new Form();
                var res = new SPC.Analysis.ResultControls.CCTResultControl() { Dock = DockStyle.Fill };
                resultform.Controls.Add(res);
                res.Init(con.Columns, SPC.Algorithm.Relations.GetCCTs(data, con.TargetColumn, con.Columns, 0));
                resultform.Show();
            }
        }


    }
}
