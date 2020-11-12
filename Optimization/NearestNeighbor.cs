using System.Collections.Generic;

namespace Optimization
{
    public class NearestNeighbor : Optimization
    {
        private List<int> _cityOrder;
        private List<int> AvailableCities { get; set; }

        private readonly CityDistances _cityDistances;

        public NearestNeighbor(CityDistances cityDistances)
        {
            _cityDistances = cityDistances;
            _cityOrder = new List<int>();
            AvailableCities = new List<int>();
            for (int i = 0; i < cityDistances.CityCount; i++)
            {
                AvailableCities.Add(i);
            }
        }
        
        public List<int> Optimize(List<int> cityOrder)
        {
            _cityOrder = cityOrder;
            int improvements;
            var iterations = 0;
            var log = new Log("/Users/rtry/log.txt");
            do
            {
                improvements = 0;
                for (int i = 1; i < _cityOrder.Count - 1; i++)
                {
                    for (int j = i + 1; j < _cityOrder.Count - 2; j++)
                    {
                        if (TryOrderImprovement(i, j))
                        {
                            log.AddToLog($"Swapped {_cityOrder[i] + 1} with {_cityOrder[j] + 1}");
                            improvements++;
                        }
                    }
                }
                log.AddToLog($"Made {improvements} improvements on iteration {++iterations}");
            } while (improvements > 0);

            return _cityOrder;
        }

        private bool TryOrderImprovement(int firstId, int secondId)
        {
            var sumBefore = 0;
            var sumAfter = 0;
            
            for (int i = 0; i < _cityOrder.Count - 1; i++)
                sumBefore += _cityDistances.GetDistanceBetweenCities(_cityOrder[i], _cityOrder[i + 1]);
            
            _cityOrder.Reverse(firstId, secondId - firstId + 1);
            
            for (int i = 0; i < _cityOrder.Count - 1; i++)
                sumAfter += _cityDistances.GetDistanceBetweenCities(_cityOrder[i], _cityOrder[i + 1]);
            
            if (sumAfter < sumBefore) 
                return true;
            
            _cityOrder.Reverse(firstId, secondId - firstId + 1);
            return false;
        }
        public override List<int> FindShortestPath(int startingId)
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
            return Optimize(_cityOrder);
        }

        public int FindNearestNeighbor(int id)
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