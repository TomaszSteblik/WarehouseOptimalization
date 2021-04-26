using System;
using System.Collections.Generic;
using Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers;

namespace Optimization.GeneticAlgorithms.Crossovers
{
    class MACrossover : Crossover
    {
        private List<Crossover> _crossovers;
        private int _counter;

        public MACrossover(CrossoverMethod[] crossoverMethods, int startPoint, ConflictResolver resolverConflict, ConflictResolver resolverRandomized, Random random, bool mutateIfSame) : base(resolverConflict, resolverRandomized,  random, mutateIfSame)
        {
            _crossovers = new List<Crossover>();
            foreach (var method in crossoverMethods)
            {
                _crossovers.Add(GeneticFactory.CreateCrossover(startPoint, method, null, resolverConflict,resolverRandomized, random, mutateIfSame));
            }
        }
        public override int[] GenerateOffspring(int[][] parents)
        {
            if (_counter >= _crossovers.Count) _counter = 0;
            var offsping = _crossovers[_counter].GenerateOffspring(parents);
            _counter++;
            return offsping;
        }
    }
}