using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//based on the source code from https://www.chadgolden.com/blog/finding-all-the-permutations-of-an-array-in-c-sharp

namespace Optimization.DistanceMode
{
    class Permutations : Optimization
    {
        private double[][] _distancesMatrix;
        
        public Permutations(OptimizationParameters optimizationParameters, double[][] distancesMatrix)
        {
            _optimizationParameters = optimizationParameters;
            _distancesMatrix = distancesMatrix;
        }
        public override int[] FindShortestPath(int[] locationsToVisit)
        {
            int[][] routes = Permute(locationsToVisit).Select(Enumerable.ToArray).ToArray();

            int ltvSize = locationsToVisit.Length;
            int rSize = routes.Length;
            double distance, minDistance = Double.MaxValue;
            int minR = -1;


            for (int r = 0; r < rSize - 1; r++)
            {
                distance = 0;
                for (int p = 0; p < ltvSize - 1; p++)
                    distance += _distancesMatrix[routes[r][p]][routes[r][p + 1]];

                distance += _distancesMatrix[routes[r][0]][routes[r][ltvSize - 1]];

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


