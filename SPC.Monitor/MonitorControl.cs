using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using SPC.Base.Interface;
using SPC.Base.Operation;
using SPC.Base.Control;
using SPC.Monitor.DrawBoards;


namespace SPC.Monitor
{
    public partial class MonitorControl : DevExpress.XtraEditors.XtraUserControl
    {
        DataTable Data;
        BindingSource DataBind = new BindingSource();
        public string ChooseColumnName = "choose";
        const int MaxSeriesCount = 50;
        int historySeriesCount = 0;
        DevExpress.XtraCharts.PaletteEntry[] Colors;
        DevExpress.XtraCharts.ChartControl basicColorChart = new DevExpress.XtraCharts.ChartControl();
        List<Type> DrawBoardTypes = new List<Type>();
        public MonitorControl()
        {
            InitializeComponent();
            this.gridControl1.DataSource = this.DataBind;
            this.bindingNavigator1.BindingSource = this.DataBind;
            this.bindingNavigator1.Items.Insert(10,new ToolStripControlHost(this.comboBoxEdit1));
            this.bindingNavigator1.Items.Insert(11,new ToolStripControlHost(this.textEdit1));
            this.bindingNavigator1.Items.Insert(13, new ToolStripControlHost(this.popUpEdit1));
            Colors = this.basicColorChart.GetPaletteEntries(MaxSeriesCount);
            this.popUpEdit1.Properties.Mask.EditMask = "([0-9]*)|"+CustomGroupMaker.StandardMaskString;
            this.popUpEdit1.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.popUpEdit1.Properties.Mask.ShowPlaceHolders = false;
            this.InitDrawBoads();
        }
        private int _SelectedTabPageIndex = 0;
        public int SelectedTabPageIndex
        {
            get
            {
                return this._SelectedTabPageIndex;
            }
            set
            {
                this._SelectedTabPageIndex = value;
                this.xtraTabControl1.SelectedTabPageIndex = value;
            }
        }
        public object DataView
        {
            get
            {
                return this.gridView1;
            }
        }
        private object _DataSource;
        [Category("Data")]
        [AttributeProvider(typeof(IListSource))]
        [DefaultValue("")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public object DataSource
        {
            get
            {
                return this._DataSource;
            }
            set
            {
                this._DataSource = value;
                if (this._DataSource is DataTable)
                    DataInit(this._DataSource as DataTable);
                else if (this._DataSource is DataSet && this._DataMember != null && this._DataMember.Trim() != "")
                    DataInit((this._DataSource as DataSet).Tables[this._DataMember]);
            }
        }
        private string _DataMember;
        [Category("Data")]
        [DefaultValue("")]
        [Editor("System.Windows.Forms.Design.DataMemberListEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string DataMember
        {
            get
            {
                return this._DataMember;
            }
            set
            {
                this._DataMember = value;
                if (this._DataSource is DataSet && this._DataMember != null && this._DataMember.Trim() != "")
                    DataInit((this._DataSource as DataSet).Tables[this._DataMember]);
            }
        }
        private void DataInit(DataTable source)
        {
            this.Data = source;
            RefreshData();
        }
        private void RefreshData()
        {
            if (this.Data == null)
            {
                MessageBox.Show("数据集为空");
                return;
            }
            this.gridView1.Columns.Clear();
            this.DataBind.DataSource = this.Data;
        }
        private void gridView1_DragObjectDrop(object sender, DevExpress.XtraGrid.Views.Base.DragObjectDropEventArgs e)
        {
            MonitorSeriesData data = null;
            List<IDrawBoard<DevExpress.XtraCharts.ChartControl>> DrawBoards = null;
            try
            {
                var col = (e.DragObject as DevExpress.XtraGrid.Columns.GridColumn);
                if (col.FieldName != this.ChooseColumnName && this.Data.Columns[col.FieldName].DataType != typeof(string) && this.Data.Columns[col.FieldName].DataType != typeof(DateTime))
                {
                    var mouseposition = this.listBoxControl1.PointToClient(MousePosition);
                    int grouptype = Convert.ToInt32(this.textEdit1.EditValue);
                    string spectrumwith = this.popUpEdit1.Text.Trim() == "" ? "0" : this.popUpEdit1.Text.Trim();
                    if (this.comboBoxEdit1.SelectedIndex == 1)
                    {
                        if (this.gridView1.GroupCount < 1)
                            throw new Exception("Group模式下请先对数据进行分组");
                        grouptype = 0;
                    }
                    if (mouseposition.X > 0 && mouseposition.X < this.listBoxControl1.Width && mouseposition.Y > 0 && mouseposition.Y < this.listBoxControl1.Height)
                    {
                        data = new MonitorSeriesData(this.gridView1, col.FieldName, grouptype, spectrumwith, this.Colors[historySeriesCount++ % MaxSeriesCount].Color, DrawBoards = this.AddDrawBoards());
                        this.AddListItem(data);
                    }
                    else if (this.xtraTabControl1.CalcHitInfo(this.xtraTabControl1.PointToClient(MousePosition)).HitTest == DevExpress.XtraTab.ViewInfo.XtraTabHitTest.PageClient && this.xtraTabControl1.SelectedTabPage.Controls.Count >= 0)
                    {
                        var targetlayout = this.xtraTabControl1.SelectedTabPage.Controls[0];
                        var targetchart = targetlayout.GetChildAtPoint(targetlayout.PointToClient(MousePosition));
                        int index = targetlayout.Controls.IndexOf(targetchart);
                        if (index >= 0)
                        {
                            data = new MonitorSeriesData(this.gridView1, col.FieldName, grouptype, spectrumwith, this.Colors[historySeriesCount++ % MaxSeriesCount].Color,DrawBoards = this.GetDrawBoards(index));
                            this.AddListItem(data);
                        }
                        else
                        {
                            data = new MonitorSeriesData(this.gridView1, col.FieldName, grouptype, spectrumwith, this.Colors[historySeriesCount++ % MaxSeriesCount].Color,DrawBoards = this.AddDrawBoards());
                            this.AddListItem(data);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                foreach(var DrawBoard in DrawBoards)
                {
                    if (DrawBoard.CheckCanRemove())
                        DrawBoard.Parent.Controls.Remove(DrawBoard as Control);
                }
                RemoveListItem(data);
            }
        }
        private void AddListItem(MonitorSeriesData lt)
        {
            this.listBoxControl1.Items.Insert(0, lt);
            this.listBoxControl1.SelectedIndex = 0;
            lt.DrawSerieses();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var temp = this.listBoxControl1.SelectedItem as MonitorSeriesData;
            RemoveListItem(temp as MonitorSeriesData);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            for(int i = this.listBoxControl1.Items.Count-1;i>=0;i--)
            {
                RemoveListItem(this.listBoxControl1.Items[i] as MonitorSeriesData);
            }
        }
        private void RemoveListItem(MonitorSeriesData lt)
        {
            if (lt != null)
            {
                lt.ClearSerieses();
                this.listBoxControl1.Items.Remove(lt);
            }
        }
        private void listBoxControl1_DrawItem(object sender, ListBoxDrawItemEventArgs e)
        {
            e.Appearance.ForeColor = (e.Item as MonitorSeriesData).SeriesColor;
        }
        private MonitorSeriesData currentItem;
        private MonitorSeriesData focusItem;
        private void listBoxControl1_MouseClick(object sender, MouseEventArgs e)
        {
            int index = this.listBoxControl1.IndexFromPoint(e.Location);
            if (index >= 0 && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                currentItem = this.listBoxControl1.Items[index] as MonitorSeriesData;
                if (currentItem != null)
                    this.popupMenu1.ShowPopup(MousePosition);
            }
            else if (focusItem != null)
            {
                DFocusDrawBoard(focusItem);
                focusItem = null;
            }
        }
        private List<IDrawBoard<DevExpress.XtraCharts.ChartControl>> AddDrawBoards()
        {
            List<IDrawBoard<DevExpress.XtraCharts.ChartControl>> drawBoards = new List<IDrawBoard<DevExpress.XtraCharts.ChartControl>>();
            for (int i = 0; i < this.xtraTabControl1.TabPages.Count; i++)
            {
                if (xtraTabControl1.TabPages[i].Controls.Count > 0&&this.DrawBoardTypes.Count>i)
                {
                    var temp = Activator.CreateInstance(this.DrawBoardTypes[i], null);
                    this.xtraTabControl1.TabPages[i].Controls[0].Controls.Add(temp as UserControl);
                    (temp as IDrawBoard<DevExpress.XtraCharts.ChartControl>).GotFocus+=DrawBoard_GotFocus;
                    (temp as IDrawBoard<DevExpress.XtraCharts.ChartControl>).Removed += DrawBoard_Removed;
                    drawBoards.Add(temp as IDrawBoard<DevExpress.XtraCharts.ChartControl>);
                }
            }
            return drawBoards;
        }

        void DrawBoard_Removed(object sender, EventArgs e)
        {
            if (this.FocusedDrawBoard == sender)
                this.FocusedDrawBoard = null;
        }
        void DrawBoard_GotFocus(object sender, EventArgs e)
        {
            this.FocusedDrawBoard = sender as IDrawBoard<DevExpress.XtraCharts.ChartControl>;
        }
        private IDrawBoard<DevExpress.XtraCharts.ChartControl> _FocusedDrawBoard = null;
        private IDrawBoard<DevExpress.XtraCharts.ChartControl> FocusedDrawBoard
        {
            get
            {
                return this._FocusedDrawBoard;
            }
            set
            {
                this._FocusedDrawBoard = value;
                if(value!=null)
                {
                    this.btnHdown.Enabled = true;
                    this.btnHup.Enabled = true;
                    this.btnVdown.Enabled = true;
                    this.btnVup.Enabled = true;
                    this.btnRe.Enabled = true;
                }
                else
                {
                    this.btnHdown.Enabled = false;
                    this.btnHup.Enabled = false;
                    this.btnVdown.Enabled = false;
                    this.btnVup.Enabled = false;
                    this.btnRe.Enabled = false;
                }
            }
        }
        private List<IDrawBoard<DevExpress.XtraCharts.ChartControl>> GetDrawBoards(int Index)
        {
            List<IDrawBoard<DevExpress.XtraCharts.ChartControl>> drawBoards = new List<IDrawBoard<DevExpress.XtraCharts.ChartControl>>();
            for (int i = 0; i < this.xtraTabControl1.TabPages.Count; i++)
            {
                if (xtraTabControl1.TabPages[i].Controls.Count > 0 && xtraTabControl1.TabPages[i].Controls[0].Controls.Count>0)
                    drawBoards.Add(xtraTabControl1.TabPages[i].Controls[0].Controls[Index] as IDrawBoard<DevExpress.XtraCharts.ChartControl>);
            }
            return drawBoards;
        }
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.buttonEdit1.Text = currentItem.Name;
            this.buttonEdit1.Visible = true;
            this.buttonEdit1.Focus();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            currentItem.ClearSerieses();
            this.listBoxControl1.Items.Remove(currentItem);
        }

        private void buttonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            currentItem.Name = this.buttonEdit1.Text;
            this.buttonEdit1.Visible = false;
        }

        private void buttonEdit1_Leave(object sender, EventArgs e)
        {
            if (this.buttonEdit1.Visible)
                this.buttonEdit1.Visible = false;
        }

        private void buttonEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                currentItem.Name = this.buttonEdit1.Text;
                this.buttonEdit1.Visible = false;
            }
        }
        private void FocusDrawBoard(MonitorSeriesData target)
        {
            foreach (var DrawBoard in target.DrawBoards)
            {
                DrawBoard.GetChart().Focus();
                DrawBoard.GetChart().BackColor = SystemColors.ActiveCaption;
            }
        }
        private void DFocusDrawBoard(MonitorSeriesData target)
        {
            foreach (var DrawBoard in target.DrawBoards)
                DrawBoard.GetChart().BackColor = default(Color);
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((sender as ComboBoxEdit).SelectedIndex == 0)
            {
                this.textEdit1.Enabled = true;
            }
            else
                this.textEdit1.Enabled = false;
        }

        private void listBoxControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.listBoxControl1.IndexFromPoint(e.Location);
            if (index >= 0)
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    if (focusItem != null)
                        DFocusDrawBoard(focusItem);
                    focusItem = (this.listBoxControl1.Items[index] as MonitorSeriesData);
                    FocusDrawBoard(focusItem);
                }
        }
        public class MonitorSeriesData
        {
            private List<SingleSeriesManager<MonitorSourceDataType, DevExpress.XtraCharts.ChartControl>> SeriesManagers = new List<SingleSeriesManager<MonitorSourceDataType, DevExpress.XtraCharts.ChartControl>>();
            public List<IDrawBoard<DevExpress.XtraCharts.ChartControl>> DrawBoards;
            private MonitorSourceDataType SourceData;
            public string Name;
            public System.Drawing.Color SeriesColor;
            private void InitSerieses()
            {
                foreach (var seriesManager in SeriesManagers)
                {
                    seriesManager.InitData(this.SourceData);
                }
            }
            public MonitorSeriesData(SPC.Base.Control.CanChooseDataGridView view, string param, int groupType, string spectrumWith, System.Drawing.Color color, List<IDrawBoard<DevExpress.XtraCharts.ChartControl>> drawBoards)
            {
                SourceData = new MonitorSourceDataType(view, param, groupType, spectrumWith);
                this.Name = param + "_" + groupType.ToString() + "_" + DateTime.Now.ToBinary();
                this.SeriesColor = color;
                this.DrawBoards = drawBoards;
                InitSeriesManagers();
                InitSerieses();
            }
            public void DrawSerieses()
            {
                foreach (var seriesManager in SeriesManagers)
                {
                    seriesManager.DrawSeries(this.SeriesColor);
                }
            }
            public void ClearSerieses()
            {
                for (int i = SeriesManagers.Count-1; i >= 0;i-- )
                {
                    var seriesManager = SeriesManagers[i];
                    seriesManager.RemoveSeries();
                    if(seriesManager.DrawBoard.CheckCanRemove())
                    {
                        seriesManager.DrawBoard.Parent.Controls.Remove(seriesManager.DrawBoard as Control);
                    }
                }
            }
            public override string ToString()
            {
                return this.Name.ToString();
            }
            public override bool Equals(object obj)
            {
                return base.Equals(obj);
            }
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
            //在此添加新需求
            private void InitSeriesManagers()
            {
                this.SeriesManagers.Add(new SampleRunSeriesManager() { DrawBoard = this.DrawBoards[0] });
                this.SeriesManagers.Add(new GroupAvgSeriesManager() { DrawBoard = this.DrawBoards[1] });
                this.SeriesManagers.Add(new GroupRangeSeriesManager() { DrawBoard = this.DrawBoards[1]});
                this.SeriesManagers.Add(new GroupAvgDataRunSeriesManager() { DrawBoard = this.DrawBoards[2] });
                this.SeriesManagers.Add(new SampleRunGroupPointsManager() { DrawBoard = this.DrawBoards[2] });
                this.SeriesManagers.Add(new NormalityCheckPointsManager() { DrawBoard = this.DrawBoards[3] });
                this.SeriesManagers.Add(new SpectralDistributionPointsManager() { DrawBoard = this.DrawBoards[4] });
                this.SeriesManagers.Add(new BoxPlotSeriesManager() { DrawBoard = this.DrawBoards[5] });
            }        
        }
        //在此添加新绘版
        private void InitDrawBoads()
        {
            this.DrawBoardTypes.Add(typeof(SampleDataRunDrawBoard));
            this.DrawBoardTypes.Add(typeof(DataControlDrawBoard));
            this.DrawBoardTypes.Add(typeof(AvgDataRunDrawBoard));
            this.DrawBoardTypes.Add(typeof(NormalityCheckDrawBoard));
            this.DrawBoardTypes.Add(typeof(SpectralDistributionDrawBoard));
            this.DrawBoardTypes.Add(typeof(BoxPlotDrawBoard));
        }

