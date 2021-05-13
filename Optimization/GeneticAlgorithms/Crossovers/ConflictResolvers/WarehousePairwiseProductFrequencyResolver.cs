using System;
using System.Collections.Generic;
using Optimization.GeneticAppliances.Warehouse;


namespace Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers
{
    class WarehousePairwiseProductFrequencyResolver : ConflictResolver
    {
        public WarehousePairwiseProductFrequencyResolver(Random random, double probability) : base(random, probability)
        {
        }

        public override int ResolveConflict(int currentPoint, List<int> availableVertexes)
        {
            int cnt = availableVertexes.Count;
            int numCandidates = cnt / 2;
            if (cnt > 16)
                numCandidates = 8;

            double maxProductFrequency = -1;
            int bestCandidate = -1;

            for (int k = 0; k < numCandidates; k++)
            {
                int candidate = Random.Next(1, cnt);
                if (Orders.ProductFrequencies[candidate] > maxProductFrequency)
                {
                    maxProductFrequency = Orders.ProductFrequencies[candidate];
                    bestCandidate = availableVertexes[candidate];
                }
            }

            return bestCandidate;
        }
    }
}
