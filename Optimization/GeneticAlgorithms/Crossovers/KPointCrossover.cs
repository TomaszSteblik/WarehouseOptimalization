using System;
using System.Linq;
using System.Threading.Tasks;

namespace Optimization.GeneticAlgorithms.Crossovers
{
    public class KPointCrossover : Crossover
    {
        public KPointCrossover(double[][] distancesMatrix) : base(distancesMatrix)
        {
        }

        public override int[][] GenerateOffsprings(int[][] parents, int numParentsForOneChild = 2)
        {
            var geneLength = parents[0].Length;
            var rnd = new Random();
            var offsprings = new int[parents.Length / 2][];
            var startingPoint = 0;
            for (int i = 0; i < parents.Length / 2; i++)
            {
                offsprings[i] = new int[geneLength];
            }

            Parallel.ForEach(Enumerable.Range(0, parents.Length / 2).Select(i => 2 * i), i => {
                var available = parents[i].ToList();
                available.Remove(startingPoint);
                int iterator = 1;
                offsprings[i / 2][0] = startingPoint;

                for (int j = 1; j < parents[i].Length; j++)
                {
                    if (offsprings[i/2].Contains(parents[i + j%2][j]))
                    {
                        int selected = -1;
                        while (selected == -1 || offsprings[i/2].Contains(selected))
                        {
                            selected = available[rnd.Next(0, available.Count)];
                        }

                        available.Remove(selected);
                        offsprings[i / 2][iterator++] = selected;
                    }
                    else
                    {
                        available.Remove(parents[i + j%2][j]);
                        offsprings[i / 2][iterator++] = parents[i + j%2][j];
                    }
                }
            });

            return offsprings;
        }
    }
}