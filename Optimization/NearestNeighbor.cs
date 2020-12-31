using System.Collections.Generic;
using System.Linq;

namespace Optimization
{
    public class NearestNeighbor : Optimization
    {
        private readonly List<int> _objectOrder;
        private readonly List<int> _availableObjects;

        private int[] objectOrder;
        private Distances _distances;

        public NearestNeighbor(OptimizationParameters optimizationParameters)
        {
            _distances = Distances.GetInstance();
            OptimizationParameters = optimizationParameters;
            
            _objectOrder = new List<int>();
            _availableObjects = new List<int>();
            if (optimizationParameters.Mode == Mode.DistancesMode)
            {
                for (int i = 0; i < Distances.ObjectCount; i++)
                {
                    _availableObjects.Add(i);
                }
            }
        }

        public NearestNeighbor(int[] order, OptimizationParameters optimizationParameters)
        {
            _distances = Distances.GetInstance();
            OptimizationParameters = optimizationParameters;
            _availableObjects = new List<int>();
            objectOrder = new int[order.Length + 1];
            for (int i = 0; i < order.Length - 1; i++)
            {
                _availableObjects.Add(order[i]);
            }
        }

        public override int[] FindShortestPath(int[] order)
        {
            objectOrder[0] = 0;
            int i = 0;
            var currentId = 0;
            while (_availableObjects.Count > 1)
            {
                currentId = FindNearestNeighbor(currentId);
                objectOrder[++i] = currentId;
            }

            objectOrder[++i] = _availableObjects[0];
            objectOrder[++i] = 0;
            if (OptimizationParameters.Use2opt)
            {
                Optimizer optimizer = new Optimizer();
                return optimizer.Optimize_2opt(objectOrder);
            }

            return objectOrder;
        }
        
        public override int[] FindShortestPath(int startingId)
        {
            _availableObjects.RemoveAt(_availableObjects.IndexOf(startingId));
            _objectOrder.Add(startingId);
            
            var currentId = startingId;
            while (_availableObjects.Count > 1)
            {
                currentId = FindNearestNeighbor(currentId);
                _objectOrder.Add(currentId);
            }
            _objectOrder.Add(_availableObjects[0]);
            _objectOrder.Add(startingId);
            if (OptimizationParameters.Use2opt)
            {
                Optimizer optimizer = new Optimizer();
                return optimizer.Optimize_2opt(_objectOrder.ToArray());
            }

            return _objectOrder.ToArray();
        }

        private int FindNearestNeighbor(int id)
        {
            if (_availableObjects.Count == 1) 
                return -1;

            int nearestObjectId;
            int point1 = id;
            int p0 = _availableObjects[0];
            int point1b = p0;

            if (_distances._warehouseDistances[point1][point1b] == 0) 
                nearestObjectId = _availableObjects[1];
            else 
                nearestObjectId = _availableObjects[0];

            double lowestDistance = _distances._warehouseDistances[point1][nearestObjectId];

            for (int i = 0; i < _availableObjects.Count; i++)
            {
                int p2 = _availableObjects[i];
                int point2 = p2;
                double currentDistance = _distances._warehouseDistances[point1][point2];
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