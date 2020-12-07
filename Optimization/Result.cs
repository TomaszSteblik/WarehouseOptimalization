using System.Collections.Generic;
using System.IO;

namespace Optimization
{
    public class Result
    {
        private readonly int[] _cityOrder;
        private readonly string _path;

        public Result(int[] cityOrder, string path)
        {
            _cityOrder = cityOrder;
            _path = path;
        }

        public void Save()
        {
            int size = CityDistances.CityCount;
            int sum = 0;
            for (int i = 0; i < size - 1; i++)
                sum += CityDistances.GetDistanceBetweenCities(_cityOrder[i], _cityOrder[i + 1]);

            var result = new string[_cityOrder.Length + 1];
            result[0] = "Shortest path length = " + sum;

            for (int i = 0; i < _cityOrder.Length; i++)
                result[i + 1] = (_cityOrder[i] + 1).ToString();

            File.WriteAllLines(_path, result);                
        }
    }
}