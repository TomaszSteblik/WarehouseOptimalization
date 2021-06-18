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
            var warehouseViewModel = new WarehouseViewModel();
            warehouseViewModel.Warehouse.FitnessGeneticAlgorithmParameters = new Distance();
            warehouseViewModel.Warehouse.WarehouseGeneticAlgorithmParameters = new Distance();
        }

        public MainViewModel(WarehouseViewModel warehouseViewModel, DistanceViewModel distanceViewModel)
        {
            WarehouseViewModel = warehouseViewModel;
            DistanceViewModel = distanceViewModel;
        }
    }
}