using System;
using System.Linq;
using System.Threading.Tasks;
using Optimization.DistanceMode;
using Optimization.DistanceMode.GeneticAlgorithms;

namespace Optimization.WarehouseMode
{
    public class Warehouse
    {
        public static void Optimizer(OptimizationParameters optimizationParameters)
        {
            WarehouseManager warehouseManager = new WarehouseManager();
            double[][] distancesMatrix = warehouseManager.CreateWarehouseDistancesMatrix(optimizationParameters.WarehousePath);
            Orders orders = new Orders(optimizationParameters.OrdersPath);

            int[] itemsToSort = new int[warehouseManager.WarehouseSize];
            for (int i = 1; i < warehouseManager.WarehouseSize; i++)
            {
                itemsToSort[i - 1] = i;
            }
            
            GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(optimizationParameters, distancesMatrix,
                (population, matrix) =>
                {
                    double[] fitness = new double[population.Length];
                    Parallel.For((long) 0, population.Length, i =>
                    {
                        fitness[i] = Fitness.CalculateAllOrdersFitness(orders, population[i], distancesMatrix, optimizationParameters);
                    });
                    Console.WriteLine(fitness.Min());
                    
                    return fitness;
                });
            
            int[] z = geneticAlgorithm.FindShortestPath(itemsToSort);

            if (optimizationParameters.ResultToFile)
            {
                double fitness = Fitness.CalculateAllOrdersFitness(orders, z, distancesMatrix, optimizationParameters);
                Log log = new Log(optimizationParameters);
                log.SaveResult(z, fitness);
            }
            
            string result =  z[2] + " " + z[4] + " " + z[5] + " " + z[7] + " " + z[9] + " " + z[11] + "       "
                          + z[13] + " " + z[15] + " " + z[17] + " " + z[19] + " " + z[21] + " " + z[23] + "\r\n"
                          + z[1] + " " + z[3] + "     " + z[6] + " " + z[8] + " " + z[10] + "       "
                          + z[12] + " " + z[14] + " " + z[16] + " " + z[18] + " " + z[20] + " " + z[22] + "\r\n";

            Console.WriteLine(result);
        }
    }
}