using System;
using System.Xml;

namespace Optimization
{
    public static class FindShortestPath
    {
        public static void Find(OptimizationParameters optimizationParameters)
        {
            Log.Create(optimizationParameters.LogPath);

            if (optimizationParameters.Mode == Mode.DistancesMode)
            {
                Distances.LoadDistances(optimizationParameters.DataPath);

                Optimization optimization = optimizationParameters.OptimizationMethod switch
                {
                    OptimizationMethod.NearestNeighbor => new NearestNeighbor(optimizationParameters),
                    OptimizationMethod.GeneticAlgorithm => new GeneticAlgorithm(optimizationParameters),
                    _ => throw new ArgumentException("Incorrect optimization method in config file")
                };
                
                var result = new Result(optimization.FindShortestPath(optimizationParameters.StartingId),
                    optimizationParameters.ResultPath);
                result.Save();
                
            }
            
        }

        public static double Find(int[] order,  OptimizationParameters optimizationParameters)
        {
            Optimization optimization = optimizationParameters.OptimizationMethod switch
            {
                OptimizationMethod.NearestNeighbor => new NearestNeighbor(order, optimizationParameters),
                OptimizationMethod.GeneticAlgorithm => null, // Genetic co zwraca dlugosc trasy
                _ => throw new ArgumentException("Incorrect optimization method in config file")
            };
            
            return Distances.CalculatePathLengthDouble(optimization.FindShortestPath(order));

        }
    }
}