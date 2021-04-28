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
        public Crossover Crossover { get => _crossover; }
        private readonly ConflictResolver _resolverConflict;
        private readonly ConflictResolver _resolverRandomized;
        private readonly Elimination _elimination;
        private readonly Mutation _mutation;
        
        private bool _canIncreaseStrictness = true;
        private readonly double _mutationProbability;
        private readonly int _childrenPerGeneration;
        private readonly int _terminationValue;
        private readonly int _parentsPerChild;
        private bool _incrementMutation = false;
        private double _incrementMutationDelta = 0;
        private double _incrementMutationEveryEpochs;

        
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
            return _modules.FirstOrDefault(x => x.GetType() == type);
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
            
            _incrementMutation = parameters.IncrementMutationEnabled; 
            _incrementMutationDelta = parameters.IncrementMutationDelta;
            _incrementMutationEveryEpochs = parameters.IncrementMutationEpochs;

            _calculateFitness = calculateFitness;

            _selection = GeneticFactory.CreateSelection(parameters, _population, random);
            
            _resolverConflict = GeneticFactory.CreateConflictResolver(parameters, parameters.ConflictResolveMethod, random);
            _resolverRandomized = GeneticFactory.CreateConflictResolver(parameters, parameters.RandomizedResolveMethod, random);
            
            _crossover = GeneticFactory.CreateCrossover(parameters.StartingId,parameters.CrossoverMethod,
                parameters.MultiCrossovers, _resolverConflict, _resolverRandomized, random, parameters.MutateParentIfTheSame);
            _elimination = GeneticFactory.CreateElimination(parameters, _population, random);
            _mutation = GeneticFactory.CreateMutation(parameters.MutationMethod,parameters.MultiMutations, _population,
                _mutationProbability, random);
        }

        public int[] OptimizeForBestIndividual()
        {
            fitness = new double[_population.Length];
            int[] bestGene = new int[_population[0].Length];
            var termination = (TerminationModule) GetModule(typeof(TerminationModule));
            
            for (int b = 0; b < _terminationValue; b++)
            {
                if (_ct.IsCancellationRequested)
                {
                    _ct.ThrowIfCancellationRequested();
                }


                if (b % _incrementMutationEveryEpochs == 0)
                    _mutation._mutationProbability += _incrementMutationDelta;

                if(termination is not null)
                    if (termination.RequestedStop) return bestGene;
                    
                OnNextIteration?.Invoke(this,b);
                    
                fitness = _calculateFitness(_population);
                Array.Sort(fitness,_population);

                var bestGeneCopy = (int[]) _population[0].Clone(); //best gene can be changed by crossover, because it mutates genes when both parents are the same
                var bestFitness = fitness[0];

                int[][] parents = _selection.GenerateParents(_childrenPerGeneration * 2, fitness);
                int[][] offsprings = _crossover.GenerateOffsprings(parents, _parentsPerChild);

                _population[0] = bestGeneCopy; 
                fitness[0] = bestFitness;
                
                RunModules();
                
                _elimination.EliminateAndReplace(offsprings, fitness);
                _mutation.Mutate(_population);
                
                

                bestGene = _population[0];

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
                case "crossover" : return _crossover;
                case "fitness": return fitness;
                case "population": return _population;
                default: return null;
            }
        }
    }
}