using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SPC.Base.Operation;

namespace SPC.Base.Interface
{
    public struct STDEVType
    {
        public string Description { get; set; }
        private static STDEVType _Std = new STDEVType("标准差", STDEV.stdGetGroup);
        public static STDEVType Std
        {
            get
            {
                return STDEVType._Std;
            }
        }
        private static STDEVType _NStd = new STDEVType("无估算标准差", STDEV.nstdGetGroup);
        public static STDEVType NStd
        {
            get
            {
                return STDEVType._NStd;
            }
        }
        private static STDEVType _Range = new STDEVType("极差", STDEV.rangeGetGroup);
        public static STDEVType Range
        {
            get
            {
                return STDEVType._Range;
            }
        }
        public Func<ICanGetProperty, double, int, int, int, double> Get;
        public STDEVType(string de, Func<ICanGetProperty, double, int, int, int, double> g)
            : this()
        {
            this.Description = de;
            this.Get = g;
        }
        public static bool operator ==(STDEVType a, STDEVType b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(STDEVType a, STDEVType b)
        {
            return !a.Equals(b);
        }
        public override bool Equals(object obj)
        {
            if (obj is STDEVType)
                return this.Description == ((STDEVType)obj).Description;
            else
                return false;
        }
        public override string ToString()
        {
            return this.Description;
        }
        public override int GetHashCode()
        {
            return this.Description.GetHashCode();
        }
    }

