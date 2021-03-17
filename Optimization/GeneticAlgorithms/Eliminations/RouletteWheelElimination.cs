using System.Linq;

namespace Optimization.GeneticAlgorithms.Eliminations
{
    internal class RouletteWheelElimination : Elimination
    {
        public RouletteWheelElimination(int[][] population) : base(population)
        {
            
        }
        public override void EliminateAndReplace(int[][] offsprings,double[] fitness)
        {
            double fitnessTotal = fitness.Sum();
            int[] toDie = new int[offsprings.Length];
            for (int j = 0; j < offsprings.Length; j++)
            {
                double approx = Random.NextDouble()*fitnessTotal;
                for (int k = 0; k < PopulationSize; k++)
                {
                    approx += fitness[k];
                    if (approx >= fitnessTotal)
                    {
                        toDie[j] = k;
                        break;
                    }
                }
            }
            for (int j = 0; j < offsprings.Length; j++)
            {
                Population[toDie[j]] = offsprings[j];
            }
        }
        
    }
}