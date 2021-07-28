﻿using System;
using System.Linq;
using Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers;

namespace Optimization.GeneticAlgorithms.Crossovers
{
    internal class KPointCrossover : Crossover
    {
        public override int[] GenerateOffspring(int[][] parents)
        {
            var geneLength = parents[0].Length;
            var parentsNumber = parents.Length;
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
                        _resolveCount++;
                        selected = ResolverConflict.ResolveConflict(offspring[j - 1], available);
                    }

                    available.Remove(selected);
                    offspring[iterator++] = selected;
                }
                else
                {
                    var selected = parents[j % parentsNumber][j];
                    if (Random.NextDouble() < ResolverRandomized.RandomizationProbability)
                    {
                        var resolverResult = ResolverRandomized.ResolveConflict(offspring[j - 1], available);
                        if (resolverResult != -1) selected = resolverResult;
                    }
                    available.Remove(selected);
                    offspring[iterator++] = selected;
                }
            }

            return offspring;
        }

        public KPointCrossover(ConflictResolver resolverConflict, ConflictResolver resolverRandomized, Random random, bool mutateIfSame) : base(resolverConflict, resolverRandomized,  random, mutateIfSame)
        {
        }
    }
}