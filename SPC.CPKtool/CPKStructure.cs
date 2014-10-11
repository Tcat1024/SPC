using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPC.Base.Interface;
using SPC.Base.Operation;

namespace SPC.CPKtool
{
    public abstract class CPKData<T> : ICanGetProperty
    {
        public int start = 0;
        //Input
        protected T _Data;
        public T Data
        {
            get
            {
                return this._Data;
            }
            set
            {
                if (this.DataChanging != null)
                    this.DataChanging(this, new PropertyChangeEventArgs(this._Data));
                this._Data = value;
                if (this.AllowAutoRefresh)
                    RefreshData();
                if (this.DataChanged != null)
                    this.DataChanged(this, new PropertyChangeEventArgs(value));
            }
        }
        protected string _Param = null;
        public string Param
        {
            get
            {
                return this._Param;
            }
            set
            {
                if (this._Param != value)
                {
                    if (this.ParamChanging != null)
                        this.ParamChanging(this, new PropertyChangeEventArgs(this._Param));
                    this._Param = value;
                    if (this.AllowAutoRefresh)
                        RefreshData();
                    if (this.ParamChanged != null)
                        this.ParamChanged(this, new PropertyChangeEventArgs(value));
                }
            }
        }
        private double _Standard;
        public double Standard
        {
            get
            {
                return this._Standard;
            }
            set
            {
                if (this._Standard != value)
                {
                    if (this.ToleChanging != null)
                        this.ToleChanging(this, new PropertyChangeEventArgs(this._Standard));
                    this._Standard = value;
                    if (this.AllowAutoRefresh)
                        RefreshTole();
                    if (this.ToleChanged != null)
                        this.ToleChanged(this, new PropertyChangeEventArgs(value));
                }
            }
        }
        private double _UpTole = 0;
        public double UpTole
        {
            get
            {
                return this._UpTole;
            }
            set
            {
                if (this._UpTole != value)
                {
                    if (this.ToleChanging != null)
                        this.ToleChanging(this, new PropertyChangeEventArgs(this._UpTole));
                    this._UpTole = value;
                    if (this.AllowAutoRefresh)
                        RefreshTole();
                    if (this.ToleChanged != null)
                        this.ToleChanged(this, new PropertyChangeEventArgs(value));

                }
            }
        }
        private double _LowTole = 0;
        public double LowTole
        {
            get
            {
                return this._LowTole;
            }
            set
            {
                if (this._LowTole != value)
                {
                    if (this.ToleChanging != null)
                        this.ToleChanging(this, new PropertyChangeEventArgs(this._LowTole));
                    this._LowTole = value;
                    if (this.AllowAutoRefresh)
                        RefreshTole();
                    if (this.ToleChanged != null)
                        this.ToleChanged(this, new PropertyChangeEventArgs(value));

                }
            }
        }
        private CPKType _SelectType = CPKType.Both;
        public CPKType SelectType
        {
            get
            {
                return this._SelectType;
            }
            set
            {
                if (this._SelectType != value)
                {
                    if (this.ToleChanging != null)
                        this.ToleChanging(this, new PropertyChangeEventArgs(this._SelectType));
                    this._SelectType = value;
                    if (this.AllowAutoRefresh)
                        RefreshTole();
                    if (this.ToleChanged != null)
                        this.ToleChanged(this, new PropertyChangeEventArgs(value));

                }
            }
        }
        private bool _AllowAutoRefresh = true;
        public bool AllowAutoRefresh
        {
            get
            {
                return this._AllowAutoRefresh;
            }
            set
            {
                this._AllowAutoRefresh = value;
            }
        }
        private STDEVType _StdevType = STDEVType.Std;
        public STDEVType StdevType
        {
            get
            {
                return this._StdevType;
            }
            set
            {
                if (this.GroupChanging != null)
                    this.GroupChanging(this, new PropertyChangeEventArgs(this._StdevType));
                this._StdevType = value;
                if (this.GroupChanged != null)
                    this.GroupChanged(this, new PropertyChangeEventArgs(value));
            }
        }
        private int _GroupLength = 5;
        public int GroupLength
        {
            get
            {
                return this._GroupLength;
            }
            set
            {
                if (this.GroupChanging != null)
                    this.GroupChanging(this, new PropertyChangeEventArgs(this._GroupLength));
                this._GroupLength = value;
                if (this.DataChanged != null)
                    this.DataChanged(this, new PropertyChangeEventArgs(value));
            }
        }
        //Output
        public double Max { get; protected set; }
        public double Min { get; protected set; }
        public double Avg { get; protected set; }
        public int Count { get; protected set; }
        public double P3Sigma { get; private set; }
        public double N3Sigma { get; private set; }
        public double SpOffset { get; private set; }
        public double SpOffsetD { get; private set; }
        public double UpStdLt { get; private set; }
        public double LowStdLt { get; private set; }
        public double TOffsetWidth { get; private set; }
        public double OffsetMid { get; private set; }
        public decimal UpBdRate_E { get; private set; }
        public decimal LowBdRate_E { get; private set; }
        public decimal SumBdRate_E { get; private set; }
        public decimal UpBdRate_G { get; private set; }
        public decimal LowBdRate_G { get; private set; }
        public decimal SumBdRate_G { get; private set; }
        public double STDev_E { get; protected set; }
        public double CP_E { get; private set; }
        public double CPK_E { get; private set; }
        public double CPL_E { get; private set; }
        public double CPU_E { get; private set; }
        public double STDev_G { get; private set; }
        public double CP_G { get; private set; }
        public double CPK_G { get; private set; }
        public double CPL_G { get; private set; }
        public double CPU_G { get; private set; }
        //Event
        public class PropertyChangeEventArgs : EventArgs
        {
            public object data;
            public PropertyChangeEventArgs(object d)
            {
                this.data = d;
            }
        }
        public event EventHandler<PropertyChangeEventArgs> DataChanged;
        public event EventHandler<PropertyChangeEventArgs> ParamChanged;
        public event EventHandler<PropertyChangeEventArgs> ToleChanged;
        public event EventHandler<PropertyChangeEventArgs> GroupChanged;
        public event EventHandler<PropertyChangeEventArgs> DataChanging;
        public event EventHandler<PropertyChangeEventArgs> ParamChanging;
        public event EventHandler<PropertyChangeEventArgs> ToleChanging;
        public event EventHandler<PropertyChangeEventArgs> GroupChanging;
        public event Action DataInitComplete;
        public event Action ToleInitComplete;
        //Constructors
        public CPKData(T data, string param)
        {
            this.Data = data;
            this.Param = param;
        }
        public CPKData()
        {
        }
        //Method
        public abstract double? GetProperty(int index);
        public abstract bool ContainParam(string param);
        protected abstract int GetCount();
        public virtual void RefreshData()
        {
            if (this.Param != null && this.Data != null && this.ContainParam(this.Param))
            {
                if (this.GetBasicStat())
                {
                    this.DataInitComplete();
                    this.RefreshTole();
                }
            }
        }
        public virtual void RefreshTole()
        {
            if (this.Param != null && this.Data != null && this.Count > 0 && this.ContainParam(this.Param))
            {
                this.GetStdLt();
                this.GetOffset();
                this.GetSpOffset();
                this.ToleInitComplete();
            }
        }
        public void InitCount()
        {
            int temp = this.GetCount();
            if (GroupLength > temp)
                GroupLength = temp;
            if (GroupLength < 1)
                GroupLength = 1;
            this.Count = temp - temp % GroupLength;
        }
        public bool GetBasicStat()
        {
            this.InitCount();
            if (this.Count < 1)
                return false;
            double avgtemp = 0;
            double maxtemp = double.NegativeInfinity;
            double mintemp = double.PositiveInfinity;
            for (int i = start, c = 0; c < Count; i++)
            {
                double? temp = this.GetProperty(i);
                if (temp != null)
                {
                    double dtemp = (double)temp;
                    avgtemp += dtemp / Count;
                    if (maxtemp < dtemp)
                        maxtemp = dtemp;
                    if (mintemp > dtemp)
                        mintemp = dtemp;
                    c++;
                }
            }
            this.Avg = avgtemp;
            this.Min = mintemp;
            this.Max = maxtemp;
            this.STDev_E = STDEV.GetEntirety(this, this.Avg, this.start, this.Count);
            return true;
        }

