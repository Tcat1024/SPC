using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SPC.Controls.ResultControls
{

    public partial class LmregressResultControl : DevExpress.XtraEditors.XtraUserControl
    {
        GlobalMouseHandler handler = new GlobalMouseHandler();
        enum ResizeMode
        {
            H, V, B, N
        }
        ResizeMode resizeMode = ResizeMode.N;
        public LmregressResultControl()
        {
            InitializeComponent();
            handler.MouseMoveGlobal += handler_MouseMoveGlobal;
            handler.MouseLeftDownGlobal += handler_MouseLeftDownGlobal;
            handler.MouseLeftUpGlobal += handler_MouseLeftUpGlobal;
        }
        void handler_MouseLeftUpGlobal(object sender, MouseEventArgGlobal e)
        {
            if (!this.ContainsFocus)
                return;
            this.resizeMode = ResizeMode.N;
        }

        void handler_MouseLeftDownGlobal(object sender, MouseEventArgGlobal e)
        {
            if (!this.ContainsFocus)
                return;
            if (this.Cursor == Cursors.SizeWE)
                this.resizeMode = ResizeMode.H;
            else if (this.Cursor == Cursors.SizeNS)
                this.resizeMode = ResizeMode.V;
            else if (this.Cursor == Cursors.SizeNWSE)
                this.resizeMode = ResizeMode.B;
            else
                this.resizeMode = ResizeMode.N;
        }

        void handler_MouseMoveGlobal(object sender, MouseEventArgGlobal e)
        {
            if (!this.ContainsFocus)
                return;
            var point = this.ChildPanelControl.PointToClient(MousePosition);
            switch (this.resizeMode)
            {
                case ResizeMode.N:
                    int i = 0;
                    var cur = Cursors.Arrow;
                    if (Math.Abs(point.X - this.ChildPanelControl.Width) < 5 && point.Y < this.ChildPanelControl.Height+5&&point.Y>0)
                    {
                        cur = Cursors.SizeWE;
                        i++;
                    }
                    if (Math.Abs(point.Y - this.ChildPanelControl.Height) < 5&&point.X<this.ChildPanelControl.Width+5&&point.X>0)
                    {
                        cur = Cursors.SizeNS;
                        i++;
                    }
                    if (i == 2)
                        cur = Cursors.SizeNWSE;
                    this.Cursor = cur;
                    break;
                case ResizeMode.H:
                    int newWidth = point.X;
                    this.ChildPanelControl.Width = newWidth > 10 ? newWidth : 10;
                    break;
                case ResizeMode.V:
                    int newHeight = point.Y;
                    this.ChildPanelControl.Height = newHeight > 10 ? newHeight : 10;
                    break;
                case ResizeMode.B:
                    newWidth = point.X;
                    this.ChildPanelControl.Width = newWidth > 10 ? newWidth : 10;
                    newHeight = point.Y;
                    this.ChildPanelControl.Height = newHeight > 10 ? newHeight : 10;
                    break;
            }
        }
        public void Init(Image pic, string input, double[] coefficients, string[] columns)
        {
            this.memoEdit1.Text = input;
            this.pictureEdit1.Image = pic;
            DataTable table = new DataTable();
            table.Columns.Add("param");
            table.Columns.Add("coefficients");
            int length = coefficients.Length;
            if (columns.Length > length-2)
            {
                table.Rows.Add("截距", coefficients[0]);
                for (int i = 1; i < length; i++)
                {
                    table.Rows.Add(columns[i - 1], coefficients[i]);
                }              
            }
            this.gridControl1.DataSource = table;
        }

        private void pictureEdit1_Enter(object sender, EventArgs e)
        {
            this.ChildPanelControl.Focus();
        }

        private void ContourPlotResultControl_Validated(object sender, EventArgs e)
        {
            Application.RemoveMessageFilter(handler);
        }

        private void ContourPlotResultControl_Enter(object sender, EventArgs e)
        {
            Application.RemoveMessageFilter(handler);
            Application.AddMessageFilter(handler);
        }
       
    }

}
