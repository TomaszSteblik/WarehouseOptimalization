using System.IO;
using Newtonsoft.Json;
using Optimization;
using System;
using System.Diagnostics;
using Optimization.DistanceMode;
using Optimization.WarehouseMode;

namespace OptimizationRunner
{
    class Program
    {
        static void Main(string[] args)
        {

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            string jsonS = File.ReadAllText(args.Length > 0 ? args[0] : @"E:\Warehouse\WarehouseOptimization\parameters23.json") ;
            OptimizationParameters optimizationParameters = JsonConvert.DeserializeObject<OptimizationParameters>(jsonS);
            switch (optimizationParameters.Mode)
            {
                case Mode.DistancesMode:
                    var matrix = Files.ReadArray(optimizationParameters.DataPath);
                    FindShortestPath.Find(PointsArrayGenerator.GeneratePointsToVisit(matrix.Length), matrix, optimizationParameters);
                    break;
                case Mode.WarehouseMode:                   
                    Warehouse.Optimizer(optimizationParameters);
                    break;
                default:
                    return;
            }

            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
            Console.ReadLine();
        }
    }
}