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
using OptimizationUI.Commands;
using OptimizationUI.Models;

namespace OptimizationUI.ViewModels
{
    public class DistanceViewModel : INotifyPropertyChanged
    {
        #region Fields
        
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties
        
        public Distance DistanceParameters { get; set; }
        public int ProgressBarValue { get; set; } = 0;
        public int ProgressBarMaximum { get; set; } = 100;
        public bool RandomSeed { get; set; } = true;
        public int CurrentSeed { get; set; }
        public string SelectedFilesString { get; set; }
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
        public string Result { get; set; }

        #endregion

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
            OptimizeWithCurrentParametersCommand = new OptimizeWithCurrentParametersCommand();
            
            CancelCommand = new CancelCommand();
        }
    }
}