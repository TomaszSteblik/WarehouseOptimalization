using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Runner.ViewModels;

namespace Runner.Commands;

public class SelectData : ICommand
{
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public async void Execute(object? parameter)
    {
        var vm = parameter as MainWindowViewModel;
        var fileDialog = new OpenFileDialog();
        var app = Application.Current.ApplicationLifetime as ClassicDesktopStyleApplicationLifetime;
        var result = await fileDialog.ShowAsync(app?.MainWindow);
        if (result is not null && result.Length > 0)
        {
            vm.DataFilePath = result[0];
        }
    }

    public event EventHandler? CanExecuteChanged;
}