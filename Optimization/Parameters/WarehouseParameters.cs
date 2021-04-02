namespace Optimization.Parameters
{
    public class WarehouseParameters
    {
        public OptimizationParameters WarehouseGeneticAlgorithmParameters { get; set; }
        public OptimizationParameters FitnessGeneticAlgorithmParameters { get; set; }
        public virtual string WarehousePath { get; set; }
        public virtual string OrdersPath { get; set; }

    }
}