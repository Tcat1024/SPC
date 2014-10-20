using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SPC.Base.Control
{
    public partial class AdvChartControl : DevExpress.XtraCharts.ChartControl
    {
        private DevExpress.XtraCharts.ConstantLine _targetLine = null;
        private DevExpress.XtraCharts.ConstantLine targetLine
        {
            get
            {
                return this._targetLine;
            }
            set
            {
                if (value == null)
                    this.Cursor = System.Windows.Forms.Cursors.Arrow;
                else if (value.Name == "X")
                    this.Cursor = System.Windows.Forms.Cursors.SizeWE;
                else if (value.Name == "Y")
                    this.Cursor = System.Windows.Forms.Cursors.SizeNS;
                this._targetLine = value;
            }
        }
        private DevExpress.XtraCharts.CrosshairElement currentElement;
        private DevExpress.XtraBars.PopupMenu RightClickPopupMenu = new DevExpress.XtraBars.PopupMenu();
        private DevExpress.XtraBars.BarEditItem popupMenuEditItem = new DevExpress.XtraBars.BarEditItem();
        private DevExpress.XtraBars.BarButtonItem popupMenuButtonItem = new DevExpress.XtraBars.BarButtonItem();
        private DevExpress.XtraCharts.ConstantLine upLine = new DevExpress.XtraCharts.ConstantLine() {Name = "Y",Visible = false};
        private DevExpress.XtraCharts.ConstantLine downLine = new DevExpress.XtraCharts.ConstantLine() { Name = "Y", Visible = false };
        private DevExpress.XtraCharts.ConstantLine leftLine = new DevExpress.XtraCharts.ConstantLine() { Name = "X", Visible = false };
        private DevExpress.XtraCharts.ConstantLine rightLine = new DevExpress.XtraCharts.ConstantLine() { Name = "Y", Visible = false };
        private DevExpress.XtraCharts.Strip XStrip = new DevExpress.XtraCharts.Strip() { Visible = false};
        private DevExpress.XtraCharts.Strip YStrip = new DevExpress.XtraCharts.Strip() { Visible = false };

        public event EventHandler<ShowRightClickPopupMenuEventArgs> CustomShowRightClickPopupMenu;
        public class ShowRightClickPopupMenuEventArgs:EventArgs
        {
            public DevExpress.XtraBars.PopupMenu RightClickPopupMenu { get; private set; }
            public DevExpress.XtraCharts.ConstantLine ConstantLine { get; private set; }
            public bool Handle;
            public ShowRightClickPopupMenuEventArgs(DevExpress.XtraBars.PopupMenu menu,DevExpress.XtraCharts.ConstantLine line,bool handle)
            {
                this.RightClickPopupMenu = menu;
                this.ConstantLine = line;
                this.Handle = handle;
            
            }
        }
        public AdvChartControl()
        {
            InitializeComponent();
            InitConstantLines();
            InitPopupMenu();
        }
        private void InitPopupMenu()
        {
            this.RightClickPopupMenu.ClearLinks();
        }
        private void InitConstantLines()
        {
            (this.Diagram as DevExpress.XtraCharts.XYDiagram).AxisX.ConstantLines.Add(leftLine);
            (this.Diagram as DevExpress.XtraCharts.XYDiagram).AxisX.ConstantLines.Add(rightLine);
            (this.Diagram as DevExpress.XtraCharts.XYDiagram).AxisY.ConstantLines.Add(upLine);
            (this.Diagram as DevExpress.XtraCharts.XYDiagram).AxisY.ConstantLines.Add(downLine);
            (this.Diagram as DevExpress.XtraCharts.XYDiagram).AxisX.Strips.Add(XStrip);
            (this.Diagram as DevExpress.XtraCharts.XYDiagram).AxisY.Strips.Add(YStrip);
        }
        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            if (this.Series.Count > 0)
            {
                var info = this.CalcHitInfo(e.Location);
                if (info.Diagram != null)
                {
                    string x = this.currentElement.SeriesPoint.Argument;
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        if (info.InConstantLine && info.ConstantLine.Title.Visible)
                        {
                            this.targetLine = info.ConstantLine;
                        }
                        else if ( !leftLine.Visible&& !rightLine.Visible)
                        {
                            leftLine.AxisValue = x;
                            leftLine.Title.Text = leftLine.AxisValue.ToString();
                            this.targetLine = leftLine;
                            leftLine.Visible = true;
                        }
                        else if (!rightLine.Visible || !leftLine.Visible)
                        {
                            var temp = rightLine.Visible ? leftLine : rightLine;
                            temp.AxisValue = x;
                            temp.Title.Text = temp.AxisValue.ToString();
                            this.targetLine = temp;
                            temp.Visible = true;
                            RefreshXStrip();
                        }
                    }
                    else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (info.InConstantLine && info.ConstantLine.Visible && info.ConstantLine.Title.Visible)
                        {
                            this.RightClickPopupMenu.AddItem(popupMenuEditItem);
                            this.RightClickPopupMenu.AddItem(popupMenuButtonItem);
                            popupMenuEditItem.Caption = info.ConstantLine.Name + "轴边界";
                            popupMenuEditItem.EditValue = info.ConstantLine.AxisValue;
                            popupMenuEditItem.Tag = info.ConstantLine;
                            popupMenuButtonItem.Tag = info.ConstantLine;
                            popupMenuButtonItem.Caption = "删除" + info.ConstantLine.Name + "轴边界";
                            var eventarg = new ShowRightClickPopupMenuEventArgs(RightClickPopupMenu,info.ConstantLine,true);
                            CustomShowRightClickPopupMenu(RightClickPopupMenu,eventarg);
                            if(eventarg.Handle)
                                this.RightClickPopupMenu.ShowPopup(MousePosition);
                            InitPopupMenu();
                        }
                        else
                        {
                            var eventarg = new ShowRightClickPopupMenuEventArgs(RightClickPopupMenu, null, false);
                            CustomShowRightClickPopupMenu(RightClickPopupMenu, eventarg);
                            if (eventarg.Handle)
                                this.RightClickPopupMenu.ShowPopup(MousePosition);
                            InitPopupMenu();
                        }
                    }
                }
                else
                {
                    var yInfo = (this.Diagram as DevExpress.XtraCharts.XYDiagram).PointToDiagram(new System.Drawing.Point(35, e.Y));
                    if (yInfo.AxisY != null)
                    {
                        double y = yInfo.NumericalValue;
                        if (e.Button == System.Windows.Forms.MouseButtons.Left)
                        {
                            if (!downLine.Visible && !upLine.Visible)
                            {
                                downLine.AxisValue = y;
                                downLine.Title.Text = downLine.AxisValue.ToString();
                                this.targetLine = downLine;
                                downLine.Visible = true;
                            }
                            else if (!upLine.Visible || !downLine.Visible)
                            {
                                var temp = upLine.Visible ? downLine : upLine;
                                temp.AxisValue = y;
                                temp.Title.Text = temp.AxisValue.ToString();
                                this.targetLine = temp;
                                temp.Visible = true;
                                RefreshYStrip();
                            }
                        }
                    }
                }
            }
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left && this.targetLine != null)
            {
                var info = (this.Diagram as DevExpress.XtraCharts.XYDiagram).PointToDiagram(e.Location);
                if (targetLine.Name == "X" && !info.IsEmpty && targetLine.AxisValue.ToString() != this.currentElement.SeriesPoint.Argument)
                {
                    targetLine.AxisValue = this.currentElement.SeriesPoint.Argument;
                    targetLine.Title.Text = targetLine.AxisValue.ToString();
                    RefreshXStrip();
                }
                else if (targetLine.Name == "Y" && !info.IsEmpty && Convert.ToDouble(targetLine.AxisValue) != info.NumericalValue)
                {
                    targetLine.AxisValue = info.NumericalValue;
                    targetLine.Title.Text = targetLine.AxisValue.ToString();
                    RefreshYStrip();
                }
            }
            this.targetLine = null;
            base.OnMouseUp(e);
        }
        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            if (this.targetLine == null)
                this.Cursor = System.Windows.Forms.Cursors.Arrow;
            if (this.Series.Count > 0)
            {
                var info = this.CalcHitInfo(e.Location);
                if (info.ConstantLine != null && info.ConstantLine.Title.Visible && info.ConstantLine.Name == "X")
                {
                    this.Cursor = System.Windows.Forms.Cursors.SizeWE;
                }
                else if (info.ConstantLine != null && info.ConstantLine.Title.Visible && info.ConstantLine.Name == "Y")
                {
                    this.Cursor = System.Windows.Forms.Cursors.SizeNS;
                }
            }
            base.OnMouseMove(e);
        }
        private void RefreshXStrip()
        {
            if (leftLine.Visible && rightLine.Visible)
            {
                if (int.Parse(leftLine.AxisValue.ToString()) < int.Parse(rightLine.AxisValue.ToString()))
                {
                    XStrip.MaxLimit.AxisValue = rightLine.AxisValue;
                    XStrip.MinLimit.AxisValue = leftLine.AxisValue;
                }
                else
                {
                    XStrip.MaxLimit.AxisValue = leftLine.AxisValue;
                    XStrip.MinLimit.AxisValue = rightLine.AxisValue;
                }
                XStrip.Visible = true;
            }
            else
                XStrip.Visible = false;
        }
        private void RefreshYStrip()
        {
            if (downLine.Visible && upLine.Visible)
            {
                if (Convert.ToDouble(downLine.AxisValue) < Convert.ToDouble(upLine.AxisValue))
                {
                    YStrip.MaxLimit.AxisValue = upLine.AxisValue;
                    YStrip.MinLimit.AxisValue = downLine.AxisValue;
                }
                else
                {
                    YStrip.MaxLimit.AxisValue = downLine.AxisValue;
                    YStrip.MinLimit.AxisValue = upLine.AxisValue;
                }
                YStrip.Visible = true;
            }
            else
                YStrip.Visible = false;
        }
        public void ClearBound()
        {
            (this.Diagram as DevExpress.XtraCharts.XYDiagram).AxisX.Strips[0].Visible = false;
            (this.Diagram as DevExpress.XtraCharts.XYDiagram).AxisY.Strips[0].Visible = false;
            (this.Diagram as DevExpress.XtraCharts.XYDiagram).AxisX.ConstantLines[0].Visible = false;
            (this.Diagram as DevExpress.XtraCharts.XYDiagram).AxisX.ConstantLines[1].Visible = false;
            (this.Diagram as DevExpress.XtraCharts.XYDiagram).AxisY.ConstantLines[0].Visible = false;
            (this.Diagram as DevExpress.XtraCharts.XYDiagram).AxisY.ConstantLines[1].Visible = false;
        }
    }
}
