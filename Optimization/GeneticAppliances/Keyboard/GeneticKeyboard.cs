using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Optimization.GeneticAlgorithms.Crossovers;
using Optimization.GeneticAlgorithms.Eliminations;
using Optimization.GeneticAlgorithms.Mutations;
using Optimization.GeneticAlgorithms.Selections;
using Optimization.GeneticAppliances;
using Optimization.Helpers;
using Optimization.Parameters;

namespace Optimization.GeneticAlgorithms
{
    class GeneticKeyboard : IGeneticAppliance
    {
        private int _geneLength;
        private int _populationSize;
        private int _terminationValue;
        private double _mutationProb;
        private Selection _selection;
        private Crossover _crossover;
        private Elimination _elimination;
        private Mutation _mutation;
        private int[][] _population;
        private int _childrenPerGeneration;

        private double[] _frequency = {
            8.167, 1.492, 2.782, 4.253, 12.702, 2.228, 2.015, 6.094, 6.966, 0.153, 0.772, 4.025,
            2.406, 6.749, 7.507, 1.929, 0.095, 5.987, 6.327, 9.056, 2.758, 0.978, 2.360, 0.150,
            1.974, 0.074, 1, 1, 0.1, 0.01
        };
        private double[] _weights = {
            4, 2, 2, 3, 4, 5, 3, 2, 2, 4,
            1.5, 1, 1, 1, 3, 3, 1, 1, 1, 1.5,
            4, 4, 3, 2, 5, 3, 2, 3, 4, 4
        };
        
        public GeneticKeyboard(OptimizationParameters optimizationParameters)
        {
            _geneLength = _frequency.Length;
            
            _mutationProb = optimizationParameters.MutationProbability;
            _terminationValue = optimizationParameters.TerminationValue;
            
            _populationSize = optimizationParameters.PopulationSize;
            _population = InitializeKeyboardPopulation(_populationSize);

            _childrenPerGeneration = optimizationParameters.ChildrenPerGeneration;
            
            _selection = GeneticFactory.CreateSelection(optimizationParameters, _population);
            _crossover = GeneticFactory.CreateCrossover(optimizationParameters, null);
            _elimination = GeneticFactory.CreateElimination(optimizationParameters, _population);
            _mutation = GeneticFactory.CreateMutation(optimizationParameters, _population, _mutationProb);
        }
        public int[] Run()
        {
            int[] bestGene = new int[_geneLength];
            
            for (int i = 0; i < _terminationValue; i++)
            {
                var fitness = new double[_populationSize];
                
                Parallel.For(0, _populationSize, j =>
                {
                    fitness[j] = Fitness.CalculateFitness(_population[j], _frequency, _weights);
                });
                var lastBestFitness = fitness.Min();
                bestGene = _population.First(p => Fitness.CalculateFitness(p, _frequency, _weights) == lastBestFitness);
                //_population = _population.OrderByDescending(x => fitness[Array.IndexOf(_population, x)]).ToArray();
                //fitness = fitness.OrderByDescending(x => x).ToArray();
                
                Console.WriteLine($"epoch: {i} best fitness: {fitness.Min()}, avg: {fitness.Average()}");
                var parents = _selection.GenerateParents(_childrenPerGeneration * 2, fitness);
                var offsprings = _crossover.GenerateOffsprings(parents);
                _elimination.EliminateAndReplace(offsprings, fitness);
                _mutation.Mutate();
            }

            Console.WriteLine(Fitness.CalculateFitness(bestGene, _frequency, _weights));
            return bestGene;

        }

        private int[][] InitializeKeyboardPopulation(int size)
        {
            var population = new int[size][];
            var available = new int[_geneLength];
            for (int i = 'A'; i <= 'Z' + 4; i++)
            {
                available[i - 'A'] = i;
            }

            for (int i = 0; i < size; i++)
            {
                population[i] = available.OrderBy(x => Guid.NewGuid()).ToArray();
            }
            return population;
        }
        
        public void WriteResult(int[] gene)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    switch (gene[10 * i + j])
                    {
                        case 91: sb.Append(". ");
                            break;
                        case 92: sb.Append(", ");
                            break;
                        case 93: sb.Append("; ");
                            break;
                        case 94: sb.Append("/ ");
                            break;
                        default: sb.Append((char) gene[10 * i + j] + " ");
                            break;
                    }
                }
                sb.Append(Environment.NewLine);
            }

            Console.WriteLine(sb.ToString());
        }
    }
}