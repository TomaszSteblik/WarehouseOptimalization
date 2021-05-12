namespace Optimization.GeneticAppliances.Warehouse
{
    public class WarehouseResult
    {
        public double FinalFitness { get; set; }
        public double[][] fitness { get; set; }
        public int[] BestGene { get; set; }
        public int[][] FinalOrderPaths { get; set; }
        public int Seed { get; set; }
    }
}