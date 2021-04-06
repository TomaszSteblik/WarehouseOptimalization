using System;
using System.Threading;
using System.Threading.Tasks;
using Optimization.GeneticAlgorithms;
using Optimization.GeneticAppliances;
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

            return PathFinding.ShortestPath.Find(PointsArrayGenerator.GeneratePointsToVisit(matrix.Length), matrix, optimizationParameters);

        }

        public static double FindShortestPath(OptimizationParameters optimizationParameters,
            DelegateFitness.CalcFitness calcFitness)
        {
            var matrix = Files.ReadArray(optimizationParameters.DataPath);
            return PathFinding.ShortestPath.Find(PointsArrayGenerator.GeneratePointsToVisit(matrix.Length), matrix, optimizationParameters,calcFitness);
        }

        public static double WarehouseOptimization(WarehouseParameters warehouseParameters)
        {
            return WarehouseOptimizer.Optimize(warehouseParameters);

        }

        public static void KeyboardOptimization(OptimizationParameters optimizationParameters)
        {
            var keyboardOptimizer = new GeneticKeyboard(optimizationParameters);
            var result = keyboardOptimizer.Run();
            keyboardOptimizer.WriteResult(result);
        }
    }
}