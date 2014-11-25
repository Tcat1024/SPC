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
    public partial class SampleDataRunPointDrawBoard : DevExpress.XtraEditors.XtraUserControl, IDrawBoard<DevExpress.XtraCharts.ChartControl>
    {
        public SampleDataRunPointDrawBoard()
        {
            InitializeComponent();
        }
        public DevExpress.XtraCharts.ChartControl GetChart()
        {
            return this.chartControl1;
        }
        public bool CheckCanRemove()
        {
            if (this.GetChart().Series.Count == 1)
                return true;
            return false;
        }
    }
}
