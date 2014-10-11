using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SPC.Base.Control
{
    public partial class MultiControlsVerticalLayout : DevExpress.XtraEditors.XtraScrollableControl
    {
        private int AvgHeight;
        private int LastHeight;
        private int _SizeChangeStep = 50;
        private bool IsScrolled = false;
        public int SizeChangeStep
        {
            get
            {
                return this._SizeChangeStep;
            }
            set
            {
                if (value > 0)
                    this._SizeChangeStep = value;
            }
        }
        public MultiControlsVerticalLayout()
            : base()
        {
            this.LastHeight = this.Height;
            this.AutoScroll = false;
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.Size.Height != this.LastHeight)
            {
                if (!this.IsScrolled)
                {
                    this.InitAvgHeight();
                }
                this.LastHeight = this.Height;
            }
        }
        protected override void OnMouseWheelCore(System.Windows.Forms.MouseEventArgs ev)
        {
            if (ev.Delta > 0)
            {
                int temp = this.AvgHeight;
                this.AvgHeight += this.SizeChangeStep;
                if (this.AvgHeight > this.Height)
                    this.AvgHeight = this.Height;
                if (temp != this.AvgHeight)
                {
                    this.AutoScroll = true;
                    this.ResizeControls();
                }
            }
            else if (ev.Delta < 0 && this.VerticalScroll.Visible)
            {
                this.AvgHeight -= this.SizeChangeStep;
                int min = this.Size.Height / (this.Controls.Count > 0 ? this.Controls.Count : 1);
                if (this.AvgHeight < min)
                    this.AvgHeight = min;
                this.ResizeControls();
                if (!this.VerticalScroll.Visible)
                    this.AutoScroll = false;
            }
            this.IsScrolled = this.VerticalScroll.Visible;
        }
        protected override void OnControlRemoved(System.Windows.Forms.ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            if (!this.IsScrolled)
            {
                this.InitAvgHeight();
            }
            else if (!this.VerticalScroll.Visible)
            {
                this.IsScrolled = false;
                this.AutoScroll = false;
                this.InitAvgHeight();
            }
        }
        protected override void OnControlAdded(System.Windows.Forms.ControlEventArgs e)
        {
            base.OnControlAdded(e);
            e.Control.Dock = System.Windows.Forms.DockStyle.Top;
            if (!this.IsScrolled)
            {
                this.InitAvgHeight();
            }
            else
            {
                e.Control.Height = this.AvgHeight;
            }
        }

        private void InitAvgHeight()
        {
            this.AvgHeight = this.Height / (this.Controls.Count > 0 ? this.Controls.Count : 1);
            ResizeControls();
        }
        private void ResizeControls()
        {
            
            this.SuspendLayout();
            for (int i = 0; i < this.Controls.Count; i++)
            {
                this.Controls[i].Height = this.AvgHeight;
            }
            this.ResumeLayout(true);
        }

    }
}
