using System;
using System.Collections.Generic;

namespace Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers
{
    public class RandomResolve : ConflictResolver
    {
        private Random rnd;
        public RandomResolve(Random random) : base(random)
        {
            rnd = new Random();
        }
        
        public override int ResolveConflict(int currentPoint, List<int> availableVertexes)
        {
            return availableVertexes[rnd.Next(0, availableVertexes.Count)];
        }
    }
}