using System.Collections.Generic;
using OptimizationIO;

namespace Optimization
{
    public class NearestNeighbor : OptimizationIO.Optimization
    {
        private List<int> _cityOrder;
        private List<int> AvailableCities { get; set; }

        private readonly CityDistances _cityDistances;

        private readonly OptimizationParameters _optimizationParameters;

        public NearestNeighbor(CityDistances cityDistances, OptimizationParameters optimizationParameters)
        {
            _optimizationParameters = optimizationParameters;
            _cityDistances = cityDistances;
            _cityOrder = new List<int>();
            AvailableCities = new List<int>();
            for (int i = 0; i < cityDistances.CityCount; i++)
            {
                AvailableCities.Add(i);
            }
        }
        
        public override int[] FindShortestPath(int startingId)
        {
            AvailableCities.RemoveAt(AvailableCities.IndexOf(startingId));
            _cityOrder.Add(startingId);
            
            var currentId = startingId;
            while (AvailableCities.Count > 1)
            {
                currentId = FindNearestNeighbor(currentId);
                _cityOrder.Add(currentId);
            }
            _cityOrder.Add(AvailableCities[0]);
            _cityOrder.Add(startingId);
            if (_optimizationParameters.Use2opt)
            {
                Optimizer optimizer = new Optimizer(_cityDistances, _optimizationParameters);
                return optimizer.Optimize_2opt(_cityOrder.ToArray());
            }

            return _cityOrder.ToArray();
        }

        private int FindNearestNeighbor(int id)
        {
            if (AvailableCities.Count == 1) return -1;
            int nearestCityId;
            if(_cityDistances.GetDistanceBetweenCities(id, AvailableCities[0]) == 0) 
                nearestCityId = AvailableCities[1];
            else nearestCityId = AvailableCities[0];
            int lowestDistance = _cityDistances.GetDistanceBetweenCities(id, nearestCityId);
            for (int i = 0; i < AvailableCities.Count; i++)
            {
                int currentDistance = _cityDistances.GetDistanceBetweenCities(id, AvailableCities[i]);
                if (currentDistance > 0 && currentDistance < lowestDistance)
                {
                    lowestDistance = currentDistance;
                    nearestCityId = AvailableCities[i];
                }
            }
            AvailableCities.RemoveAt(AvailableCities.IndexOf(nearestCityId));
            
            return nearestCityId;
        }
    }
}