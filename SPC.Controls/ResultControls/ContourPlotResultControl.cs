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

    public partial class ContourPlotResultControl : DevExpress.XtraEditors.XtraUserControl
    {        
        GlobalMouseHandler handler = new GlobalMouseHandler();
        enum ResizeMode
        {
            H,V,B,N
        }
        ResizeMode resizeMode = ResizeMode.N;
        public ContourPlotResultControl()
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
            var point = PointToClient(MousePosition);
            switch (this.resizeMode)
            {
                case ResizeMode.N:
                    int i = 0;
                    var cur = Cursors.Arrow;
                    if (Math.Abs(point.X - (this.ChildPanelControl.Location.X + this.ChildPanelControl.Width)) < 5)
                    {
                        cur = Cursors.SizeWE;
                        i++;
                    }
                    if (Math.Abs(point.Y - (this.ChildPanelControl.Location.Y + this.ChildPanelControl.Height)) < 5)
                    {
                        cur = Cursors.SizeNS;
                        i++;
                    }
                    if (i == 2)
                        cur = Cursors.SizeNWSE;
                    this.Cursor = cur;
                    break;
                case ResizeMode.H:
                    int newWidth = point.X - this.ChildPanelControl.Location.X;
                    this.ChildPanelControl.Width = newWidth > 10 ? newWidth : 10;
                    break;
                case ResizeMode.V:
                    int newHeight = point.Y - this.ChildPanelControl.Location.Y;
                    this.ChildPanelControl.Height = newHeight > 10 ? newHeight : 10;
                    break;
                case ResizeMode.B:
                    newWidth = point.X - this.ChildPanelControl.Location.X;
                    this.ChildPanelControl.Width = newWidth > 10 ? newWidth : 10;
                    newHeight = point.Y - this.ChildPanelControl.Location.Y;
                    this.ChildPanelControl.Height = newHeight > 10 ? newHeight : 10;
                    break;
            }
        }
        public void Init(Image pic,string xn,string yn,string zn)
        {
            this.pictureEdit1.Image = pic;
            this.labelControl1.Text = string.Format("X轴: {0}    Y轴: {1}    Z轴: {2}", xn, yn,zn);
            this.ChildPanelControl.Height = pic.Size.Height+labelControl1.Height;
            this.ChildPanelControl.Width = Math.Max(pic.Size.Width, labelControl1.Width);
            this.labelControl1.Location = new Point((this.ChildPanelControl.Width - this.labelControl1.Width) / 2, this.labelControl1.Location.Y);
        }

        private void pictureEdit1_Enter(object sender, EventArgs e)
        {
            this.ChildPanelControl.Focus();
        }

        private void panelControl2_Enter(object sender, EventArgs e)
        {
            this.ChildPanelControl.Focus();
        }

        private void panelControl2_Resize(object sender, EventArgs e)
        {
            this.labelControl1.Location = new Point((this.ChildPanelControl.Width - this.labelControl1.Width) / 2, this.labelControl1.Location.Y);
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
    public class GlobalMouseHandler : IMessageFilter
    {
        public event EventHandler<MouseEventArgGlobal> MouseMoveGlobal;
        public event EventHandler<MouseEventArgGlobal> MouseLeftDownGlobal;
        public event EventHandler<MouseEventArgGlobal> MouseLeftUpGlobal;
        public bool PreFilterMessage(ref Message m)
        {
            switch (m.Msg)
            {
                case 512:
                    if (MouseMoveGlobal != null)
                        MouseMoveGlobal(this, new MouseEventArgGlobal(m));
                    break;
                case 513:
                    if (MouseLeftDownGlobal != null)
                        MouseLeftDownGlobal(this, new MouseEventArgGlobal(m));
                    break;
                case 514:
                    if (MouseLeftUpGlobal != null)
                        MouseLeftUpGlobal(this, new MouseEventArgGlobal(m));
                    break;
            }
            return false;
        }
    }
    public class MouseEventArgGlobal : EventArgs
    {
        public Message message;
        public MouseEventArgGlobal(Message m)
        {
            message = m;
        }
    }
}
