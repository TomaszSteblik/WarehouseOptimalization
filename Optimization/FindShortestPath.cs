namespace Optimization
{
    public static class FindShortestPath
    {
        public static void Find(OptimizationParameters optimizationParameters)
        {
            var cityDistances = CityDistances.Create(optimizationParameters.DataPath);
            var optimizer = new NearestNeighbor(cityDistances, optimizationParameters);
            var result = new Result(optimizer.FindShortestPath(optimizationParameters.StartingId), cityDistances, optimizationParameters.ResultPath);
            result.Save();
        }
    }
}