        private void MonitorControl_SizeChanged(object sender, EventArgs e)
        {
            chartSizeInit();
        }
        private void chartSizeInit()
        {
            this.panelControl1.Height = (int)(this.Size.Height * 0.5);
        }

        private void customGroupStringBuilder1_GroupStringDetermined(object sender, Base.Control.CustomGroupStringBuilder.GroupStringDeterminedEventArgs e)
        {
            if (e.result.Trim() != "")
                this.popUpEdit1.Text = e.result;
            else
                this.popUpEdit1.Text = "0";
            this.popUpEdit1.ClosePopup();
        }

        private void customGroupStringBuilder1_GroupStringCanceled(object sender, EventArgs e)
        {
            this.popUpEdit1.ClosePopup();
        }

        private void btnHup_Click(object sender, EventArgs e)
        {
            if (FocusedDrawBoard != null)
                FocusedDrawBoard.Hup();
        }

        private void btnHdown_Click(object sender, EventArgs e)
        {
            if (FocusedDrawBoard != null)
                FocusedDrawBoard.Hdown();
        }

        private void btnVup_Click(object sender, EventArgs e)
        {
            if (FocusedDrawBoard != null)
                FocusedDrawBoard.Vup();
        }

        private void btnVdown_Click(object sender, EventArgs e)
        {
            if (FocusedDrawBoard != null)
                FocusedDrawBoard.Vdown();
        }
        private void btnRe_Click(object sender, EventArgs e)
        {
            if (FocusedDrawBoard != null)
                FocusedDrawBoard.Re();
        }
        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            this.FocusedDrawBoard = null;
        }



    }
}
