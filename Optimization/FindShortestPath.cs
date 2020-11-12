namespace Optimization
{
    public static class FindShortestPath
    {
        public static void Find(string dataFile, string resultFile, OptimizationParameters optimizationParameters)
        {
            var cityDistances = CityDistances.Create(dataFile);
            var optimizer = new NearestNeighbor(cityDistances);
            var result = new Result(optimizer.FindShortestPath(optimizationParameters.StartingId), cityDistances, resultFile);
            result.Save();
        }
    }
}