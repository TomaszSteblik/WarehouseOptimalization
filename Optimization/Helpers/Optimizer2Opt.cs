using System.Collections.Generic;
using System.Linq;

namespace Optimization.Helpers
{
    internal class Optimizer2Opt
    {

        public static int[] Optimize(int[] objectOrderArr)
        {
            List<int> objectOrder = objectOrderArr.ToList();
            int improvements;
            do
            {
                improvements = 0;
                for (int i = 1; i < objectOrder.Count - 1; i++)
                {
                    for (int j = i + 1; j < objectOrder.Count - 2; j++)
                    {
                        if (TryOrderImprovement(i, j, objectOrder))
                        {
                            improvements++;
                        }
                    }
                }
            } while (improvements > 0);

            return objectOrder.ToArray();
        }
        
        private static bool TryOrderImprovement(int firstId, int secondId, List<int> objectOrder)
        {
            var sumBefore = Fitness.CalculateFitness(objectOrder.ToArray());
            
            objectOrder.Reverse(firstId, secondId - firstId + 1);
            
            var sumAfter = Fitness.CalculateFitness(objectOrder.ToArray());

            if (sumAfter < sumBefore)
                return true;
            

            objectOrder.Reverse(firstId, secondId - firstId + 1);
            return false;
        }
    }
}