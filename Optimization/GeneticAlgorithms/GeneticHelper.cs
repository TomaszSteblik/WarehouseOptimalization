using System;
using System.Linq;

namespace Optimization.GeneticAlgorithms
{
    public class GeneticHelper
    {
        public static int[][] InitializePopulation(int[] order, int startingId, int populationSize)
        {
            Random random = new Random();
            int[][] population = new int[populationSize][];
            
            for (int i = 0; i < populationSize; i++)
            {
                int[] temp = new int[order.Length];
                for (int z = 0; z < order.Length; z++)
                {
                    temp[z] = -1;
                }

                int count = 0;
                temp[0] = startingId;
                count++;
                do
                {
                    int a = random.Next(0, order.Length);
                    if (temp.All(t => t != order[a]))
                    {
                        temp[count] = order[a];
                        count++;
                    }

                } while (count < order.Length);

                population[i] = temp;
            }

            return population;
        }
    }
}