using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using OptimizationIO;

namespace GeneticAlgorithm
{
    public class GeneticAlgorithm : Optimization.Optimization
    {
        public static Source Source;
        private Selection _selection;
        private Crossover _crossover;
        private Elimination _elimination;
        private readonly Random _random = new Random();
        private readonly OptimizationParameters Parameters;

        public GeneticAlgorithm(OptimizationParameters parameters)
        {
            Parameters = parameters;
        }


        public override int[] FindShortestPath(int start)
        {
            Source = new TxtFileSource(Parameters.DataPath);
            _crossover = new HGreXCrossover();
            
            
            int[][] population = new int[Parameters.PopulationSize][];
            InitializePopulation(population,start);
            
            _selection = new TournamentSelection(population);
            _elimination = new RouletteWheelElimination(ref population);

            bool canIncreaseStrictness = true;
            bool canMutate = Parameters.CanMutate; //true
            int terminateAfterCount = Parameters.TerminationValue; //10000
            
            
            int lastBestFitness = population.Min(p => Helper.Fitness(p));
            int[] bestGene = population.First(p => Helper.Fitness(p) == lastBestFitness);
            int countToTerminate = terminateAfterCount;
            
            do
            {
                int[][] parents = _selection.GenerateParents(Parameters.ChildrenPerGeneration*2);
                int[][] offsprings = _crossover.GenerateOffsprings(parents);
                _elimination.EliminateAndReplace(offsprings);

                if (canIncreaseStrictness) canIncreaseStrictness = _selection.IncreaseStrictness(Parameters.ChildrenPerGeneration);

                if (canMutate)
                {
                    foreach (var chromosome in population)
                    {
                        if (_random.Next(0, 1000) <= 5)
                        {
                            var a = _random.Next(1, Source.Size);
                            var b = _random.Next(1, Source.Size);
                            var temp = chromosome[a];
                            chromosome[a] = chromosome[b];
                            chromosome[b] = temp;
                        }
                    }
                }


                int currentBestFitness = population.Min(p => Helper.Fitness(p));
                
                if (lastBestFitness <= currentBestFitness)
                {
                    countToTerminate--;
                }
                else
                {
                    lastBestFitness = currentBestFitness;
                    bestGene = population.First(p => Helper.Fitness(p) == lastBestFitness);
                    countToTerminate = terminateAfterCount;
                }
                
                
            } while (countToTerminate >0);
            int[] result = new int[bestGene.Length+1];
            for (int i = 0; i < bestGene.Length; i++)
            {
                result[i] = bestGene[i];
            }
            result[result.Length - 1] = bestGene[0];
            return result;
        }

        private void InitializePopulation(int[][] population,int start)
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
                temp[0] = start;
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
