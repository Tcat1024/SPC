using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SPC.Base.Control
{
    public partial class CustomGroupStringBuilder : DevExpress.XtraEditors.XtraUserControl
    {
        public enum GroupBuildType
        {
            L,LE,LLE
        }
        private GroupBuildType _BuildType = GroupBuildType.LLE;
        public GroupBuildType BuildType
        {
            get
            {
                return this._BuildType;
            }
            set
            {
                if (this._BuildType != value)
                {
                    this._BuildType = value;
                    switch(value)
                    {
                        case GroupBuildType.L: 
                            this.repositoryItemComboBox1.Items.Clear();
                            this.repositoryItemComboBox1.Items.Add("<");
                            break;
                        case GroupBuildType.LE:
                            this.repositoryItemComboBox1.Items.Clear();
                            this.repositoryItemComboBox1.Items.Add("<=");
                            break;
                        case GroupBuildType.LLE:
                            this.repositoryItemComboBox1.Items.Clear();
                            this.repositoryItemComboBox1.Items.Add("<");
                            this.repositoryItemComboBox1.Items.Add("<=");
                            break;
                    }
                }
            }
        }
        public CustomGroupStringBuilder()
        {
            InitializeComponent();
        }
        private void repositoryItemButtonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (this.treeList1.Nodes.Count > 1)
            {
                this.treeList1.Nodes.Remove(this.treeList1.FocusedNode);
            }
            else
            {
                this.treeList1.FocusedNode[0] = "";
                this.treeList1.FocusedNode[1] = "";
            }
        }
        public event EventHandler<GroupStringDeterminedEventArgs> GroupStringDetermined;
        public event EventHandler GroupStringCanceled;
        private void repositoryItemComboBox1_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if ((e.OldValue == null || e.OldValue.ToString().Trim() == "") && e.NewValue != null && e.NewValue.ToString().Trim() != "")
                newNode = true;
            newValue = e.NewValue;
        }
        private bool newNode = false;
        private object newValue = null;
        private void repositoryItemComboBox1_EditValueChanged(object sender, EventArgs e)
        {
            if (newNode)
            {
                var focused = this.treeList1.FocusedNode;
                focused[0] = newValue;
                this.treeList1.Nodes.Add("", "");
                this.treeList1.FocusedNode = focused;
                newNode = false;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string result = "";
            foreach (DevExpress.XtraTreeList.Nodes.TreeListNode node in this.treeList1.Nodes)
            {
                if (node[0] != null && node[1] != null && node[0].ToString().Trim() != "" && node[1].ToString().Trim() != "")
                {
                    result += node[0].ToString() + node[1].ToString();
                }
            }
            GroupStringDetermined(simpleButton1, new GroupStringDeterminedEventArgs(result));
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            GroupStringCanceled(simpleButton2,new EventArgs());
        }
        public class GroupStringDeterminedEventArgs: EventArgs
        {
            public string result = "";
            public GroupStringDeterminedEventArgs(string e)
            {
                result = e;
            }
        }
    }
}
