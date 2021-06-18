using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Optimization.GeneticAlgorithms.Crossovers;
using Optimization.GeneticAlgorithms.Eliminations;
using Optimization.GeneticAlgorithms.Mutations;
using Optimization.GeneticAlgorithms.Selections;
using Optimization.Parameters;
using OptimizationUI.Models;

namespace OptimizationUI.ViewModels
{
    public class DistanceViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties

        
        public Distance DistanceParameters { get; set; }
        public int ProgressBarValue { get; set; } = 0;
        public int ProgressBarMaximum { get; set; } = 100;
        public bool RandomSeed { get; set; } = true;
        public int CurrentSeed { get; set; }
        public string SelectedFilesString { get; set; }
        public string[] SelectedFiles { get; set; }

        #endregion

        #region ChartProperties

        public bool ShowCustom { get; set; } = true;
        public bool ShowBest { get; set; } = true;
        public bool ShowAvg { get; set; } = true;
        public bool ShowWorst { get; set; } = true;

        #endregion

        #region Visibilities

        public Visibility IsCrossoverCheckBoxVisible => 
            DistanceParameters.CrossoverMethod is CrossoverMethod.MAC or CrossoverMethod.MRC 
                ? Visibility.Visible : Visibility.Collapsed;
        
        public Visibility IsMutationCheckBoxVisible => 
            DistanceParameters.MutationMethod is MutationMethod.MAM or MutationMethod.MRM 
                ? Visibility.Visible : Visibility.Collapsed;
        
        public Visibility IsCataclysmVisible => 
            DistanceParameters.EnableCataclysm ? Visibility.Visible : Visibility.Collapsed;

        public Visibility IsIncrementalMutationDeltaVisible => 
            DistanceParameters.IncrementMutationEnabled ? Visibility.Visible : Visibility.Collapsed;
        
        public Visibility IsTournamentSelectionSelected => 
            DistanceParameters.SelectionMethod == SelectionMethod.Tournament 
                ? Visibility.Visible : Visibility.Collapsed;

        public Visibility IsTournamentEliminationSelected => 
            DistanceParameters.EliminationMethod == EliminationMethod.Tournament 
                ? Visibility.Visible : Visibility.Collapsed;

        public Visibility IsAllVisible => 
            DistanceParameters.OptimizationMethod == OptimizationMethod.GeneticAlgorithm 
                ? Visibility.Visible : Visibility.Collapsed;

        #endregion
        
        #region Commands

        public ICommand ReadDistanceDataPathCommand { get; set; }
        public ICommand ReadDistanceResultPathCommand { get; set; }
        
        #endregion

        public DistanceViewModel()
        {
            DistanceParameters = new Distance();
            
            var crossoversNames = Enum.GetNames(typeof(CrossoverMethod)).ToList();
            crossoversNames.Remove("MRC");
            crossoversNames.Remove("MAC");
            foreach (var crossoverName in crossoversNames)
            {
                DistanceParameters.CrossoverCheckBoxStates.Add(new CheckBoxState(crossoverName, true));
            }
            
            var mutationsNames = Enum.GetNames(typeof(MutationMethod)).ToList();
            mutationsNames.Remove("MRM");
            mutationsNames.Remove("MAM");
            foreach (var mutationsName in mutationsNames)
            {
                DistanceParameters.MutationCheckBoxStates.Add(new CheckBoxState(mutationsName,true));
            }
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
        }
    }
}