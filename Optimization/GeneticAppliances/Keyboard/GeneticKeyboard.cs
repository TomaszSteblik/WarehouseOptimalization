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
        private BaseGenetic _genetic;

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
            int[][] population = InitializeKeyboardPopulation(_weights.Length);
            _genetic = new BaseGenetic(optimizationParameters, population, population =>
            {
                double[] fitness = new double[population.Length];
                for (int i = 0; i < population.Length; i++)
                {
                    fitness[i] = Fitness.CalculateFitness(population[i], _frequency, _weights);
                }
                return fitness;
            });

        }

        public int[] Run()
        {
            return _genetic.OptimizeForBestIndividual();
        }

        private int[][] InitializeKeyboardPopulation(int size)
        {
            var population = new int[size][];
            var available = new int[_frequency.Length];
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