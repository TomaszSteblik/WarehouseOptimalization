using System.IO;
using Newtonsoft.Json;
using OptimizationIO;

namespace OptimizationRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            OptimizationParameters optimizationParameters =
                JsonConvert.DeserializeObject<OptimizationParameters>(File.ReadAllText(args[0]));
            
            Optimization.FindShortestPath.Find(optimizationParameters);
        }
    }
}