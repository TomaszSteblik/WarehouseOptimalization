using System;

namespace Optimization.GeneticAlgorithms.Selections {
    internal abstract class Selection
    {
        protected readonly int[][] Population;
        protected readonly int PopulationSize;
        protected readonly Random Random;
        protected int Strictness = 1;

        public Selection(int[][] population, Random random)
        {
            Population = population;
            PopulationSize = population.Length;
            Random = random;
        }
        public bool IncreaseStrictness(int numberOfChildren)
        {
            if (numberOfChildren*Math.Pow(2, Strictness + 1) <= PopulationSize)
            {
                Strictness++;
                return true;
            }
            return false;
        }

        public abstract int[][] GenerateParents(int numberOfParents, double[] fitness);
    }

    public enum SelectionMethod
    {
        Elitism,
        Random,
        RouletteWheel,
        Tournament
    }
}