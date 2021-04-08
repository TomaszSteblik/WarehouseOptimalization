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
        public GeneticTSP(int[] order, OptimizationParameters parameters, DelegateFitness.CalcFitness calcFitness, CancellationToken ct)
        {
            int[][] population = new int[parameters.PopulationSize][];
            GeneticHelper.InitializePopulation(population, order, 0, parameters.PopulationSize);
            _genetic = new BaseGenetic(parameters, population, calcFitness, ct);
            
            _genetic.LoadModule(new TerminationModule());
            _genetic.LoadModule(new CataclysmModule());
            _genetic.LoadModule(new TSPModule());
        }
        

        public TSPResult Run()
        {
            _genetic.OptimizeForBestIndividual();
            var tsp = (TSPModule) _genetic.GetModule(typeof(TSPModule));
            var fitness = tsp.GetFitnessHistory();
            return new TSPResult(fitness);
        }
    }
}