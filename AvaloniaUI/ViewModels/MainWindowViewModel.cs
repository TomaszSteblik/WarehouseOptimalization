using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public DistanceViewModel DistanceViewModel { get; set; } = new DistanceViewModel();
        public WarehouseViewModel WarehouseViewModel { get; set; } = new WarehouseViewModel();
    }
}