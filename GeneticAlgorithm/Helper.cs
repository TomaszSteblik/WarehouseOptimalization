using System;
using System.Linq;

namespace GeneticAlgorithm
{
    public static class Helper
    {
        private static readonly int NumberOfCities;
        private static readonly int[][] Cities;
        static Helper()
        {
            NumberOfCities = GeneticAlgorithm.Source.Size;
            Cities = GeneticAlgorithm.Source.Data;
        }

        public static bool IsThereGene(int[] chromosome, int a)
        {
            return chromosome.Any(t => t == a);
        }
        
        public static int Fitness(int[] chromosome)
        {
            
            int sum = 0;
            int range = NumberOfCities - 1;
            for (int i = 0; i < range; i++)
            {
                sum += Cities[chromosome[i]][chromosome[i+1]];
            }
            sum += Cities[chromosome[0]][chromosome[range]]; //powrot do pierwszego miasta
            return sum;
        }
        
        public static void PrintChromosome(int[] chromosome)
        {
            foreach (var gene in chromosome)
            {
                Console.Write("{0} ",gene);
            }
        }

        public static double GetAverageOfPopulation(int[][] population)
        {
            double sum = 0;
            foreach (var chromosome in population)
            {
                sum += Fitness(chromosome);
            }
            return sum/population.Length;
        }

        
    }
}