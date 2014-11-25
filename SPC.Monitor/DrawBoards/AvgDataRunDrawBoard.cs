using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SPC.Base.Interface;

namespace SPC.Monitor.DrawBoards
{
    public partial class AvgDataRunDrawBoard :DevExpress.XtraEditors.XtraUserControl,IDrawBoard<DevExpress.XtraCharts.ChartControl>
    {
        public AvgDataRunDrawBoard()
        {
            InitializeComponent();
        }
        public DevExpress.XtraCharts.ChartControl GetChart()
        {
            return this.chartControl1;
        }
        public bool CheckCanRemove()
        {
            if (this.GetChart().Series.Count == 2)
                return true;
            return false;
        }
    }
}
