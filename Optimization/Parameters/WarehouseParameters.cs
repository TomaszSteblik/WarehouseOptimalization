namespace Optimization.Parameters
{
    public class WarehouseParameters
    {
        public virtual OptimizationParameters WarehouseGeneticAlgorithmParameters { get; set; }
        public virtual OptimizationParameters FitnessGeneticAlgorithmParameters { get; set; }
        public virtual string WarehousePath { get; set; }
        public virtual string OrdersPath { get; set; }

    }
}