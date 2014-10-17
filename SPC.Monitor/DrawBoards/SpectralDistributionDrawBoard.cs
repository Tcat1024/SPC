using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SPC.Monitor.DrawBoards
{
    public partial class SpectralDistributionDrawBoard : DevExpress.XtraEditors.XtraUserControl,IDrawBoard
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
        protected override void OnControlRemoved(ControlEventArgs e)
        {
            if(this.Parent!=null)
                this.Parent.Controls.Remove(this);
        }

        private void chartControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button==MouseButtons.Right)
            {
                this.popupMenu1.ShowPopup(MousePosition);
            }
        }

        private void repositoryItemCheckEdit1_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if((bool)e.NewValue)
            {
                foreach(var s in chartControl1.Series)
                {
                    (s as DevExpress.XtraCharts.Series).LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                }
            }
            else
            {
                foreach (var s in chartControl1.Series)
                {
                    (s as DevExpress.XtraCharts.Series).LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;
                }
            }
        }


    }
}
