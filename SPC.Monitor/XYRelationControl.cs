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
using SPC.Controls.Base;
using SPC.Monitor.DrawBoards;


namespace SPC.Monitor
{
    public partial class XYRelationControl : DevExpress.XtraEditors.XtraUserControl
    {
        DataTable Data;
        BindingSource DataBind = new BindingSource();
        public string ChooseColumnName = "choose";
        const int MaxSeriesCount = 50;
        int historySeriesCount = 0;
        DevExpress.XtraCharts.PaletteEntry[] Colors;
        DevExpress.XtraCharts.ChartControl basicColorChart = new DevExpress.XtraCharts.ChartControl();
        List<Type> DrawBoardTypes = new List<Type>();
        public XYRelationControl()
        {
            InitializeComponent();
            this.gridControl1.DataSource = this.DataBind;
            this.bindingNavigator1.BindingSource = this.DataBind;
            this.bindingNavigator1.Items.Insert(10,new ToolStripControlHost(this.cmbXAxis));
            Colors = this.basicColorChart.GetPaletteEntries(MaxSeriesCount);
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
            this.cmbXAxis.Properties.Items.Clear();
            this.cmbXAxis.Properties.Items.Add("Default");
            for(int i = 0;i<this.gridView1.Columns.Count;i++)
            {
                if (this.gridView1.Columns[i].ColumnType != typeof(string) && this.gridView1.Columns[i].ColumnType != typeof(bool)&&this.gridView1.Columns[i].ColumnType != typeof(DateTime))
                    this.cmbXAxis.Properties.Items.Add(this.gridView1.Columns[i].FieldName);
            }
        }
        private void gridView1_DragObjectDrop(object sender, DevExpress.XtraGrid.Views.Base.DragObjectDropEventArgs e)
        {
            XYRelationData data = null;
            List<IDrawBoard<DevExpress.XtraCharts.ChartControl>> DrawBoards = null;
            try
            {
                var col = (e.DragObject as DevExpress.XtraGrid.Columns.GridColumn);
                if (col.FieldName != this.ChooseColumnName && this.Data.Columns[col.FieldName].DataType != typeof(string) && this.Data.Columns[col.FieldName].DataType != typeof(DateTime))
                {
                    var mouseposition = this.listBoxControl1.PointToClient(MousePosition);
                    if (this.cmbXAxis.SelectedIndex < 0)
                        this.cmbXAxis.SelectedIndex = 0;
                    if (mouseposition.X > 0 && mouseposition.X < this.listBoxControl1.Width && mouseposition.Y > 0 && mouseposition.Y < this.listBoxControl1.Height)
                    {
                        data = new XYRelationData(this.gridView1, col.FieldName,this.cmbXAxis.Text, this.Colors[historySeriesCount++ % MaxSeriesCount].Color,DrawBoards = this.AddDrawBoards());
                        this.AddListItem(data);
                    }
                    else if (this.xtraTabControl1.CalcHitInfo(this.xtraTabControl1.PointToClient(MousePosition)).HitTest == DevExpress.XtraTab.ViewInfo.XtraTabHitTest.PageClient && this.xtraTabControl1.SelectedTabPage.Controls.Count >= 0)
                    {
                        var targetlayout = this.xtraTabControl1.SelectedTabPage.Controls[0];
                        int index = targetlayout.Controls.IndexOf(targetlayout.GetChildAtPoint(targetlayout.PointToClient(MousePosition)));
                        if (index >= 0)
                        {
                            data = new XYRelationData(this.gridView1, col.FieldName, this.cmbXAxis.Text, this.Colors[historySeriesCount++ % MaxSeriesCount].Color,DrawBoards = this.GetDrawBoards(index));
                            this.AddListItem(data);
                        }
                        else
                        {
                            data = new XYRelationData(this.gridView1, col.FieldName, this.cmbXAxis.Text, this.Colors[historySeriesCount++ % MaxSeriesCount].Color, DrawBoards = this.AddDrawBoards());
                            this.AddListItem(data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                if (DrawBoards != null)
                {
                    List<XYRelationData> templist;
                    foreach (var drawboard in DrawBoards)
                    {
                        if ((templist = drawboard.Tag as List<XYRelationData>) == null || templist.Count == 0)
                        {
                            drawboard.Parent.Controls.Remove(drawboard as Control);
                        }
                    }
                }
                RemoveListItem(data);
            }
        }
        private void AddListItem(XYRelationData lt)
        {
            this.listBoxControl1.Items.Insert(0,lt);
            this.listBoxControl1.SelectedIndex = 0;
            lt.DrawSerieses();
        }
        private void RemoveListItem(XYRelationData lt)
        {
            if (lt != null)
            {
                if (SelectedItem == lt)
                    DSelectDrawBoard(lt);
                lt.ClearSerieses();
                this.listBoxControl1.Items.Remove(lt);
            }
        }
        private void btnRemove_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var temp = this.listBoxControl1.SelectedItem as XYRelationData;
            if(temp!=null)
            {
                temp.ClearSerieses();
                this.listBoxControl1.Items.Remove(temp);
            }
        }

        private void btnClear_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach(var item in this.listBoxControl1.Items)
            {
                var temp = item as XYRelationData;
                if (temp != null)
                {
                    temp.ClearSerieses();
                }
            }
            this.listBoxControl1.Items.Clear();
        }

        private void listBoxControl1_DrawItem(object sender, ListBoxDrawItemEventArgs e)
        {
            e.Appearance.ForeColor = (e.Item as XYRelationData).SeriesColor;
        }
        private void listBoxControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right && this.listBoxControl1.Items[this.listBoxControl1.IndexFromPoint(e.Location)] == this.SelectedItem)
            {
                this.popupMenu1.ShowPopup(MousePosition);
            }
        }
        private List<IDrawBoard<DevExpress.XtraCharts.ChartControl>> AddDrawBoards()
        {
            List<IDrawBoard<DevExpress.XtraCharts.ChartControl>> drawBoards = new List<IDrawBoard<DevExpress.XtraCharts.ChartControl>>();
            for (int i = 0; i < this.xtraTabControl1.TabPages.Count; i++)
            {
                if (xtraTabControl1.TabPages[i].Controls.Count > 0 && this.DrawBoardTypes.Count > i)
                {
                    var temp = Activator.CreateInstance(this.DrawBoardTypes[i], null);
                    this.xtraTabControl1.TabPages[i].Controls[0].Controls.Add(temp as UserControl);
                    (temp as IDrawBoard<DevExpress.XtraCharts.ChartControl>).GotFocus += DrawBoard_GotFocus;
                    drawBoards.Add(temp as IDrawBoard<DevExpress.XtraCharts.ChartControl>);
                }
            }
            return drawBoards;
        }
        private List<IDrawBoard<DevExpress.XtraCharts.ChartControl>> GetDrawBoards(int Index)
        {
            List<IDrawBoard<DevExpress.XtraCharts.ChartControl>> drawBoards = new List<IDrawBoard<DevExpress.XtraCharts.ChartControl>>();
            for (int i = 0; i < this.xtraTabControl1.TabPages.Count; i++)
            {
                if (xtraTabControl1.TabPages[i].Controls.Count > 0 && xtraTabControl1.TabPages[i].Controls[0].Controls.Count > 0)
                    drawBoards.Add(xtraTabControl1.TabPages[i].Controls[0].Controls[Index] as IDrawBoard<DevExpress.XtraCharts.ChartControl>);
            }
            return drawBoards;
        }
        void DrawBoard_GotFocus(object sender, EventArgs e)
        {
            var s = sender as IDrawBoard<DevExpress.XtraCharts.ChartControl>;
            if (!s.Selected)
            {
                this.listBoxControl1.SelectedItem = (s.Tag as List<XYRelationData>)[0];
            }
        }
        private XYRelationData _SelectedItem = null;

