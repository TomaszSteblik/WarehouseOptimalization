using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers;
using Optimization.GeneticAppliances.Warehouse;
using Optimization.Parameters;
using OptimizationUI.ViewModels;

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
            
            vm.CancelationTokenSource = new CancellationTokenSource();
            CancellationToken ct = vm.CancelationTokenSource.Token;
            WarehouseResult result = null;
            double[][] fitness = null;
            Random rnd = new Random();


            await Task.Run(() =>
            {
                var warehouseParameters = vm.Warehouse;
                warehouseParameters.WarehouseGeneticAlgorithmParameters.ConflictResolveMethod =
                    ConflictResolveMethod.WarehouseSingleProductFrequency;
                warehouseParameters.WarehouseGeneticAlgorithmParameters.RandomizedResolveMethod =
                    ConflictResolveMethod.WarehouseSingleProductFrequency;
                warehouseParameters.WarehouseGeneticAlgorithmParameters.ResolveRandomizationProbability = 0.6;
                warehouseParameters.FitnessGeneticAlgorithmParameters.Use2opt = true;
                result = Optimization.OptimizationWork.WarehouseOptimization(warehouseParameters, ct, rnd.Next(1, Int32.MaxValue));
            }, ct);
            
            //WarehouseResultLabel.Content = $"Wynik: {result.FinalFitness}";
            Logger.SaveWarehouseResultToFile(result);
        }

        public event EventHandler? CanExecuteChanged;
    }
}