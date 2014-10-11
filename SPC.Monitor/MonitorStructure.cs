using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SPC.Base.Interface;
using SPC.Base.Operation;

namespace SPC.Monitor
{
    public class MonitorSourceDataType
    {
        public SPC.Base.Control.CanChooseDataGridView View;
        public String Param;
        public int GroupType;
        public MonitorSourceDataType(SPC.Base.Control.CanChooseDataGridView view, string param, int groupType)
        {
            this.View = view;
            this.Param = param;
            this.GroupType = groupType;
        }
    }
    public interface IDrawBoard
    {
        DevExpress.XtraCharts.ChartControl GetChart();
    }
}
