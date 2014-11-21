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
            //if (!this.Data.Columns.Contains(ChooseColumnName))
            //{
            //    var choosecolumn = new DataColumn(ChooseColumnName, typeof(string));
            //    this.Data.Columns.Add(choosecolumn);
            //}
            //int co = this.Data.Rows.Count;
            //for (int i = 0; i < co; i++)
            //{
            //    this.Data.Rows[i][ChooseColumnName] = true;
            //}
            this.gridView1.Columns.Clear();
            this.DataBind.DataSource = this.Data;
        }
        private void gridView1_DragObjectDrop(object sender, DevExpress.XtraGrid.Views.Base.DragObjectDropEventArgs e)
        {
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
                        var temp = new MonitorSeriesData(this.gridView1, col.FieldName, grouptype, spectrumwith, this.Colors[historySeriesCount++ % MaxSeriesCount].Color, this.AddDrawBoards());
                        this.AddListItem(temp);
                    }
                    else if (this.xtraTabControl1.CalcHitInfo(this.xtraTabControl1.PointToClient(MousePosition)).HitTest == DevExpress.XtraTab.ViewInfo.XtraTabHitTest.PageClient && this.xtraTabControl1.SelectedTabPage.Controls.Count >= 0)
                    {
                        var targetlayout = this.xtraTabControl1.SelectedTabPage.Controls[0];
                        var targetchart = targetlayout.GetChildAtPoint(targetlayout.PointToClient(MousePosition));
                        int index = targetlayout.Controls.IndexOf(targetchart);
                        if (index >= 0)
                        {
                            var temp = new MonitorSeriesData(this.gridView1, col.FieldName, grouptype, spectrumwith, this.Colors[historySeriesCount++ % MaxSeriesCount].Color, this.GetDrawBoards(index));
                            this.AddListItem(temp);
                        }
                        else
                        {
                            var temp = new MonitorSeriesData(this.gridView1, col.FieldName, grouptype, spectrumwith, this.Colors[historySeriesCount++ % MaxSeriesCount].Color, this.AddDrawBoards());
                            this.AddListItem(temp);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void AddListItem(MonitorSeriesData lt)
        {
            this.listBoxControl1.Items.Insert(0,lt);
            lt.DrawSerieses();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var temp = this.listBoxControl1.SelectedItem as MonitorSeriesData;
            if(temp!=null)
            {
                temp.ClearSerieses();
                this.listBoxControl1.Items.Remove(temp);
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach(var item in this.listBoxControl1.Items)
            {
                var temp = item as MonitorSeriesData;
                if (temp != null)
                {
                    temp.ClearSerieses();
                }
            }
            this.listBoxControl1.Items.Clear();
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
                DFocusSeries(focusItem);
                focusItem = null;
            }
        }
        private List<DevExpress.XtraCharts.ChartControl> AddDrawBoards()
        {
            List<DevExpress.XtraCharts.ChartControl> drawBoards = new List<DevExpress.XtraCharts.ChartControl>();
            for (int i = 0; i < this.xtraTabControl1.TabPages.Count; i++)
            {
                if (xtraTabControl1.TabPages[i].Controls.Count > 0&&this.DrawBoardTypes.Count>i)
                {
                    var temp = Activator.CreateInstance(this.DrawBoardTypes[i], null);
                    this.xtraTabControl1.TabPages[i].Controls[0].Controls.Add(temp as UserControl);
                    drawBoards.Add(temp.getChart());
                }
            }
            return drawBoards;
        }
        private List<DevExpress.XtraCharts.ChartControl> GetDrawBoards(int Index)
        {
            List<DevExpress.XtraCharts.ChartControl> drawBoards = new List<DevExpress.XtraCharts.ChartControl>();
            for (int i = 0; i < this.xtraTabControl1.TabPages.Count; i++)
            {
                if (xtraTabControl1.TabPages[i].Controls.Count > 0 && xtraTabControl1.TabPages[i].Controls[0].Controls.Count>0)
                    drawBoards.Add(xtraTabControl1.TabPages[i].Controls[0].Controls[Index].getChart());
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
        private void FocusSeries(MonitorSeriesData target)
        {
            foreach (var drawboard in target.DrawBoards)
            {
                drawboard.Focus();
                drawboard.BackColor = SystemColors.ActiveCaption;
            }
        }
        private void DFocusSeries(MonitorSeriesData target)
        {
            foreach (var drawboard in target.DrawBoards)
                drawboard.BackColor = default(Color);
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
                        DFocusSeries(focusItem);
                    focusItem = (this.listBoxControl1.Items[index] as MonitorSeriesData);
                    FocusSeries(focusItem);
                }
        }
        public class MonitorSeriesData
        {
            private List<SingleSeriesManager<MonitorSourceDataType, DevExpress.XtraCharts.ChartControl>> SeriesManagers = new List<SingleSeriesManager<MonitorSourceDataType, DevExpress.XtraCharts.ChartControl>>();
            public List<DevExpress.XtraCharts.ChartControl> DrawBoards;
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
            public MonitorSeriesData(SPC.Base.Control.CanChooseDataGridView view, string param, int groupType, string spectrumWith, System.Drawing.Color color, List<DevExpress.XtraCharts.ChartControl> drawBoards)
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
                foreach (var seriesManager in SeriesManagers)
                {
                    if (seriesManager.RemoveSeries())
                    {
                        seriesManager.DrawBoard.Parent.Controls.Remove(seriesManager.DrawBoard);
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
                this.SeriesManagers.Add(new GroupRangeSeriesManager() { DrawBoard = this.DrawBoards[1] });
                this.SeriesManagers.Add(new GroupAvgDataRunSeriesManager() { DrawBoard = this.DrawBoards[2] });
                this.SeriesManagers.Add(new SampleRunGroupPointsManager() { DrawBoard = this.DrawBoards[2] });
                this.SeriesManagers.Add(new NormalityCheckPointsManager() { DrawBoard = this.DrawBoards[3] });
                this.SeriesManagers.Add(new SpectralDistributionPointsManager() { DrawBoard = this.DrawBoards[4] });
                this.SeriesManagers.Add(new BoxPlotSeriesManager() { DrawBoard = this.DrawBoards[5]});
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
    //    private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    //    {
    //        int index =this.listBoxControl1.SelectedIndex;
    //        if(index>0)
    //        {
    //            var temp = this.listBoxControl1.SelectedItem;
    //            this.listBoxControl1.Items.Remove(temp);
    //            this.listBoxControl1.Items.Insert(index - 1, temp);
    //            this.listBoxControl1.SelectedIndex = index - 1;
    //            int count = (temp as MonitorSeriesData).DrawBoards.Count;
    //            for(int i = 0;i<count;i++)
    //            {
    //                if((temp as MonitorSeriesData).DrawBoards[i].Parent is IDrawBoard)
    //                {
    //                    var precontrol = (temp as MonitorSeriesData).DrawBoards[i].Parent;
    //                    Control tarcontrol;
    //                    if((this.listBoxControl1.Items[index] as MonitorSeriesData).DrawBoards[i].Parent is IDrawBoard)
    //                        tarcontrol = (this.listBoxControl1.Items[index] as MonitorSeriesData).DrawBoards[i].Parent;
    //                    else
    //                        tarcontrol = (this.listBoxControl1.Items[index] as MonitorSeriesData).DrawBoards[i];

    //                    int x = precontrol.Location.X;
    //                    int y = precontrol.Location.Y;
    //                    precontrol.Dock = DockStyle.None;
    //                    tarcontrol.Dock = DockStyle.None;
    //                    precontrol.Location = new Point(tarcontrol.Location.X,tarcontrol.Location.Y);
    //                    tarcontrol.Location = new Point(x, y);
    //                    tarcontrol.Dock = DockStyle.Top;
    //                    precontrol.Parent.Refresh();
    //                    precontrol.Dock = DockStyle.Top;


    //                }
    //                else
    //                {

    //                }
    //            }
    //        }
    //    }

    //    private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    //    {
    //        int index = this.listBoxControl1.SelectedIndex;
    //        if (index >= 0&&index<this.listBoxControl1.Items.Count-1)
    //        {
    //            var temp = this.listBoxControl1.SelectedItem;
    //            this.listBoxControl1.Items.Remove(temp);
    //            this.listBoxControl1.Items.Insert(index + 1, temp);
    //            this.listBoxControl1.SelectedIndex = index + 1;
    //        }
    //    }
    }
}
