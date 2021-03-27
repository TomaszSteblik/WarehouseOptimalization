using System;
using Optimization.GeneticAlgorithms;
using Optimization.GeneticAppliances;
using Optimization.GeneticAppliances.Warehouse;
using Optimization.Helpers;
using Optimization.Parameters;

namespace Optimization
{
    public static class OptimizationWork
    {
        public static void FindShortestPath(OptimizationParameters optimizationParameters)
        {
            var matrix = Files.ReadArray(optimizationParameters.DataPath);
            Distances.Create(matrix);
            var result = PathFinding.ShortestPath.Find(PointsArrayGenerator.GeneratePointsToVisit(matrix.Length), optimizationParameters);
            Console.WriteLine(result);
            
        }

        public static void FindShortestPath(OptimizationParameters optimizationParameters,
            DelegateFitness.CalcFitness calcFitness)
        {
            var matrix = Files.ReadArray(optimizationParameters.DataPath);
            Distances.Create(matrix);
            PathFinding.ShortestPath.Find(PointsArrayGenerator.GeneratePointsToVisit(matrix.Length), matrix, optimizationParameters,calcFitness);
        }

        public static void WarehouseOptimization(WarehouseParameters warehouseParameters)
        {
            WarehouseOptimizer.Optimize(warehouseParameters);
        }

        public static void KeyboardOptimization(OptimizationParameters optimizationParameters)
        {
            var keyboardOptimizer = new GeneticKeyboard(optimizationParameters);
            var result = keyboardOptimizer.Run();
            keyboardOptimizer.WriteResult(result);
        }
    }
}