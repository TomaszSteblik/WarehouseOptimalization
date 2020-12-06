using System;

namespace Optimization
{
    public static class FindShortestPath
    {
        public static void Find(OptimizationParameters optimizationParameters)
        {
            var cityDistances = CityDistances.Create(optimizationParameters.DataPath);
            
            Optimization optimization = optimizationParameters.OptimizationMethod switch
            {
                OptimizationMethod.NearestNeighbor => new NearestNeighbor(cityDistances, optimizationParameters),
                OptimizationMethod.GeneticAlgorithm => new GeneticAlgorithm(cityDistances, optimizationParameters),
                _ => throw new ArgumentException("Incorrect optimization method in config file")
            };

            var result = new Result(optimization.FindShortestPath(optimizationParameters.StartingId), cityDistances,
                optimizationParameters.ResultPath);
            result.Save();
            
        }
    }
}