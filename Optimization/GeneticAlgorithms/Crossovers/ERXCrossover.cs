using Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Optimization.GeneticAlgorithms.Crossovers
{

    internal class ERXCrossover : Crossover
    {
        private static int mod(int x, int m)
        {
            return (x % m + m) % m;
        }
        private static bool checkDuplicates(int val1, int[] val2,int iterator)
        {
            for (int j = 0; j<= iterator; j++) if (val1 == val2[j]) return false;
            return true;
        }
        private class Vector
        {
            public Vector(int val1, int val2)
            {
                Item1 = val1;
                Item2 = val2;
            }
            public int Item1 { get; set; }
            public int Item2 { get; set; }
        }
        public override int[] GenerateOffspring(int[][] parents)
        {
            var parentLength = parents[0].Length;
            var currentVertex = parents[0][0];
            var offspring = new int[parentLength];
            offspring[0] = currentVertex;
            var parentsList = new List<int[]>(parents);
            Random rand = new Random();
            var whichParent1 = rand.Next(0, parentsList.Count);
            var Parent1 = parentsList[whichParent1];
            parentsList.Remove(Parent1);
            var whichParent2 = rand.Next(0, parentsList.Count);
            var Parent2 = parentsList[whichParent2];



            offspring[0] = Parent1[0];

            List<Tuple<int, List<int>>> neighbourList = new List<Tuple<int, List<int>>>();
            for (int i = 0; i < parentLength; i++)
            {
                List<int> adjacencyList = new List<int>();
                adjacencyList.Add(Parent1[mod(i - 1, parentLength)]);
                adjacencyList.Add(Parent1[mod(i + 1, parentLength)]);
                for (int j = 0; j < parentLength; j++)
                {
                    if (Parent2[j] == Parent1[i])
                    {
                        adjacencyList.Add(Parent2[mod(j - 1, parentLength)]);
                        adjacencyList.Add(Parent2[mod(j + 1, parentLength)]);
                        break;
                    }
                }
                adjacencyList = adjacencyList.Distinct().ToList();
                neighbourList.Add(new Tuple<int, List<int>>(Parent1[i], adjacencyList));
            }
            for (int i = 0; i < parentLength-1; i++)
            {
                var querry = neighbourList.Where(x => x.Item1 == offspring[i]).First();
                Vector best = new Vector(-1, 5);
                foreach (var item in querry.Item2)
                {
                    foreach (var elem in neighbourList)
                    {
                        if (item == elem.Item1)
                        {
                            bool flag = checkDuplicates(elem.Item1, offspring, i);
                            if (flag)
                            {
                                if (best.Item2 > elem.Item2.Count)
                                {
                                    best.Item1 = elem.Item1;
                                    best.Item2 = elem.Item2.Count;
                                }
                                else if (best.Item2 == elem.Item2.Count)
                                {

                                    Random rnd = new Random();
                                    int randomisation = rnd.Next(2);
                                    if (randomisation == 0)
                                    {
                                        best.Item1 = elem.Item1;
                                        best.Item2 = elem.Item2.Count;
                                    }
                                }
                            }
                        }
                    }
                }
                if(best.Item1==-1)
                {
                    foreach (var item in neighbourList)
                    {
                        bool flag = checkDuplicates(item.Item1, offspring, i);
                        if (flag)
                        {
                            best.Item1 = item.Item1;
                            best.Item2 = item.Item2.Count;
                            offspring[i + 1] = best.Item1;
                            break;
                        }
                    }
                }
                else offspring[i + 1] = best.Item1;
            }
            return offspring;
        }

        public ERXCrossover(ConflictResolver resolverConflict, ConflictResolver resolverRandomized, Random random, bool mutateIfSame) : base(resolverConflict, resolverRandomized,  random, mutateIfSame)
        {
        }
    }
}
