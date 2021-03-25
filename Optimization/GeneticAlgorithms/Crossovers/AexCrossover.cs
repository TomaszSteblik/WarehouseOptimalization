using System.Collections.Generic;
using System.Linq;

namespace Optimization.GeneticAlgorithms.Crossovers
{
    internal class AexCrossover : Crossover
    {
        public AexCrossover(double[][] distancesMatrix) : base(distancesMatrix)
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
                //check for feasibility
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