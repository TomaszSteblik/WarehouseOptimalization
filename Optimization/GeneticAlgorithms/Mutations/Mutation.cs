using System;

namespace Optimization.GeneticAlgorithms.Mutations
{
    internal abstract class Mutation
    {
        protected readonly Random _random;
        protected Mutation()
        {
            _random = new Random();
        }

        public abstract void Mutate(int[] chromosome);
    }

    public enum MutationMethod
    {
        CIM,
        RSM,
        THROAS,
        THRORS,
        TWORS
    }
}