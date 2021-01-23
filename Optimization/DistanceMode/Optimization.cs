namespace Optimization.DistanceMode
{
    public abstract class Optimization
    {
        protected OptimizationParameters _optimizationParameters;
        public abstract int[] FindShortestPath(int[] order);
    }
}