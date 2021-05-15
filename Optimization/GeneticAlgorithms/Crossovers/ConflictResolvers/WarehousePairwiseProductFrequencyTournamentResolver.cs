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
            int numberOfCandidates =1+(int)(0.25 * pointCount);

            var candidates = new int[numberOfCandidates];
            for (int i = 0; i < numberOfCandidates; i++)
            {
                candidates[i] = availableVertexes[Random.Next(0, pointCount)];
            }


            if (currentPoint == 0) return availableVertexes[0];
            var bestCandidate = availableVertexes[0];
            var bestFrequency = Orders.ProductsTogetherFrequency[currentPoint][bestCandidate];
            
            for (int i = 1; i < numberOfCandidates; i++)
            {
                var frequency = Orders.ProductsTogetherFrequency[currentPoint][candidates[i]];
                if (frequency > bestFrequency)
                {
                    bestCandidate = candidates[i];
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