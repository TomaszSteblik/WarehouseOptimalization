using System;
using Optimization.DistanceMode.GeneticAlgorithms;

namespace Optimization.DistanceMode
{
    public static class FindShortestPath
    {
        public static double Find(int[] order, double[][] distancesMatrix,  OptimizationParameters optimizationParameters)
        {
            Optimization optimization = optimizationParameters.OptimizationMethod switch
            {
                OptimizationMethod.NearestNeighbor => new NearestNeighbor.NearestNeighbor(optimizationParameters, distancesMatrix),
                OptimizationMethod.GeneticAlgorithm => new GeneticAlgorithm(optimizationParameters, distancesMatrix),
                _ => throw new ArgumentException("Incorrect optimization method in config file")
            };
            int[] objectOrder = optimization.FindShortestPath(order);
            double pathLength = Distances.CalculatePathLengthDouble(objectOrder, distancesMatrix);

            if (optimizationParameters.ResultToFile)
            {
                var result = new Result(objectOrder,
                    pathLength,
                    optimizationParameters.ResultPath);
                result.Save();
            }

            return pathLength;

        }
    }
}