using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPC.Base.Operation
{
    public class CustomGroupMaker
    {
        public const string StandardMaskString = @"(<=?(-?[0-9]+(.([0-9]+))?))*";
        public static List<Tuple<double, bool>> FormatBorder(string input)
        {
            if (input.Trim() == "")
                return null;
            List<Tuple<double, bool>> result = new List<Tuple<double, bool>>();
            var tempresult = input.Split('<');
            BasicSort.QuickSort<string>(1, tempresult.Length - 1, tempresult, borderCompare);
            if (tempresult[1][0] == '=')
                result.Add(new Tuple<double, bool>(Convert.ToDouble(tempresult[1].Substring(1)), true));
            else
                result.Add(new Tuple<double, bool>(Convert.ToDouble(tempresult[1]), false));
            for (int i = 2; i < tempresult.Length; i++)
            {
                if (tempresult[i] != tempresult[i - 1])
                {
                    if (tempresult[i][0] == '=')
                        result.Add(new Tuple<double, bool>(Convert.ToDouble(tempresult[i].Substring(1)), true));
                    else
                        result.Add(new Tuple<double, bool>(Convert.ToDouble(tempresult[i]), false));
                }
            }
            return result;
        }
        private static bool borderCompare(string a, string b)
        {
            bool _a, _b;
            double aa, bb;
            if (a[0] == '=')
            {
                aa = Convert.ToDouble(a.Substring(1));
                _a = true;
            }
            else
            {
                aa = Convert.ToDouble(a);
                _a = false;
            }
            if (b[0] == '=')
            {
                bb = Convert.ToDouble(b.Substring(1));
                _b = true;
            }
            else
            {
                bb = Convert.ToDouble(b);
                _b = false;
            }
            if (aa > bb || (aa == bb && _a && !_b))
                return true;
            else
                return false;
        }
    }
}
