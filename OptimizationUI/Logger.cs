using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Win32;
using Optimization.GeneticAlgorithms.Crossovers;
using Optimization.GeneticAppliances.TSP;
using Optimization.GeneticAppliances.Warehouse;
using OptimizationUI.Models;
using OptimizationUI.ViewModels;
using OxyPlot;
using OxyPlot.Wpf;

namespace OptimizationUI
{
    public static class Logger
    {
        public static string CreateDistanceLogsPerRunsParams(TSPResult[] results, string conflictResolver, string randomResolver)
        {
            var fitness = Helpers.GetAverageFitnesses(results.Select(x => x.fitness).ToArray());
            var differences = Helpers.GetAverageFitnesses(results.Select(x => x.DifferencesInEpoch).ToArray());
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
        public static string CreateDistanceLogsBestPerRunsParams(TSPResult[] results, string conflictResolver, string randomResolver)
        {
            var fitness = Helpers.GetBestFitnesses(results.Select(x => x.fitness).ToArray());
            var differences = Helpers.GetAverageFitnesses(results.Select(x => x.DifferencesInEpoch).ToArray());
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
        
        public static void SaveDistanceResultsToDataCsv(TSPResult[] results, int runs, string dataset, Distance distance,string id = "default")
        {
            var line = File.Exists(distance.ResultPath + "\\data.txt") ? new StringBuilder() : new StringBuilder("algorithm;dataset;id;runs;distance;d_epoch;d*0.98_epoch;seed\n");

            line.Append(Enum.GetName(distance.CrossoverMethod));
            if (distance.CrossoverMethod == CrossoverMethod.MAC || distance.CrossoverMethod == CrossoverMethod.MRC)
            {
                line.Append('(');
                foreach (var crossoverMethod in distance.MultiCrossovers)
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
            File.AppendAllText(distance.ResultPath + "\\data.txt", line.ToString());
        }
        
        private static void SaveDistanceArticleResultsToFile(string path, TSPResult[] results, string conflictResolver, Distance distance ,string randomResolver, int seed,int xt, int pri = 0)
        {


            var headers =
            "dataset;crossover;conflict_resolver;random_resolver;best_distance;avg_top_10%;median;avg_worst_10%;average;worst_distance;std_dev;avg_min_epoch;d*0.98_epoch\n";
            var dataset = distance.DataPath.Split('\\')[^1]
                .Remove(distance.DataPath.Split('\\')[^1].IndexOf('.'));

            string ft =distance.ResultPath + @"\data_" +
                       System.IO.Path.GetFileNameWithoutExtension(distance.DataPath) + "_" + distance.ResolveRandomizationProbability +
                       "_" + distance.MaxEpoch + ".txt";


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
            s += Enum.GetName(distance.CrossoverMethod) + ";";
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
        
        public static void Button_Click_CompareMethods(object sender, RoutedEventArgs e)
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
        
        public static void FresMinAvgButton_OnClick()
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
        
        public static void SaveWarehouseResultToFile(WarehouseResult result, PlotModel plotModel)
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
            
            var pngExporter = new PngExporter { Width = 500, Height = 500, Background = OxyColors.White };
            pngExporter.ExportToFile(plotModel, $"../../../../WarehouseResults/{result.Seed}.png");
            
            
            File.WriteAllText(filePath,s);
        }
    }
}