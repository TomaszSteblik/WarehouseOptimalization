using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
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
using InteractiveDataDisplay.WPF;
using Microsoft.Win32;
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
        private CancellationTokenSource _cancellationTokenSource;
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

        private async void DistanceStartButtonClick(object sender, RoutedEventArgs e)
        {
            OptimizationParameters parameters = _properties.DistanceViewModel as OptimizationParameters;
            int runs = Int32.Parse(DistanceInstancesTextBox.Text);
            double[] results = new double[runs];
            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken ct = _cancellationTokenSource.Token;
            try
            {
                await Task.Run(() =>
                {
                    results[0] = OptimizationWork.FindShortestPath(parameters, ct);
                    
                    for (int i = 0; i < runs; i++)
                    {
                        results[i] = OptimizationWork.FindShortestPath(parameters, ct);
                    }
                    
                }, ct);
                Dispatcher.Invoke(() =>
                {
                    DistanceResultLabel.Content = $"Avg: {results.Average()}  Max: {results.Max()}  Min: {results.Min()}"; 
                    WritePlot(linegraphPathFinding);
                });
            }
            catch (OperationCanceledException exception)
            {
                DistanceResultLabel.Content = "Cancelled";
            }

            
        }
        
        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken ct = _cancellationTokenSource.Token;
            double result = -1d;

            try
            {
                await Task.Run(() =>
                {
                    WarehouseParameters warehouseParameters = _properties.WarehouseViewModel as WarehouseParameters;
                    result = Optimization.OptimizationWork.WarehouseOptimization(warehouseParameters, ct);
                }, ct);
            
                Dispatcher.Invoke(() =>
                {
                    WarehouseResultLabel.Content = $"Wynik: {result}";
                    WritePlot(linegraphWarehouse);
                });
            }
            catch (OperationCanceledException exception)
            {
                Dispatcher.Invoke(() =>
                {
                    WarehouseResultLabel.Content = "Cancelled";
                    
                });
            }
        }
        

        private void CancelWarehouse(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }
        
        private void CancelDistance(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource.Cancel();
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

        private void ReadDistanceDataPathButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "txt files (*.txt)|*.txt";
            fileDialog.RestoreDirectory = true;
            fileDialog.ShowDialog();
            _properties.DistanceViewModel.DataPath = fileDialog.FileName;
        }

        private void WarehouseMagPathButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "txt files (*.txt)|*.txt";
            fileDialog.RestoreDirectory = true;
            fileDialog.ShowDialog();
            _properties.WarehouseViewModel.WarehousePath = fileDialog.FileName;        }

        private void WarehouseOrdersPathButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "txt files (*.txt)|*.txt";
            fileDialog.RestoreDirectory = true;
            fileDialog.ShowDialog();
            _properties.WarehouseViewModel.OrdersPath = fileDialog.FileName; 
        }

        private void WritePlot(LineGraph linegraph)
        {
            string[] lines = File.ReadAllLines("fitness.csv");
            int nonEmptyLines = 0;
            for (int i = 0; i < lines.Length; i++)
                if (lines[i].Trim().Length > 1)
                    nonEmptyLines++;
            double[][] fitness = new double[nonEmptyLines][];
            for (int i = 0; i < nonEmptyLines; i++)
            {
                string[] s = lines[i].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                fitness[i] = Array.ConvertAll(s, double.Parse);
            }
            
            var x = Enumerable.Range(0, nonEmptyLines).ToArray();
            double[] y = new double[nonEmptyLines];
            for (int i = 0; i < nonEmptyLines; i++)
            {
                y[i] = fitness[i][0];
            }
            linegraph.Plot(x,y);
        }
    }
}