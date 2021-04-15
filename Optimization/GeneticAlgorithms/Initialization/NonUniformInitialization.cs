using System;

namespace Optimization.GeneticAlgorithms.Initialization
{
    class NonUniformInitialization : PopulationInitialization
    {
        public override int[][] InitializePopulation(int[] pointsToInclude, int populationSize, int startingPoint)
        {
            return null;
        }

        public NonUniformInitialization(Random random) : base(random)
        {
        }
    }
}