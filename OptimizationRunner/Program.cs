using System.IO;
using Newtonsoft.Json;
using Optimization;

namespace OptimizationRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            Warehouse.Optimizer(JsonConvert.DeserializeObject<OptimizationParameters>(File.ReadAllText(args[0])));
            FindShortestPath.Find(JsonConvert.DeserializeObject<OptimizationParameters>(File.ReadAllText(args[0])));
        }
    }
}