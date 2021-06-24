using System;
using System.Threading;
using System.Windows.Input;

namespace OptimizationUI.Commands
{
    public class CancelCommand : ICommand
    {
       
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            (parameter as CancellationTokenSource)?.Cancel();
        }

        public event EventHandler? CanExecuteChanged;
    }
}