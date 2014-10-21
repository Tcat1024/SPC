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
                else if (value.Name == "Y")
                    this.Cursor = System.Windows.Forms.Cursors.SizeNS;
                this._targetLine = value;
            }
        }
        private bool Inited = false;
        private DevExpress.XtraBars.PopupMenu RightClickPopupMenu = new DevExpress.XtraBars.PopupMenu();
        private DevExpress.XtraBars.BarEditItem popupMenuEditItem = new DevExpress.XtraBars.BarEditItem();
        private DevExpress.XtraBars.BarButtonItem popupMenuButtonItem = new DevExpress.XtraBars.BarButtonItem();
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit popupMenuTextEdit = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
        private DevExpress.XtraCharts.ConstantLine upLine = new DevExpress.XtraCharts.ConstantLine() {Name = "Y",Visible = false};
        private DevExpress.XtraCharts.ConstantLine downLine = new DevExpress.XtraCharts.ConstantLine() { Name = "Y", Visible = false };
        private DevExpress.XtraCharts.ConstantLineCollection yLineCollection;
        private DevExpress.XtraBars.BarManager mybarmanager = new DevExpress.XtraBars.BarManager();

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
            InitPopupMenu();
            popupMenuButtonItem.ItemClick += popupMenuButtonItem_ItemClick;
            popupMenuEditItem.EditValueChanged += popupMenuEditItem_EditValueChanged;
            this.RuntimeHitTesting = true;
            this.RightClickPopupMenu.Manager = mybarmanager;
            this.popupMenuEditItem.Edit = this.popupMenuTextEdit;
            this.mybarmanager.Form = this;
        }
        private void popupMenuEditItem_EditValueChanged(object sender, EventArgs e)
        {
            var t = ((sender as DevExpress.XtraBars.BarEditItem).Tag as DevExpress.XtraCharts.ConstantLine);
            string s = (sender as DevExpress.XtraBars.BarEditItem).EditValue.ToString();
            if (t!=null&&t.Name == "Y")
            {
                t.AxisValue = s;
                t.Title.Text = t.AxisValue.ToString();
            }
        }
        private void InitPopupMenu()
        {
            this.RightClickPopupMenu.ClearLinks();
        }
        public void Init()
        {
            if (this.Diagram is DevExpress.XtraCharts.XYDiagram)
                this.yLineCollection = (this.Diagram as DevExpress.XtraCharts.XYDiagram).AxisY.ConstantLines;
            else
                this.yLineCollection = (this.Diagram as DevExpress.XtraCharts.SwiftPlotDiagram).AxisY.ConstantLines;
            yLineCollection.AddRange(new DevExpress.XtraCharts.ConstantLine[]{upLine,downLine});
            this.Inited = true;
        }
        private void popupMenuButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            (e.Item.Tag as DevExpress.XtraCharts.ConstantLine).Visible = false;
        }
        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (!DesignMode&&Inited)
            {
                if (this.Series.Count > 0)
                {
                    var info = this.CalcHitInfo(e.Location);
                    if (info.Diagram != null)
                    {
                        if (e.Button == System.Windows.Forms.MouseButtons.Left && info.InConstantLine)
                        {
                            this.targetLine = info.ConstantLine;
                        }
                        else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                        {
                            InitPopupMenu();
                            if (info.InConstantLine && info.ConstantLine.Visible)
                            {
                                popupMenuEditItem.Caption = info.ConstantLine.Name + "轴边界";
                                popupMenuEditItem.EditValue = info.ConstantLine.AxisValue;
                                popupMenuEditItem.Tag = info.ConstantLine;
                                this.RightClickPopupMenu.AddItem(popupMenuEditItem);
                                popupMenuButtonItem.Tag = info.ConstantLine;
                                popupMenuButtonItem.Caption = "删除" + info.ConstantLine.Name + "轴边界";
                                this.RightClickPopupMenu.AddItem(popupMenuButtonItem);
                                var eventarg = new ShowRightClickPopupMenuEventArgs(RightClickPopupMenu, info.ConstantLine, true);
                                CustomShowRightClickPopupMenu(RightClickPopupMenu, eventarg);
                                if (eventarg.Handle)
                                    this.RightClickPopupMenu.ShowPopup(MousePosition);
                                //InitPopupMenu();
                            }
                            else
                            {
                                var eventarg = new ShowRightClickPopupMenuEventArgs(RightClickPopupMenu, null, false);
                                CustomShowRightClickPopupMenu(RightClickPopupMenu, eventarg);
                                if (eventarg.Handle)
                                    this.RightClickPopupMenu.ShowPopup(MousePosition);
                                //InitPopupMenu();
                            }
                        }
                    }
                    else
                    {
                        if (this.Diagram is DevExpress.XtraCharts.XYDiagram)
                        {
                            var yInfo = (this.Diagram as DevExpress.XtraCharts.XYDiagram).PointToDiagram(new System.Drawing.Point(this.Width/2, e.Y));
                            if (yInfo.AxisY != null)
                            {
                                double y = yInfo.NumericalValue;
                                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                                {
                                    if (!downLine.Visible && !upLine.Visible)
                                    {
                                        downLine.AxisValue = y;
                                        downLine.Title.Text = String.Format("{0:N3}", downLine.AxisValue); ;
                                        this.targetLine = downLine;
                                        downLine.Visible = true;
                                    }
                                    else if (!upLine.Visible || !downLine.Visible)
                                    {
                                        var temp = upLine.Visible ? downLine : upLine;
                                        temp.AxisValue = y;
                                        temp.Title.Text = String.Format("{0:N3}", temp.AxisValue);
                                        this.targetLine = temp;
                                        temp.Visible = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            var yInfo = (this.Diagram as DevExpress.XtraCharts.SwiftPlotDiagram).PointToDiagram(new System.Drawing.Point(this.Width/2, e.Y));
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
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (!DesignMode && Inited)
            {
                if (this.targetLine != null)
                {
                    if (this.Diagram is DevExpress.XtraCharts.XYDiagram)
                    {
                        var info = (this.Diagram as DevExpress.XtraCharts.XYDiagram).PointToDiagram(e.Location);
                        if (targetLine.Name == "Y" && !info.IsEmpty)
                        {
                            targetLine.AxisValue = info.NumericalValue;
                            targetLine.Title.Text = String.Format("{0:N3}", targetLine.AxisValue);
                        }
                    }
                    else
                    {
                        var info = (this.Diagram as DevExpress.XtraCharts.SwiftPlotDiagram).PointToDiagram(e.Location);
                        if (targetLine.Name == "Y" && !info.IsEmpty)
                        {
                            targetLine.AxisValue = info.NumericalValue;
                            targetLine.Title.Text = String.Format("{0:N3}", targetLine.AxisValue);
                        }
                    }
                }
                this.targetLine = null;
            }
        }
        private int lastY = 0;
        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!DesignMode && Inited)
            {
                if (this.targetLine == null)
                    this.Cursor = System.Windows.Forms.Cursors.Arrow;
                if (this.Series.Count > 0)
                {
                    var info = this.CalcHitInfo(e.Location);
                    if (info.ConstantLine != null && info.ConstantLine.Name == "Y")
                    {
                        this.Cursor = System.Windows.Forms.Cursors.SizeNS;
                    }
                }
                if (this.targetLine != null&&Math.Abs(lastY-e.Y)<3)
                {
                    if (this.Diagram is DevExpress.XtraCharts.XYDiagram)
                    {
                        var info = (this.Diagram as DevExpress.XtraCharts.XYDiagram).PointToDiagram(e.Location);
                        if (targetLine.Name == "Y" && !info.IsEmpty)
                        {
                            targetLine.AxisValue = info.NumericalValue;
                            targetLine.Title.Text = String.Format("{0:N3}", targetLine.AxisValue);
                        }
                    }
                    else
                    {
                        var info = (this.Diagram as DevExpress.XtraCharts.SwiftPlotDiagram).PointToDiagram(e.Location);
                        if (targetLine.Name == "Y" && !info.IsEmpty)
                        {
                            targetLine.AxisValue = info.NumericalValue;
                            targetLine.Title.Text = String.Format("{0:N3}", targetLine.AxisValue);
                        }
                    }
                }
                
            }
            lastY = e.Y;
        }
        public new DevExpress.XtraCharts.Diagram Diagram
        {
            get { return base.Diagram; }
            set { base.Diagram = value; this.Init(); }
        }
    }
    public static class AddExtention
    {
        public static int Add(this DevExpress.XtraCharts.ConstantLineCollection col,DevExpress.XtraCharts.ConstantLine target)
        {
            int result;
            if(col.Count>0&&(col[col.Count].Name=="X"))
            {
                DevExpress.XtraCharts.ConstantLine[] temp = new DevExpress.XtraCharts.ConstantLine[] { col[col.Count - 1], col[col.Count] };
                col.RemoveAt(col.Count);
                col.RemoveAt(col.Count - 1);
                result = col.Add(target);
                col.AddRange(temp);
            }
            else
                result = col.Add(target);
            return result;
        }
    }
}
