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
        static public event EventHandler Event1;
        static public double E = 0;

        public static void Optimizer(OptimizationParameters optimizationParameters)
        {
            Log.Create(optimizationParameters.LogPath);

            Distances.CreateWarehouse(optimizationParameters.WarehousePath); //wczytanie struktury i utworzenie macierzy Distances._warehouseDistances[][]
            Distances.LoadOrders(optimizationParameters.OrdersPath); //zaladowanie orderow z pliku -> Distances.orders[][] jest dostep
            Distances distances = Distances.GetInstance();
            

            //genereracja losowej populacji; każdy osobnik reprezentuje rozkład towarów
            int populationSize = optimizationParameters.PopulationSize;
            int[][] population = new int[populationSize][];
            InitializePopulation(population, 0);
            for (int i = 0; i < populationSize; i++)
            {
                population[i] = new[] {0, 10, 11, 12, 13, 14, 15, 1, 2, 3, 4, 5, 6, 7, 8, 9, 16, 17, 18, 19, 20, 21, 22, 23};

            }



            //order.txt
            // 3 4 5 1 50
            // 4 5 1 


            //AEX
            for (int e = 0; e < optimizationParameters.TerminationValue; e++) //opt. rozkładu produktów
            {
                double[] FitnessProductPlacement = new double[populationSize];

                //fiteness
                //for(int i =0; i<populationSize; i++)
                Parallel.For(0, populationSize, i =>
                {
                    for (int k = 0; k < distances.OrdersCount; k++)
                    {
                        double pathLength =
                            FindShortestPath.Find(distances.orders[k], population[i], optimizationParameters);
                        FitnessProductPlacement[i] +=
                            pathLength * distances.orders[k][distances.orders[k].Length - 1];
                    }

                });


                Log.AddToLog("Populacja nr." + e);
                Log.AddToLog("avg: " + FitnessProductPlacement.Average());
                Log.AddToLog("max: " + FitnessProductPlacement.Max());
                Log.AddToLog("min: " + FitnessProductPlacement.Min());
                for (int i = 0; i < populationSize; i++)
                {
                    Log.AddToLog($"Chromosome({FitnessProductPlacement[i]}): {string.Join(";", population[i])} \n");
                }

                //selekcja
                Selection
                    selection = new ElitismSelection(
                        population); // NA TEN MOMENT DZIAŁA TYLKO SELEKCJA ROULETTE WHEEL TODO: Pozostałe selekcje 
                int[][] parents = selection.GenerateParents(optimizationParameters.ChildrenPerGeneration * 2,
                    FitnessProductPlacement);

                //krzyżowanie
                Crossover crossover = new Crossover.AexCrossover();
                int[][] offsprings = crossover.GenerateOffsprings(parents);

                //eliminacja
                Elimination elimination;
                switch (optimizationParameters.EliminationMethod)
                {
                    case "Elitism":
                        elimination = new ElitismElimination(ref population);
                        break;
                    case "RouletteWheel":
                        elimination = new RouletteWheelElimination(ref population);
                        break;
                    default:
                        throw new ArgumentException("Wrong elimination name in parameters json file");
                }
                        
                elimination.EliminateAndReplace(offsprings, FitnessProductPlacement);
                //mutacja


                int x = 0;
                // E = newFitness.Min();
                // Event1?.Invoke(null, null);
                //  optimizationParameters.MutationProbability *= 1.1;
                //Console.WriteLine(newFitness[x]+"     10-11-12-13-14-15");
                if (e % 10 == 0)
                {
                    E = FitnessProductPlacement.Min();
                    Console.WriteLine(E);
                    //Event1?.Invoke(null, null);
                    //Console.WriteLine(newFitness[x]+"     10-11-12-13-14-15");
                    
                    Console.Write(GetProductByLocation(2, population[x]) + " " + GetProductByLocation(4, population[x]) + " " + GetProductByLocation(5, population[x]) + " " + GetProductByLocation(7, population[x]) + " " + GetProductByLocation(9, population[x]) + " " + GetProductByLocation(11, population[x]) + "       ");
                    Console.WriteLine(GetProductByLocation(13, population[x]) + " " + GetProductByLocation(15, population[x]) + " " + GetProductByLocation(17, population[x]) + " " + GetProductByLocation(19, population[x]) + " " + GetProductByLocation(21, population[x]) + " " + GetProductByLocation(23, population[x]));

                    Console.Write(GetProductByLocation(1, population[x]) + " " + GetProductByLocation(3, population[x]) + "     " + GetProductByLocation(6, population[x]) + " " + GetProductByLocation(8, population[x]) + " " + GetProductByLocation(10, population[x]) + "       ");
                    Console.WriteLine(GetProductByLocation(12, population[x]) + " " + GetProductByLocation(14, population[x]) + " " + GetProductByLocation(16, population[x]) + " " + population[x][18] + " " + GetProductByLocation(20, population[x]) + " " + GetProductByLocation(22, population[x]));
                    Console.WriteLine();
               
                }

                //Array.Sort(FitnessProductPlacement);
                


                foreach (var chromosome in population)
                {
                    if (_random.Next(0, 1000) <= optimizationParameters.MutationProbability)
                    {
                        //Log.AddToLog($"MUTATION RSM\nBEFORE MUTATION({Distances.CalculatePathLengthDouble(chromosome)}): {string.Join(";",chromosome)}");

                        var j = _random.Next(1, Distances.WarehouseSize);
                        var i = _random.Next(1, j);
                        Array.Reverse(chromosome, i, j - i);

                        //Log.AddToLog($"AFTER MUTATION({Distances.CalculatePathLengthDouble(chromosome)}):  {string.Join(";",chromosome)}\n");
                    }
                }
                
                
                
                //miejsca w magazynie    0  1  2  3  4  5  6
                //produkty              [0  3  4  1  5  6  2]
            }
        }
        private static bool IsThereGene(int[] chromosome, int a)
        {
            return chromosome.Any(t => t == a);
        }

        private static int GetProductByLocation(int id, int[] chromosome)
        {
            int size = chromosome.Length;
            for (int i = 0; i < size; i++)
            {
                if (chromosome[i] == id) return i;
            }

            return -1;
        }

        private static void InitializePopulation(int[][] pop, int start)
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
                    int a = _random.Next(0, Distances.WarehouseSize);
                    if (!IsThereGene(temp, a))
                    {
                        temp[count] = a;
                        count++;
                    }
                } while (count < Distances.WarehouseSize);

                pop[i] = temp;
            }
        }
    }
}