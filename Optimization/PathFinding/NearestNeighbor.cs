using System.Collections.Generic;
using Optimization.Helpers;
using Optimization.Parameters;

namespace Optimization.PathFinding
{
    internal class NearestNeighbor : IPathFinder
    {
        private List<int> _availableObjects;

        private int[] objectOrder;
        private double[][] DistancesMatrix { get; }
        private OptimizationParameters _optimizationParameters;
        
        public NearestNeighbor(OptimizationParameters optimizationParameters)
        {
            _optimizationParameters = optimizationParameters;
            DistancesMatrix = Distances.GetInstance().DistancesMatrix;

        }
        
        public int[] FindShortestPath(int[] order)
        {
            int startingId = _optimizationParameters.StartingId;
            
            _availableObjects = new List<int>();
            for (int j = 0; j < order.Length; j++)
            {
                _availableObjects.Add(order[j]);
            }

            objectOrder = new int[_availableObjects.Count + 1];
            objectOrder[0] = startingId;
            
            _availableObjects.RemoveAt(_availableObjects.IndexOf(startingId));
            
            int i = 0;
            var currentId = 0;
            while (_availableObjects.Count > 1)
            {
                currentId = FindNearestNeighbor(currentId, DistancesMatrix, _availableObjects);
                objectOrder[++i] = currentId;
            }

            objectOrder[++i] = _availableObjects[0];
            objectOrder[++i] = startingId;

            return objectOrder;
        }
        

        public static int FindNearestNeighbor(int currentPoint, double[][] distancesMatrix, List<int> availableVertexes)
        {
            if (availableVertexes.Count == 1) 
                return availableVertexes[0];

            int nearestObjectId;
            int p0 = availableVertexes[0];

            if (distancesMatrix[currentPoint][p0] == 0) 
                nearestObjectId = availableVertexes[1];
            else 
                nearestObjectId = availableVertexes[0];

            double lowestDistance = distancesMatrix[currentPoint][nearestObjectId];

            for (int i = 0; i < availableVertexes.Count; i++)
            {
                int nextPoint = availableVertexes[i];
                double currentDistance = distancesMatrix[currentPoint][nextPoint];
                if (currentDistance > 0 && currentDistance < lowestDistance)
                {
                    lowestDistance = currentDistance;
                    nearestObjectId = nextPoint;
                }
            }
            availableVertexes.RemoveAt(availableVertexes.IndexOf(nearestObjectId));
            
            return nearestObjectId;
        }
    }
}