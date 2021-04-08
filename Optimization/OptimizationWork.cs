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
        public static double FindShortestPath(OptimizationParameters optimizationParameters)
        {
            var matrix = Files.ReadArray(optimizationParameters.DataPath);
            Distances.Create(matrix);
            return PathFinding.ShortestPath.Find(PointsArrayGenerator.GeneratePointsToVisit(matrix.Length), optimizationParameters, CancellationToken.None);
            
        }
        
        public static double FindShortestPath(OptimizationParameters optimizationParameters, CancellationToken ct)
        {
            var matrix = Files.ReadArray(optimizationParameters.DataPath);
            Distances.Create(matrix);
            return PathFinding.ShortestPath.Find(PointsArrayGenerator.GeneratePointsToVisit(matrix.Length), optimizationParameters, ct);
            
        }
        
        public static TSPResult TSP(OptimizationParameters optimizationParameters, CancellationToken ct)
        {
            var matrix = Files.ReadArray(optimizationParameters.DataPath);
            Distances.Create(matrix);
            var tsp = new GeneticTSP(PointsArrayGenerator.GeneratePointsToVisit(matrix.Length), optimizationParameters,
                (population) =>
                {
                    double[] fitness = new double[population.Length];
                    for (int i = 0; i < population.Length; i++)
                        fitness[i] = Fitness.CalculateFitness(population[i]);
                    return fitness;
                }, ct);
            return tsp.Run();
            
        }

        public static void FindShortestPath(OptimizationParameters optimizationParameters,
            DelegateFitness.CalcFitness calcFitness)
        {
            var matrix = Files.ReadArray(optimizationParameters.DataPath);
            Distances.Create(matrix);
            PathFinding.ShortestPath.Find(PointsArrayGenerator.GeneratePointsToVisit(matrix.Length), optimizationParameters,calcFitness, CancellationToken.None);
        }

        public static double WarehouseOptimization(WarehouseParameters warehouseParameters, CancellationToken ct)
        {
            return WarehouseOptimizer.Optimize(warehouseParameters, ct);
        }

        public static void KeyboardOptimization(OptimizationParameters optimizationParameters)
        {
            var keyboardOptimizer = new GeneticKeyboard(optimizationParameters);
            var result = keyboardOptimizer.Run();
            keyboardOptimizer.WriteResult(result);
        }
    }
}