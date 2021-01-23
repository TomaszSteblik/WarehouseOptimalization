using System;
using System.Linq;
using System.Threading.Tasks;
using Optimization.DistanceMode;
using Optimization.DistanceMode.GeneticAlgorithms;
using Optimization.DistanceMode.GeneticAlgorithms.Crossovers;
using Optimization.DistanceMode.GeneticAlgorithms.Eliminations;
using Optimization.DistanceMode.GeneticAlgorithms.Mutations;
using Optimization.DistanceMode.GeneticAlgorithms.Selections;

namespace Optimization.WarehouseMode
{
    public class Warehouse
    {
        static Random _random = new Random();
        static public event EventHandler Event1;
        static public double E = 0;

        public static void Optimizer(OptimizationParameters optimizationParameters)
        {

            var distancesMatrix = WarehouseManager.CreateWarehouseDistancesMatrix(optimizationParameters.WarehousePath);
            WarehouseManager warehouseManager = WarehouseManager.GetInstance();
            Orders orders = new Orders(optimizationParameters.OrdersPath);
            int populationSize = optimizationParameters.PopulationSize;
            int[][] population = new int[populationSize][];
            InitializePopulation(population, 0);
            
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
                        for (int k = 0; k < orders.OrdersCount; k++)
                        {
                            int[] order = Translator.TranslateWithChromosome(orders.OrdersList[k], population[i]);
                            double pathLength = FindShortestPath.Find(order, distancesMatrix, optimizationParameters);
                            fitness[i] += pathLength * orders.OrderRepeats[k];
                        }
                    });
                    Console.WriteLine(fitness.Min());
                    string log1 = "\r\nepoch="  + " minSumDist=" + E + " avgSumDist="  + "\r\n"
                                  + population[0][2] + " " + population[0][4] + " " + population[0][5] + " " + population[0][7] + " " + population[0][9] + " " + population[0][11] + "       "
                                  + population[0][13] + " " + population[0][15] + " " + population[0][17] + " " + population[0][19] + " " + population[0][21] + " " + population[0][23] + "\r\n"
                                  + population[0][1] + " " + population[0][3] + "     " + population[0][6] + " " + population[0][8] + " " + population[0][10] + "       "
                                  + population[0][12] + " " + population[0][14] + " " + population[0][16] + " " + population[0][18] + " " + population[0][20] + " " + population[0][22] + "\r\n";
                    Console.WriteLine(log1);
                    return fitness;
                });
            
            int[] z = geneticAlgorithm.FindShortestPath(itemsToSort);
            string log1 = "\r\nepoch="  + " minSumDist=" + E + " avgSumDist="  + "\r\n"
                          + z[2] + " " + z[4] + " " + z[5] + " " + z[7] + " " + z[9] + " " + z[11] + "       "
                          + z[13] + " " + z[15] + " " + z[17] + " " + z[19] + " " + z[21] + " " + z[23] + "\r\n"
                          + z[1] + " " + z[3] + "     " + z[6] + " " + z[8] + " " + z[10] + "       "
                          + z[12] + " " + z[14] + " " + z[16] + " " + z[18] + " " + z[20] + " " + z[22] + "\r\n";
            Console.WriteLine(log1);
            Log log = new Log("C:/Users/rtry/res.txt");
            log.SaveResult(z, 123);

        }
        private static bool IsThereGene(int[] chromosome, int a)
        {
            return chromosome.Any(t => t == a);
        }

        private static void InitializePopulation(int[][] pop, int start)
        {
            WarehouseManager warehouseManager = WarehouseManager.GetInstance();
            int populationSize = pop.Length;
            for (int i = 0; i < populationSize; i++)
            {
                int[] temp = new int[warehouseManager.WarehouseSize];
                for (int z = 0; z < warehouseManager.WarehouseSize; z++)
                {
                    temp[z] = -1;
                }

                int count = 0;
                temp[0] = start;
                count++;
                do
                {
                    int a = _random.Next(0, warehouseManager.WarehouseSize);
                    if (!IsThereGene(temp, a))
                    {
                        temp[count] = a;
                        count++;
                    }
                } while (count < warehouseManager.WarehouseSize);

                pop[i] = temp;
            }
        }
    }
}