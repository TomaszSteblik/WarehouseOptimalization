using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GeneticAlgorithm
{
    public class GeneticAlgorithm
    {
        public static Source Source;
        private Selection _selection;
        private Crossover _crossover;
        private Elimination _elimination;
        private readonly Random _random = new Random();
        
        
        
        public void Go(int populationSize,int childrenPerGeneration,string path)
        {
            Source = new TxtFileSource(path);
            _crossover = new HGreXCrossover();
            
            
            int[][] population = new int[populationSize][];
            InitializePopulation(population);
            
            _selection = new TournamentSelection(population);
            _elimination = new RouletteWheelElimination(ref population);

            bool canIncreaseStrictness = true;
            bool canMutate = true;
            
            Queue<double> archive = new Queue<double>(10);
            archive.Enqueue(-5);
            
            do
            {
                int[][] parents = _selection.GenerateParents(childrenPerGeneration*2);
                int[][] offsprings = _crossover.GenerateOffsprings(parents);
                _elimination.EliminateAndReplace(offsprings);

                if (canIncreaseStrictness) canIncreaseStrictness = _selection.IncreaseStrictness(childrenPerGeneration);

                if (canMutate)
                {
                    foreach (var chromosome in population)
                    {
                        if (_random.Next(0, 10000) <= 30)
                        {
                            var a = _random.Next(1, Source.Size);
                            var b = _random.Next(1, Source.Size);
                            var temp = chromosome[a];
                            chromosome[a] = chromosome[b];
                            chromosome[b] = temp;
                        }
                    }
                }
                
                
                
                //Array.Sort(population,(a,b)=>Helper.Fitness(a) - Helper.Fitness(b));
                //Helper.PrintChromosome(population[0]);
                Console.Write(" {0}  ",Helper.GetAverageOfPopulation(population));
                Console.WriteLine(Helper.Fitness(population[0]));
                
                archive.Enqueue(Helper.GetAverageOfPopulation(population));
                Console.WriteLine(archive.Average());
                if (archive.Max() - archive.Average() <= 5)
                {
                    break;
                }

            } while (true);

            Array.Sort(population,(a,b)=>Helper.Fitness(a) - Helper.Fitness(b));
            Helper.PrintChromosome(population[0]);
            Console.WriteLine(Helper.Fitness(population[0]));
        }

        private void InitializePopulation(int[][] population)
        {
            int populationSize = population.Length;
            for (int i = 0; i < populationSize; i++)
            {
                int[] temp = new int[Source.Size];
                for (int z = 0; z < Source.Size; z++)
                {
                    temp[z] = -1;
                }
                int count = 0;
                temp[0] = 0;
                count++;
                do
                {
                    int a = _random.Next(0,Source.Size);
                    if (!Helper.IsThereGene(temp,a))
                    {
                        temp[count] = a;
                        count++;
                    }
                } while (count<Source.Size);
                population[i] = temp;
            }
        }
        
    }
}


//
