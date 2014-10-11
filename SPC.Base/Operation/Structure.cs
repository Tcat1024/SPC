using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPC.Base.Interface;

namespace SPC.Base.Operation
{
    public struct STDEVType
    {
        public string Description { get; set; }
        private static STDEVType _Std = new STDEVType("标准差",STDEV.stdGetGroup);
        public static STDEVType Std
        {
            get
            {
                return STDEVType._Std;
            }
        }
        private static STDEVType _NStd = new STDEVType("无估算标准差", STDEV.nstdGetGroup);
        public static STDEVType NStd
        {
            get
            {
                return STDEVType._NStd;
            }
        }
        private static STDEVType _Range = new STDEVType("极差", STDEV.rangeGetGroup);
        public static STDEVType Range
        {
            get
            {
                return STDEVType._Range;
            }
        }
        public Func<ICanGetProperty, double, int,int,int,double> Get;
        public STDEVType(string de,Func<ICanGetProperty, double, int,int,int,double> g)
            : this()
        {
            this.Description = de;
            this.Get = g;
        }
        public static bool operator ==(STDEVType a, STDEVType b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(STDEVType a, STDEVType b)
        {
            return !a.Equals(b);
        }
        public override bool Equals(object obj)
        {
            if (obj is STDEVType)
                return this.Description == ((STDEVType)obj).Description;
            else
                return false;
        }
        public override string ToString()
        {
            return this.Description;
        }
        public override int GetHashCode()
        {
            return this.Description.GetHashCode();
        }
    }
    
    public static class StandardConst
    {
        private static double[] _D2 = new double[] { double.NaN, 1, 1.128, 1.693, 2.059, 2.326, 2.543, 2.704, 2.847, 2.970, 3.078, 3.173, 3.258, 3.336, 3.407, 3.472, 3.532, 3.588, 3.640, 3.689, 3.735, 3.778, 3.819, 3.858, 3.895, 3.931 };
        public static double[] D2
        {
            get
            {
                return _D2;
            }
        }
        private static double[] _C4 = new double[] { double.NaN, 1, 0.797885, 0.886227, 0.921318, 0.939986, 0.951533, 0.959369, 0.965030, 0.969311, 0.972659, 0.975350, 0.977559, 0.979406, 0.980971, 0.982316, 0.983484, 0.984506, 0.985410, 0.986214, 0.986934, 0.987583, 0.988170, 0.988705, 0.989193, 0.989640 };
        public static double[] C4
        {
            get
            {
                return _C4;
            }
        }
    }
    public class BasicSeriesData
    {
        public string SeriesName;
        public List<double> X = new List<double>();
        public List<double> Y = new List<double>();
        public BasicSeriesData()
        {

        }
        public BasicSeriesData(string name)
        {
            this.SeriesName = name;
        }
        public override string ToString()
        {
            return SeriesName;
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
