using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Optimization.GeneticAlgorithms.Crossovers;
using Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers;
using Optimization.GeneticAlgorithms.Eliminations;
using Optimization.GeneticAlgorithms.Initialization;
using Optimization.GeneticAlgorithms.Mutations;
using Optimization.GeneticAlgorithms.Selections;
using Optimization.Parameters;

namespace OptimizationUI.Models
{
    public class Distance :OptimizationParameters, INotifyPropertyChanged
    {

        public override string ResultPath { get; set; } = "..\\..\\..\\..\\Results";
        public override bool MutateParentIfTheSame { get; set; } = true;
        public override bool Use2opt { get; set; } = false;
        public override OptimizationMethod OptimizationMethod { get; set; } = OptimizationMethod.GeneticAlgorithm;
        public override PopulationInitializationMethod PopulationInitializationMethod { get; set; } =
            PopulationInitializationMethod.StandardPathInitialization;
        public override EliminationMethod EliminationMethod { get; set; } = EliminationMethod.Elitism;
        public override int TournamentEliminationParticipantsCount { get; set; } = 8;
        public override SelectionMethod SelectionMethod { get; set; } = SelectionMethod.Tournament;
        public override int TournamentSelectionParticipantsCount { get; set; } = 8;
        public override CrossoverMethod CrossoverMethod { get; set; } = CrossoverMethod.Aex;
        public override MutationMethod MutationMethod { get; set; } = MutationMethod.RSM;
        public override int PopulationSize { get; set; } = 100;
        public override int ChildrenPerGeneration { get; set; } = 80;
        public override int ParentsPerChildren { get; set; } = 2;
        public override double MutationProbability { get; set; } = 30;
        public override int MaxEpoch { get; set; } = 200;
        public override string DataPath { get; set; } = "";
        public List<CheckBoxState> CrossoverCheckBoxStates { get; set; }
        public List<CheckBoxState> MutationCheckBoxStates { get; set; }
        public override CrossoverMethod[] MultiCrossovers
        {
            get
            {
                var temp = new List<CrossoverMethod>();
                foreach (var checkBoxState in CrossoverCheckBoxStates)
                {
                    if (checkBoxState.CheckBoxValue == true)
                    {
                        temp.Add((CrossoverMethod) Enum.Parse(typeof(CrossoverMethod),checkBoxState.CheckBoxText));
                    }
                }
                return temp.ToArray();
            }
        }
        
        public override MutationMethod[] MultiMutations
        {
            get
            {
                var temp = new List<MutationMethod>();
                foreach (var checkBoxState in MutationCheckBoxStates)
                {
                    if (checkBoxState.CheckBoxValue == true)
                    {
                        temp.Add((MutationMethod) Enum.Parse(typeof(MutationMethod),checkBoxState.CheckBoxText));
                    }
                }
                return temp.ToArray();
            }
        }
        
        public override bool StopAfterEpochsWithoutChange { get; set; }
        public override int StopAfterEpochCount { get; set; }
        public override bool EnableCataclysm { get; set; }
        public override int CataclysmEpoch { get; set; } = 200;
        public override int DeathPercentage { get; set; } = 90;
        public override ConflictResolveMethod ConflictResolveMethod { get; set; } = ConflictResolveMethod.Random;
        public override ConflictResolveMethod RandomizedResolveMethod { get; set; } = ConflictResolveMethod.Random;
        public override double ResolveRandomizationProbability { get; set; } = 0d;
        public override bool IncrementMutationEnabled { get; set; } = false;
        public override double IncrementMutationDelta { get; set; } = 1;
        public override int IncrementMutationEpochs { get; set; } = 1;
        
        #region Visibilities

        public Visibility IsCrossoverCheckBoxVisible => 
            CrossoverMethod is CrossoverMethod.MAC or CrossoverMethod.MRC 
                ? Visibility.Visible : Visibility.Collapsed;
        
        public Visibility IsMutationCheckBoxVisible => 
            MutationMethod is MutationMethod.MAM or MutationMethod.MRM 
                ? Visibility.Visible : Visibility.Collapsed;
        
        public Visibility IsCataclysmVisible => 
            EnableCataclysm ? Visibility.Visible : Visibility.Collapsed;

        public Visibility IsIncrementalMutationDeltaVisible => 
            IncrementMutationEnabled ? Visibility.Visible : Visibility.Collapsed;
        
        public Visibility IsTournamentSelectionSelected => 
            SelectionMethod == SelectionMethod.Tournament 
                ? Visibility.Visible : Visibility.Collapsed;

        public Visibility IsTournamentEliminationSelected => 
            EliminationMethod == EliminationMethod.Tournament 
                ? Visibility.Visible : Visibility.Collapsed;

        public Visibility IsAllVisible => 
            OptimizationMethod == OptimizationMethod.GeneticAlgorithm 
                ? Visibility.Visible : Visibility.Collapsed;

        #endregion
        
        #region CheckboxItems
        
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

        
        public event PropertyChangedEventHandler PropertyChanged;

        public Distance()
        {
            var crossoversNames = Enum.GetNames(typeof(CrossoverMethod)).ToList();
            crossoversNames.Remove("MRC");
            crossoversNames.Remove("MAC");
            CrossoverCheckBoxStates = new List<CheckBoxState>();
            foreach (var crossoverName in crossoversNames)
            {
                CrossoverCheckBoxStates.Add(new CheckBoxState(crossoverName, true));
            }

            MutationCheckBoxStates = new List<CheckBoxState>();
            var mutationsNames = Enum.GetNames(typeof(MutationMethod)).ToList();
            mutationsNames.Remove("MRM");
            mutationsNames.Remove("MAM");
            foreach (var mutationsName in mutationsNames)
            {
                MutationCheckBoxStates.Add(new CheckBoxState(mutationsName,true));
            }
        }
        
    }

}