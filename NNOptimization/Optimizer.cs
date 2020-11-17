using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OptimizationIO;

namespace Optimization
{
    public class Optimizer
    {
        public List<int> _cityOrder { get; set; }
        public OptimizationParameters _optimizationParameters { get; set; }

        public CityDistances _cityDistances;

        public Optimizer(CityDistances cityDistances,OptimizationParameters optimizationParameters)
        {
            _optimizationParameters = optimizationParameters;
            _cityDistances = cityDistances;
        }

        public int[] Optimize_2opt(int[] cityOrder)
        {
            _cityOrder = cityOrder.ToList();
            int improvements;
            var iterations = 0;
            var log = new Log(_optimizationParameters.LogPath);
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

            return _cityOrder.ToArray();
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
    }
}