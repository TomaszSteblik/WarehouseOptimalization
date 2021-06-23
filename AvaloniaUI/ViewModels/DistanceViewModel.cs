using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Optimization.GeneticAlgorithms.Crossovers;
using Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers;
using Optimization.GeneticAlgorithms.Eliminations;
using Optimization.GeneticAlgorithms.Initialization;
using Optimization.GeneticAlgorithms.Mutations;
using Optimization.GeneticAlgorithms.Selections;
using Optimization.Parameters;
using AvaloniaUI.Commands;
using AvaloniaUI.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace AvaloniaUI.ViewModels
{
    public class DistanceViewModel : ReactiveObject
    {
        #region Fields
        

        #endregion

        #region Properties
        
        public Distance DistanceParameters { get; set; }
        public int ProgressBarValue { get; set; } = 0;
        public int ProgressBarMaximum { get; set; } = 100;
        public bool RandomSeed { get; set; } = true;
        [Reactive]public int CurrentSeed { get; set; }
        [Reactive]public string SelectedFilesString { get; set; }
        public string[] SelectedFiles { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }
        public int Runs { get; set; } = 10;
        public int xt { get; set; } = 10;
        public List<string>[] Smin { get; set; } = new List<string>[11];
        public List<string>[] Savg { get; set; } = new List<string>[11];
        
        #endregion

        #region ChartProperties

        public bool ShowCustom { get; set; } = true;
        public bool ShowBest { get; set; } = true;
        public bool ShowAvg { get; set; } = true;
        public bool ShowWorst { get; set; } = true;

        #endregion

        #region UiProperties

        public bool IsDistanceStartButtonEnabled { get; set; } = true;
        public List<OptimizationMethod> Methods { get; set; } = 
            Enum.GetValues(typeof(OptimizationMethod)).Cast<OptimizationMethod>().ToList();
        public List<SelectionMethod> Selections { get; set; } = 
            Enum.GetValues(typeof(SelectionMethod)).Cast<SelectionMethod>().ToList();
        public List<CrossoverMethod> Crossovers { get; set; } = 
            Enum.GetValues(typeof(CrossoverMethod)).Cast<CrossoverMethod>().ToList();
        public List<EliminationMethod> Eliminations { get; set; } = 
            Enum.GetValues(typeof(EliminationMethod)).Cast<EliminationMethod>().ToList();
        public List<MutationMethod> Mutations { get; set; } = 
            Enum.GetValues(typeof(MutationMethod)).Cast<MutationMethod>().ToList();
        public List<PopulationInitializationMethod> Initializations { get; set; } = 
            Enum.GetValues(typeof(PopulationInitializationMethod)).Cast<PopulationInitializationMethod>().ToList();
        public List<ConflictResolveMethod> ConflictResolvers { get; set; } =
            Enum.GetValues(typeof(ConflictResolveMethod)).Cast<ConflictResolveMethod>().ToList();

        #endregion

        // #region Visibilities
        //
        // public Visibility IsCrossoverCheckBoxVisible => 
        //     DistanceParameters.CrossoverMethod is CrossoverMethod.MAC or CrossoverMethod.MRC 
        //         ? Visibility.Visible : Visibility.Collapsed;
        //
        // public Visibility IsMutationCheckBoxVisible => 
        //     DistanceParameters.MutationMethod is MutationMethod.MAM or MutationMethod.MRM 
        //         ? Visibility.Visible : Visibility.Collapsed;
        //
        // public Visibility IsCataclysmVisible => 
        //     DistanceParameters.EnableCataclysm ? Visibility.Visible : Visibility.Collapsed;
        //
        // public Visibility IsIncrementalMutationDeltaVisible => 
        //     DistanceParameters.IncrementMutationEnabled ? Visibility.Visible : Visibility.Collapsed;
        //
        // public Visibility IsTournamentSelectionSelected => 
        //     DistanceParameters.SelectionMethod == SelectionMethod.Tournament 
        //         ? Visibility.Visible : Visibility.Collapsed;
        //
        // public Visibility IsTournamentEliminationSelected => 
        //     DistanceParameters.EliminationMethod == EliminationMethod.Tournament 
        //         ? Visibility.Visible : Visibility.Collapsed;
        //
        // public Visibility IsAllVisible => 
        //     DistanceParameters.OptimizationMethod == OptimizationMethod.GeneticAlgorithm 
        //         ? Visibility.Visible : Visibility.Collapsed;
        //
        // #endregion
        
        #region Commands

        public ICommand ReadDistanceDataPathCommand { get; set; }
        public ICommand ReadDistanceResultPathCommand { get; set; }
        public ICommand OptimizeWithCurrentParametersCommand { get; set; }
        public ICommand LoopAllParametersCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        
        #endregion

        public DistanceViewModel()
        {
            DistanceParameters = new Distance();
            
            var crossoversNames = Enum.GetNames(typeof(CrossoverMethod)).ToList();
            crossoversNames.Remove("MRC");
            crossoversNames.Remove("MAC");
            DistanceParameters.CrossoverCheckBoxStates = new List<CheckBoxState>();
            foreach (var crossoverName in crossoversNames)
            {
                DistanceParameters.CrossoverCheckBoxStates.Add(new CheckBoxState(crossoverName, true));
            }

            DistanceParameters.MutationCheckBoxStates = new List<CheckBoxState>();
            var mutationsNames = Enum.GetNames(typeof(MutationMethod)).ToList();
            mutationsNames.Remove("MRM");
            mutationsNames.Remove("MAM");
            foreach (var mutationsName in mutationsNames)
            {
                DistanceParameters.MutationCheckBoxStates.Add(new CheckBoxState(mutationsName,true));
            }

            SetupCommands();
        }

        public DistanceViewModel(OptimizationParameters fitnessGeneticAlgorithmParameters)
        {
            DistanceParameters = new Distance
            {
                Use2opt = fitnessGeneticAlgorithmParameters.Use2opt,
                CrossoverMethod = fitnessGeneticAlgorithmParameters.CrossoverMethod,
                DataPath = fitnessGeneticAlgorithmParameters.DataPath,
                EliminationMethod = fitnessGeneticAlgorithmParameters.EliminationMethod,
                MutationMethod = fitnessGeneticAlgorithmParameters.MutationMethod,
                MutationProbability = fitnessGeneticAlgorithmParameters.MutationProbability,
                OptimizationMethod = fitnessGeneticAlgorithmParameters.OptimizationMethod,
                PopulationSize = fitnessGeneticAlgorithmParameters.PopulationSize,
                SelectionMethod = fitnessGeneticAlgorithmParameters.SelectionMethod,
                MaxEpoch = fitnessGeneticAlgorithmParameters.MaxEpoch,
                ChildrenPerGeneration = fitnessGeneticAlgorithmParameters.ChildrenPerGeneration,
                ParentsPerChildren = fitnessGeneticAlgorithmParameters.ParentsPerChildren
            };



            var crossoversNames = Enum.GetNames(typeof(CrossoverMethod)).ToList();
            crossoversNames.Remove("MRC");
            crossoversNames.Remove("MAC");
            foreach (var crossoverName in crossoversNames)
            {
                DistanceParameters.CrossoverCheckBoxStates.Add(new CheckBoxState(crossoverName, 
                    fitnessGeneticAlgorithmParameters.MultiCrossovers
                        .Contains<CrossoverMethod>((CrossoverMethod) Enum.Parse(typeof(CrossoverMethod),crossoverName))));
            }
            
            
            var mutationsNames = Enum.GetNames(typeof(MutationMethod)).ToList();
            mutationsNames.Remove("MRM");
            mutationsNames.Remove("MAM");
            foreach (var mutationsName in mutationsNames)
            {
                DistanceParameters.MutationCheckBoxStates.Add(new CheckBoxState(mutationsName,
                    fitnessGeneticAlgorithmParameters.MultiMutations
                        .Contains<MutationMethod>((MutationMethod) Enum.Parse(typeof(MutationMethod),mutationsName))));
            }

            SetupCommands();
        }

        private void SetupCommands()
        {
            ReadDistanceDataPathCommand = new ReadDistanceDataPathCommand();
            LoopAllParametersCommand = new LoopAllParametersCommand();
            ReadDistanceResultPathCommand = new ReadDistanceResultPathCommand();

            CancelCommand = new CancelCommand();
        }
    }
}