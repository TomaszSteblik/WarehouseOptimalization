using System;
using System.Collections.Generic;
using System.Linq;

namespace Optimization.GeneticAlgorithms.Crossovers
{
    internal class HProXCrossover : Crossover
    {
        public HProXCrossover(double[][] distancesMatrix) : base(distancesMatrix)
        {
        }

        public override int[][] GenerateOffsprings(int[][] parents, int numParentsForOneChild = 2)
        {
            var parentsLength = parents.Length;
            var amountOfChildren = parentsLength / 2;
            int[][] offsprings = new int[amountOfChildren][];

            for (int c = 0; c < amountOfChildren; c++)
            {
                int[][] prnt = new int[numParentsForOneChild][];
                for (int i = 0; i < numParentsForOneChild; i++)
                {
                    prnt[i] = parents[Random.Next(parents.Length)];
                }

                offsprings[c] = GenerateOffspring(prnt);
            }

            return offsprings;
        }
        protected int[] GenerateOffspring(int[][] parents)
        {
            var parentLength = parents[0].Length;
            var currentVertex = parents[0][0];
            var offspring = new int[parentLength];
            offspring[0] = currentVertex;
            var availableVertexes = new List<int>(parents[0]);
            availableVertexes.Remove(currentVertex);
            var counter = 1;

            while (counter < parentLength)
            {
                var feasibleParents = new List<int[]>(parents);
                var nextVertex = -1;
                for (var i = 0; i < parents.Length; i++)
                {
                    var selectedParent = parents[i];
                    var selectedParentAsList = selectedParent.ToList();
                    var indexOfCurrentVertexInSelectedParent = selectedParentAsList.IndexOf(currentVertex);
                    if (indexOfCurrentVertexInSelectedParent >= parentLength - 1 ||
                        offspring.Contains(selectedParent[indexOfCurrentVertexInSelectedParent + 1]))
                    {
                        feasibleParents.Remove(selectedParent);
                    }
                }


                double[] fitness = new double[feasibleParents.Count];

                
                for (int i = 0; i < feasibleParents.Count; i++)
                {
                    fitness[i] =
                        1/DistancesMatrix[currentVertex][
                            feasibleParents[i][feasibleParents[i].ToList().IndexOf(currentVertex) + 1]];
                }
                

                var sum = fitness.Sum();
                var approx = Random.NextDouble() * sum;
                for (int i = 0; i < fitness.Length; i++)
                {
                    approx += fitness[i];
                    if (approx >= sum)
                    {
                        nextVertex = feasibleParents[i][feasibleParents[i].ToList().IndexOf(currentVertex) + 1];
                        break;

                    }
                }

                if (nextVertex == -1)
                {
                    nextVertex = availableVertexes[Random.Next(0, availableVertexes.Count)];
                }
                offspring[counter] = nextVertex;
                availableVertexes.Remove(nextVertex);
                currentVertex = nextVertex;
                counter++;
            }
            return offspring;
        }

    }
}