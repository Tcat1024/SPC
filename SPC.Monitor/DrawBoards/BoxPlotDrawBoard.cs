using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SPC.Monitor.DrawBoards
{
    public partial class BoxPlotDrawBoard : DevExpress.XtraEditors.XtraUserControl, IDrawBoard
    {
        public BoxPlotDrawBoard()
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
            //foreach(var g in e.CrosshairElements)
            //{
            //    g.LabelElement.Text = "test";
            //}
        }

    }
}
