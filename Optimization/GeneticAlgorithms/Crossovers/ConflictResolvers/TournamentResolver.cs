using System;
using System.Collections.Generic;
using Optimization.Helpers;
using Optimization.PathFinding;

namespace Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers
{
    class TournamentResolver : ConflictResolver
    {
        private double[][] distanceMatrix;
        public TournamentResolver(Random random, double randomizationProbability) : base(random, randomizationProbability)
        {
            distanceMatrix = Distances.GetInstance().DistancesMatrix;
        }

        public override int ResolveConflict(int currentPoint, List<int> availableVertexes)
        {

            int pointCount = availableVertexes.Count;
            int numCandidates = 1+(int)(0.25 * pointCount);
            double minDistance = Double.MaxValue;
            int bestCandidate = -1;


            for (int k = 0; k < numCandidates; k++)
            {
                int candidate = Random.Next(0, pointCount);
                if (distanceMatrix[currentPoint][availableVertexes[candidate]] < minDistance)
                {
                    minDistance = distanceMatrix[currentPoint][availableVertexes[candidate]];
                    bestCandidate = availableVertexes[candidate];
                }
            }

          
            return bestCandidate;
        }


    }
}