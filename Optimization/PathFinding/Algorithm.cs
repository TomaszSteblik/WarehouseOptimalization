using Optimization.Parameters;

namespace Optimization.PathFinding
{
    internal abstract class Algorithm
    {
        protected OptimizationParameters _optimizationParameters;
        public abstract int[] FindShortestPath(int[] order);
    }
}