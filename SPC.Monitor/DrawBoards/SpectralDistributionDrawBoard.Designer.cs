namespace SPC.Monitor.DrawBoards
{
    partial class SpectralDistributionDrawBoard
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
            DevExpress.XtraCharts.XYDiagram xyDiagram22 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.SecondaryAxisY secondaryAxisY22 = new DevExpress.XtraCharts.SecondaryAxisY();
            DevExpress.XtraCharts.Series series64 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel22 = new DevExpress.XtraCharts.PointSeriesLabel();
            DevExpress.XtraCharts.PointOptions pointOptions22 = new DevExpress.XtraCharts.PointOptions();
            DevExpress.XtraCharts.StepAreaSeriesView stepAreaSeriesView43 = new DevExpress.XtraCharts.StepAreaSeriesView();
            DevExpress.XtraCharts.Series series65 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.SplineSeriesView splineSeriesView43 = new DevExpress.XtraCharts.SplineSeriesView();
            DevExpress.XtraCharts.Series series66 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.SplineSeriesView splineSeriesView44 = new DevExpress.XtraCharts.SplineSeriesView();
            DevExpress.XtraCharts.StepAreaSeriesView stepAreaSeriesView44 = new DevExpress.XtraCharts.StepAreaSeriesView();
            this.chartControl1 = new SPC.Controls.Base.AdvChartControl();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.repositoryItemCheckEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.barEditItem1 = new DevExpress.XtraBars.BarEditItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barManager1 = new DevExpress.XtraBars.BarManager();
            this.barEditItem2 = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemToggleSwitch1 = new DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram22)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(secondaryAxisY22)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series64)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel22)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(stepAreaSeriesView43)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series65)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(splineSeriesView43)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series66)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(splineSeriesView44)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(stepAreaSeriesView44)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemToggleSwitch1)).BeginInit();
            this.SuspendLayout();
            // 
            // chartControl1
            // 
            this.chartControl1.CrosshairOptions.HighlightPoints = false;
            xyDiagram22.AxisX.Tickmarks.MinorVisible = false;
            xyDiagram22.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram22.AxisX.WholeRange.AutoSideMargins = false;
            xyDiagram22.AxisX.WholeRange.SideMarginsValue = 0D;
            xyDiagram22.AxisY.VisibleInPanesSerializable = "-1";
            xyDiagram22.EnableAxisXScrolling = true;
            xyDiagram22.ScrollingOptions.UseKeyboard = false;
            xyDiagram22.ScrollingOptions.UseMouse = false;
            xyDiagram22.ScrollingOptions.UseTouchDevice = false;
            secondaryAxisY22.AxisID = 0;
            secondaryAxisY22.Name = "Secondary AxisY 1";
            secondaryAxisY22.VisibleInPanesSerializable = "-1";
            secondaryAxisY22.VisualRange.Auto = false;
            secondaryAxisY22.VisualRange.AutoSideMargins = false;
            secondaryAxisY22.VisualRange.MaxValueSerializable = "100";
            secondaryAxisY22.VisualRange.MinValueSerializable = "0";
            secondaryAxisY22.VisualRange.SideMarginsValue = 0D;
            secondaryAxisY22.WholeRange.Auto = false;
            secondaryAxisY22.WholeRange.AutoSideMargins = false;
            secondaryAxisY22.WholeRange.MaxValueSerializable = "100";
            secondaryAxisY22.WholeRange.MinValueSerializable = "0";
            secondaryAxisY22.WholeRange.SideMarginsValue = 0D;
            xyDiagram22.SecondaryAxesY.AddRange(new DevExpress.XtraCharts.SecondaryAxisY[] {
            secondaryAxisY22});
            xyDiagram22.ZoomingOptions.UseMouseWheel = false;
            this.chartControl1.Diagram = xyDiagram22;
            this.chartControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartControl1.Legend.Visible = false;
            this.chartControl1.Location = new System.Drawing.Point(0, 0);
            this.chartControl1.Name = "chartControl1";
            this.chartControl1.RuntimeHitTesting = true;
            series64.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Numerical;
            pointSeriesLabel22.Angle = 42;
            pointSeriesLabel22.Antialiasing = true;
            pointOptions22.ArgumentNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.Number;
            pointOptions22.PointView = DevExpress.XtraCharts.PointView.Argument;
            pointSeriesLabel22.PointOptions = pointOptions22;
            pointSeriesLabel22.ResolveOverlappingMode = DevExpress.XtraCharts.ResolveOverlappingMode.JustifyAroundPoint;
            pointSeriesLabel22.TextOrientation = DevExpress.XtraCharts.TextOrientation.TopToBottom;
            series64.Label = pointSeriesLabel22;
            series64.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            series64.Name = "Series 1";
            stepAreaSeriesView43.Transparency = ((byte)(0));
            series64.View = stepAreaSeriesView43;
            series65.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Numerical;
            series65.Name = "Series 2";
            splineSeriesView43.AxisYName = "Secondary AxisY 1";
            series65.View = splineSeriesView43;
            series66.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Numerical;
            series66.Name = "Series 3";
            splineSeriesView44.AxisYName = "Secondary AxisY 1";
            series66.View = splineSeriesView44;
            this.chartControl1.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series64,
        series65,
        series66};
            stepAreaSeriesView44.MarkerOptions.Size = 8;
            this.chartControl1.SeriesTemplate.View = stepAreaSeriesView44;
            this.chartControl1.SideBySideEqualBarWidth = false;
            this.chartControl1.Size = new System.Drawing.Size(724, 377);
            this.chartControl1.TabIndex = 0;
            this.chartControl1.CustomShowRightClickPopupMenu += new System.EventHandler<SPC.Controls.Base.AdvChartControl.ShowRightClickPopupMenuEventArgs>(this.chartControl1_CustomShowRightClickPopupMenu);
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Caption = "Check";
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            this.repositoryItemCheckEdit1.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.repositoryItemCheckEdit1_EditValueChanging);
            // 
            // repositoryItemCheckEdit2
            // 
            this.repositoryItemCheckEdit2.AutoHeight = false;
            this.repositoryItemCheckEdit2.Caption = "Check";
            this.repositoryItemCheckEdit2.Name = "repositoryItemCheckEdit2";
            // 
            // barEditItem1
            // 
            this.barEditItem1.Caption = "显示X坐标";
            this.barEditItem1.Edit = this.repositoryItemCheckEdit1;
            this.barEditItem1.Id = 0;
            this.barEditItem1.Name = "barEditItem1";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(724, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 377);
            this.barDockControlBottom.Size = new System.Drawing.Size(724, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 377);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(724, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 377);
            // 
            // barManager1
            // 
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barEditItem1,
            this.barEditItem2});
            this.barManager1.MaxItemId = 3;
            this.barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1,
            this.repositoryItemCheckEdit2,
            this.repositoryItemToggleSwitch1});
            // 
            // barEditItem2
            // 
            this.barEditItem2.Caption = "填充";
            this.barEditItem2.Edit = this.repositoryItemToggleSwitch1;
            this.barEditItem2.Id = 2;
            this.barEditItem2.Name = "barEditItem2";
            // 
            // repositoryItemToggleSwitch1
            // 
            this.repositoryItemToggleSwitch1.AutoHeight = false;
            this.repositoryItemToggleSwitch1.Name = "repositoryItemToggleSwitch1";
            this.repositoryItemToggleSwitch1.OffText = "Off";
            this.repositoryItemToggleSwitch1.OnText = "On";
            this.repositoryItemToggleSwitch1.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.repositoryItemToggleSwitch1_EditValueChanging);
            // 
            // SpectralDistributionDrawBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chartControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "SpectralDistributionDrawBoard";
            this.Size = new System.Drawing.Size(724, 377);
            ((System.ComponentModel.ISupportInitialize)(secondaryAxisY22)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram22)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel22)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(stepAreaSeriesView43)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series64)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(splineSeriesView43)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series65)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(splineSeriesView44)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series66)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(stepAreaSeriesView44)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemToggleSwitch1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.Base.AdvChartControl chartControl1;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit2;
        private DevExpress.XtraBars.BarEditItem barEditItem1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarEditItem barEditItem2;
        private DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch repositoryItemToggleSwitch1;
    }
}
