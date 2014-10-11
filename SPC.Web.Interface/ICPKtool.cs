using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SPC.Web.Interface
{
    public interface ICPKtool
    {
        DataTable DebugGetTable(string cmd, string conn);
        WCFData<List<string>> GetTableNames(string conn);
        WCFData<List<string>> GetColumnNames(string conn, string tablename);

    }
    public class WCFData<T>
    {
        public T data;
        public string ex = "";
        public WCFData(T data)
        {
            this.data = data;
        }
        public WCFData(string ex)
        {
            this.ex = ex;
        }
    }

}
