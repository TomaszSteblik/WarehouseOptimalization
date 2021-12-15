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

        public ParametersViewModel ParametersViewModel { get; }
        public ControlViewModel ControlViewModel { get; }
        public LogViewModel LogViewModel { get; }
        public MainWindowViewModel(ParametersViewModel parametersViewModel, ControlViewModel controlViewModel, LogViewModel logViewModel)
        {
            LogViewModel = logViewModel;
            ParametersViewModel = parametersViewModel;
            ControlViewModel = controlViewModel;
        }

        public MainWindowViewModel()
        {
            
        }
    }
}