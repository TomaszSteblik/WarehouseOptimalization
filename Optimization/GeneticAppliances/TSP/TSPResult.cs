using System.Linq;

namespace Optimization.GeneticAppliances.TSP
{
    public class TSPResult
    {
        public int EpochCount { get; }
        public double[][] fitness { get; }
        public double FinalFitness { get; }

        public TSPResult(double[][] fitness)
        {
            EpochCount = fitness.Length;
            this.fitness = fitness;
            FinalFitness = fitness[^1].Min();
        }
    }
}