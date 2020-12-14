using System;

namespace Optimization
{
    public static class FindShortestPath
    {
        public static void Find(OptimizationParameters optimizationParameters)
        {
            CityDistances.Create(optimizationParameters.DataPath, optimizationParameters.WarehousePath);
            Log.Create(optimizationParameters.LogPath);
            
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
}