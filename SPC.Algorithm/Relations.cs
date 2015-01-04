using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SPC.Base.Interface;
using SPC.Base.Operation;

namespace SPC.Algorithm
{
    public static class Relations
    {
        public static double GetCCT(IDataTable<DataRow> data, string a, string b, WaitObject flag)
        {
            int count = data.RowCount;
            double xyS = 0;
            double xS = 0;
            double yS = 0;
            double x2S = 0;
            double y2S = 0;
            double tempx = 0;
            double tempy = 0;
            DataRow temprow;
            flag.Flags = new int[1];
            flag.Max = count;
            for (int i = 0; i < count; i++)
            {
                temprow = data[i];
                tempx = temprow[a].ConvertToDouble();
                tempy = temprow[b].ConvertToDouble();
                xyS += tempx * tempy;
                xS += tempx;
                yS += tempy;
                x2S += tempx * tempx;
                y2S += tempy * tempy;
                flag.Flags[0]++;
            }
            return (xyS * count - xS * yS) / (Math.Pow((x2S * count - xS * xS), 0.5) * Math.Pow((y2S * count - yS * yS), 0.5));
        }
        public static double[] GetCCTs(IDataTable<DataRow> data, string target, string[] f,WaitObject flag)
        {
            int count = data.RowCount;
            int length = f.Length;
            double[] fs = new double[length];
            double[] xfs = new double[length];
            double[] f2s = new double[length];
            double[] result = new double[length];
            double xS = 0;
            double x2S = 0;
            int j = 0;
            double tempx = 0;
            double tempy = 0;
            DataRow temprow;
            flag.Flags = new int[1];
            flag.Max = count * length;
            for (int i = 0; i < count; i++)
            {
                temprow = data[i];
                tempx = temprow[target].ConvertToDouble();
                xS += tempx;
                x2S += tempx * tempx;
                for(j=0;j<length;j++)
                {
                    tempy = temprow[f[j]].ConvertToDouble();
                    xfs[j] += tempx * tempy;
                    fs[j] += tempy;
                    f2s[j] += tempy * tempy;
                    flag.Flags[0]++;
                }
            } 
            double down;
            for (j = 0; j < length; j++)
            {
                down = (Math.Pow((x2S * count - xS * xS), 0.5) * Math.Pow((f2s[j] * count - fs[j] * fs[j]), 0.5));
                if (down != 0)
                    result[j] = (xfs[j] * count - xS * fs[j]) / down;
                else if (x2S == 0)
                    result[j] = 1;
                else
                    result[j] = 0;
            }
            return result;
        }
    }
}
