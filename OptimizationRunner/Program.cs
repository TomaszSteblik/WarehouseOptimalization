using System.IO;
using Newtonsoft.Json;
using Optimization;

namespace OptimizationRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            OptimizationParameters optimizationParameters =
                JsonConvert.DeserializeObject<OptimizationParameters>(File.ReadAllText(args[0]));
            switch (optimizationParameters.Mode)
            {
                case Mode.DistancesMode:
                    FindShortestPath.Find(optimizationParameters);
                    break;
                case Mode.WarehouseMode:
                    Warehouse.Optimizer(optimizationParameters);
                    break;
                default:
                    return;
            }
        }
    }
}