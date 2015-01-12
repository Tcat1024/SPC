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
    public partial class NormalityCheckDrawBoard : DevChartDrawBoard
    {
        public NormalityCheckDrawBoard()
        {
            InitializeComponent();
            this.mainChart = chartControl1;
            this.baseSeriesCount = 1;
        }
    }
}
