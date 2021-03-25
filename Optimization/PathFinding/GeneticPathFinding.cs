using Optimization.GeneticAlgorithms;
using Optimization.Helpers;
using Optimization.Parameters;

namespace Optimization.PathFinding
{
    internal class GeneticPathFinding : IPathFinder
    {
        private BaseGenetic _genetic;
        public GeneticPathFinding(int[] order, OptimizationParameters parameters, DelegateFitness.CalcFitness calcFitness)
        {
            int[][] population = new int[parameters.PopulationSize][];
            GeneticHelper.InitializePopulation(population, order, 0, parameters.PopulationSize);
            _genetic = new BaseGenetic(parameters, population, calcFitness);
        }
        
        public int[] FindShortestPath(int[] order)
        {
            return _genetic.OptimizeForBestIndividual();
        }
    }
}