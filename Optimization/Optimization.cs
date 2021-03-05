using OptimizationMethods.Parameters;
using OptimizationMethods.DistanceMode;
using OptimizationMethods.DistanceMode.GeneticAlgorithms;
using OptimizationMethods.WarehouseMode;

namespace OptimizationMethods
{
    public static class Optimization
    {
        public static void FindShortestPath(OptimizationParameters optimizationParameters)
        {
            var matrix = Files.ReadArray(optimizationParameters.DataPath);
            DistanceMode.FindShortestPath.Find(PointsArrayGenerator.GeneratePointsToVisit(matrix.Length), matrix, optimizationParameters);
        }

        public static void FindShortestPath(OptimizationParameters optimizationParameters,
            GeneticAlgorithm.CalcFitness calcFitness)
        {
            var matrix = Files.ReadArray(optimizationParameters.DataPath);
            DistanceMode.FindShortestPath.Find(PointsArrayGenerator.GeneratePointsToVisit(matrix.Length), matrix, optimizationParameters,calcFitness);
        }

        public static void WarehouseOptimization(WarehouseParameters warehouseParameters)
        {
            Warehouse.Optimizer(warehouseParameters);
        }
    }
}