using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace RacuosServerInfo6
{
    public partial class Form2 : Form
    {
        DataGridView dgTarget;
        int tartgetCnNumber;
        int time;

        public Form2(DataGridView dgTarget, int tartgetCnNumber, int time)
        {
            InitializeComponent();
            this.dgTarget = dgTarget;
            this.tartgetCnNumber = tartgetCnNumber;
            this.time = time;
        }

        /// <summary>
        /// ChartControl
        /// </summary>
        /// <param name="dgTarget"></param>
        /// <param name="tartgetCnNumber"></param>
        /// <param name="time"></param>
        private void ChartControl(DataGridView dgTarget, int tartgetCnNumber, int time)
        {
            // clear
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            chart1.Titles.Clear();

            Title title1 = new Title("【CN" + tartgetCnNumber.ToString() + "】");

            // series
            Series seriesLine = new Series();
            seriesLine.ChartType = SeriesChartType.Line;
            seriesLine.LegendText = string.Empty;
            seriesLine.BorderWidth = 1;
            seriesLine.MarkerStyle = MarkerStyle.Circle;
            seriesLine.MarkerSize = 2;

            for (int i = 0; i < dgTarget.RowCount; i++)
            {
                if (!string.IsNullOrEmpty(dgTarget.Rows[i].Cells[2].Value.ToString()) &&
                    Convert.ToInt32(dgTarget.Rows[i].Cells[2].Value) > time)
                {
                    var data = new DataPoint();
                    data.SetValueXY(dgTarget.Rows[i].Cells[1].Value, dgTarget.Rows[i].Cells[2].Value);
                    seriesLine.Points.Add(data);
                }
            }

            // chartarea
            ChartArea area1 = new ChartArea();
            area1.AxisX.Title = "処理時間";
            area1.AxisY.Title = "経過時間";

            chart1.Titles.Add(title1);
            chart1.ChartAreas.Add(area1);
            chart1.Series.Add(seriesLine);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            ChartControl(this.dgTarget, this.tartgetCnNumber, this.time);
        }
    }
}
