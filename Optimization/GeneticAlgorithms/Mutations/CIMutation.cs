using System;

namespace Optimization.GeneticAlgorithms.Mutations
{
    internal class CIMutation : Mutation
    {
        public CIMutation(double mutationProbability, int[][] population) : base(mutationProbability, population)
        {
        }

        protected override void Mutate(int[] chromosome)
        {
            var pointOfDivison = Random.Next(1, chromosome.Length);
            Array.Reverse(chromosome,1,pointOfDivison-1);
            Array.Reverse(chromosome,pointOfDivison-1,chromosome.Length-pointOfDivison-1);
        }
    }
}