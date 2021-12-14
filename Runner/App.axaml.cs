using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
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
                var parametersVM = new ParametersViewModel();
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(parametersVM),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}