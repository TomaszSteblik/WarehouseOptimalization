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

            double fitnessMin = fitness.Min();
            double fitnessMax = fitness.Max();
            double fitnessSum = Random.NextDouble();
            
            for (int i = 0; i < PopulationSize; i++)
            {
                fitnessSum += ((fitness[i] - fitnessMin)/(fitnessMax-fitnessMin));
                if (fitnessSum >= 1)
                {
                    return Population[i];
                }
            }

            return Population[PopulationSize - 1];
        }
        
    }
}