    public static class StandardConst
    {
        private static double[] _D2 = new double[] { double.NaN, 1, 1.128, 1.693, 2.059, 2.326, 2.543, 2.704, 2.847, 2.970, 3.078, 3.173, 3.258, 3.336, 3.407, 3.472, 3.532, 3.588, 3.640, 3.689, 3.735, 3.778, 3.819, 3.858, 3.895, 3.931 };
        public static double[] D2
        {
            get
            {
                return _D2;
            }
        }
        private static double[] _C4 = new double[] { double.NaN, 1, 0.797885, 0.886227, 0.921318, 0.939986, 0.951533, 0.959369, 0.965030, 0.969311, 0.972659, 0.975350, 0.977559, 0.979406, 0.980971, 0.982316, 0.983484, 0.984506, 0.985410, 0.986214, 0.986934, 0.987583, 0.988170, 0.988705, 0.989193, 0.989640 };
        public static double[] C4
        {
            get
            {
                return _C4;
            }
        }
    }
    public class BasicSeriesData
    {
        public string SeriesName;
        public List<string> X = new List<string>();
        public List<double> Y = new List<double>();
        public BasicSeriesData()
        {

        }
        public BasicSeriesData(string name)
        {
            this.SeriesName = name;
        }
        public override string ToString()
        {
            return SeriesName;
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    public class ViewData : IDataTable<DataRow>
    {
        SPC.Base.Control.CanChooseDataGridView View;
        DataTable sourceTable;
        public ViewData(SPC.Base.Control.CanChooseDataGridView view)
        {
            this.View = view;
            this.sourceTable = view.TableTypeData;
            view.GridControl.Invoke(new Action(() => { this.View.ActiveFilter.Add(view.Columns[view.ChooseColumnName], new DevExpress.XtraGrid.Columns.ColumnFilterInfo(view.ChooseColumnName + " = true")); }));
        }
        public DataRow this[int index]
        {
            get
            {
                return this.View.GetDataRow(index);
            }
        }
        public object this[int rowindex, int columnindex]
        {
            get
            {
                return this.View.GetRowCellValue(rowindex, this.View.VisibleColumns[columnindex]);
            }
            set
            {
                this.View.SetRowCellValue(rowindex, this.View.VisibleColumns[columnindex], value);
            }
        }

        public object this[int rowindex, string columnname]
        {
            get
            {
                return this.View.GetRowCellValue(rowindex, columnname);
            }
            set
            {
                this.View.SetRowCellValue(rowindex, columnname, value);
            }
        }
        public int RowCount
        {
            get
            {
                return this.View.DataRowCount;
            }
        }
        public int ColumnCount
        {
            get
            {
                return this.View.VisibleColumns.Count;
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
            return this.View.GetDataSourceRowIndex(i);
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

        public object Copy()
        {
            DataTable result = this.sourceTable.Clone();
            result.TableName = this.sourceTable.TableName + " - 副本";
            var rowcount = this.View.RowCount;
            for (int j = 0; j < rowcount; j++)
                result.Rows.Add(this.View.GetDataRow(j).ItemArray);
            var columns = this.View.VisibleColumns;
            var columncount = columns.Count;
            var totalcolumncount = columns.Count;
            int i;
            for (i = 0; i < columncount; i++)
                result.Columns[columns[i].FieldName].SetOrdinal(i);
            for (; i < totalcolumncount; i++)
                result.Columns.RemoveAt(i);
            return result;
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
        public DataRow NewRow()
        {
            return this.sourceTable.NewRow();
        }


        private int index = -1;


        public DataRow Current
        {
            get { return this.View.GetDataRow(index); }
        }

        public void Dispose()
        {
            index = -1;
        }

        object System.Collections.IEnumerator.Current
        {
            get { return this.View.GetDataRow(index); }
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
    public class ChoosedData : IDataTable<DataRow>
    {
        DataTable sourceTable;
        int[] choosedRows;
        
        public ChoosedData(DataTable data,int[] choosedrows)
        {
            this.sourceTable = data;
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
            int columncount = this.ColumnCount;
            string[] columns = new string[columncount];
            for (int i = 0; i < columncount; i++)
            {
                columns[i] = sourceTable.Columns[i].ColumnName;
            }
            return columns;
        }
        public string[] GetColumnsList(bool equal = true, params Type[] ttype)
        {
            int columncount = this.ColumnCount;
            List<string> columns = new List<string>();
            int typecount = ttype.Length;
            if (equal)
            {
                for (int i = 0; i < columncount; i++)
                {
                    for (int j = 0; j < typecount; j++)
                    {
                        if (sourceTable.Columns[i].DataType == ttype[j])
                        {
                            columns.Add(sourceTable.Columns[i].ColumnName);
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
                        if (sourceTable.Columns[i].DataType == ttype[j])
                        {
                            hav = true;
                            break;
                        }
                    }
                    if (!hav)
                        columns.Add(sourceTable.Columns[i].ColumnName);
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
        }
        public void AddColumn(string name, Type datatype)
        {
            this.sourceTable.Columns.Add(name, datatype);
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
        public bool SetColumnVisible(string name)
        {
            return false;
        }

        public bool SetColumnUnvisible(string name)
        {
            return false;
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
    public class ViewChoosedData : IDataTable<DataRow>
    {
        SPC.Base.Control.CanChooseDataGridView View;
        DataTable sourceTable;
        int[] choosedRows;
        public ViewChoosedData(SPC.Base.Control.CanChooseDataGridView view)
        {
            this.View = view;
            this.sourceTable = view.TableTypeData;
            this.choosedRows = view.GetChoosedRowIndexs();
        }
        public ViewChoosedData(SPC.Base.Control.CanChooseDataGridView view, int[] choosedrows)
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
    public class DoubleCompare : IComparer<double>
    {
        public int Compare(double x, double y)
        {
            if (x < y)
                return -1;
            return 1;
        }
    }
    public class DPoint
    {
        public double X;
        public double Y;
        public DPoint(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
        public override bool Equals(object obj)
        {
            DPoint target;
            if ((target = obj as DPoint) == null)
                return false;
            if (this.X == target.X && this.Y == target.Y)
                return true;
            return false;
        }
        public static bool operator ==(DPoint a, DPoint b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(DPoint a, DPoint b)
        {
            return !a.Equals(b);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    public class DPointValue :DPoint
    {
        private int i = 1;
        private double _Value;
        public double Value
        {
            get
            {
                return _Value / i;
            }
            set
            {
                this._Value = value;
                i = 1;
            }
        }
        public void AddValue(double value)
        {
            i++;
            this._Value += value;
        }
        public DPointValue(double x, double y,double value) :base(x,y)
        {
            this.Value = value;
        }
    }
    public class DPointCompare : IComparer<DPoint>
    {
        public int Compare(DPoint x, DPoint y)
        {
            if (x.X < y.X)
                return -1;
            else if (x.X == y.X && x.Y < y.Y)
                return -1;
            else
                return 1;
        }
    }
    public class DPointCompareY : IComparer<DPoint>
    {
        public int Compare(DPoint x, DPoint y)
        {
            if (x.Y < y.Y)
                return -1;
            else if (x.Y == y.Y && x.X < y.X)
                return -1;
            else
                return 1;
        }
    }
    public class WaitObject
    {
        public int[] Flags = null;
        public int Max = -1;
        public int GetProgress()
        {
            if (Flags == null||Max==-1)
                return 0;
            int progress = 0;
            foreach(var flag in Flags)
            {
                progress += flag;
            }
            return progress * 100 / Max;
        }
    }
}
