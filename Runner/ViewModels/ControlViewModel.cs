using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Optimization;
using Runner.Commands;
using Runner.Models;

namespace Runner.ViewModels;

public class ControlViewModel : ViewModelBase
{
    public ParametersModel ParametersModel { get; set; }
    public ICommand RunDistances { get; set; }
    public List<OptimizationTask> Tasks { get; set; } = Enum.GetValues(typeof(OptimizationTask)).Cast<OptimizationTask>().ToList();
    public OptimizationTask SelectedTask { get; set; } = OptimizationTask.TSP;

    public ControlViewModel(ParametersModel parametersModel)
    {
        ParametersModel = parametersModel;
        RunDistances = new RunDistances(parametersModel);
    }
}