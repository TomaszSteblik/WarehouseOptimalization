using Optimization.WarehouseMode;

namespace Optimization.DistanceMode
{
    public class Fitness
    {
        public static double CalculateFitness(int[] path, double[][] distancesMatrix)
        {
            var sum = 0d;
            for (int i = 0; i < path.Length - 1; i++)
                sum += distancesMatrix[path[i]][path[i + 1]];
            return sum;
        }

        public static double CalculateAllOrdersFitness(Orders orders, int[] chromosome, double[][] distancesMatrix, OptimizationParameters optimizationParameters)
        {
            double fitness = 0d;
            for (int k = 0; k < orders.OrdersCount; k++)
            {
                int[] order = Translator.TranslateWithChromosome(orders.OrdersList[k], chromosome);
                double pathLength = FindShortestPath.Find(order, distancesMatrix, optimizationParameters);
                fitness += pathLength * orders.OrderRepeats[k];
            }

            return fitness;
        }
    }
}