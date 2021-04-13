using System;
using System.Data;
using System.Linq;
using Optimization.GeneticAlgorithms.Crossovers;
using Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers;
using Optimization.GeneticAlgorithms.Eliminations;
using Optimization.GeneticAlgorithms.Initialization;
using Optimization.GeneticAlgorithms.Modules;
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

        public bool WriteCsv { get; set; } = true;
        public virtual string DataPath { get; set; }

        public virtual PopulationInitializationMethod PopulationInitializationMethod { get; set; } =
            PopulationInitializationMethod.StandardPathInitialization;
        public virtual SelectionMethod SelectionMethod { get; set; } = SelectionMethod.RouletteWheel;

        public virtual ConflictResolveMethod ConflictResolveMethod { get; set; } = ConflictResolveMethod.Random;
        public virtual CrossoverMethod CrossoverMethod { get; set; } = CrossoverMethod.Aex;
        public virtual CrossoverMethod[] MultiCrossovers { get; set; }

        public virtual EliminationMethod EliminationMethod { get; set; } = EliminationMethod.Elitism;
        public virtual MutationMethod MutationMethod { get; set; } = MutationMethod.RSM;
        public virtual double MutationProbability { get; set; } = 30;

        public virtual MutationMethod[] MultiMutations { get; set; }

        public virtual int PopulationSize { get; set; } = 120;
        public virtual int ParentsPerChildren { get; set; } = 2;
        public virtual int ChildrenPerGeneration { get; set; } = 60;

        public virtual bool StopAfterEpochsWithoutChange { get; set; } = false;
        
        public virtual int MaxEpoch { get; set; } = 300;
        
        public virtual int StopAfterEpochCount { get; set; }
        
        public virtual bool EnableCataclysm { get; set; }
        public virtual int CataclysmEpoch { get; set; }
        public virtual int DeathPercentage { get; set; }
    }
}