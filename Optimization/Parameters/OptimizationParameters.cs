namespace OptimizationMethods.Parameters
{
    public enum OptimizationMethod
    {
        NearestNeighbor,
        GeneticAlgorithm,
        Permutations
    }

    public enum Mode
    {
        WarehouseMode,
        DistancesMode
    }
    public class OptimizationParameters
    {
        public OptimizationMethod OptimizationMethod { get; set; } = OptimizationMethod.GeneticAlgorithm;
        public bool Use2opt { get; set; } = false;
        public int StartingId { get; set; } = 0;
        public bool LogEnabled { get; set; }
        public string LogPath { get; set; }
        public string ResultPath { get; set; }
        public bool ResultToFile { get; set; }
        public string DataPath { get; set; }
        public string SelectionMethod { get; set; } = "RouletteWheel";
        public string CrossoverMethod { get; set; } = "HGreX";
        public string EliminationMethod { get; set; } = "Elitism";
        public string MutationMethod { get; set; } = "Inversion";
        public double MutationProbability { get; set; } = 30;
        public int PopulationSize { get; set; } = 60;
        public int ChildrenPerGeneration { get; set; } = 30;
        public int TerminationValue { get; set; } = 300;
    }
}