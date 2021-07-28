using System;
using System.Collections.Generic;
using System.Linq;
using Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers;

namespace Optimization.GeneticAlgorithms.Crossovers
{
    internal class AexCrossover : Crossover
    {
        public AexCrossover(ConflictResolver resolverConflict, ConflictResolver resolverRandomized, Random random, bool mutateIfSame) : base(resolverConflict, resolverRandomized,  random, mutateIfSame)
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
                var indexOfCurrentVertexInSelectedParent = Array.IndexOf(selectedParent, currentVertex);

                if (indexOfCurrentVertexInSelectedParent < parentLength - 1 &&
                    !offspring.Contains(selectedParent[indexOfCurrentVertexInSelectedParent + 1]))
                {
                    nextVertex = selectedParent[indexOfCurrentVertexInSelectedParent + 1];
                    if (currentParentIndex >= parentsLength - 1)
                    {
                        currentParentIndex = 0;
                    }
                    else
                    {
                        currentParentIndex++;
                    }
                }

                _randomizationChances++;

                if (Random.NextDouble() < ResolverRandomized.RandomizationProbability)
                {
                    _randomizedResolvesCount++;
                    var resolverResult = ResolverRandomized.ResolveConflict(selectedParent[indexOfCurrentVertexInSelectedParent],
                        availableVertexes);
                    if (resolverResult != -1) nextVertex = resolverResult;

                }

                if (nextVertex == -1)
                {
                    nextVertex = ResolverConflict.ResolveConflict(selectedParent[indexOfCurrentVertexInSelectedParent], availableVertexes);
                    _resolveCount++;
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