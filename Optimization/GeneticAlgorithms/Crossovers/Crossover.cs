using System;
using System.Linq;

namespace Optimization.GeneticAlgorithms.Crossovers
{
    public abstract class Crossover
    {
        //protected abstract int[] GenerateOffspring(int[] parent1, int[] parent2);
        public abstract int[][] GenerateOffsprings(int[][] parents, int numParentsForOneChild = 2);
        protected readonly Random Random = new Random();
        protected double[][] DistancesMatrix;
        protected int _startingPoint = 0;

        protected bool IsThereGene(int[] chromosome, int a)
        {
            return chromosome.Any(t => t == a);
        }

        protected Crossover(double[][] distancesMatrix, int startingPoint)
        {
            DistancesMatrix = distancesMatrix;
            _startingPoint = startingPoint;
        }

    }
}