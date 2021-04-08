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
        private bool _use2opt = false;
        public GeneticTSP(int[] order, OptimizationParameters parameters, DelegateFitness.CalcFitness calcFitness, CancellationToken ct)
        {
            _use2opt = parameters.Use2opt;
            int[][] population = new int[parameters.PopulationSize][];
            GeneticHelper.InitializePopulation(population, order, 0, parameters.PopulationSize);
            _genetic = new BaseGenetic(parameters, population, calcFitness, ct);
            
            _genetic.LoadModule(new TerminationModule());
            _genetic.LoadModule(new CataclysmModule());
            _genetic.LoadModule(new TSPModule());
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