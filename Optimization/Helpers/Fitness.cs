using System.Threading;
using Optimization.GeneticAppliances.Warehouse;
using Optimization.Parameters;
using Optimization.PathFinding;

namespace Optimization.Helpers
{
    internal class Fitness
    {
        public static double CalculateFitness(int[] path)
        {
            double[][] distancesMatrix = Distances.GetInstance().DistancesMatrix;
            var sum = 0d;
            for (int i = 0; i < path.Length - 1; i++)
                sum += distancesMatrix[path[i]][path[i + 1]];
            if (path[^1] != 0) sum += distancesMatrix[path[^1]][0];
            return sum;
        }

        public static double CalculateAllOrdersFitness(Orders orders, int[] chromosome, OptimizationParameters optimizationParameters)
        {
            double fitness = 0d;
            for (int k = 0; k < orders.OrdersCount; k++)
            {
                int[] order = Translator.TranslateWithChromosome(orders.OrdersList[k], chromosome);
                double pathLength = ShortestPath.Find(order, optimizationParameters, CancellationToken.None);
                fitness += pathLength * orders.OrderRepeats[k];
            }

            return fitness;
        }
        public static double CalculateFitness(int[] gene, double[] frequency, double[] weights)
        {
            var geneLength = gene.Length;
            var fitness = 0d;
            for (int i = 0; i < geneLength; i++)
            {
                if(gene[i] == 0) continue;
                fitness += frequency[gene[i] - 'A'] * weights[i];
            }

            return fitness;
        }
        
    }
}