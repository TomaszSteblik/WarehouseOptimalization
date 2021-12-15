using Optimization.GeneticAlgorithms.Crossovers;
using Optimization.GeneticAlgorithms.Eliminations;
using Optimization.GeneticAlgorithms.Mutations;
using Optimization.GeneticAlgorithms.Selections;
using Optimization.Parameters;

namespace Runner.Models;

public class ParametersModel : OptimizationParameters
{
    public string[] SelectedFiles { get; set; }
    public override SelectionMethod SelectionMethod { get; set; } = SelectionMethod.RouletteWheel;
    public override CrossoverMethod CrossoverMethod { get; set; } = CrossoverMethod.HProX;
    public override EliminationMethod EliminationMethod { get; set; } = EliminationMethod.Elitism;
    public override MutationMethod MutationMethod { get; set; } = MutationMethod.RSM;
    public override int PopulationSize { get; set; } = 100;
    public override int ChildrenPerGeneration { get; set; } = 50;
    public override int MaxEpoch { get; set; } = 300;
}