using System.Collections.Generic;
using Optimization.Helpers;
using Optimization.PathFinding;

namespace Optimization.GeneticAlgorithms.Crossovers
{
    internal class AexNNCrossover : AexCrossover
    {
        private double[][] DistancesMatrix { get; }
        public AexNNCrossover()
        {
            DistancesMatrix = Distances.GetInstance().DistancesMatrix;
        }

        protected override int ResolveConflict(int currentPoint, List<int> availableVertexes)
        {
            return NearestNeighbor.FindNearestNeighbor(currentPoint, DistancesMatrix, availableVertexes);
        }
    }
}