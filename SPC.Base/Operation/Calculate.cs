using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SPC.Base.Operation
{
    public class RowCalculate
    {
        public static double[] Sum(SPC.Base.Interface.IDataTable<DataRow> input, string[] columns)
        {
            int count = input.RowCount;
            double[] result = new double[count];
            int length = columns.Length;
            int i, j;
            double sum;
            DataRow temprow;
            for (i = 0; i < count; i++)
            {
                sum = 0;
                temprow = input[i];
                for (j = 0; j < length; j++)
                {
                    sum += temprow[columns[j]].ConvertToDouble();
                }
                result[i] = sum;
            }
            return result;
        }
        public static double[] Avg(SPC.Base.Interface.IDataTable<DataRow> input, string[] columns)
        {
            int count = input.RowCount;
            double[] result = new double[count];
            int length = columns.Length;
            int i, j;
            double sum;
            DataRow temprow;
            for (i = 0; i < count; i++)
            {
                sum = 0;
                temprow = input[i];
                for (j = 0; j < length; j++)
                {
                    sum += temprow[columns[j]].ConvertToDouble();
                }
                result[i] = sum / length;
            }
            return result;
        }
        public static double[] Stdev(SPC.Base.Interface.IDataTable<DataRow> input, string[] columns)
        {
            int count = input.RowCount;
            double[] result = new double[count];
            int length = columns.Length;
            int i, j;
            double sum;
            double avg;
            double std;
            DataRow temprow;
            for (i = 0; i < count; i++)
            {
                sum = 0;
                temprow = input[i];
                for (j = 0; j < length; j++)
                {
                    sum += temprow[columns[j]].ConvertToDouble();
                }
                avg = sum / length;
                std = 0;
                for (j = 0; j < length; j++)
                {
                    std += Math.Pow(temprow[columns[j]].ConvertToDouble() - avg, 2);
                }
                result[i] = Math.Pow(std / length, 0.5);
            }
            return result;
        }
        public static double[] Min(SPC.Base.Interface.IDataTable<DataRow> input, string[] columns)
        {
            int count = input.RowCount;
            double[] result = new double[count];
            int length = columns.Length;
            int i, j;
            double min, temp;
            DataRow temprow;
            for (i = 0; i < count; i++)
            {
                min = double.MaxValue;
                temprow = input[i];
                for (j = 0; j < length; j++)
                {
                    temp = temprow[columns[j]].ConvertToDouble();
                    if (temp < min)
                        min = temp;
                }
                result[i] = min;
            }
            return result;
        }
        public static double[] Max(SPC.Base.Interface.IDataTable<DataRow> input, string[] columns)
        {
            int count = input.RowCount;
            double[] result = new double[count];
            int length = columns.Length;
            int i, j;
            double max, temp;
            DataRow temprow;
            for (i = 0; i < count; i++)
            {
                max = double.MinValue;
                temprow = input[i];
                for (j = 0; j < length; j++)
                {
                    temp = temprow[columns[j]].ConvertToDouble();
                    if (temp > max)
                        max = temp;
                }
                result[i] = max;
            }
            return result;
        }
        public static double[] Range(SPC.Base.Interface.IDataTable<DataRow> input, string[] columns)
        {
            int count = input.RowCount;
            double[] result = new double[count];
            int length = columns.Length;
            int i, j;
            double max, min, temp;
            DataRow temprow;
            for (i = 0; i < count; i++)
            {
                max = double.MinValue;
                min = double.MaxValue;
                temprow = input[i];
                for (j = 0; j < length; j++)
                {
                    temp = temprow[columns[j]].ConvertToDouble();
                    if (temp > max)
                        max = temp;
                    if (temp < min)
                        min = temp;
                }
                result[i] = max - min;
            }
            return result;
        }
        public static double[] Mid(SPC.Base.Interface.IDataTable<DataRow> input, string[] columns)
        {
            int count = input.RowCount;
            double[] result = new double[count];
            int length = columns.Length;
            int i, j;
            double temp;
            DataRow temprow;
            SortedList<double, double> list;
            for (i = 0; i < count; i++)
            {
                temprow = input[i];
                list = new SortedList<double, double>();
                for (j = 0; j < length; j++)
                {
                    temp = temprow[columns[j]].ConvertToDouble();
                    list.Add(temp, temp);
                }
                if (length % 2 == 0)
                    result[i] = (list.Keys[length / 2] + list.Keys[length / 2 - 1]) / 2;
                else
                    result[i] = list.Keys[length / 2];
            }
            return result;
        }
        public static double[] QuadraticSum(SPC.Base.Interface.IDataTable<DataRow> input, string[] columns)
        {
            int count = input.RowCount;
            double[] result = new double[count];
            int length = columns.Length;
            int i, j;
            DataRow temprow;
            for (i = 0; i < count; i++)
            {
                temprow = input[i];
                for (j = 0; j < length; j++)
                {
                    result[i] += Math.Pow(temprow[columns[j]].ConvertToDouble(), 2);
                }
            }
            return result;
        }
        public static double[] Count(SPC.Base.Interface.IDataTable<DataRow> input, string[] columns)
        {
            int count = input.RowCount;
            double[] result = new double[count];
            int length = columns.Length;
            int i;
            for (i = 0; i < count; i++)
            {
                result[i] = length;
            }
            return result;
        }
        public static double[] IsNotNull(SPC.Base.Interface.IDataTable<DataRow> input, string[] columns)
        {
            int count = input.RowCount;
            double[] result = new double[count];
            int length = columns.Length;
            int i, j;
            DataRow temprow;
            for (i = 0; i < count; i++)
            {
                temprow = input[i];
                for (j = 0; j < length; j++)
                {
                    if (temprow[columns[j]] != null)
                        result[i]++;
                }
            }
            return result;
        }
        public static double[] IsNull(SPC.Base.Interface.IDataTable<DataRow> input, string[] columns)
        {
            int count = input.RowCount;
            double[] result = new double[count];
            int length = columns.Length;
            int i, j;
            DataRow temprow;
            for (i = 0; i < count; i++)
            {
                temprow = input[i];
                for (j = 0; j < length; j++)
                {
                    if (temprow[columns[j]] == null)
                        result[i]++;
                }
            }
            return result;
        }
    }
    public class ColumnCalculate
    {
        public static double[] Sum(SPC.Base.Interface.IDataTable<DataRow> input, string[] columns)
        {
            int count = input.RowCount;
            int length = columns.Length;
            double[] result = new double[length];
            int i, j;
            DataRow temprow;
            for (i = 0; i < count; i++)
            {
                temprow = input[i];
                for (j = 0; j < length; j++)
                {
                    result[j] += temprow[columns[j]].ConvertToDouble();
                }
            }
            return result;
        }
        public static double[] Avg(SPC.Base.Interface.IDataTable<DataRow> input, string[] columns)
        {
            int count = input.RowCount;
            int length = columns.Length;
            double[] result = new double[length];
            int i, j;
            DataRow temprow;
            for (i = 0; i < count; i++)
            {
                temprow = input[i];
                for (j = 0; j < length; j++)
                {
                    result[j] += temprow[columns[j]].ConvertToDouble();
                }
            }
            for (j = 0; j < length; j++)
            {
                result[j] = result[j]/count;
            }
            return result;
        }
        public static double[] Stdev(SPC.Base.Interface.IDataTable<DataRow> input, string[] columns)
        {
            int count = input.RowCount;
            int length = columns.Length;
            double[] result = new double[length];
            int i, j;
            double[] avg = new double[length];
            DataRow temprow;
            for (i = 0; i < count; i++)
            {
                temprow = input[i];
                for (j = 0; j < length; j++)
                {
                    avg[j] += temprow[columns[j]].ConvertToDouble();
                }
            }
            for (j = 0; j < length; j++)
            {
                avg[j] = avg[j]/count;
            }
            for (i = 0; i < count; i++)
            {
                temprow = input[i];
                for (j = 0; j < length; j++)
                {
                    result[j] += Math.Pow(temprow[columns[j]].ConvertToDouble() - avg[j], 2);
                }
            }
            for (j = 0; j < length; j++)
            {
                result[j] = Math.Pow(result[j] / count, 0.5);
            }
            return result;
        }
        public static double[] Stdev(SPC.Base.Interface.IDataTable<DataRow> input, string[] columns,double[] avgs)
        {
            int count = input.RowCount;
            int length = columns.Length;
            if(avgs.Length!=length)
            {
                return Stdev(input, columns);
            }
            double[] result = new double[length];
            int i, j;
            DataRow temprow;
            for (i = 0; i < count; i++)
            {
                temprow = input[i];
                for (j = 0; j < length; j++)
                {
                    result[j] += Math.Pow(temprow[columns[j]].ConvertToDouble() - avgs[j], 2);
                }
            }
            for (j = 0; j < length; j++)
            {
                result[j] = Math.Pow(result[j] / count, 0.5);
            }
            return result;
        }
        public static double[] Min(SPC.Base.Interface.IDataTable<DataRow> input, string[] columns)
        {
            int count = input.RowCount;
            int length = columns.Length;
            double[] result = new double[length];
            int i, j;
            double temp;
            DataRow temprow;
            for (j = 0; j < length; j++)
            {
                result[j] = double.MaxValue;
            }
            for (i = 0; i < count; i++)
            {
                temprow = input[i];
                for (j = 0; j < length; j++)
                {
                    temp = temprow[columns[j]].ConvertToDouble();
                    if (temp < result[j])
                        result[j] = temp;
                }
            }
            return result;
        }
        public static double[] Max(SPC.Base.Interface.IDataTable<DataRow> input, string[] columns)
        {
            int count = input.RowCount;
            int length = columns.Length;
            double[] result = new double[length];
            int i, j;
            double temp;
            DataRow temprow;
            for (j = 0; j < length; j++)
            {
                result[j] = double.MinValue;
            }
            for (i = 0; i < count; i++)
            {
                temprow = input[i];
                for (j = 0; j < length; j++)
                {
                    temp = temprow[columns[j]].ConvertToDouble();
                    if (temp > result[j])
                        result[j] = temp;
                }
            }
            return result;
        }
        public static double[] Range(SPC.Base.Interface.IDataTable<DataRow> input, string[] columns)
        {
            int count = input.RowCount;
            int length = columns.Length;
            double[] result = new double[length];
            int i, j;
            double temp;
            double[] max = new double[length];
            double[] min = new double[length];
            DataRow temprow;
            for (j = 0; j < length; j++)
            {
                max[j] = double.MinValue;
                min[j] = double.MaxValue;
            }
            for (i = 0; i < count; i++)
            {
                temprow = input[i];
                for (j = 0; j < length; j++)
                {
                    temp = temprow[columns[j]].ConvertToDouble();
                    if (temp > max[j])
                        max[j] = temp;
                    if (temp < min[j])
                        min[j] = temp;
                }
            }
            for (j = 0; j < length; j++)
            {
                result[j] = max[j] - min[j];
            }
            return result;
        }
        public static double[] Mid(SPC.Base.Interface.IDataTable<DataRow> input, string[] columns)
        {
            int count = input.RowCount;
            int length = columns.Length;
            double[] result = new double[length];
            int i, j;
            double temp;
            DataRow temprow;
            SortedList<double, double>[] list = new SortedList<double, double>[length];
            for (j = 0; j < length; j++)
            {
                list[j] = new SortedList<double, double>();
            }
            for (i = 0; i < count; i++)
            {
                temprow = input[i];
                for (j = 0; j < length; j++)
                {
                    temp = temprow[columns[j]].ConvertToDouble();
                    list[j].Add(temp, temp);
                }

            }
            if (count % 2 == 0)
            {
                for (j = 0; j < length; j++)
                {

                    result[j] = (list[j].Keys[count / 2] + list[j].Keys[count / 2 - 1]) / 2;
                }
            }
            else
            {
                for (j = 0; j < length; j++)
                {

                    result[j] = list[j].Keys[count / 2];
                }
            }
            return result;
        }
        public static double[] QuadraticSum(SPC.Base.Interface.IDataTable<DataRow> input, string[] columns)
        {
            int count = input.RowCount;
            int length = columns.Length;
            double[] result = new double[length];
            int i, j;
            DataRow temprow;
            for (i = 0; i < count; i++)
            {
                temprow = input[i];
                for (j = 0; j < length; j++)
                {
                    result[j] += Math.Pow(temprow[columns[j]].ConvertToDouble(), 2);
                }
            }
            return result;
        }
        public static double[] Count(SPC.Base.Interface.IDataTable<DataRow> input, string[] columns)
        {
            int count = input.RowCount;
            int length = columns.Length;
            double[] result = new double[length];
            int i;
            for (i = 0; i < length; i++)
            {
                result[i] = count;
            }
            return result;
        }
        public static double[] IsNotNull(SPC.Base.Interface.IDataTable<DataRow> input, string[] columns)
        {
            int count = input.RowCount;
            int length = columns.Length;
            double[] result = new double[length];
            int i, j;
            DataRow temprow;
            for (i = 0; i < count; i++)
            {
                temprow = input[i];
                for (j = 0; j < length; j++)
                {
                    if (temprow[columns[j]] != null)
                        result[j]++;
                }
            }
            return result;
        }
        public static double[] IsNull(SPC.Base.Interface.IDataTable<DataRow> input, string[] columns)
        {
            int count = input.RowCount;
            int length = columns.Length;
            double[] result = new double[length];
            int i, j;
            DataRow temprow;
            for (i = 0; i < count; i++)
            {
                temprow = input[i];
                for (j = 0; j < length; j++)
                {
                    if (temprow[columns[j]] == null)
                        result[j]++;
                }
            }
            return result;
        }
    }
    public class Standardization
    {
        public static void ZScore(SPC.Base.Interface.IDataTable<DataRow> sourcedata,string[] sourcecolumns,string[] targetcolumns)
        {
            var avgs = ColumnCalculate.Avg(sourcedata, sourcecolumns);
            var stdevs = ColumnCalculate.Stdev(sourcedata, sourcecolumns,avgs);
            int rowcount = sourcedata.RowCount;
            int columncount = sourcecolumns.Length;
            int i,j;
            DataRow temprow;
            foreach(var targetcolumn in targetcolumns)
            {
                if (!sourcedata.ContainsColumn(targetcolumn))
                {
                    sourcedata.AddColumn(targetcolumn, typeof(double));
                }
            }
            for(i = 0;i<rowcount;i++)
            {
                temprow = sourcedata[i];
                for(j=0;j<columncount;j++)
                {
                    temprow[targetcolumns[j]] = (temprow[sourcecolumns[j]].ConvertToDouble() - avgs[j]) / stdevs[j];
                }
            }
        }
        public static void MinusAvg(SPC.Base.Interface.IDataTable<DataRow> sourcedata, string[] sourcecolumns, string[] targetcolumns)
        {
            var avgs = ColumnCalculate.Avg(sourcedata, sourcecolumns);
            int rowcount = sourcedata.RowCount;
            int columncount = sourcecolumns.Length;
            int i, j;
            DataRow temprow;
            foreach (var targetcolumn in targetcolumns)
            {
                if (!sourcedata.ContainsColumn(targetcolumn))
                {
                    sourcedata.AddColumn(targetcolumn, typeof(double));
                }
            }
            for (i = 0; i < rowcount; i++)
            {
                temprow = sourcedata[i];
                for (j = 0; j < columncount; j++)
                {
                    temprow[targetcolumns[j]] = (temprow[sourcecolumns[j]].ConvertToDouble() - avgs[j]);
                }
            }
        }
        public static void DivideStdev(SPC.Base.Interface.IDataTable<DataRow> sourcedata, string[] sourcecolumns, string[] targetcolumns)
        {
            var stdevs = ColumnCalculate.Stdev(sourcedata, sourcecolumns);
            int rowcount = sourcedata.RowCount;
            int columncount = sourcecolumns.Length;
            int i, j;
            DataRow temprow;
            foreach (var targetcolumn in targetcolumns)
            {
                if (!sourcedata.ContainsColumn(targetcolumn))
                {
                    sourcedata.AddColumn(targetcolumn, typeof(double));
                }
            }
            for (i = 0; i < rowcount; i++)
            {
                temprow = sourcedata[i];
                for (j = 0; j < columncount; j++)
                {
                    temprow[targetcolumns[j]] = temprow[sourcecolumns[j]].ConvertToDouble() / stdevs[j];
                }
            }
        }
        public static void M1D2(SPC.Base.Interface.IDataTable<DataRow> sourcedata, string[] sourcecolumns, string[] targetcolumns,double arg_1,double arg_2)
        {
            int rowcount = sourcedata.RowCount;
            int columncount = sourcecolumns.Length;
            int i, j;
            DataRow temprow;
            foreach (var targetcolumn in targetcolumns)
            {
                if (!sourcedata.ContainsColumn(targetcolumn))
                {
                    sourcedata.AddColumn(targetcolumn, typeof(double));
                }
            }
            for (i = 0; i < rowcount; i++)
            {
                temprow = sourcedata[i];
                for (j = 0; j < columncount; j++)
                {
                    temprow[targetcolumns[j]] = (temprow[sourcecolumns[j]].ConvertToDouble() - arg_1) / arg_2;
                }
            }
        }
        public static void F1T2(SPC.Base.Interface.IDataTable<DataRow> sourcedata, string[] sourcecolumns, string[] targetcolumns,double arg_1,double arg_2)
        {
            var mins = ColumnCalculate.Min(sourcedata, sourcecolumns);
            var maxs = ColumnCalculate.Max(sourcedata, sourcecolumns);
            int rowcount = sourcedata.RowCount;
            int columncount = sourcecolumns.Length;

            double[] ranges = new double[columncount];
            int i, j;
            DataRow temprow;
            double r = arg_2 - arg_1;
            for (j = 0; j < columncount; j++)
            {
                if (!sourcedata.ContainsColumn(targetcolumns[j]))
                {
                    sourcedata.AddColumn(targetcolumns[j], typeof(double));
                }
                ranges[j] = maxs[j]-mins[j];
            }

            for (i = 0; i < rowcount; i++)
            {
                temprow = sourcedata[i];
                for (j = 0; j < columncount; j++)
                {
                    temprow[targetcolumns[j]] = ((temprow[sourcecolumns[j]].ConvertToDouble() - mins[j])*r) / ranges[j]+arg_1;
                }
            }
        }
    }
}
