using System;
using System.Collections.Generic;

namespace Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers
{
    public class RandomResolver : ConflictResolver
    {
        public RandomResolver(Random random, double probability) : base(random, probability)
        {
        }
        
        public override int ResolveConflict(int currentPoint, List<int> availableVertexes)
        {
            return availableVertexes[Random.Next(0, availableVertexes.Count)];
        }
    }
}