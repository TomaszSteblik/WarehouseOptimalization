using System;
using System.Collections.Generic;
using System.Linq;

namespace Optimization.GeneticAlgorithms.Crossovers
{
    internal class CycleCrossover2 : Crossover
    {
        public int[][] CX2OffspringGenerator(int[][] parents)
        {
            var parentLength = parents[0].Length;
            var currentVertex = parents[0][0];
            var offspring1 = new int[parentLength];
            var offspring2 = new int[parentLength];
            offspring1[0] = currentVertex;
            offspring2[0] = currentVertex;

            var counter = 1;

            while (counter < parentLength)
            {
                var parentsList = new List<int[]>(parents);

                var whichParent1 = Random.Next(0, parentsList.Count);
                var selectedParent1 = parentsList[whichParent1];
                parentsList.Remove(selectedParent1);
                var whichParent2 = Random.Next(0, parentsList.Count);
                var selectedParent2 = parentsList[whichParent2];

                int st1 = 0;
                int st2 = 0;
                offspring1[st1] = selectedParent2[st1];
                while (st1 < parentLength)
                {
                    int ind1 = 0;
                    for (int i = 0; i <= selectedParent1.Length; i++)
                    {
                        if (selectedParent1[i] == offspring1[st1])
                            ind1 = selectedParent1[i];
                    }
                    int val1 = selectedParent2[ind1];
                    offspring2[st2] = val1;
                    st1++;
                    int ind2 = 0;
                    for (int i = 0; i <= selectedParent2.Length; i++)
                    {
                        if (selectedParent2[i] == offspring2[st2])
                            ind2 = selectedParent1[i];
                    }
                    int val2 = selectedParent1[ind2];
                    offspring1[st1] = val2;
                    st2++;
                }

                counter++;
            }
            List<int[]> OffspringResult = new List<int[]>
            {
                offspring1,
                offspring2
            };
            return OffspringResult.ToArray();
        }

        public override int[] GenerateOffspring(int[][] parents)
        {
            return CX2OffspringGenerator(parents).First();
        }
        public override int[][] GenerateOffsprings(int[][] parents, int numParentsForOneChild)
        {
            return CX2OffspringGenerator(parents);
        }
    }
}
