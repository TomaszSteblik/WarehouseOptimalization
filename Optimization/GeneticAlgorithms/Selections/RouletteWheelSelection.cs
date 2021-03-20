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
            for (int i = 0; i < fitness.Length; i++)
            {
                reversedFitness[i] = 1 / fitness[i];
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
            //Console.WriteLine(fitnessSum);
            var target = Random.NextDouble() * fitnessSum;
            var value = 0.0;
            
            for (int i = 0; i < PopulationSize; i++)
            {
                value += fitness[i];
                //Console.WriteLine(value);
                //Console.Read();

                if (value >= target)
                {
                    //Console.WriteLine($"selected : {i}");

                    return Population[i];
                }
            }

            return Population[PopulationSize - 1];
        }
        
    }
}