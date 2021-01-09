using System.Collections.Generic;
using System.IO;

namespace Optimization
{
    public class Result
    {
        private readonly int[] _cityOrder;
        private double _length;
        private readonly string _path;

        public Result(int[] cityOrder, double length, string path)
        {
            _cityOrder = cityOrder;
            _length = length;
            _path = path;
        }

        public void Save()
        {
            int size = _cityOrder.Length;
            
            var result = new string[size + 1];
            result[0] = "Shortest path length = " + _length;

            for (int i = 0; i < size; i++)
                result[i + 1] = (_cityOrder[i] + 1).ToString();

            File.WriteAllLines(_path, result);                
        }
    }
}