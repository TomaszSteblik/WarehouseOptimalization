using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Optimization;
using Optimization.GeneticAlgorithms.Crossovers;
using Optimization.GeneticAlgorithms.Eliminations;
using Optimization.GeneticAlgorithms.Mutations;
using Optimization.GeneticAlgorithms.Selections;
using Optimization.Parameters;

namespace OptimizationUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DistanceViewModel _distanceViewModel;
        private WarehouseViewModel _warehouseViewModel;
        private DistanceViewModel _warehouseDistanceViewModel;
        private DistanceViewModel _warehouseFitnessCalculationDistanceViewModel;
        public MainWindow()
        {
            InitializeComponent();
            
            _distanceViewModel = new DistanceViewModel();
            DistancePanel.DataContext = _distanceViewModel;
            
            _warehouseFitnessCalculationDistanceViewModel = new DistanceViewModel();
            WarehouseFitnessPanel.DataContext = _warehouseFitnessCalculationDistanceViewModel;
            
            _warehouseViewModel = new WarehouseViewModel();
            WarehousePanel.DataContext = _warehouseViewModel;
            
            _warehouseDistanceViewModel = new DistanceViewModel();
            WarehouseStackPanel.DataContext = _warehouseDistanceViewModel;

            _warehouseViewModel.FitnessGeneticAlgorithmParameters = _warehouseFitnessCalculationDistanceViewModel;
            _warehouseViewModel.WarehouseGeneticAlgorithmParameters = _warehouseDistanceViewModel;
            
            var methods = Enum.GetValues(typeof(OptimizationMethod)).Cast<OptimizationMethod>().ToList();
            var selections = Enum.GetValues(typeof(SelectionMethod)).Cast<SelectionMethod>().ToList();
            var crossovers = Enum.GetValues(typeof(CrossoverMethod)).Cast<CrossoverMethod>().ToList();
            var eliminations = Enum.GetValues(typeof(EliminationMethod)).Cast<EliminationMethod>().ToList();
            var mutations = Enum.GetValues(typeof(MutationMethod)).Cast<MutationMethod>().ToList();
            
            DistanceMethodComboBox.ItemsSource = methods;
            DistanceSelectionComboBox.ItemsSource = selections;
            DistanceCrossoverComboBox.ItemsSource = crossovers;
            DistanceEliminationComboBox.ItemsSource = eliminations;
            DistanceMutationComboBox.ItemsSource = mutations;

            WarehouseSelectionComboBox.ItemsSource = selections;
            WarehouseCrossoverComboBox.ItemsSource = crossovers;
            WarehouseEliminationComboBox.ItemsSource = eliminations;
            WarehouseMutationComboBox.ItemsSource = mutations;
            
            WarehouseFitnessMethodComboBox.ItemsSource = methods;
            WarehouseFitnessSelectionComboBox.ItemsSource = selections;
            WarehouseFitnessCrossoverComboBox.ItemsSource = crossovers;
            WarehouseFitnessEliminationComboBox.ItemsSource = eliminations;
            WarehouseFitnessMutationComboBox.ItemsSource = mutations;
        }

        private void DistanceStartButtonClick(object sender, RoutedEventArgs e)
        {
            OptimizationParameters parameters = _distanceViewModel as OptimizationParameters;
            List<double> results = new List<double>();
            for (int i = 0; i < Double.Parse(DistanceInstancesTextBox.Text); i++)
            {
                results.Add(OptimizationWork.FindShortestPath(parameters));
            }

            DistanceResultLabel.Content = $"Avg: {results.Average()}  Max: {results.Max()}  Min: {results.Min()}";
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            WarehouseParameters warehouseParameters = _warehouseViewModel as WarehouseParameters;

            var result = Optimization.OptimizationWork.WarehouseOptimization(warehouseParameters);
            WarehouseResultLabel.Content = $"Wynik: {result}";

        }
    }
}