using System;
using System.Collections.Generic;

namespace Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers
{
    public class RandomResolve : ConflictResolver
    {
        public RandomResolve(Random random) : base(random)
        {
        }
        
        public override int ResolveConflict(int currentPoint, List<int> availableVertexes)
        {
            return availableVertexes[Random.Next(0, availableVertexes.Count)];
        }
    }
}