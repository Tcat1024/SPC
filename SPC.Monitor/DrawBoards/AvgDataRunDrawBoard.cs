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
    public partial class AvgDataRunDrawBoard : DevChartDrawBoard
    {
        public AvgDataRunDrawBoard()
        {
            InitializeComponent();
            this.mainChart = chartControl1;
        }

    }
}
