using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Optimization.GeneticAlgorithms;
using Optimization.Helpers;
using Optimization.Parameters;
using Optimization.PathFinding;

namespace Optimization.GeneticAppliances.Warehouse
{
    internal class WarehouseOptimizer
    {
        public static WarehouseResult Optimize(WarehouseParameters warehouseParameters, CancellationToken ct, Random random)
        {
            WarehouseManager warehouseManager = new WarehouseManager();
            double[][] distancesMatrix = warehouseManager.CreateWarehouseDistancesMatrix(warehouseParameters.WarehousePath);
            Distances.Create(distancesMatrix);
            Orders orders = new Orders(warehouseParameters.OrdersPath, warehouseManager.WarehouseSize);
            
            GeneticWarehouse geneticWarehouse = new GeneticWarehouse(warehouseParameters.WarehouseGeneticAlgorithmParameters,
                warehouseManager.WarehouseSize,
                (population) =>
                {
                    double[] fitness = new double[population.Length];
                    warehouseParameters.FitnessGeneticAlgorithmParameters.WriteCsv = false;
                    Parallel.For( 0, population.Length, i =>
                    {
                        var results = Fitness.CalculateAllOrdersFitness(orders, population[i],
                            warehouseParameters.FitnessGeneticAlgorithmParameters, random);
                        fitness[i] = results.Sum(x => x.Fitness);
                    });

                    return fitness;
                }, ct, random);
            
            var z = geneticWarehouse.Run();
            var result = Fitness.CalculateAllOrdersFitness(orders, z.BestChromosome, warehouseParameters.FitnessGeneticAlgorithmParameters, random);
            z.FinalFitness = result.Sum(x => x.Fitness);
            z.FinalOrderPaths = result.Select(x => x.Path).ToArray();
            z.AvgFitnessInEpoch = z.fitness.Select(x => x.Average()).ToArray();
            z.BestFitnessInEpoch = z.fitness.Select(x => x.Min()).ToArray();
            return z;
        }
    }
}