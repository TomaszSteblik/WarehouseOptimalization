using System;
using System.Threading;
using System.Threading.Tasks;
using Optimization.GeneticAlgorithms;
using Optimization.GeneticAppliances;
using Optimization.GeneticAppliances.TSP;
using Optimization.GeneticAppliances.Warehouse;
using Optimization.Helpers;
using Optimization.Parameters;

namespace Optimization
{
    public static class OptimizationWork
    {
        public static double FindShortestPath(OptimizationParameters optimizationParameters, int seed = 0)
        {
            seed = GetSeed(seed);
            var random = new Random(seed);
            var matrix = Files.ReadArray(optimizationParameters.DataPath);
            Distances.Create(matrix);
            return PathFinding.ShortestPath.Find(PointsArrayGenerator.GeneratePointsToVisit(matrix.Length), optimizationParameters, CancellationToken.None, random);
            
        }
        
        public static double FindShortestPath(OptimizationParameters optimizationParameters, CancellationToken ct, int seed = 0)
        {
            seed = GetSeed(seed);
            var random = new Random(seed);
            var matrix = Files.ReadArray(optimizationParameters.DataPath);
            Distances.Create(matrix);
            return PathFinding.ShortestPath.Find(PointsArrayGenerator.GeneratePointsToVisit(matrix.Length), optimizationParameters, ct, random);
            
        }
        
        public static TSPResult TSP(OptimizationParameters optimizationParameters, CancellationToken ct, int seed = 0)
        {
            seed = GetSeed(seed);
            var random = new Random(seed);
            
            var matrix = Files.ReadArray(optimizationParameters.DataPath);
            Distances.Create(matrix);
            var tsp = new GeneticTSP(PointsArrayGenerator.GeneratePointsToVisit(matrix.Length), optimizationParameters,
                (population) =>
                {
                    double[] fitness = new double[population.Length];
                    for (int i = 0; i < population.Length; i++)
                        fitness[i] = Fitness.CalculateFitness(population[i]);
                    return fitness;
                }, ct, random);
            var result = tsp.Run();
            result.Seed = seed;
            return result;
            
        }

        public static void FindShortestPath(OptimizationParameters optimizationParameters,
            DelegateFitness.CalcFitness calcFitness, int seed = 0)
        {
            seed = GetSeed(seed);
            var random = new Random(seed);
            var matrix = Files.ReadArray(optimizationParameters.DataPath);
            Distances.Create(matrix);
            PathFinding.ShortestPath.Find(PointsArrayGenerator.GeneratePointsToVisit(matrix.Length), optimizationParameters,calcFitness, CancellationToken.None, random);
        }

        public static WarehouseResult WarehouseOptimization(WarehouseParameters warehouseParameters, CancellationToken ct, int seed = 0)
        {
            seed = GetSeed(seed);
            var random = new Random(seed);
            return WarehouseOptimizer.Optimize(warehouseParameters, ct, random);
        }

        public static void KeyboardOptimization(OptimizationParameters optimizationParameters, int seed = 0)
        {
            seed = GetSeed(seed);
            var random = new Random(seed);
            var keyboardOptimizer = new GeneticKeyboard(optimizationParameters, random);
            var result = keyboardOptimizer.Run();
            keyboardOptimizer.WriteResult(result);
        }

        private static int GetSeed(int givenValue)
        {
            if (givenValue == 0)
            {
                return (int) DateTime.Now.Ticks;
            }

            return givenValue;
        }
        
    }
}