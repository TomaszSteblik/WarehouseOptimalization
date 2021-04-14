using System;
using System.Linq;

namespace Optimization.GeneticAlgorithms.Initialization
{
    class StandardPathInitialization : PopulationInitialization
    {
        public override int[][] InitializePopulation(int[] pointsToInclude, int populationSize, int startingPoint)
        {
            var population = new int[populationSize][];
            for (int i = 0; i < populationSize; i++)
            {
                var availablePoints = pointsToInclude.Except(new [] {startingPoint});
                var unit = new[] {startingPoint};
                population[i] = unit.Concat(availablePoints.OrderBy(x => Guid.NewGuid())).ToArray();
            }

            return population;

        }

        public StandardPathInitialization(Random random) : base(random)
        {
        }
    }
}