using System;
using System.Linq;
using System.Threading.Tasks;
using Optimization.DistanceMode;
using Optimization.DistanceMode.GeneticAlgorithms.Crossovers;
using Optimization.DistanceMode.GeneticAlgorithms.Eliminations;
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
            Log.Create(optimizationParameters.LogPath);

            var distancesMatrix = WarehouseManager.CreateWarehouseDistancesMatrix(optimizationParameters.WarehousePath);
            Orders orders = new Orders(optimizationParameters.OrdersPath);
            int populationSize = optimizationParameters.PopulationSize;
            int[][] population = new int[populationSize][];
            InitializePopulation(population, 0);

            //AEX
            for (int e = 0; e < optimizationParameters.TerminationValue; e++) //opt. rozkładu produktów
            {
                double[] FitnessProductPlacement = new double[populationSize];

                //fiteness
                Parallel.For(0, populationSize, i =>
                {
                    for (int k = 0; k < orders.OrdersCount; k++)
                    {
                        int[] order = Translator.TranslateWithChromosome(orders.OrdersList[k], population[i]);
                        double pathLength = FindShortestPath.Find(order, distancesMatrix, optimizationParameters);
                        FitnessProductPlacement[i] += pathLength * orders.OrderRepeats[k];
                    }

                });

                if (e == 0)
                {
                    E = FitnessProductPlacement.Min();
                    double E2 = FitnessProductPlacement.Average();  
                    string log1 = "\r\nepoch=start  minSumDist=" + E + " avgSumDist=" + E2 + "\r\n";
                    Console.WriteLine(log1);
                    //System.IO.File.AppendAllText(@"E:\Warehouse\WarehouseOptimization\logV2-b.txt", log1);
                }



                Log.AddToLog("Populacja nr." + e);
                Log.AddToLog("avg: " + FitnessProductPlacement.Average());
                Log.AddToLog("max: " + FitnessProductPlacement.Max());
                Log.AddToLog("min: " + FitnessProductPlacement.Min());
                for (int i = 0; i < populationSize; i++)
                {
                    Log.AddToLog($"Chromosome({FitnessProductPlacement[i]}): {string.Join(";", population[i])} \n");
                }

                //selekcja
                Selection selection;
                switch (optimizationParameters.SelectionMethod)
                {
                    case "Tournament":
                        selection = new TournamentSelection(population);
                        break;
                    case "Random":
                        selection = new RandomSelection(population);
                        break;
                    case "Elitism":
                        selection = new ElitismSelection(population);
                        break;
                    case "RouletteWheel":
                        selection = new RouletteWheelSelection(population);
                        break;
                    default:
                        throw new ArgumentException("Wrong selection name in parameters json file");
                }
                int[][] parents = selection.GenerateParents(optimizationParameters.ChildrenPerGeneration * 2, FitnessProductPlacement);

                //krzyżowanie
                Crossover crossover = new AexCrossover(distancesMatrix);
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

                Array.Sort(FitnessProductPlacement, population);
                if (e % 10 == 0 || e < 10)
                {
                    if (e > 10 && optimizationParameters.MutationProbability < 0.1)
                        optimizationParameters.MutationProbability *= 1.2;

                    E = FitnessProductPlacement.Min();
                    double E2 = FitnessProductPlacement.Average();
                    int x = 0;

                    //Event1?.Invoke(null, null);
                    //Console.WriteLine(newFitness[x]+"     10-11-12-13-14-15");
                    Console.WriteLine(E);
                   

                    string log1 = "\r\nepoch=" + e + " minSumDist=" + E + " avgSumDist=" + E2 + "\r\n"
                    + population[x][2] + " " + population[x][4] + " " + population[x][5] + " " + population[x][7] + " " + population[x][9] + " " + population[x][11] + "       "
                    + population[x][13] + " " + population[x][15] + " " + population[x][17] + " " + population[x][19] + " " + population[x][21] + " " + population[x][23] + "\r\n"
                    + population[x][1] + " " + population[x][3] + "     " + population[x][6] + " " + population[x][8] + " " + population[x][10] + "       "
                    + population[x][12] + " " + population[x][14] + " " + population[x][16] + " " + population[x][18] + " " + population[x][20] + " " + population[x][22] + "\r\n";
                    Console.WriteLine(log1);
                    //System.IO.File.AppendAllText(@"E:\Warehouse\WarehouseOptimization\logV2-b.txt", log1);

                }

                for (int m = (int)(0.1 * populationSize); m < populationSize; m++)

                //  foreach (var chromosome in population)
                {
                    if (_random.NextDouble() <= optimizationParameters.MutationProbability)
                    {
                        //Log.AddToLog($"MUTATION RSM\nBEFORE MUTATION({Distances.CalculatePathLengthDouble(chromosome)}): {string.Join(";",chromosome)}");

                        var j = _random.Next(1, WarehouseManager.GetInstance().WarehouseSize);
                        var i = _random.Next(1, j);
                        Array.Reverse(population[m], i, j - i);

                        //Log.AddToLog($"AFTER MUTATION({Distances.CalculatePathLengthDouble(chromosome)}):  {string.Join(";",chromosome)}\n");
                    }
                }
            }
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