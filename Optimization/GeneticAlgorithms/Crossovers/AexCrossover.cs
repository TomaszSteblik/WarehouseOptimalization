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
            var parentsLength = parents.Length;
            var currentParentIndex = 0;
            
            while (counter < parentLength)
            {
                var nextVertex = -1;


                var selectedParent =  parents[currentParentIndex];
                var selectedParentAsList = selectedParent.ToList();
                var indexOfCurrentVertexInSelectedParent = selectedParentAsList.IndexOf(currentVertex);
                if (indexOfCurrentVertexInSelectedParent >= parentLength - 1 ||
                    offspring.Contains(selectedParent[indexOfCurrentVertexInSelectedParent + 1]))
                {
                    
                    
                }
                else
                {
                    nextVertex = selectedParent[indexOfCurrentVertexInSelectedParent + 1];
                    if (currentParentIndex >= parentsLength -1)
                    {
                        currentParentIndex = 0;
                    }
                    else
                    {
                        currentParentIndex++;
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