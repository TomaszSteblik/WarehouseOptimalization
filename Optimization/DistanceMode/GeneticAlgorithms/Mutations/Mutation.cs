using System;

namespace Optimization.DistanceMode.GeneticAlgorithms.Mutations
{
    public abstract class Mutation
    {
        protected int[][] _population;
        protected readonly Random _random;
        protected double _probability;
        protected Mutation(int[][] population, double mutationProbability)
        {
            _population = population;
            _probability = mutationProbability;
            _random = new Random();
        }

        public abstract void Mutate();
    }
}