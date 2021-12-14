using System.Windows.Input;
using Runner.Commands;
using Runner.Models;

namespace Runner.ViewModels;

public class ControlViewModel : ViewModelBase
{
    public ParametersModel ParametersModel { get; set; }
    public ICommand RunDistances { get; set; }

    public ControlViewModel(ParametersModel parametersModel)
    {
        ParametersModel = parametersModel;
        RunDistances = new RunDistances(parametersModel);
    }
}