using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EAS.Services;
using System.Text.RegularExpressions;

namespace QtDataTrace.UI.CPKtool
{
    public partial class DebugDataSelectForm : Form
    {
        public string conStr = "";
        public string tablename = "";
        public string command = "";
        private List<string> tableNames = new List<string>();
        private List<string>[] columnNames;
        public DebugDataSelectForm()
        {
            InitializeComponent();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            if (this.listBoxControl1.Items != null)
            {
                this.listBoxControl2.Items.Clear();
                foreach (var item in this.listBoxControl1.Items)
                    this.listBoxControl2.Items.Add(item);
            }
            this.check();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (this.listBoxControl1.SelectedItems != null)
            {
                foreach (object item in listBoxControl1.SelectedItems)
                {
                    if (!listBoxControl2.Items.Contains(item))
                        this.listBoxControl2.Items.Add(item);
                }
            }
            this.check();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (this.listBoxControl2.SelectedItems != null)
            {
                for (int i = this.listBoxControl2.SelectedItems.Count - 1; i > -1; i--)
                    this.listBoxControl2.Items.Remove(this.listBoxControl2.SelectedItems[i]);
            }
            this.check();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            this.listBoxControl2.Items.Clear();
            this.check();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.textEdit1.Text != null && this.textEdit1.Text != "")
            {
                if (selectTable(this.textEdit1.Text))
                {
                    this.comboBoxEdit1.Properties.Items.Clear();
                    this.comboBoxEdit1.Properties.Items.AddRange(tableNames);
                    this.comboBoxEdit1.Enabled = true;
                    this.conStr = this.textEdit1.Text;
                    if (this.textEdit1.Properties.Items.IndexOf(conStr) == -1)
                    {
                        this.textEdit1.Properties.Items.Add(conStr);
                    }
                }   
            }
            this.check();
        }
        private bool selectTable(string con)
        {
            var temp = ServiceContainer.GetService<SPC.Web.Interface.ICPKtool>().GetTableNames(this.textEdit1.Text);
            if (temp.ex == "")
            {
                tableNames = temp.data;
                columnNames = new List<string>[tableNames.Count];
                return true;
            }
            else
            {
                MessageBox.Show(temp.ex);
                return false;
            }
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = comboBoxEdit1.SelectedIndex;
            if (comboBoxEdit1.Text != null && comboBoxEdit1.Text != "")
            {
                if (columnNames[index] == null)
                {
                    if (needColumns(index))
                    {
                        this.tablename = tableNames[index];
                    }
                    else
                        return;
                }
                else
                    this.tablename = tableNames[index];
                this.listBoxControl2.Items.Clear();
                this.listBoxControl1.Items.Clear();
                this.listBoxControl1.Items.AddRange(columnNames[index].ToArray());      
            }
            this.check();
        }
        private bool needColumns(int index)
        {
            columnNames[index] = new List<string>();
            var temp = ServiceContainer.GetService<SPC.Web.Interface.ICPKtool>().GetColumnNames(this.conStr,this.tableNames[index]);
            if (temp.ex == "")
            {
                columnNames[index] = temp.data;
                return true;
            }
            else
            {
                MessageBox.Show(temp.ex);
                return false;
            } 
            
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            if (!this.panelControl1.Visible)
            {
                this.command = "select ";
                string t = listBoxControl2.Items[0].ToString();
                if (Regex.IsMatch(t, @"\b\S*(PK)\b"))
                    t = t.Replace("(PK)", "");
                this.command += t;
                for (int i = 1; i < listBoxControl2.Items.Count; i++)
                {
                    string temp = listBoxControl2.Items[i].ToString();
                    if (Regex.IsMatch(temp, @"\b\S*(PK)\b"))
                        temp = temp.Replace("(PK)", "");
                    this.command += "," + temp;
                }
                this.command += " from " + tablename;
                this.command += " "+this.memoEdit1.Text;
                if (Convert.ToInt32(this.textEdit2.Text)>0)
                this.command += " where rownum<"+this.textEdit2.Text;
            }
            else
                this.command = this.memoEdit1.Text.TrimStart().TrimEnd();
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.check();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            this.panelControl1.Visible = !this.panelControl1.Visible;
            this.check();
        }
        private void simpleButton8_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }
        private void check()
        {
            if ((this.panelControl1.Visible && this.memoEdit1.Text.Trim() != "") || (!this.panelControl1.Visible && this.listBoxControl2.Items.Count > 0))
                this.simpleButton7.Enabled = true;
            else
                this.simpleButton7.Enabled = false;
        }

        private void memoEdit1_TextChanged(object sender, EventArgs e)
        {
            check();
        }
    }
}
