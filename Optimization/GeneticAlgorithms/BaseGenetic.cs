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
    public class BaseGenetic
    {
        private readonly Selection _selection;
        private readonly Crossover _crossover;
        private readonly Elimination _elimination;
        private readonly Mutation _mutation;
        private readonly Random _random = new Random();
        
        private bool _canIncreaseStrictness = true;
        private readonly int _populationSize;
        private readonly double _mutationProbability;
        private readonly int _childrenPerGeneration;
        private readonly int _terminationValue;
        private readonly int _parentsPerChild;

        private readonly bool _writeCsv;
        
        private DelegateFitness.CalcFitness _calculateFitness;

        private int[][] _population;

        public BaseGenetic(OptimizationParameters parameters, int[][] population,
            DelegateFitness.CalcFitness calculateFitness)
        {
            _population = population;
            _populationSize = population.Length;

            _writeCsv = parameters.WriteCsv;

            _mutationProbability = parameters.MutationProbability;
            _childrenPerGeneration = parameters.ChildrenPerGeneration;
            _terminationValue = parameters.TerminationValue;
            _parentsPerChild = parameters.ParentsPerChildren;

            _calculateFitness = calculateFitness;

            _selection = GeneticFactory.CreateSelection(parameters, _population);
            _crossover = GeneticFactory.CreateCrossover(parameters.StartingId,parameters.CrossoverMethod,
                parameters.MultiCrossovers);
            _elimination = GeneticFactory.CreateElimination(parameters, _population);
            _mutation = GeneticFactory.CreateMutation(parameters.MutationMethod,parameters.MultiMutations, _population,
                _mutationProbability);
        }

        public int[] OptimizeForBestIndividual()
        {
            double[] fitness = new double[_population.Length];
            int[] bestGene = new int[_population[0].Length];
            EpochFitness epochFitness = null;
            if(_writeCsv) epochFitness = new EpochFitness("fitness.csv");
            
            for (int b = 0; b < _terminationValue; b++)
            {
                fitness = _calculateFitness(_population);
                int[][] parents = _selection.GenerateParents(_childrenPerGeneration * 2, fitness);
                int[][] offsprings = _crossover.GenerateOffsprings(parents, _parentsPerChild);
                _elimination.EliminateAndReplace(offsprings, fitness);
                if (_canIncreaseStrictness)
                    _canIncreaseStrictness = _selection.IncreaseStrictness(_childrenPerGeneration);
                
                Array.Sort(fitness,_population);
                _mutation.Mutate(_population);

                epochFitness?.AddLine(fitness.Min(), fitness.Max(), fitness.Average());

                bestGene = _population[0];

            }
            
            return bestGene;
        }
    }
}