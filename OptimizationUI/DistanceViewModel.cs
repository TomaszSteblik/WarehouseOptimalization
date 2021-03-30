using Optimization;
using System;
using System.ComponentModel;
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

        private OptimizationMethod _optimizationMethod = OptimizationMethod.NearestNeighbor;
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

        private int _terminationValue = 200;
        public override int TerminationValue
        {
            get
            {
                return _terminationValue;
            }
            set
            {
                _terminationValue = value;
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}