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
                var tempParents = new List<int[]>();

                var feasibleParents = new List<int[]>(parents);
                int nextVertex = -1;
                for (var i = 0; i < parents.Length; i++)
                {
                    var selectedParent = feasibleParents[i];
                    var selectedParentAsList = selectedParent.ToList();
                    var indexOfCurrentVertexInSelectedParent = selectedParentAsList.IndexOf(currentVertex);
                    if (indexOfCurrentVertexInSelectedParent >= parentLength - 1 ||
                        offspring.Contains(selectedParent[indexOfCurrentVertexInSelectedParent + 1]))
                    {
                        continue;
                    }
                    tempParents.Add(selectedParent);
                }

                feasibleParents = tempParents;

                List<double> parentsArcsFitnesses = new List<double>();
                foreach (var parent in feasibleParents)
                {
                    var selectedParentAsList = parent.ToList();
                    var indexOfCurrentVertexInSelectedParent = selectedParentAsList.IndexOf(currentVertex);
                    parentsArcsFitnesses.Add(DistancesMatrix[currentVertex][parent[indexOfCurrentVertexInSelectedParent+1]]);
                }

                if (parentsArcsFitnesses.Any())
                {
                    var sum = parentsArcsFitnesses.Sum();
                    var target = Random.NextDouble();
                    double current = 0;

                    for (int i = 0; i < parentsArcsFitnesses.Count; i++)
                    {
                        current += parentsArcsFitnesses[i]/sum;
                        if (current >= target)
                        {
                            var selectedParentAsList = feasibleParents[i].ToList();
                            var indexOfCurrentVertexInSelectedParent = selectedParentAsList.IndexOf(currentVertex);
                            nextVertex = feasibleParents[i][indexOfCurrentVertexInSelectedParent + 1];
                        }
                    }
                }
                else
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