using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPC.Base.Operation;
using SPC.Base.Interface;

namespace SPC.Monitor
{

    #region GroupAvgSeries
    public class GroupAvgSeriesMaker : ISeriesMaker<MonitorSourceDataType>
    {
        private double? doOpreation(int s, int e, MonitorSourceDataType sourcedata)
        {
            if (s >= e)
                return null;
            var view = sourcedata.View;
            var groupType = sourcedata.GroupType;
            var param = sourcedata.Param;
            double result = 0;
            int count = 0;
            for (int i = s; i < e; i++)
            {
                var rowtemp = view.GetDataRow(i);
                if (rowtemp[view.ChooseColumnName].ToString() == true.ToString())
                {
                    var temp = rowtemp[param];
                    if (!CheckMethod.checkDoubleCanConvert(temp))
                        result += 0;
                    else result += Convert.ToDouble(temp);
                    count++;
                }
            }
            if (count == 0)
                return null;
            return result / count;
        }
        public BasicSeriesData Make(MonitorSourceDataType sourcedata)
        {
            var view = sourcedata.View;
            var groupType = sourcedata.GroupType;
            var param = sourcedata.Param;
            BasicSeriesData result = new BasicSeriesData();
            if (groupType < 2)
            {
                int x = 0;
                int start = view.GetDataRowHandleByGroupRowHandle(-1);
                int end = 0;
                if (start >= 0)
                {
                    for (int i = -2; ; i--)
                    {
                        int flag = view.GetDataRowHandleByGroupRowHandle(i);
                        end = flag < 0 ? view.DataRowCount : flag;
                        var y = doOpreation(start, end, sourcedata);
                        if (y != null)
                        {
                            x++;
                            result.X.Add(x.ToString());
                            result.Y.Add((double)y);
                        }
                        if (flag < 0)
                            break;
                        start = end;
                    }
                }
            }
            else
            {
                int x = 0;
                double y = 0;
                for (int i = 0, j = 0; i < view.DataRowCount; i++)
                {
                    var rowtemp = view.GetDataRow(i);
                    if (rowtemp[view.ChooseColumnName].ToString() == true.ToString())
                    {
                        var temp = rowtemp[param];
                        if (!CheckMethod.checkDoubleCanConvert(temp))
                            y += 0;
                        else y += Convert.ToDouble(temp);
                        j++;
                        if (j == groupType)
                        {
                            y = y / groupType;
                            j = 0;
                            x++;
                            result.X.Add(x.ToString());
                            result.Y.Add(y);
                            y = 0;
                        }
                    }
                }
            }
            return result;
        }
    }
    public class GroupAvgSeriesDrawer : ISeriesDrawer<DevExpress.XtraCharts.ChartControl>
    {
        private DevExpress.XtraCharts.Series Series;
        private DevExpress.XtraCharts.ChartControl DrawBoard;
        public void Draw(BasicSeriesData data, System.Drawing.Color color, DevExpress.XtraCharts.ChartControl drawBoard)
        {
            DrawBoard = drawBoard;
            Series = new DevExpress.XtraCharts.Series();
            Series.View = DrawBoard.Series[0].View.Clone() as DevExpress.XtraCharts.SeriesViewBase;
            Series.View.Color = color;
            Series.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Qualitative;
            DrawBoard.Series.Add(Series);
            Series.Points.BeginUpdate();
            Series.Points.Clear();
            for (int i = 0; i < data.Y.Count; i++)
                Series.Points.Add(new DevExpress.XtraCharts.SeriesPoint(data.X[i], data.Y[i]));
            Series.Points.EndUpdate();
        }

        public bool Clear()
        {
            if (Series != null && DrawBoard != null)
            {
                this.DrawBoard.Series.Remove(Series);
            }
            if (this.DrawBoard.Series.Count == 2)
                return true;
            else
                return false;
        }
    }
    public class GroupAvgSeriesManager : SingleSeriesManager<MonitorSourceDataType, DevExpress.XtraCharts.ChartControl>
    {
        protected override void Init()
        {
            this.SeriesMaker = new GroupAvgSeriesMaker();
            this.SeriesDrawer = new GroupAvgSeriesDrawer();
        }
    }
    #endregion

