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

        public void Dispose()
        {
            Series.Dispose();
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

        public void Dispose()
        {
            this.Series.Dispose();
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

        public void Dispose()
        {
            this.Series.Dispose();
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

        public void Dispose()
        {
            this.Series.Dispose();
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

        public void Dispose()
        {
            this.Series.Dispose();
        }
    }
    public class SampleRunGroupPointsManager : SingleSeriesManager<MonitorSourceDataType, DevExpress.XtraCharts.ChartControl>
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

        public void Dispose()
        {
            this.Series.Dispose();
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
        private DevExpress.XtraCharts.Series[] Series;
        private DevExpress.XtraCharts.ChartControl DrawBoard;
        public void Draw(BasicSeriesData data, System.Drawing.Color color, DevExpress.XtraCharts.ChartControl drawBoard)
        {
            DrawBoard = drawBoard;
            Series = new DevExpress.XtraCharts.Series[3];
            Series[0] = new DevExpress.XtraCharts.Series();
            Series[0].View = DrawBoard.Series[0].View.Clone() as DevExpress.XtraCharts.SeriesViewBase;
            Series[0].View.Color = color;
            Series[0].ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Numerical;
            Series[0].LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            Series[0].Label.PointOptions.PointView = DevExpress.XtraCharts.PointView.Argument;
            Series[0].Label.PointOptions.ArgumentNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.Number;
            Series[0].Label.ResolveOverlappingMode = DevExpress.XtraCharts.ResolveOverlappingMode.JustifyAroundPoint;
            Series[0].Label.TextOrientation = DevExpress.XtraCharts.TextOrientation.TopToBottom;
            DrawBoard.Series.Add(Series[0]);
            Series[0].Points.BeginUpdate();
            Series[0].Points.Clear();
            double f = 0.5;
            var newColor = System.Drawing.Color.FromArgb((int)color.A,(int)(color.R + (255 - color.R) * f), (int)(color.G + (255 - color.G) * f), (int)(color.B + (255 - color.B) * f));
            for (int i = 1; i < 3; i++)
            {
                Series[i] = new DevExpress.XtraCharts.Series();
                Series[i].View = DrawBoard.Series[i].View.Clone() as DevExpress.XtraCharts.SeriesViewBase;
                Series[i].View.Color = newColor;
                Series[i].ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Numerical;
                DrawBoard.Series.Add(Series[i]);
                Series[i].Points.BeginUpdate();
                Series[i].Points.Clear();
            }
            double sum = 0;
            for (int i = 0; i < data.Y.Count; i++)
            {
                Series[0].Points.Add(new DevExpress.XtraCharts.SeriesPoint(data.X[i], data.Y[i]));
                sum += data.Y[i];
            }
            Series[1].Points.Add(new DevExpress.XtraCharts.SeriesPoint(data.X[0], 0));
            Series[2].Points.Add(new DevExpress.XtraCharts.SeriesPoint(data.X[0], 0));
            double y2 = 0;
            for (int i = 0; i < data.Y.Count - 1; i++)
            {
                double y1 = data.Y[i] * 100 / sum;
                Series[1].Points.Add(new DevExpress.XtraCharts.SeriesPoint((Convert.ToDouble(data.X[i]) + Convert.ToDouble(data.X[i + 1])) / 2, y1));
                y2 += y1;
                Series[2].Points.Add(new DevExpress.XtraCharts.SeriesPoint((Convert.ToDouble(data.X[i]) + Convert.ToDouble(data.X[i + 1])) / 2, y2));
            }
            for (int i = 0; i < 3; i++)
            {
                Series[i].Points.EndUpdate();
            }
        }
        public bool Clear()
        {
            if (Series != null && DrawBoard != null)
            {
                for (int i = Series.Length - 1; i >= 0; i--)
                    this.DrawBoard.Series.Remove(Series[i]);
            }
            if (this.DrawBoard.Series.Count == 3)
                return true;
            else
                return false;
        }

        public void Dispose()
        {
            for (int i = Series.Length - 1; i >= 0; i--)
                Series[i].Dispose();
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

    #region SampleXYRelationSeries
    public class SampleXYRelationSeriesMaker : ISeriesMaker<XYRelationSourceDataType>
    {
        public BasicSeriesData Make(XYRelationSourceDataType sourcedata)
        {
            var view = sourcedata.View;
            var paramX = sourcedata.ParamX;
            var paramY = sourcedata.ParamY;
            BasicSeriesData result = new BasicSeriesData();
            int x = 0;
            double y = 0;
            if (paramX == "Default")
            {
                for (int i = 0; i < view.DataRowCount; i++)
                {
                    var rowtemp = view.GetDataRow(i);
                    if (rowtemp[view.ChooseColumnName].ToString() == true.ToString())
                    {
                        var tempy = rowtemp[paramY];
                        if (!CheckMethod.checkDoubleCanConvert(tempy))
                            y = 0;
                        else
                            y = Convert.ToDouble(tempy);
                        x++;

                        result.X.Add(x.ToString());
                        result.Y.Add(y);
                    }
                }
            }
            else
            {
                for (int i = 0; i < view.DataRowCount; i++)
                {
                    var rowtemp = view.GetDataRow(i);
                    if (rowtemp[view.ChooseColumnName].ToString() == true.ToString())
                    {
                        var tempy = rowtemp[paramY];
                        if (!CheckMethod.checkDoubleCanConvert(tempy))
                            y = 0;
                        else
                            y = Convert.ToDouble(tempy);
                        x++;

                        result.X.Add(rowtemp[paramX].ToString());
                        result.Y.Add(y);
                    }
                }
            }
            return result;
        }

    }
    public class SampleXYRelationSeriesDrawer : ISeriesDrawer<DevExpress.XtraCharts.ChartControl>
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
            DrawBoard.Series.Add(Series);
            Series.Points.BeginUpdate();
            Series.Points.Clear();
            double xyS = 0;
            double xS = 0;
            double yS = 0;
            double x2S = 0;
            double y2S = 0;
            int count = data.Y.Count;
            for (int i = 0; i < count; i++)
            {
                var x = Convert.ToDouble(data.X[i]);
                var y = Convert.ToDouble(data.Y[i]);
                Series.Points.Add(new DevExpress.XtraCharts.SeriesPoint(data.X[i], data.Y[i]));
                xyS += x * y;
                xS += x;
                yS += y;
                x2S += x * x;
                y2S += y * y;
            }
            double result = (xyS*count - xS*yS)/(Math.Pow((x2S*count-xS*xS),0.5)*Math.Pow((y2S*count-yS*yS),0.5));
            Series.Name = string.Format("{0:N3}",result);
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

        public void Dispose()
        {
            this.Series.Dispose();
        }
    }
    public class SampleXYRelationSeriesManager : SingleSeriesManager<XYRelationSourceDataType, DevExpress.XtraCharts.ChartControl>
    {
        protected override void Init()
        {
            this.SeriesMaker = new SampleXYRelationSeriesMaker();
            this.SeriesDrawer = new SampleXYRelationSeriesDrawer();
        }
    }
    #endregion

    #region BoxPlotSeries
    public class BoxPlotSeriesMaker : ISeriesMaker<MonitorSourceDataType>
    {
        private bool groupAnalysis(int s, int e, MonitorSourceDataType sourcedata,BasicSeriesData result)
        {
            if (s >= e)
                return false;
            var view = sourcedata.View;
            var groupType = sourcedata.GroupType;
            var param = sourcedata.Param;
            List<double> ys = new List<double>();
            for (int i = s; i < e; i++)
            {
                var rowtemp = view.GetDataRow(i);
                if (rowtemp[view.ChooseColumnName].ToString() == true.ToString())
                {
                    var temp = rowtemp[param];
                    if (!CheckMethod.checkDoubleCanConvert(temp))
                        ys.Add(0);
                    else ys.Add(Convert.ToDouble(temp));
                }
            }
            return Analysis(ys,result);
        }
        private bool Analysis(List<double> ys, BasicSeriesData result)
        {
            int count = ys.Count;
            if (count == 0)
                return false;
            ys.Sort();
            double ql = 0;
            double q0 = 0;
            double q1 = 0;
            double q2 = 0;
            double q3 = 0;
            double q4 = 0;
            double qu = 0;
            int low = 0;
            int high = 0;
            if (count == 1)
            {
                q1 = ys[0];
                q2 = q1;
                q3 = q2;
            }
            else if (count % 2 == 0)
            {
                q2 = (ys[count / 2 - 1] + ys[count / 2]) / 2;
                if (count % 4 == 0)
                {
                    q1 = (ys[count / 4 - 1] + ys[count / 4]) / 2;
                    low = count / 4;
                    q3 = (ys[count / 4 * 3-1] + ys[count / 4 * 3]) / 2;
                    high = count / 4 * 3-1;
                }
                else
                {
                    q1 = ys[count / 4];
                    low = count / 4;
                    q3 = ys[count / 4 * 3 + 1];
                    high = count / 4 * 3 + 1;
                }
            }
            else
            {
                q2 = ys[count / 2];
                if ((count - 1) % 4 == 0)
                {
                    q1 = (ys[count / 4 - 1] + ys[count / 4]) / 2;
                    low = count / 4;
                    q3 = (ys[count / 4 * 3] + ys[count / 4 * 3 + 1]) / 2;
                    high = count / 4 * 3;
                }
                else
                {
                    q1 = ys[count / 4];
                    low = count / 4;
                    q3 = ys[count / 4 * 3 + 2];
                    high = count / 4 * 3 + 2;
                }
            }
            q0 = q1 - (q3 - q1) * 1.5;
            q4 = q3 + (q3 - q1) * 1.5;
            ql = q1 - (q3 - q1) * 3;
            qu = q3 + (q3 - q1) * 3;
            int ylc = 0;
            int y0c = 0;
            int y4c = 0;
            int yuc = 0;
            for (int i = 0; ; i++)
            {
                if (ys[i] < ql)
                {
                    result.Y.Add(ys[i]);
                    ylc++;
                }
                else if (ys[i] < q0)
                {
                    result.Y.Add(ys[i]);
                    y0c++;
                }
                else
                {
                    result.Y.Add(ys[i]);
                    break;
                }
            }
            result.Y.Add(q1);
            result.Y.Add(q2);
            result.Y.Add(q3);
            for (int i = count-1; ; i--)
            {
                if (ys[i] > qu)
                {
                    result.Y.Add(ys[i]);
                    yuc++;
                }
                else if (ys[i] > q4)
                {
                    result.Y.Add(ys[i]);
                    y4c++;
                }
                else
                {
                    result.Y.Add(ys[i]);
                    break;
                }
            }
            result.X.Add(ylc.ToString());
            result.X.Add(y0c.ToString());
            result.X.Add(yuc.ToString());
            result.X.Add(y4c.ToString());
            return true;
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
                        if (groupAnalysis(start, end, sourcedata, result))
                            result.X.Add((++x).ToString());
                        if (flag < 0)
                            break;
                        start = end;
                    }
                }
            }
            else
            {
                int x = 0;
                List<double> ys = new List<double>();
                for (int i = 0, j = 0; i < view.DataRowCount; i++)
                {
                    var rowtemp = view.GetDataRow(i);
                    if (rowtemp[view.ChooseColumnName].ToString() == true.ToString())
                    {
                        var temp = rowtemp[param];
                        if (!CheckMethod.checkDoubleCanConvert(temp))
                            ys.Add(0);
                        else ys.Add(Convert.ToDouble(temp));
                        j++;
                        if (j == groupType)
                        {
                            j = 0;
                            Analysis(ys, result);
                            x++;
                            result.X.Add(x.ToString());
                            ys.Clear();
                        }
                    }
                }
            }
            return result;
        }
    }
    public class BoxPlotSeriesDrawer : ISeriesDrawer<DevExpress.XtraCharts.ChartControl>
    {
        private DevExpress.XtraCharts.Series[] Series;
        private DevExpress.XtraCharts.ChartControl DrawBoard;
        private int index;
        public void Draw(BasicSeriesData data, System.Drawing.Color color, DevExpress.XtraCharts.ChartControl drawBoard)
        {
            int q;
            DrawBoard = drawBoard;
            Series = new DevExpress.XtraCharts.Series[4];
            int seriescount =  drawBoard.Series.Count;
            if (seriescount < 5)
                index = 1;
            else
            {
                index =Convert.ToInt32(drawBoard.Series[(seriescount/4-1)*4].Points[0].Argument.Split('-')[0])+1;
            }
            for (int i = 0; i < 4; i++)
            {
                Series[i] = new DevExpress.XtraCharts.Series();
                Series[i].View = DrawBoard.Series[i].View.Clone() as DevExpress.XtraCharts.SeriesViewBase;
                Series[i].CrosshairLabelPattern = DrawBoard.Series[i].CrosshairLabelPattern;
                Series[i].View.Color = color;
                Series[i].ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Qualitative;
                DrawBoard.Series.Add(Series[i]);
                Series[i].Points.BeginUpdate();
                Series[i].Points.Clear();
            }
            for (int i = 4, j = 0; i < data.X.Count; i += 5)
            {
                string x = data.X[i];
                int temp = Convert.ToInt32(data.X[i - 4]);
                for (int k = 0; k < temp; k++)
                {
                    Series[3].Points.Add(new DevExpress.XtraCharts.SeriesPoint(index + "-" + x, data.Y[j++]));
                }
                temp = Convert.ToInt32(data.X[i - 3]);
                for (int k = 0; k < temp; k++)
                {
                    Series[2].Points.Add(new DevExpress.XtraCharts.SeriesPoint(index + "-" + x, data.Y[j++]));
                }
                q = j;
                j += 4;
                temp = Convert.ToInt32(data.X[i - 2]);
                for (int k = 0; k < temp;k++)
                {
                    Series[3].Points.Add(new DevExpress.XtraCharts.SeriesPoint(index + "-" + x, data.Y[j++]));
                }
                temp = Convert.ToInt32(data.X[i - 1]);
                for (int k = 0; k < temp;k++)
                {
                    Series[2].Points.Add(new DevExpress.XtraCharts.SeriesPoint(index + "-" + x, data.Y[j++]));
                }
                Series[1].Points.Add(new DevExpress.XtraCharts.SeriesPoint(index+"-"+x, new object[] { (object)data.Y[q], (object)data.Y[q + 2], (object)data.Y[q + 1], (object)data.Y[q + 2] }));
                Series[0].Points.Add(new DevExpress.XtraCharts.SeriesPoint(index + "-" + x, new object[] { (object)data.Y[q + 2], (object)data.Y[j++], (object)data.Y[q + 2], (object)data.Y[q + 3] }));
            }
            for (int i = 0; i < 4; i++)
            {
                Series[i].Points.EndUpdate();
            }
        }

        public bool Clear()
        {
            if (Series != null && DrawBoard != null)
            {
                for (int i =Series.Length-1; i >=0;i-- )
                    this.DrawBoard.Series.Remove(Series[i]);
            }
            if (this.DrawBoard.Series.Count == 4)
                return true;
            else
                return false;
        }

        public void Dispose()
        {
            for (int i = Series.Length - 1; i >= 0; i--)
                Series[i].Dispose();
        }
    }
    public class BoxPlotSeriesManager : SingleSeriesManager<MonitorSourceDataType, DevExpress.XtraCharts.ChartControl>
    {
        protected override void Init()
        {
            this.SeriesMaker = new BoxPlotSeriesMaker();
            this.SeriesDrawer = new BoxPlotSeriesDrawer();
        }
    }
    #endregion

    #region SPCDetermineSeries
    public class SPCDetermineSeriesMaker : ISeriesMaker<SPCDetermineDataType>
    {
        public BasicSeriesData Make(SPCDetermineDataType sourcedata)
        {
            var view = sourcedata.View;
            var param = sourcedata.Param;
            BasicSeriesData result = new BasicSeriesData();
            SPCDetermineMethod method = new SPCDetermineMethod(sourcedata.UCL, sourcedata.LCL, sourcedata.Standard, sourcedata.Commands);
            result.Y.Add(sourcedata.Standard + sourcedata.UCL);
            result.Y.Add(sourcedata.Standard);
            result.Y.Add(sourcedata.Standard + sourcedata.LCL);
            result.X.Add(null);
            result.X.Add(null);
            result.X.Add(null);
            List<SPCCommandbase> excuteresult;
            double x = 0, y = 0;
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
                    excuteresult = method.Excute(y); 
                    x++;
                    result.X.Add(x.ToString());
                    result.Y.Add(y);
                    foreach (var command in excuteresult)
                    {
                        result.X.Add(null);
                        result.Y.Add(command.ID);
                    }
                }
            }
            return result;
        }
    }
    public class SPCDetermineSeriesDrawer : ISeriesDrawer<DevExpress.XtraCharts.ChartControl>
    {
        private DevExpress.XtraCharts.Series[] Series;
        private DevExpress.XtraCharts.ChartControl DrawBoard;
        public void Draw(BasicSeriesData data, System.Drawing.Color color, DevExpress.XtraCharts.ChartControl drawBoard)
        {
            DrawBoard = drawBoard;
            Series = new DevExpress.XtraCharts.Series[2];
            string pre = "";
            for (int i = 0; i <2; i++)
            {
                Series[i] = new DevExpress.XtraCharts.Series();
                Series[i].View = DrawBoard.Series[i].View.Clone() as DevExpress.XtraCharts.SeriesViewBase;
                Series[i].CrosshairLabelPattern = DrawBoard.Series[i].CrosshairLabelPattern;
                Series[i].View.Color = color;
                Series[i].ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Qualitative;
                DrawBoard.Series.Add(Series[i]);
                Series[i].Points.BeginUpdate();
                Series[i].Points.Clear();
            }
             (drawBoard.Diagram as DevExpress.XtraCharts.XYDiagram2D).GetAllAxesY()[0].ConstantLines.Add(new DevExpress.XtraCharts.ConstantLine("UCL", data.Y[0]) { Color = color });
             (drawBoard.Diagram as DevExpress.XtraCharts.XYDiagram2D).GetAllAxesY()[0].ConstantLines.Add(new DevExpress.XtraCharts.ConstantLine("STD", data.Y[1]) { Color = color });
             (drawBoard.Diagram as DevExpress.XtraCharts.XYDiagram2D).GetAllAxesY()[0].ConstantLines.Add(new DevExpress.XtraCharts.ConstantLine("LCL", data.Y[2]) { Color = color });

            for (int i = 3; i < data.X.Count; i ++)
            {
                var x = data.X[i];
                if(x==null)
                {
                    Series[1].Points.Add(new DevExpress.XtraCharts.SeriesPoint(pre, data.Y[i]));
                }
                else
                {
                    Series[0].Points.Add(new DevExpress.XtraCharts.SeriesPoint(x, data.Y[i]));
                    pre = x;
                }
            }
            for (int i = 0; i < 2; i++)
            {
                Series[i].Points.EndUpdate();
            }
        }

        public bool Clear()
        {
            if (Series != null && DrawBoard != null)
            {
                for (int i = Series.Length - 1; i >= 0; i--)
                    this.DrawBoard.Series.Remove(Series[i]);
            }
            if (this.DrawBoard.Series.Count == 2)
                return true;
            else
                return false;
        }
        public void Dispose()
        {
            for (int i = Series.Length - 1; i >= 0; i--)
                Series[i].Dispose();
        }
    }
    public class SPCDetermineSeriesManager : SingleSeriesManager<SPCDetermineDataType, DevExpress.XtraCharts.ChartControl>
    {
        protected override void Init()
        {
            this.SeriesMaker = new SPCDetermineSeriesMaker();
            this.SeriesDrawer = new SPCDetermineSeriesDrawer();
        }
    }
    #endregion

}
