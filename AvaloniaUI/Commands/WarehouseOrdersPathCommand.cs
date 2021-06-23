using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using AvaloniaUI.Models;

namespace AvaloniaUI.Commands
{
    public class WarehouseOrdersPathCommand : ICommand
    {
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public async void Execute(object? parameter)
        {
            var fileDialog = new OpenFileDialog
            {
                
            };
            if (parameter is Warehouse warehouse)
            {
                warehouse.OrdersPath = (await fileDialog.ShowAsync(null)).First();

            }
        }

        public event EventHandler? CanExecuteChanged;
    }
}