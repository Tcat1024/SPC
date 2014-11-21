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


    }
}
