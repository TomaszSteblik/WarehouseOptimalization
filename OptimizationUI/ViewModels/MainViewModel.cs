using OptimizationUI.Models;

namespace OptimizationUI.ViewModels
{
    public class MainViewModel
    {
        public DistanceViewModel DistanceViewModel { get; set; }
        public WarehouseViewModel WarehouseViewModel { get; set; }

        public MainViewModel()
        {
            DistanceViewModel = new DistanceViewModel();
            WarehouseViewModel = new WarehouseViewModel();
        }

        public MainViewModel(WarehouseViewModel warehouseViewModel, DistanceViewModel distanceViewModel)
        {
            WarehouseViewModel = warehouseViewModel;
            DistanceViewModel = distanceViewModel;
        }
    }
}