    #region GroupRangeSeries
    public class GroupRangeSeriesMaker : ISeriesMaker<MonitorSourceDataType>
    {
        private double? doOpreation(int s, int e, MonitorSourceDataType sourcedata)
        {
            if (s >= e)
                return null;
            var view = sourcedata.View;
            var groupType = sourcedata.GroupType;
            var param = sourcedata.Param;
            double max = double.MinValue;
            double min = double.MaxValue;
            int count = 0;
            for (int i = s; i < e; i++)
            {
                var rowtemp = view.GetDataRow(i);
                if (rowtemp[view.ChooseColumnName].ToString() == true.ToString())
                {
                    var temp = rowtemp[param];
                    double y = 0;
                    if (!CheckMethod.checkDoubleCanConvert(temp))
                        y = 0;
                    else y = Convert.ToDouble(temp);
                    if (y > max)
                        max = y;
                    if (y < min)
                        min = y;
                    count++;
                }
            }
            if (count == 0)
                return null;
            return max - min;
        }
        public BasicSeriesData Make(MonitorSourceDataType sourcedata)
        {
            var view = sourcedata.View;
            var groupType = sourcedata.GroupType;
            var param = sourcedata.Param;
            BasicSeriesData result = new BasicSeriesData();
            if (groupType < 2)
            {
                int x = 0;
                int start = view.GetDataRowHandleByGroupRowHandle(-1);
                int end = 0;
                if (start >= 0)
                {
                    for (int i = -2; ; i--)
                    {
                        int flag = view.GetDataRowHandleByGroupRowHandle(i);
                        end = flag < 0 ? view.DataRowCount : flag;
                        var y = doOpreation(start, end, sourcedata);
                        if (y != null)
                        {
                            x++;
                            result.X.Add(x.ToString());
                            result.Y.Add((double)y);
                        }
                        if (flag < 0)
                            break;
                        start = end;
                    }
                }
            }
            else
            {
                int x = 0;
                double max = double.MinValue;
                double min = double.MaxValue;
                double y = 0;
                for (int i = 0, j = 0; i < view.DataRowCount; i++)
                {
                    var rowtemp = view.GetDataRow(i);
                    if (rowtemp[view.ChooseColumnName].ToString() == true.ToString())
                    {
                        var temp = rowtemp[param];
                        if (!CheckMethod.checkDoubleCanConvert(temp))
                            y = 0;
                        else
                            y = Convert.ToDouble(temp);
                        if (y > max)
                            max = y;
                        if (y < min)
                            min = y;
                        j++;
                        if (j == groupType)
                        {
                            y = max - min;
                            j = 0;
                            x++;
                            result.X.Add(x.ToString());
                            result.Y.Add(y);
                            max = double.MinValue;
                            min = double.MaxValue;
                        }
                    }
                }
            }
            return result;
        }

    }
    public class GroupRangeSeriesDrawer : ISeriesDrawer<DevExpress.XtraCharts.ChartControl>
    {
        private DevExpress.XtraCharts.Series Series;
        private DevExpress.XtraCharts.ChartControl DrawBoard;
        public void Draw(BasicSeriesData data, System.Drawing.Color color, DevExpress.XtraCharts.ChartControl drawBoard)
        {
            DrawBoard = drawBoard;
            Series = new DevExpress.XtraCharts.Series();
            Series.View = DrawBoard.Series[1].View.Clone() as DevExpress.XtraCharts.SeriesViewBase;
            Series.View.Color = color;
            Series.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Qualitative;
            DrawBoard.Series.Add(Series);
            Series.Points.BeginUpdate();
            Series.Points.Clear();
            for (int i = 0; i < data.Y.Count; i++)
                Series.Points.Add(new DevExpress.XtraCharts.SeriesPoint(data.X[i], data.Y[i]));
            Series.Points.EndUpdate();
        }

        public bool Clear()
        {
            if (Series != null && DrawBoard != null)
            {
                this.DrawBoard.Series.Remove(Series);
            }
            if (this.DrawBoard.Series.Count == 2)
                return true;
            else
                return false;
        }
    }
    public class GroupRangeSeriesManager : SingleSeriesManager<MonitorSourceDataType, DevExpress.XtraCharts.ChartControl>
    {
        protected override void Init()
        {
            this.SeriesMaker = new GroupRangeSeriesMaker();
            this.SeriesDrawer = new GroupRangeSeriesDrawer();
        }
    }
    #endregion

