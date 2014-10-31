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
    public partial class AvgDataRunDrawBoard : DevExpress.XtraEditors.XtraUserControl, IDrawBoard
    {
        public AvgDataRunDrawBoard()
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
    }
}
