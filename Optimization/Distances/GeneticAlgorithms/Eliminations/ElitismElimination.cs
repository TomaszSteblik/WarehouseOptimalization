using System;

namespace Optimization.Distances.GeneticAlgorithms.Eliminations
{
    internal class ElitismElimination : Elimination
    {
        public ElitismElimination(int[][] pop) : base(pop)
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