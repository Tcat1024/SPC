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
            string name = Guid.NewGuid() + ".png";
            var root = new DirectoryInfo(System.Windows.Forms.Application.StartupPath+"\\..\\Temp");
            if (!root.Exists)
                root.Create();
            string fullpath = root.FullName + "\\" + name;
            byte[] buffer;
            try
            {
                (domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(Graphics).FullName) as Graphics).DrawContourPlot(root.FullName, name, x, y, z,width,height,levels,drawline);
                FileStream fs = new FileStream(fullpath, FileMode.Open);
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
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
    }
}
