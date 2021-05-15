using System;
using System.Collections.Generic;
using System.Linq;
using Optimization.GeneticAppliances.Warehouse;


namespace Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers
{
    class WarehousePairwiseProductFrequencyTournamentResolver : ConflictResolver
    {
        public WarehousePairwiseProductFrequencyTournamentResolver(Random random, double probability) : base(random, probability)
        {
        }

        public override int ResolveConflict(int currentPoint, List<int> availableVertexes)
        {
            int pointCount = availableVertexes.Count;
            int numberOfCandidates = pointCount > 16 ? 8 : pointCount / 2;
            if (pointCount == 1) numberOfCandidates = 1;

            var candidates = availableVertexes.OrderBy(x => Random.Next()).Take(numberOfCandidates);

            if (currentPoint == 0) return candidates.ToArray()[0];
            
            return candidates.OrderByDescending(x => Orders.ProductsTogetherFrequency[currentPoint][x]).ToArray()[0];

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