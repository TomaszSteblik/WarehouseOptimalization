using System;
using System.Collections.Generic;
using System.Linq;
using Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers;
using Optimization.Helpers;

namespace Optimization.GeneticAlgorithms.Crossovers
{
    internal class HProXCrossover : Crossover
    {
        private double[][] DistancesMatrix { get; }
        public HProXCrossover(ConflictResolver resolverConflict, ConflictResolver resolverRandomized, Random random) : base(resolverConflict, resolverRandomized,  random)
        {
            DistancesMatrix = Distances.GetInstance().DistancesMatrix;
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

                if (Random.NextDouble() < ResolverRandomized.RandomizationProbability)
                {
                    nextVertex = ResolverRandomized.ResolveConflict(currentVertex, availableVertexes);
                }
                
                if (nextVertex == -1)
                {
                    _resolveCount++;
                    nextVertex = ResolverConflict.ResolveConflict(currentVertex, availableVertexes);
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