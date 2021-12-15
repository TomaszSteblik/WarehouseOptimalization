using ReactiveUI.Fody.Helpers;
using Runner.Models;

namespace Runner.ViewModels;

public class LogViewModel : ViewModelBase
{
    [Reactive] public ConsoleLogModel LogModel { get; set; }

    public LogViewModel(ConsoleLogModel logModel)
    {
        LogModel = logModel;
    }
}