using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
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
using Optimization.GeneticAppliances.TSP;
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
            WarehouseFitnessPanel.DataContext =
                _properties.WarehouseViewModel.FitnessGeneticAlgorithmParameters as DistanceViewModel;
            WarehouseStackPanel.DataContext =
                _properties.WarehouseViewModel.WarehouseGeneticAlgorithmParameters as DistanceViewModel;
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
            TSPResult[] results = new TSPResult[runs];
            double[][][] runFitnesses = new double[runs][][];

            _cancellationTokenSource = new CancellationTokenSource();
            _properties.DistanceViewModel.ProgressBarMaximum = runs - 1;
            CancellationToken ct = _cancellationTokenSource.Token;
            try
            {
                await Task.Run(() =>
                {
                    Parallel.For(0, runs, i =>
                    {
                        results[i] = OptimizationWork.TSP(parameters, ct);
                        _properties.DistanceViewModel.ProgressBarValue = i;
                        runFitnesses[i] = results[i].fitness;

                    });
                }, ct);
                
                Dispatcher.Invoke(() =>
                {
                    DistanceResultLabel.Content =
                        $"Avg: {results.Average(x=>x.FinalFitness)}  " +
                        $"Max: {results.Max(x=>x.FinalFitness)}  " +
                        $"Min: {results.Min(x=>x.FinalFitness)}  " +
                        $"Avg epoch count: {results.Average(x=>x.EpochCount)}";
                    WritePlotDistances(linesGridDistances, GetAverageFitnesses(runFitnesses));
                });
            }
            catch (AggregateException)
            {
                DistanceResultLabel.Content = "Cancelled";
            }


        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken ct = _cancellationTokenSource.Token;
            double result = -1d;
            double[][] fitness = null;

            try
            {
                await Task.Run(() =>
                {
                    WarehouseParameters warehouseParameters = _properties.WarehouseViewModel as WarehouseParameters;
                    result = Optimization.OptimizationWork.WarehouseOptimization(warehouseParameters, ct);
                    fitness = ReadFitness();
                }, ct);

                Dispatcher.Invoke(() =>
                {
                    WarehouseResultLabel.Content = $"Wynik: {result}";
                    WritePlotWarehouse(linesGridWarehouse, fitness);
                });
            }
            catch (OperationCanceledException exception)
            {
                Dispatcher.Invoke(() => { WarehouseResultLabel.Content = "Cancelled"; });
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

        private void RefreshDistanceGraph(object sender, RoutedEventArgs e)
        {
            RefreshLinesDistances(linesGridDistances);
        }
        
        private void RefreshWarehouseGraph(object sender, RoutedEventArgs e)
        {
            RefreshLinesWarehouse(linesGridWarehouse);
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
            string jsonString = JsonSerializer.Serialize(_properties, options);
            if (File.Exists("properties.json"))
                File.Delete("properties.json");
            File.WriteAllText("properties.json", jsonString);
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
            _properties.WarehouseViewModel.WarehousePath = fileDialog.FileName;
        }

        private void WarehouseOrdersPathButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "txt files (*.txt)|*.txt";
            fileDialog.RestoreDirectory = true;
            fileDialog.ShowDialog();
            _properties.WarehouseViewModel.OrdersPath = fileDialog.FileName;
        }

        private double[][] GetAverageFitnesses(double[][][] runFitnesses)
        {
            double[][][] expandedFitness = new double[runFitnesses.Length][][];

            int[] lengths = new int[runFitnesses.Length];
            for (int i = 0; i < runFitnesses.Length; i++)
            {
                lengths[i] = runFitnesses[i].Length;
            }
            int epoch = lengths.Max();
            
            for (int i = 0; i < runFitnesses.Length; i++)
            {
                expandedFitness[i] = new double[epoch][];
            }

            for (int j = 0; j < runFitnesses.Length; j++)
            {
                for (int i = 0; i < epoch; i++)
                {
                    expandedFitness[j][i] = new double[runFitnesses[0][0].Length];
                }
            }

            for (int i = 0; i < expandedFitness.Length; i++)
            {
                for (int j = 0; j < expandedFitness[0].Length; j++)
                {
                    for (int k = 0; k < expandedFitness[0][0].Length; k++)
                    {
                        if (j >= lengths[i])
                        {
                            expandedFitness[i][j][k] = runFitnesses[i][lengths[i] - 1][k];
                        }
                        else
                        {
                            expandedFitness[i][j][k] = runFitnesses[i][j][k];
                        }
                    }
                }
            }


            double[][] fitness = new double[epoch][];

            for (int i = 0; i < epoch; i++)
            {
                fitness[i] = new double[runFitnesses[0][0].Length];
            }
            

            for (int i = 0; i < epoch; i++)
            {
                for (int j = 0; j < runFitnesses[0][0].Length; j++)
                {
                    fitness[i][j] = expandedFitness.Average(x => x[i][j]);
                }
            }

            return fitness;
        }

        private double[][] ReadFitness()
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

            return fitness;
        }

        private void WritePlotDistances(Grid linesGrid, double[][] fitness)
        {
            linesGrid.Children.Clear();
            int epoch = fitness.Length;

            var x = Enumerable.Range(0, epoch).ToArray();
            double[] y = new double[epoch];
            double[] z = new double[epoch];
            double[] a = new double[epoch];
            double[] m = new double[epoch];
            for (int i = 0; i < epoch; i++)
            {
                y[i] = fitness[i].Min();
                a[i] = fitness[i].Max();
                z[i] = fitness[i].Average();
                m[i] = fitness[i].OrderBy(j => j).Skip((int)(Convert.ToInt32(DistancesPercentText.Text) * 0.01 * fitness[i].Length)).Take(Convert.ToInt32(DistancesIndividualsText.Text)).Average();
            }

            var lineBest = new LineGraph
            {
                Stroke = new SolidColorBrush(Colors.Green),
                Description = "Best fitness",
                StrokeThickness = 1,
            };
            var lineAvg = new LineGraph
            {
                Stroke = new SolidColorBrush(Colors.Blue),
                Description = "Avg fitness",
                StrokeThickness = 1
            };
            var lineWorst = new LineGraph
            {
                Stroke = new SolidColorBrush(Colors.Red),
                Description = "Worst fitness",
                StrokeThickness = 1
            };
            var lineCustom = new LineGraph
            {
                Stroke = new SolidColorBrush(Colors.Purple),
                Description = "Selected fitness",
                StrokeThickness = 1
            };
            
            lineBest.Plot(x, y);
            lineAvg.Plot(x,z);
            lineWorst.Plot(x, a);
            lineCustom.Plot(x, m);

            lineBest.Visibility = _properties.DistanceViewModel.ShowBest ? Visibility.Visible : Visibility.Hidden;
            lineAvg.Visibility = _properties.DistanceViewModel.ShowAvg ? Visibility.Visible : Visibility.Hidden;
            lineWorst.Visibility = _properties.DistanceViewModel.ShowWorst ? Visibility.Visible : Visibility.Hidden;
            lineCustom.Visibility = _properties.DistanceViewModel.ShowCustom ? Visibility.Visible : Visibility.Hidden;
            
            linesGrid.Children.Add(lineBest);
            linesGrid.Children.Add(lineAvg);
            linesGrid.Children.Add(lineWorst);
            linesGrid.Children.Add(lineCustom);

            distancesChart.Content = linesGrid;
            distancesChart.UpdateLayout();

        }
        
        private void RenderChartDistances(object sender, RoutedEventArgs e)
        {

            RenderTargetBitmap bmp = new RenderTargetBitmap(3000, 2450, 720, 660, PixelFormats.Pbgra32);
            bmp.Render(distancesChart);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            
            string name = "distances_chart_" + DateTime.Now + ".png";
            name = name.Replace(" ", "_").Replace(":", ".");
            
            Assembly asm = Assembly.GetExecutingAssembly();
            string path = System.IO.Path.GetDirectoryName(asm.Location);

            using (Stream stm = File.Create(name)) { encoder.Save(stm); }

            DistanceSavingChartLabel.Text = "Saving successful to " + path + "\\" + name;
        }
        
        private void RenderChartWarehouse(object sender, RoutedEventArgs e)
        {

            RenderTargetBitmap bmp = new RenderTargetBitmap(3000, 2450, 720, 660, PixelFormats.Pbgra32);
            bmp.Render(warehouseChart);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            string name = "warehouse_chart_" + DateTime.Now + ".png";
            name = name.Replace(" ", "_").Replace(":", ".");
            
            Assembly asm = Assembly.GetExecutingAssembly();
            string path = System.IO.Path.GetDirectoryName(asm.Location);

            using (Stream stm = File.Create(name)) { encoder.Save(stm); }

            WarehouseSavingChartLabel.Text = "Saving successful to " + path + "\\" + name;
        }
        
        private void WritePlotWarehouse(Grid linesGrid, double[][] fitness)
        {
            linesGrid.Children.Clear();
            int epoch = fitness.Length;

            var x = Enumerable.Range(0, epoch).ToArray();
            double[] y = new double[epoch];
            double[] z = new double[epoch];
            double[] a = new double[epoch];
            double[] m = new double[epoch];
            for (int i = 0; i < epoch; i++)
            {
                y[i] = fitness[i].Min();
                a[i] = fitness[i].Max();
                z[i] = fitness[i].Average();
                m[i] = fitness[i].OrderBy(j => j)
                    .Skip((int)(Convert.ToInt32(WarehousePercentText.Text) * 0.01 * fitness[i].Length))
                    .Take(Convert.ToInt32(WarehouseIndividualsText.Text)).Average();
            }

            var lineBest = new LineGraph
            {
                Stroke = new SolidColorBrush(Colors.Green),
                Description = "Best fitness",
                StrokeThickness = 1,
            };
            var lineAvg = new LineGraph
            {
                Stroke = new SolidColorBrush(Colors.Blue),
                Description = "Avg fitness",
                StrokeThickness = 1
            };
            var lineWorst = new LineGraph
            {
                Stroke = new SolidColorBrush(Colors.Red),
                Description = "Worst fitness",
                StrokeThickness = 1
            };
            var lineCustom = new LineGraph
            {
                Stroke = new SolidColorBrush(Colors.Purple),
                Description = "Selected fitness",
                StrokeThickness = 1
            };
            lineBest.Plot(x, y);
            lineAvg.Plot(x,z);
            lineWorst.Plot(x, a);
            lineCustom.Plot(x, m);

            lineBest.Visibility = _properties.WarehouseViewModel.ShowBest ? Visibility.Visible : Visibility.Hidden;
            lineAvg.Visibility = _properties.WarehouseViewModel.ShowAvg ? Visibility.Visible : Visibility.Hidden;
            lineWorst.Visibility = _properties.WarehouseViewModel.ShowWorst ? Visibility.Visible : Visibility.Hidden;
            lineCustom.Visibility = _properties.WarehouseViewModel.ShowCustom ? Visibility.Visible : Visibility.Hidden;
            
            linesGrid.Children.Add(lineBest);
            linesGrid.Children.Add(lineAvg);
            linesGrid.Children.Add(lineWorst);
            linesGrid.Children.Add(lineCustom);
        }

        private void RefreshLinesDistances(Grid linesGrid)
        {
            if (linesGrid.Children.Count > 0)
            {
                linesGrid.Children[0].Visibility =
                    _properties.DistanceViewModel.ShowBest ? Visibility.Visible : Visibility.Hidden;
                linesGrid.Children[1].Visibility =
                    _properties.DistanceViewModel.ShowAvg ? Visibility.Visible : Visibility.Hidden;
                linesGrid.Children[2].Visibility =
                    _properties.DistanceViewModel.ShowWorst ? Visibility.Visible : Visibility.Hidden;
                linesGrid.Children[3].Visibility =
                    _properties.DistanceViewModel.ShowCustom ? Visibility.Visible : Visibility.Hidden;
            }
        }
        
        private void RefreshLinesWarehouse(Grid linesGrid)
        {
            if (linesGrid.Children.Count > 0)
            {
                linesGrid.Children[0].Visibility =
                    _properties.WarehouseViewModel.ShowBest ? Visibility.Visible : Visibility.Hidden;
                linesGrid.Children[1].Visibility =
                    _properties.WarehouseViewModel.ShowAvg ? Visibility.Visible : Visibility.Hidden;
                linesGrid.Children[2].Visibility =
                    _properties.WarehouseViewModel.ShowWorst ? Visibility.Visible : Visibility.Hidden;
                linesGrid.Children[3].Visibility =
                    _properties.WarehouseViewModel.ShowCustom ? Visibility.Visible : Visibility.Hidden;
            }
        }
    }
}