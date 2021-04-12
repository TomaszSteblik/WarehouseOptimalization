using System.Linq;
using Optimization.Helpers;

namespace Optimization.GeneticAppliances.TSP
{
    public class TSPResult
    {
        public int EpochCount { get; }
        public double[][] fitness { get; }
        
        public int[] BestGene { get; }
        public double FinalFitness { get; }

        public TSPResult(double[][] fitness, int[] bestGene)
        {
            BestGene = bestGene;
            EpochCount = fitness.Length;
            this.fitness = fitness;
            FinalFitness = Fitness.CalculateFitness(BestGene);
        }
    }
}