        private XYRelationData SelectedItem
        {
            get
            {
                return this._SelectedItem;
            }
            set
            {
                this._SelectedItem = value;
                if (value != null)
                {
                    this.cmbXAxis.Text = value.SourceData.ParamX;
                    setAxisControlItemEnable(true);
                }
                else
                {
                    setAxisControlItemEnable(false);
                }
            }
        }
        private void btnRenameItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.bteRenameItem.Text = SelectedItem.Name;
            this.bteRenameItem.Visible = true;
            this.bteRenameItem.Focus();
        }

        private void btnDeleteItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SelectedItem.ClearSerieses();
            this.listBoxControl1.Items.Remove(SelectedItem);
        }

        private void buttonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            SelectedItem.Name = this.bteRenameItem.Text;
            this.bteRenameItem.Visible = false;
        }

        private void bteRenameItem_Leave(object sender, EventArgs e)
        {
            if (this.bteRenameItem.Visible)
                this.bteRenameItem.Visible = false;
        }

        private void bteRenameItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectedItem.Name = this.bteRenameItem.Text;
                this.bteRenameItem.Visible = false;
            }
        }
        private void SelectDrawBoard(XYRelationData target)
        {
            DSelectDrawBoard(SelectedItem);
            this.SelectedItem = target;
            foreach (var DrawBoard in target.DrawBoards)
            {
                DrawBoard.Selected = true;
            }
        }
        private void DSelectDrawBoard(XYRelationData target)
        {
            if (target != null)
                foreach (var DrawBoard in target.DrawBoards)
                {
                    DrawBoard.Selected = false;
                }
        }
        public class XYRelationData
        {
            private List<SingleSeriesManager<XYRelationSourceDataType, DevExpress.XtraCharts.ChartControl>> SeriesManagers = new List<SingleSeriesManager<XYRelationSourceDataType, DevExpress.XtraCharts.ChartControl>>();
            public List<IDrawBoard<DevExpress.XtraCharts.ChartControl>> DrawBoards;
            public XYRelationSourceDataType SourceData;
            public string Name;
            public System.Drawing.Color SeriesColor;
            private void InitSerieses()
            {
                foreach (var seriesManager in SeriesManagers)
                {
                    seriesManager.InitData(this.SourceData);
                }
            }
            public XYRelationData(SPC.Controls.Base.CanChooseDataGridView view, string paramY, string paramX, System.Drawing.Color color, List<IDrawBoard<DevExpress.XtraCharts.ChartControl>> drawBoards)
            {
                SourceData = new XYRelationSourceDataType(view, paramX, paramY);
                this.Name ="X:"+ paramX + "_Y:" + paramY + "_" + DateTime.Now.ToBinary();
                this.SeriesColor = color;
                this.DrawBoards = drawBoards;
                List<XYRelationData> templist;
                foreach (var drawboard in drawBoards)
                {
                    if (drawboard.Tag == null || (templist = drawboard.Tag as List<XYRelationData>) == null)
                        drawboard.Tag = new List<XYRelationData>() { this };
                    else
                        templist.Add(this);
                }
                InitSeriesManagers();
                InitData();
            }
            public void InitData()
            {
                foreach (var seriesManager in SeriesManagers)
                {
                    seriesManager.InitData(this.SourceData);
                }
            }
            public void RemoveSerieses()
            {
                foreach (var seriesManager in SeriesManagers)
                {
                    seriesManager.RemoveSeries();
                }
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
                RemoveSerieses();
                this.SeriesManagers.Clear();
                foreach (var drawboard in DrawBoards)
                {
                    var templist = drawboard.Tag as List<XYRelationData>;
                    templist.Remove(this);
                    if (templist.Count == 0)
                    {
                        drawboard.Parent.Controls.Remove(drawboard as Control);
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
                this.SeriesManagers.Add(new SampleXYRelationSeriesManager() { DrawBoard = this.DrawBoards[0] });

            }        
        }
        //在此添加新绘版
        private void InitDrawBoads()
        {
            this.DrawBoardTypes.Add(typeof(SampleXYRelationDrawBoard));
        }

        private void MonitorControl_SizeChanged(object sender, EventArgs e)
        {
            chartSizeInit();
        }
        private void chartSizeInit()
        {
            this.panelControl1.Height = (int)(this.Size.Height * 0.5);
        }
        private void setAxisControlItemEnable(bool target)
        {
            this.btnHdown.Enabled = target;
            this.btnHup.Enabled = target;
            this.btnVdown.Enabled = target;
            this.btnVup.Enabled = target;
            this.btnRe.Enabled = target;
        }
        private void btnHup_Click(object sender, EventArgs e)
        {
            if (SelectedItem != null)
                SelectedItem.DrawBoards[this.xtraTabControl1.SelectedTabPageIndex].Hup();
        }

        private void btnHdown_Click(object sender, EventArgs e)
        {
            if (SelectedItem != null)
                SelectedItem.DrawBoards[this.xtraTabControl1.SelectedTabPageIndex].Hdown();
        }

        private void btnVup_Click(object sender, EventArgs e)
        {
            if (SelectedItem != null)
                SelectedItem.DrawBoards[this.xtraTabControl1.SelectedTabPageIndex].Vup();
        }

        private void btnVdown_Click(object sender, EventArgs e)
        {
            if (SelectedItem != null)
                SelectedItem.DrawBoards[this.xtraTabControl1.SelectedTabPageIndex].Vdown();
        }
        private void btnRe_Click(object sender, EventArgs e)
        {
            if (SelectedItem != null)
                SelectedItem.DrawBoards[this.xtraTabControl1.SelectedTabPageIndex].Re();
        }

        private void listBoxControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i;
            if ((i = listBoxControl1.SelectedIndex) >= 0)
                this.SelectDrawBoard(listBoxControl1.Items[i] as XYRelationData);
            else
                this.DSelectDrawBoard(this.SelectedItem);
        }

        private void btnReDraw_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.SelectedItem != null)
            {
                if (this.cmbXAxis.SelectedIndex < 0)
                    this.cmbXAxis.SelectedIndex = 0;
                SelectedItem.SourceData.ParamY = this.cmbXAxis.Text;
                SelectedItem.InitData();
                SelectedItem.RemoveSerieses();
                SelectedItem.DrawSerieses();
            }
        }
    }
}
