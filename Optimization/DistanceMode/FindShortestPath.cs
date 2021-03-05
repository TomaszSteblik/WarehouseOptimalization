using System;
using System.Threading.Tasks;
using OptimizationMethods.DistanceMode.GeneticAlgorithms;
using OptimizationMethods.Parameters;

namespace OptimizationMethods.DistanceMode
{
    public static class FindShortestPath
    {
        public static double Find(int[] order, double[][] distancesMatrix,  OptimizationParameters optimizationParameters)
        {
            Algorithm algorithm = optimizationParameters.OptimizationMethod switch
            {
                OptimizationMethod.Permutations => new Permutations(optimizationParameters, distancesMatrix),
                OptimizationMethod.NearestNeighbor => new NearestNeighbor(optimizationParameters, distancesMatrix),
                OptimizationMethod.GeneticAlgorithm => new GeneticAlgorithm(optimizationParameters, distancesMatrix,
                    (population, distances) =>
                    {
                        double[] fitness = new double[population.Length];
                        for (int i = 0; i < population.Length; i++)
                            fitness[i] = Fitness.CalculateFitness(population[i], distances);
                        return fitness;
                    }),
                _ => throw new ArgumentException("Incorrect optimization method in config file")
            };
            int[] objectOrder = algorithm.FindShortestPath(order);
            double pathLength = Fitness.CalculateFitness(objectOrder, distancesMatrix);

            if (optimizationParameters.ResultToFile)
            {
                Log log = new Log(optimizationParameters);
                log.SaveResult(objectOrder, pathLength);
            }

            return pathLength;

        }
        public static double Find(int[] order, double[][] distancesMatrix,  OptimizationParameters optimizationParameters,GeneticAlgorithm.CalcFitness calcFitness)
        {
            Algorithm algorithm = optimizationParameters.OptimizationMethod switch
            {
                OptimizationMethod.Permutations => new Permutations(optimizationParameters, distancesMatrix),
                OptimizationMethod.NearestNeighbor => new NearestNeighbor(optimizationParameters, distancesMatrix),
                OptimizationMethod.GeneticAlgorithm => new GeneticAlgorithm(optimizationParameters, distancesMatrix,calcFitness),
                _ => throw new ArgumentException("Incorrect optimization method in config file")
            };
            int[] objectOrder = algorithm.FindShortestPath(order);
            double pathLength = Fitness.CalculateFitness(objectOrder, distancesMatrix);

            if (optimizationParameters.ResultToFile)
            {
                Log log = new Log(optimizationParameters);
                log.SaveResult(objectOrder, pathLength);
            }

            return pathLength;

        }

    }
}