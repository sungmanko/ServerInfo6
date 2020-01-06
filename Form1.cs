using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace RacuosServerInfo6
{
    public partial class Form1 : Form
    {
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

        private string FileNameFrontTime1 = "f1time.log";
        private string FileNameFrontTime2 = "f2time.log";

        int typeA1 = 0;
        int typeB1 = 0;
        int typeC1 = 0;

        int typeA2 = 0;
        int typeB2 = 0;
        int typeC2 = 0;

        int typeA3 = 0;
        int typeB3 = 0;
        int typeC3 = 0;

        int typeA4 = 0;
        int typeB4 = 0;
        int typeC4 = 0;

        int typeA5 = 0;
        int typeB5 = 0;
        int typeC5 = 0;

        int typeA6 = 0;
        int typeB6 = 0;
        int typeC6 = 0;

        private List<Item> _tasksItem;
        private List<ItemFeature> _tasksFeature;

        private List<ItemFeature> _tasksFeature1;
        private List<ItemFeature> _tasksFeature2;
        private List<ItemFeature> _tasksFeature3;
        private List<ItemFeature> _tasksFeature4;
        private List<ItemFeature> _tasksFeature5;
        private List<ItemFeature> _tasksFeature6;

        public Form1()
        {
            InitializeComponent();
        }

        public class ItemFeature
        {
            public int RowNumber { get; set; }

            public string MessageID { get; set; }

            public string FeatureName { get; set; }
        }


        public class Item
        {
            public int RowNumber { get; set; }

            public string StartTime { get; set; }

            public int ProcessTime { get; set; }

            public string MessageID { get; set; }

            public string FeatureName { get; set; }

            public string FrontKind { get; set; }

            public string FrontTime { get; set; }
        }

        public class ItemCsv
        {
            public string ProcessTime { get; set; }

            public string MessageID { get; set; }

            public string FeatureName { get; set; }

            public string FrontKind { get; set; }

            public string FrontTime { get; set; }

            public string BackEndKind { get; set; }

            public string BackEndTime { get; set; }

            public string TotalTime { get; set; }
        }

        private List<ItemFeature> DataConvertFeature(string[] lineStandard)
        {
            string key1;
            string key2;
            string key3;

            int targetS;
            int feature_S;
            int dataCnt = 1;
            _tasksFeature = new List<ItemFeature>();

            foreach (string instr in lineStandard)
            {
                targetS = 0;
                feature_S = 0;

                key1 = instr;
                key2 = string.Empty;
                key3 = string.Empty;

                if (!string.IsNullOrEmpty(key1) && key1.IndexOf("FeatureName=") > 0)
                {
                    key2 = key1.Substring(11, 8);
                    key3 = string.Empty;

                    targetS = key1.IndexOf("messageId=");

                    if (targetS > 0)
                    {
                        // messageID
                        key2 = key1.Substring(targetS + 10, 32);
                    }

                    // featureName
                    feature_S = key1.IndexOf("FeatureName=");
                    key3 = key1.Substring(feature_S + 12, key1.Length - feature_S - 12);

                    ItemFeature taskItem = new ItemFeature()
                    {
                        RowNumber = dataCnt++,
                        MessageID = key2,
                        FeatureName = key3,
                    };

                    _tasksFeature.Add(taskItem);
                }
            }

            lineStandard = null;

            // 並び替え
            //_tasks = (from Item a in _tasks
            //          orderby a.ProcessTime descending
            //          select a).ToList();

            return _tasksFeature;
        }

        private List<Item> DataConvert(string[] lineTimes, List<ItemFeature> lineStandard)
        {
            string key1;
            string key2;
            int key3;
            string key4;
            string key5;

            int targetS, targetE;
            int messageIS_S, messageIS_E;
            int dataCnt = 1;
            ItemFeature targetData;

            typeA1 = 0;
            typeB1 = 0;
            typeC1 = 0;

            typeA2 = 0;
            typeB2 = 0;
            typeC2 = 0;

            typeA3 = 0;
            typeB3 = 0;
            typeC3 = 0;

            typeA4 = 0;
            typeB4 = 0;
            typeC4 = 0;

            typeA5 = 0;
            typeB5 = 0;
            typeC5 = 0;

            typeA6 = 0;
            typeB6 = 0;
            typeC6 = 0;

            _tasksItem = new List<Item>();

            foreach (string instr in lineTimes)
            {
                targetS = 0;
                targetE = 0;
                messageIS_S = 0;
                messageIS_E = 0;

                //key1 = sr.ReadLine();
                key1 = instr;
                key2 = string.Empty;
                key3 = 0;
                key4 = string.Empty;
                key5 = string.Empty;

                if (!string.IsNullOrEmpty(key1))
                {
                    key2 = key1.Substring(11, 8);
                    key3 = 0;
                    key4 = string.Empty;

                    targetS = key1.IndexOf("time: ");
                    targetE = key1.IndexOf("(ms)");

                    if (targetE > 0)
                    {
                        key3 = Convert.ToInt32(key1.Substring(targetS + 5, targetE - (targetS + 5)));
                    }

                    // 集計開始
                    int timeMM = Convert.ToInt32(key2.Substring(3, 2));

                    if (0 <= timeMM && timeMM < 10)
                    {
                        if (key3 <= 2000)
                            typeA1++;
                        else if (2000 < key3 && key3 <= 5000)
                            typeB1++;
                        else
                            typeC1++;
                    }
                    else if (10 <= timeMM && timeMM < 20)
                    {
                        if (key3 <= 2000)
                            typeA2++;
                        else if (2000 < key3 && key3 <= 5000)
                            typeB2++;
                        else
                            typeC2++;
                    }
                    else if (20 <= timeMM && timeMM < 30)
                    {
                        if (key3 <= 2000)
                            typeA3++;
                        else if (2000 < key3 && key3 <= 5000)
                            typeB3++;
                        else
                            typeC3++;
                    }
                    else if (30 <= timeMM && timeMM < 40)
                    {
                        if (key3 <= 2000)
                            typeA4++;
                        else if (2000 < key3 && key3 <= 5000)
                            typeB4++;
                        else
                            typeC4++;
                    }
                    else if (40 <= timeMM && timeMM < 50)
                    {
                        if (key3 <= 2000)
                            typeA5++;
                        else if (2000 < key3 && key3 <= 5000)
                            typeB5++;
                        else
                            typeC5++;
                    }
                    else
                    {
                        if (key3 <= 2000)
                            typeA6++;
                        else if (2000 < key3 && key3 <= 5000)
                            typeB6++;
                        else
                            typeC6++;
                    }
                    // 集計終了

                    messageIS_S = key1.IndexOf("messageId=");
                    messageIS_E = key1.IndexOf("] Exec");

                    key4 = key1.Substring(messageIS_S + 10, messageIS_E - (messageIS_S + 10));

                    targetData = null;
                    if (!string.IsNullOrEmpty(key4) &&
                        key4 != "null")
                    {
                        targetData = lineStandard.Where(x => (x as ItemFeature).MessageID == key4).FirstOrDefault();
                        if (targetData != null)
                        {
                            key5 = targetData.FeatureName;
                        }
                    }
                }

                Item taskItem = new Item()
                {
                    RowNumber = dataCnt++,
                    StartTime = key2,
                    ProcessTime = key3,
                    MessageID = key4,
                    FeatureName = key5,
                };

                _tasksItem.Add(taskItem);
            }

            lineTimes = null;
            lineStandard = null;

            // 並び替え
            //_tasks = (from Item a in _tasks
            //          orderby a.ProcessTime descending
            //          select a).ToList();

            return _tasksItem;
        }

        Label[] _label1 = new Label[21];
        Label[] _label2 = new Label[21];
        Label[] _label3 = new Label[21];
        Label[] _label4 = new Label[21];
        Label[] _label5 = new Label[21];
        Label[] _label6 = new Label[21];

        int cnt = 0;

        private async void button7_Click(object sender, EventArgs e)
        {
            button7.BackColor = System.Drawing.Color.Red;

            await Task.Run(() =>
            {
                DataConvert(File.ReadAllLines(Application.StartupPath + @"\" + FileNameT1 + txtDate.Text, Encoding.GetEncoding("Shift_JIS")),
                            _tasksFeature1);
            });

            dataGridView1.DataSource = _tasksItem;

            this.TextBoxControl(_label1);


            await Task.Run(() =>
            {
                DataConvert(File.ReadAllLines(Application.StartupPath + @"\" + FileNameT2 + txtDate.Text, Encoding.GetEncoding("Shift_JIS")),
                            _tasksFeature2);
            });

            dataGridView2.DataSource = _tasksItem;

            this.TextBoxControl(_label2);


            await Task.Run(() =>
            {
                DataConvert(File.ReadAllLines(Application.StartupPath + @"\" + FileNameT3 + txtDate.Text, Encoding.GetEncoding("Shift_JIS")),
                            _tasksFeature3);
            });

            dataGridView3.DataSource = _tasksItem;

            this.TextBoxControl(_label3);


            await Task.Run(() =>
            {
                DataConvert(File.ReadAllLines(Application.StartupPath + @"\" + FileNameT4 + txtDate.Text, Encoding.GetEncoding("Shift_JIS")),
                            _tasksFeature4);
            });

            dataGridView4.DataSource = _tasksItem;

            this.TextBoxControl(_label4);

            await Task.Run(() =>
            {
                DataConvert(File.ReadAllLines(Application.StartupPath + @"\" + FileNameT5 + txtDate.Text, Encoding.GetEncoding("Shift_JIS")),
                            _tasksFeature5);
            });

            dataGridView5.DataSource = _tasksItem;

            this.TextBoxControl(_label5);


            await Task.Run(() =>
            {
                DataConvert(File.ReadAllLines(Application.StartupPath + @"\" + FileNameT6 + txtDate.Text, Encoding.GetEncoding("Shift_JIS")),
                            _tasksFeature6);
            });

            dataGridView6.DataSource = _tasksItem;

            this.TextBoxControl(_label6);

            button7.BackColor = System.Drawing.Color.Green;
        }

        private void TextBoxControl(Label[] target)
        {
            int cnt = 0;
            target[cnt++].Text = typeA1.ToString();
            target[cnt++].Text = typeB1.ToString();
            target[cnt++].Text = typeC1.ToString();

            target[cnt++].Text = typeA2.ToString();
            target[cnt++].Text = typeB2.ToString();
            target[cnt++].Text = typeC2.ToString();

            target[cnt++].Text = typeA3.ToString();
            target[cnt++].Text = typeB3.ToString();
            target[cnt++].Text = typeC3.ToString();

            target[cnt++].Text = typeA4.ToString();
            target[cnt++].Text = typeB4.ToString();
            target[cnt++].Text = typeC4.ToString();

            target[cnt++].Text = typeA5.ToString();
            target[cnt++].Text = typeB5.ToString();
            target[cnt++].Text = typeC5.ToString();

            target[cnt++].Text = typeA6.ToString();
            target[cnt++].Text = typeB6.ToString();
            target[cnt++].Text = typeC6.ToString();

            target[cnt++].Text = (typeA1 + typeA2 + typeA3 + typeA4 + typeA5 + typeA6).ToString();
            target[cnt++].Text = (typeB1 + typeB2 + typeB3 + typeB4 + typeB5 + typeB6).ToString();
            target[cnt++].Text = (typeC1 + typeC2 + typeC3 + typeC4 + typeC5 + typeC6).ToString();
        }

        /// <summary>
        /// ChartControl
        /// </summary>
        /// <param name="targetChart"></param>
        /// <param name="dgTarget"></param>
        /// <param name="tartgetCnNumber"></param>
        private void ChartControl(Chart targetChart, DataGridView dgTarget, int tartgetCnNumber)
        {
            // clear
            targetChart.Series.Clear();
            targetChart.ChartAreas.Clear();
            targetChart.Titles.Clear();

            Title title1 = new Title("【CN" + tartgetCnNumber.ToString() + "】");

            // series
            Series seriesLine = new Series();
            seriesLine.ChartType = SeriesChartType.Column;
            seriesLine.LegendText = " ";
            seriesLine.BorderWidth = 1;
            seriesLine.MarkerStyle = MarkerStyle.Circle;
            seriesLine.MarkerSize = 2;

            for (int i = 0; i < dgTarget.RowCount; i++)
            {
                // 1. RowNumber
                // 2. StartTime
                // 3. ProcessTime
                // 4. MessageID
                // 5. FeatureName
                // 6. FrontKind
                // 7. FrontTime

                if (!string.IsNullOrEmpty(dgTarget.Rows[i].Cells[2].Value.ToString()) &&
                    Convert.ToInt32(dgTarget.Rows[i].Cells[2].Value) > Convert.ToInt32(txtTime.Text) * 1000)
                //&& dgTarget.Rows[i].Cells[6].Value != null)
                {
                    if (chkHensai.Checked && dgTarget.Rows[i].Cells[4].Value.ToString().IndexOf("Repay") > -1)
                    {
                        // 返済変更の場合は処理から除外する。
                    }
                    else if (chkHand.Checked && dgTarget.Rows[i].Cells[4].Value.ToString().IndexOf("Handover") > -1)
                    {
                        // 申送情報の場合は処理から除外する。
                    }
                    else
                    {
                        var data = new DataPoint();
                        data.SetValueXY(dgTarget.Rows[i].Cells[1].Value, Convert.ToInt32(dgTarget.Rows[i].Cells[2].Value) / 1000);
                        data.ToolTip =
                            "ProcessTime : " + dgTarget.Rows[i].Cells[1].Value.ToString() + Environment.NewLine +
                            "Feature = " + dgTarget.Rows[i].Cells[4].Value.ToString() + Environment.NewLine +
                            //"Front" + dgTarget.Rows[i].Cells[5].Value.ToString() + "経由" + Environment.NewLine + Environment.NewLine +
                            //"[Total] " + dgTarget.Rows[i].Cells[6].Value.ToString() + "(ms)" + Environment.NewLine +
                            //"[Front] " + (Convert.ToInt32(dgTarget.Rows[i].Cells[6].Value.ToString()) - Convert.ToInt32(dgTarget.Rows[i].Cells[2].Value.ToString())).ToString() + "(ms)" + Environment.NewLine +
                            "[BackEnd] " + dgTarget.Rows[i].Cells[2].Value.ToString() + "(ms)";

                        data.Tag = dgTarget.Rows[i].Cells[4].Value;
                        seriesLine.Points.Add(data);

                        // 出力用データ生成
                        //ItemCsv row = new ItemCsv()
                        //{
                        //    ProcessTime = dgTarget.Rows[i].Cells[1].Value.ToString(),
                        //    MessageID = dgTarget.Rows[i].Cells[3].Value.ToString(),
                        //    FeatureName = dgTarget.Rows[i].Cells[4].Value.ToString(),

                        //    FrontKind = "Front[" + dgTarget.Rows[i].Cells[5].Value.ToString() + "]",
                        //    FrontTime = (Convert.ToInt32(dgTarget.Rows[i].Cells[6].Value.ToString()) - Convert.ToInt32(dgTarget.Rows[i].Cells[2].Value.ToString())).ToString() + "(ms)",

                        //    BackEndKind = "CN[" + tartgetCnNumber.ToString() + "]",
                        //    BackEndTime = dgTarget.Rows[i].Cells[2].Value.ToString() + "(ms)",
                        //    TotalTime = dgTarget.Rows[i].Cells[6].Value.ToString(),
                        //};

                        // 出力用データ生成
                        // 0. 処理時間
                        // 1. メッセージID
                        // 2. FeatureName
                        // 3. Front種類
                        // 4. Front処理時間
                        // 5. BackEnd種類
                        // 6. BackEnd処理時間
                        // 7. 全体処理時間

                        dgCsv.Rows.Add(
                            dgTarget.Rows[i].Cells[1].Value.ToString(),
                            dgTarget.Rows[i].Cells[3].Value.ToString(),
                            dgTarget.Rows[i].Cells[4].Value.ToString(),
                            "",
                            "",
                            "CN[" + tartgetCnNumber.ToString() + "]",
                            dgTarget.Rows[i].Cells[2].Value.ToString(),
                            ""
                        );
                    }
                }
            }

            // chartarea
            ChartArea area1 = new ChartArea();
            area1.AxisX.Title = "処理時間(秒)";
            area1.AxisY.Title = dgTarget.Rows[0].Cells[1].Value.ToString().Substring(0, 2) + "時";

            area1.AxisY.Maximum = 50;

            targetChart.Titles.Add(title1);
            targetChart.ChartAreas.Add(area1);
            targetChart.Series.Add(seriesLine);
        }

        /// <summary>
        /// DataGridViewをEXCELに出力する
        /// </summary>
        public void SaveExcel()
        {
            SaveFileDialog save = new SaveFileDialog
            {
                Title = "ファイルを保存する",
                InitialDirectory = @"D:\",
                FileName = @"output.csv",
                Filter = "CSV(カンマ付き)|*.csv"
            };

            DialogResult output = save.ShowDialog();

            using (StreamWriter writer = new StreamWriter(save.FileName, false, Encoding.GetEncoding("shift_Jis")))
            {
                int row = dgCsv.RowCount;
                int colunms = dgCsv.ColumnCount;

                List<string> strList = new List<string>();

                for (int k = 0; k < colunms; k++)
                {
                    strList.Add(dgCsv.Columns[k].HeaderCell.Value.ToString());
                }

                string[] strHeader = strList.ToArray();

                string strCsvData2 = string.Join(",", strHeader);
                writer.WriteLine(strCsvData2);

                for (int i = 0; i < row; i++)
                {
                    strList = new List<string>();

                    for (int j = 0; j < colunms; j++)
                    {
                        if (dgCsv[j, i].Value != null)
                        {
                            strList.Add(dgCsv[j, i].Value.ToString());
                        }
                    }

                    string[] strArray = strList.ToArray();

                    string strCsvData = string.Join(",", strArray);
                    writer.WriteLine(strCsvData);

                }
            }

            MessageBox.Show("CSV出力完了しました。");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTime.Text))
            {
                MessageBox.Show("計測基準の時間を入力してください。");
            }
            else
            {
                dgCsv.Rows.Clear();
                ChartControl(chart1, dataGridView1, 1);
                ChartControl(chart2, dataGridView2, 2);
                ChartControl(chart3, dataGridView3, 3);
                ChartControl(chart4, dataGridView4, 4);
                ChartControl(chart5, dataGridView5, 5);
                ChartControl(chart6, dataGridView6, 6);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTime.Text))
            {
                MessageBox.Show("計測基準の時間を入力してください。");
            }
            else
            {
                Form2 fm2 = new Form2(this.dataGridView1, 1, Convert.ToInt32(txtTime.Text));
                fm2.Show();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTime.Text))
            {
                MessageBox.Show("計測基準の時間を入力してください。");
            }
            else
            {
                Form2 fm2 = new Form2(this.dataGridView2, 2, Convert.ToInt32(txtTime.Text));
                fm2.Show();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTime.Text))
            {
                MessageBox.Show("計測基準の時間を入力してください。");
            }
            else
            {
                Form2 fm2 = new Form2(this.dataGridView3, 3, Convert.ToInt32(txtTime.Text));
                fm2.Show();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTime.Text))
            {
                MessageBox.Show("計測基準の時間を入力してください。");
            }
            else
            {
                Form2 fm2 = new Form2(this.dataGridView4, 4, Convert.ToInt32(txtTime.Text));
                fm2.Show();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTime.Text))
            {
                MessageBox.Show("計測基準の時間を入力してください。");
            }
            else
            {
                Form2 fm2 = new Form2(this.dataGridView5, 5, Convert.ToInt32(txtTime.Text));
                fm2.Show();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTime.Text))
            {
                MessageBox.Show("計測基準の時間を入力してください。");
            }
            else
            {
                Form2 fm2 = new Form2(this.dataGridView6, 6, Convert.ToInt32(txtTime.Text));
                fm2.Show();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            #region CN1

            cnt = 0;
            _label1[cnt++] = cn1_10_1;
            _label1[cnt++] = cn1_10_2;
            _label1[cnt++] = cn1_10_3;

            _label1[cnt++] = cn1_20_1;
            _label1[cnt++] = cn1_20_2;
            _label1[cnt++] = cn1_20_3;

            _label1[cnt++] = cn1_30_1;
            _label1[cnt++] = cn1_30_2;
            _label1[cnt++] = cn1_30_3;

            _label1[cnt++] = cn1_40_1;
            _label1[cnt++] = cn1_40_2;
            _label1[cnt++] = cn1_40_3;

            _label1[cnt++] = cn1_50_1;
            _label1[cnt++] = cn1_50_2;
            _label1[cnt++] = cn1_50_3;

            _label1[cnt++] = cn1_60_1;
            _label1[cnt++] = cn1_60_2;
            _label1[cnt++] = cn1_60_3;

            _label1[cnt++] = cn1Total1;
            _label1[cnt++] = cn1Total2;
            _label1[cnt++] = cn1Total3;

            #endregion

            #region CN2

            cnt = 0;
            _label2[cnt++] = cn2_10_1;
            _label2[cnt++] = cn2_10_2;
            _label2[cnt++] = cn2_10_3;

            _label2[cnt++] = cn2_20_1;
            _label2[cnt++] = cn2_20_2;
            _label2[cnt++] = cn2_20_3;

            _label2[cnt++] = cn2_30_1;
            _label2[cnt++] = cn2_30_2;
            _label2[cnt++] = cn2_30_3;

            _label2[cnt++] = cn2_40_1;
            _label2[cnt++] = cn2_40_2;
            _label2[cnt++] = cn2_40_3;

            _label2[cnt++] = cn2_50_1;
            _label2[cnt++] = cn2_50_2;
            _label2[cnt++] = cn2_50_3;

            _label2[cnt++] = cn2_60_1;
            _label2[cnt++] = cn2_60_2;
            _label2[cnt++] = cn2_60_3;

            _label2[cnt++] = cn2Total1;
            _label2[cnt++] = cn2Total2;
            _label2[cnt++] = cn2Total3;

            #endregion

            #region cn3

            cnt = 0;
            _label3[cnt++] = cn3_10_1;
            _label3[cnt++] = cn3_10_2;
            _label3[cnt++] = cn3_10_3;

            _label3[cnt++] = cn3_20_1;
            _label3[cnt++] = cn3_20_2;
            _label3[cnt++] = cn3_20_3;

            _label3[cnt++] = cn3_30_1;
            _label3[cnt++] = cn3_30_2;
            _label3[cnt++] = cn3_30_3;

            _label3[cnt++] = cn3_40_1;
            _label3[cnt++] = cn3_40_2;
            _label3[cnt++] = cn3_40_3;

            _label3[cnt++] = cn3_50_1;
            _label3[cnt++] = cn3_50_2;
            _label3[cnt++] = cn3_50_3;

            _label3[cnt++] = cn3_60_1;
            _label3[cnt++] = cn3_60_2;
            _label3[cnt++] = cn3_60_3;

            _label3[cnt++] = cn3Total1;
            _label3[cnt++] = cn3Total2;
            _label3[cnt++] = cn3Total3;

            #endregion

            #region cn4

            cnt = 0;
            _label4[cnt++] = cn4_10_1;
            _label4[cnt++] = cn4_10_2;
            _label4[cnt++] = cn4_10_3;

            _label4[cnt++] = cn4_20_1;
            _label4[cnt++] = cn4_20_2;
            _label4[cnt++] = cn4_20_3;

            _label4[cnt++] = cn4_30_1;
            _label4[cnt++] = cn4_30_2;
            _label4[cnt++] = cn4_30_3;

            _label4[cnt++] = cn4_40_1;
            _label4[cnt++] = cn4_40_2;
            _label4[cnt++] = cn4_40_3;

            _label4[cnt++] = cn4_50_1;
            _label4[cnt++] = cn4_50_2;
            _label4[cnt++] = cn4_50_3;

            _label4[cnt++] = cn4_60_1;
            _label4[cnt++] = cn4_60_2;
            _label4[cnt++] = cn4_60_3;

            _label4[cnt++] = cn4Total1;
            _label4[cnt++] = cn4Total2;
            _label4[cnt++] = cn4Total3;

            #endregion

            #region cn5

            cnt = 0;
            _label5[cnt++] = cn5_10_1;
            _label5[cnt++] = cn5_10_2;
            _label5[cnt++] = cn5_10_3;

            _label5[cnt++] = cn5_20_1;
            _label5[cnt++] = cn5_20_2;
            _label5[cnt++] = cn5_20_3;

            _label5[cnt++] = cn5_30_1;
            _label5[cnt++] = cn5_30_2;
            _label5[cnt++] = cn5_30_3;

            _label5[cnt++] = cn5_40_1;
            _label5[cnt++] = cn5_40_2;
            _label5[cnt++] = cn5_40_3;

            _label5[cnt++] = cn5_50_1;
            _label5[cnt++] = cn5_50_2;
            _label5[cnt++] = cn5_50_3;

            _label5[cnt++] = cn5_60_1;
            _label5[cnt++] = cn5_60_2;
            _label5[cnt++] = cn5_60_3;

            _label5[cnt++] = cn5Total1;
            _label5[cnt++] = cn5Total2;
            _label5[cnt++] = cn5Total3;

            #endregion

            #region cn6

            cnt = 0;
            _label6[cnt++] = cn6_10_1;
            _label6[cnt++] = cn6_10_2;
            _label6[cnt++] = cn6_10_3;

            _label6[cnt++] = cn6_20_1;
            _label6[cnt++] = cn6_20_2;
            _label6[cnt++] = cn6_20_3;

            _label6[cnt++] = cn6_30_1;
            _label6[cnt++] = cn6_30_2;
            _label6[cnt++] = cn6_30_3;

            _label6[cnt++] = cn6_40_1;
            _label6[cnt++] = cn6_40_2;
            _label6[cnt++] = cn6_40_3;

            _label6[cnt++] = cn6_50_1;
            _label6[cnt++] = cn6_50_2;
            _label6[cnt++] = cn6_50_3;

            _label6[cnt++] = cn6_60_1;
            _label6[cnt++] = cn6_60_2;
            _label6[cnt++] = cn6_60_3;

            _label6[cnt++] = cn6Total1;
            _label6[cnt++] = cn6Total2;
            _label6[cnt++] = cn6Total3;

            #endregion

        }

        private void chart1_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            if (e.HitTestResult.ChartElementType != ChartElementType.DataPoint)
                return;
            int index = e.HitTestResult.PointIndex;
            DataPoint dp = e.HitTestResult.Series.Points[index];
            e.Text = string.Format("{0}", dp.ToolTip);
        }

        private void chart2_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            if (e.HitTestResult.ChartElementType != ChartElementType.DataPoint)
                return;
            int index = e.HitTestResult.PointIndex;
            DataPoint dp = e.HitTestResult.Series.Points[index];
            e.Text = string.Format("{0}", dp.ToolTip);
        }

        private void chart3_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            if (e.HitTestResult.ChartElementType != ChartElementType.DataPoint)
                return;
            int index = e.HitTestResult.PointIndex;
            DataPoint dp = e.HitTestResult.Series.Points[index];
            e.Text = string.Format("{0}", dp.ToolTip);
        }

        private void chart4_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            if (e.HitTestResult.ChartElementType != ChartElementType.DataPoint)
                return;
            int index = e.HitTestResult.PointIndex;
            DataPoint dp = e.HitTestResult.Series.Points[index];
            e.Text = string.Format("{0}", dp.ToolTip);
        }

        private void chart5_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            if (e.HitTestResult.ChartElementType != ChartElementType.DataPoint)
                return;
            int index = e.HitTestResult.PointIndex;
            DataPoint dp = e.HitTestResult.Series.Points[index];
            e.Text = string.Format("{0}", dp.ToolTip);
        }

        private void chart6_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            if (e.HitTestResult.ChartElementType != ChartElementType.DataPoint)
                return;
            int index = e.HitTestResult.PointIndex;
            DataPoint dp = e.HitTestResult.Series.Points[index];
            e.Text = string.Format("{0}", dp.ToolTip);
        }

        private async void btnFeature_Click(object sender, EventArgs e)
        {
            btnFeature.BackColor = System.Drawing.Color.Red;

            await Task.Run(() =>
            {
                DataConvertFeature(File.ReadAllLines(Application.StartupPath + @"\" + FileNameS1 + txtDate.Text, Encoding.GetEncoding("Shift_JIS")));
            });
            _tasksFeature1 = _tasksFeature;
            dataGridView7.DataSource = _tasksFeature;

            await Task.Run(() =>
            {
                DataConvertFeature(File.ReadAllLines(Application.StartupPath + @"\" + FileNameS2 + txtDate.Text, Encoding.GetEncoding("Shift_JIS")));
            });
            _tasksFeature2 = _tasksFeature;
            dataGridView7.BackgroundColor = System.Drawing.Color.Green;
            dataGridView8.DataSource = _tasksFeature;

            await Task.Run(() =>
            {
                DataConvertFeature(File.ReadAllLines(Application.StartupPath + @"\" + FileNameS3 + txtDate.Text, Encoding.GetEncoding("Shift_JIS")));
            });
            _tasksFeature3 = _tasksFeature;
            dataGridView7.BackgroundColor = System.Drawing.Color.Green;
            dataGridView9.DataSource = _tasksFeature;

            await Task.Run(() =>
            {
                DataConvertFeature(File.ReadAllLines(Application.StartupPath + @"\" + FileNameS4 + txtDate.Text, Encoding.GetEncoding("Shift_JIS")));
            });
            _tasksFeature4 = _tasksFeature;
            dataGridView7.BackgroundColor = System.Drawing.Color.Green;
            dataGridView10.DataSource = _tasksFeature;

            await Task.Run(() =>
            {
                DataConvertFeature(File.ReadAllLines(Application.StartupPath + @"\" + FileNameS5 + txtDate.Text, Encoding.GetEncoding("Shift_JIS")));
            });
            _tasksFeature5 = _tasksFeature;
            dataGridView7.BackgroundColor = System.Drawing.Color.Green;
            dataGridView11.DataSource = _tasksFeature;

            await Task.Run(() =>
            {
                DataConvertFeature(File.ReadAllLines(Application.StartupPath + @"\" + FileNameS6 + txtDate.Text, Encoding.GetEncoding("Shift_JIS")));
            });
            _tasksFeature6 = _tasksFeature;
            dataGridView7.BackgroundColor = System.Drawing.Color.Green;
            dataGridView12.DataSource = _tasksFeature;

            btnFeature.BackColor = System.Drawing.Color.Green;
        }

        private void btnLoginUser_Click(object sender, EventArgs e)
        {
            Form3 fm3 = new Form3();
            fm3.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            // CSV出力
            SaveExcel();
        }

        private async void button11_Click(object sender, EventArgs e)
        {
            button11.BackColor = System.Drawing.Color.Red;

            await Task.Run(() =>
            {
                FinalConvert();
            });

            button11.BackColor = System.Drawing.Color.Green;
        }

        private void FinalConvert()
        {
            string key1;
            string key2;
            string key3;

            int targetS;
            int time_S;
            int time_E;

            string[] front1 = File.ReadAllLines(Application.StartupPath + @"\" + FileNameFrontTime1, Encoding.GetEncoding("Shift_JIS"));
            string[] front2 = File.ReadAllLines(Application.StartupPath + @"\" + FileNameFrontTime2, Encoding.GetEncoding("Shift_JIS"));

            for (int i = 0; i < dgCsv.Rows.Count; i++)
            {
                if (dgCsv.Rows[i].Cells[1].Value != null)
                {
                    key1 = front1.Where(x => x.IndexOf(dgCsv.Rows[i].Cells[1].Value.ToString()) > -1).LastOrDefault();
                    if (!string.IsNullOrEmpty(key1))
                    {
                        // ★Front 1★
                        targetS = key1.IndexOf("messageId=");

                        // messageID
                        key2 = key1.Substring(targetS + 10, 32);

                        time_S = key1.IndexOf("Exec time: ");
                        time_E = key1.IndexOf("(ms)");
                        key3 = key1.Substring(time_S + 11, time_E - (time_S + 11));

                        dgCsv.Rows[i].Cells[3].Value = "Front[1]";
                        dgCsv.Rows[i].Cells[4].Value = (Convert.ToInt32(key3) - Convert.ToInt32(dgCsv.Rows[i].Cells[6].Value.ToString())).ToString();
                        dgCsv.Rows[i].Cells[7].Value = key3;

                        if (dgCsv.Rows[i].Cells[4].Value.ToString().IndexOf("-") > -1)
                        {
                            // ★Front 2★
                            key1 = front2.Where(x => x.IndexOf(dgCsv.Rows[i].Cells[1].Value.ToString()) > -1).LastOrDefault();
                            if (!string.IsNullOrEmpty(key1))
                            {
                                targetS = key1.IndexOf("messageId=");

                                // messageID
                                key2 = key1.Substring(targetS + 10, 32);

                                time_S = key1.IndexOf("Exec time: ");
                                time_E = key1.IndexOf("(ms)");
                                key3 = key1.Substring(time_S + 11, time_E - (time_S + 11));

                                dgCsv.Rows[i].Cells[3].Value = "Front[2]";
                                dgCsv.Rows[i].Cells[4].Value = (Convert.ToInt32(key3) - Convert.ToInt32(dgCsv.Rows[i].Cells[6].Value.ToString())).ToString();
                                dgCsv.Rows[i].Cells[7].Value = key3;
                            }
                        }
                    }
                    else
                    {
                        // ★Front 2★
                        key1 = front2.Where(x => x.IndexOf(dgCsv.Rows[i].Cells[1].Value.ToString()) > -1).LastOrDefault();
                        if (!string.IsNullOrEmpty(key1))
                        {
                            targetS = key1.IndexOf("messageId=");

                            // messageID
                            key2 = key1.Substring(targetS + 10, 32);

                            time_S = key1.IndexOf("Exec time: ");
                            time_E = key1.IndexOf("(ms)");
                            key3 = key1.Substring(time_S + 11, time_E - (time_S + 11));

                            dgCsv.Rows[i].Cells[3].Value = "Front[2]";
                            dgCsv.Rows[i].Cells[4].Value = (Convert.ToInt32(key3) - Convert.ToInt32(dgCsv.Rows[i].Cells[6].Value.ToString())).ToString();
                            dgCsv.Rows[i].Cells[7].Value = key3;

                            if (dgCsv.Rows[i].Cells[4].Value.ToString().IndexOf("-") > -1)
                            {
                                // ★Front 2★
                                key1 = front1.Where(x => x.IndexOf(dgCsv.Rows[i].Cells[1].Value.ToString()) > -1).LastOrDefault();
                                if (!string.IsNullOrEmpty(key1))
                                {
                                    targetS = key1.IndexOf("messageId=");

                                    // messageID
                                    key2 = key1.Substring(targetS + 10, 32);

                                    time_S = key1.IndexOf("Exec time: ");
                                    time_E = key1.IndexOf("(ms)");
                                    key3 = key1.Substring(time_S + 11, time_E - (time_S + 11));

                                    dgCsv.Rows[i].Cells[3].Value = "Front[1]";
                                    dgCsv.Rows[i].Cells[4].Value = (Convert.ToInt32(key3) - Convert.ToInt32(dgCsv.Rows[i].Cells[6].Value.ToString())).ToString();
                                    dgCsv.Rows[i].Cells[7].Value = key3;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Form4 fm4 = new Form4();
            fm4.Show();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            // dgTimeCsv初期化
            dgTimeCsv.Rows.Clear();

            // Label集計データCSV出力の為、グリッドのデータ入れ込む
            for (int i = 0; i <= 20; i++)
            {
                // CN1 ~ CN6 までの結果を一列で導入
                dgTimeCsv.Rows.Add
                (
                    // 1~21
                    _label1[i].Text, _label2[i].Text, _label3[i].Text, _label4[i].Text, _label5[i].Text, _label6[i].Text
                );
            }

            // CSV出力を行う。
            this.TimeResultSaveExcel();
        }


        /// <summary>
        /// Time集計分析結果をCSV出力
        /// </summary>
        public void TimeResultSaveExcel()
        {
            SaveFileDialog save = new SaveFileDialog
            {
                Title = "ファイルを保存する",
                InitialDirectory = @"D:\",
                FileName = @"TimeResult.csv",
                Filter = "CSV(カンマ付き)|*.csv"
            };

            DialogResult output = save.ShowDialog();

            using (StreamWriter writer = new StreamWriter(save.FileName, false, Encoding.GetEncoding("shift_Jis")))
            {
                int row = dgTimeCsv.RowCount;
                int colunms = dgTimeCsv.ColumnCount;

                List<string> strList = new List<string>();

                for (int k = 0; k < colunms; k++)
                {
                    strList.Add(dgTimeCsv.Columns[k].HeaderCell.Value.ToString());
                }

                string[] strHeader = strList.ToArray();

                string strCsvData2 = "CN1,CN2,CN3,CN4,CN5,CN6";
                writer.WriteLine(strCsvData2);

                for (int i = 0; i < row; i++)
                {
                    strList = new List<string>();

                    for (int j = 0; j < colunms; j++)
                    {
                        if (dgTimeCsv[j, i].Value != null)
                        {
                            strList.Add(dgTimeCsv[j, i].Value.ToString());
                        }
                    }

                    string[] strArray = strList.ToArray();

                    string strCsvData = string.Join(",", strArray);
                    writer.WriteLine(strCsvData);

                }
            }

            MessageBox.Show("CSV出力完了しました。");
        }
    }
}
