using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Avalonia;
using Avalonia.Interactivity;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Runner.Commands;

namespace Runner.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        public MainWindowViewModel()
        {
            SelectData = new SelectData();
            RunDistances = new RunDistances();
        }
        public ICommand SelectData { get; set; }
        public ICommand RunDistances { get; set; }
        [Reactive] public string? DataFilePath { get; set; } = "";
        [Reactive] public string Result { get; set; } = "";
    }
}