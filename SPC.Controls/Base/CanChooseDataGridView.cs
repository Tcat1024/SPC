using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;
using SPC.Base.Operation;

namespace SPC.Controls.Base
{
    public class CanChooseDataGridView : DevExpress.XtraGrid.Views.Grid.GridView
    {
        private DevExpress.XtraGrid.Columns.GridColumn chooseColumn;
        private string _ChooseColumnName = "choose";
        private bool innerChange = false;
        public DataTable TableTypeData = null;
        public string ChooseColumnName
        {
            get
            {
                return this._ChooseColumnName;
            }
            set
            {
                if (this._ChooseColumnName != value)
                {
                    this._ChooseColumnName = value;
                    InitSummary();
                }
            }
        }
        private int _ChooseCount = 0;
        private int ChooseCount
        {
            get
            {
                return _ChooseCount;
            }
            set
            {
                this._ChooseCount = value;
            }
        }
        private string _NumberDisplayFormat = "0.####";
        public string NumberDisplayFormat
        {
            get
            {
                return this._NumberDisplayFormat;
            }
            set
            {
                this._NumberDisplayFormat = value;
            }
        }
        private bool _AllowChoose;
        [DefaultValue(true)]
        public bool AllowChoose
        {
            get
            {
                return this._AllowChoose;
            }
            set
            {
                if (this._AllowChoose != value)
                {
                    if (value)
                        this._AllowChoose = true;
                    else
                    {
                        this._AllowChoose = false;
                        this._AllowChooseGroup = false;
                        this._AutoMode = false;
                    }
                }
            }
        }
        private bool _AllowChooseGroup;
        [DefaultValue(true)]
        public bool AllowChooseGroup
        {
            get
            {
                return _AllowChooseGroup;
            }
            set
            {
                if (this._AllowChooseGroup != value)
                {
                    if (value)
                    {
                        this._AllowChooseGroup = true;
                        this._AllowChoose = true;
                    }
                    else
                    {
                        this._AllowChooseGroup = false;
                    }
                }
            }
        }
        private bool _AutoMode;
        [DefaultValue(true)]
        public bool AutoMode
        {
            get
            {
                return this._AutoMode;
            }
            set
            {
                if (value != this._AutoMode)
                {
                    this._AutoMode = value;
                    if (value)
                        this._AllowChoose = true;
                }
            }
        }
        private bool _AutoRefresh = true;
        [DefaultValue(true)]
        public bool AutoRefresh
        {
            get
            {
                return this._AutoRefresh;
            }
            set
            {
                if (value != this._AutoRefresh)
                {
                    this._AutoRefresh = value;
                }
            }
        }
        public event EventHandler ChooseChanged;
        private ContextMenuStrip gridViewRightClickPopupMenu = new ContextMenuStrip();
        private DevExpress.Utils.Menu.DXMenuItem GroupDetailConfigButton;
        private Dictionary<DevExpress.XtraGrid.Columns.GridColumn, CustomGroupStringBuildForm> myGroupEditForms = new Dictionary<DevExpress.XtraGrid.Columns.GridColumn, CustomGroupStringBuildForm>();
        public CanChooseDataGridView()
            : base()
        {
            this.AutoMode = true;
            this.AllowChooseGroup = true;
            this.AutoRefresh = true;

            GroupDetailConfigButton = new DevExpress.Utils.Menu.DXMenuItem("GroupDetailConfig", GroupDetailConfigButtonClicked);

            this.InitChooseRelationControls();

            this.DataSourceChanged -= DataSourceChangeEventMethod;
            this.DataSourceChanged += DataSourceChangeEventMethod;

            this.CustomDrawGroupRow -= DrawChooseGroupRowEventMethod;
            this.CustomDrawGroupRow += DrawChooseGroupRowEventMethod;


            this.CustomSummaryCalculate -= ChooseSummaryCalculate;
            this.CustomSummaryCalculate += ChooseSummaryCalculate;

            this.Click -= ChooseViewClickEventMethod;
            this.Click += ChooseViewClickEventMethod;

            this.CustomDrawColumnHeader -= CustomDrawChooseColumnHeaderEventMethod;
            this.CustomDrawColumnHeader += CustomDrawChooseColumnHeaderEventMethod;

            this.MouseDown -= MouseDownEventMethod;
            this.MouseDown += MouseDownEventMethod;

            this.PopupMenuShowing -= CanChooseDataGridView_PopupMenuShowing;
            this.PopupMenuShowing += CanChooseDataGridView_PopupMenuShowing;
        }
        protected override void OnColumnAdded(DevExpress.XtraGrid.Columns.GridColumn column)
        {
            if ((column.ColumnType == typeof(double) || column.ColumnType == typeof(decimal) || column.ColumnType == typeof(float)) && this._NumberDisplayFormat != "")
            {
                column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                column.DisplayFormat.FormatString = this._NumberDisplayFormat;
            }
            base.OnColumnAdded(column);
        }
        protected override void OnColumnChanged(DevExpress.XtraGrid.Columns.GridColumn column)
        {
            if ((column.ColumnType == typeof(double) || column.ColumnType == typeof(decimal) || column.ColumnType == typeof(float)) && this._NumberDisplayFormat != "")
            {
                column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                column.DisplayFormat.FormatString = this._NumberDisplayFormat;
            }
            base.OnColumnChanged(column);
        }
        protected override void OnSummaryCollectionChanged(object sender, CollectionChangeEventArgs e)
        {
            Type a = sender.GetType();
            if (innerChange)
                base.OnSummaryCollectionChanged(sender, e);
            else
            {
                var summary = e.Element as DevExpress.XtraGrid.GridSummaryItem;
                if (!innerChange && summary != null && summary.FieldName == this.ChooseColumnName)
                    return;
                base.OnSummaryCollectionChanged(sender, e);
                if (AllowChoose && sender is DevExpress.XtraGrid.GridGroupSummaryItemCollection)
                    checkGroupSummary();
            }
        }
        private void checkColumnSummary()
        {
            innerChange = true;
            this.chooseColumn.Summary.Clear();
            this.chooseColumn.Summary.Add(this.ChooseNeedSummary);
            innerChange = false;
        }
        private void checkGroupSummary()
        {
            innerChange = true;
            for (int i = GroupSummary.Count - 1; i >= 0; i--)
            {
                if (GroupSummary[i].FieldName == ChooseColumnName)
                    GroupSummary.RemoveAt(i);
            }
            GroupSummary.Add(this.GroupChooseNeedSummary);
            innerChange = false;
        }
        protected override void OnColumnSummaryCollectionChanged(DevExpress.XtraGrid.Columns.GridColumn column, CollectionChangeEventArgs e)
        {
            if (!innerChange && column.FieldName == ChooseColumnName)
            {
                chooseColumn = column;
                checkColumnSummary();
            }
            base.OnColumnSummaryCollectionChanged(column, e);
        }
        protected override void RaiseCustomColumnGroup(DevExpress.XtraGrid.Views.Base.CustomColumnSortEventArgs e)
        {
            CustomGroupStringBuildForm form;
            if (myGroupEditForms.TryGetValue(e.Column, out form))
            {
                var format = form.Tag as List<Tuple<double, bool>>;
                if (format != null)
                {
                    e.Result = Comparer<int>.Default.Compare(getIndexOfFormatBoarders(Convert.ToDouble(e.Value1), format), getIndexOfFormatBoarders(Convert.ToDouble(e.Value2), format));
                    e.Handled = true;
                }
            }
            base.RaiseCustomColumnGroup(e);
        }
        protected override void RaiseCustomColumnSort(DevExpress.XtraGrid.Views.Base.CustomColumnSortEventArgs e)
        {
            CustomGroupStringBuildForm form;
            if (myGroupEditForms.TryGetValue(e.Column, out form))
            {
                var format = form.Tag as List<Tuple<double, bool>>;
                if (format != null)
                {
                    e.Result = Comparer<int>.Default.Compare(getIndexOfFormatBoarders(Convert.ToDouble(e.Value1), format), getIndexOfFormatBoarders(Convert.ToDouble(e.Value2), format));
                    e.Handled = true;
                }
            }
            base.RaiseCustomColumnSort(e);
        }
        protected override void OnBeforeGrouping()
        {
            refreshGroupForms();
            base.OnBeforeGrouping();
        }
        private void refreshGroupForms()
        {
            for (int i = this.myGroupEditForms.Count - 1; i >= 0; i--)
            {
                var key = myGroupEditForms.Keys.ElementAt(i);
                if (!this.GroupedColumns.Contains(key))
                {
                    key.SortMode = DevExpress.XtraGrid.ColumnSortMode.Default;
                    this.myGroupEditForms.Remove(key);
                }
            }
        }
        protected override void RaiseCustomDrawGroupRow(DevExpress.XtraGrid.Views.Base.RowObjectCustomDrawEventArgs e)
        {
            var info = e.Info as DevExpress.XtraGrid.Views.Grid.ViewInfo.GridGroupRowInfo;
            if (info != null)
            {
                CustomGroupStringBuildForm form;
                List<Tuple<double, bool>> format;
                if (this.myGroupEditForms.TryGetValue(info.Column, out form) && (format = (form.Tag as List<Tuple<double, bool>>)) != null)
                {
                    string sumText = this.GetGroupSummaryText(info.RowHandle);
                    string interval = "";
                    int index = getIndexOfFormatBoarders(Convert.ToDouble(this.GetGroupRowValue(info.RowHandle)), format);
                    if (index == 0)
                        interval = " Value " + "<" + (format[index].Item2 ? "=" : "") + format[index].Item1;
                    else if (index == format.Count)
                        interval = format[index - 1].Item1 + "<" + (!format[index - 1].Item2 ? "=" : "") + " Value ";
                    else
                        interval = format[index - 1].Item1 + "<" + (!format[index - 1].Item2 ? "=" : "") + " Value " + "<" + (format[index].Item2 ? "=" : "") + format[index].Item1;
                    info.GroupText = info.Column.FieldName + ": " + interval + " " + sumText;
                }
            }
            base.RaiseCustomDrawGroupRow(e);
        }
        private int getIndexOfFormatBoarders(double input, List<Tuple<double, bool>> format)
        {
            int index = format.Count;
            for (int i = 0; i < format.Count; i++)
            {
                if ((format[i].Item2 && Convert.ToDouble(input) <= format[i].Item1) || (!format[i].Item2 && Convert.ToDouble(input) < format[i].Item1))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
        private void GroupDetailConfigButtonClicked(object sender, EventArgs e)
        {
            DevExpress.XtraGrid.Columns.GridColumn column = (sender as DevExpress.Utils.Menu.DXMenuItem).Tag as DevExpress.XtraGrid.Columns.GridColumn;
            if (column == null)
                return;
            CustomGroupStringBuildForm form;
            if (!this.myGroupEditForms.TryGetValue(column, out form))
            {
                form = new CustomGroupStringBuildForm();
                form.StartPosition = FormStartPosition.WindowsDefaultLocation;
                this.myGroupEditForms.Add(column, form);
            }
            if (form.ShowDialog() == DialogResult.Yes)
            {
                form.Tag = CustomGroupMaker.FormatBorder(form.result);
                if (form.Tag == null)
                    column.SortMode = DevExpress.XtraGrid.ColumnSortMode.Default;
                else
                    column.SortMode = DevExpress.XtraGrid.ColumnSortMode.Custom;
                this.RefreshData();
            }
        }
        private void CanChooseDataGridView_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            if (e.MenuType == DevExpress.XtraGrid.Views.Grid.GridMenuType.Column)
            {
                var col = this.CalcHitInfo(e.Point).Column;
                if (col != null && col.ColumnType != typeof(string) && col.ColumnType != typeof(DateTime) && col.GroupIndex >= 0)
                {
                    GroupDetailConfigButton.Tag = col;
                    e.Menu.Items.Insert(7, GroupDetailConfigButton);
                }
            }
        }
        private void DataSourceChangeEventMethod(object sender, EventArgs e)
        {
            this.InitData(sender);
        }
        private object CurrentDataVector;
        public Type CurrentDataVectorType;
        public string CurrentDataUpdateEventName;

        public void InitData(object sender)
        {
            TableTypeData = null;
            System.Collections.IList list = null;
            Type rowType = null;
            object source = this.DataSource;
            int type = 0;
            while (true)
            {
                if (source is BindingSource)
                {
                    var bs = (source as BindingSource);
                    if (bs.DataSource is DataSet)
                    {
                        source = (bs.DataSource as DataSet).Tables[bs.DataMember];
                    }
                    else
                    {
                        source = bs.DataSource;
                    }
                    continue;
                }
                else if (source is DataTable)
                {
                    TableTypeData = (source as DataTable);
                    type = 1;
                    break;
                }
                else if (source is DataView)
                {
                    TableTypeData = (source as DataView).Table;
                    type = 1;
                    break;
                }
                else if (source is System.Collections.IList)
                {
                    list = source as System.Collections.IList;
                    type = 2;
                    break;
                }
                else
                    return;
            }
            if (this.AutoMode || this.AllowChoose)
            {
                if ((chooseColumn = this.Columns.ColumnByFieldName(ChooseColumnName)) != null)
                {
                    chooseColumn.VisibleIndex = 0;
                    chooseColumn.OptionsColumn.AllowEdit = false;
                    if (chooseColumn.Summary.IndexOf(ChooseNeedSummary) < 0)
                    {
                        this.innerChange = true;
                        chooseColumn.Summary.Add(ChooseNeedSummary);
                        this.innerChange = false;
                    }
                }
                else if (this.AutoMode)
                {
                    if (type == 1)
                    {
                        if (TableTypeData != null)
                        {
                            if (!TableTypeData.Columns.Contains(ChooseColumnName))
                            {
                                TableTypeData.Columns.Add(ChooseColumnName, typeof(bool));
                                for (int i = 0; i < TableTypeData.Rows.Count; i++)
                                    TableTypeData.Rows[i][ChooseColumnName] = true;
                            }
                            addChooseColumn();
                        }
                    }
                    else if (type == 2)
                    {
                        if (list != null && list.Count > 0)
                        {
                            rowType = list[0].GetType();
                            var c = rowType.GetProperty(ChooseColumnName);
                            if (c != null)
                            {
                                addChooseColumn();
                            }
                        }
                    }
                }
            }
            if (AllowChooseGroup && this.GroupSummary.IndexOf(GroupChooseNeedSummary) < 0)
            {
                this.innerChange = true;
                this.GroupSummary.Add(GroupChooseNeedSummary);
                this.innerChange = false;
            }
            else if (!AllowChooseGroup && this.GroupSummary.IndexOf(GroupChooseNeedSummary) >= 0)
            {
                this.innerChange = true;
                this.GroupSummary.Remove(GroupChooseNeedSummary);
                this.innerChange = false;
            }
        }
        private void addChooseColumn()
        {
            chooseColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            chooseColumn.Name = ChooseColumnName;
            chooseColumn.FieldName = ChooseColumnName;
            chooseColumn.VisibleIndex = 0;
            chooseColumn.OptionsColumn.AllowEdit = false;
            this.Columns.Insert(0, chooseColumn);

            this.innerChange = true;
            chooseColumn.Summary.Add(ChooseNeedSummary);
            this.innerChange = false;

        }

        private void GetDataVector(object target)
        {
            if (target == null)
                return;
            if (target is DataTable)
            {
                this.CurrentDataVector = target;
                this.CurrentDataVectorType = typeof(DataTable);
                this.CurrentDataUpdateEventName = "RowChanged";
            }
            else
            {
                var datasource = target.GetType().GetProperty("DataSource").GetValue(target, null);
                var datamember = target.GetType().GetProperty("DataMember").GetValue(target, null);
                if (datasource != null && datamember != null && datamember.ToString().Trim() != "" && datasource.GetType().GetProperty("Tables") != null)
                    GetDataVector(datasource.GetType().GetProperty("Tables").GetValue(datasource, new object[] { datamember }));
                else if (datasource != null)
                    GetDataVector(datasource);
            }
        }
        private void InitChooseRelationControls()
        {
            InitPopMenu();
            InitSummary();
        }
        public void InitPopMenu()
        {
            this.gridViewRightClickPopupMenu.ShowImageMargin = false;
            this.gridViewRightClickPopupMenu.RenderMode = ToolStripRenderMode.Professional;
            this.gridViewRightClickPopupMenu.Items.Add(new ToolStripButton("选       择", Resource.CheckMark_Glyph, this.MultiChooseButtonClick));
            this.gridViewRightClickPopupMenu.Items.Add(new ToolStripButton("取消选择", Resource.CrossMark_Glyph, this.MultiUnChooseButtonClick));
        }
        public void InitSummary()
        {
            GroupChooseNeedSummary = new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Custom, ChooseColumnName, null, "Count:{0}") { Tag = "GroupChooseNeedSummary" };
            ChooseNeedSummary = new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Custom, "", "{0}");
        }
        private void DataValueChangedEventMethod(object sender, EventArgs e)
        {
            if (this.AutoRefresh)
                this.RefreshChoose();
        }
        public void RefreshChoose()
        {
            this.UpdateSummary();
        }
        private void MultiChooseButtonClick(object sender, EventArgs e)
        {
            int[] index = this.GetSelectedRows();
            this.BeginDataUpdate();
            foreach (var i in index)
            {
                this.SetChooseRow(i, true);
            }
            this.EndDataUpdate();
        }
        private void MultiUnChooseButtonClick(object sender, EventArgs e)
        {
            int[] index = this.GetSelectedRows();
            this.BeginDataUpdate();
            foreach (var i in index)
            {
                this.SetChooseRow(i, false);
            }
            this.EndDataUpdate();
        }
        public void SetChooseRow(int index, bool result)
        {
            this.SetRowCellValue(index, ChooseColumnName, result);
        }
        private DevExpress.XtraGrid.GridGroupSummaryItem GroupChooseNeedSummary;
        private DevExpress.XtraGrid.GridColumnSummaryItem ChooseNeedSummary;
        private void CustomDrawChooseColumnHeaderEventMethod(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            if (this.AllowChoose && e.Column != null && e.Column.FieldName == ChooseColumnName)
            {
                e.Info.InnerElements.Clear();
                e.Info.Caption = "";
                e.Painter.DrawObject(e.Info);
                var repositoryCheck = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
                repositoryCheck.Caption = "";
                var info = repositoryCheck.CreateViewInfo() as DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo;
                var painter = repositoryCheck.CreatePainter() as DevExpress.XtraEditors.Drawing.CheckEditPainter;
                if (this.ChooseCount == this.DataRowCount)
                    info.EditValue = true;
                else if (this.ChooseCount > 0)
                    info.EditValue = null;
                else
                    info.EditValue = false;
                info.Bounds = e.Bounds;
                info.CalcViewInfo(e.Graphics);
                var args = new DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs(info, new DevExpress.Utils.Drawing.GraphicsCache(e.Graphics), e.Bounds);
                painter.Draw(args);
                args.Cache.Dispose();
                e.Handled = true;
            }

        }
        private MouseButtons currentGridViewMouseButton;
        private void MouseDownEventMethod(object sender, MouseEventArgs e)
        {
            this.currentGridViewMouseButton = e.Button;
        }
        private void ChooseViewClickEventMethod(object sender, EventArgs e)
        {
            //this.ClearSorting();
            if (this.AllowChoose && this.DataRowCount > 0 && this.GridControl != null)
            {
                if (this.currentGridViewMouseButton == System.Windows.Forms.MouseButtons.Left)
                {
                    this.PostEditor();
                    DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo info;
                    Point pt = this.GridControl.PointToClient(System.Windows.Forms.Control.MousePosition);
                    info = this.CalcHitInfo(pt);
                    if (info.InColumn && info.Column != null && info.Column.FieldName == ChooseColumnName)
                    {
                        bool re = !(this.ChooseCount == this.DataRowCount);
                        this.BeginDataUpdate();
                        /*Parallel.For(0, this.DataRowCount, (i) => */
                        for (int i = 0; i < this.DataRowCount; i++)
                        {
                            this.SetChooseRow(i, re);
                        };
                        this.EndDataUpdate();
                        if (ChooseChanged != null)
                            this.ChooseChanged(this, new EventArgs());
                    }
                    else if (this.AllowChooseGroup && info.RowHandle < 0 && groupCheckBoxPosition - info.HitPoint.X < 15)
                    {
                        int start = this.GetDataRowHandleByGroupRowHandle(info.RowHandle);
                        int end = ((GroupSummaryDataType)this.GetGroupSummaryValue(info.RowHandle, GroupChooseNeedSummary)).end;
                        int count = ((GroupSummaryDataType)this.GetGroupSummaryValue(info.RowHandle, GroupChooseNeedSummary)).choosecount;
                        if (count < end - start + 1)
                        {
                            this.BeginDataUpdate();
                            for (int i = start; i <= end; i++)
                            {
                                bool re = Convert.ToBoolean(this.GetRowCellValue(i, ChooseColumnName));
                                if (!re)
                                    this.SetChooseRow(i, true);
                            }
                            this.EndDataUpdate();
                            if (ChooseChanged != null)
                                this.ChooseChanged(this, new EventArgs());
                        }
                        else
                        {
                            this.BeginDataUpdate();
                            for (int i = start; i <= end; i++)
                                this.SetChooseRow(i, false);
                            this.EndDataUpdate();
                            if (ChooseChanged != null)
                                this.ChooseChanged(this, new EventArgs());
                        }
                    }
                    else if (info.Column != null && info.Column.FieldName == ChooseColumnName && info.RowHandle >= 0)
                    {
                        this.BeginDataUpdate();
                        bool re = Convert.ToBoolean(this.GetRowCellValue(info.RowHandle, ChooseColumnName));
                        if (re)
                            this.SetChooseRow(info.RowHandle, false);
                        else
                            this.SetChooseRow(info.RowHandle, true);
                        this.EndDataUpdate();
                        if (ChooseChanged != null)
                            this.ChooseChanged(this, new EventArgs());
                    }
                }
                else if (this.OptionsSelection.MultiSelect && this.OptionsSelection.MultiSelectMode == DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.RowSelect && this.currentGridViewMouseButton == System.Windows.Forms.MouseButtons.Right && this.SelectedRowsCount > 0)
                {
                    this.gridViewRightClickPopupMenu.Show(System.Windows.Forms.Control.MousePosition);
                }
            }
        }
        private int groupCheckBoxPosition = int.MaxValue;

