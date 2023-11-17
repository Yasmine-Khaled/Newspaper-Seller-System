using NewspaperSellerModels;
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

namespace NewspaperSellerSimulation
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fileLocation = "";
            try
            {

                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "txt files(*.txt)|*.txt";

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    fileLocation = dialog.FileName.Replace('\\', '/');
                    //MessageBox.Show("File uploaded");
                    Program.init(fileLocation, false);
                }
                getPerformanceSystem();
                getSimulationTable();

            }
            catch (Exception error)
            {
                MessageBox.Show("An error occured while uploading your file. " + error.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        void getPerformanceSystem()
        {
            PerformanceMeasures measures = Program.system.PerformanceMeasures;
            label5.Text = measures.TotalSalesProfit.ToString();
            label7.Text=measures.TotalCost.ToString();
            label9.Text = measures.TotalLostProfit.ToString();
            label11.Text=measures.TotalScrapProfit.ToString();
            label13.Text=measures.TotalNetProfit.ToString();
            label15.Text=measures.DaysWithMoreDemand.ToString();
            label17.Text=measures.DaysWithUnsoldPapers.ToString();
        }
        void getSimulationTable()
        {
            List<SimulationCase> tableData = Program.system.SimulationTable;
            dataGridView1.Rows.Clear();
            foreach (SimulationCase simulationCase in tableData)
            {
                dataGridView1.Rows.Add(
                    simulationCase.DayNo,
                    simulationCase.RandomNewsDayType,
                    simulationCase.NewsDayType,
                    simulationCase.RandomDemand,
                    simulationCase.Demand,
                    simulationCase.DailyCost,
                    simulationCase.LostProfit,
                    simulationCase.ScrapProfit,
                    simulationCase.DailyNetProfit
                );
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {



          
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
