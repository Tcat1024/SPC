using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using SPC.Base.Interface;

namespace SPC.Monitor.DrawBoards
{
    public partial class SpectralDistributionDrawBoard : DevExpress.XtraEditors.XtraUserControl, IDrawBoard<DevExpress.XtraCharts.ChartControl>
    {
        public SpectralDistributionDrawBoard()
        {
            InitializeComponent();
            this.barEditItem1.EditValue = true;
        }
        public DevExpress.XtraCharts.ChartControl GetChart()
        {
            return this.chartControl1;
        }

        private void repositoryItemCheckEdit1_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                foreach (var s in chartControl1.Series)
                {
                    var ss = (s as DevExpress.XtraCharts.Series);
                    if (ss.View is DevExpress.XtraCharts.StepAreaSeriesView||ss.View is DevExpress.XtraCharts.StepLineSeriesView)
                        ss.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                }
            }
            else
            {
                foreach (var s in chartControl1.Series)
                {
                    var ss = (s as DevExpress.XtraCharts.Series);
                    if (ss.View is DevExpress.XtraCharts.StepAreaSeriesView || ss.View is DevExpress.XtraCharts.StepLineSeriesView)
                        ss.LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;
                }
            }
        }

        private void chartControl1_CustomShowRightClickPopupMenu(object sender, Base.Control.AdvChartControl.ShowRightClickPopupMenuEventArgs e)
        {
            e.RightClickPopupMenu.AddItem(this.barEditItem1);
            e.RightClickPopupMenu.AddItem(this.barEditItem2);
            e.Handle = true;
        }

        private void repositoryItemToggleSwitch1_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                foreach (DevExpress.XtraCharts.Series s in this.chartControl1.Series)
                {
                    if ((s as DevExpress.XtraCharts.Series).View is DevExpress.XtraCharts.StepAreaSeriesView)
                    {
                        s.ChangeView(DevExpress.XtraCharts.ViewType.StepLine);
                    }
                }
                this.barEditItem2.Caption = "描线";
            }
            else
            {
                foreach (DevExpress.XtraCharts.Series s in this.chartControl1.Series)
                {
                    if ((s as DevExpress.XtraCharts.Series).View is DevExpress.XtraCharts.StepLineSeriesView)
                    {
                        s.ChangeView(DevExpress.XtraCharts.ViewType.StepArea);
                    }
                }
                this.barEditItem2.Caption = "填充";
            }
        }
        public bool CheckCanRemove()
        {
            if (this.GetChart().Series.Count == 3)
                return true;
            return false;
        }
    }
}
