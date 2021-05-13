using System;
using System.Collections.Generic;
using Optimization.Helpers;
using Optimization.PathFinding;

namespace Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers
{
    public class NearestNeighborResolver : ConflictResolver
    {
        private double[][] _distancesMatrix;
        public NearestNeighborResolver(Random random, double probability) : base(random, probability)
        {
            _distancesMatrix = Distances.GetInstance().DistancesMatrix;
        }

        public override int ResolveConflict(int currentPoint, List<int> availableVertexes)
        {
            return NearestNeighbor.FindNearestNeighbor(currentPoint, _distancesMatrix, availableVertexes);
        }
    }
}