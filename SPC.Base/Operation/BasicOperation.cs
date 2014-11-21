using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;
using System.Data;
using SPC.Base.Interface;

namespace SPC.Base.Operation
{
    /*public class AdvanceOperationData
    {
        private long _ParallelMaxCount = 500;
        public long ParallelMaxCount
        {
            get { return this._ParallelMaxCount; }
            set { this._ParallelMaxCount = value; }
        }
        public double[] Data { get; set; }
        public double Average { get; set; }
        public double Variance { get; set; }
        public double StDeviation { get; set; }
        public double Max { get; set; }
        public double Min { get; set; }
        private double[] sums;
        private double[] maxs;
        private double[] mins;
        public void InitData()
        {
            long count = this.Data.Length;
            long thcount = (count - 1) / this.ParallelMaxCount + 1;
            long lastthnum = (count - 1) % this.ParallelMaxCount + 1;
            Barrier myBarrier = new Barrier((int)thcount);
            sums = new double[thcount];
            maxs = new double[thcount];
            mins = new double[thcount];
            Thread temp = new Thread(Thread_InitData);
            for (int i = 0; i < thcount - 1; i++)
            {
                temp.Start(new ThreadData((int)(i * this.ParallelMaxCount), (int)((i + 1) * this.ParallelMaxCount), i, myBarrier));
            }
            temp.Start(new ThreadData((int)((thcount - 1) * this.ParallelMaxCount), (int)((thcount - 1) * this.ParallelMaxCount + lastthnum), (int)thcount - 1, myBarrier));
            myBarrier.SignalAndWait();
            double sum = this.sums[0];
            this.Max = this.maxs[0];
            this.Min = this.mins[0];
            for (int i = 1; i < thcount; i++)
            {
                sum += this.sums[i];
                if (this.Min > this.mins[i])
                    this.Min = this.mins[i];
                if (this.Max < this.maxs[i])
                    this.Max = this.maxs[i];
            }
            this.Average = sum / count;
            //for(int i)
        }
        private void Thread_InitData(object o)
        {
            var thd = o as ThreadData;
            this.sums[thd.thn] = this.Data[thd.start];
            this.mins[thd.thn] = this.Data[thd.start];
            this.maxs[thd.thn] = this.Data[thd.start];
            for (int i = thd.start + 1; i < thd.end; i++)
            {
                double temp = this.Data[i];
                this.sums[thd.thn] += temp;
                if (this.mins[thd.thn] > temp)
                    this.mins[thd.thn] = temp;
                if (this.maxs[thd.thn] < temp)
                    this.maxs[thd.thn] = temp;
            }
            thd.myBarrier.RemoveParticipant();
        }
        private class ThreadData
        {
            public int start;
            public int end;
            public int thn;
            public Barrier myBarrier;
            public ThreadData(int start, int end, int thn, Barrier myBarrier)
            {
                this.start = start;
                this.end = end;
                this.thn = thn;
                this.myBarrier = myBarrier;
            }
        }
    }*/
    public static class STDEV
    {
        public static double GetGroup(ICanGetProperty data, double avg, int start,int count,int grouplength,STDEVType type)
        {
            return type.Get(data, avg, start, count, grouplength);
        }
        public static double GetEntirety(ICanGetProperty data, double avg, int start, int count)
        {
            double sum = 0;
            if (count < 1)
                throw new Exception("GetEntirety无数据");
            for (int i = start,c = 0; c < count; i++)
            {
                double? temp = data[i];
                if (temp != null)
                {
                    sum += Math.Pow((double)temp - avg, 2);
                    c++;
                }
            }
            if (count == 1)
                count++;
            return Math.Pow(sum / (count - 1), 0.5);
        }
        public static double stdGetGroup(ICanGetProperty data, double avg, int start,int count,int grouplength)
        {
            int a = (int)(count / grouplength);
            if (a < 1)
                throw new Exception("子组容量过大");
            if (grouplength < 1)
                throw new Exception("子组大小必须大于0");
            if (grouplength >= StandardConst.D2.Length || grouplength > count)
                throw new Exception("子组容量过大");
            ArrayVector std = new ArrayVector(a);
            int s = start;
            double result = 0;
            ArrayVector gavg = new ArrayVector(a);
            for (int i = 0; i < a; i++)
            {
                int j = s;
                for (int c = 0; c < grouplength; j++)
                {
                    double? temp = data[j];
                    if (temp != null)
                    {
                        gavg.Data[i] += (double)temp / grouplength;
                        c++;
                    }
                }
                gavg.AVG += gavg.Data[i]/a;
                std.Data[i]=STDEV.GetEntirety(data, gavg.Data[i], s, grouplength);
                std.AVG += std.Data[i]/a;
                s = j;
            }
            std.AVG = std.AVG / StandardConst.C4[(int)grouplength];
            //double rbar = (std.data[1] - std.data[0]) / StandardConst.D2[2];
            //double rbar = GetAvgMovingRangeXBar(data, start, count);
            //bgstd =Math.Max(0, Math.Pow(Math.Pow(rbar,2) - Math.Pow(std.AVG,2)/a,0.5));
            //result = Math.Pow(Math.Pow(bgstd, 2) + Math.Pow(std.AVG, 2), 0.5);
            result = std.AVG;
            return result;
        }
        public static double GetAvgMovingRangeXBar(ICanGetProperty data, int start, int count)
        {
            int s = start;
            double? pre;
            double rbar = 0;
            while ((pre = data[s++]) == null) ;
            for (int i = s,c=1;c<count ; i++)
            {
                double? temp = data[i];
                if(temp!=null)
                {
                    rbar += Math.Abs((double)(temp - pre));
                    c++;
                    pre = temp;
                }
            }
            rbar = rbar / (count -1);
            return rbar/StandardConst.D2[2];
        }
        public static double rangeGetGroup(ICanGetProperty data, double avg, int start, int count, int grouplength)
        {
            int a = (int)(count / grouplength);
            if (a < 1)
                throw new Exception("子组容量过大");
            if (grouplength < 1)
                throw new Exception("子组大小必须大于0");
            if (grouplength >= StandardConst.D2.Length || grouplength > count)
                throw new Exception("子组容量过大");
            double range = 0;
            int s = start;
            for (int i = 0; i < a; i++)
            {
                int j = s;
                double max = double.NegativeInfinity;
                double min = double.PositiveInfinity;
                for (int c=0; c<grouplength; j++)
                {
                    double? temp = data[j];
                    if (temp != null)
                    {
                        double dtemp = (double)temp;
                        if (dtemp > max)
                            max = dtemp;
                        if (dtemp < min)
                            min = dtemp;
                        c++;
                    }
                }
                range += (max - min) / a;
                s = j;
            }
            range = range / StandardConst.D2[(int)grouplength];
            return range;
        }
        public static double nstdGetGroup(ICanGetProperty data, double avg, int start, int count, int grouplength)
        {
            int a = (int)(count / grouplength);
            if (a < 1)
                throw new Exception("子组容量过大");
            if (grouplength < 1)
                throw new Exception("子组大小必须大于0");
            double std = 0;
            int s = start;
            for (int i = 0; i < a; i++)
            {
                double gavg = 0;
                int j = s;
                for (int c=0; c<grouplength; j++)
                {
                    double? temp = data[j];
                    if (temp != null)
                    {
                        gavg += (double)temp / grouplength;
                        c++;
                    }
                }
                std += STDEV.GetEntirety(data, gavg, s, grouplength) / a;
                s = j;
            }
            return std;
        }
    }
    public static class NormalDistribution
    {
        public static double InverseSqrt2PI = 1 / Math.Sqrt(2 * Math.PI);
        public static double GetY(double x,double avg,double std)
        {
            double PowOfE = -(Math.Pow(Math.Abs(x - avg), 2) / (2 * Math.Pow(std,2)));

            double result = (InverseSqrt2PI / std) * Math.Pow(Math.E, PowOfE);

            return result;
        }
        public static decimal GetP(double b)
        {
            if (double.IsNaN(b))
                throw new Exception("正态分布输入数据错误，为\""+b.ToString()+"\"");
            int S = 2;
            double Q = 0;
            while (true)
            {
                double a = b - S;
                int M = 1, N = 1, k = 1, m = 1;
                double ep, I, h;
                ep = 0.000000000001;
                h = b - a;
                I = h * (f(a) + f(b)) / 2;
                double[,] T = new double[5000, 5000];
                T[1, 1] = I;
                while (1 > 0)
                {
                    N = (int)Math.Pow(2, m - 1);
                    h = h / 2;
                    I = I / 2;
                    for (int i = 1; i <= N; i++)
                        I = I + h * f(a + (2 * i - 1) * h);
                    T[m + 1, 1] = I;
                    M = 2 * N;
                    k = 1;
                    while (M > 1)
                    {
                        T[m + 1, k + 1] = (Math.Pow(4, k) * T[m + 1, k] - T[m, k]) / (Math.Pow(4, k) - 1);
                        M = M / 2;
                        k = k + 1;
                    }
                    if (Math.Abs(T[k, k] - T[k - 1, k - 1]) < ep)
                        break;
                    m = m + 1;
                }
                I = T[k, k];
                Q = Q + I;
                if (Math.Abs(I) < ep)
                    return (decimal)Q;
                b = a; S = 2 * S;
            }
        }
        public static double f(double x)
        {
            double f = Math.Exp(-x * x / 2) / Math.Sqrt(2 * Math.PI);
            return f;
        }
    }
    //public class BasicOperation
    //{
    //    public static double STDEVA(double[] data)
    //    {
    //        double result = EVA(data);
    //        return result < 0 ? -1 : Math.Pow(result, 0.5);
    //    }
    //    public static double EVA(double[] data)
    //    {
    //        int count = data.Length;
    //        if(count==0)
    //            return -1;
    //        double avg = data.Average();
    //        double sum = 0;
    //        for(int i =0;i<count;i++)
    //        {
    //            sum+=Math.Pow(data[i]-avg,2);
    //        }
    //        return sum / count;
    //    }
    //    public static double AVG(double[] data)
    //    {
    //        return data.Average();
    //    }
    //    public static double SPOFFSET(double m,double avg)
    //    {
    //        return Math.Abs(m - avg);
    //    }
    //    //public static double SPOFFSETD()
    //}
    public class CheckMethod
    {
        public static bool checkDoubleCanConvert(object o)
        {
            return !(o is DBNull || !(o is System.IConvertible));
        }
    }
    public static class DataConvertExtern
    {
        public static double ConvertToDouble(this object o)
        {
            if (o is DBNull)
                return 0;
            else
                return Convert.ToDouble(o);
        }
    }
    public class BasicSort
    {
        public static void QuickSort<T>(int start, int end, T[] data, Func<T, T, bool> compare)
        {
            if (start >= end)
                return;
            int i = start, j = end;
            var temp = data[i];
            while (i < j)
            {
                if (compare(temp, data[j]))
                {
                    data[i] = data[j];
                    i++;
                    while (i < j)
                    {
                        if (compare(data[i], temp))
                        {
                            data[j] = data[i];
                            break;
                        }
                        i++;
                    }
                }
                j--;
            }
            data[i] = temp;
            QuickSort<T>(start, i - 1, data, compare);
            QuickSort<T>(i + 1, end, data, compare);
        }
    }
}
