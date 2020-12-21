using System;
using System.Linq;

namespace Optimization
{
    public abstract class Elimination
    {
        public abstract void EliminateAndReplace(int[][] offsprings);
        protected readonly int[][] Population;
        protected readonly int PopulationSize;
        protected readonly Random Random= new Random();
        protected Elimination(ref int[][] population)
        {
            Population = population;
            PopulationSize = population.Length;
        }

        public virtual void EliminateAndReplace(int[][] offsprings, double[] fitnessProductPlacement)
        {
            throw new NotImplementedException();
        }
    }

    public class RouletteWheelElimination : Elimination
    {
        public RouletteWheelElimination(ref int[][] population) : base(ref population)
        {
            
        }
        public override void EliminateAndReplace(int[][] offsprings)
        {
            int fitnessTotal =0;
            foreach (var chromosome in Population)
            {
                fitnessTotal += Distances.CalculatePathLength(chromosome);
            }
            int[] toDie = new int[offsprings.Length];
            for (int j = 0; j < offsprings.Length; j++)
            {
                int approx = Random.Next(0, fitnessTotal);
                for (int k = 0; k < PopulationSize; k++)
                {
                    approx += Distances.CalculatePathLength(Population[k]);
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
        public override void EliminateAndReplace(int[][] offsprings)
        {
            int numberToEliminate = offsprings.Length;
            Array.Sort(Population,(x,y)=>Distances.CalculatePathLength(x)-Distances.CalculatePathLength(y));
            for (int i = 0; i < numberToEliminate; i++)
            {
                Population[PopulationSize - 1 - i] = offsprings[i];
            }
        }

        public override void EliminateAndReplace(int[][] offsprings, double[] fitnessProductPlacement)
        {
            int numberToEliminate = offsprings.Length;
            Array.Sort(fitnessProductPlacement, Population);
            for (int i = 0; i < numberToEliminate; i++)
            {
                Population[PopulationSize - 1 - i] = offsprings[i];
            }
        }
    }
}