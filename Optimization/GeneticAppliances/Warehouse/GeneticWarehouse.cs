using System;
using System.Linq;
using System.Threading;
using Optimization.GeneticAlgorithms;
using Optimization.GeneticAlgorithms.Crossovers;
using Optimization.GeneticAlgorithms.Eliminations;
using Optimization.GeneticAlgorithms.Mutations;
using Optimization.GeneticAlgorithms.Selections;
using Optimization.Helpers;
using Optimization.Parameters;

namespace Optimization.GeneticAppliances.Warehouse
{
    internal class GeneticWarehouse : IGeneticAppliance
    {

        private int _warehouseSize;
        private BaseGenetic _genetic;

        public GeneticWarehouse(OptimizationParameters optimizationParameters, int warehouseSize,
            DelegateFitness.CalcFitness calcFitness, CancellationToken ct)
        {
            _warehouseSize = warehouseSize;
            int[][] population = new int[optimizationParameters.PopulationSize][];
            int[] itemsToSort = new int[_warehouseSize];
            for (int i = 1; i < _warehouseSize; i++)
            {
                itemsToSort[i - 1] = i;
            }
            GeneticHelper.InitializePopulation(population, itemsToSort, 0, population.Length);
            _genetic = new BaseGenetic(optimizationParameters, population, calcFitness, ct);

        }

        public int[] Run()
        {
            return _genetic.OptimizeForBestIndividual();
        }

        
    }
}
