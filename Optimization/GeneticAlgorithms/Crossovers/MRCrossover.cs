using System.Collections.Generic;
using Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers;

namespace Optimization.GeneticAlgorithms.Crossovers
{
    class MRCrossover : Crossover
    {
        private readonly List<Crossover> _crossovers;
        public MRCrossover(CrossoverMethod[] crossoverMethods, int startPoint, ConflictResolver resolver) : base(resolver)
        {
            _crossovers = new List<Crossover>();
            foreach (var method in crossoverMethods)
            {
                _crossovers.Add(GeneticFactory.CreateCrossover(startPoint, method, null, resolver));
            }
        }

        public override int[] GenerateOffspring(int[][] parents)
        {
            return _crossovers[Random.Next(0,_crossovers.Count)].GenerateOffspring(parents);
        }
    }
}