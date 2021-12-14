using System;
using System.Threading;
using System.Windows.Input;
using Optimization;
using Optimization.GeneticAlgorithms.Crossovers;
using Optimization.GeneticAlgorithms.Eliminations;
using Optimization.GeneticAlgorithms.Selections;
using Optimization.Parameters;
using Runner.Models;
using Runner.ViewModels;

namespace Runner.Commands;

public class RunDistances : ICommand
{
    private ParametersModel paramaters;

    public RunDistances(ParametersModel model)
    {
        paramaters = model;
    }
    
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        
        if (paramaters.SelectedFiles[0] == "") return;
        var param = new OptimizationParameters
        {
            SelectionMethod = SelectionMethod.RouletteWheel,
            EliminationMethod = EliminationMethod.Elitism,
            CrossoverMethod = CrossoverMethod.HProX,
            MaxEpoch = 300,
            ChildrenPerGeneration = 50,
            PopulationSize = 100,
            DataPath = paramaters.SelectedFiles[0]
        };
        var result = OptimizationWork.TSP(param, CancellationToken.None);
        Console.WriteLine(result.FinalFitness);
        //vm.Result = result.FinalFitness.ToString();
    }

    public event EventHandler? CanExecuteChanged;
}