using System;
using System.Collections.Generic;
using Optimization.GeneticAppliances.Warehouse;


namespace Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers
{
    class WarehouseSingleProductFrequencyResolver : ConflictResolver
    {
        public WarehouseSingleProductFrequencyResolver(Random random, double probability) : base(random, probability)
        {
        }

        public override int ResolveConflict(int currentPoint, List<int> availableVertexes)
        {
            int cnt = availableVertexes.Count;
            int numCandidates = cnt/2;
            if (cnt>16)
               numCandidates = 8;

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

            return bestCandidate;
        }
    }
    
}
