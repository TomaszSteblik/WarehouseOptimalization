using System;

namespace Optimization.GeneticAlgorithms.Selections
{
    internal class ElitismSelection : Selection
    {
        public ElitismSelection(int[][] population, Random random) : base(population, random)
        {
            
        }

        public override int[][] GenerateParents(int numberOfParents, double[] fitness)
        {
            Array.Sort(fitness,Population);
            
            int[][] parents = new int[numberOfParents][];

            for (int i = 0; i < numberOfParents; i++)
            {
                parents[i] = (int[]) Population[i].Clone();
            }
                       
            
            return parents;
        }
    }
}