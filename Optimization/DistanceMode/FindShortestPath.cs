using System;
using System.Threading.Tasks;
using Optimization.DistanceMode.GeneticAlgorithms;

namespace Optimization.DistanceMode
{
    public static class FindShortestPath
    {
        public static double Find(int[] order, double[][] distancesMatrix,  OptimizationParameters optimizationParameters)
        {
            Optimization optimization = optimizationParameters.OptimizationMethod switch
            {
                OptimizationMethod.NearestNeighbor => new NearestNeighbor(optimizationParameters, distancesMatrix),
                OptimizationMethod.GeneticAlgorithm => new GeneticAlgorithm(optimizationParameters, distancesMatrix,
                    (population, distances) =>
                    {
                        double[] fitness = new double[population.Length];
                        Parallel.For(0, population.Length, i =>
                        {
                            fitness[i] = Distances.CalculatePathLengthDouble(population[i], distances);
                        });
                        return fitness;
                    }),
                _ => throw new ArgumentException("Incorrect optimization method in config file")
            };
            int[] objectOrder = optimization.FindShortestPath(order);
            double pathLength = Distances.CalculatePathLengthDouble(objectOrder, distancesMatrix);

            if (optimizationParameters.ResultToFile)
            {
                Log log = new Log(optimizationParameters.ResultPath);
                log.SaveResult(objectOrder, pathLength);
            }

            return pathLength;

        }
    }
}