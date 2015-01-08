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
        internal void DrawContourPlot(string path, string name, double[] x, double[] y, double[] z)
        {
            var con = new StatConnectorClass();
            try
            {
                con.Init("R");
                con.EvaluateNoReturn("setwd(\"" + path.Replace("\\", "/") + "/\")");
                con.EvaluateNoReturn("png(file=\"" + name + "\", bg=\"transparent\")");
                con.EvaluateNoReturn("library(akima)");
                con.SetSymbol("x", x);
                con.SetSymbol("y", y);
                con.SetSymbol("z", z);
                con.EvaluateNoReturn("r<-interp(x,y,z,seq(min(x),max(x),length = 100),seq(min(y),max(y),length = 100))");
                con.EvaluateNoReturn("zz<-matrix(r$z,nrow = 100,ncol = 100,byrow = TRUE)");
                con.EvaluateNoReturn("image(r$x,r$y,zz)");
                con.EvaluateNoReturn("contour(r$x,r$y,zz,col = \"black\", add = TRUE, method = \"edge\",vfont = c(\"sans serif\", \"plain\"))");
                con.EvaluateNoReturn("dev.off()");
            }
            catch
            {
                throw new Exception(con.GetErrorText());
            }
        }
    }
}
