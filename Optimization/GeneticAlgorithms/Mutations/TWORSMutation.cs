using System;

namespace Optimization.GeneticAlgorithms.Mutations
{
    internal class TWORSMutation : Mutation
    {
        public TWORSMutation(int[][] population, double mutationProbability) : base(population, mutationProbability)
        {
        }

        public override void Mutate()
        {
            if (_probability > 0d)
            {
                for (int m = (int)(0.1 * _population.Length); m < _population.Length; m++)
                {
                    if (_random.Next(0, 1000) <= _probability)
                    {
                        var pointA = _random.Next(1, _population[m].Length);
                        var pointB = _random.Next(1, _population[m].Length);
                        var temp = _population[m][pointB];
                        _population[m][pointB] = _population[m][pointA];
                        _population[m][pointA] = temp;
                    }
                }
            }
        }
    }
}