using System;
using System.IO;

namespace OptimizationIO
{
    public class CityDistances
    {
        public int CityCount { get; set; }
        public int[][] Distances { get; set; }

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
    }
}