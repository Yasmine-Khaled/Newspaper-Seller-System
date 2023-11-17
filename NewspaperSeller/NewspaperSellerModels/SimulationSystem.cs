using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewspaperSellerModels
{
    public class SimulationSystem
    {
        public Random random = new Random();

        public SimulationSystem()
        {
            DayTypeDistributions = new List<DayTypeDistribution>();
            DemandDistributions = new List<DemandDistribution>();
            SimulationTable = new List<SimulationCase>();
            PerformanceMeasures = new PerformanceMeasures();
        }

        ///////////// INPUTS /////////////
        public int NumOfNewspapers { get; set; }
        public int NumOfRecords { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal ScrapPrice { get; set; }
        public decimal UnitProfit { get; set; }
        public List<DayTypeDistribution> DayTypeDistributions { get; set; }
        public List<DemandDistribution> DemandDistributions { get; set; }

        ///////////// OUTPUTS /////////////
        public List<SimulationCase> SimulationTable { get; set; }
        public PerformanceMeasures PerformanceMeasures { get; set; }

        public void start()
        {
            for (int i = 0; i < NumOfRecords; i++)
            {
                SimulationCase simulationCase = new SimulationCase();

                // day Column
                simulationCase.DayNo = i + 1;

                // Rondom Digits for type of newsday
                int randomDayTypeNumber = random.Next(1, 101);
                simulationCase.RandomNewsDayType = randomDayTypeNumber;

                // Type of Newsday
                simulationCase.NewsDayType = generateRandomDay(randomDayTypeNumber);

                // Rondom Digits for demand
                int randomDemandNumber = random.Next(1, 101);
                simulationCase.RandomDemand = randomDemandNumber;

                // Demand
                simulationCase.Demand = generateRandomDemand(randomDemandNumber, simulationCase.NewsDayType);

                // Daily Cost
                simulationCase.DailyCost = NumOfNewspapers * PurchasePrice;

                // Revenue from sales
                simulationCase.SalesProfit = calculateRevenuFromSales(simulationCase.Demand);

                // Lost profit from excess demand
                if (simulationCase.Demand > NumOfNewspapers)
                {
                    simulationCase.LostProfit = calculateLostProfit(simulationCase.Demand);
                }
                else
                {
                    simulationCase.LostProfit = 0;
                }

                // Salvage From sale of scrape
                if (NumOfNewspapers > simulationCase.Demand)
                {
                    simulationCase.ScrapProfit = calculateSalvage(simulationCase.Demand);
                }
                else
                {
                    simulationCase.ScrapProfit = 0;
                }

                // Daily profit
                simulationCase.DailyNetProfit = calculateDailyCost(simulationCase.SalesProfit, simulationCase.DailyCost, simulationCase.LostProfit, simulationCase.ScrapProfit);

                SimulationTable.Add(simulationCase);
            }
        }

        public Enums.DayType generateRandomDay(int randomDayTypeNumber)
        {
            foreach (DayTypeDistribution day in DayTypeDistributions)
            {
                if (randomDayTypeNumber >= day.MinRange && randomDayTypeNumber <= day.MaxRange)
                {
                    return day.DayType;
                }
            }

            return 0;
        }

        public int generateRandomDemand(int randomDemandNumber, Enums.DayType dayType)
        {
            foreach (DemandDistribution demand in DemandDistributions)
            {
                foreach (DayTypeDistribution day in demand.DayTypeDistributions)
                {
                    if (dayType == day.DayType && randomDemandNumber >= day.MinRange && randomDemandNumber <= day.MaxRange)
                    {
                        return demand.Demand;
                    }
                }
            }
            return 0;
        }

        public decimal calculateRevenuFromSales(int demand)
        {
            decimal cost = 0;
            if (demand <= NumOfNewspapers)
            {
                cost = demand * SellingPrice;
            }
            else
            {
                cost = NumOfNewspapers * SellingPrice;
            }

            return cost;
        }

        public decimal calculateLostProfit(int demand)
        {
            return (demand - NumOfNewspapers) * (SellingPrice - PurchasePrice);
        }

        public decimal calculateSalvage(int demand)
        {
            return (NumOfNewspapers - demand) * ScrapPrice;
        }

        public decimal calculateDailyCost(decimal salesProfit, decimal costOfNewsPapers, decimal lostProfit, decimal salvage)
        {
            return salesProfit - costOfNewsPapers - lostProfit + salvage;
        }
    }
}
