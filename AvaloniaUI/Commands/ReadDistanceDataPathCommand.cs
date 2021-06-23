using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Win32;
using AvaloniaUI.Models;
using AvaloniaUI.ViewModels;

namespace AvaloniaUI.Commands
{
    public class ReadDistanceDataPathCommand : ICommand
    {
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public async void Execute(object? parameter)
        {
            var distance = parameter as DistanceViewModel;
            
            Assembly asm = Assembly.GetExecutingAssembly();
            string path = System.IO.Path.GetDirectoryName(asm.Location);
            StringBuilder sb = new StringBuilder();
            var pathParts = path.Split("\\");
            if (pathParts.Length > 4)
                for (int i = 0; i < pathParts.Length - 4; i++)
                {
                    sb.Append(pathParts[i] + "\\");
                }

            sb.Append("Data");

            OpenFileDialog fileDialog = new OpenFileDialog();
            var filters = new List<FileDialogFilter>();
            var extenstions = new List<string>();
            extenstions.Add("txt");
            extenstions.Add("tsp");
            
            filters.Add(new FileDialogFilter
            {
                Extensions = extenstions
            });
            // fileDialog.Filter = "All files (*.*)|*.*|txt files (*.txt)|*.txt|tsp files (*.tsp)|*.tsp";
            // fileDialog.RestoreDirectory = true;
            // fileDialog.Multiselect = true;
            fileDialog.Directory = sb.ToString();
            var app = Application.Current.ApplicationLifetime as ClassicDesktopStyleApplicationLifetime;
            distance.SelectedFiles =(await fileDialog.ShowAsync(app?.MainWindow));
            distance.SelectedFilesString = GetFilesString(distance.SelectedFiles);
        }
        
        private string GetFilesString(string[] distanceSelectedFiles)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var file in distanceSelectedFiles)
            {
                sb.Append($"{file.Split("\\")[^1]}, ");
            }

            return sb.ToString();
        }

        public event EventHandler? CanExecuteChanged;
    }
}