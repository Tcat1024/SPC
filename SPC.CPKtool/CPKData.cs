using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SPC.Base.Operation;

namespace SPC.CPKtool
{

    public class CPKCanChooseViewData : CPKData<SPC.Controls.Base.CanChooseDataGridView>
    {
        public string ChooseColumnName = "choose";
        public override int ChooseCount
        {
            get
            {
                return this._Data.GetChoosedCount();
            }
        }
        //Method
        public override double? GetProperty(int index)
        {
            try
            {
                DataRow rowtemp = this._Data.GetDataRow(index);
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
            return this._Data.Columns.ColumnByFieldName(param)!=null && this.Data.Columns.ColumnByFieldName(ChooseColumnName)!=null;
        }
        public CPKCanChooseViewData()
            : base()
        {
        }
    }
}
