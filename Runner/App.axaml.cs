using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Runner.Models;
using Runner.ViewModels;
using Runner.Views;

namespace Runner
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var parametersModel = new ParametersModel();
                var logModel = new ConsoleLogModel();
                var parametersVM = new ParametersViewModel(parametersModel);
                var controlVM = new ControlViewModel(parametersModel, logModel);
                var logVM = new LogViewModel(logModel);
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(parametersVM, controlVM, logVM),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}