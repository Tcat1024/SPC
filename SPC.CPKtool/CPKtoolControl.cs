using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using SPC.Base.Operation;

namespace SPC.CPKtool
{
    public partial class CPKtoolControl : DevExpress.XtraEditors.XtraUserControl
    {
        DataTable Data;
        CPKCanChooseTableData Result = new CPKCanChooseTableData();
        BindingSource ResultBind = new BindingSource();
        BindingSource DataBind = new BindingSource();
        DevExpress.XtraCharts.CrosshairElement currentElement;
        int MemberGroupCount = 0;
        public string ChooseColumnName = "choose";
        private object _DataSource;
        [Category("Data")]
        [AttributeProvider(typeof(IListSource))]
        [DefaultValue("")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public object DataSource
        {
            get
            {
                return this._DataSource;
            }
            set
            {
                this._DataSource = value;
                if (this._DataSource is DataTable)
                    DataInit(this._DataSource as DataTable);
                else if (this._DataSource is DataSet && this._DataMember != null && this._DataMember.Trim() != "")
                    DataInit((this._DataSource as DataSet).Tables[this._DataMember]);
            }
        }
        private string _DataMember;
        [Category("Data")]
        [DefaultValue("")]
        [Editor("System.Windows.Forms.Design.DataMemberListEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string DataMember
        {
            get
            {
                return this._DataMember;
            }
            set
            {
                this._DataMember = value;
                if (this._DataSource is DataSet && this._DataMember != null && this._DataMember.Trim() != "")
                    DataInit((this._DataSource as DataSet).Tables[this._DataMember]);
            }
        }
        public CPKtoolControl()
        {
            InitializeComponent();
            this.Result.AllowAutoRefresh = true;
            this.repositoryItemRadioGroup1.Items[0].Value = CPKType.Both;
            this.repositoryItemRadioGroup1.Items[1].Value = CPKType.Up;
            this.repositoryItemRadioGroup1.Items[2].Value = CPKType.Low;
            this.repositoryItemComboBox3.Items.AddRange(new STDEVType[] { STDEVType.Std, STDEVType.Range });
            this.barEditItem4.EditValue = STDEVType.Std;
            this.barEditItem2.EditValue = 0;
            this.Result.DataInitComplete += this.NewDataRefresh;
            this.Result.ParamChanged += (sender,e) => { this.DrawData(); this.InitSD(); };
            this.Result.ToleInitComplete += this.NewToleRefresh;
            this.gridControl2.DataSource = this.DataBind;
            ResultBind.DataSource = this.Result;
            this.gridControl1.DataSource = this.ResultBind;
        }
        private void CPKtoolControl_Load(object sender, EventArgs e)
        {
            chartSizeInit();
        }
        private void GetProperties()
        {
            this.repositoryItemComboBox2.Items.Clear();
            for (int i = 0; i < this.Data.Columns.Count;i++ )
            {
                if (this.Data.Columns[i].DataType != typeof(string) && this.Data.Columns[i].ColumnName != ChooseColumnName)
                    this.repositoryItemComboBox2.Items.Add(this.Data.Columns[i].Caption);
            }
        }
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if(this.Result.SelectType == CPKType.Both&&this.Result.UpTole<this.Result.LowTole)
            {
                MessageBox.Show("上公差必须大于下公差！");
                return;
            }
            if (this.Result.Data != null&&this.Result.Param!=null)
            {
                try
                {
                    RefreshResult();
                    this.Result.GetPPK();
                    this.Result.GetBdRate();
                    this.Result.GetCPK();
                    this.Result.GetGroupBdRate();
                    this.layoutView1.Refresh();
                    this.DrawSD();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("请选择数据");
            }
        }
        private void barEditItem1_EditValueChanged(object sender, EventArgs e)
        {
            if (this.barEditItem1.EditValue != null)
            {
                repositoryItemComboBox2.Appearance.ForeColor = Color.Black;
                this.Result.Param = this.barEditItem1.EditValue.ToString();
                //this.gridView1.GroupSummary.Clear();
                //this.gridView1.GroupSummary.AddRange(this.makeGroupSummaryItems(this.Result.Param));
            }
        }
        private void InitConstantLines()
        {
            var xstrip = (DataChart.Diagram as DevExpress.XtraCharts.SwiftPlotDiagram).AxisX.Strips[0];
            for (int i = 0; i < 2; i++)
            {
                var temp = (DataChart.Diagram as DevExpress.XtraCharts.SwiftPlotDiagram).AxisX.ConstantLines[i];
                if (temp.Visible && (this.Result[Convert.ToInt32(temp.AxisValue)] == null || temp.AxisValue.ToString() == DataChart.Series[0].Points[0].Argument || temp.AxisValue.ToString() == DataChart.Series[0].Points[DataChart.Series[0].Points.Count - 1].Argument))
                {
                    temp.Visible = false;
                    xstrip.Visible = false;
                }
            }
            var ystrip = (DataChart.Diagram as DevExpress.XtraCharts.SwiftPlotDiagram).AxisY.Strips[0];
            for (int i = 3; i < 5; i++)
            {
                var temp = (DataChart.Diagram as DevExpress.XtraCharts.SwiftPlotDiagram).AxisY.ConstantLines[i];
                if (temp.Visible && (Convert.ToDouble(temp.AxisValue) <= Convert.ToDouble((DataChart.Diagram as DevExpress.XtraCharts.SwiftPlotDiagram).AxisY.WholeRange.MinValue) || Convert.ToDouble(temp.AxisValue) >= Convert.ToDouble((DataChart.Diagram as DevExpress.XtraCharts.SwiftPlotDiagram).AxisY.WholeRange.MaxValue)))
                {
                    temp.Visible = false;
                    ystrip.Visible = false;
                }
            }          
        }
        private void InitSD()
        {
            for (int i = 0; i < 3;i++ )
                (this.DataChart.Diagram as DevExpress.XtraCharts.SwiftPlotDiagram).AxisY.ConstantLines[i].Visible = false;
            for (int i = 0; i < 6;i++ )
                (this.chartControl2.Diagram as DevExpress.XtraCharts.XYDiagram).AxisX.ConstantLines[i].Visible = false;
        }
        private void NewDataRefresh()
        {
            layoutView1.Refresh();
        }
        private void DrawData()
        {
            DrawSeries(0);
        }
        private void DrawSeries(int index)
        {
            if (this.MemberGroupCount < 1)
                this.MemberGroupCount = (int)Math.Pow(Result.Count, 0.5);
            double min = this.Result.Min;
            double max = this.Result.Max;
            double length = (max - min) / MemberGroupCount;
            int[] y = new int[MemberGroupCount];
            if (length == 0)
                length = 1;
            (this.chartControl2.Series[index].View as DevExpress.XtraCharts.BarSeriesView).BarWidth = length;
            this.DataChart.Series[index].Points.BeginUpdate();
            this.DataChart.Series[index].Points.Clear();
            for (int i = this.Result.start, c = 0; c < this.Result.Count; i++)
            {
                double? temp = this.Result[i];
                if (temp != null)
                {
                    this.DataChart.Series[index].Points.Add(new DevExpress.XtraCharts.SeriesPoint(i, temp));
                    int k = (int)(((double)temp - min) / length);
                    if (k >= MemberGroupCount)
                        k = MemberGroupCount - 1;
                    y[k]++;
                    c++;
                }
            }
            this.DataChart.Series[index].Points.EndUpdate();
            this.chartControl2.Series[index].Points.BeginUpdate();
            this.chartControl2.Series[index].Points.Clear();
            for (int i = 0; i < MemberGroupCount; i++)
                 this.chartControl2.Series[index].Points.Add(new DevExpress.XtraCharts.SeriesPoint(i * length + min + length / 2, y[i]));
            this.chartControl2.Series[index].Points.EndUpdate();
            (this.DataChart.Diagram as DevExpress.XtraCharts.SwiftPlotDiagram).AxisX.VisualRange.Auto = true;
            InitConstantLines();
            this.chartControl2.Series[1].Points.Clear();
            this.chartControl2.Series[2].Points.Clear();
        }
        private void DrawSD()
        {
            double avg = Result.Avg;
            double stde = Result.STDev_E;
            double stdg = Result.STDev_G;
            double count = Result.Count;
            double x1p,x1n,x2p, x2n,y1,y2;

            (this.chartControl2.Diagram as DevExpress.XtraCharts.XYDiagram).AxisX.ConstantLines[0].Visible = true;
            (this.chartControl2.Diagram as DevExpress.XtraCharts.XYDiagram).AxisX.ConstantLines[0].AxisValue = this.Result.N3Sigma;
            (this.chartControl2.Diagram as DevExpress.XtraCharts.XYDiagram).AxisX.ConstantLines[1].Visible = true;
            (this.chartControl2.Diagram as DevExpress.XtraCharts.XYDiagram).AxisX.ConstantLines[1].AxisValue = this.Result.Avg;
            (this.chartControl2.Diagram as DevExpress.XtraCharts.XYDiagram).AxisX.ConstantLines[2].Visible = true;
            (this.chartControl2.Diagram as DevExpress.XtraCharts.XYDiagram).AxisX.ConstantLines[2].AxisValue = this.Result.P3Sigma;
            this.chartControl2.Series[1].Points.BeginUpdate();
            this.chartControl2.Series[2].Points.BeginUpdate();
            this.chartControl2.Series[1].Points.Clear();
            this.chartControl2.Series[2].Points.Clear();
            for(int i=0;;i++)
            {
                x1p = avg + stde / 10 * i;
                x1n = avg - stde / 10 * i;
                x2p = avg + stdg / 10 * i;
                x2n = avg - stdg / 10 * i;
                y1 = NormalDistribution.GetY(x1p, avg, stde)*stde;
                y2 = NormalDistribution.GetY(x2p, avg, stdg)*stdg;
                this.chartControl2.Series[1].Points.Add(new DevExpress.XtraCharts.SeriesPoint(x1p,y1));
                this.chartControl2.Series[1].Points.Add(new DevExpress.XtraCharts.SeriesPoint(x1n, y1));
                this.chartControl2.Series[2].Points.Add(new DevExpress.XtraCharts.SeriesPoint(x2p, y2));
                this.chartControl2.Series[2].Points.Add(new DevExpress.XtraCharts.SeriesPoint(x2n, y2));
                if (y1 < 0.001)
                    break;
            }
            this.chartControl2.Series[1].Points.EndUpdate();
            this.chartControl2.Series[2].Points.EndUpdate();
            (this.chartControl2.Diagram as DevExpress.XtraCharts.XYDiagram).AxisY.WholeRange.MinValue = 0;
            (this.chartControl2.Diagram as DevExpress.XtraCharts.XYDiagram).AxisY.VisualRange.MinValue = 0;
            (this.chartControl2.Diagram as DevExpress.XtraCharts.XYDiagram).SecondaryAxesY[0].WholeRange.MinValue = 0;
            (this.chartControl2.Diagram as DevExpress.XtraCharts.XYDiagram).SecondaryAxesY[0].VisualRange.MinValue = 0;
        }
        private void NewToleRefresh()
        {
            layoutView1.Refresh();
            (this.DataChart.Diagram as DevExpress.XtraCharts.SwiftPlotDiagram).AxisY.ConstantLines[0].Visible = true;
            (this.DataChart.Diagram as DevExpress.XtraCharts.SwiftPlotDiagram).AxisY.ConstantLines[0].AxisValue = this.Result.UpTole + this.Result.Standard;
            (this.DataChart.Diagram as DevExpress.XtraCharts.SwiftPlotDiagram).AxisY.ConstantLines[1].Visible = true;
            (this.DataChart.Diagram as DevExpress.XtraCharts.SwiftPlotDiagram).AxisY.ConstantLines[1].AxisValue = this.Result.Standard;
            (this.DataChart.Diagram as DevExpress.XtraCharts.SwiftPlotDiagram).AxisY.ConstantLines[2].Visible = true;
            (this.DataChart.Diagram as DevExpress.XtraCharts.SwiftPlotDiagram).AxisY.ConstantLines[2].AxisValue = this.Result.LowTole + this.Result.Standard;
            (this.chartControl2.Diagram as DevExpress.XtraCharts.XYDiagram).AxisX.ConstantLines[3].Visible = true;
            (this.chartControl2.Diagram as DevExpress.XtraCharts.XYDiagram).AxisX.ConstantLines[3].AxisValue = this.Result.UpTole + this.Result.Standard;
            (this.chartControl2.Diagram as DevExpress.XtraCharts.XYDiagram).AxisX.ConstantLines[4].Visible = true;
            (this.chartControl2.Diagram as DevExpress.XtraCharts.XYDiagram).AxisX.ConstantLines[4].AxisValue = this.Result.Standard;
            (this.chartControl2.Diagram as DevExpress.XtraCharts.XYDiagram).AxisX.ConstantLines[5].Visible = true;
            (this.chartControl2.Diagram as DevExpress.XtraCharts.XYDiagram).AxisX.ConstantLines[5].AxisValue = this.Result.LowTole + this.Result.Standard;
        }
        private void barEditItem2_EditValueChanged(object sender, EventArgs e)
        {
            if(barEditItem2.EditValue!=null)
            {
                this.MemberGroupCount = Convert.ToInt32(barEditItem2.EditValue);
            }
            if (this.Data != null && this.barEditItem1.EditValue != null)
                NewDataRefresh();
        }
        private void barEditItem4_EditValueChanged(object sender, EventArgs e)
        {
            this.Result.StdevType = (STDEVType)this.barEditItem4.EditValue;
        }
        private void repositoryItemRadioGroup1_EditValueChanged(object sender, EventArgs e)
        {
            this.Result.SelectType = (CPKType)(this.layoutView1.ActiveEditor as DevExpress.XtraEditors.RadioGroup).EditValue;
        }
        private DevExpress.XtraGrid.GridSummaryItem[] makeGroupSummaryItems(string fieldname)
        {
            DevExpress.XtraGrid.GridSummaryItem[] result = new DevExpress.XtraGrid.GridSummaryItem[3];
            result[0] = new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Custom, fieldname, null, "Avg:{0}");
            result[1] = new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Custom, fieldname, null, "Max:{0}");
            result[2] = new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Custom, fieldname, null, "Min:{0}");
             return result;
        }
        private void chartControl1_CustomDrawCrosshair(object sender, DevExpress.XtraCharts.CustomDrawCrosshairEventArgs e)
        {
            if (e.CrosshairElements.Count()!=0)
                this.currentElement = e.CrosshairElements.First();
        }
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var s = e.Item.Description;
            if (s != null && s.Trim() != "")
            {
                try
                {
                    int index = int.Parse(s);
                    DeleteSinglePointFromData(index);
                    RefreshResult();
                    DrawData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }   
        }
        private void gridView1_DragObjectDrop(object sender, DevExpress.XtraGrid.Views.Base.DragObjectDropEventArgs e)
        {
            if (this.DataChart.CalcHitInfo(this.DataChart.PointToClient(MousePosition)).InDiagram)
            {
                var col = (e.DragObject as DevExpress.XtraGrid.Columns.GridColumn);
                if (this.repositoryItemComboBox2.Items.Contains(col.FieldName))
                    this.barEditItem1.EditValue = col.FieldName;
            }
        }
        private void DataInit(DataTable source)
        {
            this.Data = source;
            RefreshData();
        }
        private void RefreshData()
        {
            if (this.Data == null)
            {
                MessageBox.Show("数据集为空");
                return;
            }
            if (!this.Data.Columns.Contains(ChooseColumnName))
            {
                var choosecolumn = new DataColumn(ChooseColumnName, typeof(bool));
                this.Data.Columns.Add(choosecolumn);
            }
            int co = this.Data.Rows.Count;
            for (int i = 0; i < co; i++)
            {
                this.Data.Rows[i][ChooseColumnName] = true;
            }
            this.GetProperties();
            Result.Data = this.Data;
            this.gridView1.Columns.Clear();
            this.DataBind.DataSource = this.Data;
        }
        private DevExpress.XtraCharts.ConstantLine _targetLine = null;
        private DevExpress.XtraCharts.ConstantLine targetLine
        {
            get
            {
                return this._targetLine;
            }
            set
            {
                if (value == null)
                    DataChart.Cursor = Cursors.Arrow;
                else if (value.Name == "X")
                    DataChart.Cursor = Cursors.SizeWE;
                else if (value.Name == "Y")
                    DataChart.Cursor = Cursors.SizeNS;
                this._targetLine = value;
            }
        }
        private void chartControl1_MouseDown(object sender, MouseEventArgs e)
        {
            var chart = sender as DevExpress.XtraCharts.ChartControl;
            if (chart.Series[0].Points.Count > 0)
            {
                var info = chart.CalcHitInfo(e.Location);
                if (info.Diagram != null)
                {
                    string x = this.currentElement.SeriesPoint.Argument;
                    var left = (chart.Diagram as DevExpress.XtraCharts.SwiftPlotDiagram).AxisX.ConstantLines[0];
                    var right = (chart.Diagram as DevExpress.XtraCharts.SwiftPlotDiagram).AxisX.ConstantLines[1];
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        if(info.InConstantLine&&info.ConstantLine.Title.Visible)
                        {
                            this.targetLine = info.ConstantLine;
                        }
                        else if (!left.Visible && !right.Visible)
                        {
                            left.AxisValue = x;
                            left.Title.Text = left.AxisValue.ToString();
                            this.targetLine = left;
                            left.Visible = true;
                        }
                        else if (!right.Visible||!left.Visible)
                        {
                            var temp = right.Visible ? left : right;
                            temp.AxisValue = x;
                            temp.Title.Text = temp.AxisValue.ToString();
                            this.targetLine = temp;
                            temp.Visible = true;
                            RefreshXStrip();
                        }
                    }
                    else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if(info.InConstantLine&&info.ConstantLine.Visible&&info.ConstantLine.Title.Visible)
                        {
                            ConfigPopMenu(ChartBoundRightClickPopupMenu, x, this.currentElement.SeriesPoint.Values[0], info.ConstantLine, info.ConstantLine.Name+"轴边界");
                            this.ChartBoundRightClickPopupMenu.ShowPopup(MousePosition);
                        }
                        else
                        {
                            this.barStaticItem1.Caption = String.Format("X:{0:0.00} Y:{1:0.0000}", x, this.currentElement.SeriesPoint.Values[0]);
                            this.barButtonItem2.Description = x;
                            this.chartPointRightClickPopupMenu.ShowPopup(MousePosition);
                        }
                    }
                }
                else
                {
                    var yInfo = (chart.Diagram as DevExpress.XtraCharts.SwiftPlotDiagram).PointToDiagram(new Point(35, e.Y));
                    if(yInfo.AxisY!=null)
                    {
                        double y = yInfo.NumericalValue;
                        var down = (chart.Diagram as DevExpress.XtraCharts.SwiftPlotDiagram).AxisY.ConstantLines[3];
                        var up = (chart.Diagram as DevExpress.XtraCharts.SwiftPlotDiagram).AxisY.ConstantLines[4];
                        if (e.Button == System.Windows.Forms.MouseButtons.Left)
                        {
                            if (!down.Visible && !up.Visible)
                            {
                                down.AxisValue = y;
                                down.Title.Text = down.AxisValue.ToString();
                                this.targetLine = down;
                                down.Visible = true;
                            }
                            else if (!up.Visible || !down.Visible)
                            {
                                var temp = up.Visible ? down : up;
                                temp.AxisValue = y;
                                temp.Title.Text = temp.AxisValue.ToString();
                                this.targetLine = temp;
                                temp.Visible = true;
                                RefreshYStrip();
                            }
                        }
                    }
                }
            }
        }
        private void chartControl1_MouseUp(object sender, MouseEventArgs e)
        {
            var chart = sender as DevExpress.XtraCharts.ChartControl;
            if (e.Button == System.Windows.Forms.MouseButtons.Left && this.targetLine != null)
            {
                var info = (chart.Diagram as DevExpress.XtraCharts.SwiftPlotDiagram).PointToDiagram(e.Location);
                if (targetLine.Name == "X" && !info.IsEmpty && targetLine.AxisValue.ToString() != this.currentElement.SeriesPoint.Argument)
                {
                    targetLine.AxisValue = this.currentElement.SeriesPoint.Argument;
                    targetLine.Title.Text = targetLine.AxisValue.ToString();
                    RefreshXStrip();
                }
                else if(targetLine.Name=="Y"&&!info.IsEmpty&&Convert.ToDouble(targetLine.AxisValue) != info.NumericalValue)
                {
                    targetLine.AxisValue = info.NumericalValue;
                    targetLine.Title.Text = targetLine.AxisValue.ToString();
                    RefreshYStrip();
                }
            }
            this.targetLine = null;
        }
        private void chartControl1_MouseMove(object sender, MouseEventArgs e)
        {
            var chart = sender as DevExpress.XtraCharts.ChartControl;
            if(this.targetLine==null)
                DataChart.Cursor = Cursors.Arrow;
            if (chart.Series[0].Points.Count > 0)
            {
                var info = DataChart.CalcHitInfo(e.Location);
                if (info.ConstantLine != null&&info.ConstantLine.Title.Visible&&info.ConstantLine.Name=="X")
                {
                    DataChart.Cursor = Cursors.SizeWE;
                }
                else if(info.ConstantLine != null&&info.ConstantLine.Title.Visible&&info.ConstantLine.Name=="Y")
                {
                    DataChart.Cursor = Cursors.SizeNS;
                }
            }
        }
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            (barButtonItem6.Tag as DevExpress.XtraCharts.ConstantLine).Visible = false;
            RefreshXStrip();
            RefreshYStrip();
            
        }
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel 工作簿(*.xlsx)|*.xlsx|Excel 97-2003 工作簿(*.xls)|*.xls";
                openFileDialog.FilterIndex = 0;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = "导入文件";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string path = openFileDialog.FileName;
                    string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=yes'";
                    //  Excel 2007
                    if (path.ToLower().IndexOf(".xlsx") > 0 && path.ToLower().EndsWith("xlsx"))
                    {
                        //strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + physicalPath + "';Extended Properties='Excel 12.0;HDR=YES'";
                        //strConn = "'Microsoft.ACE.OLEDB.12.0','Data Source=" + physicalPath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"'";
                        strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=yes'";
                    }
                    //  Excel 2003
                    if (path.ToLower().IndexOf(".xls") > 0 && path.ToLower().EndsWith("xls"))
                    {
                        //strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + physicalPath + "';Extended Properties='Excel 8.0;HDR=YES;'";
                        strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + path + "';Extended Properties=Excel 8.0";
                    }
                    OleDbConnection conn = new OleDbConnection(strConn);
                    try
                    {
                        if (conn.State.ToString() == "Closed")
                        {
                            conn.Open();
                        }
                        DataSet temp = new DataSet();
                        string strSql = "SELECT * FROM  [Sheet1$]";
                        OleDbDataAdapter adapter = new OleDbDataAdapter(strSql, conn);
                        adapter.Fill(temp, "[Sheet1$]");
                        conn.Close();
                        this.DataInit(temp.Tables[0]);
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show(ee.Message);
                    }

                }
            }
        }
        private void DeleteSinglePointFromData(int index)
        {
            this.Data.Rows[index][ChooseColumnName] = false;
        }
        private void barEditItem3_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                var t = ((sender as DevExpress.XtraBars.BarEditItem).Tag as DevExpress.XtraCharts.ConstantLine);
                string s = (sender as DevExpress.XtraBars.BarEditItem).EditValue.ToString();
                if(t.Name=="X"&&this.Result[int.Parse(s)]!=null)
                {
                    t.AxisValue = s;
                    t.Title.Text = t.AxisValue.ToString();
                    RefreshXStrip();
                }
                else if(t.Name=="Y")
                {
                    t.AxisValue = (sender as DevExpress.XtraBars.BarEditItem).EditValue;
                    t.Title.Text = t.AxisValue.ToString();
                    RefreshYStrip();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void RefreshXStrip()
        {
            var xstrip = (DataChart.Diagram as DevExpress.XtraCharts.SwiftPlotDiagram).AxisX.Strips[0];
            var left = (DataChart.Diagram as DevExpress.XtraCharts.SwiftPlotDiagram).AxisX.ConstantLines[0];
            var right = (DataChart.Diagram as DevExpress.XtraCharts.SwiftPlotDiagram).AxisX.ConstantLines[1];
            if (left.Visible && right.Visible)
            {
                if (int.Parse(left.AxisValue.ToString()) < int.Parse(right.AxisValue.ToString()))
                {
                    xstrip.MaxLimit.AxisValue = right.AxisValue;
                    xstrip.MinLimit.AxisValue = left.AxisValue;
                }
                else
                {
                    xstrip.MaxLimit.AxisValue = left.AxisValue;
                    xstrip.MinLimit.AxisValue = right.AxisValue;
                }
                xstrip.Visible = true;
            }
            else
                xstrip.Visible = false;
        }
        private void RefreshYStrip()
        {
            var ystrip = (DataChart.Diagram as DevExpress.XtraCharts.SwiftPlotDiagram).AxisY.Strips[0];
            var down = (DataChart.Diagram as DevExpress.XtraCharts.SwiftPlotDiagram).AxisY.ConstantLines[3];
            var up = (DataChart.Diagram as DevExpress.XtraCharts.SwiftPlotDiagram).AxisY.ConstantLines[4];
            if (down.Visible && up.Visible)
            {
                if (Convert.ToDouble(down.AxisValue) < Convert.ToDouble(up.AxisValue))
                {
                    ystrip.MaxLimit.AxisValue = up.AxisValue;
                    ystrip.MinLimit.AxisValue = down.AxisValue;
                }
                else
                {
                    ystrip.MaxLimit.AxisValue = down.AxisValue;
                    ystrip.MinLimit.AxisValue = up.AxisValue;
                }
                ystrip.Visible = true;
            }
            else
                ystrip.Visible = false;
        }
        private void ConfigPopMenu(DevExpress.XtraBars.PopupMenu temp, string x, double pointy,object btn2tag,string type)
        {
            temp.LinksPersistInfo[0].Item.Caption = String.Format("X:{0:0.00} Y:{1:0.0000}", x, pointy);
            temp.LinksPersistInfo[1].Item.Description = x;
            temp.LinksPersistInfo[2].Item.Caption = type;
            (temp.LinksPersistInfo[2].Item as DevExpress.XtraBars.BarEditItem).EditValueChanged -= barEditItem3_EditValueChanged;
            (temp.LinksPersistInfo[2].Item as DevExpress.XtraBars.BarEditItem).EditValue = (btn2tag as DevExpress.XtraCharts.ConstantLine).AxisValue;
            (temp.LinksPersistInfo[2].Item as DevExpress.XtraBars.BarEditItem).EditValueChanged += barEditItem3_EditValueChanged;
            temp.LinksPersistInfo[2].Item.Tag = btn2tag;
            temp.LinksPersistInfo[3].Item.Caption = "删除" + type;
            temp.LinksPersistInfo[3].Item.Description = x;
            temp.LinksPersistInfo[3].Item.Tag = btn2tag;
        }
        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.RefreshResult();
            this.DrawData();
        }
        private void RefreshResult()
        {
            this.Result.RefreshData();
            this.layoutView1.RefreshData();
        }
        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int? left =null;
            int? right = null;
            double? up = null;
            double? down = null;
            var xstrip = (DataChart.Diagram as DevExpress.XtraCharts.SwiftPlotDiagram).AxisX.Strips[0];
            var ystrip = (DataChart.Diagram as DevExpress.XtraCharts.SwiftPlotDiagram).AxisY.Strips[0];
            if (!xstrip.Visible && !ystrip.Visible)
                return;
            if(xstrip.Visible)
            {
                left = Convert.ToInt32(xstrip.MinLimit.AxisValue);
                right = Convert.ToInt32(xstrip.MaxLimit.AxisValue);
            }
            if(ystrip.Visible)
            {
                down = Convert.ToDouble(ystrip.MinLimit.AxisValue);
                up = Convert.ToDouble(ystrip.MaxLimit.AxisValue);
            }
            this.gridView1.BeginDataUpdate();
            this.chooseDataFromBound(left, right, up, down);
            this.gridView1.EndDataUpdate();
        }
        private void chooseDataFromBound(int? left,int? right,double? up,double? down)
        {
            int count = this.Data.Rows.Count;
            if(left==null||right==null)
            {
                left = 0;
                right = count-1;
            }
            if(left>right||left<0||right>count-1||(up!=null&&down!=null&&up<down))
                return;
            for(int i = 0;i<left;i++)
            {
                DeleteSinglePointFromData(i);
            }
            if (up != null&& down != null)
                for (int i = (int)left; i <= right; i++)
                {
                    double? temp = this.Result[i];
                    if (temp != null && (temp > up || temp < down))
                        DeleteSinglePointFromData(i);
                }
            for (int i = (int)right+1; i < count; i++)
                DeleteSinglePointFromData(i);
            RefreshResult();
        }
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int[] index = this.gridView1.GetSelectedRows();
        }

        private void chartSizeInit()
        {
            this.panelControl3.Height =(int)(this.Size.Height * 0.8);
        }

        private void CPKtoolControl_SizeChanged(object sender, EventArgs e)
        {
            chartSizeInit();
        }

        //private void repositoryItemTextEdit4_EditValueChanged(object sender, EventArgs e)
        //{
        //    this.Result.GroupLength = Convert.ToInt32((this.layoutView1.ActiveEditor as DevExpress.XtraEditors.TextEdit).EditValue);
        //}

        //private void repositoryItemTextEdit5_EditValueChanged(object sender, EventArgs e)
        //{
        //    this.Result.Standard = Convert.ToDouble((this.layoutView1.ActiveEditor as DevExpress.XtraEditors.TextEdit).EditValue);
        //}

        //private void repositoryItemTextEdit6_EditValueChanged(object sender, EventArgs e)
        //{
        //    this.Result.UpTole = Convert.ToDouble((this.layoutView1.ActiveEditor as DevExpress.XtraEditors.TextEdit).EditValue);
        //}

        //private void repositoryItemTextEdit7_EditValueChanged(object sender, EventArgs e)
        //{
        //    this.Result.LowTole = Convert.ToDouble((this.layoutView1.ActiveEditor as DevExpress.XtraEditors.TextEdit).EditValue);
        //}

        //private void gridView1_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        //{
        //    if (e.IsGroupSummary)
        //    {
        //        if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
        //        {
        //            TempGroupCount = 0;
        //        }
        //        else if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate && Convert.ToBoolean(this.GetRowCellValue(e.RowHandle, ChooseColumnName)))
        //        {
        //            TempGroupCount++;
        //        }
        //        else if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
        //        {
        //            e.TotalValue = new GroupSummaryDataType(TempGroupCount, e.RowHandle);
        //        }
        //    }
        //}
    }
}