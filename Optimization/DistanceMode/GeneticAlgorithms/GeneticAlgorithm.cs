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
        private int terminateAfterCount;
        private int[][] population;
        private double[][] _distancesMatrix;
        
        public delegate double[] CalcFitness(int[][] population, double[][] distancesMatrix);
        
        private CalcFitness _calculateFitness;
        
        public GeneticAlgorithm(OptimizationParameters optimizationParameters, double[][] distancesMatrix, CalcFitness calcFitness)
        {
            _optimizationParameters = optimizationParameters;
            _distancesMatrix = distancesMatrix;
            population = new int[_optimizationParameters.PopulationSize][];

            _crossover = Factory.CreateCrossover(optimizationParameters, _distancesMatrix);
            _selection = Factory.CreateSelection(optimizationParameters, population);
            _elimination = Factory.CreateElimination(optimizationParameters, population);
            _mutation = new InversionMutation(population, _optimizationParameters);
            
            _calculateFitness = calcFitness;
            
            terminateAfterCount = _optimizationParameters.TerminationValue;
        }
        

        public override int[] FindShortestPath(int[] order)
        {
            InitializePopulation(order);
            
            double lastBestFitness = population.Min(p => Distances.CalculatePathLengthDouble(p, _distancesMatrix));
            int[] bestGene = population.First(p => Distances.CalculatePathLengthDouble(p, _distancesMatrix) == lastBestFitness);

            double[] fitness = new double[population.Length];

            for (int b = 0; b < _optimizationParameters.TerminationValue; b++)
            {
                fitness = _calculateFitness(population, _distancesMatrix);

                int[][] parents = _selection.GenerateParents(_optimizationParameters.ChildrenPerGeneration*2,fitness);
                int[][] offsprings = _crossover.GenerateOffsprings(parents);
                _elimination.EliminateAndReplace(offsprings,fitness);
                if (_canIncreaseStrictness) _canIncreaseStrictness = _selection.IncreaseStrictness(_optimizationParameters.ChildrenPerGeneration);

                _mutation.Mutate();
                
            }
            
            int[] result = new int[bestGene.Length+1];
            for (int i = 0; i < bestGene.Length; i++)
            {
                result[i] = bestGene[i];
            }
            result[^2] = bestGene[0];
            if (_optimizationParameters.Use2opt)
            {
                Optimizer optimizer = new Optimizer();
                return optimizer.Optimize_2opt(result, _distancesMatrix);
            }
            return result;
        }

        private bool IsThereGene(int[] chromosome, int a)
        {
            return chromosome.Any(t => t == a);
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
                population[i] = temp;
            }

        }

    }
    
}
