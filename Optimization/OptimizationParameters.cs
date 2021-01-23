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
        public OptimizationMethod OptimizationMethod { get; set; } = OptimizationMethod.NearestNeighbor;
        public bool Use2opt { get; set; } = false;
        public int StartingId { get; set; } = 0;
        public bool LogEnabled { get; set; }
        public string LogPath { get; set; }
        public string DataPath { get; set; }
        public Mode Mode { get; set; }
        public string WarehousePath { get; set; }
        public bool ResultToFile { get; set; }
        public string ResultPath { get; set; }
        public string OrdersPath { get; set; }
        public string SelectionMethod { get; set; } = "Elitism";
        public string CrossoverMethod { get; set; } = "Aex";
        public string EliminationMethod { get; set; } = "Elitism";

        public string MutationMethod { get; set; } = "Inversion";
        public double MutationProbabilityHGreX { get; set; } = 30;
        public int PopulationSizeHGreX { get; set; } = 120;
        public int ChildrenPerGenerationHGreX { get; set; } = 60;
        public int TerminationValueHGreX { get; set; } = 400;

        public double MutationProbabilityAEX { get; set; } = 30;
        public int PopulationSizeAEX { get; set; } = 60;
        public int ChildrenPerGenerationAEX { get; set; } = 30;
        public int TerminationValueAEX { get; set; } = 400;
    }
}