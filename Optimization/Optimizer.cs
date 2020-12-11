using System.Collections.Generic;
using System.Linq;

namespace Optimization
{
    public class Optimizer
    {
        private List<int> _cityOrder;
        private int _improvementsSum;

        public Optimizer()
        {
            _improvementsSum = 0;
        }

        public int[] Optimize_2opt(int[] cityOrder)
        {
            _cityOrder = cityOrder.ToList();
            int improvements;
            var iterations = 0;
            do
            {
                improvements = 0;
                for (int i = 1; i < _cityOrder.Count - 1; i++)
                {
                    for (int j = i + 1; j < _cityOrder.Count - 2; j++)
                    {
                        if (TryOrderImprovement(i, j))
                        {
                            improvements++;
                        }
                    }
                }
                Log.AddToLog($"Made {improvements} improvements on iteration {++iterations}");
            } while (improvements > 0);
            Log.AddToLog($"Sum of improvements: {_improvementsSum}");

            return _cityOrder.ToArray();
        }
        
        private bool TryOrderImprovement(int firstId, int secondId)
        {
            var sumBefore = CityDistances.CalculatePathLength(_cityOrder.ToArray());
            
            _cityOrder.Reverse(firstId, secondId - firstId + 1);
            
            var sumAfter = CityDistances.CalculatePathLength(_cityOrder.ToArray());

            if (sumAfter < sumBefore)
            {
                Log.AddToLog($"Swapped {_cityOrder[firstId] + 1} with {_cityOrder[secondId] + 1} - improved by {sumBefore - sumAfter}");
                _improvementsSum += sumBefore - sumAfter;
                return true;
            }

            _cityOrder.Reverse(firstId, secondId - firstId + 1);
            return false;
        }
    }
}