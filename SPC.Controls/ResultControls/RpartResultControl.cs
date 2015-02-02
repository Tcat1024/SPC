using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SPC.Controls.ResultControls
{

    public partial class RpartResultControl : DevExpress.XtraEditors.XtraUserControl
    {
        test aa = new test();
        GlobalMouseHandler handler = new GlobalMouseHandler();
        enum ResizeMode
        {
            H, V, B, N
        }
        ResizeMode resizeMode = ResizeMode.N;
        public RpartResultControl()
        {
            InitializeComponent();
            handler.MouseMoveGlobal += handler_MouseMoveGlobal;
            handler.MouseLeftDownGlobal += handler_MouseLeftDownGlobal;
            handler.MouseLeftUpGlobal += handler_MouseLeftUpGlobal;
        }
        void handler_MouseLeftUpGlobal(object sender, MouseEventArgGlobal e)
        {
            if (!this.ContainsFocus)
                return;
            this.resizeMode = ResizeMode.N;
        }

        void handler_MouseLeftDownGlobal(object sender, MouseEventArgGlobal e)
        {
            if (!this.ContainsFocus)
                return;
            if (this.Cursor == Cursors.SizeWE)
                this.resizeMode = ResizeMode.H;
            else if (this.Cursor == Cursors.SizeNS)
                this.resizeMode = ResizeMode.V;
            else if (this.Cursor == Cursors.SizeNWSE)
                this.resizeMode = ResizeMode.B;
            else
                this.resizeMode = ResizeMode.N;
        }

        void handler_MouseMoveGlobal(object sender, MouseEventArgGlobal e)
        {
            if (!this.ContainsFocus)
                return;
            var point = this.ChildPanelControl.PointToClient(MousePosition);
            switch (this.resizeMode)
            {
                case ResizeMode.N:
                    int i = 0;
                    var cur = Cursors.Arrow;
                    if (Math.Abs(point.X - this.ChildPanelControl.Width) < 5 && point.Y < this.ChildPanelControl.Height+5&&point.Y>0)
                    {
                        cur = Cursors.SizeWE;
                        i++;
                    }
                    if (Math.Abs(point.Y - this.ChildPanelControl.Height) < 5&&point.X<this.ChildPanelControl.Width+5&&point.X>0)
                    {
                        cur = Cursors.SizeNS;
                        i++;
                    }
                    if (i == 2)
                        cur = Cursors.SizeNWSE;
                    this.Cursor = cur;
                    break;
                case ResizeMode.H:
                    int newWidth = point.X;
                    this.ChildPanelControl.Width = newWidth > 10 ? newWidth : 10;
                    break;
                case ResizeMode.V:
                    int newHeight = point.Y;
                    this.ChildPanelControl.Height = newHeight > 10 ? newHeight : 10;
                    break;
                case ResizeMode.B:
                    newWidth = point.X;
                    this.ChildPanelControl.Width = newWidth > 10 ? newWidth : 10;
                    newHeight = point.Y;
                    this.ChildPanelControl.Height = newHeight > 10 ? newHeight : 10;
                    break;
            }
        }
        public void Init(Image pic,string input, double[,] cpdata)
        {
            this.pictureEdit1.Image = pic;
            try
            {
                analyzeString(input);
                this.treeList1.ExpandAll();
            }
            catch(Exception ex)
            {
                MessageBox.Show("输入决策树字符串格式错误：" + ex.Message);
            }
            DataTable cptable = new DataTable();
            cptable.Columns.Add("CP");
            cptable.Columns.Add("nsplit");
            cptable.Columns.Add("rel_error");
            cptable.Columns.Add("xerror");
            cptable.Columns.Add("xstd");
            if (cpdata != null)
            {
                int i,j;
                int count = cpdata.GetLength(0);
                int length = cpdata.GetLength(1);
                DataRow temprow;
                for (i = 0; i < count; i++)
                {
                    temprow = cptable.NewRow();
                    for (j = 0; j < length; j++)
                        temprow[j] = cpdata[i, j];
                    cptable.Rows.Add(temprow);
                }
            }
            this.gridControl1.DataSource = cptable;
        }

        private void pictureEdit1_Enter(object sender, EventArgs e)
        {
            this.ChildPanelControl.Focus();
        }

        private void ContourPlotResultControl_Validated(object sender, EventArgs e)
        {
            Application.RemoveMessageFilter(handler);
        }

        private void ContourPlotResultControl_Enter(object sender, EventArgs e)
        {
            Application.RemoveMessageFilter(handler);
            Application.AddMessageFilter(handler);
        }
        private void analyzeString(string input)
        {
            var rows = input.Split('\n');
            string[] objs;
            int i = -1;
            int length = rows.Length;
            char end;
            while ((++i) < length && (rows[i].TrimStart().Length<=0||rows[i].TrimStart()[0] != '1')) ;
            if (i >= length)
                return;
            string row = rows[i];
            int j = 0;
            int rowlength = row.Length;
            DevExpress.XtraTreeList.Nodes.TreeListNode tempnode;
            while (j < rowlength && row[j++] != ')') ;
            if (j >= rowlength)
                return;
            objs = row.TrimStart().TrimEnd().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            end = objs[1][objs[1].Length - 1];
            if (end=='='||end=='<'||end=='>')
                tempnode = this.treeList1.Nodes.Add(objs[0].TrimEnd(')'), objs[1]+objs[2], objs[3], objs[4], objs[5]);
            else
                tempnode = this.treeList1.Nodes.Add(objs[0].TrimEnd(')'), objs[1],objs[2], objs[3], objs[4]);
            tempnode.Tag = j;
            i++;
            while (i < length)
            {
                row = rows[i];
                rowlength = row.Length;
                j = 0;
                while (j < rowlength && row[j++] != ')') ;
                if (j >= rowlength)
                    return;
                while (Convert.ToInt32(tempnode.Tag) >= j)
                {
                    tempnode = tempnode.ParentNode;
                }
                objs = row.TrimStart().TrimEnd().Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
                end = objs[1][objs[1].Length - 1];
                if (end == '=' || end == '<' || end == '>')
                    tempnode = tempnode.Nodes.Add(objs[0].TrimEnd(')'), objs[1] + objs[2], objs[3], objs[4], objs[5]);
                else
                    tempnode = tempnode.Nodes.Add(objs[0].TrimEnd(')'), objs[1], objs[2], objs[3], objs[4]);
                tempnode.Tag = j;
                i++;
            }
        }
    }
    public class test
    {
        ~test()
        {
            MessageBox.Show("Released");
        }
    }

}
