using System;
using System.Collections.Generic;
using System.Linq;
using Optimization.Helpers;
using Optimization.Parameters;

//based on the source code from https://www.chadgolden.com/blog/finding-all-the-permutations-of-an-array-in-c-sharp

namespace Optimization.PathFinding
{
    internal class Permutations : IPathFinder
    {
        private double[][] _distancesMatrix;
        private OptimizationParameters _optimizationParameters;
        
        public Permutations(OptimizationParameters optimizationParameters)
        {
            _optimizationParameters = optimizationParameters;
            _distancesMatrix = Distances.GetInstance().DistancesMatrix;
        }
        public int[] FindShortestPath(int[] locationsToVisit)
        {
            ConvertedMatrix arr = new ConvertedMatrix(_distancesMatrix, locationsToVisit);
            _distancesMatrix = arr.GetConvertedMatrix();
            int[][] routes = Permute(locationsToVisit).Select(Enumerable.ToArray).ToArray();

            int ltvSize = locationsToVisit.Length;
            int rSize = routes.Length;
            double distance, minDistance = Double.MaxValue;
            int minR = -1;

            int[] translation = arr.GetTranslation();
            for (int r = 0; r < rSize - 1; r++)
            {
                
                distance = 0;
                for (int p = 0; p < ltvSize - 1; p++)
                    distance += _distancesMatrix[translation[routes[r][p]]][translation[routes[r][p+1]]];
                
                distance += _distancesMatrix[translation[routes[r][0]]][translation[routes[r][ltvSize - 1]]];

                if (distance < minDistance)
                {
                    minDistance = distance;
                    minR = r;
                }
            }

            return routes[minR];
        }
        public static IList<IList<int>> Permute(int[] nums)
        {
            var list = new List<IList<int>>();
            return DoPermute(nums, 0, nums.Length - 1, list);
        }
        
        static IList<IList<int>> DoPermute(int[] nums, int start, int end, IList<IList<int>> list)
        {
            int temp;
            if (start == end)
            {
                list.Add(new List<int>(nums));
            }
            else
            {
                for (var i = start; i <= end; i++)
                {
                    temp = nums[start];
                    nums[start] = nums[i];
                    nums[i] = temp;

                    DoPermute(nums, start + 1, end, list);

                    temp = nums[start];
                    nums[start] = nums[i];
                    nums[i] = temp;
                }
            }
            return list;
        }
    }
}


