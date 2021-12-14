using System.Windows.Input;
using ReactiveUI.Fody.Helpers;
using Runner.Commands;

namespace Runner.ViewModels;

public class ParametersViewModel : ViewModelBase
{
    public ICommand SelectData { get; set; }
    public string[] SelectedFiles { get; set; }
    [Reactive] public string SelectedFilesString { get; set; }
    public ParametersViewModel()
    {
        SelectData = new SelectData();
    }
    
    
}