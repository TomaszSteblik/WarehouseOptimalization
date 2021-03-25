using System;
using System.Collections.Generic;
using System.Linq;

namespace Optimization.GeneticAlgorithms.Crossovers
{
    internal class HGreXCrossover : Crossover
        {
            public HGreXCrossover(double[][] distancesMatrix) : base(distancesMatrix)
            {
            }

            public override int[] GenerateOffspring(int[][] parents)
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

                
                var min = double.MaxValue;
                int minIndex =-1;
                
                for (int i = 0; i < feasibleParents.Count; i++)
                {
                    if (DistancesMatrix[currentVertex][
                        feasibleParents[i][feasibleParents[i].ToList().IndexOf(currentVertex) + 1]] < min)
                    {
                        min = DistancesMatrix[currentVertex][
                            feasibleParents[i][feasibleParents[i].ToList().IndexOf(currentVertex) + 1]];
                        minIndex = i;
                    }
                }
                
                if(minIndex>=0)
                    nextVertex = feasibleParents[minIndex][feasibleParents[minIndex].ToList().IndexOf(currentVertex) + 1];
                
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