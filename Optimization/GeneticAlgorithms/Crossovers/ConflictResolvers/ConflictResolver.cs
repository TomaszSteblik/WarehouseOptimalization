using System;
using System.Collections.Generic;
using System.Threading;

namespace Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers
{
    public enum ConflictResolveMethod
    {
        Random,
        NearestNeighbor
    }
    public abstract class ConflictResolver
    {
        public abstract int ResolveConflict(int currentPoint, List<int> availableVertexes);
        
    }
}