using System;

namespace Optimization.GeneticAlgorithms.Mutations
{
    internal class TWORSMutation : Mutation
    {
        public TWORSMutation() : base()
        {
        }

        public override void Mutate(int[] chromosome)
        {
            var pointA = _random.Next(1, chromosome.Length);
            var pointB = _random.Next(1, chromosome.Length);
            var temp = chromosome[pointB];
            chromosome[pointB] = chromosome[pointA];
            chromosome[pointA] = temp;
        }
    }
}