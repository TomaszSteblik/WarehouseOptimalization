using System;

namespace Optimization.GeneticAlgorithms.Mutations
{
    internal class RSMutation : Mutation
    {
        public RSMutation() : base()
        {
        }

        public override void Mutate(int[] chromosome)
        {
            var j = _random.Next(1, chromosome.Length);
            var i = _random.Next(1, j);
            Array.Reverse(chromosome, i, j - i);
        }
    }
}