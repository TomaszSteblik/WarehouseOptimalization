using Optimization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Windows;
using Optimization.GeneticAlgorithms.Crossovers;
using Optimization.GeneticAlgorithms.Eliminations;
using Optimization.GeneticAlgorithms.Mutations;
using Optimization.GeneticAlgorithms.Selections;
using Optimization.Parameters;

namespace OptimizationUI
{
    public class DistanceViewModel :OptimizationParameters, INotifyPropertyChanged
    {
        private bool _use2opt = false;
        public override bool Use2opt
        {
            get
            {
                return _use2opt;
            }
            set
            {
                _use2opt = value;
                NotifyPropertyChanged();
            }
        }

        private bool _showCustom = true;

        public bool ShowCustom
        {
            get => _showCustom;
            set
            {
                _showCustom = value;
                NotifyPropertyChanged();
            }
        }

        private bool _showBest = true;

        public bool ShowBest
        {
            get => _showBest;
            set

            {
                _showBest = value;
                NotifyPropertyChanged();
            }
        }
        private bool _showAvg = true;

        public bool ShowAvg
        {
            get => _showAvg;
            set

            {
                _showAvg = value;
                NotifyPropertyChanged();
            }
        }
        private bool _showWorst = true;

        public bool ShowWorst
        {
            get => _showWorst;
            set

            {
                _showWorst = value;
                NotifyPropertyChanged();
            }
        }

        private OptimizationMethod _optimizationMethod = OptimizationMethod.GeneticAlgorithm;
        public override OptimizationMethod OptimizationMethod
        {
            get
            {
                return _optimizationMethod;
            }
            set
            {
                _optimizationMethod = value;
                NotifyPropertyChanged("IsAllVisible");
                NotifyPropertyChanged();
            }
        }

