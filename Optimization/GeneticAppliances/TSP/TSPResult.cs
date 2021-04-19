using System.Linq;
using Optimization.Helpers;

namespace Optimization.GeneticAppliances.TSP
{
    public class TSPResult
    {
        public int EpochCount { get; }
        public double[][] fitness { get; }
        
        public int[] BestGene { get; }
        
        public int[] ResolveInEpoch { get; set; }
        public double[] ResolvePercentInEpoch { get; set; }
        public int[] RandomizedResolveInEpoch { get; set; }
        
        public double[] RandomizedResolvePercentInEpoch { get; set; }

        public double FinalFitness { get; }
        
        public int Seed { get; set; }

        public TSPResult(double[][] fitness, int[] bestGene)
        {
            BestGene = bestGene;
            EpochCount = fitness.Length;
            this.fitness = fitness;
            FinalFitness = Fitness.CalculateFitness(BestGene);
            
        }
    }
}