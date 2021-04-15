using System;
using System.Threading;
using System.Threading.Tasks;
using Optimization.GeneticAlgorithms;
using Optimization.GeneticAppliances;
using Optimization.GeneticAppliances.TSP;
using Optimization.GeneticAppliances.Warehouse;
using Optimization.Helpers;
using Optimization.Parameters;

namespace Optimization
{
    public static class OptimizationWork
    {
        public static double FindShortestPath(OptimizationParameters optimizationParameters)
        {
            var matrix = Files.ReadArray(optimizationParameters.DataPath);
            Distances.Create(matrix);
            return PathFinding.ShortestPath.Find(PointsArrayGenerator.GeneratePointsToVisit(matrix.Length), optimizationParameters, CancellationToken.None, random);
            
        }
        
<<<<<<< HEAD
        public static double FindShortestPath(OptimizationParameters optimizationParameters, CancellationToken ct)
=======
        public static double FindShortestPath(OptimizationParameters optimizationParameters, CancellationToken ct, int seed = 0)
>>>>>>> parent of d32bc4c (Merge branch 'master' into random)
        {
            var matrix = Files.ReadArray(optimizationParameters.DataPath);
            Distances.Create(matrix);
            return PathFinding.ShortestPath.Find(PointsArrayGenerator.GeneratePointsToVisit(matrix.Length), optimizationParameters, ct, random);
            
        }
        
<<<<<<< HEAD
        public static TSPResult TSP(OptimizationParameters optimizationParameters, CancellationToken ct)
=======
        public static TSPResult TSP(OptimizationParameters optimizationParameters, CancellationToken ct, int seed = 0)
>>>>>>> parent of d32bc4c (Merge branch 'master' into random)
        {
            var matrix = Files.ReadArray(optimizationParameters.DataPath);
            Distances.Create(matrix);
            var tsp = new GeneticTSP(PointsArrayGenerator.GeneratePointsToVisit(matrix.Length), optimizationParameters,
                (population) =>
                {
                    double[] fitness = new double[population.Length];
                    for (int i = 0; i < population.Length; i++)
                        fitness[i] = Fitness.CalculateFitness(population[i]);
                    return fitness;
<<<<<<< HEAD
                }, ct);
            return tsp.Run();
=======
                }, ct, random);
            var result = tsp.Run();
            result.Seed = seed;
            return result;
>>>>>>> parent of d32bc4c (Merge branch 'master' into random)
            
        }

        public static void FindShortestPath(OptimizationParameters optimizationParameters,
            DelegateFitness.CalcFitness calcFitness)
        {
            var matrix = Files.ReadArray(optimizationParameters.DataPath);
            Distances.Create(matrix);
            PathFinding.ShortestPath.Find(PointsArrayGenerator.GeneratePointsToVisit(matrix.Length), optimizationParameters,calcFitness, CancellationToken.None, random);
        }

<<<<<<< HEAD
        public static double WarehouseOptimization(WarehouseParameters warehouseParameters, CancellationToken ct)
=======
        public static double WarehouseOptimization(WarehouseParameters warehouseParameters, CancellationToken ct, int seed = 0)
>>>>>>> parent of d32bc4c (Merge branch 'master' into random)
        {
            return WarehouseOptimizer.Optimize(warehouseParameters, ct);
        }

        public static void KeyboardOptimization(OptimizationParameters optimizationParameters)
        {
<<<<<<< HEAD
            var keyboardOptimizer = new GeneticKeyboard(optimizationParameters);
=======
            seed = GetSeed(seed);
            var random = new Random(seed);
            var keyboardOptimizer = new GeneticKeyboard(optimizationParameters, random);
>>>>>>> parent of d32bc4c (Merge branch 'master' into random)
            var result = keyboardOptimizer.Run();
            keyboardOptimizer.WriteResult(result);
        }
    }
}