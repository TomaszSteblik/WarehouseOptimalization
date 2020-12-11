using System;
using System.IO;

namespace Optimization
{
    public class CityDistances
    {

        private int _cityCount;
        public int[][] _distances;

        public static int CityCount
        {
            get => _instance._cityCount;
            private set => _instance._cityCount = value;
        }

        private static CityDistances _instance;

        private CityDistances()
        {
            _cityCount = 0;
        }

        public static CityDistances GetInstance()
        {
            return _instance;
        }

        public static void Create(string dataSource)
        {
            _instance = new CityDistances();
            var fileLines = File.ReadAllLines(dataSource);
            _instance._cityCount = fileLines.GetLength(0);
            _instance._distances = new int[_instance._cityCount][];
            for (int i = 0; i < _instance._cityCount; i++)
            {
                _instance._distances[i] = Array.ConvertAll(fileLines[i].Split(" "
                    , StringSplitOptions.RemoveEmptyEntries), int.Parse);
            }
        }
        
        public static int GetDistanceBetweenCities(int firstId, int secondId)
        {
            return _instance._distances[firstId][secondId];
        }

        public static int CalculatePathLength(int[] path)
        {
            var sum = 0;
            for (int i = 0; i < path.Length - 1; i++)
                sum += GetDistanceBetweenCities(path[i], path[i + 1]);
            return sum;
        }
    }
}