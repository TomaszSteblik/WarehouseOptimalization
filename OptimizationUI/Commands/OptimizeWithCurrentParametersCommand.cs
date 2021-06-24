using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Optimization;
using Optimization.GeneticAppliances.TSP;
using Optimization.Parameters;
using OptimizationUI.ViewModels;

namespace OptimizationUI.Commands
{
    public class OptimizeWithCurrentParametersCommand : ICommand
    {
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public async void Execute(object? parameter)
        {
            var vm = parameter as DistanceViewModel;
            
            if (!Directory.Exists(vm.DistanceParameters.ResultPath))
                Directory.CreateDirectory(vm.DistanceParameters.ResultPath);
            vm.IsDistanceStartButtonEnabled = false;
            EventHandler<int> BaseGeneticOnOnNextIteration()
            {
                return (sender, iteration) =>
                {
                    vm.ProgressBarValue++;
                };
            }
            vm.DistanceParameters.DataPath = vm.SelectedFiles[0];

            OptimizationParameters parameters = vm.DistanceParameters as OptimizationParameters;
            int runs = vm.Runs;
            int seed = vm.CurrentSeed;
            if (vm.RandomSeed)
            {
                Random random = new Random();
                seed = random.Next(1, Int32.MaxValue);
                vm.CurrentSeed = seed;
            }
            TSPResult[] results = new TSPResult[runs];
            double[][][] runFitnesses = new double[runs][][];

            vm.CancellationTokenSource = new CancellationTokenSource();
            vm.ProgressBarMaximum = runs * vm.DistanceParameters.MaxEpoch - 1;
            vm.ProgressBarValue = 0;
            Optimization.GeneticAlgorithms.BaseGenetic.OnNextIteration += BaseGeneticOnOnNextIteration();
            CancellationToken ct = vm.CancellationTokenSource.Token;
            if (vm.DistanceParameters.OptimizationMethod == OptimizationMethod.GeneticAlgorithm)
            {
                Directory.CreateDirectory(vm.DistanceParameters.ResultPath + "\\" + seed.ToString());
                //SerializeParameters(vm.DistanceParameters.ResultPath + "\\" + seed + "/parameters.json");

                Application.Current.MainWindow.Cursor = Cursors.Wait;
                
                
                await Task.Run(() =>
                    {

                        foreach (var dataset in vm.SelectedFiles)
                        {
                            var s = "epoch;best_distance;avg_best_10%;median;avg_worst_10%;avg;worst_distance;std_deviation;conflict_percentage;avgDiff;0Diff;02Diff\n";
                            vm.DistanceParameters.DataPath = dataset;
                            var fileName = seed + "/" + runs + "_BEST_" + vm.DistanceParameters.DataPath.Split("\\")[^1] + ".txt";
                            var datasetName = dataset.Split('\\')[^1]
                                .Remove(vm.DistanceParameters.DataPath.Split('\\')[^1].IndexOf('.'));
                            parameters.DataPath = dataset;
                            results = new TSPResult[runs];

                            Parallel.For(0, runs, i =>
                            {
                                results[i] = OptimizationWork.TSP(parameters, ct, seed + i);
                                runFitnesses[i] = results[i].fitness;
                            });

                            //s += CreateDistanceLogsBestPerRunsParams(results,
                            //    Enum.GetName(parameters.ConflictResolveMethod),
                            //    Enum.GetName(parameters.RandomizedResolveMethod));
                            //SaveDistanceArticleResultsToFile($"{seed}/data.txt", results,
                            //    Enum.GetName(parameters.ConflictResolveMethod),
                            //    Enum.GetName(parameters.RandomizedResolveMethod), seed);
                            File.AppendAllText(vm.DistanceParameters.ResultPath + "\\" + fileName, s);

                            fileName = seed + "/" + runs + "_AVG_" + vm.DistanceParameters.DataPath.Split("\\")[^1] + ".txt";

                            s = "epoch;best_distance;avg_best_10%;median;avg_worst_10%;avg;worst_distance;std_deviation;conflict_percentage;avgDiff;0Diff;02Diff\n";
                            //s += CreateDistanceLogsPerRunsParams(results,
                            //    Enum.GetName(parameters.ConflictResolveMethod),
                            //    Enum.GetName(parameters.RandomizedResolveMethod));
                            File.AppendAllText(vm.DistanceParameters.ResultPath + "\\" + fileName, s);
                        }

                    }, ct);

                vm.ProgressBarValue =
                    runs * vm.DistanceParameters.MaxEpoch - 1;
                vm.Result =
                    $"Avg: {results.Average(x => x.FinalFitness)}  " +
                    $"Max: {results.Max(x => x.FinalFitness)}  " +
                    $"Min: {results.Min(x => x.FinalFitness)}  " +
                    $"Avg epoch count: {results.Average(x => x.EpochCount)}";
                vm.CurrentSeed = results[0].Seed;

                //SaveDistanceResultsToDataCsv(results, runs,
                 //   vm.DistanceParameters.DataPath.Split('\\')[^1]
                  //      .Remove(vm.DistanceParameters.DataPath.Split('\\')[^1].IndexOf('.'))
                //);


                Optimization.GeneticAlgorithms.BaseGenetic.OnNextIteration -= BaseGeneticOnOnNextIteration();
            }
            else
            {
                double result = OptimizationWork.FindShortestPath(parameters);
                vm.Result =
                    $"Result: {result}";
            }
            vm.IsDistanceStartButtonEnabled = true;
            Application.Current.MainWindow.Cursor = Cursors.Arrow;

        }

        public event EventHandler? CanExecuteChanged;
    }
}