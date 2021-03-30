namespace Optimization.Parameters
{
    public class WarehouseParameters
    {
        public OptimizationParameters WarehouseGeneticAlgorithmParameters;
        public OptimizationParameters FitnessGeneticAlgorithmParameters;
        public virtual string WarehousePath { get; set; }
        public virtual string OrdersPath { get; set; }

    }
}