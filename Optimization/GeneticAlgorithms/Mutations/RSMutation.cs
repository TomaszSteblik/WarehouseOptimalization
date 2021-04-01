using System;

namespace Optimization.GeneticAlgorithms.Mutations
{
    internal class RSMutation : Mutation
    {
        public RSMutation(double mutationProbability, int[][] population) : base(mutationProbability, population)
        {
        }

        protected override void Mutate(int[] chromosome)
        {
            var j = Random.Next(1, chromosome.Length);
            var i = Random.Next(1, j);
            Array.Reverse(chromosome, i, j - i);
        }
    }
}