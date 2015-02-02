using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Microsoft.CSharp;
using DevExpress.XtraEditors;

namespace SPC.Controls.ConfigControls
{
    public partial class EngineConfigControl :ConfigControlBase
    {
        public override event EventHandler OKEvent;
        public override event EventHandler CancelEvent;
        Assembly methoddll = null;
        MethodInfo method = null;
        string[] columnList = null;
        string baseString = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPC.Engine
{{
    public static class Method
    {{
        public static {0} Start({1})
        {{
            {0} {3};
            {2}
            return {3};
        }}
    }}
}}
";
        string helpMessage = @"语法采用C#常用语法格式，参数用[参数]表示,在代码中对现有的列赋值无效。";
        string compileString;
        List<string> inputColumns;
        Type targettype;
        string targetcolumn;
        SPC.Base.Interface.IDataTable<DataRow> Data;
        public EngineConfigControl()
        {
            InitializeComponent();
        }
        public void Init(SPC.Base.Interface.IDataTable<DataRow> data)
        {
            this.Data = data;
            this.listBoxControl1.Items.Clear();
            this.columnList = Data.GetColumnsList();
            foreach (string column in columnList)
            {
                this.listBoxControl1.Items.Add(column);
            }
        }

        private void listBoxControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index;
            if ((index = this.listBoxControl1.IndexFromPoint(e.Location)) >= 0)
                this.memoEdit1.SelectedText = "[" + this.listBoxControl1.Items[index].ToString() + "]";
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            targetcolumn = this.textEdit1.Text.Trim(); 
            if(targetcolumn=="")
            {
                MessageBox.Show("请指定输出列");
                return;
            }
            string mainstring = this.memoEdit1.Text;
            string id = "a" + Guid.NewGuid().ToString().Substring(0, 5);
            string inputstring = "";
            string returnstring = "";
            inputColumns = new List<string>();
            returnstring = id + "_" + targetcolumn;
            if (columnList.Contains(targetcolumn))
            {
                if (MessageBox.Show("原数据中已有列" + targetcolumn + ",是否要覆盖", "Warning", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    return;
                inputColumns.Add(targetcolumn);
                this.targettype = this.Data.GetColumnType(targetcolumn);
                inputstring += targettype.ToString() + " " + returnstring + ",";
            }
            else
            {
                switch (this.comboBoxEdit1.SelectedIndex)
                {
                    case 0: this.targettype = typeof(double); break;
                    case 1: this.targettype = typeof(string); break;
                    case 2: this.targettype = typeof(DateTime); break;
                    case 3: this.targettype = typeof(bool); break;
                    default: this.targettype = typeof(double); break;
                }
            }
            mainstring = mainstring.Replace("[" + targetcolumn + "]", returnstring);
            var columns = Regex.Matches(mainstring, @"(?<=\[)(\w*)(?=\])");
            string tempvalue;
            foreach (Match column in columns)
            {
                tempvalue = column.Value;
                if (columnList.Contains(tempvalue) && !inputColumns.Contains(tempvalue))
                {
                    inputColumns.Add(tempvalue);
                    inputstring += this.Data.GetColumnType(tempvalue).ToString() + " " + id + "_" + tempvalue + ",";
                    mainstring = mainstring.Replace("[" + tempvalue + "]", id + "_" + tempvalue);
                }
            }
            if (inputstring.EndsWith(","))
            {
                inputstring = inputstring.Substring(0, inputstring.Length - 1);
            }
            this.compileString = string.Format(baseString, targettype.ToString(), inputstring, mainstring, returnstring);
            compile(compileString);
        }

        private void compile(string target)
        {
            CSharpCodeProvider objCSharpCodePrivoder = new CSharpCodeProvider();
            CompilerParameters objCompilerParameters = new CompilerParameters();
            objCompilerParameters.ReferencedAssemblies.Add("System.dll");

            objCompilerParameters.ReferencedAssemblies.Add("System.Data.dll");

            objCompilerParameters.ReferencedAssemblies.Add("System.Core.dll");
            objCompilerParameters.GenerateExecutable = false;
            objCompilerParameters.GenerateInMemory = true;
            CompilerResults cr = objCSharpCodePrivoder.CompileAssemblyFromSource(objCompilerParameters, this.compileString);
            if (cr.Errors.HasErrors)
            {
                string errorstring = "";
                foreach (CompilerError err in cr.Errors)
                {
                    errorstring += err.ErrorText + "\n";
                }
                MessageBox.Show("编译错误：\n" + errorstring);
                this.simpleButton2.Enabled = false;
                return;
            }
            this.methoddll = cr.CompiledAssembly;
            this.method = methoddll.GetType("SPC.Engine.Method").GetMethod("Start");
            this.simpleButton2.Enabled = true;

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (OKEvent != null)
                OKEvent(this, new EventArgs());
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (CancelEvent != null)
                CancelEvent(this, new EventArgs());
        }
        public void doMethod()
        {
            int length = this.inputColumns.Count;
            object[] inputs = new object[length];
            int i,j;
            bool addcolumn = false ;
            if (this.method != null)
            {
                try
                {
                    if (!this.columnList.Contains(this.targetcolumn))
                    {
                        Data.AddColumn(targetcolumn, targettype);
                        addcolumn = true;
                    }
                    int count = Data.RowCount;
                    DataRow temprow;
                    for (i = 0; i < count; i++)
                    {
                        temprow = Data[i];
                        for (j = 0; j < length; j++)
                        {
                            inputs[j] = temprow[inputColumns[j]];
                        }
                        temprow[targetcolumn] = method.Invoke(null, inputs);
                    }
                }
                catch(Exception ex)
                {
                    if (addcolumn)
                        Data.RemoveColumn(targetcolumn);
                    throw ex;
                }
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            MessageBox.Show(helpMessage);
        }
    }
}
