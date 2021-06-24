using System;
using System.Collections.Generic;
using System.Linq;
using Optimization.GeneticAppliances.Warehouse;
using Optimization.Helpers;


namespace Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers
{
    class WarehousePairwiseProductFrequencyResolver : ConflictResolver
    {
        private int _participantsCount;
        private double[][] _distancesMatrix;
        private int[] _warehousePointsByLocation;
        private int[][] _productsByPairwiseFrequency;
        public WarehousePairwiseProductFrequencyResolver(Random random, double probability, int participantsCount) : base(random, probability)
        {
            _participantsCount = participantsCount;

            _distancesMatrix = Distances.GetInstance().DistancesMatrix;
            _warehousePointsByLocation = Enumerable.Range(0, _distancesMatrix.Length)
                .OrderBy(x => _distancesMatrix[0][x]).ToArray();
        }

        public override int ResolveConflict(int currentPoint, List<int> availableVertexes)
        {
            int pointCount = availableVertexes.Count;
            int numberOfCandidates = _participantsCount < pointCount ? _participantsCount : pointCount;
            if (_participantsCount == 0) numberOfCandidates = pointCount;

            var indexOfCurrent = Array.IndexOf(_warehousePointsByLocation, currentPoint);

            var candidates = new int[numberOfCandidates];
            for (int i = 0; i < numberOfCandidates; i++)
            {
                candidates[i] = availableVertexes[Random.Next(0, pointCount)];
            }


            if (currentPoint == 0) return availableVertexes[0];
            var bestCandidate = availableVertexes[0];
            var bestWeight = Orders.ProductsTogetherFrequency[currentPoint][bestCandidate] * (1 / _distancesMatrix[currentPoint][bestCandidate]);

            if(indexOfCurrent + 1 != _warehousePointsByLocation.Length)
                if (_warehousePointsByLocation[indexOfCurrent] != _warehousePointsByLocation[indexOfCurrent + 1] - 1)
                {
                    return availableVertexes[0];
                }
            
            for (int i = 1; i < numberOfCandidates; i++)
            {
                var currentWeight = Orders.ProductsTogetherFrequency[currentPoint][candidates[i]] * (1 / _distancesMatrix[currentPoint][candidates[i]]);
                if (currentWeight > bestWeight)
                {
                    bestCandidate = candidates[i];
                    bestWeight = currentWeight;
                }
            }

            return bestCandidate;

            // double maxProductFrequency = -1;
            // int bestCandidate = -1;
            //
            // for (int k = 0; k < numCandidates; k++)
            // {
            //     int candidate = Random.Next(1, cnt);
            //     if (Orders.ProductFrequency[candidate] > maxProductFrequency)
            //     {
            //         maxProductFrequency = Orders.ProductFrequency[candidate];
            //         bestCandidate = availableVertexes[candidate];
            //     }
            // }
            //
            // return bestCandidate;
        }
    }
}
