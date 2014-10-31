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
    public partial class NormalityCheckDrawBoard : DevExpress.XtraEditors.XtraUserControl,IDrawBoard
    {
        public NormalityCheckDrawBoard()
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
