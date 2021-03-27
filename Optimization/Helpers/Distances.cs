namespace Optimization.Helpers
{
    public class Distances
    {
        public double[][] DistancesMatrix { get; }

        private static Distances _instance;
        public static Distances GetInstance() => _instance;

        private Distances(double[][] distancesMatrix)
        {
            DistancesMatrix = distancesMatrix;
        }

        public static void Create(double[][] distancesMatrix)
        {
            _instance = new Distances(distancesMatrix);
        }
        
    }
}