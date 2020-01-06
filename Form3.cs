using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace RacuosServerInfo6
{
    public partial class Form3 : Form
    {
        List<ItemLogin> _tasksItem;

        List<Item> _tasksItemTime;

        private string FileNameT1 = "time_CN1.log";
        private string FileNameT2 = "time_CN2.log";
        private string FileNameT3 = "time_CN3.log";
        private string FileNameT4 = "time_CN4.log";
        private string FileNameT5 = "time_CN5.log";
        private string FileNameT6 = "time_CN6.log";

        private string FileNameS1 = "standard_CN1.log";
        private string FileNameS2 = "standard_CN2.log";
        private string FileNameS3 = "standard_CN3.log";
        private string FileNameS4 = "standard_CN4.log";
        private string FileNameS5 = "standard_CN5.log";
        private string FileNameS6 = "standard_CN6.log";

        public Form3()
        {
            InitializeComponent();
        }


        public class ItemLogin
        {
            public int RowNumber { get; set; }

            public string LoginTime { get; set; }

            public string UserID { get; set; }
        }

        public class Item
        {
            public int RowNumber { get; set; }

            public string StartTime { get; set; }

            public int ProcessCounter { get; set; }

        }

        private List<Item> DataConvertTime(string[] lineTimes)
        {
            string key1;
            string key2;

            int targetS;
            List<int> data = new List<int>();
            data.Add(0);
            data.Add(0);
            data.Add(0);
            data.Add(0);
            data.Add(0);
            data.Add(0);

            _tasksItemTime = new List<Item>();

            foreach (string instr in lineTimes)
            {
                targetS = 0;

                key1 = instr;
                key2 = string.Empty;

                targetS = key1.IndexOf("Exec time:");

                if (!string.IsNullOrEmpty(key1) && targetS > -1)
                {
                    key2 = key1.Substring(11, 8);

                    // 集計開始
                    int timeMM = Convert.ToInt32(key2.Substring(3, 2));

                    if (0 <= timeMM && timeMM < 10)
                    {
                        data[0]++;
                    }
                    else if (10 <= timeMM && timeMM < 20)
                    {
                        data[1]++;
                    }
                    else if (20 <= timeMM && timeMM < 30)
                    {
                        data[2]++;
                    }
                    else if (30 <= timeMM && timeMM < 40)
                    {
                        data[3]++;
                    }
                    else if (40 <= timeMM && timeMM < 50)
                    {
                        data[4]++;
                    }
                    else
                    {
                        data[5]++;
                    }
                }
            }

            int dataCnt = 1;
            foreach (int target in data)
            {
                Item taskItem = new Item()
                {
                    RowNumber = dataCnt,
                    StartTime = dataCnt.ToString() + "0",
                    ProcessCounter = target,
                };
                _tasksItemTime.Add(taskItem);
                dataCnt++;
            }

            lineTimes = null;

            return _tasksItemTime;
        }


        private List<ItemLogin> DataConvert(string[] lineTimes)
        {
            string key1;
            string key2;
            string key3;

            int targetS;
            int loginID_S;
            int dataCnt = 1;

            _tasksItem = new List<ItemLogin>();

            foreach (string instr in lineTimes)
            {
                targetS = 0;
                loginID_S = 0;

                key1 = instr;
                key2 = string.Empty;
                key3 = string.Empty;

                if (!string.IsNullOrEmpty(key1))
                {
                    targetS = key1.IndexOf("FeatureName=MainMenuSearch");
                    if (targetS > -1)
                    {
                        key2 = key1.Substring(11, 8);

                        loginID_S = key1.IndexOf("LoggedOnEmployeeNo=");
                        key3 = key1.Substring(loginID_S + 19, 6);

                        ItemLogin taskItem = new ItemLogin()
                        {
                            RowNumber = dataCnt++,
                            LoginTime = key2,
                            UserID = key3,
                        };

                        _tasksItem.Add(taskItem);
                    }
                }
            }

            lineTimes = null;

            // 並び替え
            //_tasks = (from Item a in _tasks
            //          orderby a.ProcessTime descending
            //          select a).ToList();

            return _tasksItem;
        }

        private async void btnDataInput_Click(object sender, EventArgs e)
        {

            await Task.Run(() =>
            {
                DataConvert(File.ReadAllLines(Application.StartupPath + @"\" + FileNameS1, Encoding.GetEncoding("Shift_JIS")));
            });

            dataGridView1.DataSource = _tasksItem;


            await Task.Run(() =>
            {
                DataConvert(File.ReadAllLines(Application.StartupPath + @"\" + FileNameS2, Encoding.GetEncoding("Shift_JIS")));
            });

            dataGridView2.DataSource = _tasksItem;


            await Task.Run(() =>
            {
                DataConvert(File.ReadAllLines(Application.StartupPath + @"\" + FileNameS3, Encoding.GetEncoding("Shift_JIS")));
            });

            dataGridView3.DataSource = _tasksItem;


            await Task.Run(() =>
            {
                DataConvert(File.ReadAllLines(Application.StartupPath + @"\" + FileNameS4, Encoding.GetEncoding("Shift_JIS")));
            });

            dataGridView4.DataSource = _tasksItem;


            await Task.Run(() =>
            {
                DataConvert(File.ReadAllLines(Application.StartupPath + @"\" + FileNameS5, Encoding.GetEncoding("Shift_JIS")));
            });

            dataGridView5.DataSource = _tasksItem;


            await Task.Run(() =>
            {
                DataConvert(File.ReadAllLines(Application.StartupPath + @"\" + FileNameS6, Encoding.GetEncoding("Shift_JIS")));
            });

            dataGridView6.DataSource = _tasksItem;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Visible = true;
            chart2.Visible = true;
            chart3.Visible = true;
            chart4.Visible = true;
            chart5.Visible = true;
            chart6.Visible = true;
            chart7.Visible = false;

            _data1 = ChartControl(chart1, dataGridView1, dataGridView7, 1);
            _data2 = ChartControl(chart2, dataGridView2, dataGridView8, 2);
            _data3 = ChartControl(chart3, dataGridView3, dataGridView9, 3);
            _data4 = ChartControl(chart4, dataGridView4, dataGridView10, 4);
            _data5 = ChartControl(chart5, dataGridView5, dataGridView11, 5);
            _data6 = ChartControl(chart6, dataGridView6, dataGridView12, 6);
        }

        private List<int> _data1 = new List<int>(6);
        private List<int> _data2 = new List<int>(6);
        private List<int> _data3 = new List<int>(6);
        private List<int> _data4 = new List<int>(6);
        private List<int> _data5 = new List<int>(6);
        private List<int> _data6 = new List<int>(6);

        /// <summary>
        /// ChartControl
        /// </summary>
        /// <param name="targetChart"></param>
        /// <param name="dgTargetStandard"></param>
        /// <param name="tartgetCnNumber"></param>
        private List<int> ChartControl(Chart targetChart, DataGridView dgTargetStandard, DataGridView dgTargetTime, int tartgetCnNumber)
        {
            if (txtMax.Text == string.Empty)
            {
                MessageBox.Show("MAX数値を入れてください。");
                return null;
            }

            string keyTime = string.Empty;
            int KeyTimeInt = 0;
            int A1, A2, A3, A4, A5, A6;
            A1 = 0;
            A2 = 0;
            A3 = 0;
            A4 = 0;
            A5 = 0;
            A6 = 0;

            for (int i = 0; i < dgTargetStandard.RowCount; i++)
            {
                keyTime = dgTargetStandard.Rows[i].Cells[1].Value.ToString().Substring(3, 2);
                KeyTimeInt = Convert.ToInt32(keyTime);

                if (0 <= KeyTimeInt && KeyTimeInt < 10)
                {
                    A1++;
                }
                else if (10 <= KeyTimeInt && KeyTimeInt < 20)
                {
                    A2++;
                }
                else if (20 <= KeyTimeInt && KeyTimeInt < 30)
                {
                    A3++;
                }
                else if (30 <= KeyTimeInt && KeyTimeInt < 40)
                {
                    A4++;
                }
                else if (40 <= KeyTimeInt && KeyTimeInt < 50)
                {
                    A5++;
                }
                else
                {
                    A6++;
                }
            }

            // clear
            targetChart.Series.Clear();
            targetChart.ChartAreas.Clear();
            targetChart.Titles.Clear();

            Title title1 = new Title("【CN" + tartgetCnNumber.ToString() + "】");

            // ■ Series1
            Series seriesLine = new Series();
            seriesLine.ChartType = SeriesChartType.Column;
            seriesLine.LegendText = "ユーザ数";
            seriesLine.BorderWidth = 1;
            seriesLine.MarkerStyle = MarkerStyle.Circle;
            seriesLine.MarkerSize = 2;

            // masterDataIdx
            // ※ xx時
            string masterX = dgTargetStandard.Rows[0].Cells[1].Value.ToString().Substring(0, 2);

            // #1
            var data1 = new DataPoint();
            data1.SetValueXY("10", A1);
            seriesLine.Points.Add(data1);
            // #2
            var data2 = new DataPoint();
            data2.SetValueXY("20", A2);
            seriesLine.Points.Add(data2);
            // #3
            var data3 = new DataPoint();
            data3.SetValueXY("30", A3);
            seriesLine.Points.Add(data3);
            // #4
            var data4 = new DataPoint();
            data4.SetValueXY("40", A4);
            seriesLine.Points.Add(data4);
            // #5
            var data5 = new DataPoint();
            data5.SetValueXY("50", A5);
            seriesLine.Points.Add(data5);
            // #6
            var data6 = new DataPoint();
            data6.SetValueXY("60", A6);
            seriesLine.Points.Add(data6);


            // ■ Series2
            Series seriesColumn = new Series();
            seriesColumn.LegendText = "レス数(単位100)";
            seriesColumn.ChartType = SeriesChartType.Line;

            // #1
            for (int i = 0; i < dgTargetTime.Rows.Count; i++)
            {
                var dataTime = new DataPoint();
                dataTime.SetValueXY(dgTargetTime.Rows[i].Cells[1].Value.ToString(),
                                (Convert.ToInt32(dgTargetTime.Rows[i].Cells[2].Value.ToString()) / 100).ToString());
                seriesColumn.Points.Add(dataTime);
            }

            // chartarea
            ChartArea area1 = new ChartArea();
            area1.AxisX.Title = "ユーザ数";
            area1.AxisY.Title = masterX + "時のログイン数";

            area1.AxisY.Maximum = Convert.ToInt32(txtMax.Text);

            targetChart.Titles.Add(title1);
            targetChart.ChartAreas.Add(area1);
            targetChart.Series.Add(seriesLine);
            targetChart.Series.Add(seriesColumn);


            List<int> rtInt = new List<int>();
            rtInt.Add(A1);
            rtInt.Add(A2);
            rtInt.Add(A3);
            rtInt.Add(A4);
            rtInt.Add(A5);
            rtInt.Add(A6);
            return rtInt;
        }

        private void chart1_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            if (e.HitTestResult.ChartElementType != ChartElementType.DataPoint)
                return;
            int index = e.HitTestResult.PointIndex;
            DataPoint dp = e.HitTestResult.Series.Points[index];
            e.Text = string.Format("[{0}] {1} {2}名\r\n{3}", dp.AxisLabel, dp.LegendText, dp.YValues[0], dp.Tag);
        }

        private void chart2_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            if (e.HitTestResult.ChartElementType != ChartElementType.DataPoint)
                return;
            int index = e.HitTestResult.PointIndex;
            DataPoint dp = e.HitTestResult.Series.Points[index];
            e.Text = string.Format("[{0}] {1} {2}名\r\n{3}", dp.AxisLabel, dp.LegendText, dp.YValues[0], dp.Tag);
        }

        private void chart3_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            if (e.HitTestResult.ChartElementType != ChartElementType.DataPoint)
                return;
            int index = e.HitTestResult.PointIndex;
            DataPoint dp = e.HitTestResult.Series.Points[index];
            e.Text = string.Format("[{0}] {1} {2}名\r\n{3}", dp.AxisLabel, dp.LegendText, dp.YValues[0], dp.Tag);
        }

        private void chart4_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            if (e.HitTestResult.ChartElementType != ChartElementType.DataPoint)
                return;
            int index = e.HitTestResult.PointIndex;
            DataPoint dp = e.HitTestResult.Series.Points[index];
            e.Text = string.Format("[{0}] {1} {2}名\r\n{3}", dp.AxisLabel, dp.LegendText, dp.YValues[0], dp.Tag);
        }

        private void chart5_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            if (e.HitTestResult.ChartElementType != ChartElementType.DataPoint)
                return;
            int index = e.HitTestResult.PointIndex;
            DataPoint dp = e.HitTestResult.Series.Points[index];
            e.Text = string.Format("[{0}] {1} {2}名\r\n{3}", dp.AxisLabel, dp.LegendText, dp.YValues[0], dp.Tag);
        }

        private void chart6_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            if (e.HitTestResult.ChartElementType != ChartElementType.DataPoint)
                return;
            int index = e.HitTestResult.PointIndex;
            DataPoint dp = e.HitTestResult.Series.Points[index];
            e.Text = string.Format("[{0}] {1} {2}名\r\n{3}", dp.AxisLabel, dp.LegendText, dp.YValues[0], dp.Tag);
        }

        private async void btnDataInput2_Click(object sender, EventArgs e)
        {

            await Task.Run(() =>
            {
                DataConvertTime(File.ReadAllLines(Application.StartupPath + @"\" + FileNameS1, Encoding.GetEncoding("Shift_JIS")));
            });

            dataGridView7.DataSource = _tasksItemTime;


            await Task.Run(() =>
            {
                DataConvertTime(File.ReadAllLines(Application.StartupPath + @"\" + FileNameS2, Encoding.GetEncoding("Shift_JIS")));
            });

            dataGridView8.DataSource = _tasksItemTime;


            await Task.Run(() =>
            {
                DataConvertTime(File.ReadAllLines(Application.StartupPath + @"\" + FileNameS3, Encoding.GetEncoding("Shift_JIS")));
            });

            dataGridView9.DataSource = _tasksItemTime;


            await Task.Run(() =>
            {
                DataConvertTime(File.ReadAllLines(Application.StartupPath + @"\" + FileNameS4, Encoding.GetEncoding("Shift_JIS")));
            });

            dataGridView10.DataSource = _tasksItemTime;


            await Task.Run(() =>
            {
                DataConvertTime(File.ReadAllLines(Application.StartupPath + @"\" + FileNameS5, Encoding.GetEncoding("Shift_JIS")));
            });

            dataGridView11.DataSource = _tasksItemTime;


            await Task.Run(() =>
            {
                DataConvertTime(File.ReadAllLines(Application.StartupPath + @"\" + FileNameS6, Encoding.GetEncoding("Shift_JIS")));
            });

            dataGridView12.DataSource = _tasksItemTime;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            chart1.Visible = false;
            chart2.Visible = false;
            chart3.Visible = false;
            chart4.Visible = false;
            chart5.Visible = false;
            chart6.Visible = false;
            chart7.Visible = true;

            ChartControlAll(chart7);
        }

        /// <summary>
        /// ChartControlAll
        /// </summary>
        /// <param name="targetChart"></param>
        private void ChartControlAll(Chart targetChart)
        {
            if (txtMax.Text == string.Empty)
            {
                MessageBox.Show("MAX数値を入れてください。");
                return;
            }

            string keyTime = string.Empty;

            // clear
            targetChart.Series.Clear();
            targetChart.ChartAreas.Clear();
            targetChart.Titles.Clear();

            Title title1 = new Title("【CN1～CN6】");

            // ■ Series1
            Series seriesLine = new Series();
            seriesLine.ChartType = SeriesChartType.Column;
            seriesLine.LegendText = "ユーザ数";
            seriesLine.BorderWidth = 1;
            seriesLine.MarkerStyle = MarkerStyle.Circle;
            seriesLine.MarkerSize = 2;

            // masterDataIdx
            // ※ xx時
            string masterX = dataGridView7.Rows[0].Cells[1].Value.ToString().Substring(0, 2); ;

            // #1
            var data1 = new DataPoint();
            data1.SetValueXY("10", _data1[0] + _data2[0] + _data3[0] + _data4[0] + _data5[0] + _data6[0]);
            seriesLine.Points.Add(data1);
            // #2
            var data2 = new DataPoint();
            data2.SetValueXY("20", _data1[1] + _data2[1] + _data3[1] + _data4[1] + _data5[1] + _data6[1]);
            seriesLine.Points.Add(data2);
            // #3
            var data3 = new DataPoint();
            data3.SetValueXY("30", _data1[2] + _data2[2] + _data3[2] + _data4[2] + _data5[2] + _data6[2]);
            seriesLine.Points.Add(data3);
            // #4
            var data4 = new DataPoint();
            data4.SetValueXY("40", _data1[3] + _data2[3] + _data3[3] + _data4[3] + _data5[3] + _data6[3]);
            seriesLine.Points.Add(data4);
            // #5
            var data5 = new DataPoint();
            data5.SetValueXY("50", _data1[4] + _data2[4] + _data3[4] + _data4[4] + _data5[4] + _data6[4]);
            seriesLine.Points.Add(data5);
            // #6
            var data6 = new DataPoint();
            data6.SetValueXY("60", _data1[5] + _data2[5] + _data3[5] + _data4[5] + _data5[5] + _data6[5]);
            seriesLine.Points.Add(data6);

            // ■ Series2
            Series seriesColumn = new Series();
            seriesColumn.LegendText = "レス数(単位100)";
            seriesColumn.ChartType = SeriesChartType.Line;

            // #1
            for (int i = 0; i < 6; i++)
            {
                var dataTime = new DataPoint();
                dataTime.SetValueXY(dataGridView7.Rows[i].Cells[1].Value.ToString(),
                                ((Convert.ToInt32(dataGridView7.Rows[i].Cells[2].Value) +
                                  Convert.ToInt32(dataGridView8.Rows[i].Cells[2].Value) +
                                  Convert.ToInt32(dataGridView9.Rows[i].Cells[2].Value) +
                                  Convert.ToInt32(dataGridView10.Rows[i].Cells[2].Value) +
                                  Convert.ToInt32(dataGridView11.Rows[i].Cells[2].Value) +
                                  Convert.ToInt32(dataGridView12.Rows[i].Cells[2].Value)) / 100).ToString());

                seriesColumn.Points.Add(dataTime);
            }

            // chartarea
            ChartArea area1 = new ChartArea();
            area1.AxisX.Title = "ユーザ数";
            area1.AxisY.Title = masterX + "時のログイン数";

            area1.AxisY.Maximum = Convert.ToInt32(txtMax.Text);

            targetChart.Titles.Add(title1);
            targetChart.ChartAreas.Add(area1);
            targetChart.Series.Add(seriesLine);
            targetChart.Series.Add(seriesColumn);
        }

        private void chart7_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            if (e.HitTestResult.ChartElementType != ChartElementType.DataPoint)
                return;
            int index = e.HitTestResult.PointIndex;
            DataPoint dp = e.HitTestResult.Series.Points[index];
            e.Text = string.Format("[{0}] {1} {2}名\r\n{3}", dp.AxisLabel, dp.LegendText, dp.YValues[0], dp.Tag);
        }
    }
}
