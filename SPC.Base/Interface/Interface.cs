using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SPC.Base.Operation;

namespace SPC.Base.Interface
{
    public class ArrayVector:ICanGetProperty
    {
        public double[] Data;
        public double AVG { get; set; }
        public ArrayVector(int count)
        {
            Data = new double[count];
        }
        public bool ContainParam(string param)
        {
            return true;
        }
        public double? GetProperty(int input)
        {
            return Data[input];
        }
        public double? this[int index]
        {
            get { return Data[index]; }
        }
    }
    public interface ICanGetProperty
    {
        double? GetProperty(int input);
        bool ContainParam(string param);
        double? this[int index] { get; }
    }
    public abstract class SingleSeriesManager<SourceDataType,DrawBoardType>
    {
        protected ISeriesMaker<SourceDataType> SeriesMaker;
        protected ISeriesDrawer<DrawBoardType> SeriesDrawer;
        private BasicSeriesData SeriesData;
        public IDrawBoard<DrawBoardType> DrawBoard;
        protected abstract void Init();
        public SingleSeriesManager()
        {
            Init();
        }
        public void InitData(SourceDataType sourceData)
        {
            this.SeriesData = this.SeriesMaker.Make(sourceData);
        }
        public void DrawSeries(System.Drawing.Color color)
        {
            this.SeriesDrawer.Draw(SeriesData, color, this.DrawBoard.GetChart());
        }
        public void RemoveSeries()
        {
            this.SeriesDrawer.Clear();
        }
    }
    public interface ISeriesMaker<SourceDataType>
    {
        BasicSeriesData Make(SourceDataType sourceData);
    }
    public interface ISeriesDrawer<DrawBoardType>:IDisposable
    {
        void Draw(BasicSeriesData data, System.Drawing.Color color,DrawBoardType drawBoard);
        void Clear();
    }
    public interface IDrawBoard<ChartType>
    {
        ChartType GetChart();
        System.Windows.Forms.Control Parent { get; }
        bool Selected { get; set; }
        object Tag { get; set; }
        event EventHandler GotFocus;
        event EventHandler LostFocus;
        event EventHandler Removed;
        void Hup();
        void Vup();
        void Hdown();
        void Vdown();
        void Re();
    }
}
