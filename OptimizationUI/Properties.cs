using System.Text.Json.Serialization;
using OptimizationUI.Models;

namespace OptimizationUI
{
    public class Properties
    {
        public Properties(Warehouse warehouse, Distance distance)
        {
            Warehouse = warehouse;
            Distance = distance;
        }
        [JsonInclude]
        public Warehouse Warehouse;
        [JsonInclude]
        public Distance Distance;

        public Properties()
        {
            Warehouse = new Warehouse();
            Warehouse.FitnessGeneticAlgorithmParameters = new Distance();
            Warehouse.WarehouseGeneticAlgorithmParameters = new Distance();
            Distance = new Distance();
        }
    }
}