using System;
using OptimizationIO;

namespace Optimization
{
    public static class FindShortestPath
    {
        public static void Find(OptimizationParameters optimizationParameters)
        {
            var cityDistances = CityDistances.Create(optimizationParameters.DataPath);
            Optimization optimization;
            switch (optimizationParameters.OptimizationMethod)
            {
                case OptimizationMethod.NearestNeighbor:
                    optimization = new NearestNeighbor(cityDistances, optimizationParameters);
                    break;
                
                default:
                    optimization = new NearestNeighbor(cityDistances, optimizationParameters);
                    break;
            }
            
            var result = new Result(optimization.FindShortestPath(optimizationParameters.StartingId), cityDistances,
                optimizationParameters.ResultPath);
            result.Save();
            


        }
    }
}