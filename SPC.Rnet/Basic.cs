using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using StatConnectorCommonLib;
using STATCONNECTORSRVLib;

namespace SPC.Rnet
{
    internal class Basic : MarshalByRefObject
    {
        internal List<object> runScript(string path, List<object> inputs)
        {
            var lines = File.ReadAllLines(path);
            int length = lines.Length;
            int i =-1 ;
            List<object> result = new List<object>();
            IStatConnector con = new StatConnectorClass();
            con.Init("R");
            try
            {
                for (i = 0; i < length; i++)
                {
                    if(lines[i].StartsWith("SetSymbol(")&&lines[i].EndsWith(")"))
                    {
                        try
                        {
                            var parts = lines[i].Split('(', ',', ')');
                            con.SetSymbol(parts[1], inputs[int.Parse(parts[2])]);
                        }
                        catch (Exception ex)
                        {
                            throw new FormatException(ex.Message);
                        }
                    }
                    else if (lines[i].StartsWith("GetSymbol(") && lines[i].EndsWith(")"))
                    {
                        try
                        {
                            var parts = lines[i].Split('(',')');
                            result.Add(con.GetSymbol(parts[1]));
                        }
                        catch (Exception ex)
                        {
                            throw new FormatException(ex.Message);
                        }
                    }
                    else
                        con.EvaluateNoReturn(lines[i]);
                }
            }
            catch(FormatException ex)
            {
                con.Close();
                throw new Exception("Line:"+(i+1)+" "+ex.Message);
            }
            catch
            {
                con.Close();
                throw new Exception("Line:" + (i + 1) + " " + con.GetErrorText());
            }
            return result;
        }
    }
    public class FormatException:Exception
    {
        public FormatException(string message):base(message)
        {
           
        }
    }
    public class Convert
    {
        public static string[] StringToUnicode(string s)
        {
            System.Text.Encoding.Unicode.GetBytes(s);
            char[] charbuffers = s.ToCharArray();
            byte[] buffer;
            int length = charbuffers.Length;
            string[] result = new string[length];
            for (int i = 0; i < length; i++)
            {
                buffer = System.Text.Encoding.Unicode.GetBytes(charbuffers[i].ToString());
                result[i] = String.Format("0x{0:X2}{1:X2}", buffer[1], buffer[0]);
            }
            return result;
        }
    }
}
