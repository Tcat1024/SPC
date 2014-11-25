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
        public string SpectrumWith;
        public MonitorSourceDataType(SPC.Base.Control.CanChooseDataGridView view, string param, int groupType, string spectrumWith)
        {
            this.View = view;
            this.Param = param;
            this.GroupType = groupType;
            this.SpectrumWith = spectrumWith;
        }
    }
    public class XYRelationSourceDataType
    {
        public SPC.Base.Control.CanChooseDataGridView View;
        public String ParamX;
        public String ParamY;
        public XYRelationSourceDataType(SPC.Base.Control.CanChooseDataGridView view, string paramX, string paramY)
        {
            this.View = view;
            this.ParamX = paramX;
            this.ParamY = paramY;
        }
    }
    public class XYZRelationSourceDataType
    {
        public SPC.Base.Control.CanChooseDataGridView View;
        public String ParamX;
        public String ParamY;
        public String ParamZ;
        public XYZRelationSourceDataType(SPC.Base.Control.CanChooseDataGridView view, string paramX, string paramY, string paramZ)
        {
            this.View = view;
            this.ParamX = paramX;
            this.ParamY = paramY;
            this.ParamZ = paramZ;
        }
    }
    public class SPCDetermineDataType
    {
        public SPC.Base.Control.CanChooseDataGridView View;
        public String Param;
        public double UCL;
        public double LCL;
        public double Standard;
        public List<SPCCommandbase> Commands;
        public SPCDetermineDataType(SPC.Base.Control.CanChooseDataGridView view, string param, double ucl, double lcl, double standard, List<SPCCommandbase> commands)
        {
            this.View = view;
            this.Param = param;
            this.UCL = ucl;
            this.LCL = lcl;
            this.Standard = standard;
            this.Commands = commands;
        }
    }

    //public static class CanGetChartMethod
    //{
    //    public static DevExpress.XtraCharts.ChartControl getChart(this object input)
    //    {
    //        if (input is DevExpress.XtraCharts.ChartControl)
    //            return input as DevExpress.XtraCharts.ChartControl;
    //        else if (input is IDrawBoard)
    //            return (input as IDrawBoard).GetChart() as DevExpress.XtraCharts.ChartControl;
    //        else
    //            return null;
    //    }
    //}
}
