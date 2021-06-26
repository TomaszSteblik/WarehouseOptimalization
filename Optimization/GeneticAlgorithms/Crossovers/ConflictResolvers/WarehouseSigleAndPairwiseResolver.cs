using System;
using System.Collections.Generic;


namespace Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers
{
    class WarehouseSigleAndPairwiseResolver : ConflictResolver
    {
        WarehouseSingleProductFrequencyResolver ws;
        WarehousePairwiseProductFrequencyResolver wp;
        Random random;
        double frequencyWS;


        public WarehouseSigleAndPairwiseResolver(Random random, double probability, int participantsCount, double frequencyWS = 0.7) : base(random, probability)
        {
            this.random = random;
            this.frequencyWS = frequencyWS;
            ws = new WarehouseSingleProductFrequencyResolver(random, probability, participantsCount);
            wp = new WarehousePairwiseProductFrequencyResolver(random, probability, participantsCount);

        }



        public override int ResolveConflict(int currentPoint, List<int> availableVertexes)
        {
            int bestCandidate;
            if (random.NextDouble() < frequencyWS)
                bestCandidate = ws.ResolveConflict(currentPoint, availableVertexes);
            else
                bestCandidate = wp.ResolveConflict(currentPoint, availableVertexes);

            return bestCandidate;
        }
    }
}
