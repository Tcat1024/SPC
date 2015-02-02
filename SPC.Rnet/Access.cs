using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Drawing;
using System.IO;
using SPC.Base.Interface;
using SPC.Base.Operation;

namespace SPC.Rnet
{
    public static class Methods
    {
        private static List<string> RpartMethod = new List<string>() {"anova","poisson","class","exp"};
        public static List<object> RunScript(string path,List<object> inputs)
        {
            AppDomain domain = AppDomain.CreateDomain(Guid.NewGuid().ToString());
            List<object> result = null;
            try
            {
                result = (domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(Basic).FullName) as Basic).runScript(path, inputs);             
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                AppDomain.Unload(domain);
            }
            return result;

        }
        public static Image DrawContourPlot(IDataTable<DataRow> data, string xs, string ys, string zs,int width,int height,double[] levels,bool drawline)
        {
            int rowcount = data.RowCount;
            double[] x = new double[rowcount];
            double[] y = new double[rowcount];
            double[] z = new double[rowcount];
            DataRow row;
            for (int i = 0; i < rowcount; i++)
            {
                row = data[i];
                x[i] = row[xs].ConvertToDouble();
                y[i] = row[ys].ConvertToDouble();
                z[i] = row[zs].ConvertToDouble();
            }
            AppDomain domain = AppDomain.CreateDomain(Guid.NewGuid().ToString());
            string name = Guid.NewGuid().ToString();
            var root = new DirectoryInfo(System.Windows.Forms.Application.StartupPath+"\\..\\Temp");
            if (!root.Exists)
                root.Create();
            string fullpath = root.FullName + "\\" + name+".png";
            byte[] buffer;
            try
            {
                (domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(Graphics).FullName) as Graphics).DrawContourPlot(root.FullName, name, x, y, z,width,height,levels,drawline);
                using (FileStream fs = new FileStream(fullpath, FileMode.Open))
                {
                    buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                AppDomain.Unload(domain);
                File.Delete(fullpath);
            }
            return Image.FromStream(new MemoryStream(buffer));
        }
        public static Tuple<Image,string,double[,]> Rpart(IDataTable<DataRow> data,int width,int height, string targetcolumn,string[] sourcecolumns,string method,double cp)
        {
            AppDomain domain = AppDomain.CreateDomain(Guid.NewGuid().ToString());
            string name = Guid.NewGuid().ToString();
            var root = new DirectoryInfo(System.Windows.Forms.Application.StartupPath + "\\..\\Temp");
            if (!root.Exists)
                root.Create();
            string fullimagepath = root.FullName + "\\" + name + ".png";
            string fulltextpath = root.FullName + "\\" + name + ".txt";
            byte[] buffer =null;
            string result = "";
            double[] tempd;
            string[] temps;
            double[,] cptable = null;
            try
            {
                int rcount = data.RowCount;
                int ccount = sourcecolumns.Length+1;
                int i;
                List<double[]> sourcedata = new List<double[]>();
                Dictionary<string, double[]> doubledata = new Dictionary<string, double[]>();
                Dictionary<string, string[]> stringdata = new Dictionary<string, string[]>();
                var type = data.GetColumnType(targetcolumn);
                if (type == typeof(double) || type == typeof(int) || type == typeof(decimal) || type == typeof(float))
                {
                    tempd = new double[rcount];
                    for (i = 0; i < rcount; i++)
                        tempd[i] = data[i, targetcolumn].ConvertToDouble();
                    doubledata.Add(targetcolumn, tempd);
                }
                else
                {
                    temps = new string[rcount];
                    for (i = 0; i < rcount; i++)
                        temps[i] = data[i, targetcolumn].ToString();
                    stringdata.Add(targetcolumn, temps);
                }
                foreach(var sourcecolumn in sourcecolumns)
                {
                    type = data.GetColumnType(sourcecolumn);
                    if (type == typeof(double) || type == typeof(int) || type == typeof(decimal) || type == typeof(float))
                    {
                        tempd = new double[rcount];
                        for (i = 0; i < rcount; i++)
                            tempd[i] = data[i, sourcecolumn].ConvertToDouble();
                        doubledata.Add(sourcecolumn, tempd);
                    }
                    else
                    {
                        temps = new string[rcount];
                        for (i = 0; i < rcount; i++)
                            temps[i] = data[i, sourcecolumn].ToString();
                        stringdata.Add(sourcecolumn, temps);
                    }
                }
                if (!RpartMethod.Contains(method.ToLower()))
                    throw new Exception("不支持的方法参数");
                cptable = (domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(Rpart).FullName) as Rpart).BaseStart(root.FullName, name, width, height, targetcolumn, sourcecolumns, doubledata, stringdata, method.ToString().ToLower(),cp);
                using (FileStream fs = new FileStream(fullimagepath, FileMode.Open))
                {
                    buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                }
                using (StreamReader sr = new StreamReader(fulltextpath,Encoding.Default))
                {
                    result = sr.ReadToEnd();
                }          
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                AppDomain.Unload(domain);
                File.Delete(fullimagepath);
                File.Delete(fulltextpath);
            }
            return new Tuple<Image,string,double[,]>(Image.FromStream(new MemoryStream(buffer)),result,cptable);
        }
        public static Tuple<Image, string, double[]> LmGress(IDataTable<DataRow> data, int width, int height, string targetcolumn, string[] sourcecolumns)
        {
            AppDomain domain = AppDomain.CreateDomain(Guid.NewGuid().ToString());
            string name = Guid.NewGuid().ToString();
            var root = new DirectoryInfo(System.Windows.Forms.Application.StartupPath + "\\..\\Temp");
            if (!root.Exists)
                root.Create();
            string fullimagepath = root.FullName + "\\" + name + ".png";
            string fulltextpath = root.FullName + "\\" + name + ".txt";
            byte[] buffer = null;
            string result = "";
            double[] coe = null;
            try
            {
                int rcount = data.RowCount;
                int ccount = sourcecolumns.Length + 1;
                int i;
                List<double[]> sourcedata = new List<double[]>();
                Dictionary<string, double[]> doubledata = new Dictionary<string, double[]>();
                Dictionary<string, string[]> stringdata = new Dictionary<string, string[]>();
                double[] temp = new double[rcount];
                    for (i = 0; i < rcount; i++)
                        temp[i] = data[i, targetcolumn].ConvertToDouble();
                    doubledata.Add(targetcolumn, temp);
               
                foreach (var sourcecolumn in sourcecolumns)
                {
                    temp = new double[rcount];
                    for (i = 0; i < rcount; i++)
                        temp[i] = data[i, sourcecolumn].ConvertToDouble();
                    doubledata.Add(sourcecolumn, temp);
                }
                coe = (domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(Lmregress).FullName) as Lmregress).BaseStart(root.FullName, name, width, height, targetcolumn, sourcecolumns, doubledata);
                using (FileStream fs = new FileStream(fullimagepath, FileMode.Open))
                {
                    buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                }
                using (StreamReader sr = new StreamReader(fulltextpath, Encoding.Default))
                {
                    result = sr.ReadToEnd();
                    int start = result.IndexOf("Residuals:");
                    if (start >= 0)
                        result = result.Substring(start);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                AppDomain.Unload(domain);
                File.Delete(fullimagepath);
                File.Delete(fulltextpath);
            }
            return new Tuple<Image, string, double[]>(Image.FromStream(new MemoryStream(buffer)), result, coe);
        }
    }
}
