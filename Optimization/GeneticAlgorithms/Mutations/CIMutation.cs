using System;

namespace Optimization.GeneticAlgorithms.Mutations
{
    internal class CIMutation : Mutation
    {
        public CIMutation() : base()
        {
        }

        public override void Mutate(int[] chromosome)
        {
            var pointOfDivison = _random.Next(1, chromosome.Length);
            Array.Reverse(chromosome,1,pointOfDivison-1);
            Array.Reverse(chromosome,pointOfDivison-1,chromosome.Length-pointOfDivison-1);
        }
    }
}