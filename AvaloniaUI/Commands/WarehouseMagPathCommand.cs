using System;
using System.Windows.Input;
using Microsoft.Win32;
using AvaloniaUI.Models;

namespace AvaloniaUI.Commands
{
    public class WarehouseMagPathCommand : ICommand
    {
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {

            // var fileDialog = new OpenFileDialog
            // {
            //     Filter = "txt files (*.txt)|*.txt", 
            //     RestoreDirectory = true
            // };
            // fileDialog.ShowDialog();
            // var warehouse = parameter as Warehouse;
            // warehouse.WarehousePath = fileDialog.FileName;
        }

        public event EventHandler? CanExecuteChanged;
    }
}