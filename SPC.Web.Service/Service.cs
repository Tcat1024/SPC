using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using EAS.Services;

namespace SPC.Web.Service
{
    [ServiceObject]
    [ServiceBind(typeof(SPC.Web.Interface.ICPKtool))]
    public class Service:ServiceObject,SPC.Web.Interface.ICPKtool
    {
        public DataTable DebugGetTable(string cmd, string conn)
        {
            OleDbConnection con = new OleDbConnection(conn);
            OleDbDataAdapter adp = new OleDbDataAdapter(cmd,conn);
            adp.MissingSchemaAction = MissingSchemaAction.Add;
            DataTable result = new DataTable();
            adp.Fill(result);
            return result;
        }
        SPC.Web.Interface.WCFData<List<string>> SPC.Web.Interface.ICPKtool.GetTableNames(string conn)
        {
            OleDbConnection con = new OleDbConnection(conn);
            OleDbCommand com = new OleDbCommand("select * from user_tables order by table_name", con);
            try
            {
                con.Open();
                OleDbDataReader reader = com.ExecuteReader();
                List<string> result = new List<string>();
                while (reader.Read())
                {
                    result.Add(reader[0].ToString());
                }
                return new SPC.Web.Interface.WCFData<List<string>>(result);
            }
            catch (Exception ex)
            {
                return new SPC.Web.Interface.WCFData<List<string>>(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        public SPC.Web.Interface.WCFData<List<string>> GetColumnNames(string conn, string tablename)
        {
            OleDbConnection con = new OleDbConnection(conn);
            OleDbCommand com = new OleDbCommand("select aa.COLUMN_NAME,bb.CONSTRAINT_TYPE from (select column_name from user_tab_col" +
                "umns where table_name='" + tablename.ToUpper() + "' order by column_id) aa left join (select a.column_name,b.CONSTRAINT_TYPE " +
                "from user_cons_columns a, user_constraints b where a.constraint_name = b.constraint_name and b.constraint_type = 'P' " +
                "and a.table_name = '" + tablename.ToUpper() + "') bb on aa.column_name = bb.column_name", con);
            try
            {
                con.Open();
                OleDbDataReader reader = com.ExecuteReader();
                List<string> result = new List<string>();
                while (reader.Read())
                {

                    if (reader[1] != null)
                    {
                        if (reader[1].ToString() == "P")
                        {
                            result.Add(reader[0].ToString() + "(PK)");
                            continue;
                        }
                    }
                    result.Add(reader[0].ToString());
                }
                return new SPC.Web.Interface.WCFData<List<string>>(result);
            }
            catch (Exception ex)
            {
                return new SPC.Web.Interface.WCFData<List<string>>(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
    }
}
