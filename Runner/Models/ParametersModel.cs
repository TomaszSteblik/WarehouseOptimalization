using Optimization.GeneticAlgorithms.Crossovers;
using Optimization.GeneticAlgorithms.Eliminations;
using Optimization.GeneticAlgorithms.Mutations;
using Optimization.GeneticAlgorithms.Selections;
using Optimization.Parameters;

namespace Runner.Models;

public class ParametersModel : OptimizationParameters
{
    public string[] SelectedFiles { get; set; }
}