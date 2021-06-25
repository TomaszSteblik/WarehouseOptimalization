using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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
            var sum = 0d;

            Application.Current.MainWindow.Cursor = Cursors.Wait;
            for (int i = 0; i<10; i++)
            {
                await Task.Run(() =>
                {
                    var warehouseParameters = vm.Warehouse;
                
                    result = Optimization.OptimizationWork.WarehouseOptimization(warehouseParameters, ct, rnd.Next(1, Int32.MaxValue));
                }, ct);
                sum += result.FinalFitness;

            }

            result!.FinalFitness = sum / 10;
            
            vm.Result = $"Wynik: {result.FinalFitness}";
            Application.Current.MainWindow.Cursor = Cursors.Arrow;
            Logger.SaveWarehouseResultToFile(result);
        }

        public event EventHandler? CanExecuteChanged;
    }
}