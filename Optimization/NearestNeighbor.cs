using System.Collections.Generic;
using System.Linq;

namespace Optimization
{
    public class NearestNeighbor : Optimization
    {
        private readonly List<int> _cityOrder;
        private readonly List<int> _availableCities;

        private Distances _distances;

        public NearestNeighbor(OptimizationParameters optimizationParameters)
        {
            _distances = Distances.GetInstance();
            OptimizationParameters = optimizationParameters;
            _cityOrder = new List<int>();
            _availableCities = new List<int>();
            if (optimizationParameters.Mode == Mode.DistancesMode)
            {
                for (int i = 0; i < Distances.ObjectCount; i++)
                {
                    _availableCities.Add(i);
                }
            }
        }

        public NearestNeighbor(int[] order, OptimizationParameters optimizationParameters)
        {
            _distances = Distances.GetInstance();
            OptimizationParameters = optimizationParameters;
            _availableCities = new List<int>();
            _cityOrder = new List<int>();
            for (int i = 0; i < order.Length - 1; i++)
            {
                _availableCities.Add(order[i]);
            }
        }

        public override int[] FindShortestPath(int[] order)
        {
            _cityOrder.Add(0);
            
            var currentId = 0;
            while (_availableCities.Count > 1)
            {
                currentId = FindNearestNeighbor(currentId);
                _cityOrder.Add(currentId);
            }
            _cityOrder.Add(_availableCities[0]);
            _cityOrder.Add(0);
            if (OptimizationParameters.Use2opt)
            {
                Optimizer optimizer = new Optimizer();
                return optimizer.Optimize_2opt(_cityOrder.ToArray());
            }

            return _cityOrder.ToArray();
        }
        
        public override int[] FindShortestPath(int startingId)
        {
            _availableCities.RemoveAt(_availableCities.IndexOf(startingId));
            _cityOrder.Add(startingId);
            
            var currentId = startingId;
            while (_availableCities.Count > 1)
            {
                currentId = FindNearestNeighbor(currentId);
                _cityOrder.Add(currentId);
            }
            _cityOrder.Add(_availableCities[0]);
            _cityOrder.Add(startingId);
            if (OptimizationParameters.Use2opt)
            {
                Optimizer optimizer = new Optimizer();
                return optimizer.Optimize_2opt(_cityOrder.ToArray());
            }

            return _cityOrder.ToArray();
        }

        private int FindNearestNeighbor(int id)
        {
            if (_availableCities.Count == 1) return -1;
            int nearestCityId;
            if(_distances._warehouseDistances[id][_availableCities[0]] == 0) 
                nearestCityId = _availableCities[1];
            else nearestCityId = _availableCities[0];
            double lowestDistance = _distances._warehouseDistances[id][nearestCityId];
            for (int i = 0; i < _availableCities.Count; i++)
            {
                double currentDistance = _distances._warehouseDistances[id][_availableCities[i]];
                if (currentDistance > 0 && currentDistance < lowestDistance)
                {
                    lowestDistance = currentDistance;
                    nearestCityId = _availableCities[i];
                }
            }
            _availableCities.RemoveAt(_availableCities.IndexOf(nearestCityId));
            
            return nearestCityId;
        }
    }
}