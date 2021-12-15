using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Optimization.GeneticAlgorithms.Crossovers;
using Optimization.GeneticAlgorithms.Eliminations;
using Optimization.GeneticAlgorithms.Mutations;
using Optimization.GeneticAlgorithms.Selections;
using ReactiveUI.Fody.Helpers;
using Runner.Commands;
using Runner.Models;

namespace Runner.ViewModels;

public class ParametersViewModel : ViewModelBase
{
    
    public List<SelectionMethod> Selections { get; set; } = Enum.GetValues(typeof(SelectionMethod)).Cast<SelectionMethod>().ToList();
    public List<CrossoverMethod> Crossovers { get; set; } = Enum.GetValues(typeof(CrossoverMethod)).Cast<CrossoverMethod>().ToList();
    public List<EliminationMethod> Eliminations { get; set; } = Enum.GetValues(typeof(EliminationMethod)).Cast<EliminationMethod>().ToList();
    public List<MutationMethod> Mutations { get; set; } = Enum.GetValues(typeof(MutationMethod)).Cast<MutationMethod>().ToList();
    public ICommand SelectData { get; set; }
    public ParametersModel ParametersModel { get; set; }
    [Reactive] public string SelectedFilesString { get; set; }
    
    public ParametersViewModel(ParametersModel model)
    {
        ParametersModel = model;
        SelectData = new SelectData(ParametersModel);
    }
    
    
}