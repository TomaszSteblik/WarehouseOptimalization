using Optimization.GeneticAlgorithms.Crossovers;
using Optimization.GeneticAlgorithms.Eliminations;
using Optimization.GeneticAlgorithms.Mutations;
using Optimization.GeneticAlgorithms.Selections;

namespace Runner.Models;

public class ParametersModel
{
    public string[] SelectedFiles { get; set; }
    public SelectionMethod SelectedSelection { get; set; } = SelectionMethod.RouletteWheel;
    public CrossoverMethod SelectedCrossover { get; set; } = CrossoverMethod.HProX;
    public EliminationMethod SelectedElimination { get; set; } = EliminationMethod.Elitism;
    public MutationMethod SelectedMutation { get; set; } = MutationMethod.RSM;
}