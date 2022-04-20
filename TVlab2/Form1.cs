using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace TVlab2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "{0:0.000}";
            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "{0:0.000}";
            chart1.Series[1].BorderWidth = 4;
            chart1.Series[2].BorderWidth = 4;
            //Инициализируем переменные
            double A = double.Parse(textBox3.Text);
            int V = int.Parse(textBox1.Text);
            int Nsections = int.Parse(textBox2.Text);

            double koefDouble = 0;
            //if (mark.Checked == true) 
            //koefDouble = double.Parse(textBox5.Text);

            double[] RandArray = new double[V];

            Random random = new Random();

            for (int i = 0; i < V; i++)
            {
                RandArray[i] = Math.Round(-(1 / A) * Math.Log(random.NextDouble()), 6);
            }

            double min = RandArray.Min();
            double max = RandArray.Max();

            //Промежуточные вычисления
            double h = Math.Abs((max - min)) / Nsections;

            h = Math.Round(h, 3);

            //Создаём и заполняем массив дискретных значений (точек) по оси x
            double[] PointsX = new double[Nsections + 1];

            for (int i = 0; i < PointsX.Length; i++)
            {
                PointsX[i] = min + i * h;
            }

            //Создаём и заполняем массив попаданий случайных величин в каждый из интервалов
            double[] hits = new double[Nsections];

            for (int i = 0; i < V; i++)
            {
                for (int j = 0; j < Nsections; j++)
                {
                    if (RandArray[i] > PointsX[j] && RandArray[i] <= PointsX[j + 1]) hits[j]++;
                }
            }

            for (int i = 0; i < hits.Length; i++)
            {
                hits[i] = hits[i] / V / h;
            }

            for (int i = 0; i < hits.Length; i++)
            {
                hits[i] = Math.Round(hits[i], 3);
            }

            //Выводим гистограмму
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart1.Series[2].Points.Clear();

            //Подсчитываем Ymax
            double Ymax = 0;
            List<double> Y = new List<double>();

            for (double i = min; i <= max; i += h / 10)
            {
                Y.Add(A * Math.Pow(2.718, (-A * i)));
            }

            if (Y.Max() > hits.Max()) Ymax = Y.Max();
            else Ymax = hits.Max();

            chart1.ChartAreas[0].AxisY.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Maximum = Ymax;
            chart1.ChartAreas[0].AxisX.Minimum = min;
            chart1.ChartAreas[0].AxisX.Maximum = max;

            //Выводим гистограмму
            for (int i = 0; i < Nsections; i++)
            {
                this.chart1.Series[0].Points.AddXY((min + i * h), hits[i]);
            }

            int k = 0;
            //Выводим плотность распределения
            for (double i = min; i <= max; i += h / 10)
            {
                this.chart1.Series[2].Points.AddXY(i, Y[k]);
                k++;
            }
        }
    }
}
