using System;
using System.Linq;
using Optimization.GeneticAlgorithms.Crossovers;
using Optimization.GeneticAlgorithms.Eliminations;
using Optimization.GeneticAlgorithms.Mutations;
using Optimization.GeneticAlgorithms.Selections;
using Optimization.Helpers;
using Optimization.Parameters;
using Optimization.PathFinding;

namespace Optimization.GeneticAlgorithms
{
    internal class GeneticAlgorithmPathFinding : IGeneticAlgorithm, IPathFinder
    {
        private readonly Selection _selection;
        private readonly Crossover _crossover;
        private readonly Elimination _elimination;
        private readonly Mutation _mutation;
        private readonly Random _random = new Random();

        private bool _canIncreaseStrictness = true;
        private readonly int[][] _population;
        private readonly double[][] _distancesMatrix;

        private readonly int _populationSize;
        private readonly double _mutationProbability;
        private readonly int _childrenPerGeneration;
        private readonly int _terminationValue; 
        
        private DelegateFitness.CalcFitness _calculateFitness;

        private OptimizationParameters _optimizationParameters;
        
        public GeneticAlgorithmPathFinding(OptimizationParameters optimizationParameters, double[][] distancesMatrix, DelegateFitness.CalcFitness calcFitness)
        {
            _optimizationParameters = optimizationParameters;

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
        
        public void Run()
        {
        }

        public int[] FindShortestPath(int[] order)
        {
            InitializePopulation(order);
            
            double lastBestFitness = _population.Min(p => Fitness.CalculateFitness(p, _distancesMatrix));
            int[] bestGene = _population.First(p => Fitness.CalculateFitness(p, _distancesMatrix) == lastBestFitness);

            double[] fitness = new double[_population.Length];

            for (int b = 0; b < _terminationValue; b++)
            {
                fitness = _calculateFitness(_population, _distancesMatrix);

                int[][] parents = _selection.GenerateParents(_childrenPerGeneration * 2, fitness);
                int[][] offsprings = _crossover.GenerateOffsprings(parents);
                _elimination.EliminateAndReplace(offsprings, fitness);
                if (_canIncreaseStrictness) _canIncreaseStrictness = _selection.IncreaseStrictness(_childrenPerGeneration);

                _mutation.Mutate();
                bestGene = _population[0];

            }

            if (_optimizationParameters.Use2opt)
            {
                Optimizer2Opt optimizer2Opt = new Optimizer2Opt();
                return optimizer2Opt.Optimize(bestGene, _distancesMatrix);
            }

            return bestGene;
        }

        private void InitializePopulation(int[] order)
        {
            for (int i = 0; i < _populationSize; i++)
            {
                int[] temp = new int[order.Length];
                for (int z = 0; z < order.Length; z++)
                {
                    temp[z] = -1;
                }
                int count = 0;
                temp[0] = _optimizationParameters.StartingId;
                count++;
                do
                {
                    int a = _random.Next(0,order.Length);
                    if (temp.All(t => t != order[a]))
                    {
                        temp[count] = order[a];
                        count++;
                    }

                } while (count<order.Length);

                _population[i] = temp;
            }
        }

       
    }
}