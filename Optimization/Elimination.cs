using System;
using System.Linq;

namespace Optimization
{
    public abstract class Elimination
    {
        public abstract void EliminateAndReplace(int[][] offsprings, double[] fitnessProductPlacement);
        protected readonly int[][] Population;
        protected readonly int PopulationSize;
        protected readonly Random Random= new Random();
        protected Elimination(ref int[][] population)
        {
            Population = population;
            PopulationSize = population.Length;
        }
    }

    public class RouletteWheelElimination : Elimination
    {
        public RouletteWheelElimination(ref int[][] population) : base(ref population)
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

    public class ElitismElimination : Elimination
    {
        public ElitismElimination(ref int[][] pop) : base(ref pop)
        {
            
        }

        public override void EliminateAndReplace(int[][] offsprings, double[] fitnessProductPlacement)
        {
            int numberToEliminate = offsprings.Length;
            Array.Sort(fitnessProductPlacement,Population);
            for (int i = 0; i < numberToEliminate; i++)
            {
                Population[PopulationSize - 1 - i] = offsprings[i];
            }
        }
    }
}