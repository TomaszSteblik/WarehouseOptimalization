using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization
{
    public class Warehouse
    {
        static Random _random = new Random();

        public static void Optimizer(OptimizationParameters optimizationParameters)
        {
            
            Distances.CreateWarehouse(optimizationParameters.WarehousePath); //wczytanie struktury i utworzenie macierzy Distances._warehouseDistances[][]
            Distances.LoadOrders(optimizationParameters.OrdersPath); //zaladowanie orderow z pliku -> Distances.orders[][] jest dostep
            Distances distances = Distances.GetInstance();

            int populationSize = optimizationParameters.PopulationSize;
            int[][] population = new int[populationSize][];
            InitializePopulation(population, 0);
            foreach (var VARIABLE in population[4])
            {
                Console.WriteLine(VARIABLE);
            }
            //genereracja losowej populacji; każdy osobnik reprezentuje rozkład towarów

            //order.txt
            // 3 4 5 1 50
            // 4 5 1 

            //double[] FitnessProductPlacement;

            //AEX
            //for (int e = 0; e < maxEpoch; e++) //opt. rozkładu produktów
            //{

            //selekcja

            //krzyżowanie


            //fiteness
            //Parallel.For(0, liczbaOsobnikow_RoznychRozkladowMagazynu, i =>
            //{

            //   for (int k = 0; k < numberOfOrders; k++)
            //  {
            //      int dl_sciezki =  //znajdz najkrótsza ścieżke albo przez NN albo przez HGrex opcjonalnie z 2-opt albo Permutations.FindShortetsRoute();
            //         FitnessProductPlacement[i] += dl_sciezki * ile_razy_bylo_takie_samo_zam;
            //  }

            // });


            //mutacje 2        4

            // }

            //miejsca w magazynie    0  1  2  3  4  5  6
            //produkty              [0  3  4  1  5  6  2]
        }
        
        
        private static bool IsThereGene(int[] chromosome, int a)
        {
            return chromosome.Any(t => t == a);
        }

        private static void InitializePopulation(int[][] pop,int start)
        {
            int populationSize = pop.Length;
            for (int i = 0; i < populationSize; i++)
            {
                int[] temp = new int[Distances.WarehouseSize];
                for (int z = 0; z < Distances.WarehouseSize; z++)
                {
                    temp[z] = -1;
                }
                int count = 0;
                temp[0] = start;
                count++;
                do
                {
                    int a = _random.Next(0,Distances.WarehouseSize);
                    if (!IsThereGene(temp,a))
                    {
                        temp[count] = a;
                        count++;
                    }
                } while (count<Distances.WarehouseSize);
                pop[i] = temp;
            }
        }

    }
}