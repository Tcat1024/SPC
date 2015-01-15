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
    public partial class BoxPlotDrawBoard : DevChartDrawBoard
    {
        public BoxPlotDrawBoard()
        {
            InitializeComponent();
            this.mainChart = chartControl1;
        }
    }
}
