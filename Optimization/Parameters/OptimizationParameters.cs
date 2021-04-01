using System;
using System.Linq;
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
        public virtual OptimizationMethod OptimizationMethod { get; set; } = OptimizationMethod.GeneticAlgorithm;
        public virtual bool Use2opt { get; set; } = false;
        public virtual int StartingId { get; set; } = 0;
        public virtual bool LogEnabled { get; set; }
        public virtual string LogPath { get; set; }
        public virtual string ResultPath { get; set; }
        public virtual bool ResultToFile { get; set; }
        public virtual string DataPath { get; set; }
        public virtual SelectionMethod SelectionMethod { get; set; } = SelectionMethod.RouletteWheel;
        public virtual CrossoverMethod CrossoverMethod { get; set; } = CrossoverMethod.Aex;

        public virtual CrossoverMethod[] MultiCrossovers { get; set; } =
            Enum.GetValues(typeof(CrossoverMethod)).Cast<CrossoverMethod>().ToArray();
        public virtual EliminationMethod EliminationMethod { get; set; } = EliminationMethod.Elitism;
        public virtual MutationMethod MutationMethod { get; set; } = MutationMethod.RSM;
        public virtual double MutationProbability { get; set; } = 30;

        public virtual MutationMethod[] MultiMutations { get; set; } =
            Enum.GetValues(typeof(MutationMethod)).Cast<MutationMethod>().ToArray();
        public virtual int PopulationSize { get; set; } = 120;
        public virtual int ParentsPerChildren { get; set; } = 2;
        public virtual int ChildrenPerGeneration { get; set; } = 60;
        public virtual int TerminationValue { get; set; } = 300;
    }
}