namespace OptimizationIO
{
    public enum OptimizationMethod
    {
        NearestNeighbor,
        GeneticAlgorithm
    }
    public class OptimizationParameters
    {
        public OptimizationMethod OptimizationMethod { get; set; }
        
        public bool Use2opt { get; set; }
        public int StartingId { get; set; }
        public string LogPath { get; set; }
        public string DataPath { get; set; }
        public string ResultPath { get; set; }
        //genethic algorithm parameters
        public string SelectionMethod { get; set; }
        public string CrossoverMethod { get; set; }
        public string EliminationMethod { get; set; }
        public OptimizationParameters()
        {
        }
    }
}