using Optimization.Helpers;
using Optimization.Parameters;
using Optimization.Warehouse;

namespace Optimization
{
    public static class OptimizationWork
    {
        public static void FindShortestPath(OptimizationParameters optimizationParameters)
        {
            var matrix = Files.ReadArray(optimizationParameters.DataPath);
            PathFinding.ShortestPath.Find(PointsArrayGenerator.GeneratePointsToVisit(matrix.Length), matrix, optimizationParameters);
        }

        public static void FindShortestPath(OptimizationParameters optimizationParameters,
            DelegateFitness.CalcFitness calcFitness)
        {
            var matrix = Files.ReadArray(optimizationParameters.DataPath);
            PathFinding.ShortestPath.Find(PointsArrayGenerator.GeneratePointsToVisit(matrix.Length), matrix, optimizationParameters,calcFitness);
        }

        public static void WarehouseOptimization(WarehouseParameters warehouseParameters)
        {
            WarehouseOptimizer.Optimize(warehouseParameters);
        }
    }
}