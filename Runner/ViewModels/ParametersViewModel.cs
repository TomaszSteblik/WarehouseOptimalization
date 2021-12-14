using System.Windows.Input;
using ReactiveUI.Fody.Helpers;
using Runner.Commands;
using Runner.Models;

namespace Runner.ViewModels;

public class ParametersViewModel : ViewModelBase
{
    public ICommand SelectData { get; set; }
    public ParametersModel ParametersModel { get; set; }
    [Reactive] public string SelectedFilesString { get; set; }
    public ParametersViewModel(ParametersModel model)
    {
        ParametersModel = model;
        SelectData = new SelectData(ParametersModel);
    }
    
    
}