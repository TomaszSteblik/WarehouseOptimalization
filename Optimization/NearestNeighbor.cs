using System.Collections.Generic;

namespace Optimization
{
    public class NearestNeighbor : Optimization
    {
        private readonly List<int> _cityOrder;
        private readonly List<int> _availableCities;
        //private readonly OptimizationParameters _optimizationParameters;

        public NearestNeighbor(OptimizationParameters optimizationParameters)
        {
            OptimizationParameters = optimizationParameters;
            _cityOrder = new List<int>();
            _availableCities = new List<int>();
            for (int i = 0; i < CityDistances.CityCount; i++)
            {
                _availableCities.Add(i);
            }
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
            if(CityDistances.GetDistanceBetweenCities(id, _availableCities[0]) == 0) 
                nearestCityId = _availableCities[1];
            else nearestCityId = _availableCities[0];
            int lowestDistance = CityDistances.GetDistanceBetweenCities(id, nearestCityId);
            for (int i = 0; i < _availableCities.Count; i++)
            {
                int currentDistance = CityDistances.GetDistanceBetweenCities(id, _availableCities[i]);
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