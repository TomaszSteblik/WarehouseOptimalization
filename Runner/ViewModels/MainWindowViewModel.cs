using System;
using System.Collections.Generic;
using System.Text;
using Avalonia;
using Avalonia.Interactivity;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Runner.ViewModels
{
    public class MainWindowViewModel : AvaloniaObject
    {
        [Reactive] public string DataFilePath { get; set; } = "test";
        public string Result { get; set; }
    }
}