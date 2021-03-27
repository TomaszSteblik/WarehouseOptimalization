using System;
using Optimization.GeneticAlgorithms;
using Optimization.Helpers;
using Optimization.Parameters;

namespace Optimization.PathFinding
{
    internal static class ShortestPath
    {
        public static double Find(int[] order,  OptimizationParameters optimizationParameters)
        {
            IPathFinder algorithmPathFinding = optimizationParameters.OptimizationMethod switch
            {
                OptimizationMethod.Permutations => new Permutations(optimizationParameters),
                OptimizationMethod.NearestNeighbor => new NearestNeighbor(optimizationParameters),
                OptimizationMethod.GeneticAlgorithm => new GeneticPathFinding(order, optimizationParameters,
                    (population) =>
                    {
                        double[] fitness = new double[population.Length];
                        for (int i = 0; i < population.Length; i++)
                            fitness[i] = Fitness.CalculateFitness(population[i]);
                        return fitness;
                    }),
                _ => throw new ArgumentException("Incorrect optimization method in config file")
            };
            int[] objectOrder = algorithmPathFinding.FindShortestPath(order);
            
            if (optimizationParameters.Use2opt)
                objectOrder = Optimizer2Opt.Optimize(objectOrder);
            
            double pathLength = Fitness.CalculateFitness(objectOrder);

            if (optimizationParameters.ResultToFile)
            {
                Log log = new Log(optimizationParameters);
                log.SaveResult(objectOrder, pathLength);
            }

            return pathLength;

        }
        public static double Find(int[] order, double[][] distancesMatrix,  OptimizationParameters optimizationParameters,DelegateFitness.CalcFitness calcFitness)
        {
            IPathFinder algorithmPathFinding = optimizationParameters.OptimizationMethod switch
            {
                OptimizationMethod.Permutations => new Permutations(optimizationParameters),
                OptimizationMethod.NearestNeighbor => new NearestNeighbor(optimizationParameters),
                OptimizationMethod.GeneticAlgorithm => new GeneticPathFinding(order,optimizationParameters,calcFitness),
                _ => throw new ArgumentException("Incorrect optimization method in config file")
            };
            int[] objectOrder = algorithmPathFinding.FindShortestPath(order);
            double pathLength = Fitness.CalculateFitness(objectOrder);

            if (optimizationParameters.ResultToFile)
            {
                Log log = new Log(optimizationParameters);
                log.SaveResult(objectOrder, pathLength);
            }

            return pathLength;

        }

    }
}