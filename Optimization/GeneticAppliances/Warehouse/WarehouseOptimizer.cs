using System;
using System.Linq;
using System.Threading.Tasks;
using Optimization.GeneticAlgorithms;
using Optimization.Helpers;
using Optimization.Parameters;

namespace Optimization.GeneticAppliances.Warehouse
{
    internal class WarehouseOptimizer
    {
        public static void Optimize(WarehouseParameters warehouseParameters)
        {
            WarehouseManager warehouseManager = new WarehouseManager();
            double[][] distancesMatrix = warehouseManager.CreateWarehouseDistancesMatrix(warehouseParameters.WarehousePath);
            Distances.Create(distancesMatrix);
            Orders orders = new Orders(warehouseParameters.OrdersPath);
            
            IGeneticAppliance geneticWarehouse = new GeneticWarehouse(warehouseParameters.WarehouseGeneticAlgorithmParameters,
                warehouseManager.WarehouseSize,
                (population) =>
                {
                    double[] fitness = new double[population.Length];
                    Parallel.For( 0, population.Length, i =>
                    {
                        fitness[i] = Fitness.CalculateAllOrdersFitness(orders, population[i], warehouseParameters.FitnessGeneticAlgorithmParameters);
                    });
                    Console.WriteLine(fitness.Min());
                    
                    return fitness;
                });
            
            int[] z = geneticWarehouse.Run();
            /*
            if (warehouseParameters.WarehouseGeneticAlgorithmParameters.ResultToFile)
            {
                double fitness = Fitness.CalculateAllOrdersFitness(orders, z, distancesMatrix, optimizationParameters);
                Log log = new Log(optimizationParameters);
                log.SaveResult(z, fitness);
            }
            */
            string result =  z[2] + " " + z[4] + " " + z[5] + " " + z[7] + " " + z[9] + " " + z[11] + "       "
                          + z[13] + " " + z[15] + " " + z[17] + " " + z[19] + " " + z[21] + " " + z[23] + "\r\n"
                          + z[1] + " " + z[3] + "     " + z[6] + " " + z[8] + " " + z[10] + "       "
                          + z[12] + " " + z[14] + " " + z[16] + " " + z[18] + " " + z[20] + " " + z[22] + "\r\n";

            Console.WriteLine(result);
        }
    }
}