namespace Optimization.GeneticAppliances.Warehouse
{
    public class WarehouseResult
    {
        public double FinalFitness { get; set; }
        public double[][] fitness { get; set; }
        public int[] BestChromosome { get; set; }
        public int[][] FinalOrderPaths { get; set; }
        public int Seed { get; set; }
        
        public double[] BestFitnessInEpoch { get; set; }
        public double[] AvgFitnessInEpoch { get; set; }
    }
}