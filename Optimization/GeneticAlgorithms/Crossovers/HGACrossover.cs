using System;
using System.Collections.Generic;
using System.Linq;
using Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers;
using Optimization.Helpers;

namespace Optimization.GeneticAlgorithms.Crossovers
{
    internal class HGACrossover : Crossover
    {
        private double[][] DistancesMatrix { get; }
        public HGACrossover(ConflictResolver resolverConflict, ConflictResolver resolverRandomized, Random random, bool mutateIfSame) : base(resolverConflict, resolverRandomized, random, mutateIfSame)
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
                //for (var i = 0; i < parents.Length; i++)
                //{
                //    var selectedParent = parents[i];
                //    var indexOfCurrentVertexInSelectedParent = Array.IndexOf(selectedParent, currentVertex);
                //    if (indexOfCurrentVertexInSelectedParent >= parentLength - 1 ||
                //        offspring.Contains(selectedParent[indexOfCurrentVertexInSelectedParent + 1]))
                //    {
                //        feasibleParents.Remove(selectedParent);
                //    }
                //}


                //var min = double.MaxValue;
                //int minIndex = -1;

                //for (int i = 0; i < feasibleParents.Count; i++)
                //{
                //    if (DistancesMatrix[currentVertex][
                //        feasibleParents[i][Array.IndexOf(feasibleParents[i], currentVertex) + 1]] < min)
                //    {
                //        min = DistancesMatrix[currentVertex][
                //            feasibleParents[i][Array.IndexOf(feasibleParents[i], currentVertex) + 1]];
                //        minIndex = i;
                //    }
                //}


                Dictionary<int, double> fitness = new Dictionary<int, double>();

                for (int i = 0; i < feasibleParents.Count; i++)
                {
                    if (Array.IndexOf(feasibleParents[i], currentVertex) < (parentLength - 1))
                    {
                        if (!fitness.ContainsKey(feasibleParents[i][Array.IndexOf(feasibleParents[i], currentVertex) + 1]))
                        {
                            fitness.Add(feasibleParents[i][Array.IndexOf(feasibleParents[i], currentVertex) + 1],
                                   DistancesMatrix[currentVertex][feasibleParents[i][Array.IndexOf
                                   (feasibleParents[i], currentVertex) + 1]]);
                        }
                    }
                    if (Array.IndexOf(feasibleParents[i], currentVertex) != 0)
                    {
                        if (!fitness.ContainsKey(feasibleParents[i][Array.IndexOf(feasibleParents[i], currentVertex) - 1]))
                        {
                            fitness.Add(feasibleParents[i][Array.IndexOf(feasibleParents[i], currentVertex) - 1],
                                        DistancesMatrix[currentVertex][feasibleParents[i][Array.IndexOf
                                        (feasibleParents[i], currentVertex) - 1]]);
                        }
                    }


                }
                foreach (var elem in fitness)
                {
                    if (offspring.ToList().Contains(elem.Key))
                    {
                        fitness.Remove(elem.Key);
                    }
                }
                if (fitness.Count() == 0)
                {
                    bool flag = false;
                    while (flag = false)
                    {
                        var rand = new Random();
                        int randomParent = rand.Next(parents.Count());
                        int randomElem = rand.Next(parentLength);
                        flag = true;
                        for (int i = 0; i < parentLength; i++)
                        {
                            if (offspring[i] == parents[randomParent][randomElem])
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                        {
                            nextVertex = parents[randomParent][randomElem];
                        }
                    }
                }
                else
                {
                    int minimalValue = fitness.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
                    nextVertex = minimalValue;
                }
                


                _randomizationChances++;
                if (Random.NextDouble() < ResolverRandomized.RandomizationProbability)
                {
                    _randomizedResolvesCount++;
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