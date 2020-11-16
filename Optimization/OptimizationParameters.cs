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

        public string LogPath { get; set; }
        public string DataPath { get; set; }
        public string ResultPath { get; set; }

        public OptimizationParameters()
        {
        }
    }
}