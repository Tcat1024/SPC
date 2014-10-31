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
    public partial class DataControlDrawBoard : DevExpress.XtraEditors.XtraUserControl,IDrawBoard
    {
        public DataControlDrawBoard()
        {
            InitializeComponent();
        }

        public Control GetChart()
        {
            return this.chartControl1;
        }
        protected override void OnControlRemoved(ControlEventArgs e)
        {
            this.Parent.Controls.Remove(this);
        }

        private void chartControl1_CustomDrawCrosshair(object sender, DevExpress.XtraCharts.CustomDrawCrosshairEventArgs e)
        {
        }
        public class SeriesPointClickEventArgs:EventArgs
        {
            public double x;
            public double y;
        }
    }
}