    #region SampleRunSeries
    public class SampleRunSeriesMaker : ISeriesMaker<MonitorSourceDataType>
    {
        public BasicSeriesData Make(MonitorSourceDataType sourcedata)
        {
            var view = sourcedata.View;
            var groupType = sourcedata.GroupType;
            var param = sourcedata.Param;
            BasicSeriesData result = new BasicSeriesData();
            int x = 0;
            double y = 0;
            for (int i = 0; i < view.DataRowCount; i++)
            {
                var rowtemp = view.GetDataRow(i);
                if (rowtemp[view.ChooseColumnName].ToString() == true.ToString())
                {
                    var temp = rowtemp[param];
                    if (!CheckMethod.checkDoubleCanConvert(temp))
                        y = 0;
                    else
                        y = Convert.ToDouble(temp);
                    x++;
                    result.X.Add(x.ToString());
                    result.Y.Add(y);
                }
            }
            return result;
        }

    }
    public class SampleRunSeriesDrawer : ISeriesDrawer<DevExpress.XtraCharts.ChartControl>
    {
        private DevExpress.XtraCharts.Series Series;
        private DevExpress.XtraCharts.ChartControl DrawBoard;
        public void Draw(BasicSeriesData data, System.Drawing.Color color, DevExpress.XtraCharts.ChartControl drawBoard)
        {
            DrawBoard = drawBoard;
            Series = new DevExpress.XtraCharts.Series();
            Series.View = DrawBoard.Series[0].View.Clone() as DevExpress.XtraCharts.SeriesViewBase;
            Series.View.Color = color;
            Series.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Qualitative;
            DrawBoard.Series.Add(Series);
            Series.Points.BeginUpdate();
            Series.Points.Clear();
            for (int i = 0; i < data.Y.Count; i++)
                Series.Points.Add(new DevExpress.XtraCharts.SeriesPoint(data.X[i], data.Y[i]));
            Series.Points.EndUpdate();
        }

        public bool Clear()
        {
            if (Series != null && DrawBoard != null)
            {
                this.DrawBoard.Series.Remove(Series);
            }
            if (this.DrawBoard.Series.Count == 1)
                return true;
            else
                return false;
        }
    }
    public class SampleRunSeriesManager : SingleSeriesManager<MonitorSourceDataType, DevExpress.XtraCharts.ChartControl>
    {
        protected override void Init()
        {
            this.SeriesMaker = new SampleRunSeriesMaker();
            this.SeriesDrawer = new SampleRunSeriesDrawer();
        }
    }
    #endregion

    #region GroupAvgDataRunSeries
    public class GroupAvgDataRunSeriesDrawer : ISeriesDrawer<DevExpress.XtraCharts.ChartControl>
    {
        private DevExpress.XtraCharts.Series Series;
        private DevExpress.XtraCharts.ChartControl DrawBoard;
        public void Draw(BasicSeriesData data, System.Drawing.Color color, DevExpress.XtraCharts.ChartControl drawBoard)
        {
            DrawBoard = drawBoard;
            Series = new DevExpress.XtraCharts.Series();
            Series.View = DrawBoard.Series[0].View.Clone() as DevExpress.XtraCharts.SeriesViewBase;
            Series.View.Color = color;
            Series.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Qualitative;
            DrawBoard.Series.Add(Series);
            Series.Points.BeginUpdate();
            Series.Points.Clear();
            for (int i = 0; i < data.Y.Count; i++)
                Series.Points.Add(new DevExpress.XtraCharts.SeriesPoint(data.X[i], data.Y[i]));
            Series.Points.EndUpdate();
        }

        public bool Clear()
        {
            if (Series != null && DrawBoard != null)
            {
                this.DrawBoard.Series.Remove(Series);
            }
            if (this.DrawBoard.Series.Count == 2)
                return true;
            else
                return false;
        }
    }
    public class GroupAvgDataRunSeriesManager : SingleSeriesManager<MonitorSourceDataType, DevExpress.XtraCharts.ChartControl>
    {
        protected override void Init()
        {
            this.SeriesMaker = new GroupAvgSeriesMaker();
            this.SeriesDrawer = new GroupAvgDataRunSeriesDrawer();
        }
    }
    #endregion

