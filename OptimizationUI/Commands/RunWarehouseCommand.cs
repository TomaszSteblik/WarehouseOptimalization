using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers;
using Optimization.GeneticAppliances.Warehouse;
using Optimization.Parameters;
using OptimizationUI.ViewModels;
using OxyPlot;
using OxyPlot.Wpf;
using LineSeries = OxyPlot.Series.LineSeries;

namespace OptimizationUI.Commands
{
    public class RunWarehouseCommand : ICommand
    {
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public async void Execute(object? parameter)
        {
            var vm = parameter as WarehouseViewModel;
            var runs = vm.Runs;
            var epochs = vm.Warehouse.WarehouseGeneticAlgorithmParameters.MaxEpoch;
            var dataSet = vm.Warehouse.WarehousePath.Split(@"\").Last();
            var randomizedResolver =
                vm.Warehouse.WarehouseGeneticAlgorithmParameters.RandomizedResolveMethod.ToString();
            var conflictResolver = vm.Warehouse.WarehouseGeneticAlgorithmParameters.ConflictResolveMethod.ToString();

            var name = epochs + "_" + dataSet + "_" + conflictResolver + "_" + randomizedResolver;
            
            vm.CancelationTokenSource = new CancellationTokenSource();
            CancellationToken ct = vm.CancelationTokenSource.Token;
            WarehouseResult result = null;
            double[][] AvgFitnessRunEpoch = new double[runs][];
            double[][] MinFitnessRunEpoch = new double[runs][];
            Random rnd = new Random();
            var sum = 0d;
            WarehouseResult[] results = new WarehouseResult[runs];
            double[][][] runFitnesses = new double[runs][][];

            Application.Current.MainWindow.Cursor = Cursors.Wait;
            for (int i = 0; i<runs; i++)
            {
                await Task.Run(() =>
                {
                    var warehouseParameters = vm.Warehouse;
                
                    result = Optimization.OptimizationWork.WarehouseOptimization(warehouseParameters, ct, rnd.Next(1, Int32.MaxValue));
                }, ct);
                results[i] = result;
                runFitnesses[i] = result.fitness;
                sum += result.FinalFitness;

            }

            result!.FinalFitness = sum / runs;
            var bestFitnesses = GetBestFitnesses(runFitnesses);
            var avgFitnesses = GetAverageFitnesses(runFitnesses);
            
            vm.Result = $"Wynik: {sum / runs}";
            var tmpPlotModel = new PlotModel();
            var minSeries = new LineSeries()
            {
                Title = "MinFitness",
            };
            for (int i = 0; i < bestFitnesses[0].Length; i++)
            {
                minSeries.Points.Add(new DataPoint(i, bestFitnesses.Min(d => d[i])));
            }
            
            
            var avgSeries = new LineSeries()
            {
                Title = "AvgFitness"
            };
            for (int i = 0; i < bestFitnesses[0].Length; i++)
            {
                avgSeries.Points.Add(new DataPoint(i, bestFitnesses.Average(d => d[i])));
            }

            
            Application.Current.MainWindow.Cursor = Cursors.Arrow;
            tmpPlotModel.Series.Add(minSeries);
            tmpPlotModel.Series.Add(avgSeries);

            vm.PlotModel = tmpPlotModel;
            
            
            
            Logger.SaveWarehouseResultToFile(result, tmpPlotModel, name);
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

        

        public event EventHandler? CanExecuteChanged;
    }
}