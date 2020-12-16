using System.IO;
using Newtonsoft.Json;
using Optimization;
using System;
using System.Diagnostics;

namespace OptimizationRunner
{
    class Program
    {
        static void Main(string[] args)
        {

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Warehouse.Event1 += Warehouse_Event1;

            string jsonS = File.ReadAllText(args.Length > 0 ? args[0] : @"E:\Warehouse\WarehouseOptimization-master\parameters2.json") ;
            OptimizationParameters optimizationParameters = JsonConvert.DeserializeObject<OptimizationParameters>(jsonS);
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

            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
            Console.ReadLine();
        }

        private static void Warehouse_Event1(object sender, System.EventArgs e)
        {
            Console.WriteLine(Warehouse.E);
        }
    }
}