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
            var rnd = new Random();
            var max = fitness.Max();
            var min = fitness.Min();

            var weights = new double[fitness.Length];
            for (int i = 0; i < fitness.Length; i++)
            {
                weights[i] = min / (min + fitness[i]) - min / (min + max);
            }

            var weightsSum = weights.Sum();
            
            int[][] parents = new int[numberOfParents][];
            for (int i = 0; i < numberOfParents; i++)
            {
                var roll = rnd.NextDouble() * weightsSum;
                var tmp = 0d;
                for (int j = 0; j < fitness.Length; j++)
                {
                    tmp += weights[j];
                    if (tmp >= roll)
                    {
                        parents[i] = Population[j];
                        break;
                    }
                    
                }
            }

            return parents;
            
        }

    }
}