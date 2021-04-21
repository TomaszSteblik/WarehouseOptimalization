using System;
using System.Linq;
using System.Threading;
using Optimization.GeneticAlgorithms;
using Optimization.GeneticAlgorithms.Initialization;
using Optimization.GeneticAlgorithms.Modules;
using Optimization.Helpers;
using Optimization.Parameters;

namespace Optimization.GeneticAppliances.TSP
{
    public class GeneticTSP
    {
        private BaseGenetic _genetic;
        private bool _use2opt;
        private TSPModule _tspModule;
        public GeneticTSP(int[] order, OptimizationParameters parameters, DelegateFitness.CalcFitness calcFitness, CancellationToken ct, Random random)
        {
            _use2opt = parameters.Use2opt;
            var populationInitialization =
                GeneticFactory.CreatePopulationInitialization(parameters.PopulationInitializationMethod, random);
            var population = populationInitialization.InitializePopulation(order, parameters.PopulationSize, parameters.StartingId);
            _genetic = new BaseGenetic(parameters, population, calcFitness, ct, random);
            
            if(parameters.StopAfterEpochsWithoutChange)
                _genetic.LoadModule(new TerminationModule(parameters.StopAfterEpochCount));
            if(parameters.EnableCataclysm)
                _genetic.LoadModule(new CataclysmModule(populationInitialization, parameters.DeathPercentage, parameters.CataclysmEpoch));

            _tspModule = new TSPModule();
            _tspModule.LoadCrossoverOperator(_genetic.Crossover);
            _genetic.LoadModule(_tspModule);
        }

        

        public TSPResult Run()
        {
            var result = _genetic.OptimizeForBestIndividual();
            if (_use2opt) result = Optimizer2Opt.Optimize(result);
            var fitness = _tspModule.GetFitnessHistory();
            return new TSPResult(fitness, result)
            {
                ResolveInEpoch = _tspModule.ResolveCountInEpoch,
                RandomizedResolveInEpoch = _tspModule.RandomizedResolveCountInEpoch,
                ResolvePercentInEpoch = _tspModule.ConflictResolvesPercent,
                RandomizedResolvePercentInEpoch = _tspModule.RandomResolvesPercent
            };
        }
    }
}