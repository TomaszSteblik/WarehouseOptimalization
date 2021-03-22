using System;
using System.Linq;
using Optimization.GeneticAlgorithms.Crossovers;
using Optimization.GeneticAlgorithms.Eliminations;
using Optimization.GeneticAlgorithms.Mutations;
using Optimization.GeneticAlgorithms.Selections;
using Optimization.Helpers;
using Optimization.Parameters;

namespace Optimization.GeneticAlgorithms
{
    internal class GeneticWarehouse : IGeneticAppliance
    {

        private readonly Selection _selection;
        private readonly Crossover _crossover;
        private readonly Elimination _elimination;
        private readonly Mutation _mutation;
        private readonly Random _random = new Random();

        private bool _canIncreaseStrictness = true;
        private int[][] _population;
        private readonly double[][] _distancesMatrix;

        private readonly int _populationSize;
        private readonly double _mutationProbability;
        private readonly int _childrenPerGeneration;
        private readonly int _terminationValue;

        private DelegateFitness.CalcFitness _calculateFitness;

        private OptimizationParameters _optimizationParameters;
        private int _warehouseSize;

        public GeneticWarehouse(OptimizationParameters optimizationParameters, int warehouseSize, double[][] distancesMatrix,
            DelegateFitness.CalcFitness calcFitness)
        {
            _optimizationParameters = optimizationParameters;
            _warehouseSize = warehouseSize;

            _populationSize = optimizationParameters.PopulationSize;
            _mutationProbability = optimizationParameters.MutationProbability;
            _childrenPerGeneration = optimizationParameters.ChildrenPerGeneration;
            _terminationValue = optimizationParameters.TerminationValue;

            _distancesMatrix = distancesMatrix;
            _population = new int[_populationSize][];

            _crossover = GeneticFactory.CreateCrossover(optimizationParameters, _distancesMatrix);
            _selection = GeneticFactory.CreateSelection(optimizationParameters, _population);
            _elimination = GeneticFactory.CreateElimination(optimizationParameters, _population);
            _mutation = GeneticFactory.CreateMutation(optimizationParameters, _population, _mutationProbability);

            _calculateFitness = calcFitness;
        }

        public int[] Run()
        {
            int[] itemsToSort = new int[_warehouseSize];
            for (int i = 1; i < _warehouseSize; i++)
            {
                itemsToSort[i - 1] = i;
            }
            GeneticHelper.InitializePopulation(_population, itemsToSort, 0, _populationSize);
            
            double lastBestFitness = _population.Min(p => Fitness.CalculateFitness(p, _distancesMatrix));
            int[] bestGene = _population.First(p => Fitness.CalculateFitness(p, _distancesMatrix) == lastBestFitness);

            double[] fitness = new double[_population.Length];

            for (int b = 0; b < _terminationValue; b++)
            {
                fitness = _calculateFitness(_population, _distancesMatrix);

                int[][] parents = _selection.GenerateParents(_childrenPerGeneration * 2, fitness);
                int[][] offsprings = _crossover.GenerateOffsprings(parents);
                _elimination.EliminateAndReplace(offsprings, fitness);
                if (_canIncreaseStrictness)
                    _canIncreaseStrictness = _selection.IncreaseStrictness(_childrenPerGeneration);

                _mutation.Mutate();
                bestGene = _population[0];

            }

            return bestGene;
        }

        
    }
}
