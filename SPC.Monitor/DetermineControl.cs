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
    public partial class DetermineControl : DevExpress.XtraEditors.XtraUserControl
    {
        DataTable Data;
        BindingSource DataBind = new BindingSource();
        public string ChooseColumnName = "choose";
        const int MaxSeriesCount = 50;
        int historySeriesCount = 0;
        DevExpress.XtraCharts.PaletteEntry[] Colors;
        DevExpress.XtraCharts.ChartControl basicColorChart = new DevExpress.XtraCharts.ChartControl();
        List<Type> DrawBoardTypes = new List<Type>();
        public DetermineControl()
        {
            InitializeComponent();
            this.gridControl1.DataSource = this.DataBind;
            this.bindingNavigator1.BindingSource = this.DataBind;
            this.bindingNavigator1.Items.Insert(10, new ToolStripControlHost(this.textEdit1));
            this.bindingNavigator1.Items.Insert(12, new ToolStripControlHost(this.textEdit2));
            this.bindingNavigator1.Items.Insert(14, new ToolStripControlHost(this.textEdit3));
            this.bindingNavigator1.Items.Insert(16, new ToolStripControlHost(this.checkedComboBoxEdit1));
            Colors = this.basicColorChart.GetPaletteEntries(MaxSeriesCount);
            this.checkedComboBoxEdit1.Properties.Items.AddRange(SPCCommand.GetCommandArray());
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
            var col = (e.DragObject as DevExpress.XtraGrid.Columns.GridColumn);
            if (col.FieldName != this.ChooseColumnName && this.Data.Columns[col.FieldName].DataType != typeof(string) && this.Data.Columns[col.FieldName].DataType!=typeof(DateTime))
            {
                var mouseposition = this.listBoxControl1.PointToClient(MousePosition);
                
                if (mouseposition.X > 0 && mouseposition.X < this.listBoxControl1.Width && mouseposition.Y > 0 && mouseposition.Y < this.listBoxControl1.Height)
                {
                    var temp = new SPCDetermineData(this.gridView1, col.FieldName, Convert.ToDouble(this.textEdit1.Text), Convert.ToDouble(this.textEdit2.Text), Convert.ToDouble(this.textEdit3.Text), this.getSeletedCommands(), this.Colors[historySeriesCount++ % MaxSeriesCount].Color, this.AddDrawBoards());
                    this.AddListItem(temp);
                }
                else if (this.xtraTabControl1.CalcHitInfo(this.xtraTabControl1.PointToClient(MousePosition)).HitTest == DevExpress.XtraTab.ViewInfo.XtraTabHitTest.PageClient && this.xtraTabControl1.SelectedTabPage.Controls.Count >= 0)
                {
                    var targetlayout = this.xtraTabControl1.SelectedTabPage.Controls[0];
                    var targetchart = targetlayout.GetChildAtPoint(targetlayout.PointToClient(MousePosition));
                    int index = targetlayout.Controls.IndexOf(targetchart);
                    if (index >= 0)
                    {
                        var temp = new SPCDetermineData(this.gridView1, col.FieldName, Convert.ToDouble(this.textEdit1.Text), Convert.ToDouble(this.textEdit2.Text), Convert.ToDouble(this.textEdit3.Text), this.getSeletedCommands(), this.Colors[historySeriesCount++ % MaxSeriesCount].Color, this.GetDrawBoards(index));
                        this.AddListItem(temp);
                    }
                    else
                    {
                        var temp = new SPCDetermineData(this.gridView1, col.FieldName, Convert.ToDouble(this.textEdit1.Text), Convert.ToDouble(this.textEdit2.Text), Convert.ToDouble(this.textEdit3.Text), this.getSeletedCommands(), this.Colors[historySeriesCount++ % MaxSeriesCount].Color, this.AddDrawBoards());
                        this.AddListItem(temp);
                    }
                }
            }
        }
        private List<SPCCommandbase> getSeletedCommands()
        {
            List<SPCCommandbase> result = new List<SPCCommandbase>();
            foreach(DevExpress.XtraEditors.Controls.CheckedListBoxItem item in this.checkedComboBoxEdit1.Properties.Items)
            {
                if (item.CheckState == CheckState.Checked)
                    result.Add(item.Value as SPCCommandbase);
            }
            return result;
        }
        private void AddListItem(SPCDetermineData lt)
        {
            this.listBoxControl1.Items.Insert(0,lt);
            lt.DrawSerieses();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var temp = this.listBoxControl1.SelectedItem as SPCDetermineData;
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
                var temp = item as SPCDetermineData;
                if (temp != null)
                {
                    temp.ClearSerieses();
                }
            }
            this.listBoxControl1.Items.Clear();
        }

        private void listBoxControl1_DrawItem(object sender, ListBoxDrawItemEventArgs e)
        {
            e.Appearance.ForeColor = (e.Item as SPCDetermineData).SeriesColor;
        }
        private SPCDetermineData currentItem;
        private SPCDetermineData focusItem;
        private void listBoxControl1_MouseClick(object sender, MouseEventArgs e)
        {
            int index = this.listBoxControl1.IndexFromPoint(e.Location);
            if (index >= 0 && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                currentItem = this.listBoxControl1.Items[index] as SPCDetermineData;
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
        private void FocusSeries(SPCDetermineData target)
        {
            foreach (var drawboard in target.DrawBoards)
            {
                drawboard.Focus();
                drawboard.BackColor = SystemColors.ActiveCaption;
            }
        }
        private void DFocusSeries(SPCDetermineData target)
        {
            foreach (var drawboard in target.DrawBoards)
                drawboard.BackColor = default(Color);
        }

        private void listBoxControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.listBoxControl1.IndexFromPoint(e.Location);
            if (index >= 0)
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    if (focusItem != null)
                        DFocusSeries(focusItem);
                    focusItem = (this.listBoxControl1.Items[index] as SPCDetermineData);
                    FocusSeries(focusItem);
                }
        }
        public class SPCDetermineData
        {
            private List<SingleSeriesManager<SPCDetermineDataType, DevExpress.XtraCharts.ChartControl>> SeriesManagers = new List<SingleSeriesManager<SPCDetermineDataType, DevExpress.XtraCharts.ChartControl>>();
            public List<DevExpress.XtraCharts.ChartControl> DrawBoards;
            private SPCDetermineDataType SourceData;
            public string Name;
            public System.Drawing.Color SeriesColor;
            private void InitSerieses()
            {
                foreach (var seriesManager in SeriesManagers)
                {
                    seriesManager.InitData(this.SourceData);
                }
            }
            public SPCDetermineData(SPC.Base.Control.CanChooseDataGridView view, string param, double ucl,double lcl,double standard,List<SPCCommandbase> commands, System.Drawing.Color color, List<DevExpress.XtraCharts.ChartControl> drawBoards)
            {
                SourceData = new SPCDetermineDataType(view, param, ucl,lcl,standard,commands);
                this.Name =param + "_" + DateTime.Now.ToBinary();
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
                this.SeriesManagers.Add(new SPCDetermineSeriesManager() { DrawBoard = this.DrawBoards[0] });

            }        
        }
        //在此添加新绘版
        private void InitDrawBoads()
        {
            this.DrawBoardTypes.Add(typeof(SPCDetermineDrawBoard));
        }

        private void MonitorControl_SizeChanged(object sender, EventArgs e)
        {
            chartSizeInit();
        }
        private void chartSizeInit()
        {
            this.panelControl1.Height = (int)(this.Size.Height * 0.5);
        }

    }
}
