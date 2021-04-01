using System;

namespace Optimization.GeneticAlgorithms.Mutations
{
    internal abstract class Mutation
    {
        protected readonly Random Random;
        private double _mutationProbability;
        private int[][] _population;

        protected Mutation(double mutationProbability, int[][] population)
        {
            _mutationProbability = mutationProbability;
            _population = population;
            Random = new Random();
        }
        
        public void Mutate(int[][] population)
        {
            if (_mutationProbability > 0d)
            {
                for (int m = (int) (0.1 * _population.Length); m < _population.Length; m++)
                {
                    if (Random.Next(0, 1000) <= _mutationProbability)
                    {
                        Mutate(_population[m]);
                    }
                }
            }
        }
        protected abstract void Mutate(int[] chromosome);

    }

    public enum MutationMethod
    {
        CIM,
        RSM,
        THROAS,
        THRORS,
        TWORS,
        MRPM,
        MEPM
    }
}