using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPC.Monitor
{
    public class SPCDetermineMethod
    {
        public double UCL { get; set; }
        public double LCL { get; set; }
        public double Standard { get; set; }
        public List<SPCCommandbase> Commands = new List<SPCCommandbase>();
        public SPCDetermineMethod(double ucl,double lcl,double standard,params SPCCommandbase[] commands)
        {
            this.UCL = ucl;
            this.LCL = lcl;
            this.Standard = standard;
            foreach (var command in commands)
            {
                this.Commands.Add(command);
            }
        }
        public SPCDetermineMethod(double ucl, double lcl, double standard,List<SPCCommandbase> commands)
        {
            this.UCL = ucl;
            this.LCL = lcl;
            this.Standard = standard;
            this.Commands = commands;
        }
        public List<SPCCommandbase> Excute(double data)
        {
            List<SPCCommandbase> result = new List<SPCCommandbase>();
            foreach(var command in Commands)
            {
                if (command.Excute(data, this.UCL, this.LCL, this.Standard))
                    result.Add(command);
            }
            return result;
        }
    }
    public static class SPCCommand
    {
        public static List<SPCCommandbase> GetCommandList()
        {
            List<SPCCommandbase> result = new List<SPCCommandbase>();
            var properties = typeof(SPCCommand).GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            foreach(var property in properties)
            {
               result.Add((SPCCommandbase)property.GetValue(null, null));
            }
            return result;
        }
        public static SPCCommandbase[] GetCommandArray()
        {
            
            var properties = typeof(SPCCommand).GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            int count = properties.Length;
            var result = new SPCCommandbase[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = (SPCCommandbase)properties[i].GetValue(null, null);
            }
            return result;
        }
        public static SPCCommandbase SPCRule1 
        { 
            get
            {
                return new spcRule1();
            }
         }
        public static SPCCommandbase SPCRule2
        {
            get
            {
                return new spcRule2();
            }
        }
        public static SPCCommandbase SPCRule3
        {
            get
            {
                return new spcRule3();
            }
        }
        public static SPCCommandbase SPCRule4
        {
            get
            {
                return new spcRule4();
            }
        }
        public static SPCCommandbase SPCRule5
        {
            get
            {
                return new spcRule5();
            }
        }
        public static SPCCommandbase SPCRule6
        {
            get
            {
                return new spcRule6();
            }
        }
        public static SPCCommandbase SPCRule7
        {
            get
            {
                return new spcRule7();
            }
        }
        public static SPCCommandbase SPCRule8
        {
            get
            {
                return new spcRule8();
            }
        }
        private class spcRule1 : SPCCommandbase
        {
            public override int ID
            {
                get { return 1; }
            }
            public override int WarningCount
            {
                get { return 1; }
            }
            public override string Title
            {
                get { return "判定规则1"; }
            }

            public override string Description
            {
                get { return "1个点落在A区以外"; }
            }

            public override bool Excute(double data, double ucl, double lcl, double standard)
            {
                if (data > ucl+standard || data < lcl+standard)
                    return true;
                return false;
            }
        }
        private class spcRule2 : SPCCommandbase
        {
            public override int ID
            {
                get { return 2; }
            }
            public override int WarningCount
            {
                get { return 9; }
            }
            public override string Title
            {
                get { return "判定规则2"; }
            }

            public override string Description
            {
                get { return "连续9个点落在中心线同一侧"; }
            }
            private int count = 0;
            private bool? up = true;
            public override bool Excute(double data, double ucl, double lcl, double standard)
            {
                if(data==standard)
                {
                    up = null;
                    count = 0;
                    return false;
                }
                bool temp = data > standard;
                if (count > 0 && temp == up)
                {
                    count++;
                    if (count >= 9)
                        return true;
                }
                else
                {
                    up = temp;
                    count = 1;
                }
                return false;
            }
        }
        private class spcRule3 : SPCCommandbase
        {
            public override int ID
            {
                get { return 3; }
            }
            public override int WarningCount
            {
                get { return 6; }
            }
            public override string Title
            {
                get { return "判定规则3"; }
            }

            public override string Description
            {
                get { return "连续6个点递增或递减"; }
            }
            private int count = 0;
            private double last = 0;
            private bool? up = true;
            public override bool Excute(double data, double ucl, double lcl, double standard)
            {
                if(data == last)
                {
                    last = data;
                    up = null;
                    count = 0;
                    return false;
                }
                bool temp = data > last;
                if (count > 0 && temp == up)
                {
                    count++;
                    last = data;
                    if (count >= 6)
                        return true;
                }
                else
                {
                    up = temp;
                    count = 1;
                    last = data;
                }
                return false;
            }
        }
        private class spcRule4 : SPCCommandbase
        {
            public override int ID
            {
                get { return 4; }
            }
            public override int WarningCount
            {
                get { return 14; }
            }
            public override string Title
            {
                get { return "判定规则4"; }
            }

            public override string Description
            {
                get { return "连续14个点中相邻点交替上下"; }
            }
            private int count = 0;
            private double last = 0;
            private bool? up = true;
            public override bool Excute(double data, double ucl, double lcl, double standard)
            {
                if (data == last)
                {
                    last = data;
                    up = null;
                    count = 0;
                    return false;
                }
                bool temp = (data > last);
                if (count > 0 && temp != up)
                {
                    count++;
                    last = data;
                    up = temp;
                    if (count >= 14)
                        return true;
                }
                else
                {
                    up = temp;
                    count = 1;
                    last = data;
                }
                return false;
            }
        }
        private class spcRule5 : SPCCommandbase
        {
            public override int ID
            {
                get { return 5; }
            }
            public override int WarningCount
            {
                get { return 3; }
            }
            public override string Title
            {
                get { return "判定规则5"; }
            }

            public override string Description
            {
                get { return "连续3个点中有2个点落在中心线同一侧的B区以外"; }
            }
            private List<double> old = new List<double>();
            private int count;
            public override bool Excute(double data, double ucl, double lcl, double standard)
            {
                double bu =standard+ ucl/ 3 * 2;
                double bl =standard+ lcl / 3 * 2;
                int u = 0;
                int l = 0;
                old.Add(data);
                count++;
                if (count > 3)
                {
                    count--;
                    old.RemoveAt(0);
                }
                foreach(double o in old)
                {
                    if (o > bu)
                        u++;
                    else if (o < bl)
                        l++;
                }
                if (u >= 2 || l >= 2)
                    return true;
                return false;
            }
        }
        private class spcRule6 : SPCCommandbase
        {
            public override int ID
            {
                get { return 6; }
            }
            public override int WarningCount
            {
                get { return 5; }
            }
            public override string Title
            {
                get { return "判定规则6"; }
            }

            public override string Description
            {
                get { return "连续5个点中有4个点落在中心线同一侧的C区以外"; }
            }
            private List<double> old = new List<double>();
            private int count;
            public override bool Excute(double data, double ucl, double lcl, double standard)
            {
                double bu = standard + ucl / 3;
                double bl = standard + lcl / 3;
                int u = 0;
                int l = 0;
                old.Add(data);
                count++;
                if (count > 5)
                {
                    count--;
                    old.RemoveAt(0);
                }
                foreach (double o in old)
                {
                    if (o > bu)
                        u++;
                    else if (o < bl)
                        l++;
                }
                if (u >= 4 || l >= 4)
                    return true;
                return false;
            }
        }
        private class spcRule7 : SPCCommandbase
        {
            public override int ID
            {
                get { return 7; }
            }
            public override int WarningCount
            {
                get { return 15; }
            }
            public override string Title
            {
                get { return "判定规则7"; }
            }

            public override string Description
            {
                get { return "连续15个点落在中心线两侧的C区内"; }
            }
            private int count;
            public override bool Excute(double data, double ucl, double lcl, double standard)
            {
                double bu =standard+ ucl / 3;
                double bl = standard + lcl / 3;
                if (data < bu && data > bl)
                    count++;
                else
                    count = 0;
                if (count >= 15)
                    return true;
                return false;
            }
        }
        private class spcRule8 : SPCCommandbase
        {
            public override int ID
            {
                get { return 8; }
            }
            public override int WarningCount
            {
                get { return 8; }
            }
            public override string Title
            {
                get { return "判定规则8"; }
            }

            public override string Description
            {
                get { return "连续8个点落在中心线两侧且无一在C区内"; }
            }
            private int count;
            public override bool Excute(double data, double ucl, double lcl, double standard)
            {
                double bu = standard + ucl/ 3;
                double bl = standard + lcl / 3;
                if (data > bu || data < bl)
                    count++;
                else
                    count = 0;
                if (count >= 8)
                    return true;
                return false;
            }
        }
        public static SPCCommandbase GetCommandfromTitle(string name)
        {
            return (SPCCommandbase)typeof(SPCCommand).GetProperty(name).GetValue(null, null);
        }

    }
    
    public abstract class SPCCommandbase
    {
        public abstract int ID { get; }
        public abstract int WarningCount { get; }
        public abstract string Title { get; }
        public abstract string Description { get; }
        public abstract bool Excute(double data, double ucl, double lcl, double standard);
        public override string ToString()
        {
            return this.Title+":"+this.Description;
        }
    }
}
