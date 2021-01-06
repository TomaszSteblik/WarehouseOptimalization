using System.Collections.Generic;
using System.Linq;

namespace Optimization
{
    public class NearestNeighbor : Optimization
    {
        private readonly List<int> _objectOrder;
        private List<int> _availableObjects;

        private int[] objectOrder;
        private Distances _distances;
        
        public NearestNeighbor(OptimizationParameters optimizationParameters)
        {
            _distances = Distances.GetInstance();
            OptimizationParameters = optimizationParameters;
        }
        
        public override int[] FindShortestPath(int[] order)
        {
            int startingId = 0;
            
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
                currentId = FindNearestNeighbor(currentId);
                objectOrder[++i] = currentId;
            }

            objectOrder[++i] = _availableObjects[0];
            objectOrder[++i] = startingId;
            if (OptimizationParameters.Use2opt)
            {
                Optimizer optimizer = new Optimizer();
                return optimizer.Optimize_2opt(objectOrder);
            }

            return objectOrder;
        }
        

        private int FindNearestNeighbor(int id)
        {
            if (_availableObjects.Count == 1) 
                return -1;

            int nearestObjectId;
            int point1 = id;
            int p0 = _availableObjects[0];
            int point1b = p0;

            if (_distances._distances[point1][point1b] == 0) 
                nearestObjectId = _availableObjects[1];
            else 
                nearestObjectId = _availableObjects[0];

            double lowestDistance = _distances._distances[point1][nearestObjectId];

            for (int i = 0; i < _availableObjects.Count; i++)
            {
                int p2 = _availableObjects[i];
                int point2 = p2;
                double currentDistance = _distances._distances[point1][point2];
                if (currentDistance > 0 && currentDistance < lowestDistance)
                {
                    lowestDistance = currentDistance;
                    nearestObjectId = p2;
                }
            }
            _availableObjects.RemoveAt(_availableObjects.IndexOf(nearestObjectId));
            
            return nearestObjectId;
        }
    }
}