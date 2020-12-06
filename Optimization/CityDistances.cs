using System;
using System.IO;

namespace Optimization
{
    public class CityDistances
    {
        public int CityCount { get; private set; }
        private int[][] Distances { get; set; }

        private CityDistances()
        {
            CityCount = 0;
        }

        public static CityDistances Create(string dataSource)
        {
            var cityDistances = new CityDistances();
            var fileLines = File.ReadAllLines(dataSource);
            cityDistances.CityCount = fileLines.GetLength(0);
            cityDistances.Distances = new int[cityDistances.CityCount][];
            for (int i = 0; i < cityDistances.CityCount; i++)
            {
                cityDistances.Distances[i] = Array.ConvertAll(fileLines[i].Split(" "
                    , StringSplitOptions.RemoveEmptyEntries), int.Parse);
            }
            return cityDistances;
        }
        
        public int GetDistanceBetweenCities(int firstId, int secondId)
        {
            if (firstId > secondId) return Distances[firstId][secondId];
            return Distances[secondId][firstId];
        }

        public int CalculatePathLength(int[] path)
        {
            var sum = 0;
            for (int i = 0; i < path.Length - 1; i++)
                sum += GetDistanceBetweenCities(path[i], path[i + 1]);
            return sum;
        }
    }
}