    #region SampleRunPoints
    public class SampleRunPointsMaker : ISeriesMaker<MonitorSourceDataType>
    {
        public BasicSeriesData Make(MonitorSourceDataType sourcedata)
        {
            var view = sourcedata.View;
            var groupType = sourcedata.GroupType;
            var param = sourcedata.Param;
            BasicSeriesData result = new BasicSeriesData();
            if (groupType < 2)
            {
                int x = 1;
                int start = view.GetDataRowHandleByGroupRowHandle(-1);
                int end = 0;
                if (start >= 0)
                {
                    for (int i = -2; ; i--)
                    {
                        int flag = view.GetDataRowHandleByGroupRowHandle(i);
                        end = flag < 0 ? view.DataRowCount : flag;
                        int count = 0;
                        for (int j = start; j < end; j++)
                        {
                            var rowtemp = view.GetDataRow(j);
                            if (rowtemp[view.ChooseColumnName].ToString() == true.ToString())
                            {
                                var temp = rowtemp[param];
                                if (!CheckMethod.checkDoubleCanConvert(temp))
                                    result.Y.Add(0);
                                else result.Y.Add(Convert.ToDouble(temp));
                                result.X.Add(x.ToString());
                                count++;
                            }
                        }
                        if (count > 0)
                            x++;
                        if (flag < 0)
                            break;
                        start = end;
                    }
                }
            }
            else
            {
                int x = 1;
                int j = 0;
                double y = 0;
                for (int i = 0; i < view.DataRowCount; i++)
                {
                    var rowtemp = view.GetDataRow(i);
                    if (rowtemp[view.ChooseColumnName].ToString() == true.ToString())
                    {
                        var temp = rowtemp[param];
                        if (!CheckMethod.checkDoubleCanConvert(temp))
                            y = 0;
                        else
                            y = Convert.ToDouble(temp);
                        j++;
                        result.X.Add(x.ToString());
                        result.Y.Add(y);
                        if (j == groupType)
                        {
                            j = 0;
                            x++;
                        }
                    }
                }
                for (int i = 0; i < j; i++)
                {
                    result.X.RemoveAt(result.X.Count - 1 - i);
                    result.Y.RemoveAt(result.Y.Count - 1 - i);
                }
            }
            return result;
        }

    }
    public class SampleRunPointsDrawer : ISeriesDrawer<DevExpress.XtraCharts.ChartControl>
    {
        private DevExpress.XtraCharts.Series Series;
        private DevExpress.XtraCharts.ChartControl DrawBoard;
        public void Draw(BasicSeriesData data, System.Drawing.Color color, DevExpress.XtraCharts.ChartControl drawBoard)
        {
            DrawBoard = drawBoard;
            Series = new DevExpress.XtraCharts.Series();
            Series.View = DrawBoard.Series[1].View.Clone() as DevExpress.XtraCharts.SeriesViewBase;
            Series.View.Color = color;
            Series.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Qualitative;
            DrawBoard.Series.Add(Series);
            Series.Points.BeginUpdate();
            Series.Points.Clear();
            for (int i = 0; i < data.Y.Count; i++)
                Series.Points.Add(new DevExpress.XtraCharts.SeriesPoint(data.X[i], data.Y[i]));
            Series.Points.EndUpdate();
        }

        public bool Clear()
        {
            if (Series != null && DrawBoard != null)
            {
                this.DrawBoard.Series.Remove(Series);
            }
            if (this.DrawBoard.Series.Count == 2)
                return true;
            else
                return false;
        }
    }
    public class SampleRunPointsManager : SingleSeriesManager<MonitorSourceDataType, DevExpress.XtraCharts.ChartControl>
    {
        protected override void Init()
        {
            this.SeriesMaker = new SampleRunPointsMaker();
            this.SeriesDrawer = new SampleRunPointsDrawer();
        }
    }
    #endregion

