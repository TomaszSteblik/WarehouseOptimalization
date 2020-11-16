using System;

namespace GeneticAlgorithm
{
    public abstract class Elimination
    {
        public abstract void EliminateAndReplace(int[][] offsprings);
        protected int[][] Population;
        protected int PopulationSize;
        protected readonly Random Random= new Random();
        public Elimination(ref int[][] population)
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
        public override void EliminateAndReplace(int[][] offsprings)
        {
            int fitnessTotal =0;
            foreach (var chromosome in Population)
            {
                fitnessTotal += Helper.Fitness(chromosome);
            }
            int[] toDie = new int[offsprings.Length];
            for (int j = 0; j < offsprings.Length; j++)
            {
                int approx = Random.Next(0, fitnessTotal);
                for (int k = 0; k < PopulationSize; k++)
                {
                    approx += Helper.Fitness(Population[k]);
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
            Array.Sort(Population,(x,y)=>Helper.Fitness(x)-Helper.Fitness(y));
            for (int i = 0; i < numberToEliminate; i++)
            {
                Population[PopulationSize - 1 - i] = offsprings[i];
            }
        }
    }
}