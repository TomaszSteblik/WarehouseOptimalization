using System;
using System.Collections.Generic;
using System.Linq;
using Optimization.GeneticAppliances.Warehouse;
using Optimization.Helpers;


namespace Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers
{
    class WarehouseSingleProductFrequencyTournamentResolver : ConflictResolver
    {
        private double[][] _distancesMatrix;
        private int[] _warehousePointsByLocation;
        private int[] _productsByFrequency;
        public WarehouseSingleProductFrequencyTournamentResolver(Random random, double probability) : base(random, probability)
        {
            _distancesMatrix = Distances.GetInstance().DistancesMatrix;
            _warehousePointsByLocation = Enumerable.Range(0, _distancesMatrix.Length)
                .OrderBy(x => _distancesMatrix[0][x]).ToArray();
            _productsByFrequency = Enumerable.Range(0, _distancesMatrix.Length)
                .OrderByDescending(x => Orders.ProductFrequency[x]).ToArray();
        }

        public override int ResolveConflict(int currentPoint, List<int> availableVertexes)
        {
            int pointCount = availableVertexes.Count;
            int numCandidates = 1+(int)(0.25 * pointCount);

            var candidates = new int[numCandidates];
            for (int i = 0; i < numCandidates; i++)
            {
                candidates[i] = availableVertexes[Random.Next(0, pointCount)];
            }

            var bestCandidate = candidates[0];
            var currentLocationInChromosome = Orders.ProductFrequency.Length - pointCount - 1;
            var locationIndex = Array.IndexOf(_warehousePointsByLocation, currentLocationInChromosome);
            var bestFit =
                Math.Abs(Array.IndexOf(_productsByFrequency, candidates[0]) - locationIndex);

            for (int i = 0; i < candidates.Length; i++)
            {
                var candidateIndex = Array.IndexOf(_productsByFrequency, candidates[i]);
                var currentFit = Math.Abs(candidateIndex - locationIndex);
                if (currentFit < bestFit)
                {
                    bestCandidate = candidates[i];
                    bestFit = currentFit;
                }
            }

            return bestCandidate;
            
/*
            double maxProductFrequency = -1;
            int bestCandidate = -1;
            //proportional to the distance matrix distance
            for (int k = 0; k < numCandidates; k++)
            {
                int candidate = (int) ( numCandidates*(0.5-Random.Next(1, cnt)));
                if (Orders.ProductFrequencies[candidate] > maxProductFrequency)
                {
                    int f = currentPoint + candidate;
                    if (currentPoint + candidate > Orders.ProductFrequencies.Count)
                        f = Orders.ProductFrequencies[Orders.ProductFrequencies.Count];

                    maxProductFrequency = Orders.ProductFrequencies[f];
                    bestCandidate = availableVertexes[candidate];
                }
            }
*/
            //return default; //bestCandidate;
        }
    }
    
}