    #region NormalityCheck
    public class NormalityCheckPointsMaker : ISeriesMaker<MonitorSourceDataType>
    {
        public BasicSeriesData Make(MonitorSourceDataType sourcedata)
        {
            var view = sourcedata.View;
            var groupType = sourcedata.GroupType;
            var param = sourcedata.Param;
            BasicSeriesData result = new BasicSeriesData();
            List<double> sortdata = new List<double>();
            for (int i = 0; i < view.DataRowCount; i++)
            {
                var rowtemp = view.GetDataRow(i);
                if (rowtemp[view.ChooseColumnName].ToString() == true.ToString())
                {
                    var temp = rowtemp[param];
                    if (!CheckMethod.checkDoubleCanConvert(temp))
                        sortdata.Add(0);
                    else
                        sortdata.Add(Convert.ToDouble(temp));
                }
            }
            sortdata.Sort();
            int count = sortdata.Count;
            for (int i = 0; i < sortdata.Count; i++)
            {
                result.X.Add(sortdata[i].ToString());
                result.Y.Add(100 * ((double)i / count));
            }
            return result;
        }

    }
    public class NormalityCheckPointsDrawer : ISeriesDrawer<DevExpress.XtraCharts.ChartControl>
    {
        private DevExpress.XtraCharts.Series Series;
        private DevExpress.XtraCharts.ChartControl DrawBoard;
        public void Draw(BasicSeriesData data, System.Drawing.Color color, DevExpress.XtraCharts.ChartControl drawBoard)
        {
            DrawBoard = drawBoard;
            Series = new DevExpress.XtraCharts.Series();
            Series.View = DrawBoard.Series[0].View.Clone() as DevExpress.XtraCharts.SeriesViewBase;
            Series.View.Color = color;
            (Series.View as DevExpress.XtraCharts.PointSeriesView).Indicators[0].Color = color;
            DrawBoard.Series.Add(Series);
            Series.Points.BeginUpdate();
            Series.Points.Clear();
            for (int i = 0; i < data.Y.Count; i++)
                Series.Points.Add(new DevExpress.XtraCharts.SeriesPoint(data.X[i], data.Y[i]));
            Series.Points.EndUpdate();
        }

        public bool Clear()
        {
            if (Series != null && DrawBoard != null)
            {
                this.DrawBoard.Series.Remove(Series);
            }
            if (this.DrawBoard.Series.Count == 1)
                return true;
            else
                return false;
        }
    }
    public class NormalityCheckPointsManager : SingleSeriesManager<MonitorSourceDataType, DevExpress.XtraCharts.ChartControl>
    {
        protected override void Init()
        {
            this.SeriesMaker = new NormalityCheckPointsMaker();
            this.SeriesDrawer = new NormalityCheckPointsDrawer();
        }
    }
    #endregion

