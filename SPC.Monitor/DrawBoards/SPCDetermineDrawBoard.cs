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
    public partial class SPCDetermineDrawBoard : DevChartDrawBoard
    {
        public SPCDetermineDrawBoard()
        {
            InitializeComponent();
            this.mainChart = chartControl1;
            this.baseSeriesCount = 2;
            var secondY = (this.chartControl1.Diagram as DevExpress.XtraCharts.XYDiagram2D).GetAllAxesY()[1];
            secondY.CustomLabels.Clear();
            var commands = SPCCommand.GetCommandArray();
            foreach(var command in commands)
            {
                secondY.CustomLabels.Add(new DevExpress.XtraCharts.CustomAxisLabel(command.Title, command.ID.ToString()));
            }
        }
    }
}
