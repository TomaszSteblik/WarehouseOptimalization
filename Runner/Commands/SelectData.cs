using System;
using System.Text;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Runner.Models;
using Runner.ViewModels;

namespace Runner.Commands;

public class SelectData : ICommand
{
    public ParametersModel parameters;

    public SelectData(ParametersModel model)
    {
        parameters = model;
    }
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public async void Execute(object? parameter)
    {
        var vm = parameter as ParametersViewModel;
        var fileDialog = new OpenFileDialog();
        fileDialog.AllowMultiple = true;
        var app = Application.Current.ApplicationLifetime as ClassicDesktopStyleApplicationLifetime;
        var result = await fileDialog.ShowAsync(app?.MainWindow);
        if (result is not null && result.Length > 0)
        {
            parameters.SelectedFiles = result;
            vm.SelectedFilesString = GetFilesString(result);
        }
    }
    
    private string GetFilesString(string[] distanceSelectedFiles)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var file in distanceSelectedFiles)
        {
            if (OperatingSystem.IsWindows())
            {
                sb.Append($"{file.Split("\\")[^1].Replace(".tsp", "")}, ");
            }
            else
            {
                sb.Append($"{file.Split("/")[^1].Replace(".tsp", "")}, ");
            }
        }

        return sb.ToString();
    }

    public event EventHandler? CanExecuteChanged;
}