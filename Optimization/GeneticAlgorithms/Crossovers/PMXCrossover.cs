using Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers;
using System;
using System.Collections.Generic;

namespace Optimization.GeneticAlgorithms.Crossovers
{
    internal class PMXCrossover : Crossover
    {
        public override int[] GenerateOffspring(int[][] parents)
        {
            var parentLength = parents[0].Length;
            var offspring = new int[parentLength];
            var counter = 1;

            while (counter < parentLength)
            {
                var parentsList = new List<int[]>(parents);

                var whichParent1 = Random.Next(0, parentsList.Count);
                var Parent1 = parentsList[whichParent1];
                parentsList.Remove(Parent1);
                var whichParent2 = Random.Next(0, parentsList.Count);
                var Parent2 = parentsList[whichParent2];

                int num1;
                int num2;
                do
                {
                    num1 = Random.Next(parentLength);
                    num2 = Random.Next(parentLength);
                }
                while (num1 == num2);
                int start = Math.Min(num1, num2);
                int stop = Math.Max(num1, num2);

                int cutSize = stop - start;
                var parent1CutPart = new int[cutSize];
                var parent2CutPart = new int[cutSize];
                for (int i = start; i < stop; i++)
                {
                    int j = 0;
                    offspring[i] = Parent1[i];
                    parent1CutPart[j] = Parent1[i];
                    parent2CutPart[j] = Parent2[i];
                    j++;
                }

                int geneIndex;
                for (int i = 0; i < parentLength - (stop - start); i++)
                {
                    geneIndex = (stop + i) % parentLength;
                    for(int j = 0; j < parent1CutPart.Length; j++)
                    {
                        if (parent1CutPart[j] == Parent2[geneIndex]) 
                        {
                            offspring[geneIndex] = parent2CutPart[j];
                        }
                        else
                        {
                            offspring[geneIndex] = Parent2[geneIndex];
                        }
                    }
                }
                counter++;
            }

            return offspring;
        }

        public PMXCrossover(ConflictResolver resolver,ConflictResolver resolverRandomized, Random random) : base(resolver,resolverRandomized, random)
        {
        }
    }
}
