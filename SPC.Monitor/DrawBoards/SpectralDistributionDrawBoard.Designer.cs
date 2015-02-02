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
            DevExpress.XtraCharts.XYDiagram xyDiagram6 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.SecondaryAxisY secondaryAxisY6 = new DevExpress.XtraCharts.SecondaryAxisY();
            DevExpress.XtraCharts.Series series16 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel6 = new DevExpress.XtraCharts.PointSeriesLabel();
            DevExpress.XtraCharts.PointOptions pointOptions6 = new DevExpress.XtraCharts.PointOptions();
            DevExpress.XtraCharts.StepAreaSeriesView stepAreaSeriesView11 = new DevExpress.XtraCharts.StepAreaSeriesView();
            DevExpress.XtraCharts.Series series17 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.SplineSeriesView splineSeriesView11 = new DevExpress.XtraCharts.SplineSeriesView();
            DevExpress.XtraCharts.Series series18 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.SplineSeriesView splineSeriesView12 = new DevExpress.XtraCharts.SplineSeriesView();
            DevExpress.XtraCharts.StepAreaSeriesView stepAreaSeriesView12 = new DevExpress.XtraCharts.StepAreaSeriesView();
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
            ((System.ComponentModel.ISupportInitialize)(xyDiagram6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(secondaryAxisY6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(stepAreaSeriesView11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series17)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(splineSeriesView11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series18)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(splineSeriesView12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(stepAreaSeriesView12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemToggleSwitch1)).BeginInit();
            this.SuspendLayout();
            // 
            // chartControl1
            // 
            this.chartControl1.CrosshairOptions.HighlightPoints = false;
            xyDiagram6.AxisX.Tickmarks.MinorVisible = false;
            xyDiagram6.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram6.AxisX.WholeRange.AutoSideMargins = false;
            xyDiagram6.AxisX.WholeRange.SideMarginsValue = 0D;
            xyDiagram6.AxisY.VisibleInPanesSerializable = "-1";
            xyDiagram6.EnableAxisXScrolling = true;
            xyDiagram6.ScrollingOptions.UseKeyboard = false;
            xyDiagram6.ScrollingOptions.UseMouse = false;
            xyDiagram6.ScrollingOptions.UseTouchDevice = false;
            secondaryAxisY6.AxisID = 0;
            secondaryAxisY6.Name = "Secondary AxisY 1";
            secondaryAxisY6.VisibleInPanesSerializable = "-1";
            secondaryAxisY6.VisualRange.Auto = false;
            secondaryAxisY6.VisualRange.AutoSideMargins = false;
            secondaryAxisY6.VisualRange.MaxValueSerializable = "100";
            secondaryAxisY6.VisualRange.MinValueSerializable = "0";
            secondaryAxisY6.VisualRange.SideMarginsValue = 0D;
            secondaryAxisY6.WholeRange.Auto = false;
            secondaryAxisY6.WholeRange.AutoSideMargins = false;
            secondaryAxisY6.WholeRange.MaxValueSerializable = "100";
            secondaryAxisY6.WholeRange.MinValueSerializable = "0";
            secondaryAxisY6.WholeRange.SideMarginsValue = 0D;
            xyDiagram6.SecondaryAxesY.AddRange(new DevExpress.XtraCharts.SecondaryAxisY[] {
            secondaryAxisY6});
            xyDiagram6.ZoomingOptions.UseMouseWheel = false;
            this.chartControl1.Diagram = xyDiagram6;
            this.chartControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartControl1.Legend.Visible = false;
            this.chartControl1.Location = new System.Drawing.Point(4, 2);
            this.chartControl1.Name = "chartControl1";
            this.chartControl1.RuntimeHitTesting = true;
            series16.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Numerical;
            pointSeriesLabel6.Angle = 42;
            pointSeriesLabel6.Antialiasing = true;
            pointOptions6.ArgumentNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.Number;
            pointOptions6.PointView = DevExpress.XtraCharts.PointView.Argument;
            pointSeriesLabel6.PointOptions = pointOptions6;
            pointSeriesLabel6.ResolveOverlappingMode = DevExpress.XtraCharts.ResolveOverlappingMode.JustifyAroundPoint;
            pointSeriesLabel6.TextOrientation = DevExpress.XtraCharts.TextOrientation.TopToBottom;
            series16.Label = pointSeriesLabel6;
            series16.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            series16.Name = "Series 1";
            stepAreaSeriesView11.Transparency = ((byte)(0));
            series16.View = stepAreaSeriesView11;
            series17.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Numerical;
            series17.Name = "Series 2";
            splineSeriesView11.AxisYName = "Secondary AxisY 1";
            series17.View = splineSeriesView11;
            series18.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Numerical;
            series18.Name = "Series 3";
            splineSeriesView12.AxisYName = "Secondary AxisY 1";
            series18.View = splineSeriesView12;
            this.chartControl1.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series16,
        series17,
        series18};
            stepAreaSeriesView12.MarkerOptions.Size = 8;
            this.chartControl1.SeriesTemplate.View = stepAreaSeriesView12;
            this.chartControl1.SideBySideEqualBarWidth = false;
            this.chartControl1.Size = new System.Drawing.Size(718, 373);
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
            this.barDockControlTop.Location = new System.Drawing.Point(4, 2);
            this.barDockControlTop.Size = new System.Drawing.Size(718, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(4, 375);
            this.barDockControlBottom.Size = new System.Drawing.Size(718, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(4, 2);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 373);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(722, 2);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 373);
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
            this.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chartControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "SpectralDistributionDrawBoard";
            this.Size = new System.Drawing.Size(724, 377);
            ((System.ComponentModel.ISupportInitialize)(secondaryAxisY6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(stepAreaSeriesView11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(splineSeriesView11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series17)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(splineSeriesView12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series18)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(stepAreaSeriesView12)).EndInit();
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
