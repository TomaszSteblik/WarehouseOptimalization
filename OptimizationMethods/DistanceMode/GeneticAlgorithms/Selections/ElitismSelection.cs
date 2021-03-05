using System;

namespace OptimizationMethods.DistanceMode.GeneticAlgorithms.Selections
{
    public class ElitismSelection : Selection
    {
        public ElitismSelection(int[][] population) : base(population)
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