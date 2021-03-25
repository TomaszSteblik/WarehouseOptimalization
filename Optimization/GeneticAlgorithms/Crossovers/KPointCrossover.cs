using System;
using System.Linq;
using System.Threading.Tasks;
using Optimization.Parameters;

namespace Optimization.GeneticAlgorithms.Crossovers
{
    public class KPointCrossover : Crossover
    {
        public KPointCrossover(double[][] distancesMatrix) : base(distancesMatrix)
        {
        }

        public override int[] GenerateOffspring(int[][] parents)
        {
            var geneLength = parents[0].Length;
            var parentsNumber = parents.Length;
            var rnd = new Random();
            var offspring = new int[geneLength];

            var available = parents[0].ToList();
            available.Remove(parents[0][0]);
            offspring[0] = parents[0][0];

            int iterator = 1;

            for (int j = 1; j < geneLength; j++)
            {
                if (offspring.Contains(parents[j % parentsNumber][j]))
                {
                    int selected = -1;
                    while (selected == -1 || offspring.Contains(selected))
                    {
                        selected = available[rnd.Next(0, available.Count)];
                    }

                    available.Remove(selected);
                    offspring[iterator++] = selected;
                }
                else
                {
                    available.Remove(parents[j % parentsNumber][j]);
                    offspring[iterator++] = parents[j % parentsNumber][j];
                }
            }

            return offspring;
        }
    }
}