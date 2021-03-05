using Optimization.Parameters;

namespace OptimizationMethods.DistanceMode
{
    public abstract class Algorithm
    {
        protected OptimizationParameters _optimizationParameters;
        public abstract int[] FindShortestPath(int[] order);
    }
}