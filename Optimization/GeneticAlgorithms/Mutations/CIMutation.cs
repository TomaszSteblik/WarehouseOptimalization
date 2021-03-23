using System;

namespace Optimization.GeneticAlgorithms.Mutations
{
    internal class CIMutation : Mutation
    {
        public CIMutation(int[][] population, double mutationProbability) : base(population, mutationProbability)
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
                        var pointOfDivison = _random.Next(1, _population[m].Length);
                        Array.Reverse(_population[m],1,pointOfDivison-1);
                        Array.Reverse(_population[m],pointOfDivison-1,_population[m].Length-pointOfDivison-1);
                    }
                }
            }
        }
    }
}