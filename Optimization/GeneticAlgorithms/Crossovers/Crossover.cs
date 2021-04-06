using System;

namespace Optimization.GeneticAlgorithms.Crossovers
{
    public abstract class Crossover
    {
        public abstract int[] GenerateOffspring(int[][] parents);
        public virtual int[][] GenerateOffsprings(int[][] parents, int numParentsForOneChild)
        {
            var parentsLength = parents.Length;
            var amountOfChildren = parentsLength / 2;
            int[][] offsprings = new int[amountOfChildren][];

            for (int c = 0; c < amountOfChildren; c++)
            {
                int[][] prnt = new int[numParentsForOneChild][];
                for (int i = 0; i < numParentsForOneChild; i++)
                {
                    prnt[i] = parents[Random.Next(parents.Length)];
                }

                offsprings[c] = GenerateOffspring(prnt);
            }

            return offsprings;
        }
        protected readonly Random Random = new Random();

        protected bool IsThereGene(int[] chromosome, int a)
        {
            foreach (var t in chromosome)
            {
                if (t == a) return true;
            }
            return false;
        }
        public enum CrossoverType
        {
            Aex,
            KPoint,
            HGreX,
            MPHGreX,
            HRndX,
            MPHRndX,
            HProX,
            MPHProX
        }

    }
}