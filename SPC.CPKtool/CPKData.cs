using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SPC.Base.Operation;

namespace SPC.CPKtool
{
    /// <summary>
    /// IList<Class>类型输入
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CPKClassData<T> : CPKData<IList<T>>
    {
        //Input
        private Type DataType { get; set; }
        public CPKClassData(IList<T> data, string param)
            : base(data, param)
        {
            this.DataType = typeof(T);
        }
        public CPKClassData()
            : base()
        {
            this.DataType = typeof(T);
        }
        //Method
        public override double? GetProperty(int index)
        {
            return Convert.ToDouble(this.DataType.GetProperty(this.Param).GetValue(this.Data[index], null));
        }
        public override bool ContainParam(string param)
        {
            return typeof(T).GetMember(param).Length != 0;
        }
        protected override int GetCount()
        {
            return this.Data.Count;
        }
    }

    /// <summary>
    /// DataTable格式输入
    /// </summary>
    public class CPKTableData : CPKData<DataTable>
    {
        //Method
        public override double? GetProperty(int index)
        {
            try
            {
                var temp = this.Data.Rows[index][Param];
                if (!CheckMethod.checkDoubleCanConvert(temp))
                    return 0;
                return Convert.ToDouble(temp);
            }
            catch
            {
                return 0;
            }
        }
        protected override int GetCount()
        {
            return this.Data.Rows.Count;
        }
        public override bool ContainParam(string param)
        {
            return this.Data.Columns.Contains(param);
        }
    }
    public class CPKCanChooseTableData : CPKData<DataTable>
    {
        public string ChooseColumnName = "choose";
        private int _ChooseCount = 0;
        public int ChooseCount
        {
            get
            {
                this._ChooseCount = 0;
                for (int i = 0; i < this.Data.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(this.Data.Rows[i][ChooseColumnName]) == true)
                        this._ChooseCount++;
                }
                return this._ChooseCount;
            }
        }
        //Method
        public override double? GetProperty(int index)
        {
            try
            {
                DataRow rowtemp = this.Data.Rows[index];
                if (rowtemp[ChooseColumnName].ToString() == false.ToString())
                    return null;
                var temp = rowtemp[Param];
                if (!CheckMethod.checkDoubleCanConvert(temp))
                    return 0;
                return Convert.ToDouble(temp);
            }
            catch
            {
                return 0;
            }
        }
        public override bool ContainParam(string param)
        {
            return this.Data.Columns.Contains(param) && this.Data.Columns.Contains(ChooseColumnName);
        }
        public CPKCanChooseTableData():base()
        {
        }
        protected override int GetCount()
        {
            return this.ChooseCount;
        }
    }
    public class CPKCanChooseGridViewData : CPKData<DevExpress.XtraGrid.Views.Grid.GridView>
    {
        //Method
        public override double? GetProperty(int index)
        {
            try
            {
                var temp = this.Data.GetRowCellValue(index, this.Param);
                if (!CheckMethod.checkDoubleCanConvert(temp))
                    return 0;
                return Convert.ToDouble(temp);
            }
            catch
            {
                return 0;
            }
        }
        public override bool ContainParam(string param)
        {
            return (this.Data.GridControl.DataSource as DataTable).Columns.Contains(param) && (this.Data.GridControl.DataSource as DataTable).Columns.Contains("choose");
        }
        protected override int GetCount()
        {
            return (this.Data.GridControl.DataSource as DataTable).Select("where choose = true").Length;
        }
        public override void getSTDev_G()
        {
            //if(this.GroupLength<1)
            //for(int i =0;i+)
            //this.Data.GetDataRowHandleByGroupRowHandle
        }
    }
}
