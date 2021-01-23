using System;
using System.Linq;
using System.Threading.Tasks;
using Optimization.DistanceMode.GeneticAlgorithms.Crossovers;
using Optimization.DistanceMode.GeneticAlgorithms.Eliminations;
using Optimization.DistanceMode.GeneticAlgorithms.Mutations;
using Optimization.DistanceMode.GeneticAlgorithms.Selections;

namespace Optimization.DistanceMode.GeneticAlgorithms
{
    public class GeneticAlgorithm : Optimization
    {
        private readonly Selection _selection;
        private readonly Crossover _crossover;
        private readonly Elimination _elimination;
        private readonly Mutation _mutation;
        private readonly Random _random = new Random();

        private bool _canIncreaseStrictness = true;
        private bool canMutate;
        private int[][] _population;
        private double[][] _distancesMatrix;
        
        public delegate double[] CalcFitness(int[][] population, double[][] distancesMatrix);
        
        private CalcFitness _calculateFitness;
        
        public GeneticAlgorithm(OptimizationParameters optimizationParameters, double[][] distancesMatrix, CalcFitness calcFitness)
        {
            _optimizationParameters = optimizationParameters;
            _distancesMatrix = distancesMatrix;
            _population = new int[_optimizationParameters.PopulationSize][];

            _crossover = Factory.CreateCrossover(optimizationParameters, _distancesMatrix);
            _selection = Factory.CreateSelection(optimizationParameters, _population);
            _elimination = Factory.CreateElimination(optimizationParameters, _population);
            _mutation = new InversionMutation(_population, _optimizationParameters);
            
            _calculateFitness = calcFitness;
        }
        

        public override int[] FindShortestPath(int[] order)
        {
            InitializePopulation(order);
            
            double lastBestFitness = _population.Min(p => Distances.CalculatePathLengthDouble(p, _distancesMatrix));
            int[] bestGene = _population.First(p => Distances.CalculatePathLengthDouble(p, _distancesMatrix) == lastBestFitness);

            double[] fitness = new double[_population.Length];

            for (int b = 0; b < _optimizationParameters.TerminationValue; b++)
            {
                fitness = _calculateFitness(_population, _distancesMatrix);

                int[][] parents = _selection.GenerateParents(_optimizationParameters.ChildrenPerGeneration*2, fitness);
                int[][] offsprings = _crossover.GenerateOffsprings(parents);
                _elimination.EliminateAndReplace(offsprings,fitness);
                if (_canIncreaseStrictness) _canIncreaseStrictness = _selection.IncreaseStrictness(_optimizationParameters.ChildrenPerGeneration);

                _mutation.Mutate();
                bestGene = _population[0];

            }

            if (_optimizationParameters.Use2opt && _optimizationParameters.Mode == Mode.DistancesMode)
            {
                Optimizer optimizer = new Optimizer();
                return optimizer.Optimize_2opt(bestGene, _distancesMatrix);
            }

            return bestGene;
        }

        private void InitializePopulation(int[] order)
        {
            for (int i = 0; i < _optimizationParameters.PopulationSize; i++)
            {
                int[] temp = new int[order.Length];
                for (int z = 0; z < order.Length; z++)
                {
                    temp[z] = -1;
                }
                int count = 0;
                temp[0] = 0;
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
