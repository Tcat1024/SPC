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
    public partial class DataControlDrawBoard :DevExpress.XtraEditors.XtraUserControl,IDrawBoard<DevExpress.XtraCharts.ChartControl>
    {
        public DataControlDrawBoard()
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
