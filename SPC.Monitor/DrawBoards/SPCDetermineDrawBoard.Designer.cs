namespace SPC.Monitor.DrawBoards
{
    partial class SPCDetermineDrawBoard
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.XYDiagramPane xyDiagramPane1 = new DevExpress.XtraCharts.XYDiagramPane();
            DevExpress.XtraCharts.SecondaryAxisY secondaryAxisY1 = new DevExpress.XtraCharts.SecondaryAxisY();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView1 = new DevExpress.XtraCharts.LineSeriesView();
            DevExpress.XtraCharts.Series series2 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.PointSeriesView pointSeriesView1 = new DevExpress.XtraCharts.PointSeriesView();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView2 = new DevExpress.XtraCharts.LineSeriesView();
            this.chartControl1 = new DevExpress.XtraCharts.ChartControl();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagramPane1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(secondaryAxisY1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView2)).BeginInit();
            this.SuspendLayout();
            // 
            // chartControl1
            // 
            this.chartControl1.CrosshairOptions.ShowOnlyInFocusedPane = false;
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisY.WholeRange.AlwaysShowZeroLevel = false;
            xyDiagram1.EnableAxisXScrolling = true;
            xyDiagram1.PaneDistance = 0;
            xyDiagramPane1.Name = "Pane 1";
            xyDiagramPane1.PaneID = 0;
            xyDiagramPane1.Weight = 0.6D;
            xyDiagram1.Panes.AddRange(new DevExpress.XtraCharts.XYDiagramPane[] {
            xyDiagramPane1});
            xyDiagram1.ScrollingOptions.UseKeyboard = false;
            xyDiagram1.ScrollingOptions.UseMouse = false;
            xyDiagram1.ScrollingOptions.UseTouchDevice = false;
            secondaryAxisY1.Alignment = DevExpress.XtraCharts.AxisAlignment.Near;
            secondaryAxisY1.AxisID = 0;
            secondaryAxisY1.GridLines.Visible = true;
            secondaryAxisY1.Name = "Secondary AxisY 1";
            secondaryAxisY1.Tickmarks.MinorVisible = false;
            secondaryAxisY1.VisibleInPanesSerializable = "0";
            secondaryAxisY1.WholeRange.AlwaysShowZeroLevel = false;
            secondaryAxisY1.WholeRange.AutoSideMargins = true;
            xyDiagram1.SecondaryAxesY.AddRange(new DevExpress.XtraCharts.SecondaryAxisY[] {
            secondaryAxisY1});
            xyDiagram1.ZoomingOptions.UseMouseWheel = false;
            this.chartControl1.Diagram = xyDiagram1;
            this.chartControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartControl1.Legend.Visible = false;
            this.chartControl1.Location = new System.Drawing.Point(4, 2);
            this.chartControl1.Name = "chartControl1";
            series1.Name = "Series 1";
            lineSeriesView1.LineStyle.Thickness = 1;
            series1.View = lineSeriesView1;
            series2.CrosshairLabelPattern = "规则{V}";
            series2.Name = "Series 2";
            pointSeriesView1.AxisYName = "Secondary AxisY 1";
            pointSeriesView1.PaneName = "Pane 1";
            pointSeriesView1.PointMarkerOptions.Kind = DevExpress.XtraCharts.MarkerKind.Star;
            series2.View = pointSeriesView1;
            this.chartControl1.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1,
        series2};
            this.chartControl1.SeriesTemplate.View = lineSeriesView2;
            this.chartControl1.Size = new System.Drawing.Size(689, 444);
            this.chartControl1.TabIndex = 0;
            // 
            // SPCDetermineDrawBoard
            // 
            this.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chartControl1);
            this.Name = "SPCDetermineDrawBoard";
            this.Size = new System.Drawing.Size(695, 448);
            ((System.ComponentModel.ISupportInitialize)(xyDiagramPane1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(secondaryAxisY1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraCharts.ChartControl chartControl1;

    }
}
