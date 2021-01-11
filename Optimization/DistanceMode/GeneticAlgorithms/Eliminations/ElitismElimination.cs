using System;

namespace Optimization.DistanceMode.GeneticAlgorithms.Eliminations
{
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