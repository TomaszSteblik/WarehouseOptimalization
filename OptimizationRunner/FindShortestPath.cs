using OptimizationIO;
using GeneticAlgorithm = GeneticAlgorithm.GeneticAlgorithm;

namespace Optimization
{
    public static class FindShortestPath
    {
        public static void Find(OptimizationParameters optimizationParameters)
        {
            var cityDistances = CityDistances.Create(optimizationParameters.DataPath);
            OptimizationIO.Optimization optimization;
            switch (optimizationParameters.OptimizationMethod)
            {
                case OptimizationMethod.NearestNeighbor:
                    optimization = new NearestNeighbor(cityDistances, optimizationParameters);
                    break;
                case OptimizationMethod.GeneticAlgorithm:
                    optimization = new GeneticAlgorithm(optimizationParameters);
                    break;
                default:
                    return;
            }
            
            var result = new Result(optimization.FindShortestPath(optimizationParameters.StartingId), cityDistances,
                optimizationParameters.ResultPath);
            result.Save();
            


        }
    }
}