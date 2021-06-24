using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
    public class WarehouseViewModel
    {
        #region Properties

        public Warehouse Warehouse { get; set; }
        public Distance FitnessGeneticAlgorithmParameters { get; set; }
        public Distance WarehouseGeneticAlgorithmParameters { get; set; }
        public CancellationTokenSource CancelationTokenSource { get; set; }


        #endregion
        
        #region ChartProperties

        public bool ShowCustom { get; set; } = true;
        public bool ShowBest { get; set; } = true;
        public bool ShowAvg { get; set; } = true;
        public bool ShowWorst { get; set; } = true;

        #endregion
        
        #region UiProperties

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
        
        #region Commands

        public ICommand CancelCommand { get; set; }
        public ICommand WarehouseMagPathCommand { get; set; }
        public ICommand WarehouseOrdersPathCommand { get; set; }

        #endregion

        public WarehouseViewModel()
        {
            Warehouse = new Warehouse();
            FitnessGeneticAlgorithmParameters = new Distance();
            WarehouseGeneticAlgorithmParameters = new Distance();
            Warehouse.FitnessGeneticAlgorithmParameters = FitnessGeneticAlgorithmParameters;
            Warehouse.WarehouseGeneticAlgorithmParameters = WarehouseGeneticAlgorithmParameters;
        }

        private void SetupCommands()
        {
            CancelCommand = new CancelCommand();
            WarehouseMagPathCommand = new WarehouseMagPathCommand();
            WarehouseOrdersPathCommand = new WarehouseOrdersPathCommand();
        }

    }
}