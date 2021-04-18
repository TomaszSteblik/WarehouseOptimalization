using System;
using System.Collections.Generic;
using System.Threading;

namespace Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers
{
    public enum ConflictResolveMethod
    {
        Random,
        NearestNeighbor,
        Tournament
    }
    public abstract class ConflictResolver
    {
        public abstract int ResolveConflict(int currentPoint, List<int> availableVertexes);
        
        public double RandomizationProbability { get; set; }
        protected Random Random;

        public ConflictResolver(Random random, double randomizationProbability)
        {
            Random = random;
            RandomizationProbability = randomizationProbability;
        }
        
    }
}