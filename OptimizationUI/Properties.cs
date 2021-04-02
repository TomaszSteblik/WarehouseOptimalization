using System.Text.Json.Serialization;

namespace OptimizationUI
{
    public class Properties
    {
        public Properties(WarehouseViewModel warehouseViewModel, DistanceViewModel distanceViewModel)
        {
            WarehouseViewModel = warehouseViewModel;
            DistanceViewModel = distanceViewModel;
        }
        [JsonInclude]
        public WarehouseViewModel WarehouseViewModel;
        [JsonInclude]
        public DistanceViewModel DistanceViewModel;

        public Properties()
        {
            WarehouseViewModel = new WarehouseViewModel();
            WarehouseViewModel.FitnessGeneticAlgorithmParameters = new DistanceViewModel();
            WarehouseViewModel.WarehouseGeneticAlgorithmParameters = new DistanceViewModel();
            DistanceViewModel = new DistanceViewModel();
        }
    }
}