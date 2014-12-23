using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPC.Base.Interface
{
    public interface IDataTable<T> : IEnumerator<T>
    {
        string Name { get; set; }
        int RowCount { get; }
        int ColumnCount { get; }
        T this[int index] { get; }
        object this[int rowindex, int columnindex] { get; set; }
        object this[int rowindex, string columnname] { get; set; }
        object[] GetGroup(int index);
        T NewRow();
        int GetSourceIndex(int i);
        T GetSourceRowbySourceIndex(int i);
        string[] GetColumnsList();
        string[] GetColumnsList(bool equal = true, params Type[] ttype);
        Type GetColumnType(int index);
        Type GetColumnType(string name);
        bool ContainsColumn(string name);
        void AddColumn(string name);
        void AddColumn(string name, Type datatype);
        bool SetColumnVisible(string name);
        bool SetColumnUnvisible(string name);
        object Copy();
    }
}
