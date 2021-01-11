namespace Optimization.DistanceMode
{
    public abstract class Optimization
    {
        protected OptimizationParameters OptimizationParameters;
        public abstract int[] FindShortestPath(int[] order);
    }
}