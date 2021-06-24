using System;
using System.Windows.Input;

namespace OptimizationUI.Commands
{
    public class ReadDistanceResultPathCommand : ICommand
    {
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            var resultPath = parameter as string;
            var fileDialog = new FolderBrowserForWPF.Dialog();
            if (fileDialog.ShowDialog() == true)
            {
                resultPath = fileDialog.FileName;
            }
        }

        public event EventHandler? CanExecuteChanged;
    }
}