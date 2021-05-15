using System;
using System.Collections.Generic;
using System.Linq;
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
            int pointCount = availableVertexes.Count;

            if (currentPoint == 0) return availableVertexes[0];
            var bestCandidate = availableVertexes[0];
            var bestFrequency = Orders.ProductsTogetherFrequency[currentPoint][bestCandidate];
            
            for (int i = 1; i < pointCount; i++)
            {
                var frequency = Orders.ProductsTogetherFrequency[currentPoint][availableVertexes[i]];
                if (frequency > bestFrequency)
                {
                    bestCandidate = availableVertexes[i];
                    bestFrequency = frequency;
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
