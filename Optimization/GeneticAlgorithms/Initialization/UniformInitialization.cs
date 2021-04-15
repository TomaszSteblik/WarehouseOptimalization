using System;

namespace Optimization.GeneticAlgorithms.Initialization
{
    class UniformInitialization : PopulationInitialization
    {
        public override int[][] InitializePopulation(int[] pointsToInclude, int populationSize, int startingPoint)
        {
            return null;
        }

        public UniformInitialization(Random random) : base(random)
        {
        }
    }
}