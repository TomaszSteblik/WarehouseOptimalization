using System.Linq;

namespace Optimization.GeneticAlgorithms.Selections
{
    internal class RouletteWheelSelection : Selection
    {
        public RouletteWheelSelection(int[][] population) : base(population)
        {
            
        }

        public override int[][] GenerateParents(int numberOfParents, double[] fitness)
        {
            int[][] parents = new int[numberOfParents][];
            for (int i = 0; i < numberOfParents; i++)
            {
                parents[i] = GenerateSingleParent(fitness);
            }

            return parents;
        }

        private int[] GenerateSingleParent(double[] fitness)
        {

            var fitnessSum = fitness.Sum();
            var target = Random.NextDouble();
            var value = 0.0;
            
            for (int i = 0; i < PopulationSize; i++)
            {
                value += fitness[i]/fitnessSum;
                if (value >= target)
                {
                    return Population[i];
                }
            }

            return Population[PopulationSize - 1];
        }
        
    }
}