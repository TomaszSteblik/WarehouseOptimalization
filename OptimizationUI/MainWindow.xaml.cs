using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using Optimization.GeneticAlgorithms.Crossovers;
using Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers;
using Optimization.GeneticAppliances.TSP;
using Optimization.GeneticAppliances.Warehouse;
using Optimization.Parameters;
using OptimizationUI.ViewModels;

namespace OptimizationUI
{
    public partial class MainWindow : Window
    {
        private CancellationTokenSource _cancellationTokenSource;

        public MainWindow()
        {
            InitializeComponent();
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            DataContext = new MainViewModel();
            
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
                WarehouseParameters warehouseParameters = _properties.Warehouse as WarehouseParameters;
                warehouseParameters.WarehouseGeneticAlgorithmParameters.ConflictResolveMethod =
                    ConflictResolveMethod.WarehouseSingleProductFrequency;
                warehouseParameters.WarehouseGeneticAlgorithmParameters.RandomizedResolveMethod =
                    ConflictResolveMethod.WarehouseSingleProductFrequency;
                warehouseParameters.WarehouseGeneticAlgorithmParameters.ResolveRandomizationProbability = 0.6;
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
        
        private void WarehouseMagPathButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "txt files (*.txt)|*.txt";
            fileDialog.RestoreDirectory = true;
            fileDialog.ShowDialog();
            _properties.Warehouse.WarehousePath = fileDialog.FileName;
        }

        private void WarehouseOrdersPathButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "txt files (*.txt)|*.txt";
            fileDialog.RestoreDirectory = true;
            fileDialog.ShowDialog();
            _properties.Warehouse.OrdersPath = fileDialog.FileName;
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
        
        private void SaveDistanceResultsToDataCsv(TSPResult[] results, int runs, string dataset, string id = "default")
        {
            var line = File.Exists(_properties.Distance.ResultPath + "\\data.txt") ? new StringBuilder() : new StringBuilder("algorithm;dataset;id;runs;distance;d_epoch;d*0.98_epoch;seed\n");

            line.Append(Enum.GetName(_properties.Distance.CrossoverMethod));
            if (_properties.Distance.CrossoverMethod == CrossoverMethod.MAC || _properties.Distance.CrossoverMethod == CrossoverMethod.MRC)
            {
                line.Append('(');
                foreach (var crossoverMethod in _properties.Distance.MultiCrossovers)
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
            File.AppendAllText(_properties.Distance.ResultPath + "\\data.txt", line.ToString());
        }
        
        private void SaveDistanceArticleResultsToFile(string path, TSPResult[] results, string conflictResolver, string randomResolver, int seed,int xt, int pri = 0)
        {


            var headers =
            "dataset;crossover;conflict_resolver;random_resolver;best_distance;avg_top_10%;median;avg_worst_10%;average;worst_distance;std_dev;avg_min_epoch;d*0.98_epoch\n";
            var dataset = _properties.Distance.DataPath.Split('\\')[^1]
                .Remove(_properties.Distance.DataPath.Split('\\')[^1].IndexOf('.'));

            string ft = _properties.Distance.ResultPath + @"\data_" +
                System.IO.Path.GetFileNameWithoutExtension(_properties.Distance.DataPath) + "_" + _properties.Distance.ResolveRandomizationProbability +
                 "_" + _properties.Distance.MaxEpoch + ".txt";


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
            s += Enum.GetName(_properties.Distance.CrossoverMethod) + ";";
            s += conflictResolver + ";";
            s += randomResolver + ";";
            //tego nie jestem do końca pewien, które wartości będą potrzebne - może lepiej ich naprodukować więcej, by mieć z czego wybierać:
            s += results.Min(x => x.FinalFitness).ToString("#.000") + ";";  //najlepszy wynik
            //TODO: Zainicjowac liste Smin, narazie komentujemy ten fragment kodu
            //Smin[pri].Add(results.Min(x => x.FinalFitness).ToString("#.000"));

            s += results.OrderBy(x => x.FinalFitness).Take((int)(0.1 * results.Length)).Average(x => x.FinalFitness).ToString("#.000") + ";"; //średnia z najlepszych 10%
            s += results.OrderBy(x => x.FinalFitness).Skip((int)(0.5 * results.Length)).Take(1).Average(x => x.FinalFitness).ToString("#.000") + ";";  //mediana
            s += results.OrderBy(x => x.FinalFitness).Skip((int)(0.9 * results.Length)).Take((int)(0.1 * results.Length)).Average(x => x.FinalFitness).ToString("#.000") + ";"; //średnia z najgorszych 10%
            s += results.Average(x => x.FinalFitness).ToString("#.000") + ";"; //średnia
            //TODO: Zainicjowac liste Smin, narazie komentujemy ten fragment kodu
            //Savg[pri].Add(results.Average(x => x.FinalFitness).ToString("#.000"));

            s += results.Max(x => x.FinalFitness).ToString("#.000") + ";"; // najgorszy wynik
            s += results.StandardDeviation(x => x.FinalFitness).ToString("#.000") + ";"; // odchylenie standardowe
            s += averageMinEpoch.ToString("#.0") + ";";
            s += epochNumbersWhenSlowedDown.Average().ToString("#.000") + ";";
            s += "\r\n";

            File.AppendAllText(ft, s);
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

            var finalValues = Helpers.GetAverageFitnesses(convertedFiles.ToArray());
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
