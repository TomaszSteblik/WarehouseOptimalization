using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Optimization;
using Optimization.GeneticAlgorithms.Crossovers;
using Optimization.GeneticAlgorithms.Eliminations;
using Optimization.GeneticAlgorithms.Selections;
using Optimization.GeneticAppliances.TSP;
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

    public async void Execute(object? parameter)
    {
        if(_parametersModel.SelectedFiles is null) return;
        if (_parametersModel.SelectedFiles[0] == "") return;
        _parametersModel.DataPath = _parametersModel.SelectedFiles[0];
        var dataset = OperatingSystem.IsWindows()
            ? _parametersModel.DataPath.Split("\\")[^1]
            : _parametersModel.DataPath.Split("/")[^1];
        _logModel.AppendLog($"Started TSP on {dataset} dataset");
        TSPResult result = null;
        _ = await Task.Run(async () => result = OptimizationWork.TSP(_parametersModel, CancellationToken.None));
        if (result is not null)
        {
            _logModel.AppendLog("Result: " + result.FinalFitness.ToString("0.##"));

        }
        //vm.Result = result.FinalFitness.ToString();
    }

    public event EventHandler? CanExecuteChanged;
}