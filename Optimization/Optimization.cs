namespace Optimization
{
    public abstract class Optimization
    {
        protected OptimizationParameters OptimizationParameters;
        public abstract int[] FindShortestPath(int startingId);
        public abstract int[] FindShortestPath(int[] order);
    }
}