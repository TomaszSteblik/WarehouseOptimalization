namespace Optimization
{
    public enum OptimizationMethod
    {
        NearestNeighbor,
        GeneticAlgorithm
    }
    public class OptimizationParameters
    {
        public OptimizationMethod OptimizationMethod { get; set; }
        public int StartingId { get; set; }

        public OptimizationParameters()
        {
        }
    }
}