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
    public partial class SPCDetermineDrawBoard : DevExpress.XtraEditors.XtraUserControl, IDrawBoard
    {
        public SPCDetermineDrawBoard()
        {
            InitializeComponent();
            var secondY = (this.chartControl1.Diagram as DevExpress.XtraCharts.XYDiagram2D).GetAllAxesY()[1];
            secondY.CustomLabels.Clear();
            var commands = SPCCommand.GetCommandArray();
            foreach(var command in commands)
            {
                secondY.CustomLabels.Add(new DevExpress.XtraCharts.CustomAxisLabel(command.Title, command.ID.ToString()));
            }
        }
        public Control GetChart()
        {
            return this.chartControl1;
        }
        protected override void OnControlRemoved(ControlEventArgs e)
        {
            if (this.Parent != null)
                this.Parent.Controls.Remove(this);
        }
    }
}
