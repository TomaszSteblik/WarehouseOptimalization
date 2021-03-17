using System.Collections.Generic;
using System.Linq;

namespace Optimization.Distances
{
    internal class Optimizer2Opt
    {
        private List<int> _objectOrder;
        private double[][] _distances;

        public int[] Optimize(int[] objectOrder, double[][] distances)
        {
            _objectOrder = objectOrder.ToList();
            _distances = distances;
            int improvements;
            do
            {
                improvements = 0;
                for (int i = 1; i < _objectOrder.Count - 1; i++)
                {
                    for (int j = i + 1; j < _objectOrder.Count - 2; j++)
                    {
                        if (TryOrderImprovement(i, j))
                        {
                            improvements++;
                        }
                    }
                }
            } while (improvements > 0);

            return _objectOrder.ToArray();
        }
        
        private bool TryOrderImprovement(int firstId, int secondId)
        {
            var sumBefore = Fitness.CalculateFitness(_objectOrder.ToArray(), _distances);
            
            _objectOrder.Reverse(firstId, secondId - firstId + 1);
            
            var sumAfter = Fitness.CalculateFitness(_objectOrder.ToArray(), _distances);

            if (sumAfter < sumBefore)
                return true;
            

            _objectOrder.Reverse(firstId, secondId - firstId + 1);
            return false;
        }
    }
}