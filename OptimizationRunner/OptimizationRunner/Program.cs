using System;
using Optimization;
using OptimizationIO;

namespace OptimizationRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            //GeneticAlgorithm.GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm.GeneticAlgorithm();
            //geneticAlgorithm.Go(100, 20, "/home/rtry/WarehouseOptimization/distances-usa.txt");
            FindShortestPath.Find(new OptimizationParameters()
                {
                    OptimizationMethod = OptimizationMethod.NearestNeighbor,
                    Use2opt = true,
                    DataPath = "/home/rtry/WarehouseOptimization/distances-usa.txt",
                    ResultPath = "/home/rtry/WarehouseOptimization/result.txt",
                    LogPath = "/home/rtry/WarehouseOptimization/log.txt",
                    StartingId = 0
                });
        }
    }
}