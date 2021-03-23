using System.Collections.Generic;
using System.Linq;

namespace Optimization.GeneticAlgorithms.Mutations
{
    internal class THRORSMutation : Mutation
    {
        public THRORSMutation(int[][] population, double mutationProbability) : base(population, mutationProbability)
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
                        var range = Enumerable.Range(1, _population[m].Length-1).ToList();
                        var pointA = range.ElementAt(_random.Next(0, range.Count));
                        range.Remove(pointA);
                        var pointB = range.ElementAt(_random.Next(0, range.Count));
                        range.Remove(pointB);
                        var pointC = range.ElementAt(_random.Next(0, _population[m].Length - range.Count));

                        var valueA = _population[m][pointA];
                        var valueB = _population[m][pointB];
                        var valueC = _population[m][pointC];

                        _population[m][pointB] = valueA;
                        _population[m][pointC] = valueB;
                        _population[m][pointA] = valueC;

                    }
                }
            }
        }
    }
}