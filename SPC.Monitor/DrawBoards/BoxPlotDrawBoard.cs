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
    public partial class BoxPlotDrawBoard : DevExpress.XtraEditors.XtraUserControl,IDrawBoard<DevExpress.XtraCharts.ChartControl>
    {
        public BoxPlotDrawBoard()
        {
            InitializeComponent();
        }
        public DevExpress.XtraCharts.ChartControl GetChart()
        {
            return this.chartControl1;
        }
        public bool CheckCanRemove()
        {
            if(this.GetChart().Series.Count==4)
            {
                return true;
            }
            return false;
        }
    }
}
