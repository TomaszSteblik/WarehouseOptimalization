using System.Collections.Generic;
using System.Linq;

namespace Optimization.GeneticAlgorithms.Mutations
{
    internal class THRORSMutation : Mutation
    {
        public THRORSMutation(double mutationProbability, int[][] population) : base(mutationProbability, population)
        {
        }

        public override void Mutate(int[] chromosome)
        {
            var range = Enumerable.Range(1, chromosome.Length-1).ToList();
            var pointA = range.ElementAt(Random.Next(0, range.Count));
            range.Remove(pointA);
            var pointB = range.ElementAt(Random.Next(0, range.Count));
            range.Remove(pointB);
            var pointC = range.ElementAt(Random.Next(0, chromosome.Length - range.Count));

            var valueA = chromosome[pointA];
            var valueB = chromosome[pointB];
            var valueC = chromosome[pointC];

            chromosome[pointB] = valueA;
            chromosome[pointC] = valueB;
            chromosome[pointA] = valueC;
        }
    }
}