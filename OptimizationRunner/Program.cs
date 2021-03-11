using System.IO;
using Newtonsoft.Json;
using OptimizationMethods;
using System;
using System.Diagnostics;
using OptimizationMethods.Parameters;

namespace OptimizationRunner
{
    class Program
    {
        static void Main(string[] args)
        {

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            string jsonS = File.ReadAllText(args.Length > 0 ? args[0] : @"C:/Users/rtry/RiderProjects/WarehouseOptimization/parameters.json") ;
            OptimizationParameters optimizationParameters = JsonConvert.DeserializeObject<OptimizationParameters>(jsonS);
            
            string jsonSe = File.ReadAllText(args.Length > 0 ? args[0] : @"C:/Users/rtry/RiderProjects/WarehouseOptimization/warehouseParameters.json") ;
            WarehouseParameters warehouseParameters = JsonConvert.DeserializeObject<WarehouseParameters>(jsonSe);
            
            //Optimization.FindShortestPath(optimizationParameters);
            Optimization.WarehouseOptimization(warehouseParameters);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
            Console.ReadLine();
        }
    }
}