        public Visibility IsAllVisible
        {
            get
            {
                if (_optimizationMethod == OptimizationMethod.GeneticAlgorithm)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }

        private EliminationMethod _eliminationMethod = EliminationMethod.Elitism;
        public override EliminationMethod EliminationMethod
        {
            get
            {
                return _eliminationMethod;
            }
            set
            {
                _eliminationMethod = value;
                NotifyPropertyChanged();
            }
        }

        private SelectionMethod _selectionMethod = SelectionMethod.Tournament;
        public override SelectionMethod SelectionMethod
        {
            get
            {
                return _selectionMethod;
            }
            set
            {
                _selectionMethod = value;
                NotifyPropertyChanged();
            }
        }

        private CrossoverMethod _crossoverMethod = CrossoverMethod.Aex;
        public override CrossoverMethod CrossoverMethod
        {
            get
            {
                return _crossoverMethod;
            }
            set
            {
                _crossoverMethod = value;
                NotifyPropertyChanged();
            }
        }

        private MutationMethod _mutationMethod = MutationMethod.RSM;
        public override MutationMethod MutationMethod
        {
            get
            {
                return _mutationMethod;
            }
            set
            {
                _mutationMethod = value;
                NotifyPropertyChanged();
            }
        }

        private int _populationSize = 100;
        public override int PopulationSize
        {
            get
            {
                return _populationSize;
            }
            set
            {
                _populationSize = value;
                NotifyPropertyChanged();
            }
        }

        private int _childrenPerGeneration = 20;
        public override int ChildrenPerGeneration
        {
            get
            {
                return _childrenPerGeneration;
            }
            set
            {
                _childrenPerGeneration = value;
                NotifyPropertyChanged();
            }
        }

        private int _parentsPerChildren = 2;
        public override int ParentsPerChildren
        {
            get
            {
                return _parentsPerChildren;
            }
            set
            {
                _parentsPerChildren = value;
                NotifyPropertyChanged();
            }
        }

        private double _mutationProbability = 30;
        public override double MutationProbability
        {
            get
            {
                return _mutationProbability;
            }
            set
            {
                _mutationProbability = value;
                NotifyPropertyChanged();
            }
        }

        private int _maxEpoch = 200;
        public override int MaxEpoch
        {
            get
            {
                return _maxEpoch;
            }
            set
            {
                _maxEpoch = value;
                NotifyPropertyChanged();
            }
        }

        private string _dataPath = "";
        public override string DataPath
        {
            get
            {
                return _dataPath;
            }
            set
            {
                _dataPath = value;
                NotifyPropertyChanged();
            }
        }
        
        private List<CheckBoxState> _crossoverCheckBoxStates = new List<CheckBoxState>();
        public List<CheckBoxState> CrossoverCheckBoxStates
        {
            get => _crossoverCheckBoxStates;
            set
            {
                _crossoverCheckBoxStates = value;
                NotifyPropertyChanged();
            }
        }

        public override CrossoverMethod[] MultiCrossovers
        {
            get
            {
                var temp = new List<CrossoverMethod>();
                foreach (var checkBoxState in _crossoverCheckBoxStates)
                {
                    if (checkBoxState.CheckBoxValue == true)
                    {
                        temp.Add((CrossoverMethod) Enum.Parse(typeof(CrossoverMethod),checkBoxState.CheckBoxText));
                    }
                }
                return temp.ToArray();
            }
        }

        public Visibility IsCrossoverCheckBoxVisible
        {
            get
            {
                if (_crossoverMethod == CrossoverMethod.MAC
                    || _crossoverMethod == CrossoverMethod.MRC)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        
        private List<CheckBoxState> _mutationCheckBoxStates = new List<CheckBoxState>();
        

        public List<CheckBoxState> MutationCheckBoxStates
        {
            get => _mutationCheckBoxStates;
            set
            {
                _mutationCheckBoxStates = value;
                NotifyPropertyChanged();
            }
        }
        
        public override MutationMethod[] MultiMutations
        {
            get
            {
                var temp = new List<MutationMethod>();
                foreach (var checkBoxState in _mutationCheckBoxStates)
                {
                    if (checkBoxState.CheckBoxValue == true)
                    {
                        temp.Add((MutationMethod) Enum.Parse(typeof(MutationMethod),checkBoxState.CheckBoxText));
                    }
                }
                return temp.ToArray();
            }
        }
        
        public Visibility IsMutationCheckBoxVisible
        {
            get
            {
                if (_mutationMethod == MutationMethod.MAM
                    || _mutationMethod == MutationMethod.MRM)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }

        private int _progessBarValue = 0;

        public int ProgressBarValue
        {
            get
            {
                return _progessBarValue;
            }
            set
            {
                _progessBarValue = value;
                NotifyPropertyChanged();
            }
        }

        private int _progessBarMaximum = 100;

        public int ProgressBarMaximum
        {
            get
            {
                return _progessBarMaximum;
            }
            set
            {
                _progessBarMaximum = value;
                NotifyPropertyChanged();
            }
        }

        private bool _stopAfterEpochsWithoutChange = false;
        public override bool StopAfterEpochsWithoutChange
        {
            get => _stopAfterEpochsWithoutChange;
            set
            {
                _stopAfterEpochsWithoutChange = value;
                NotifyPropertyChanged();
            }
        }

        private int _stopAfterEpochCount;

        public override int StopAfterEpochCount
        {
            get => _stopAfterEpochCount;
            set
            {
                _stopAfterEpochCount = value;
                NotifyPropertyChanged();
            }
        }


        public DistanceViewModel(OptimizationParameters fitnessGeneticAlgorithmParameters)
        {
            this._use2opt = fitnessGeneticAlgorithmParameters.Use2opt;
            this._crossoverMethod = fitnessGeneticAlgorithmParameters.CrossoverMethod;
            this._dataPath = fitnessGeneticAlgorithmParameters.DataPath;
            this._eliminationMethod = fitnessGeneticAlgorithmParameters.EliminationMethod;
            this._mutationMethod = fitnessGeneticAlgorithmParameters.MutationMethod;
            this._mutationProbability = fitnessGeneticAlgorithmParameters.MutationProbability;
            this._optimizationMethod = fitnessGeneticAlgorithmParameters.OptimizationMethod;
            this._populationSize = fitnessGeneticAlgorithmParameters.PopulationSize;
            this._selectionMethod = fitnessGeneticAlgorithmParameters.SelectionMethod;
            this._maxEpoch = fitnessGeneticAlgorithmParameters.MaxEpoch;
            this._childrenPerGeneration = fitnessGeneticAlgorithmParameters.ChildrenPerGeneration;
            this._parentsPerChildren = fitnessGeneticAlgorithmParameters.ParentsPerChildren;
            
            
            var crossoversNames = Enum.GetNames(typeof(CrossoverMethod)).ToList();
            crossoversNames.Remove("MRC");
            crossoversNames.Remove("MAC");
            foreach (var crossoverName in crossoversNames)
            {
                CrossoverCheckBoxStates.Add(new CheckBoxState(crossoverName, fitnessGeneticAlgorithmParameters.MultiCrossovers.Contains<CrossoverMethod>((CrossoverMethod) Enum.Parse(typeof(CrossoverMethod),crossoverName))));
            }

            var mutationsNames = Enum.GetNames(typeof(MutationMethod)).ToList();
            mutationsNames.Remove("MRM");
            mutationsNames.Remove("MAM");
            foreach (var mutationsName in mutationsNames)
            {
                MutationCheckBoxStates.Add(new CheckBoxState(mutationsName,fitnessGeneticAlgorithmParameters.MultiMutations.Contains<MutationMethod>((MutationMethod) Enum.Parse(typeof(MutationMethod),mutationsName))));
            }
            
        }

        public DistanceViewModel()
        {
            var crossoversNames = Enum.GetNames(typeof(CrossoverMethod)).ToList();
            crossoversNames.Remove("MRC");
            crossoversNames.Remove("MAC");
            foreach (var crossoverName in crossoversNames)
            {
                CrossoverCheckBoxStates.Add(new CheckBoxState(crossoverName, true));
            }

            var mutationsNames = Enum.GetNames(typeof(MutationMethod)).ToList();
            mutationsNames.Remove("MRM");
            mutationsNames.Remove("MAM");
            foreach (var mutationsName in mutationsNames)
            {
                MutationCheckBoxStates.Add(new CheckBoxState(mutationsName,true));
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CheckBoxState
    {
        public CheckBoxState(string checkBoxText, bool checkBoxValue)
        {
            CheckBoxText = checkBoxText;
            CheckBoxValue = checkBoxValue;
        }

        public string CheckBoxText { get; set; }
        public bool CheckBoxValue { get; set; }
    }
}