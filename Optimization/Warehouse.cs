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
            
            //genereracja losowej populacji; każdy osobnik reprezentuje rozkład towarów
            int populationSize = optimizationParameters.PopulationSize;
            int[][] population = new int[populationSize][];
            InitializePopulation(population, 0);
            
            
            

            //order.txt
            // 3 4 5 1 50
            // 4 5 1 

            double[] FitnessProductPlacement;

            //AEX
            for (int e = 0; e < optimizationParameters.TerminationValue; e++) //opt. rozkładu produktów
            {

           


            //fiteness
            Parallel.For((long) 0, populationSize, i =>
            {

               for (int k = 0; k < distances.OrdersCount; k++)
               {
                   double pathLength = FindShortestPath.Find(distances.orders[k], optimizationParameters);
                   FitnessProductPlacement[i] += pathLength * distances.orders[k][distances.orders.GetLength(k) - 1];
              }

             });

            //selekcja
            Selection selection;
            int[][] parents = new int[optimizationParameters.ChildrenPerGeneration*2][];
            
            //krzyżowanie
            Crossover crossover = new Crossover.AexCrossover();
            int[][] offsprings = crossover.GenerateOffsprings(parents);
            
            //eliminacja
            Elimination elimination;
            
            //mutacja
            
            foreach (var chromosome in population)
            {
                if (_random.Next(0, 1000) <= optimizationParameters.MutationProbability)
                {
                    //Log.AddToLog($"MUTATION RSM\nBEFORE MUTATION({Distances.CalculatePathLength(chromosome)}): {string.Join(";",chromosome)}");

                    var j = _random.Next(1, Distances.WarehouseSize);
                    var i = _random.Next(1, j);
                    Array.Reverse(chromosome,i,j-i);

                    //Log.AddToLog($"AFTER MUTATION({Distances.CalculatePathLength(chromosome)}):  {string.Join(";",chromosome)}\n");
                }
            }
            

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