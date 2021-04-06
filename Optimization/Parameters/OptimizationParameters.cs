
using Optimization.GeneticAlgorithms.Crossovers;
using Optimization.GeneticAlgorithms.Eliminations;
using Optimization.GeneticAlgorithms.Mutations;
using Optimization.GeneticAlgorithms.Selections;

namespace Optimization.Parameters
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
        public bool LogEnabled { get; set; } = false;
        public string LogPath { get; set; }
        public string ResultPath { get; set; }
        public bool ResultToFile { get; set; } = false;
        public string DataPath { get; set; }
        public Selection.SelectionType SelectionMethod { get; set; } = Selection.SelectionType.RouletteWheel;
        public Crossover.CrossoverType CrossoverMethod { get; set; } = Crossover.CrossoverType.Aex;
        public Elimination.EliminationType EliminationMethod { get; set; } = Elimination.EliminationType.Elitism;
        public Mutation.MutationType MutationMethod { get; set; } = Mutation.MutationType.RSM;
        public double MutationProbability { get; set; } = 30;
        public int PopulationSize { get; set; } = 120;
        public int ChildrenPerGeneration { get; set; } = 60;
        public int TerminationValue { get; set; } = 100;
}
}