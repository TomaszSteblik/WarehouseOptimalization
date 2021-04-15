using System.Collections.Generic;
using System.Linq;
using Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers;

namespace Optimization.GeneticAlgorithms.Crossovers
{
    internal class CycleCrossover : Crossover
    {
        public override int[] GenerateOffspring(int[][] parents)
        {
            var parentLength = parents[0].Length;
            var currentVertex = parents[0][0];
            var offspring = new int[parentLength];
            offspring[0] = currentVertex;
            //var availableVertexes = new List<int>(parents[0]);
            //availableVertexes.Remove(currentVertex);
            var counter = 1;


            int[] filledCheck = new int[parentLength];
            filledCheck[0] = 1;
            for (int i = 1; i < filledCheck.Length; i++)
            {
                filledCheck[i] = 0;
            }
            int parent2Value;
            while (counter < parentLength)
            {
                var parentsList = new List<int[]>(parents);

                var whichParent1 = Random.Next(0, parentsList.Count);
                var selectedParent1 = parentsList[whichParent1];
                parentsList.Remove(selectedParent1);
                var whichParent2 = Random.Next(0, parentsList.Count);
                var selectedParent2 = parentsList[whichParent2];

                if (filledCheck[currentVertex] != 0)
                {
                    for (int j = 0; j < selectedParent1.Length; j++)
                    {
                        if (filledCheck[j] == 0)
                        {
                            offspring[j] = selectedParent2[j];
                        }
                    }
                    break;
                }
                

                offspring[currentVertex] = selectedParent1[currentVertex];
                parent2Value = selectedParent2[currentVertex];
                filledCheck[currentVertex] = 1;
                for (int j = 0; j <= selectedParent1.Length; j++)
                {
                    if (selectedParent1[j] == parent2Value)
                    {
                        currentVertex = selectedParent1[j];
                    }
                }
                counter++;
            }

            return offspring;
        }

        public CycleCrossover(ConflictResolver resolver) : base(resolver)
        {
        }
    }
}
