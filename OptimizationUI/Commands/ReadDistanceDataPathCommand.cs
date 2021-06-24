using System;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using Microsoft.Win32;
using OptimizationUI.Models;
using OptimizationUI.ViewModels;

namespace OptimizationUI.Commands
{
    public class ReadDistanceDataPathCommand : ICommand
    {
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
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
            fileDialog.Filter = "All files (*.*)|*.*|txt files (*.txt)|*.txt|tsp files (*.tsp)|*.tsp";
            fileDialog.RestoreDirectory = true;
            fileDialog.Multiselect = true;
            fileDialog.InitialDirectory = sb.ToString();
            fileDialog.ShowDialog();
            distance.SelectedFiles = fileDialog.FileNames;
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