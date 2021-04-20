using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
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
using Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers;
using Optimization.GeneticAlgorithms.Eliminations;
using Optimization.GeneticAlgorithms.Initialization;
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
            DeserializeParameters("properties.json");
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
            var initializations = Enum.GetValues(typeof(PopulationInitializationMethod))
                .Cast<PopulationInitializationMethod>().ToList();
            var conflictResolvers =
                Enum.GetValues(typeof(ConflictResolveMethod)).Cast<ConflictResolveMethod>().ToList();

            DistanceMethodComboBox.ItemsSource = methods;
            DistanceSelectionComboBox.ItemsSource = selections;
            DistanceCrossoverComboBox.ItemsSource = crossovers;
            DistanceEliminationComboBox.ItemsSource = eliminations;
            DistanceMutationComboBox.ItemsSource = mutations;
            DistancePopulationInitializationMethod.ItemsSource = initializations;
            DistanceConflictResolveComboBox.ItemsSource = conflictResolvers;
            DistanceRandomizedResolveComboBox.ItemsSource = conflictResolvers;

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
            DistanceStartButton.IsEnabled = false;
            EventHandler<int> BaseGeneticOnOnNextIteration()
            {
                return (sender,iteration) =>
                {
                    _properties.DistanceViewModel.ProgressBarValue++;
                };
            }

            OptimizationParameters parameters = _properties.DistanceViewModel as OptimizationParameters;
            int runs = Int32.Parse(DistanceRunsTextBox.Text);
            int seed = Int32.Parse(DistanceSeedTextBox.Text);
            TSPResult[] results = new TSPResult[runs];
            double[][][] runFitnesses = new double[runs][][];

            _cancellationTokenSource = new CancellationTokenSource();
            _properties.DistanceViewModel.ProgressBarMaximum = runs*_properties.DistanceViewModel.MaxEpoch - 1;
            _properties.DistanceViewModel.ProgressBarValue = 0;
            Optimization.GeneticAlgorithms.BaseGenetic.OnNextIteration += BaseGeneticOnOnNextIteration();
            CancellationToken ct = _cancellationTokenSource.Token;
            if ((OptimizationMethod) DistanceMethodComboBox.SelectedItem == OptimizationMethod.GeneticAlgorithm)
            {
                try
                {
                    await Task.Run(() =>
                    {
                        Parallel.For(0, runs, i =>
                        {
                            if (_properties.DistanceViewModel.RandomSeed)
                            {
                                var rand = new Random();
                                var randomSeed = rand.Next(1, Int32.MaxValue); 
                                results[i] = OptimizationWork.TSP(parameters, ct, randomSeed + i);
                            }
                            else results[i] = OptimizationWork.TSP(parameters, ct, seed + i);
                            runFitnesses[i] = results[i].fitness;

                        });
                    }, ct);

                    Dispatcher.Invoke(() =>
                    {
                        _properties.DistanceViewModel.ProgressBarValue =
                            runs * _properties.DistanceViewModel.MaxEpoch - 1;
                        DistanceResultLabel.Content =
                            $"Avg: {results.Average(x => x.FinalFitness)}  " +
                            $"Max: {results.Max(x => x.FinalFitness)}  " +
                            $"Min: {results.Min(x => x.FinalFitness)}  " +
                            $"Avg epoch count: {results.Average(x => x.EpochCount)}";
                        _properties.DistanceViewModel.CurrentSeed = results[0].Seed;
                        WritePlotDistances(linesGridDistances, GetAverageFitnesses(runFitnesses));
                        
                        SaveDistanceResultsToDataCsv(results,runs,
                            _properties.DistanceViewModel.DataPath.Split('\\')[^1]
                                .Remove(_properties.DistanceViewModel.DataPath.Split('\\')[^1].IndexOf('.'))
                            );
                    });
                }
                catch (AggregateException)
                {
                    DistanceResultLabel.Content = "Cancelled";
                }

                Optimization.GeneticAlgorithms.BaseGenetic.OnNextIteration -= BaseGeneticOnOnNextIteration();
            }
            else
            {
                double result = OptimizationWork.FindShortestPath(parameters);
                DistanceResultLabel.Content =
                    $"Result: {result}";
            }
            DistanceStartButton.IsEnabled = true;
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
            SerializeParameters("properties.json");
        }

        private void SerializeParameters(string path)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            string jsonString = JsonSerializer.Serialize(_properties, options);
            if (File.Exists(path))
                File.Delete(path);
            File.WriteAllText(path, jsonString);
        }

        private void DeserializeParameters(string location)
        {
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
            fileDialog.Filter = "All files (*.*)|*.*|txt files (*.txt)|*.txt|tsp files (*.tsp)|*.tsp";
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

        private void LoadDistanceParamsButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "json files (*.json)|*.json";
            fileDialog.RestoreDirectory = true;
            fileDialog.ShowDialog();
            DeserializeParameters(fileDialog.FileName);
            DistancePanel.DataContext = _properties.DistanceViewModel;
            WarehouseFitnessPanel.DataContext =
                _properties.WarehouseViewModel.FitnessGeneticAlgorithmParameters as DistanceViewModel;
            WarehouseStackPanel.DataContext =
                _properties.WarehouseViewModel.WarehouseGeneticAlgorithmParameters as DistanceViewModel;
            WarehousePanel.DataContext = _properties.WarehouseViewModel;
        }

        private void SaveDistanceParamsButton_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "json files (*.json)|*.json";
            fileDialog.RestoreDirectory = true;
            fileDialog.ShowDialog();
            SerializeParameters(fileDialog.FileName);
        }

        private void SaveDistanceResultsToDataCsv(TSPResult[] results,int runs, string dataset, string id = "default")
        {
            var line = File.Exists("data.csv") ? new StringBuilder() : new StringBuilder("algorithm;dataset;id;runs;distance;d_epoch;d*0.98_epoch;seed\n");
            
            line.Append(Enum.GetName(_properties.DistanceViewModel.CrossoverMethod));
            if(_properties.DistanceViewModel.CrossoverMethod == CrossoverMethod.MAC || _properties.DistanceViewModel.CrossoverMethod == CrossoverMethod.MRC)
            {
                line.Append('(');
                foreach (var crossoverMethod in _properties.DistanceViewModel.MultiCrossovers)
                {
                    line.Append($"{Enum.GetName(crossoverMethod)} ");
                }
                line.Remove(line.Length-1, 1);
                line.Append(')');
            }
            var averageMinEpoch = results.Select(x => x.EpochCount - x.fitness.Count(y => y[0] == x.fitness[^1][0])).Average();


            var z = results.Select(x => x.fitness).ToArray();
            var epochNumbersWhenSlowedDown = new int[z.Length];
            for (var i = 0; i < z.Length; i++)
            {
                var bestsPerEpochs = new double[z[i].Length];
                for (var j = 0; j < z[i].Length; j++)
                {
                    bestsPerEpochs[j] = z[i][j].Min();
                }

                var currentMin = bestsPerEpochs[0];
                var indexOfMin = 0;
                for (var j = 1; j < bestsPerEpochs.Length; j++)
                {
                    if (!(bestsPerEpochs[j] < currentMin * 0.98)) continue;
                    currentMin = bestsPerEpochs[j];
                    indexOfMin = j;
                }

                epochNumbersWhenSlowedDown[i] = indexOfMin;
            }
            
            
            line.Append($";{dataset};{id};{runs};{results.Average(x => x.FinalFitness)};{averageMinEpoch};{epochNumbersWhenSlowedDown.Average()};{results[0].Seed}\n");
            File.AppendAllText("data.csv",line.ToString());
        }

        private void SaveDistanceArticleResultsToFile(string path,TSPResult[] results,
            string conflictResolver, string randomResolver,int seed)
        {
            var headers =
                "dataset;crossover;conflict_resolver;random_resolver;best_distance;avg_top_10%;median;avg_worst_10%;average;worst_distance;std_dev;avg_min_epoch;d*0.98_epoch\n";
            var dataset = _properties.DistanceViewModel.DataPath.Split('\\')[^1]
                .Remove(_properties.DistanceViewModel.DataPath.Split('\\')[^1].IndexOf('.'));
            var s = File.Exists(path) ? "" : headers;
            
            var averageMinEpoch = results.Select(x => x.EpochCount - x.fitness.Count(y => y[0] == x.fitness[^1][0])).Average();
            var z = results.Select(x => x.fitness).ToArray();
            var epochNumbersWhenSlowedDown = new int[z.Length];
            for (var i = 0; i < z.Length; i++)
            {
                var bestsPerEpochs = new double[z[i].Length];
                for (var j = 0; j < z[i].Length; j++)
                {
                    bestsPerEpochs[j] = z[i][j].Min();
                }

                var currentMin = bestsPerEpochs[0];
                var indexOfMin = 0;
                for (var j = 1; j < bestsPerEpochs.Length; j++)
                {
                    if (!(bestsPerEpochs[j] < currentMin * 0.98)) continue;
                    currentMin = bestsPerEpochs[j];
                    indexOfMin = j;
                }

                epochNumbersWhenSlowedDown[i] = indexOfMin;
            }

            s += dataset + ";";
            s += Enum.GetName(_properties.DistanceViewModel.CrossoverMethod) + ";";
            s += conflictResolver + ";";
            s += randomResolver + ";";
            //tego nie jestem do końca pewien, które wartości będą potrzebne - może lepiej ich naprodukować więcej, by mieć z czego wybierać:
            s += results.Min(x => x.FinalFitness) + ";";  //najlepszy wynik
            s += results.OrderBy(x => x.FinalFitness).Take((int)(0.1 * results.Length)).Average(x => x.FinalFitness) + ";"; //średnia z najlepszych 10%
            s += results.OrderBy(x => x.FinalFitness).Skip((int)(0.5 * results.Length)).Take(1).Average(x => x.FinalFitness) + ";";  //mediana
            s += results.OrderBy(x => x.FinalFitness).Skip((int)(0.9 * results.Length)).Take((int)(0.1 * results.Length)).Average(x => x.FinalFitness) + ";"; //średnia z najgorszych 10%
            s += (int)results.Average(x => x.FinalFitness) + ";"; //średnia
            s += results.Max(x => x.FinalFitness) + ";"; // najgorszy wynik
            s += results.StandardDeviation(x => x.FinalFitness) + ";"; // odchylenie standardowe
            s += averageMinEpoch + ";";
            s += epochNumbersWhenSlowedDown.Average() + ";";
            s += "\n";
            
            
            var s2 = File.Exists("data_for_article.csv") ? File.Exists(path) ? s : s.Remove(0,headers.Length) : File.Exists(path) ? headers + s : s;
            File.AppendAllText(path,s);    
            File.AppendAllText($"data_for_article_{seed}.csv",s2);    
        }
        private string CreateDistanceLogsPerRunsParams(TSPResult[] results,string conflictResolver, string randomResolver)
        {
            var fitness = GetAverageFitnesses(results.Select(x => x.fitness).ToArray());
            var z = results.Select(x => x.ResolvePercentInEpoch).ToArray();

            var maxLenght = z.Max(x => x.Length);
            var transponsedZ = new List<List<double>>();
            for (int i = 0; i < maxLenght; i++)
            {
                transponsedZ.Add(new List<double>());
                for (int j = 0; j < z.Length; j++)
                {
                    if(z[j].Length>i) transponsedZ[i].Add(z[j][i]);
                }
            }

            var epochsPerc = transponsedZ.Select(x => x.Average()).ToArray();

            string s = "";
            for (int i = 0; i < fitness.Length; i++)
            {
                var epochFitnesses = fitness[i];
                s += conflictResolver + ";";
                s += randomResolver+ ";";
                s += i+ ";";
                //tego nie jestem do końca pewien, które wartości będą potrzebne - może lepiej ich naprodukować więcej, by mieć z czego wybierać:
                s += epochFitnesses.Min()+ ";";  //najlepszy wynik
                s += epochFitnesses.OrderBy(x => x).Take((int)(0.1 * epochFitnesses.Length)).Average() + ";"; //średnia z najlepszych 10%
                s += epochFitnesses.OrderBy(x => x).Skip((int) (0.5 * epochFitnesses.Length)).Take(1).Average() + ";";  //mediana
                s += epochFitnesses.OrderBy(x => x).Skip((int)(0.9 * epochFitnesses.Length)).Take((int)(0.1 * epochFitnesses.Length)).Average() + ";"; //średnia z najgorszych 10%
                s += (int)epochFitnesses.Average()+ ";"; //średnia
                s += epochFitnesses.Max()+ ";"; // najgorszy wynik
                s += epochFitnesses.StandardDeviation()+ ";"; // odchylenie standardowe
                s += epochsPerc[i] + ";";
                s += "\n";
            }

            return s;
        }

        private async void DistanceArticleStartButton_OnClick(object sender, RoutedEventArgs e)
        {

            DistanceStartButton.IsEnabled = false;

            EventHandler<int> BaseGeneticOnOnNextIteration()
            {
                return (_, iteration) =>
                {
                    _properties.DistanceViewModel.ProgressBarValue++;
                    Console.WriteLine(iteration);
                };
            }


            var files = Directory.GetFiles(_properties.DistanceViewModel.DatasetDirectoryPath);
            var runs = int.Parse(DistanceRunsTextBox.Text);
            int seed = _properties.DistanceViewModel.CurrentSeed;
            if (_properties.DistanceViewModel.RandomSeed)
            {
                Random random = new Random();
                seed = random.Next(1, Int32.MaxValue);
                _properties.DistanceViewModel.CurrentSeed = seed;
            }
            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken ct = _cancellationTokenSource.Token;
            OptimizationParameters parameters = _properties.DistanceViewModel as OptimizationParameters;

            var crossovers = new CrossoverMethod[4];
            crossovers[0] = CrossoverMethod.Aex;
            crossovers[1] = CrossoverMethod.HProX;
            crossovers[2] = CrossoverMethod.HGreX;
            crossovers[3] = CrossoverMethod.HRndX;

            try
            {
                await Task.Run(() =>
                {
                    Directory.CreateDirectory(seed.ToString());
                    SerializeParameters(seed+"/parameters.json");
                
                    _properties.DistanceViewModel.ProgressBarMaximum = (runs*_properties.DistanceViewModel.MaxEpoch*9*4*files.Length) - 1;
                    _properties.DistanceViewModel.ProgressBarValue = 0;
                    Optimization.GeneticAlgorithms.BaseGenetic.OnNextIteration += BaseGeneticOnOnNextIteration();
                
                    foreach (var dataset in files)
                    {
                        var datasetName = dataset.Split('\\')[^1]
                            .Remove(_properties.DistanceViewModel.DataPath.Split('\\')[^1].IndexOf('.'));
                        parameters.DataPath = dataset;
                        var results = new TSPResult[runs];
                        
                        foreach (var crossoverMethod in crossovers)
                        {
                            parameters.CrossoverMethod = crossoverMethod;
                            
                            var fileName = seed+"/"+runs + "_" + dataset.Split('\\')[^1]
                                               .Remove(_properties.DistanceViewModel.DataPath.Split('\\')[^1].IndexOf('.'))+ "_"
                                           + Enum.GetName(crossoverMethod)+".csv";
                            var s = "conflict_resolver;random_resolver;epoch;best_distance;avg_best_10%;median;avg_worst_10%;avg;worst_distance;std_deviation;resolver_percentage\n";

                            
                            foreach (ConflictResolveMethod randomizedResolve in Enum.GetValues(typeof(ConflictResolveMethod)))
                            {
                                parameters.RandomizedResolveMethod = randomizedResolve;

                                foreach (ConflictResolveMethod conflictResolve in Enum.GetValues(typeof(ConflictResolveMethod)))
                                {
                                    parameters.ConflictResolveMethod = conflictResolve;
                                    Parallel.For(0, runs, i =>
                                    {
                                        results[i] = OptimizationWork.TSP(parameters, ct, seed+i);
                                    });
                    
                                    s += CreateDistanceLogsPerRunsParams(results, Enum.GetName(parameters.ConflictResolveMethod),
                                        Enum.GetName(parameters.RandomizedResolveMethod));
                                    SaveDistanceArticleResultsToFile($"{seed}/{datasetName}_data.csv", results, 
                                        Enum.GetName(parameters.ConflictResolveMethod),
                                        Enum.GetName(parameters.RandomizedResolveMethod),seed);
                                }
                        
                            }
                            File.AppendAllText(fileName, s);

                        }

                        
                    }
                }, ct);

                Dispatcher.Invoke(() =>
                {
                    _properties.DistanceViewModel.ProgressBarValue =
                        _properties.DistanceViewModel.ProgressBarMaximum;
                    Optimization.GeneticAlgorithms.BaseGenetic.OnNextIteration -= BaseGeneticOnOnNextIteration();
                });
            }
            catch (AggregateException)
            {
                DistanceResultLabel.Content = "Cancelled";
            }
            
        }

        private void ReadDistanceDataSetPathButton_OnClick(object sender, RoutedEventArgs e)
        {
            var fileDialog = new FolderBrowserForWPF.Dialog();
            if(fileDialog.ShowDialog() == true)
            {
                _properties.DistanceViewModel.DatasetDirectoryPath = fileDialog.FileName;
                var files = Directory.GetFiles(fileDialog.FileName,"*.tsp");
                _properties.DistanceViewModel.DataPath = files[0];
            }
            
        }
    }
    
}