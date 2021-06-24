using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Optimization;
using Optimization.GeneticAlgorithms.Crossovers;
using Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers;
using Optimization.GeneticAppliances.TSP;
using Optimization.Parameters;
using OptimizationUI.ViewModels;

namespace OptimizationUI.Commands
{
    public class LoopAllParametersCommand : ICommand
    {
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public async void Execute(object? parameter)
        {
            var vm = parameter as DistanceViewModel;
            List<string>[] Savg;
            List<string>[] Smin;

            if (!Directory.Exists(vm?.DistanceParameters.ResultPath))
                Directory.CreateDirectory(vm.DistanceParameters.ResultPath);

            vm.IsDistanceStartButtonEnabled = false;

            EventHandler<int> BaseGeneticOnOnNextIteration()
            {
                return (_, iteration) =>
                {
                    vm.ProgressBarValue++;
                    
                };
            }


            var runs = vm.Runs;
            var seed = vm.CurrentSeed;
            if (vm.RandomSeed)
            {
                var random = new Random();
                seed = random.Next(1, Int32.MaxValue);
                vm.CurrentSeed = seed;
            }
            
            vm.CancellationTokenSource = new CancellationTokenSource();
            var ct = vm.CancellationTokenSource.Token;
            var parameters = vm.DistanceParameters as OptimizationParameters;

            var crossovers = new CrossoverMethod[4];
            crossovers[0] = CrossoverMethod.Aex;
            crossovers[1] = CrossoverMethod.HProX;
            crossovers[2] = CrossoverMethod.HGreX;
            crossovers[3] = CrossoverMethod.HRndX;

            Application.Current.MainWindow.Cursor = Cursors.Wait;


            await Task.Run(() =>
            {
                Directory.CreateDirectory(vm.DistanceParameters.ResultPath + "\\" + seed.ToString());
                //SerializeParameters(vm.DistanceParameters.ResultPath + "\\" + seed + "/parameters.json");

               
                Optimization.GeneticAlgorithms.BaseGenetic.OnNextIteration += BaseGeneticOnOnNextIteration();
                var operationType = new List<string>();
                var ren = -1;

                foreach (var dataset in vm.SelectedFiles)
                {
                    vm.DistanceParameters.DataPath = dataset;
                    var datasetName = dataset.Split('\\')[^1]
                            .Remove(vm.DistanceParameters.DataPath.Split('\\')[^1].IndexOf('.'));
                    parameters.DataPath = dataset;
                    var results = new TSPResult[runs];
                    

                    double[] pr = { 0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0 };
                    
                    vm.ProgressBarMaximum = (runs * vm.DistanceParameters.MaxEpoch * 9 * crossovers.Length 
                        * vm.SelectedFiles.Length * Enum.GetValues(typeof(ConflictResolveMethod)).Length 
                        * Enum.GetValues(typeof(ConflictResolveMethod)).Length * pr.Length) - 1;
                    vm.ProgressBarValue = 0;

                    for (int pri = 0; pri < pr.Length; pri++)
                    {
                        vm.Smin[pri] = new List<string>();
                        vm.Savg[pri] = new List<string>();

                        vm.xt = 0;
                        parameters.ResolveRandomizationProbability = pr[pri];
                        vm.DistanceParameters.ResolveRandomizationProbability = pr[pri];

                        ren++;
                        foreach (var crossoverMethod in crossovers)
                        {
                            parameters.CrossoverMethod = crossoverMethod;

                            foreach (ConflictResolveMethod randomizedResolve in Enum.GetValues(typeof(ConflictResolveMethod)))
                            {
                                parameters.RandomizedResolveMethod = randomizedResolve;
                                if(randomizedResolve.ToString().Contains("Warehouse"))
                                    continue;

                                foreach (ConflictResolveMethod conflictResolve in Enum.GetValues(typeof(ConflictResolveMethod)))
                                {
                                    if(conflictResolve.ToString().Contains("Warehouse"))
                                        continue;
                                    
                                    var fileName = seed + "/" + runs + "_" + dataset.Split('\\')[^1]
                                                       .Remove(vm.DistanceParameters.DataPath.Split('\\')[^1].IndexOf('.')) + "_BEST_"
                                                   + Enum.GetName(crossoverMethod) + "_" + Enum.GetName(randomizedResolve) + "_" + Enum.GetName(conflictResolve) + ".txt";
                                    var s = "epoch;best_distance;avg_best_10%;median;avg_worst_10%;avg;worst_distance;std_deviation;conflict_percentage;avgDiff;0Diff;02Diff\n";

                                    parameters.ConflictResolveMethod = conflictResolve;
                                    Parallel.For(0, runs, i =>
                                    {
                                        results[i] = OptimizationWork.TSP(parameters, ct, seed + i);
                                    });

                                    //s += CreateDistanceLogsBestPerRunsParams(results, Enum.GetName(parameters.ConflictResolveMethod),
                                    //    Enum.GetName(parameters.RandomizedResolveMethod));

                                    string n0 = $"{seed}/{datasetName}_data.txt";
                                    string n1 = Enum.GetName(parameters.ConflictResolveMethod);
                                    string n2 = Enum.GetName(parameters.RandomizedResolveMethod);
                                    string n3 = Enum.GetName(parameters.CrossoverMethod);

                                    if (ren < 1)
                                        operationType.Add(n3 + ";" + n1 + ";" + n2 + ";");

                                    //SaveDistanceArticleResultsToFile(n0, results, n1, n2, seed, pri);

                                    File.AppendAllText(vm.DistanceParameters.ResultPath + "\\" + fileName, s);

                                    fileName = seed + "/" + runs + "_" + dataset.Split('\\')[^1]
                                                   .Remove(vm.DistanceParameters.DataPath.Split('\\')[^1].IndexOf('.')) + "_AVG_"
                                               + Enum.GetName(crossoverMethod) + "_" + Enum.GetName(randomizedResolve) + "_" + Enum.GetName(conflictResolve) + ".txt";


                                    //s += CreateDistanceLogsPerRunsParams(results,
                                    //    Enum.GetName(parameters.ConflictResolveMethod),
                                    //    Enum.GetName(parameters.RandomizedResolveMethod));
                                    File.AppendAllText(vm.DistanceParameters.ResultPath + "\\" + fileName, s);

                                }

                            }

                        }
                    }

                    string fresMin = "crossover;conflict;random;m00;m01;m02;m03;m04;m05;m06;m07;m08;m09;m10\r\n";
                    string fresAvg = "crossover;conflict;random;a00;a01;a02;a03;a04;a05;a06;a07;a08;a09;a10\r\n";

                    for (int j = 0; j < vm.Smin[0].Count; j++)
                    {

                        fresMin += operationType[j];
                        fresAvg += operationType[j];

                        for (int i = 0; i < 11; i++)
                        {
                            fresMin += vm.Smin[i][j] + ";";
                            fresAvg += vm.Savg[i][j] + ";";
                        }
                        fresMin += "\r\n";
                        fresAvg += "\r\n";
                    }

                    string f1 = vm.DistanceParameters.ResultPath + @"\" + System.IO.Path.GetFileNameWithoutExtension(dataset) + "_" + vm.DistanceParameters.MaxEpoch;

                    File.WriteAllText(f1 + "_fresMin.txt", fresMin);
                    File.WriteAllText(f1 + "_fresAvg.txt", fresAvg);


                }
            }, ct);


            vm.ProgressBarValue = 0;
            Application.Current.MainWindow.Cursor = Cursors.Arrow;
        }
        
        public event EventHandler? CanExecuteChanged;
    }
}