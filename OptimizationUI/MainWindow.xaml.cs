using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
        private Properties _properties;
        public MainWindow()
        {
            InitializeComponent();
            DeserializeParameters();
            DistancePanel.DataContext = _properties.DistanceViewModel;
            WarehouseFitnessPanel.DataContext = _properties.WarehouseViewModel.FitnessGeneticAlgorithmParameters as DistanceViewModel;
            WarehouseStackPanel.DataContext = _properties.WarehouseViewModel.WarehouseGeneticAlgorithmParameters as DistanceViewModel;
            WarehousePanel.DataContext = _properties.WarehouseViewModel;

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
            OptimizationParameters parameters = _properties.DistanceViewModel as OptimizationParameters;
            List<double> results = new List<double>();
            for (int i = 0; i < Double.Parse(DistanceInstancesTextBox.Text); i++)
            {
                results.Add(OptimizationWork.FindShortestPath(parameters));
            }

            DistanceResultLabel.Content = $"Avg: {results.Average()}  Max: {results.Max()}  Min: {results.Min()}";
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            WarehouseParameters warehouseParameters = _properties.WarehouseViewModel as WarehouseParameters;

            var result = Optimization.OptimizationWork.WarehouseOptimization(warehouseParameters);
            WarehouseResultLabel.Content = $"Wynik: {result}";

        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            SerializeParameters();
        }

        private void SerializeParameters()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            string jsonString = JsonSerializer.Serialize(_properties,options);
            if(File.Exists("properties.json"))
                File.Delete("properties.json");
            File.WriteAllText("properties.json",jsonString);
        }

        private void DeserializeParameters()
        {
            string location = "properties.json";
            if (File.Exists(location))
            {
                string jsonString = File.ReadAllText(location);
                _properties = JsonSerializer.Deserialize<Properties>(jsonString);
                _properties.WarehouseViewModel.FitnessGeneticAlgorithmParameters =
                    new DistanceViewModel(_properties.WarehouseViewModel.FitnessGeneticAlgorithmParameters);
                _properties.WarehouseViewModel.WarehouseGeneticAlgorithmParameters =
                    new DistanceViewModel(_properties.WarehouseViewModel.WarehouseGeneticAlgorithmParameters);
            }
            else
            {
                _properties = new Properties();
            }
            
        }
    }
}