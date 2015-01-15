using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Data;
using StatConnectorCommonLib;
using STATCONNECTORSRVLib;

namespace SPC.Rnet
{
    internal class Graphics : MarshalByRefObject
    {
        internal void DrawContourPlot(string path, string name, string xname, string yname, string zname, double[] x, double[] y, double[] z, int width, int height)
        {
            var con = new StatConnectorClass();
            try
            {
                con.Init("R");
                con.EvaluateNoReturn("setwd(\"" + path.Replace("\\", "/") + "/\")");
                con.EvaluateNoReturn("png(file=\"" + name + "\", bg=\"transparent\",width=" + width + ",height=" + height + ")");
                con.EvaluateNoReturn("library(akima)");
                con.SetSymbol("x", x);
                con.SetSymbol("y", y);
                con.SetSymbol("z", z);
                con.EvaluateNoReturn("r<-interp(x,y,z,seq(min(x),max(x),length = 100),seq(min(y),max(y),length = 100),duplicate = \"mean\")");
                con.EvaluateNoReturn("zz<-matrix(r$z,nrow = 100,ncol = 100,byrow = TRUE)");
                con.EvaluateNoReturn("lvs = pretty(range(zz,finite=TRUE),20)");
                con.EvaluateNoReturn("leg = length(lvs)-1");
                con.EvaluateNoReturn("filled.contour(r$x,r$y,zz,color.palette=heat.colors,col=heat.colors(leg)[leg:1])");
                con.EvaluateNoReturn("dev.off()");
            }
            catch
            {  
                throw new Exception(con.GetErrorText());
            }
            finally
            {
                con.Close();
            }
        }
    }
}
