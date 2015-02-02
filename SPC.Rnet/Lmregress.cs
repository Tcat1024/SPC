using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StatConnectorCommonLib;
using STATCONNECTORSRVLib;

namespace SPC.Rnet
{
    internal class Lmregress : MarshalByRefObject
    {
        internal double[] BaseStart(string path, string name, int width, int height, string targetcolumn, string[] sourcecolumns, Dictionary<string, double[]> doubledata)
        {
            var con = new StatConnectorClass();
            double[] coefficients = null;
            try
            {
                con.Init("R");
                con.EvaluateNoReturn("setwd(\"" + path.Replace("\\", "/") + "/\")");
                con.EvaluateNoReturn("png(file=\"" + name + ".png\", bg=\"transparent\",width=" + width + ",height=" + height + ")");
                string function;
                int i, length = sourcecolumns.Length;
                foreach (var item in doubledata)
                {
                    con.SetSymbol(item.Key, item.Value);
                    con.EvaluateNoReturn(item.Key + "<-as.vector(" + item.Key + ")");
                }
                function = targetcolumn + " ~ ";
                for (i = 0; i < length - 1; i++)
                {
                    function += sourcecolumns[i] + " + ";
                }
                function += sourcecolumns[length - 1];
                con.EvaluateNoReturn("result<-lm(" + function + ")");
                con.EvaluateNoReturn("par(mfrow=c(2,2))");
                con.EvaluateNoReturn("plot(result,which=1:4)");
                con.EvaluateNoReturn(string.Format("sink(\"{0}.txt\")", name));
                con.EvaluateNoReturn("print(summary(result))");
                con.EvaluateNoReturn("sink()");
                coefficients = con.Evaluate("as.array(result$coefficient)") as double[];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " RException:" + con.GetErrorText());
            }
            finally
            {
                con.Close();
            }
            return coefficients;
        }
    }
}
