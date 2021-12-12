using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Runner.ViewModels;

namespace Runner.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void SelectFile(object? sender, RoutedEventArgs e)
        {
            Console.WriteLine((DataContext as MainWindowViewModel)?.DataFilePath);
            var fileDialog = new OpenFileDialog();
            var result = await fileDialog.ShowAsync(this);
            if (result is not null && result.Length > 0)
            {
                (DataContext as MainWindowViewModel).DataFilePath = result[0];
            }
        }
    }
}