        private void DrawChooseGroupRowEventMethod(object sender, DevExpress.XtraGrid.Views.Base.RowObjectCustomDrawEventArgs e)
        {
            e.Painter.DrawObject(e.Info);
            if (this.AllowChooseGroup)
            {
                var repositoryCheck = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
                repositoryCheck.Caption = "";
                repositoryCheck.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far;
                var info = repositoryCheck.CreateViewInfo() as DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo;
                var painter = repositoryCheck.CreatePainter() as DevExpress.XtraEditors.Drawing.CheckEditPainter;
                int start = this.GetDataRowHandleByGroupRowHandle(e.RowHandle);
                int end = ((GroupSummaryDataType)this.GetGroupSummaryValue(e.RowHandle, GroupChooseNeedSummary)).end;
                int count = ((GroupSummaryDataType)this.GetGroupSummaryValue(e.RowHandle, GroupChooseNeedSummary)).choosecount;
                if (count == end - start + 1)
                    info.EditValue = true;
                else if (count == 0)
                    info.EditValue = false;
                else
                    info.EditValue = null;
                info.Bounds = e.Bounds;
                info.CalcViewInfo(e.Graphics);
                groupCheckBoxPosition = e.Bounds.Left + e.Bounds.Width;
                var args = new DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs(info, new DevExpress.Utils.Drawing.GraphicsCache(e.Graphics), e.Bounds);
                painter.Draw(args);
                args.Cache.Dispose();
            }
            e.Handled = true;
        }
        private int TotalCount = 0;
        private int TempGroupChooseCount = 0;
        private int TempGroupCount = 0;
        private void ChooseSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            if (this.AllowChooseGroup && e.Item == this.GroupChooseNeedSummary)
            {
                if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
                {
                    TempGroupChooseCount = 0;
                    TempGroupCount = 0;
                }
                else if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
                {
                    TempGroupCount++;
                    if (Convert.ToBoolean(this.GetRowCellValue(e.RowHandle, ChooseColumnName)))
                        TempGroupChooseCount++;
                }
                else if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
                {
                    e.TotalValue = new GroupSummaryDataType(TempGroupChooseCount, e.RowHandle, TempGroupCount);
                }
            }
            else if (this.AllowChoose && e.Item == this.ChooseNeedSummary)
            {
                if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
                {
                    this.TotalCount = 0;
                }
                else if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate && Convert.ToBoolean(this.GetRowCellValue(e.RowHandle, ChooseColumnName)))
                {
                    this.TotalCount++;
                }
                else if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
                {
                    e.TotalValue = this.TotalCount;
                    this.ChooseCount = this.TotalCount;
                }
            }
        }
        private struct GroupSummaryDataType
        {
            public int choosecount;
            public int end;
            public int count;
            public GroupSummaryDataType(int cc, int e, int c)
            {
                this.choosecount = cc;
                this.count = c;
                this.end = e;
            }
            public override string ToString()
            {
                return choosecount.ToString() + "/" + count.ToString();
            }
        }
        public int GetChoosedCount()
        {
            return Convert.ToInt32(this.ChooseNeedSummary.SummaryValue);
        }
        public int[] GetChoosedRowIndexs()
        {
            if (this._AllowChoose)
            {
                List<int> result = new List<int>();
                if (TableTypeData != null)
                {
                    int count = TableTypeData.Rows.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (TableTypeData.Rows[i][ChooseColumnName].ToString() == Boolean.TrueString)
                        {
                            result.Add(i);
                        }
                    }
                }
                return result.ToArray();
            }
            else
                return null;
        }
        public string[] GetVisibleColumnNames(bool equal = true, params Type[] ttype)
        {
            int columncount = this.VisibleColumns.Count;
            List<string> columns = new List<string>();
            int typecount = ttype.Length;
            if (equal)
            {
                for (int i = 0; i < columncount; i++)
                {
                    for (int j = 0; j < typecount; j++)
                    {
                        if (this.VisibleColumns[i].ColumnType == ttype[j])
                        {
                            columns.Add(this.VisibleColumns[i].FieldName);
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < columncount; i++)
                {
                    bool hav = false;
                    for (int j = 0; j < typecount; j++)
                    {
                        if (this.VisibleColumns[i].ColumnType == ttype[j])
                        {
                            hav = true;
                            break;
                        }
                    }
                    if (!hav)
                        columns.Add(this.VisibleColumns[i].FieldName);
                }
            }
            if (columns.Contains(ChooseColumnName))
                columns.Remove(ChooseColumnName);
            return columns.ToArray();
        }
        public SPC.Base.Interface.IDataTable<DataRow> GetIDataTable()
        {
            return new ViewChoosedData(this);
        }
        public SPC.Base.Interface.IDataTable<DataRow> GetIDataTable(int[] choosed)
        {
            return new ViewChoosedData(this,choosed);
        }
        private class ViewChoosedData : SPC.Base.Interface.IDataTable<DataRow>
        {
            SPC.Controls.Base.CanChooseDataGridView View;
            DataTable sourceTable;
            int[] choosedRows;
            public ViewChoosedData(SPC.Controls.Base.CanChooseDataGridView view)
            {
                this.View = view;
                this.sourceTable = view.TableTypeData;
                this.choosedRows = view.GetChoosedRowIndexs();
            }
            public ViewChoosedData(SPC.Controls.Base.CanChooseDataGridView view, int[] choosedrows)
            {
                this.View = view;
                this.sourceTable = view.TableTypeData;
                this.choosedRows = choosedrows;
            }
            public DataRow this[int index]
            {
                get
                {
                    return this.sourceTable.Rows[choosedRows[index]];
                }
            }
            public object this[int rowindex, int columnindex]
            {
                get
                {
                    return this[rowindex][columnindex];
                }
                set
                {
                    this[rowindex][columnindex] = value;
                }
            }

            public object this[int rowindex, string columnname]
            {
                get
                {
                    return this[rowindex][columnname];
                }
                set
                {
                    this[rowindex][columnname] = value;
                }
            }
            public int RowCount
            {
                get
                {
                    return this.choosedRows.Length;
                }
            }
            public int ColumnCount
            {
                get
                {
                    return this.sourceTable.Columns.Count;
                }
            }
            public string Name
            {
                get
                {
                    return this.sourceTable.TableName;
                }
                set
                {
                    this.sourceTable.TableName = value;
                }
            }
            public object[] GetGroup(int index)
            {
                throw new NotImplementedException();
            }
            public int GetSourceIndex(int i)
            {
                return this.choosedRows[i];
            }
            public DataRow GetSourceRowbySourceIndex(int i)
            {
                return sourceTable.Rows[i];
            }
            public string[] GetColumnsList()
            {
                var columnlist = this.View.VisibleColumns;
                int columncount = columnlist.Count;
                string[] columns = new string[columncount];
                for (int i = 0; i < columncount; i++)
                {
                    columns[i] = columnlist[i].FieldName;
                }
                return columns;
            }
            public string[] GetColumnsList(bool equal = true, params Type[] ttype)
            {
                var columnlist = this.View.VisibleColumns;
                int columncount = columnlist.Count;
                List<string> columns = new List<string>();
                int typecount = ttype.Length;
                if (equal)
                {
                    for (int i = 0; i < columncount; i++)
                    {
                        for (int j = 0; j < typecount; j++)
                        {
                            if (columnlist[i].ColumnType == ttype[j])
                            {
                                columns.Add(columnlist[i].FieldName);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < columncount; i++)
                    {
                        bool hav = false;
                        for (int j = 0; j < typecount; j++)
                        {
                            if (columnlist[i].ColumnType == ttype[j])
                            {
                                hav = true;
                                break;
                            }
                        }
                        if (!hav)
                            columns.Add(columnlist[i].FieldName);
                    }
                }
                return columns.ToArray();
            }

            public Type GetColumnType(int index)
            {
                return this.sourceTable.Columns[index].DataType;
            }

            public Type GetColumnType(string name)
            {
                return this.sourceTable.Columns[name].DataType;
            }

            public bool ContainsColumn(string name)
            {
                return this.sourceTable.Columns.Contains(name); ;
            }

            public void AddColumn(string name)
            {
                this.sourceTable.Columns.Add(name);
                this.View.Columns.AddVisible(name);
            }
            public void AddColumn(string name, Type datatype)
            {
                this.sourceTable.Columns.Add(name, datatype);
                this.View.Columns.AddVisible(name);
            }
            public void RemoveColumn(string name)
            {
                this.sourceTable.Columns.Remove(name);
            }
            public bool SetColumnVisible(string name)
            {
                DevExpress.XtraGrid.Columns.GridColumn target;
                if (this.sourceTable.Columns.Contains(name) && (target = this.View.Columns.ColumnByFieldName(name)) != null && target.Visible == false)
                {
                    target.Visible = true;
                    return true;
                }
                return false;
            }

            public bool SetColumnUnvisible(string name)
            {
                DevExpress.XtraGrid.Columns.GridColumn target;
                if (this.sourceTable.Columns.Contains(name) && (target = this.View.Columns.ColumnByFieldName(name)) != null && target.Visible == true)
                {
                    target.Visible = false;
                    return true;
                }
                return false;
            }


            public object Copy()
            {
                DataTable result = this.sourceTable.Clone();
                result.TableName = this.sourceTable.TableName + " - 副本";
                var rowcount = this.RowCount;
                for (int j = 0; j < rowcount; j++)
                    result.Rows.Add(this[j].ItemArray);
                return result;
            }
            public DataRow NewRow()
            {
                return this.sourceTable.NewRow();
            }
            private int index = -1;


            public DataRow Current
            {
                get { return this[index]; }
            }

            public void Dispose()
            {
                index = -1;
            }

            object System.Collections.IEnumerator.Current
            {
                get { return this[index]; }
            }

            public bool MoveNext()
            {
                if (index == this.RowCount - 1)
                    return false;
                index++;
                return true;
            }

            public void Reset()
            {
                index = -1;
            }



        }
        private List<Tuple<int, string, int>> groupInfoTree;
        protected override void OnAfterGrouping()
        {
            base.OnAfterGrouping();
            groupInfoTree = new List<Tuple<int, string, int>>();
            int temp,level;
            if(this.GetDataRowHandleByGroupRowHandle(-1)>=0)
            {
                groupInfoTree.Add(new Tuple<int,string,int>(-1,EncodeGoupString(this.GetGroupRowPrintValue(-1).ToString()),0));
                for (int i = -2; ; i--)
                {
                    if ((temp = this.GetDataRowHandleByGroupRowHandle(i)) < 0)
                        break;
                    level = this.GetRowLevel(i);
                    if (level > this.GetRowLevel(i + 1))
                        groupInfoTree.Add(new Tuple<int, string, int>(i, EncodeGoupString(this.GetGroupRowPrintValue(i).ToString()), groupInfoTree[-i - 2].Item1));
                    else if (level == this.GetRowLevel(i + 1))
                        groupInfoTree.Add(new Tuple<int, string, int>(i, EncodeGoupString(this.GetGroupRowPrintValue(i).ToString()), groupInfoTree[-i - 2].Item3));
                    else
                    {
                        int j = i + 1;
                        while (j != 0 && this.GetRowLevel(j = groupInfoTree[-j - 1].Item3) != level) ;
                        groupInfoTree.Add(new Tuple<int, string, int>(i, EncodeGoupString(this.GetGroupRowPrintValue(i).ToString()), groupInfoTree[-j - 1].Item3));
                    }
                }
            }
        }
        protected string EncodeGoupString(string orginal)
        {
            int s =orginal.IndexOf(":");
            int e = orginal.IndexOf("Count:");           
            string result = orginal.Substring(s+1, e-s-1);
            return result.Trim();
        }
        public string GetGroupRowText(int index)
        {
            int t = -index-1;
            if (groupInfoTree == null || t < 0 || t >= groupInfoTree.Count)
                return null;
            string result = groupInfoTree[t].Item2;
            t = groupInfoTree[t].Item3;
            while(t<0)
            {
                t = -t - 1;
                result = groupInfoTree[t].Item2 +"+"+ result;
                t = groupInfoTree[t].Item3;
            }
            return result;
        }
    }

}
