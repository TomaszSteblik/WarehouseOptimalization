using Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Optimization.GeneticAlgorithms.Crossovers
{
    internal class PMXCrossover : Crossover
    {
        public override int[] GenerateOffspring(int[][] parents)
        {
            var parentLength = parents[0].Length;
            var offspring = new int[parentLength];
            var counter = 1;

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
                while (num1 == num2 || Math.Abs(num1 - num2) == parentLength);
                int start = Math.Min(num1, num2);
                int stop = Math.Max(num1, num2);

                //offspring = Parent2;
                //int cutSize = stop - start;
                //var parent1CutPart = new int[cutSize];
                //var parent2CutPart = new int[cutSize];
                //int k = 0;
                //for (int i = start; i < stop; i++)
                //{
                //    parent1CutPart[k] = Parent1[i];
                //    parent2CutPart[k] = Parent2[i];
                //    k++;
                //}

                //for (int i = 0; i < cutSize; i++)
                //{
                //    int position = i + start;
                //    for (int j = 0; j < start; j++)
                //    {
                //        if (offspring[j] == parent1CutPart[i])
                //        {
                //            offspring[j] = offspring[position];
                //            break;
                //        }
                //    }


                //    for (int j = stop; j < parentLength; j++)
                //    {
                //        if (offspring[j] == parent1CutPart[i])
                //        {
                //            offspring[j] = offspring[position];
                //            break;
                //        }
                //    }
                //    offspring[position] = parent1CutPart[i];
                //}

                int cutSize = stop - start;
                var parent1CutPart = new int[cutSize];
                var parent2CutPart = new int[cutSize];
                int k = 0;
                for (int i = 0; i < Parent2.Length; i++)
                {
                    offspring[i] = Parent2[i];
                }
                for (int i = start; i < stop; i++)
                {
                    offspring[i] = Parent1[i];


                    parent1CutPart[k] = Parent1[i];
                    parent2CutPart[k] = Parent2[i];
                    k++;

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

                    for (int j = 0; j < cutSize; j++)
                    {
                        if (parent1CutPart[j] == offspring[geneIndex])
                        {
                            offspring[geneIndex] = parent2CutPart[j];
                        }
                    }

                }
            }


            return offspring;
        }

        public PMXCrossover(ConflictResolver resolver,ConflictResolver resolverRandomized, Random random, bool mutateIfSame) : base(resolver, resolverRandomized,  random, mutateIfSame)
        {
        }
    }
}
