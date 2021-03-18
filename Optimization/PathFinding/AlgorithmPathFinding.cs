using Optimization.Parameters;

namespace Optimization.PathFinding
{
    internal abstract class AlgorithmPathFinding
    {
        protected OptimizationParameters _optimizationParameters;
        public abstract int[] FindShortestPath(int[] order);
    }
}