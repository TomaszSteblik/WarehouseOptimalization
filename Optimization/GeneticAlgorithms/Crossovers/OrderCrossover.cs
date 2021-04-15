using System;
using System.Collections.Generic;
using Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers;

namespace Optimization.GeneticAlgorithms.Crossovers
{
    internal class OrderCrossover : Crossover
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

                int num1 = 0;
                int num2 = 0;
                do
                {
                    num1 = Random.Next(parentLength);
                    num2 = Random.Next(parentLength);
                }
                while (num1 == num2);
                int start = Math.Min(num1, num2);
                int stop = Math.Max(num1, num2);

                for (int i = start; i < stop; i++)
                {
                    offspring[i] = Parent1[i];
                }

                int geneIndex = 0;
                int geneInParent1 = 0;
                int geneInParent2 = 0;
                for (int i = 0; i < parentLength-(stop-start); i++)
                {
                    geneIndex = (stop + i) % parentLength;
                    geneInParent1 = Parent1[geneIndex];
                    geneInParent2 = Parent2[geneIndex];
                    bool check = false;
                    for (int j = 0; j < parentLength; j++)
                    {
                        if (offspring[j] == geneInParent2)
                        {
                            check = true;
                        }
                    }
                    if (!check)
                    {
                        offspring[geneIndex] = geneInParent2;
                    }
                }
                counter++;
            }

            return offspring;
        }

        public OrderCrossover(ConflictResolver resolver, Random random) : base(resolver, random)
        {
        }
    }
}
