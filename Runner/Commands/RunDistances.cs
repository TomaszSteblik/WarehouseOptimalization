using System;
using System.Threading;
using System.Windows.Input;
using Optimization;
using Optimization.GeneticAlgorithms.Crossovers;
using Optimization.GeneticAlgorithms.Eliminations;
using Optimization.GeneticAlgorithms.Selections;
using Optimization.Parameters;
using Runner.ViewModels;

namespace Runner.Commands;

public class RunDistances : ICommand
{
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        var vm = parameter as MainWindowViewModel;
        //if (vm.DataFilePath == "") return;
        var param = new OptimizationParameters
        {
            SelectionMethod = SelectionMethod.RouletteWheel,
            EliminationMethod = EliminationMethod.Elitism,
            CrossoverMethod = CrossoverMethod.HProX,
            MaxEpoch = 300,
            ChildrenPerGeneration = 50,
            PopulationSize = 100,
            //DataPath = vm.DataFilePath
        };
        var result = OptimizationWork.TSP(param, CancellationToken.None);
        //vm.Result = result.FinalFitness.ToString();
    }

    public event EventHandler? CanExecuteChanged;
}