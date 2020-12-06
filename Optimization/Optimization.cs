namespace Optimization
{
    public abstract class Optimization
    {
        protected OptimizationParameters OptimizationParameters;
        protected CityDistances CityDistances;
        public abstract int[] FindShortestPath(int startingId);
    }
}