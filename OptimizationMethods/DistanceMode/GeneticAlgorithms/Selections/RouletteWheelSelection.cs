using System.Linq;

namespace OptimizationMethods.DistanceMode.GeneticAlgorithms.Selections
{
    public class RouletteWheelSelection : Selection
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
            double fitnessSum = fitness.Sum();
            
            for (int i = 0; i < PopulationSize; i++)
            {
                fitnessSum += (1.0 / fitness[i]);
                if (fitnessSum >= 1)
                {
                    return Population[i];
                }
            }

            return Population[PopulationSize - 1];
        }
        
    }
}