using System.Collections.Generic;

namespace Optimization
{
    public class NearestNeighbor : Optimization
    {
        private readonly List<int> _cityOrder;
        private readonly List<int> _availableCities;

        private readonly List<int> _orderPieces;
        //private readonly OptimizationParameters _optimizationParameters;

        public NearestNeighbor(OptimizationParameters optimizationParameters)
        {
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

        public NearestNeighbor(int[] order)
        {
            _cityOrder = new List<int>();
            _orderPieces = new List<int>();
            for (int i = 0; i < order.Length - 1; i++)
            {
                _orderPieces.Add(order[i]);
            }
        }

        public override int[] FindShortestPath(int[] order)
        {
            _cityOrder.Add(0);
            
            var currentId = 0;
            while (_orderPieces.Count > 1)
            {
                currentId = FindNearestNeighbor(currentId);
                _orderPieces.Add(currentId);
            }
            _orderPieces.Add(_availableCities[0]);
            _orderPieces.Add(0);
            if (OptimizationParameters.Use2opt)
            {
                Optimizer optimizer = new Optimizer();
                return optimizer.Optimize_2opt(_orderPieces.ToArray());
            }

            return _orderPieces.ToArray();
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
            if(Distances.GetDistanceBetweenObjects(id, _availableCities[0]) == 0) 
                nearestCityId = _availableCities[1];
            else nearestCityId = _availableCities[0];
            int lowestDistance = Distances.GetDistanceBetweenObjects(id, nearestCityId);
            for (int i = 0; i < _availableCities.Count; i++)
            {
                int currentDistance = Distances.GetDistanceBetweenObjects(id, _availableCities[i]);
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