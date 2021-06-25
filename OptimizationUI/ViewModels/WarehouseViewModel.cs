using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;
using Optimization.GeneticAlgorithms.Crossovers;
using Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers;
using Optimization.GeneticAlgorithms.Eliminations;
using Optimization.GeneticAlgorithms.Initialization;
using Optimization.GeneticAlgorithms.Mutations;
using Optimization.GeneticAlgorithms.Selections;
using Optimization.Parameters;
using OptimizationUI.Annotations;
using OptimizationUI.Commands;
using OptimizationUI.Models;
using OxyPlot;
using OxyPlot.Wpf;

namespace OptimizationUI.ViewModels
{
    public class WarehouseViewModel : INotifyPropertyChanged
    {
        #region Properties

        public Warehouse Warehouse { get; set; }
        public Distance FitnessGeneticAlgorithmParameters { get; set; }
        public Distance WarehouseGeneticAlgorithmParameters { get; set; }
        public CancellationTokenSource CancelationTokenSource { get; set; }

        public int Runs
        {
            get => _runs;
            set
            {
                _runs = value;
                OnPropertyChanged();
            }
        }

        public PlotModel PlotModel
        {
            get => _plotModel;
            set
            {
                _plotModel = value;
                OnPropertyChanged();
            }
        }


        #endregion
        
        #region ChartProperties

        private int _runs = 10;

        private PlotModel _plotModel;
        public bool ShowCustom { get; set; } = true;
        public bool ShowBest { get; set; } = true;
        public bool ShowAvg { get; set; } = true;
        public bool ShowWorst { get; set; } = true;

        #endregion
        
        #region UiProperties

        public string Result { get; set; }

        #endregion
        
        #region Commands

        public ICommand CancelCommand { get; set; }
        public ICommand WarehouseMagPathCommand { get; set; }
        public ICommand WarehouseOrdersPathCommand { get; set; }
        public ICommand RunWarehouseCommand { get; set; }

        #endregion

        public WarehouseViewModel()
        {
            Warehouse = new Warehouse();
            FitnessGeneticAlgorithmParameters = new Distance();
            WarehouseGeneticAlgorithmParameters = new Distance();
            Warehouse.FitnessGeneticAlgorithmParameters = FitnessGeneticAlgorithmParameters;
            Warehouse.WarehouseGeneticAlgorithmParameters = WarehouseGeneticAlgorithmParameters;
            
            SetupCommands();
        }

        private void SetupCommands()
        {
            CancelCommand = new CancelCommand();
            WarehouseMagPathCommand = new WarehouseMagPathCommand();
            WarehouseOrdersPathCommand = new WarehouseOrdersPathCommand();
            RunWarehouseCommand = new RunWarehouseCommand();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}