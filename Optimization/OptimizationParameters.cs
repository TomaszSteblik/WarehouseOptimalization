namespace Optimization
{
    public enum OptimizationMethod
    {
        NearestNeighbor,
        GeneticAlgorithm
    }

    public enum Mode
    {
        WarehouseMode,
        DistancesMode
    }
    public class OptimizationParameters
    {
        public OptimizationMethod OptimizationMethod { get; set; }
        
        public bool Use2opt { get; set; }
        public int StartingId { get; set; }
        public string LogPath { get; set; }
        public string DataPath { get; set; }
        
        public Mode Mode { get; set; }
        public string WarehousePath { get; set; }
        
        public bool ResultToFile { get; set; }
        public string ResultPath { get; set; }
        
        public string OrdersPath { get; set; }
        //genethic algorithm parameters
        public string SelectionMethod { get; set; }
        public string CrossoverMethod { get; set; }
        public string EliminationMethod { get; set; }
        public bool CanMutate { get; set; }
        public double MutationProbability { get; set; }
        public int PopulationSize { get; set; }
        public int ChildrenPerGeneration { get; set; }
        public int TerminationValue { get; set; }
        public OptimizationParameters()
        {
            
        }
    }
}