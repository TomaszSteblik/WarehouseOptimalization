using System;
using System.Threading;
using Optimization.GeneticAlgorithms;
using Optimization.GeneticAlgorithms.Initialization;
using Optimization.Helpers;
using Optimization.Parameters;

namespace Optimization.PathFinding
{
    internal class GeneticPathFinding : IPathFinder
    {
        private BaseGenetic _genetic;
        public GeneticPathFinding(int[] order, OptimizationParameters parameters, DelegateFitness.CalcFitness calcFitness, CancellationToken ct, Random random)
        {
            var populationInitialization = new StandardPathInitialization(random);
            var population = populationInitialization.InitializePopulation(order, parameters.PopulationSize, 0);
            _genetic = new BaseGenetic(parameters, population, calcFitness, ct, random);
        }
        
        public int[] FindShortestPath(int[] order)
        {
            return _genetic.OptimizeForBestIndividual();
        }
    }
}