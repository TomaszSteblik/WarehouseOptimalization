using Optimization.Distances;
using Optimization.Parameters;

namespace Optimization
{
    public static class OptimizationWork
    {
        public static void FindShortestPath(OptimizationParameters optimizationParameters)
        {
            var matrix = Files.ReadArray(optimizationParameters.DataPath);
            global::Optimization.Distances.FindShortestPath.Find(PointsArrayGenerator.GeneratePointsToVisit(matrix.Length), matrix, optimizationParameters);
        }

        public static void FindShortestPath(OptimizationParameters optimizationParameters,
            DelegateFitness.CalcFitness calcFitness)
        {
            var matrix = Files.ReadArray(optimizationParameters.DataPath);
            global::Optimization.Distances.FindShortestPath.Find(PointsArrayGenerator.GeneratePointsToVisit(matrix.Length), matrix, optimizationParameters,calcFitness);
        }

        public static void WarehouseOptimization(WarehouseParameters warehouseParameters)
        {
            Warehouse.Warehouse.Optimizer(warehouseParameters);
        }
    }
}