        public void GetStdLt()
        {
            if (this.SelectType == CPKType.Both)
            {
                this.UpStdLt = this.Standard + this.UpTole;
                this.LowStdLt = this.Standard + this.LowTole;
            }
            else if (this.SelectType == CPKType.Up)
            {
                this.UpStdLt = this.Standard + this.UpTole;
                this.LowStdLt = this.Standard;
            }
            else if (this.SelectType == CPKType.Low)
            {
                this.UpStdLt = this.Standard;
                this.LowStdLt = this.Standard + this.LowTole;
            }
            else
            {
                throw new Exception("没有选择规格类型");
            }
        }
        public void GetOffset()
        {
            if (this.SelectType == CPKType.Both)
            {
                this.TOffsetWidth = this.UpStdLt - this.LowStdLt;
                this.OffsetMid = this.Standard + (this.UpTole + this.LowTole) / 2;
            }
            else if (this.SelectType == CPKType.Up)
            {
                this.TOffsetWidth = this.UpStdLt - this.Standard;
                this.OffsetMid = this.Standard + this.UpTole / 2;
            }
            else if (this.SelectType == CPKType.Low)
            {
                this.TOffsetWidth = this.Standard - this.LowStdLt;
                this.OffsetMid = this.Standard + this.LowTole / 2;
            }
            else
            {
                throw new Exception("没有选择规格类型");
            }
        }
        public void GetSpOffset()
        {
            this.SpOffset = Math.Abs(this.OffsetMid - this.Avg);
            this.SpOffsetD = 2 * this.SpOffset / this.TOffsetWidth;
        }
        public void GetPPK()
        {
            this.CPU_E = (this.UpStdLt - this.Avg) / (3 * this.STDev_E);
            this.CPL_E = (this.Avg - this.LowStdLt) / (3 * this.STDev_E);
            this.CP_E = double.NaN;
            if (this.SelectType == CPKType.Both)
            {
                this.CP_E = this.TOffsetWidth / (6 * this.STDev_E);
                this.CPK_E = Math.Min(this.CPU_E, this.CPL_E);
            }
            else if (this.SelectType == CPKType.Up)
            {
                this.CP_E = (this.UpStdLt - this.Avg) / (3 * this.STDev_E);
                this.CPL_E = double.NaN;
                this.CPK_E = this.CPU_E;
            }
            else if (this.SelectType == CPKType.Low)
            {
                this.CP_E = (this.Avg - this.LowStdLt) / (3 * this.STDev_E);
                this.CPU_E = double.NaN;
                this.CPK_E = this.CPL_E;
            }
            else
            {
                throw new Exception("没有选择规格类型");
            }
        }
        public virtual void getSTDev_G()
        {
            this.STDev_G = STDEV.GetGroup(this, this.Avg, start, this.Count, this.GroupLength, this.StdevType);
        }
        public void GetCPK()
        {
            getSTDev_G();
            this.P3Sigma = this.Avg + this.STDev_G * 3;
            this.N3Sigma = this.Avg - this.STDev_G * 3;
            this.CPU_G = (this.UpStdLt - this.Avg) / (3 * this.STDev_G);
            this.CPL_G = (this.Avg - this.LowStdLt) / (3 * this.STDev_G);
            this.CP_G = double.NaN;
            if (this.SelectType == CPKType.Both)
            {
                this.CP_G = this.TOffsetWidth / (6 * this.STDev_G);
                this.CPK_G = Math.Min(this.CPU_G, this.CPL_G);
            }
            else if (this.SelectType == CPKType.Up)
            {
                this.CP_G = (this.UpStdLt - this.Avg) / (3 * this.STDev_G);
                this.CPL_G = double.NaN;
                this.CPK_G = this.CPU_G;
            }
            else if (this.SelectType == CPKType.Low)
            {
                this.CP_G = (this.Avg - this.LowStdLt) / (3 * this.STDev_G);
                this.CPU_G = double.NaN;
                this.CPK_G = this.CPL_G;
            }
            else
            {
                throw new Exception("没有选择规格类型");
            }
        }
        public void GetBdRate()
        {
            if (this.SelectType == CPKType.Both)
            {
                this.UpBdRate_E = 1 - NormalDistribution.GetP((this.UpStdLt - this.Avg) / this.STDev_E);
                this.LowBdRate_E = NormalDistribution.GetP((this.LowStdLt - this.Avg) / this.STDev_E);
                this.SumBdRate_E = this.UpBdRate_E + this.LowBdRate_E;
            }
            else if (this.SelectType == CPKType.Up)
            {
                this.UpBdRate_E = 1 - NormalDistribution.GetP((this.UpStdLt - this.Avg) / this.STDev_E);
                this.LowBdRate_E = 0;
                this.SumBdRate_E = this.UpBdRate_E;
            }
            else if (this.SelectType == CPKType.Low)
            {
                this.UpBdRate_E = 0;
                this.LowBdRate_E = NormalDistribution.GetP((this.LowStdLt - this.Avg) / this.STDev_E);
                this.SumBdRate_E = this.LowBdRate_E;
            }
            else
            {
                throw new Exception("没有选择规格类型");
            }
        }
        public void GetGroupBdRate()
        {
            if (this.SelectType == CPKType.Both)
            {
                this.UpBdRate_G = 1 - NormalDistribution.GetP((this.UpStdLt - this.Avg) / this.STDev_G);
                this.LowBdRate_G = NormalDistribution.GetP((this.LowStdLt - this.Avg) / this.STDev_G);
                this.SumBdRate_G = this.UpBdRate_G + this.LowBdRate_G;
            }
            else if (this.SelectType == CPKType.Up)
            {
                this.UpBdRate_G = 1 - NormalDistribution.GetP((this.UpStdLt - this.Avg) / this.STDev_G);
                this.LowBdRate_G = 0;
                this.SumBdRate_G = this.UpBdRate_G;
            }
            else if (this.SelectType == CPKType.Low)
            {
                this.UpBdRate_G = 0;
                this.LowBdRate_G = NormalDistribution.GetP((this.LowStdLt - this.Avg) / this.STDev_G);
                this.SumBdRate_G = this.LowBdRate_G;
            }
            else
            {
                throw new Exception("没有选择规格类型");
            }
        }


        public double? this[int index]
        {
            get
            {
                return this.GetProperty(index);
            }
        }
    }
    public struct CPKType
    {
        public string Description { get; set; }
        private static CPKType _Both = new CPKType("双侧公差");
        public static CPKType Both
        {
            get
            {
                return CPKType._Both;
            }
        }
        private static CPKType _Up = new CPKType("上限公差");
        public static CPKType Up
        {
            get
            {
                return CPKType._Up;
            }
        }
        private static CPKType _Low = new CPKType("下限公差");
        public static CPKType Low
        {
            get
            {
                return CPKType._Low;
            }
        }
        public CPKType(string de)
            : this()
        {
            this.Description = de;
        }
        public static bool operator ==(CPKType a, CPKType b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(CPKType a, CPKType b)
        {
            return !a.Equals(b);
        }
        public override bool Equals(object obj)
        {
            if (obj is CPKType)
                return this.Description == ((CPKType)obj).Description;
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
}