    #region SpectralDistribution
    public class SpectralDistributionPointsMaker : ISeriesMaker<MonitorSourceDataType>
    {
        public BasicSeriesData Make(MonitorSourceDataType sourcedata)
        {
            var view = sourcedata.View;
            var groupType = sourcedata.GroupType;
            var param = sourcedata.Param;
            var spectrumWith = sourcedata.SpectrumWith;
            BasicSeriesData result = new BasicSeriesData();
            int count = 0;
            double min = double.MaxValue;
            double max = double.MinValue;
            int r;
            for (int i = 0; i < view.DataRowCount; i++)
            {
                var rowtemp = view.GetDataRow(i);
                if (rowtemp[view.ChooseColumnName].ToString() == true.ToString())
                {
                    var temp = rowtemp[param];
                    double tempy = 0;
                    if (CheckMethod.checkDoubleCanConvert(temp))
                        tempy = Convert.ToDouble(temp);
                    if (tempy < min)
                        min = tempy;
                    if (tempy > max)
                        max = tempy;
                    count++;
                }
            }
            if(count==0)
                return result;
            if (int.TryParse(spectrumWith, out r))
            {
                if (r == 0)
                    r = (int)Math.Pow(count, 0.5);
                double width = (max - min) / r;
                if(width<=0)
                {
                    width = max-min;
                    r = 1;
                    result.X.Add(min.ToString());
                    result.Y.Add(count);
                    result.X.Add(max.ToString());
                    result.Y.Add(count);
                    return result;
                }
                int[] y = new int[r];
                for (int i = 0; i < view.DataRowCount; i++)
                {
                    var rowtemp = view.GetDataRow(i);
                    if (rowtemp[view.ChooseColumnName].ToString() == true.ToString())
                    {
                        var temp = rowtemp[param];
                        double tempy = 0;
                        if (CheckMethod.checkDoubleCanConvert(temp))
                            tempy = Convert.ToDouble(temp);
                        int t = (int)((tempy - min) / width);
                        y[t<r?t:r-1]++;
                    }
                }
                for(int i = 0;i<r;i++)
                {
                    result.X.Add((min+ i * width).ToString());
                    result.Y.Add(y[i]);
                }
                result.X.Add(max.ToString());
                result.Y.Add(y[r-1]);
            }
            else
            {
                var data = CustomGroupMaker.FormatBorder(spectrumWith);
                int dcount = data.Count;
                int[] y = new int[dcount+1];
                for (int i = 0; i < view.DataRowCount; i++)
                {
                    var rowtemp = view.GetDataRow(i);
                    if (rowtemp[view.ChooseColumnName].ToString() == true.ToString())
                    {
                        var temp = rowtemp[param];
                        double tempy = 0;
                        if (CheckMethod.checkDoubleCanConvert(temp))
                            tempy = Convert.ToDouble(temp);
                        if ((data[dcount - 1].Item2 && tempy > data[dcount - 1].Item1) || (!data[dcount - 1].Item2 && tempy >= data[dcount - 1].Item1))
                            y[dcount]++;
                        else
                        {
                            for (int j = 0; j < dcount; j++)
                            {
                                if (data[j].Item2)
                                {
                                    if (tempy <= data[j].Item1)
                                    {
                                        y[j]++;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (tempy < data[j].Item1)
                                    {
                                        y[j]++;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                //result.X.Add(" " + "<" + (data[0].Item2 ? "=" : "") + string.Format("{0:F2}", data[0].Item1));
                //result.Y.Add(y[0]);
                //for (int i = 1; i < dcount; i++)
                //{
                //    result.X.Add(string.Format("{0:F2}", data[i - 1].Item1) + "<" + (data[i - 1].Item2 ? "" : "=") + " " + "<" + (data[i].Item2 ? "=" : "") + string.Format("{0:F2}", data[i].Item1));
                //    result.Y.Add(y[i]);
                //}
                //result.X.Add(string.Format("{0:F2}", data[dcount-1].Item1) + "<" + (data[dcount - 1].Item2 ? "" : "=")+ " ");
                //result.Y.Add(y[dcount]);
                result.X.Add(min.ToString());
                result.Y.Add(y[0]);
                for (int i = 1; i < dcount+1; i++)
                {
                    result.X.Add(data[i - 1].Item1.ToString());
                    result.Y.Add(y[i]);
                }
                result.X.Add(max.ToString());
                result.Y.Add(y[dcount]);
            }
            return result;
        }
    }
    public class SpectralDistributionPointsDrawer : ISeriesDrawer<DevExpress.XtraCharts.ChartControl>
    {
        private DevExpress.XtraCharts.Series Series;
        private DevExpress.XtraCharts.ChartControl DrawBoard;
        public void Draw(BasicSeriesData data, System.Drawing.Color color, DevExpress.XtraCharts.ChartControl drawBoard)
        {
            DrawBoard = drawBoard;
            Series = new DevExpress.XtraCharts.Series();
            Series.View = DrawBoard.Series[0].View.Clone() as DevExpress.XtraCharts.SeriesViewBase;
            Series.View.Color = color;
            Series.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Numerical;
            Series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            Series.Label.PointOptions.PointView = DevExpress.XtraCharts.PointView.Argument;
            Series.Label.PointOptions.ArgumentNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.Number;
            Series.Label.ResolveOverlappingMode = DevExpress.XtraCharts.ResolveOverlappingMode.JustifyAroundPoint;
            Series.Label.TextOrientation = DevExpress.XtraCharts.TextOrientation.TopToBottom;
            DrawBoard.Series.Add(Series);
            Series.Points.BeginUpdate();
            Series.Points.Clear();
            for (int i = 0; i < data.Y.Count; i++)
                Series.Points.Add(new DevExpress.XtraCharts.SeriesPoint(data.X[i], data.Y[i]));
            Series.Points.EndUpdate();
        }

        public bool Clear()
        {
            if (Series != null && DrawBoard != null)
            {
                this.DrawBoard.Series.Remove(Series);
            }
            if (this.DrawBoard.Series.Count == 1)
                return true;
            else
                return false;
        }
    }
    public class SpectralDistributionPointsManager : SingleSeriesManager<MonitorSourceDataType, DevExpress.XtraCharts.ChartControl>
    {
        protected override void Init()
        {
            this.SeriesMaker = new SpectralDistributionPointsMaker();
            this.SeriesDrawer = new SpectralDistributionPointsDrawer();
        }
    }
    #endregion
}
