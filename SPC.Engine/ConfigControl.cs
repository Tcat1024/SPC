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

namespace SPC.Engine
{
    public partial class ConfigControl : DevExpress.XtraEditors.XtraUserControl
    {
        Assembly methoddll = null;
        DataColumnCollection Columns;
        string baseString = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPC.Engine
{
    class Method
    {
        public static {0} Start({1})
        {
            {2}
            return {3};
        }
    }
}
";
        public ConfigControl()
        {
            InitializeComponent();
        }
        public void Init(DataColumnCollection columns)
        {
            Columns = columns;
            this.listBoxControl1.Items.Clear();
            foreach (DataColumn column in this.Columns)
            {
                this.listBoxControl1.Items.Add(column.ColumnName);
            }
        }

        private void listBoxControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index;
            if ((index = this.listBoxControl1.IndexFromPoint(this.listBoxControl1.PointToClient(e.Location))) >= 0)
                this.memoEdit1.SelectedText = "[" + this.listBoxControl1.Items[index].ToString() + "]";
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Analyze(this.memoEdit1.Text);
        }
        string inputstring;
        string id;
        string returnstring;
        List<string> inputColumns;
        private string Analyze(string target)
        {
            string mainstring = target;
            id = Guid.NewGuid().ToString().Substring(0, 5);
            inputstring = "";
            returnstring = "";
            inputColumns = new List<string>();
            string targetcolumn = this.textEdit1.Text.Trim();
            returnstring = id + "_" + targetcolumn;
            if (this.Columns.Contains(targetcolumn))
            {
                inputColumns.Add(targetcolumn);
                inputstring += this.Columns[targetcolumn].DataType.ToString() + " " + returnstring;
            }
            mainstring = mainstring.Replace("[" + targetcolumn + "]", returnstring);
            var columns = Regex.Matches(target, @"(?<=\[)(\w*)(?=\])");
            string tempvalue;
            foreach (Match column in columns)
            {
                tempvalue = column.Value;
                if (this.Columns.Contains(tempvalue) && !inputColumns.Contains(tempvalue))
                {
                    inputColumns.Add(tempvalue);
                    inputstring += this.Columns[tempvalue].DataType.ToString() + " " + id + "_" + tempvalue;
                    mainstring = mainstring.Replace("[" + tempvalue + "]", id + "_" + tempvalue);
                }
            }
            string returntype = "double";
            switch(this.comboBoxEdit1.SelectedIndex)
            {
                case 0:returntype = "double";break;
                case 1:returntype = "string";break;
                case 2:returntype = "DateTime";break;
                case 3:returntype = "bool";break;
            }
            return string.Format(baseString, returntype, inputstring, mainstring, returnstring);
        }
        private void compileString(string target)
        {
            CSharpCodeProvider objCSharpCodePrivoder = new CSharpCodeProvider();
            CompilerParameters objCompilerParameters = new CompilerParameters();
            objCompilerParameters.ReferencedAssemblies.Add("System.dll");
            objCompilerParameters.GenerateExecutable = false;
            objCompilerParameters.GenerateInMemory = true;
            CompilerResults cr = objCSharpCodePrivoder.CompileAssemblyFromSource(objCompilerParameters, this.memoEdit1.Text);
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
            else
            {
                // 通过反射，调用HelloWorld的实例
                Assembly objAssembly = cr.CompiledAssembly;
                object objHelloWorld = objAssembly.CreateInstance("DynamicCodeGenerate.Method");
                MethodInfo objMI = objHelloWorld.GetType().GetMethod("OutPut");
                Console.WriteLine(objMI.Invoke(objHelloWorld, null));
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {

        }
    }
}
