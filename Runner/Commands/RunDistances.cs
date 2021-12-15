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
    private ParametersModel _parametersModel;
    private ConsoleLogModel _logModel;

    public RunDistances(ParametersModel parametersModel, ConsoleLogModel logModel)
    {
        _parametersModel = parametersModel;
        _logModel = logModel;
    }
    
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        if(_parametersModel.SelectedFiles is null) return;
        if (_parametersModel.SelectedFiles[0] == "") return;
        _parametersModel.DataPath = _parametersModel.SelectedFiles[0];
        _logModel.AppendLog("test");
        var result = OptimizationWork.TSP(_parametersModel, CancellationToken.None);
        _logModel.AppendLog(result.FinalFitness.ToString());
        //vm.Result = result.FinalFitness.ToString();
    }

    public event EventHandler? CanExecuteChanged;
}