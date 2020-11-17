using System.Collections.Generic;
using System.IO;

namespace OptimizationIO
{
    public class Result
    {
        private readonly int[] _cityOrder;
        private readonly CityDistances _cityDistances;
        private readonly string _path;

        public Result(int[] cityOrder, CityDistances cityDistances, string path)
        {
            _cityOrder = cityOrder;
            _cityDistances = cityDistances;
            _path = path;
        }

        public void Save()
        {
            int size = _cityDistances.CityCount;
            int sum = 0;
            for (int i = 0; i < size - 1; i++)
                sum += _cityDistances.GetDistanceBetweenCities(_cityOrder[i], _cityOrder[i + 1]);

            var result = new string[_cityOrder.Length + 1];
            result[0] = "Shortest path length = " + sum;

            for (int i = 0; i < _cityOrder.Length; i++)
                result[i + 1] = (_cityOrder[i] + 1).ToString();

            File.WriteAllLines(_path, result);                
        }
    }
}