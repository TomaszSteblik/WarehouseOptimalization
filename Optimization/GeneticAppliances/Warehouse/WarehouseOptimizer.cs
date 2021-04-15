using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Optimization.GeneticAlgorithms;
using Optimization.Helpers;
using Optimization.Parameters;

namespace Optimization.GeneticAppliances.Warehouse
{
    internal class WarehouseOptimizer
    {
        public static double Optimize(WarehouseParameters warehouseParameters, CancellationToken ct, Random random)
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
                    warehouseParameters.FitnessGeneticAlgorithmParameters.WriteCsv = false;
                    Parallel.For( 0, population.Length, i =>
                    {
                        fitness[i] = Fitness.CalculateAllOrdersFitness(orders, population[i], warehouseParameters.FitnessGeneticAlgorithmParameters, random);
                    });

                    return fitness;
                }, ct, random);
            
            int[] z = geneticWarehouse.Run();
            
            return Fitness.CalculateAllOrdersFitness(orders, z, warehouseParameters.FitnessGeneticAlgorithmParameters, random);
        }
    }
}