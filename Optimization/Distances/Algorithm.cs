using Optimization.Parameters;

namespace Optimization.Distances
{
    internal abstract class Algorithm
    {
        protected OptimizationParameters _optimizationParameters;
        public abstract int[] FindShortestPath(int[] order);
    }
}