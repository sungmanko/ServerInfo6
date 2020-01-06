using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RacuosServerInfo6
{
    public partial class Form4 : Form
    {
        private string FileNameT1 = "time_CN1.log";
        private string FileNameT2 = "time_CN2.log";
        private string FileNameT3 = "time_CN3.log";
        private string FileNameT4 = "time_CN4.log";
        private string FileNameT5 = "time_CN5.log";
        private string FileNameT6 = "time_CN6.log";

        private string FileNameFrontTime1 = "f1time.log";
        private string FileNameFrontTime2 = "f2time.log";

        private List<ItemTime> _tasksFeature1;
        private List<ItemTime> _tasksFeature2;
        private List<ItemTime> _tasksFeature3;
        private List<ItemTime> _tasksFeature4;
        private List<ItemTime> _tasksFeature5;
        private List<ItemTime> _tasksFeature6;
        private List<ItemTime> _tasksFeatureTotal;

        private List<ItemCsv> _tmpList = new List<ItemCsv>();

        private List<ItemCsv> _tmpList1 = new List<ItemCsv>();
        private List<ItemCsv> _tmpList2 = new List<ItemCsv>();

        private List<ItemTime> _tasksFeature;

        private List<ItemFrontCount> _tasksCount;

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

        public class ItemTime
        {
            public int RowNumber { get; set; }

            public string MessageID { get; set; }

            public string ProcessTime { get; set; }

            public string BackendKind { get; set; }
        }

        public class ItemFrontCount
        {
            /// <summary>
            /// 5 / 10 / 15 ~~~ 
            /// </summary>
            public string RowComment { get; set; }

            public string CountIN { get; set; }

            public string CountOUT { get; set; }
        }


        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.BackColor = Color.Red;

            _tmpList = X1(
                File.ReadAllLines(Application.StartupPath + @"\" + FileNameFrontTime1, Encoding.GetEncoding("Shift_JIS")),
                1
              );
            _tmpList1 = _tmpList;
            dataGridView1.DataSource = _tmpList;

            button1.BackColor = Color.Yellow;

            _tmpList = X1(
                File.ReadAllLines(Application.StartupPath + @"\" + FileNameFrontTime2, Encoding.GetEncoding("Shift_JIS")),
                2
              );
            _tmpList2 = _tmpList;
            dataGridView2.DataSource = _tmpList;

            button1.BackColor = Color.Green;
        }

        private List<ItemCsv> X1(string[] frontData, int cnNumber)
        {
            string key2;
            string key3;

            int targetS;
            int time_S;
            int time_E;

            _tmpList = new List<ItemCsv>();

            foreach (string key1 in frontData)
            {
                if (!string.IsNullOrEmpty(key1))
                {
                    targetS = key1.IndexOf("messageId=");
                    if (targetS > -1)
                    {
                        // messageID
                        key2 = key1.Substring(targetS + 10, 32);

                        time_S = key1.IndexOf("Exec time: ");
                        time_E = key1.IndexOf("(ms)");

                        // ProcessTime
                        key3 = key1.Substring(time_S + 11, time_E - (time_S + 11));

                        if (Convert.ToInt32(key3) > Convert.ToInt32(textBox1.Text))
                        {
                            // データ導入対象
                            ItemCsv icsv = new ItemCsv()
                            {
                                ProcessTime = key1.Substring(11, 8),
                                MessageID = key2,
                                FeatureName = string.Empty,
                                FrontKind = "Front[" + cnNumber.ToString() + "]",
                                FrontTime = string.Empty,
                                BackEndKind = string.Empty,
                                BackEndTime = string.Empty,
                                TotalTime = key3,
                            };
                            _tmpList.Add(icsv);
                        }
                    }
                }
            }

            return _tmpList;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveExcel(dataGridView1, "front1");
        }


        /// <summary>
        /// DataGridViewをEXCELに出力する
        /// </summary>
        /// <param name="dgCsv"></param>
        /// <param name="fileName"></param>
        public void SaveExcel(DataGridView dgCsv, string fileName)
        {
            SaveFileDialog save = new SaveFileDialog
            {
                Title = "ファイルを保存する",
                InitialDirectory = @"D:\",
                FileName = fileName + ".csv",
                Filter = "CSV(カンマ付き)|*.csv"
            };

            DialogResult output = save.ShowDialog();

            using (StreamWriter writer = new StreamWriter(save.FileName, false, Encoding.GetEncoding("shift_Jis")))
            {
                int row = dgCsv.RowCount;
                int colunms = dgCsv.ColumnCount;

                List<string> strList = new List<string>();

                writer.WriteLine("処理時間,メッセージID,処理名,Front種類,Front処理時間(ms),BackEnd種類,BackEnd処理時間(ms),全体処理時間(ms)");

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

        private void button3_Click(object sender, EventArgs e)
        {
            SaveExcel(dataGridView2, "front2");
        }

        private async void btnFeature_Click(object sender, EventArgs e)
        {
            btnFeature.BackColor = System.Drawing.Color.Red;

            await Task.Run(() =>
            {
                DataConvertFeature(File.ReadAllLines(Application.StartupPath + @"\" + FileNameT1 + txtDate.Text, Encoding.GetEncoding("Shift_JIS")), 1);
            });
            _tasksFeature1 = _tasksFeature;
            dataGridView7.DataSource = _tasksFeature;

            await Task.Run(() =>
            {
                DataConvertFeature(File.ReadAllLines(Application.StartupPath + @"\" + FileNameT2 + txtDate.Text, Encoding.GetEncoding("Shift_JIS")), 2);
            });
            _tasksFeature2 = _tasksFeature;
            dataGridView7.BackgroundColor = System.Drawing.Color.Green;
            dataGridView8.DataSource = _tasksFeature;

            await Task.Run(() =>
            {
                DataConvertFeature(File.ReadAllLines(Application.StartupPath + @"\" + FileNameT3 + txtDate.Text, Encoding.GetEncoding("Shift_JIS")), 3);
            });
            _tasksFeature3 = _tasksFeature;
            dataGridView7.BackgroundColor = System.Drawing.Color.Green;
            dataGridView9.DataSource = _tasksFeature;

            await Task.Run(() =>
            {
                DataConvertFeature(File.ReadAllLines(Application.StartupPath + @"\" + FileNameT4 + txtDate.Text, Encoding.GetEncoding("Shift_JIS")), 4);
            });
            _tasksFeature4 = _tasksFeature;
            dataGridView7.BackgroundColor = System.Drawing.Color.Green;
            dataGridView10.DataSource = _tasksFeature;

            await Task.Run(() =>
            {
                DataConvertFeature(File.ReadAllLines(Application.StartupPath + @"\" + FileNameT5 + txtDate.Text, Encoding.GetEncoding("Shift_JIS")), 5);
            });
            _tasksFeature5 = _tasksFeature;
            dataGridView7.BackgroundColor = System.Drawing.Color.Green;
            dataGridView11.DataSource = _tasksFeature;

            await Task.Run(() =>
            {
                DataConvertFeature(File.ReadAllLines(Application.StartupPath + @"\" + FileNameT6 + txtDate.Text, Encoding.GetEncoding("Shift_JIS")), 6);
            });
            _tasksFeature6 = _tasksFeature;
            dataGridView7.BackgroundColor = System.Drawing.Color.Green;
            dataGridView12.DataSource = _tasksFeature;

            btnFeature.BackColor = System.Drawing.Color.Green;
        }


        private List<ItemTime> DataConvertFeature(string[] lineStandard, int CnNumber)
        {
            string key1;
            string key2;
            string key3;
            string key4;

            int targetS;
            int dataCnt = 1;
            int time_S;
            int time_E;
            int messageIS_S, messageIS_E;

            _tasksFeature = new List<ItemTime>();

            foreach (string instr in lineStandard)
            {
                targetS = 0;

                key1 = instr;
                key2 = string.Empty;
                key3 = string.Empty;

                if (!string.IsNullOrEmpty(key1) && key1.IndexOf("messageId=") > 0)
                {
                    key2 = key1.Substring(11, 8);
                    key3 = string.Empty;

                    messageIS_S = key1.IndexOf("messageId=");
                    messageIS_E = key1.IndexOf("] Exec");

                    key4 = key1.Substring(messageIS_S + 10, messageIS_E - (messageIS_S + 10));

                    if (key4.Length > 20)
                    {
                        // messageID
                        //key4;

                        // ProcessTime
                        time_S = key1.IndexOf("Exec time: ");
                        time_E = key1.IndexOf("(ms)");
                        key3 = key1.Substring(time_S + 11, time_E - (time_S + 11));

                        ItemTime taskItem = new ItemTime()
                        {
                            RowNumber = dataCnt++,
                            MessageID = key4,
                            ProcessTime = key3,
                            BackendKind = "CN[" + CnNumber.ToString() + "]",
                        };

                        _tasksFeature.Add(taskItem);
                    }
                }
            }

            lineStandard = null;

            return _tasksFeature;
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            button4.BackColor = Color.Red;

            // CSV1
            await Task.Run(() =>
            {
                CSV_output(_tmpList1);
            });

            button4.BackColor = Color.Yellow;

            // CSV2
            await Task.Run(() =>
            {
                CSV_output(_tmpList2);
            });

            button4.BackColor = Color.Green;
        }

        private void CSV_output(List<ItemCsv> targetList)
        {
            string msgID = string.Empty;
            List<ItemTime> targetData = null;
            _tasksFeatureTotal = new List<ItemTime>();

            _tasksFeatureTotal.AddRange(_tasksFeature1);
            _tasksFeatureTotal.AddRange(_tasksFeature2);
            _tasksFeatureTotal.AddRange(_tasksFeature3);
            _tasksFeatureTotal.AddRange(_tasksFeature4);
            _tasksFeatureTotal.AddRange(_tasksFeature5);
            _tasksFeatureTotal.AddRange(_tasksFeature6);

            // CSV1
            foreach (ItemCsv data in targetList)
            {
                targetData = null;
                msgID = data.MessageID;
                if (!string.IsNullOrEmpty(msgID))
                {
                    targetData = _tasksFeatureTotal.Where(x => x.MessageID == msgID).ToList();
                    if (targetData != null && targetData.Count > 0)
                    {
                        data.BackEndTime = targetData[targetData.Count - 1].ProcessTime;
                        data.BackEndKind = targetData[targetData.Count - 1].BackendKind;
                        data.FrontTime = (Convert.ToInt32(data.TotalTime) - Convert.ToInt32(data.BackEndTime)).ToString();
                    }
                }
            }
        }

        private void FrontCountParse(string key1, ref int tarA, ref int tarB)
        {
            if (!string.IsNullOrEmpty(key1) && key1.IndexOf("Start remote call") > -1)
            {
                // Start処理
                tarA++;
            }
            else if (!string.IsNullOrEmpty(key1) && key1.IndexOf("(ms)") > -1)
            {
                // End処理
                tarB++;
            }
        }

        private List<ItemFrontCount> FrontCountCSV(string[] lineStandard)
        {
            string key1;
            string key2;
            string key3;

            _tasksCount = new List<ItemFrontCount>();
            int[] cntStart = new int[12];
            int[] cntEnd = new int[12];

            foreach (string instr in lineStandard)
            {
                key1 = instr;
                key2 = string.Empty;
                key3 = string.Empty;

                // スタート時間
                //keyTime = key1.Substring(11, 8);
                int target = Convert.ToInt32(key1.Substring(14, 2));

                if (0 <= target && target <= 5)
                {
                    FrontCountParse(key1, ref cntStart[0], ref cntEnd[0]);
                }
                else if (6 <= target && target <= 10)
                {
                    FrontCountParse(key1, ref cntStart[1], ref cntEnd[1]);
                }
                else if (11 <= target && target <= 15)
                {
                    FrontCountParse(key1, ref cntStart[2], ref cntEnd[2]);
                }
                else if (16 <= target && target <= 20)
                {
                    FrontCountParse(key1, ref cntStart[3], ref cntEnd[3]);
                }
                else if (21 <= target && target <= 25)
                {
                    FrontCountParse(key1, ref cntStart[4], ref cntEnd[4]);
                }
                else if (26 <= target && target <= 30)
                {
                    FrontCountParse(key1, ref cntStart[5], ref cntEnd[5]);
                }
                else if (31 <= target && target <= 35)
                {
                    FrontCountParse(key1, ref cntStart[6], ref cntEnd[6]);
                }
                else if (36 <= target && target <= 40)
                {
                    FrontCountParse(key1, ref cntStart[7], ref cntEnd[7]);
                }
                else if (41 <= target && target <= 45)
                {
                    FrontCountParse(key1, ref cntStart[8], ref cntEnd[8]);
                }
                else if (46 <= target && target <= 50)
                {
                    FrontCountParse(key1, ref cntStart[9], ref cntEnd[9]);
                }
                else if (51 <= target && target <= 55)
                {
                    FrontCountParse(key1, ref cntStart[10], ref cntEnd[10]);
                }
                else if (56 <= target && target <= 59)
                {
                    FrontCountParse(key1, ref cntStart[11], ref cntEnd[11]);
                }
            }

            ItemFrontCount taskCountItem = new ItemFrontCount()
            {
                RowComment = "00～05",
                CountIN = cntStart[0].ToString(),
                CountOUT = cntEnd[0].ToString(),
            };
            _tasksCount.Add(taskCountItem);
            taskCountItem = new ItemFrontCount()
            {
                RowComment = "06～10",
                CountIN = cntStart[1].ToString(),
                CountOUT = cntEnd[1].ToString(),
            };
            _tasksCount.Add(taskCountItem);
            taskCountItem = new ItemFrontCount()
            {
                RowComment = "11～15",
                CountIN = cntStart[2].ToString(),
                CountOUT = cntEnd[2].ToString(),
            };
            _tasksCount.Add(taskCountItem);
            taskCountItem = new ItemFrontCount()
            {
                RowComment = "16～20",
                CountIN = cntStart[3].ToString(),
                CountOUT = cntEnd[3].ToString(),
            };
            _tasksCount.Add(taskCountItem);
            taskCountItem = new ItemFrontCount()
            {
                RowComment = "21～25",
                CountIN = cntStart[4].ToString(),
                CountOUT = cntEnd[4].ToString(),
            };
            _tasksCount.Add(taskCountItem);
            taskCountItem = new ItemFrontCount()
            {
                RowComment = "26～30",
                CountIN = cntStart[5].ToString(),
                CountOUT = cntEnd[5].ToString(),
            };
            _tasksCount.Add(taskCountItem);
            taskCountItem = new ItemFrontCount()
            {
                RowComment = "31～35",
                CountIN = cntStart[6].ToString(),
                CountOUT = cntEnd[6].ToString(),
            };
            _tasksCount.Add(taskCountItem);
            taskCountItem = new ItemFrontCount()
            {
                RowComment = "36～40",
                CountIN = cntStart[7].ToString(),
                CountOUT = cntEnd[7].ToString(),
            };
            _tasksCount.Add(taskCountItem);
            taskCountItem = new ItemFrontCount()
            {
                RowComment = "41～45",
                CountIN = cntStart[8].ToString(),
                CountOUT = cntEnd[8].ToString(),
            };
            _tasksCount.Add(taskCountItem);
            taskCountItem = new ItemFrontCount()
            {
                RowComment = "46～50",
                CountIN = cntStart[9].ToString(),
                CountOUT = cntEnd[9].ToString(),
            };
            _tasksCount.Add(taskCountItem);
            taskCountItem = new ItemFrontCount()
            {
                RowComment = "51～55",
                CountIN = cntStart[10].ToString(),
                CountOUT = cntEnd[10].ToString(),
            };
            _tasksCount.Add(taskCountItem);
            taskCountItem = new ItemFrontCount()
            {
                RowComment = "56～59",
                CountIN = cntStart[11].ToString(),
                CountOUT = cntEnd[11].ToString(),
            };
            _tasksCount.Add(taskCountItem);

            return _tasksCount;
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                FrontCountCSV(File.ReadAllLines(Application.StartupPath + @"\" + FileNameFrontTime1, Encoding.GetEncoding("Shift_JIS")));
            });
            dataGridView3.DataSource = _tasksCount;

            // CSV出力
            TimeResultSaveExcel(dataGridView3, 1);
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                FrontCountCSV(File.ReadAllLines(Application.StartupPath + @"\" + FileNameFrontTime2, Encoding.GetEncoding("Shift_JIS")));
            });
            dataGridView4.DataSource = _tasksCount;

            // CSV出力
            TimeResultSaveExcel(dataGridView4, 2);
        }

        /// <summary>
        /// Time集計分析結果をCSV出力
        /// </summary>
        public void TimeResultSaveExcel(DataGridView targetGrid, int Number)
        {
            SaveFileDialog save = new SaveFileDialog
            {
                Title = "ファイルを保存する",
                InitialDirectory = @"D:\",
                FileName = @"FrontCount" + Number.ToString() + ".csv",
                Filter = "CSV(カンマ付き)|*.csv"
            };

            DialogResult output = save.ShowDialog();

            using (StreamWriter writer = new StreamWriter(save.FileName, false, Encoding.GetEncoding("shift_Jis")))
            {
                int row = targetGrid.RowCount;
                int colunms = targetGrid.ColumnCount;

                List<string> strList = new List<string>();

                for (int k = 0; k < colunms; k++)
                {
                    strList.Add(targetGrid.Columns[k].HeaderCell.Value.ToString());
                }

                string[] strHeader = strList.ToArray();

                string strCsvData2 = string.Join(",", strHeader);

                for (int i = 0; i < row; i++)
                {
                    strList = new List<string>();

                    for (int j = 0; j < colunms; j++)
                    {
                        if (targetGrid[j, i].Value != null)
                        {
                            strList.Add(targetGrid[j, i].Value.ToString());
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
