using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Optimization
{
    public class Distances
    {
        public static int[] GenerateObjectIdList(int size)
        {
            int[] result = new int[size];
            for (int i = 0; i < size; i++)
            {
                result[i] = i;
            }

            return result;
        }

        public static double CalculatePathLengthDouble(int[] path, double[][] distances)
        {
            var sum = 0d;
            for (int i = 0; i < path.Length - 1; i++)
                sum += distances[path[i]][path[i + 1]];
            return sum;
        }

    }
}