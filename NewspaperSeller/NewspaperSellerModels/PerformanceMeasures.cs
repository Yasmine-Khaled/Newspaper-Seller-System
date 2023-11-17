using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewspaperSellerModels
{
    public class PerformanceMeasures
    {
        public decimal TotalSalesProfit { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalLostProfit { get; set; }
        public decimal TotalScrapProfit { get; set; }
        public decimal TotalNetProfit { get; set; }
        public int DaysWithMoreDemand { get; set; }
        public int DaysWithUnsoldPapers { get; set; }

        decimal total_sales_profit(List<SimulationCase> SimulationTable)
        {
            decimal Total_sales_profit = 0;

            foreach (SimulationCase simulationCase in SimulationTable)
            {
                Total_sales_profit += simulationCase.SalesProfit;
            }

            return Total_sales_profit;
        }
        decimal total_cost(List<SimulationCase> SimulationTable)
        {
            decimal Total_cost = 0;

            int n_days = SimulationTable.Count;
            Total_cost = SimulationTable[0].DailyCost * n_days;

            return Total_cost;
        }
        decimal total_lostProfit(List<SimulationCase> SimulationTable)
        {
            decimal Total_lostProfit = 0;

            foreach (SimulationCase simulationCase in SimulationTable)
            {
                Total_lostProfit += simulationCase.LostProfit;
            }

            return Total_lostProfit;
        }
        decimal total_ScrapProfit(List<SimulationCase> SimulationTable)
        {
            decimal Total_ScrapProfit = 0;

            foreach (SimulationCase simulationCase in SimulationTable)
            {
                Total_ScrapProfit += simulationCase.ScrapProfit;
            }

            return Total_ScrapProfit;
        }
        decimal Total_netprofit(decimal total_sales_profit, decimal total_cost, decimal total_lost_profit, decimal total_scrap_profit)
        {
            decimal Total_NetProfit = 0;
            Total_NetProfit = total_sales_profit - total_cost - total_lost_profit + total_scrap_profit;

            //  Total_NetProfit = total_sales_profit(SimulationTable) - total_cost(SimulationTable) - total_lostProfit(SimulationTable) + total_ScrapProfit(SimulationTable);

            return Total_NetProfit;
        }

        int NDaysExceedDemand(List<SimulationCase> SimulationTable)
        {
            int NDays = 0;
            foreach (SimulationCase simulationCase in SimulationTable)
            {
                if (simulationCase.LostProfit > 0)
                    NDays++;
            }

            return NDays;
        }
        int NDaysOfScrap(List<SimulationCase> SimulationTable)
        {
            int NDays = 0;
            foreach (SimulationCase simulationCase in SimulationTable)
            {
                if (simulationCase.ScrapProfit > 0)
                    NDays++;
            }

            return NDays;
        }
        public void CalcPerformace(List<SimulationCase> SimulationTable)
        {
            TotalSalesProfit = total_sales_profit(SimulationTable);
            TotalCost = total_cost(SimulationTable);
            TotalLostProfit = total_lostProfit(SimulationTable);
            TotalScrapProfit = total_ScrapProfit(SimulationTable);
            TotalNetProfit = Total_netprofit(TotalSalesProfit, TotalCost, TotalLostProfit, TotalScrapProfit);

            DaysWithMoreDemand = NDaysExceedDemand(SimulationTable);
            DaysWithUnsoldPapers = NDaysOfScrap(SimulationTable);
        }

    }
}
