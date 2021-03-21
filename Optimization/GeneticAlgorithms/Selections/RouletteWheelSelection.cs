using System;
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
            double[] reversedFitness = new double[fitness.Length];
            var max = fitness.Max();
            var min = fitness.Min();
            for (int i = 0; i < fitness.Length; i++)
            {
                reversedFitness[i] = (1-((fitness[i] - min) / (max - min)));
            }

            int[][] parents = new int[numberOfParents][];
            for (int i = 0; i < numberOfParents; i++)
            {
                parents[i] = GenerateSingleParent(reversedFitness);
            }

            return parents;
        }
        private int[] GenerateSingleParent(double[] fitness)
        {
            var fitnessSum = fitness.Sum();
            var target = Random.NextDouble() * fitnessSum;
            for (int i = 0; i < PopulationSize; i++)
            {
                target += fitness[i];
                if (target >= fitnessSum)
                {
                    return Population[i];
                }
            }

            return Population[PopulationSize - 1];
        }
        
    }
}