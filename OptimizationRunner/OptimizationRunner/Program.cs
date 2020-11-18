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
                    DataPath = "../../../../../distances-usa.txt",
                    ResultPath = "../../../../../result.txt",
                    LogPath = "../../../../../log.txt",
                    StartingId = 0
                });
        }
    }
}