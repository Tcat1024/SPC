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
    public partial class DevChartDrawBoard : DevExpress.XtraEditors.XtraUserControl,SPC.Base.Interface.IDrawBoard<DevExpress.XtraCharts.ChartControl>
    {
        private DevExpress.XtraCharts.ChartControl _mainChart;
        protected DevExpress.XtraCharts.ChartControl mainChart
        {
            get
            {
                return _mainChart;
            }
            set
            {
                _mainChart = value;
                if(value!=null)
                {
                    _mainChart.GotFocus += mainChart_GotFocus;
                    _mainChart.LostFocus += _mainChart_LostFocus;
                    (_mainChart.Diagram as DevExpress.XtraCharts.XYDiagram2D).EnableAxisXScrolling = true;
                    (_mainChart.Diagram as DevExpress.XtraCharts.XYDiagram2D).EnableAxisYScrolling = true;
                }
            }
        }

        void _mainChart_LostFocus(object sender, EventArgs e)
        {
            InvokeLostFocus(this, e);
        }
        protected int baseSeriesCount;
        public DevChartDrawBoard()
        {
            InitializeComponent();
        }
        void mainChart_GotFocus(object sender, EventArgs e)
        {
            InvokeGotFocus(this, e);
        }
        public event EventHandler Removed;
        public DevExpress.XtraCharts.ChartControl GetChart()
        {
            return mainChart;
        }

        public bool CheckCanRemove()
        {
            if (this.mainChart.Series.Count == baseSeriesCount)
                return true;
            return false;
        }


        public void Hup()
        {
            foreach (var ax in (this._mainChart.Diagram as DevExpress.XtraCharts.XYDiagram2D).GetAllAxesX())
            {
                var t = ax.VisualRange.MinValueInternal;
                var k = ax.VisualRange.MaxValueInternal;
                ax.VisualRange.SetMinMaxValues(ax.GetScaleValueFromInternal(t + (k - t) / 6), ax.GetScaleValueFromInternal(k - (k - t) / 6));
            }
        }

        public void Vup()
        {
            foreach (var ax in (this._mainChart.Diagram as DevExpress.XtraCharts.XYDiagram2D).GetAllAxesY())
            {
                var t = ax.VisualRange.MinValueInternal;
                var k = ax.VisualRange.MaxValueInternal;
                ax.VisualRange.SetMinMaxValues(ax.GetScaleValueFromInternal(t + (k - t) / 6), ax.GetScaleValueFromInternal(k - (k - t) / 6));
            }
        }

        public void Hdown()
        {
            foreach (var ax in (this._mainChart.Diagram as DevExpress.XtraCharts.XYDiagram2D).GetAllAxesX())
            {
                var t = ax.VisualRange.MinValueInternal;
                var k = ax.VisualRange.MaxValueInternal;
                ax.VisualRange.SetMinMaxValues(ax.GetScaleValueFromInternal(t - (k - t) / 8),ax.GetScaleValueFromInternal(k + (k - t) / 8));
            }
        }

        public void Vdown()
        {
            foreach (var ax in (this._mainChart.Diagram as DevExpress.XtraCharts.XYDiagram2D).GetAllAxesY())
            {
                var t = ax.VisualRange.MinValueInternal;
                var k = ax.VisualRange.MaxValueInternal;
                ax.VisualRange.SetMinMaxValues(ax.GetScaleValueFromInternal(t - (k - t) / 8), ax.GetScaleValueFromInternal(k + (k - t) / 8));
            }
        }
        public void Re()
        {
            foreach (var ax in (this._mainChart.Diagram as DevExpress.XtraCharts.XYDiagram2D).GetAllAxesX())
            {
                ax.VisualRange.SetMinMaxValues(ax.WholeRange.MinValueInternal, ax.WholeRange.MaxValueInternal);
            }
            foreach (var ax in (this._mainChart.Diagram as DevExpress.XtraCharts.XYDiagram2D).GetAllAxesY())
            {
                ax.VisualRange.SetMinMaxValues(ax.WholeRange.MinValueInternal, ax.WholeRange.MaxValueInternal);
            }
        }
        private void DevChartDrawBoard_ParentChanged(object sender, EventArgs e)
        {
            if (this.Parent == null && this.Removed != null)
                this.Removed(this, new EventArgs());

        }



    }
}
