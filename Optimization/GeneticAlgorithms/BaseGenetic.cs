using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Optimization.GeneticAlgorithms.Crossovers;
using Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers;
using Optimization.GeneticAlgorithms.Eliminations;
using Optimization.GeneticAlgorithms.Modules;
using Optimization.GeneticAlgorithms.Mutations;
using Optimization.GeneticAlgorithms.Selections;
using Optimization.GeneticAppliances.TSP;
using Optimization.Helpers;
using Optimization.Parameters;

namespace Optimization.GeneticAlgorithms
{
    public class BaseGenetic
    {
        private readonly Selection _selection;
        private readonly Crossover _crossover;
        private readonly ConflictResolver _resolverConflict;
        private readonly ConflictResolver _resolverRandomized;
        private readonly Elimination _elimination;
        private readonly Mutation _mutation;
        
        private bool _canIncreaseStrictness = true;
        private readonly double _mutationProbability;
        private readonly int _childrenPerGeneration;
        private readonly int _terminationValue;
        private readonly int _parentsPerChild;

        
        private DelegateFitness.CalcFitness _calculateFitness;

        private int[][] _population;

        private readonly CancellationToken _ct;

        public static event EventHandler<int> OnNextIteration; 

        private List<IModule> _modules;
        private double[] fitness;

        public void LoadModule(IModule module)
        {
            _modules.Add(module);
        }

        public IModule GetModule(Type type)
        {
            return _modules.First(x => x.GetType() == type);
        }

        public BaseGenetic(OptimizationParameters parameters, int[][] population,
            DelegateFitness.CalcFitness calculateFitness, CancellationToken ct, Random random)
        {
            _ct = ct;
            _modules = new List<IModule>();
            
            _population = population;

            _mutationProbability = parameters.MutationProbability;
            _childrenPerGeneration = parameters.ChildrenPerGeneration;
            _terminationValue = parameters.MaxEpoch;
            _parentsPerChild = parameters.ParentsPerChildren;

            _calculateFitness = calculateFitness;

            _selection = GeneticFactory.CreateSelection(parameters, _population, random);
            
            _resolverConflict = GeneticFactory.CreateConflictResolver(parameters, parameters.ConflictResolveMethod, random);
            _resolverRandomized = GeneticFactory.CreateConflictResolver(parameters, parameters.RandomizedResolveMethod, random);
            
            _crossover = GeneticFactory.CreateCrossover(parameters.StartingId,parameters.CrossoverMethod,
                parameters.MultiCrossovers, _resolverConflict, _resolverRandomized, random);
            _elimination = GeneticFactory.CreateElimination(parameters, _population, random);
            _mutation = GeneticFactory.CreateMutation(parameters.MutationMethod,parameters.MultiMutations, _population,
                _mutationProbability, random);
        }

        public int[] OptimizeForBestIndividual()
        {
            fitness = new double[_population.Length];
            int[] bestGene = new int[_population[0].Length];

            try
            {
                for (int b = 0; b < _terminationValue; b++)
                {
                    if (_ct.IsCancellationRequested)
                    {
                        _ct.ThrowIfCancellationRequested();
                    }
                    
                    OnNextIteration?.Invoke(this,b);
                    
                    fitness = _calculateFitness(_population);

                    Array.Sort(fitness,_population);

                    RunModules();
                
                    int[][] parents = _selection.GenerateParents(_childrenPerGeneration * 2, fitness);
                    int[][] offsprings = _crossover.GenerateOffsprings(parents, _parentsPerChild);
                    
                    var tsp = (TSPModule) GetModule(typeof(TSPModule));
                    tsp?.AddResolveCount(_crossover.ResolveCount);
                    tsp?.AddRandomizedResolveCount(_crossover.RandomizedResolvesCount);
                    tsp?.AddConflictResolvesPercent(100.0 * _crossover.ResolveCount / _crossover.RandomizationChances);
                    tsp?.AddRandomResolvesPercent(100.0 * _crossover.RandomizedResolvesCount / _crossover.RandomizationChances);
                    
                    _elimination.EliminateAndReplace(offsprings, fitness);
                    _mutation.Mutate(_population);

                    bestGene = _population[0];

                }
            }
            catch (GeneticModuleExit)
            {
                return bestGene;
            }
            
            return bestGene;
            
        }

        private void RunModules()
        {
            for (int i = 0; i < _modules.Count; i++)
            {
                IModule currentModule = _modules[i];
                object obj = GetObject(currentModule.GetDesiredObject());
                currentModule.RunAction(obj);
            }
        }

        private object GetObject(string name)
        {
            switch (name)
            {
                case "fitness": return fitness;
                case "population": return _population;
                default: return null;
            }
        }
    }
}