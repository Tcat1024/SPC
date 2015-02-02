using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using StatConnectorCommonLib;
using STATCONNECTORSRVLib;

namespace SPC.Rnet
{
    internal class Rpart:MarshalByRefObject
    {
        internal double[,] BaseStart(string path,string name,int width,int height,string targetcolumn, string[] sourcecolumns,Dictionary<string,double[]> doubledata,Dictionary<string,string[]> stringdata,string method,double cp)
        {
            var con = new StatConnectorClass();
            double[,] result;
            try
            {
                con.Init("R");
                con.EvaluateNoReturn("setwd(\"" + path.Replace("\\", "/") + "/\")");
                con.EvaluateNoReturn("png(file=\"" + name + ".png\", bg=\"transparent\",width=" + width + ",height=" + height + ")");
                string function;
                int i,length = sourcecolumns.Length;
                foreach(var item in doubledata)
                {
                    con.SetSymbol(item.Key, item.Value);
                    con.EvaluateNoReturn(item.Key + "<-as.vector(" + item.Key + ")");
                }
                foreach (var item in stringdata)
                {
                    con.SetSymbol(item.Key, item.Value);
                    con.EvaluateNoReturn(item.Key + "<-as.vector(" + item.Key + ")");
                }
                function = targetcolumn + " ~ ";
                for (i = 0; i < length-1;i++ )
                {
                    function += sourcecolumns[i] + " + ";
                }
                function += sourcecolumns[length - 1];
                con.EvaluateNoReturn("library(rpart)");
                con.EvaluateNoReturn("library(rpart.plot)");
                con.EvaluateNoReturn("result<-rpart("+function+",method=\""+method+"\")");
                if(cp>0)
                {
                    con.EvaluateNoReturn("result<-prune(result,cp="+cp+")");
                }
                con.EvaluateNoReturn("rpart.plot(result,type=0,nn=T,yesno=F,cex=0.0000001,nn.cex=1,split.col=0)");
                con.EvaluateNoReturn(string.Format("sink(\"{0}.txt\")",name));
                con.EvaluateNoReturn("print(result)");
                con.EvaluateNoReturn("sink()");
                result = con.Evaluate("result$cptable") as double[,];
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message+" RException:"+con.GetErrorText());
            }
            finally
            {
                con.Close();
            }
            return result;
        }
    }
}
