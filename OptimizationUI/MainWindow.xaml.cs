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
using Optimization.GeneticAppliances.Warehouse;
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
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
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

        private async void OtpimizeWithCurrentParametersButtonClick(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(_properties.DistanceViewModel.ResultPath))
                Directory.CreateDirectory(_properties.DistanceViewModel.ResultPath);
            DistanceStartButton.IsEnabled = false;
            EventHandler<int> BaseGeneticOnOnNextIteration()
            {
                return (sender, iteration) =>
                {
                    _properties.DistanceViewModel.ProgressBarValue++;
                };
            }
            _properties.DistanceViewModel.DataPath = _properties.DistanceViewModel.SelectedFiles[0];

            OptimizationParameters parameters = _properties.DistanceViewModel as OptimizationParameters;
            int runs = Int32.Parse(DistanceRunsTextBox.Text);
            int seed = _properties.DistanceViewModel.CurrentSeed;
            if (_properties.DistanceViewModel.RandomSeed)
            {
                Random random = new Random();
                seed = random.Next(1, Int32.MaxValue);
                _properties.DistanceViewModel.CurrentSeed = seed;
            }
            TSPResult[] results = new TSPResult[runs];
            double[][][] runFitnesses = new double[runs][][];

            _cancellationTokenSource = new CancellationTokenSource();
            _properties.DistanceViewModel.ProgressBarMaximum = runs * _properties.DistanceViewModel.MaxEpoch - 1;
            _properties.DistanceViewModel.ProgressBarValue = 0;
            Optimization.GeneticAlgorithms.BaseGenetic.OnNextIteration += BaseGeneticOnOnNextIteration();
            CancellationToken ct = _cancellationTokenSource.Token;
            if ((OptimizationMethod)DistanceMethodComboBox.SelectedItem == OptimizationMethod.GeneticAlgorithm)
            {
                Directory.CreateDirectory(_properties.DistanceViewModel.ResultPath + "\\" + seed.ToString());
                SerializeParameters(_properties.DistanceViewModel.ResultPath + "\\" + seed + "/parameters.json");

                await Task.Run(() =>
                    {

                        foreach (var dataset in _properties.DistanceViewModel.SelectedFiles)
                        {
                            var s = "epoch;best_distance;avg_best_10%;median;avg_worst_10%;avg;worst_distance;std_deviation;conflict_percentage;avgDiff;0Diff;02Diff\n";
                            _properties.DistanceViewModel.DataPath = dataset;
                            var fileName = seed + "/" + runs + "_BEST_" + _properties.DistanceViewModel.DataPath.Split("\\")[^1] + ".txt";
                            var datasetName = dataset.Split('\\')[^1]
                                .Remove(_properties.DistanceViewModel.DataPath.Split('\\')[^1].IndexOf('.'));
                            parameters.DataPath = dataset;
                            results = new TSPResult[runs];

                            Parallel.For(0, runs, i =>
                            {
                                results[i] = OptimizationWork.TSP(parameters, ct, seed + i);
                                runFitnesses[i] = results[i].fitness;
                            });

                            s += CreateDistanceLogsBestPerRunsParams(results,
                                Enum.GetName(parameters.ConflictResolveMethod),
                                Enum.GetName(parameters.RandomizedResolveMethod));
                            SaveDistanceArticleResultsToFile($"{seed}/data.txt", results,
                                Enum.GetName(parameters.ConflictResolveMethod),
                                Enum.GetName(parameters.RandomizedResolveMethod), seed);
                            File.AppendAllText(_properties.DistanceViewModel.ResultPath + "\\" + fileName, s);

                            fileName = seed + "/" + runs + "_AVG_" + _properties.DistanceViewModel.DataPath.Split("\\")[^1] + ".txt";

                            s = "epoch;best_distance;avg_best_10%;median;avg_worst_10%;avg;worst_distance;std_deviation;conflict_percentage;avgDiff;0Diff;02Diff\n";
                            s += CreateDistanceLogsPerRunsParams(results,
                                Enum.GetName(parameters.ConflictResolveMethod),
                                Enum.GetName(parameters.RandomizedResolveMethod));
                            File.AppendAllText(_properties.DistanceViewModel.ResultPath + "\\" + fileName, s);
                        }

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
                        WritePlotDistances(linesGridDistances, GetBestFitnesses(runFitnesses));

                        SaveDistanceResultsToDataCsv(results, runs,
                            _properties.DistanceViewModel.DataPath.Split('\\')[^1]
                                .Remove(_properties.DistanceViewModel.DataPath.Split('\\')[^1].IndexOf('.'))
                            );
                    });


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
            WarehouseResult result = null;
            double[][] fitness = null;
            Random rnd = new Random();


            await Task.Run(() =>
            {
                WarehouseParameters warehouseParameters = _properties.WarehouseViewModel as WarehouseParameters;
                warehouseParameters.WarehouseGeneticAlgorithmParameters.ConflictResolveMethod =
                    ConflictResolveMethod.WarehouseSingleProductFrequency;
                warehouseParameters.WarehouseGeneticAlgorithmParameters.RandomizedResolveMethod =
                    ConflictResolveMethod.WarehouseSingleProductFrequency;
                warehouseParameters.WarehouseGeneticAlgorithmParameters.ResolveRandomizationProbability = 1;
                warehouseParameters.FitnessGeneticAlgorithmParameters.Use2opt = true;
                result = Optimization.OptimizationWork.WarehouseOptimization(warehouseParameters, ct, rnd.Next(1, Int32.MaxValue));
            }, ct);

            Dispatcher.Invoke(() =>
            {
                WarehouseResultLabel.Content = $"Wynik: {result.FinalFitness}";
                WritePlotWarehouse(linesGridWarehouse, result.fitness);
                SaveWarehouseResultToFile(result);
            });

        }

        private void SaveWarehouseResultToFile(WarehouseResult result)
        {
            if (!Directory.Exists("../../../../WarehouseResults"))
                Directory.CreateDirectory("../../../../WarehouseResults");
            var filePath = $"../../../../WarehouseResults/{result.Seed}.txt";
            string s = "";

            s += $"BEST: ";
            foreach (var gene in result.BestChromosome)
            {
                s += $"{gene} ";
            }

            s += "\n";

            s += $"FITNESS: {result.FinalFitness}\n\n";
            for (int i = 0; i < result.FinalOrderPaths.Length; i++)
            {
                s += $"Order nr.{i}: ";
                foreach (var gene in result.FinalOrderPaths[i])
                {
                    s += $"{gene} ";
                }

                s += "\n";
            }
            
            
            File.WriteAllText(filePath,s);
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
            Assembly asm = Assembly.GetExecutingAssembly();
            string path = System.IO.Path.GetDirectoryName(asm.Location);
            StringBuilder sb = new StringBuilder();
            var pathParts = path.Split("\\");
            if (pathParts.Length > 4)
                for (int i = 0; i < pathParts.Length - 4; i++)
                {
                    sb.Append(pathParts[i] + "\\");
                }

            sb.Append("Data");

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "All files (*.*)|*.*|txt files (*.txt)|*.txt|tsp files (*.tsp)|*.tsp";
            fileDialog.RestoreDirectory = true;
            fileDialog.Multiselect = true;
            fileDialog.InitialDirectory = sb.ToString();
            fileDialog.ShowDialog();
            _properties.DistanceViewModel.SelectedFiles = fileDialog.FileNames;
            _properties.DistanceViewModel.SelectedFilesString = GetFilesString();

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

        private double[][] GetBestFitnesses(double[][][] runFitnesses)
        {

            int runs = runFitnesses.Length;
            int epoch = runFitnesses[0].Length;

            var expandedFitness = GetExpandedFitesses(runFitnesses);

            double[][] fitness = new double[runs][];

            for (int i = 0; i < runs; i++)
            {
                fitness[i] = new double[runFitnesses[0].Length];
            }

            for (int i = 0; i < runs; i++)
            {
                for (int j = 0; j < epoch; j++)
                {
                    fitness[i][j] = expandedFitness[i][j].Min();
                }
            }


            return fitness;
        }

        private double[][][] GetExpandedFitesses(double[][][] runFitnesses)
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

            return expandedFitness;
        }

        private double[][] GetAverageFitnesses(double[][][] runFitnesses)
        {

            int epoch = runFitnesses[0].Length;

            var expandedFitness = GetExpandedFitesses(runFitnesses);

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
            string[] lines = File.ReadAllLines("fitness.txt");
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
            // int epoch = fitness.Length;
            //
            // var x = Enumerable.Range(0, epoch).ToArray();
            // double[] y = new double[epoch];
            // double[] z = new double[epoch];
            // double[] a = new double[epoch];
            // double[] m = new double[epoch];
            // for (int i = 0; i < epoch; i++)
            // {
            //     y[i] = fitness[i].Min();
            //     a[i] = fitness[i].Max();
            //     z[i] = fitness[i].Average();
            //     m[i] = fitness[i].OrderBy(j => j).Skip((int)(Convert.ToInt32(DistancesPercentText.Text) * 0.01 * fitness[i].Length)).Take(Convert.ToInt32(DistancesIndividualsText.Text)).Average();
            // }
            int epoch = fitness[0].Length;

            var x = Enumerable.Range(0, epoch).ToArray();
            double[] y = new double[epoch];
            double[] z = new double[epoch];
            double[] a = new double[epoch];
            double[] m = new double[epoch];
            for (int i = 0; i < epoch; i++)
            {
                y[i] = fitness.Min(d => d[i]);
                a[i] = fitness.Max(d => d[i]);
                z[i] = fitness.Average(d => d[i]);
                m[i] = fitness.OrderBy(d => d[i]).Skip((int)(Convert.ToInt32(DistancesPercentText.Text) * 0.01 * fitness.Length)).Take(Convert.ToInt32(DistancesIndividualsText.Text)).ToArray().Average(d => d[i]);
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
            lineAvg.Plot(x, z);
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
            legendItemsPanel.MasterPlot = lineAvg;
            distancesChart.UpdateLayout();

        }

        private void RenderChartDistances(object sender, RoutedEventArgs e)
        {

            RenderTargetBitmap bmp = new RenderTargetBitmap(3000, 2450, 720, 660, PixelFormats.Pbgra32);
            bmp.Render(distancesChart);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            string name = "distances_chart_" + DateTime.Now.Day + "-" + DateTime.Now.Month + "_" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + ".png";

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
            lineAvg.Plot(x, z);
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

        private void SaveDistanceResultsToDataCsv(TSPResult[] results, int runs, string dataset, string id = "default")
        {
            var line = File.Exists(_properties.DistanceViewModel.ResultPath + "\\data.txt") ? new StringBuilder() : new StringBuilder("algorithm;dataset;id;runs;distance;d_epoch;d*0.98_epoch;seed\n");

            line.Append(Enum.GetName(_properties.DistanceViewModel.CrossoverMethod));
            if (_properties.DistanceViewModel.CrossoverMethod == CrossoverMethod.MAC || _properties.DistanceViewModel.CrossoverMethod == CrossoverMethod.MRC)
            {
                line.Append('(');
                foreach (var crossoverMethod in _properties.DistanceViewModel.MultiCrossovers)
                {
                    line.Append($"{Enum.GetName(crossoverMethod)} ");
                }
                line.Remove(line.Length - 1, 1);
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
            File.AppendAllText(_properties.DistanceViewModel.ResultPath + "\\data.txt", line.ToString());
        }

        int xt = 0;
        List<string>[] Smin = new List<string>[11];
        List<string>[] Savg = new List<string>[11];

        private void SaveDistanceArticleResultsToFile(string path, TSPResult[] results, string conflictResolver, string randomResolver, int seed, int pri = 0)
        {


            var headers =
            "dataset;crossover;conflict_resolver;random_resolver;best_distance;avg_top_10%;median;avg_worst_10%;average;worst_distance;std_dev;avg_min_epoch;d*0.98_epoch\n";
            var dataset = _properties.DistanceViewModel.DataPath.Split('\\')[^1]
                .Remove(_properties.DistanceViewModel.DataPath.Split('\\')[^1].IndexOf('.'));

            string ft = _properties.DistanceViewModel.ResultPath + @"\data_" +
                System.IO.Path.GetFileNameWithoutExtension(_properties.DistanceViewModel.DataPath) + "_" + _properties.DistanceViewModel.ResolveRandomizationProbability +
                 "_" + _properties.DistanceViewModel.MaxEpoch + ".txt";


            var s = "";
            if (xt == 0)
                s = headers;

            xt++;

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
                    if (!(bestsPerEpochs[j] < currentMin * 0.98))
                        continue;
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
            s += results.Min(x => x.FinalFitness).ToString("#.000") + ";";  //najlepszy wynik
            Smin[pri].Add(results.Min(x => x.FinalFitness).ToString("#.000"));

            s += results.OrderBy(x => x.FinalFitness).Take((int)(0.1 * results.Length)).Average(x => x.FinalFitness).ToString("#.000") + ";"; //średnia z najlepszych 10%
            s += results.OrderBy(x => x.FinalFitness).Skip((int)(0.5 * results.Length)).Take(1).Average(x => x.FinalFitness).ToString("#.000") + ";";  //mediana
            s += results.OrderBy(x => x.FinalFitness).Skip((int)(0.9 * results.Length)).Take((int)(0.1 * results.Length)).Average(x => x.FinalFitness).ToString("#.000") + ";"; //średnia z najgorszych 10%
            s += results.Average(x => x.FinalFitness).ToString("#.000") + ";"; //średnia
            Savg[pri].Add(results.Average(x => x.FinalFitness).ToString("#.000"));

            s += results.Max(x => x.FinalFitness).ToString("#.000") + ";"; // najgorszy wynik
            s += results.StandardDeviation(x => x.FinalFitness).ToString("#.000") + ";"; // odchylenie standardowe
            s += averageMinEpoch.ToString("#.0") + ";";
            s += epochNumbersWhenSlowedDown.Average().ToString("#.000") + ";";
            s += "\r\n";

            File.AppendAllText(ft, s);
        }

        private string CreateDistanceLogsPerRunsParams(TSPResult[] results, string conflictResolver, string randomResolver)
        {
            var fitness = GetAverageFitnesses(results.Select(x => x.fitness).ToArray());
            var differences = GetAverageFitnesses(results.Select(x => x.DifferencesInEpoch).ToArray());
            var z = results.Select(x => x.ResolvePercentInEpoch).ToArray();

            var maxLenght = z.Max(x => x.Length);
            var transponsedZ = new List<List<double>>();
            for (int i = 0; i < maxLenght; i++)
            {
                transponsedZ.Add(new List<double>());
                for (int j = 0; j < z.Length; j++)
                {
                    if (z[j].Length > i) transponsedZ[i].Add(z[j][i]);
                }
            }

            var epochsPerc = transponsedZ.Select(x => x.Average()).ToArray();

            string s = "";

            for (int i = 0; i < fitness.Length; i++)
            {
                var epochFitnesses = fitness[i];
                var difference = differences[i];
                s += i + ";";
                //tego nie jestem do końca pewien, które wartości będą potrzebne - może lepiej ich naprodukować więcej, by mieć z czego wybierać:
                s += epochFitnesses.Min().ToString("#.000") + ";";  //najlepszy wynik
                s += epochFitnesses.OrderBy(x => x).Take((int)(0.1 * epochFitnesses.Length)).Average().ToString("#.000") + ";"; //średnia z najlepszych 10%
                s += epochFitnesses.OrderBy(x => x).Skip((int)(0.5 * epochFitnesses.Length)).Take(1).Average().ToString("#.000") + ";";  //mediana
                s += epochFitnesses.OrderBy(x => x).Skip((int)(0.9 * epochFitnesses.Length)).Take((int)(0.1 * epochFitnesses.Length)).Average().ToString("#.000") + ";"; //średnia z najgorszych 10%
                s += epochFitnesses.Average().ToString("#.000") + ";"; //średnia
                s += epochFitnesses.Max().ToString("#.000") + ";"; // najgorszy wynik
                s += epochFitnesses.StandardDeviation().ToString("#.000") + ";"; // odchylenie standardowe
                s += epochsPerc[i].ToString("#.000") + ";";
                s += difference.Average().ToString("#.000") + ";";
                s += difference.Count(x => x == 0).ToString("#.000") + ";"; //identyczne
                s += difference.Count(x => x < Math.Max(2.1, 0.021 * results[0].BestGene.Length)).ToString("#.000"); //2% różnicy
                s += "\r\n";

            }

            return s;
        }
        private string CreateDistanceLogsBestPerRunsParams(TSPResult[] results, string conflictResolver, string randomResolver)
        {
            var fitness = GetBestFitnesses(results.Select(x => x.fitness).ToArray());
            var differences = GetAverageFitnesses(results.Select(x => x.DifferencesInEpoch).ToArray());
            var z = results.Select(x => x.ResolvePercentInEpoch).ToArray();

            var maxLenght = z.Max(x => x.Length);
            var transponsedZ = new List<List<double>>();
            for (int i = 0; i < maxLenght; i++)
            {
                transponsedZ.Add(new List<double>());
                for (int j = 0; j < z.Length; j++)
                {
                    if (z[j].Length > i) transponsedZ[i].Add(z[j][i]);
                }
            }

            var epochsPerc = transponsedZ.Select(x => x.Average()).ToArray();

            string s = "";

            for (int i = 0; i < fitness[0].Length; i++)
            {
                var difference = differences[i];
                s += i + ";";
                //tego nie jestem do końca pewien, które wartości będą potrzebne - może lepiej ich naprodukować więcej, by mieć z czego wybierać:
                s += fitness.Min(x => x[i]).ToString("#.000") + ";";  //najlepszy wynik
                s += fitness.OrderBy(x => x[i]).Take((int)(0.1 * fitness.Length)).Average(x => x[i]).ToString("#.000") + ";"; //średnia z najlepszych 10%
                s += fitness.OrderBy(x => x[i]).Skip((int)(0.5 * fitness.Length)).Take(1).Average(x => x[i]).ToString("#.000") + ";";  //mediana
                s += fitness.OrderBy(x => x[i]).Skip((int)(0.9 * fitness.Length)).Take((int)(0.1 * fitness.Length)).Average(x => x[i]).ToString("#.000") + ";"; //średnia z najgorszych 10%
                s += fitness.Average(x => x[i]).ToString("#.000") + ";"; //średnia
                s += fitness.Max(x => x[i]).ToString("#.000") + ";"; // najgorszy wynik
                s += fitness.StandardDeviation(x => x[i]).ToString("#.000") + ";"; // odchylenie standardowe
                s += epochsPerc[i].ToString("#.000") + ";";
                s += difference.Average().ToString("#.000") + ";";
                s += difference.Count(x => x == 0).ToString("#.000") + ";"; //identyczne
                s += difference.Count(x => x < Math.Max(2.1, 0.021 * results[0].BestGene.Length)).ToString("#.000"); //2% różnicy
                s += "\n";

            }

            return s;
        }

        private async void LoopAllParametersButton_OnClick(object sender, RoutedEventArgs e)
        {
            Cursor cursor = this.Cursor;
            this.Cursor = Cursors.Wait;

            if (!Directory.Exists(_properties.DistanceViewModel.ResultPath))
                Directory.CreateDirectory(_properties.DistanceViewModel.ResultPath);

            DistanceStartButton.IsEnabled = false;

            EventHandler<int> BaseGeneticOnOnNextIteration()
            {
                return (_, iteration) =>
                {
                    _properties.DistanceViewModel.ProgressBarValue++;
                    //Console.WriteLine(iteration);
                };
            }


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


            await Task.Run(() =>
            {
                Directory.CreateDirectory(_properties.DistanceViewModel.ResultPath + "\\" + seed.ToString());
                SerializeParameters(_properties.DistanceViewModel.ResultPath + "\\" + seed + "/parameters.json");

                _properties.DistanceViewModel.ProgressBarMaximum = (runs * _properties.DistanceViewModel.MaxEpoch * 9 * crossovers.Length * _properties.DistanceViewModel.SelectedFiles.Length) - 1;
                _properties.DistanceViewModel.ProgressBarValue = 0;
                Optimization.GeneticAlgorithms.BaseGenetic.OnNextIteration += BaseGeneticOnOnNextIteration();
                List<string> operationType = new List<string>();
                int ren = -1;

                foreach (var dataset in _properties.DistanceViewModel.SelectedFiles)
                {
                    _properties.DistanceViewModel.DataPath = dataset;
                    var datasetName = dataset.Split('\\')[^1]
                            .Remove(_properties.DistanceViewModel.DataPath.Split('\\')[^1].IndexOf('.'));
                    parameters.DataPath = dataset;
                    var results = new TSPResult[runs];

                    Smin = new List<string>[11];
                    Savg = new List<string>[11];

                    double[] pr = { 0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0 };

                    for (int pri = 0; pri < pr.Length; pri++)
                    {
                        Smin[pri] = new List<string>();
                        Savg[pri] = new List<string>();

                        xt = 0;
                        parameters.ResolveRandomizationProbability = pr[pri];
                        _properties.DistanceViewModel.ResolveRandomizationProbability = pr[pri];

                        ren++;
                        foreach (var crossoverMethod in crossovers)
                        {
                            parameters.CrossoverMethod = crossoverMethod;

                            foreach (ConflictResolveMethod randomizedResolve in Enum.GetValues(typeof(ConflictResolveMethod)))
                            {
                                parameters.RandomizedResolveMethod = randomizedResolve;

                                foreach (ConflictResolveMethod conflictResolve in Enum.GetValues(typeof(ConflictResolveMethod)))
                                {
                                    var fileName = seed + "/" + runs + "_" + dataset.Split('\\')[^1]
                                                       .Remove(_properties.DistanceViewModel.DataPath.Split('\\')[^1].IndexOf('.')) + "_BEST_"
                                                   + Enum.GetName(crossoverMethod) + "_" + Enum.GetName(randomizedResolve) + "_" + Enum.GetName(conflictResolve) + ".txt";
                                    var s = "epoch;best_distance;avg_best_10%;median;avg_worst_10%;avg;worst_distance;std_deviation;conflict_percentage;avgDiff;0Diff;02Diff\n";

                                    parameters.ConflictResolveMethod = conflictResolve;
                                    Parallel.For(0, runs, i =>
                                    {
                                        results[i] = OptimizationWork.TSP(parameters, ct, seed + i);
                                    });

                                    s += CreateDistanceLogsBestPerRunsParams(results, Enum.GetName(parameters.ConflictResolveMethod),
                                        Enum.GetName(parameters.RandomizedResolveMethod));

                                    string n0 = $"{seed}/{datasetName}_data.txt";
                                    string n1 = Enum.GetName(parameters.ConflictResolveMethod);
                                    string n2 = Enum.GetName(parameters.RandomizedResolveMethod);
                                    string n3 = Enum.GetName(parameters.CrossoverMethod);

                                    if (ren < 1)
                                        operationType.Add(n3 + ";" + n1 + ";" + n2 + ";");

                                    SaveDistanceArticleResultsToFile(n0, results, n1, n2, seed, pri);

                                    File.AppendAllText(_properties.DistanceViewModel.ResultPath + "\\" + fileName, s);

                                    fileName = seed + "/" + runs + "_" + dataset.Split('\\')[^1]
                                                   .Remove(_properties.DistanceViewModel.DataPath.Split('\\')[^1].IndexOf('.')) + "_AVG_"
                                               + Enum.GetName(crossoverMethod) + "_" + Enum.GetName(randomizedResolve) + "_" + Enum.GetName(conflictResolve) + ".txt";


                                    s += CreateDistanceLogsPerRunsParams(results,
                                        Enum.GetName(parameters.ConflictResolveMethod),
                                        Enum.GetName(parameters.RandomizedResolveMethod));
                                    File.AppendAllText(_properties.DistanceViewModel.ResultPath + "\\" + fileName, s);

                                }

                            }

                        }
                    }

                    string fresMin = "crossover;conflict;random;m00;m01;m02;m03;m04;m05;m06;m07;m08;m09;m10\r\n";
                    string fresAvg = "crossover;conflict;random;a00;a01;a02;a03;a04;a05;a06;a07;a08;a09;a10\r\n";

                    for (int j = 0; j < Smin[0].Count; j++)
                    {

                        fresMin += operationType[j];
                        fresAvg += operationType[j];

                        for (int i = 0; i < 11; i++)
                        {
                            fresMin += Smin[i][j] + ";";
                            fresAvg += Savg[i][j] + ";";
                        }
                        fresMin += "\r\n";
                        fresAvg += "\r\n";
                    }

                    string f1 = _properties.DistanceViewModel.ResultPath + @"\" + System.IO.Path.GetFileNameWithoutExtension(dataset) + "_" + _properties.DistanceViewModel.MaxEpoch;

                    File.WriteAllText(f1 + "_fresMin.txt", fresMin);
                    File.WriteAllText(f1 + "_fresAvg.txt", fresAvg);


                }
            }, ct);


            _properties.DistanceViewModel.ProgressBarValue = 0;
            this.Cursor = cursor;

        }

        private void ReadDistanceResultPathButton_OnClick(object sender, RoutedEventArgs e)
        {
            var fileDialog = new FolderBrowserForWPF.Dialog();
            if (fileDialog.ShowDialog() == true)
            {
                _properties.DistanceViewModel.ResultPath = fileDialog.FileName;

            }

        }
        private void Button_Click_CompareMethods(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() != true)
                return;

            double[,][] fitnesses = new double[2, openFileDialog.FileNames.Length][];
            string[] resolvers = new string[openFileDialog.FileNames.Length];

            for (int i = 0; i < openFileDialog.FileNames.Length; i++)
            {
                string[] allLines = File.ReadAllLines(openFileDialog.FileNames[i]);
                resolvers[i] = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileNames[i].Substring(openFileDialog.FileNames[i].IndexOf("\\")));

                fitnesses[0, i] = new double[allLines.Length - 1];
                fitnesses[1, i] = new double[allLines.Length - 1];

                for (int j = 1; j < allLines.Length; j++)
                {
                    string[] thisLine = allLines[j].Split(';');
                    fitnesses[0, i][j - 1] = Convert.ToDouble(thisLine[3]);
                    fitnesses[1, i][j - 1] = Convert.ToDouble(thisLine[4]);
                }
            }


            WritePlotDistancesCompare(linesGridDistances, fitnesses, resolvers);
        }


        private void WritePlotDistancesCompare(Grid linesGrid, double[,][] fitness, string[] resolvers)
        {
            linesGrid.Children.Clear();
            var x = Enumerable.Range(0, fitness[0, 0].Length).ToArray();

            LineGraph[,] lineGragh = new LineGraph[2, fitness.GetLength(1)];

            Color[] colors = new Color[9];
            colors[0] = Colors.Black;
            colors[1] = Colors.Red;
            colors[2] = Colors.LightBlue;
            colors[3] = Colors.Green;
            colors[4] = Colors.Navy;
            colors[5] = Colors.Orange;
            colors[6] = Colors.Gray;
            colors[7] = Colors.LightGreen;
            colors[8] = Colors.Brown;


            int ii = 0;
            for (int i = 0; i < fitness.GetLength(1); i++)
            {

                lineGragh[0, i] = new LineGraph() { Stroke = new SolidColorBrush(colors[ii]), Description = resolvers[i] + "_min", StrokeThickness = 1 };
                lineGragh[1, i] = new LineGraph() { Stroke = new SolidColorBrush(colors[ii]), Description = resolvers[i] + "_avg", StrokeThickness = 1 };

                if (++ii == 8)
                    ii = 0;

                lineGragh[0, i].Plot(x, fitness[0, i]);
                lineGragh[1, i].Plot(x, fitness[1, i]);

                linesGrid.Children.Add(lineGragh[0, i]);
                linesGrid.Children.Add(lineGragh[1, i]);
            }

            distancesChart.Content = linesGrid;
            legendItemsPanel.MasterPlot = lineGragh[0, 0];
            distancesChart.UpdateLayout();
        }

        private string GetFilesString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var file in _properties.DistanceViewModel.SelectedFiles)
            {
                sb.Append($"{file.Split("\\")[^1]}, ");
            }

            return sb.ToString();
        }

        private void FresMinAvgButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() != true)
                return;

            var minimalDistances = new List<double>();
            var convertedFiles = new List<double[][]>();
            string header = null;
            string[] rowHeaders = new string[] { };
            foreach (var fileName in openFileDialog.FileNames)
            {
                var min = Double.MaxValue;
                var allLines = File.ReadLines(fileName).ToArray();

                var convertedFile = new double[allLines.Length-1][];
                header = allLines[0];
                rowHeaders = new string[allLines.Length];
                for (var i = 1; i < allLines.Length; i++)
                {
                    var line = allLines[i].Split(';');
                    var result = line.ToList();

                    rowHeaders[i - 1] = result[0] + ";"+ result[1] +";"+ result[2]+";";
                    
                    result.RemoveAt(line.Length - 1);
                    result.RemoveRange(0,3);

                    convertedFile[i - 1] = new double[result.Count];
                    
                    for (var j = 0; j < result.Count; j++)
                    {
                        var value = Convert.ToDouble(result[j]);
                        convertedFile[i - 1][j] = value;
                        if (value < min)
                            min = value;
                    }
                }
                
                minimalDistances.Add(min);
                convertedFiles.Add(convertedFile);

                


            }
            
            for (var i = 0; i < convertedFiles.Count; i++)
            {
                for (var j = 0; j < convertedFiles[i].Length; j++)
                {
                    for (var k = 0; k < convertedFiles[i][j].Length; k++)
                    {
                        convertedFiles[i][j][k] /= minimalDistances[i];
                    }
                }
            }

            var finalValues = GetAverageFitnesses(convertedFiles.ToArray());
            var lines = new string[finalValues.Length+1];
            lines[0] = header;
            for (var i = 0; i < finalValues.Length; i++)
            {
                var s = rowHeaders[i];
                    
                for (var j = 0; j < finalValues[i].Length; j++)
                {
                    s += finalValues[i][j] + ";";
                }

                lines[i + 1] = s;
            }
                
            File.AppendAllLines("fresMinAvg"+DateTime.Now.Ticks+".txt",lines);
        }
    }

}