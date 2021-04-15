using System;

namespace Optimization.GeneticAlgorithms.Mutations
{
    internal class THROASMutation : Mutation
    {
        public THROASMutation(double mutationProbability, int[][] population, Random random) : base(mutationProbability, population, random)
        {
        }

        public override void Mutate(int[] chromosome)
        {
            var pointA = Random.Next(1, chromosome.Length-2);
            var pointB = Random.Next(pointA+1, chromosome.Length-1);
            var pointC = Random.Next(pointB+1, chromosome.Length);

            var valueA = chromosome[pointA];
            var valueB = chromosome[pointB];
            var valueC = chromosome[pointC];

            chromosome[pointA] = valueC;
            chromosome[pointC] = valueB;
            chromosome[pointB] = valueA;
        }
    }
}