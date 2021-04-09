using System;
using System.Linq;
using System.Threading;
using Optimization.GeneticAlgorithms;
using Optimization.GeneticAlgorithms.Modules;
using Optimization.Helpers;
using Optimization.Parameters;

namespace Optimization.GeneticAppliances.TSP
{
    public class GeneticTSP
    {
        private BaseGenetic _genetic;
        private bool _use2opt;
        public GeneticTSP(int[] order, OptimizationParameters parameters, DelegateFitness.CalcFitness calcFitness, CancellationToken ct)
        {
            _use2opt = parameters.Use2opt;
            var population = GeneratePopulation(order, parameters.PopulationSize, parameters.StartingId);
            _genetic = new BaseGenetic(parameters, population, calcFitness, ct);
            
            _genetic.LoadModule(new TerminationModule());
            _genetic.LoadModule(new CataclysmModule(GeneratePopulation));
            _genetic.LoadModule(new TSPModule());
        }

        private int[][] GeneratePopulation(int[] pointsToInclude, int populationSize, int startingPoint)
        {
            var population = new int[populationSize][];
            for (int i = 0; i < populationSize; i++)
            {
                var availablePoints = pointsToInclude.Except(new [] {startingPoint});
                var unit = new[] {startingPoint};
                population[i] = unit.Concat(availablePoints.OrderBy(x => Guid.NewGuid())).ToArray();
            }

            return population;
        }
        

        public TSPResult Run()
        {
            var result = _genetic.OptimizeForBestIndividual();
            if (_use2opt) result = Optimizer2Opt.Optimize(result);
            var tsp = (TSPModule) _genetic.GetModule(typeof(TSPModule));
            var fitness = tsp.GetFitnessHistory();
            return new TSPResult(fitness, result);
        }
    }
}