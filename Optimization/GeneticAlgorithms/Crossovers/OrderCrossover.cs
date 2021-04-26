using System;
using System.Collections.Generic;
using System.Linq;
using Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers;

namespace Optimization.GeneticAlgorithms.Crossovers
{
    internal class OrderCrossover : Crossover
    {
        public override int[] GenerateOffspring(int[][] parents)
        {
            var parentLength = parents[0].Length;
            var offspring = new int[parentLength];


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
            while (num1 == num2 /*|| Math.Abs(num1-num2)==parentLength*/);
            int start = Math.Min(num1, num2);
            int stop = Math.Max(num1, num2);
            //start = 3;
            //stop = 7;
            Console.WriteLine(start + " " + stop);

            int cutSize = stop - start;
            var parent1CutPart = new int[cutSize];
            var parent2CutPart = new int[cutSize];
            int k = 0;
            for (int i = 0; i < Parent2.Length; i++)
            {
                offspring[i] = 0;
            }
            for (int i = start; i < stop; i++)
            {
                offspring[i] = Parent1[i];
                parent1CutPart[k] = Parent1[i];
                parent2CutPart[k] = Parent2[i];
                k++;
            }

            var parent2RemainingPart = new int[parentLength - cutSize];
            int parent2point = 0;
            for (int i = 0; i < parentLength - cutSize; i++)
            {
                if (parent2point == start)
                {
                    parent2point = stop;
                }
                parent2RemainingPart[i] = Parent2[parent2point];
                parent2point++;
            }


            bool flag = true;
            while (flag)
            {
                flag = offspring
                .Distinct()
                .Count() != offspring.Count();

                int geneIndex;
                for (int i = 0; i < parentLength - (cutSize); i++)
                {
                    geneIndex = (stop + i) % parentLength;
                    bool is_there = false;
                    offspring[geneIndex] = Parent2[geneIndex];
                    for (int j = 0; j < cutSize; j++)
                    {
                        if (offspring[geneIndex] == parent1CutPart[j])
                        {
                            is_there = true;
                            break;
                        }
                    }
                    if (!is_there)
                    {
                        offspring[geneIndex] = Parent2[geneIndex];
                    }
                    else
                    {
                        for (int j = 0; j < parentLength; j++)
                        {
                            int tempIndex = (geneIndex + j + 1) % parentLength;
                            bool check = false;

                            for (int l = 0; l < parent1CutPart.Length; l++)
                            {
                                if (Parent2[tempIndex] == parent1CutPart[l])
                                {
                                    check = true;
                                    break;
                                }
                                for (int m = 0; m < parentLength; m++)
                                {
                                    if (Parent2[tempIndex] == offspring[m])
                                    {
                                        check = true;
                                    }
                                }
                            }
                            if (!check)
                            {
                                offspring[geneIndex] = Parent2[tempIndex];
                                break;
                            }
                        }

                    }

                }
            }

            return offspring;
        }

        public OrderCrossover(ConflictResolver resolverConflict, ConflictResolver resolverRandomized, Random random, bool mutateIfSame) : base(resolverConflict, resolverRandomized,  random, mutateIfSame)
        {
        }
    }
}
