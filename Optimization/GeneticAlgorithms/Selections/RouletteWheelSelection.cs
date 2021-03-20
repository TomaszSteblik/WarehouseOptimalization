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
            var maxFitness = fitness.Max();
            double[] reversedFitness = new double[fitness.Length];
            for (int i = 0; i < fitness.Length; i++)
            {
                reversedFitness[i] = maxFitness - fitness[i];
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
            var target = Random.NextDouble();
            var value = 0.0;
            
            for (int i = 0; i < PopulationSize; i++)
            {
                value += (fitness[i])/fitnessSum;
                Console.WriteLine(value);
                if (value >= target)
                {
                    return Population[i];
                }
            }

            return Population[PopulationSize - 1];
        }
        
    }
}