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
            int pointCount = availableVertexes.Count;
            double minDistance = Double.MaxValue;
            int bestCandidate = -1;


            for (int k = 0; k < pointCount; k++)
            {
                if (_distancesMatrix[currentPoint][availableVertexes[k]] < minDistance)
                {
                    minDistance = _distancesMatrix[currentPoint][availableVertexes[k]];
                    bestCandidate = availableVertexes[k];
                }
            }

            return bestCandidate;
        }
    }
}