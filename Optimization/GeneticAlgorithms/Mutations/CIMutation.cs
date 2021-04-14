using System;

namespace Optimization.GeneticAlgorithms.Mutations
{
    internal class CIMutation : Mutation
    {
        public CIMutation(double mutationProbability, int[][] population) : base(mutationProbability, population)
        {
        }

        public override void Mutate(int[] chromosome)
        {
            var pointOfDivision = Random.Next(1, chromosome.Length);
            Array.Reverse(chromosome,1,pointOfDivision-1);
            Array.Reverse(chromosome,pointOfDivision-1,chromosome.Length-pointOfDivision-1);
        }
    }
}