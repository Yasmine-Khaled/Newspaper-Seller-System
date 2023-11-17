using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewspaperSellerModels
{
    public class GetData
    {
        public int NumOfNewspapers { get; set; }
        public int NumOfRecords { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal ScrapPrice { get; set; }
        public List<DayTypeDistribution> DayTypeDistributions { get; set; }
        public List<DemandDistribution> DemandDistributions { get; set; }

        public GetData(string path)
        {
            DayTypeDistributions = new List<DayTypeDistribution>();
            DemandDistributions = new List<DemandDistribution>();

            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName.Replace('\\', '/');
            string testDirectory = projectDirectory + "/NewspaperSellerSimulation/TestCases/";
            path = Path.Combine(testDirectory, path);

            string[] lines = File.ReadAllLines(path);
            get_numbers(lines);
            fill_DayTypeDistributions(lines);
            fill_DemandDistributions(lines);
        }

        void get_numbers(string[] lines)
        {
            var configMap = new Dictionary<string, Action<string>>
            {
                { "NumOfNewspapers", value => this.NumOfNewspapers = int.Parse(value) },
                { "NumOfRecords", value => this.NumOfRecords = int.Parse(value) },
                { "PurchasePrice", value => this.PurchasePrice = decimal.Parse(value) },
                { "SellingPrice", value => this.SellingPrice = decimal.Parse(value) },
                { "ScrapPrice", value => this.ScrapPrice = decimal.Parse(value) }
            };
            for (int i = 0; i < lines.Length; i++)
            {
                if (configMap.TryGetValue(lines[i], out Action<string> setProperty))
                {
                    setProperty(lines[i + 1]);
                    i++;
                }
            }
        }

        void fill_DayTypeDistributions(string[] lines)
        {
            string[] line = lines[16].Split(',');
            decimal sum = 0;
            for (int j=0; j<line.Length; j++)
            {
                DayTypeDistribution dist = new DayTypeDistribution();
                dist.Probability = decimal.Parse(line[j]);
                dist.MinRange = (int)(sum * 100 + 1);
                sum += dist.Probability;
                dist.MaxRange = (int)(sum * 100);
                dist.DayType = (Enums.DayType)(j % 3);
                dist.CummProbability = sum;
                DayTypeDistributions.Add(dist);
            }
        }

        void fill_DemandDistributions(string[] lines)
        {
            for (int i=19, idx=0; i<lines.Length; i++, idx++)
            {
                string[] line = lines[i].Split(',');
                DemandDistribution tmp = new DemandDistribution();
                tmp.Demand = int.Parse(line[0]);
                for (int j=1; j<4; j++)
                {
                    DayTypeDistribution dist = new DayTypeDistribution();
                    dist.Probability = decimal.Parse(line[j]);
                    if (idx > 0)
                    {
                        if (this.DemandDistributions[idx - 1].DayTypeDistributions[j - 1].MaxRange == 100 || this.DemandDistributions[idx - 1].DayTypeDistributions[j - 1].MaxRange == -1)
                        {
                            dist.MinRange = -1;
                            dist.MaxRange = -1;
                        }
                        else
                        {
                            dist.MinRange = this.DemandDistributions[idx - 1].DayTypeDistributions[j - 1].MaxRange + 1;
                            dist.CummProbability = this.DemandDistributions[idx - 1].DayTypeDistributions[j - 1].CummProbability + dist.Probability;
                            dist.MaxRange = (int)(dist.CummProbability * 100);
                        }
                    }
                    else
                    {
                        dist.MinRange = 1;
                        dist.CummProbability = dist.Probability;
                        dist.MaxRange = (int)(dist.CummProbability * 100);
                    }
                    dist.DayType = (Enums.DayType)((j - 1) % 3);
                    tmp.DayTypeDistributions.Add(dist);
                }
                DemandDistributions.Add(tmp);
            }
        }

        public void FillSystem (ref SimulationSystem sys)
        {
            sys.NumOfNewspapers = NumOfNewspapers;
            sys.NumOfRecords = NumOfRecords;
            sys.PurchasePrice = PurchasePrice;
            sys.ScrapPrice = ScrapPrice;
            sys.SellingPrice = SellingPrice;
            sys.DayTypeDistributions = DayTypeDistributions;
            sys.DemandDistributions = DemandDistributions;
        }

        public override string ToString()
        {
            Console.WriteLine("----------------\nDayTypeDistributions\n----------------");
            foreach (DayTypeDistribution d in this.DayTypeDistributions)
            {
                Console.WriteLine($"DayType: {d.DayType}, Probability: {d.Probability}, CummProbability: {d.CummProbability}, MinRange: {d.MinRange}, MaxRange: {d.MaxRange}");
            }
            Console.WriteLine("________________________________");
            Console.WriteLine("----------------\nDemandDistributions\n----------------");
            foreach (DemandDistribution t in this.DemandDistributions)
            {
                Console.WriteLine($"demand = {t.Demand}");
                foreach (DayTypeDistribution d in t.DayTypeDistributions)
                {
                    Console.WriteLine($"DayType: {d.DayType}, Probability: {d.Probability}, CummProbability: {d.CummProbability}, MinRange: {d.MinRange}, MaxRange: {d.MaxRange}");
                }
                Console.WriteLine("----------------------");
            }
            Console.WriteLine("________________________________");
            return $"NumOfNewspapers: {NumOfNewspapers}, NumOfRecords: {NumOfRecords}, " +
                   $"PurchasePrice: {PurchasePrice}, SellingPrice: {SellingPrice}, ScrapPrice: {ScrapPrice}\n\n\n";
        }
    }
}
