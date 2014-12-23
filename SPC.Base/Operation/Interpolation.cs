using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SPC.Base.Interface;

namespace SPC.Base.Operation
{
    public class Interpolation
    {
        public Tuple<double[,],double,double,double,double> Start(ViewData data,string xp,string yp,string zp,int countperaxis)
        {
            int i,j,index;
            double x,y;
            int rowcount = data.RowCount;
            List<DPointValue> sortlist = new List<DPointValue>();
            List<DPointValue> sortlisty = new List<DPointValue>();
            DPointValue targetpoint;
            DPointValue temppoint;
            DataRow temprow;
            for(i = 0;i<rowcount;i++)
            {
                temprow = data[i];
                targetpoint = new DPointValue(temprow[xp].ConvertToDouble(), temprow[yp].ConvertToDouble(), temprow[zp].ConvertToDouble());
                sortlist.Add(targetpoint);
                sortlisty.Add(targetpoint);
            }
            sortlist.Sort(new DPointCompare());
            sortlisty.Sort(new DPointCompareY());
            targetpoint = sortlist[0];
            int tempcount = rowcount;
            for (i = 1; i < tempcount; )
            {
                temppoint = sortlist[i];
                if (targetpoint == temppoint)
                {
                    targetpoint.AddValue(temppoint.Value);
                    sortlist.RemoveAt(i);
                    tempcount--;
                }
                else
                {
                    targetpoint = sortlist[i];
                    i++;
                }
            }
            targetpoint = sortlisty[0];
            tempcount = rowcount;
            for (i = 1; i < tempcount; )
            {
                temppoint = sortlisty[i];
                if (targetpoint == temppoint)
                {
                    targetpoint.AddValue(temppoint.Value);
                    sortlisty.RemoveAt(i);
                    tempcount--;
                }
                else
                {
                    targetpoint = sortlisty[i];
                    i++;
                }
            }
            rowcount = sortlist.Count;
            double xmin = sortlist[0].X;
            double xmax = sortlist[rowcount-1].X;
            double ymin = sortlisty[0].Y;
            double ymax = sortlisty[rowcount - 1].Y;
            double xper = (xmax-xmin)/countperaxis;
            double yper = (ymax - ymin)/countperaxis;
            double[,] result = new double[countperaxis,countperaxis];
            for(i=0;i<countperaxis;i++)
            {
                x = xmin+xper*i;
                for(j=0;j<countperaxis;j++)
                {
                    y = ymin+yper*j;
                    targetpoint = new DPointValue(x, y,0);
                    if (sortlist.TryFind(targetpoint, out index))
                    {
                        result[i, j] = sortlist[index].Value;
                    }
                    else
                    {

                    }
                }
            }
            return new Tuple<double[,], double, double, double, double>(result, xmin, xmax, ymin, ymax);
        }

    }
    public static class ListExtern
    {
        public static bool TryFind<T>(this List<T> list,T target,out int index)
        {
            index = 1;
            return true;
        }
    }
}
