using System;
using System.Linq;
using OptimizationIO;

namespace GeneticAlgorithm
{
    public class GeneticAlgorithm : OptimizationIO.Optimization
    {
        public static Source Source;
        private Selection _selection;
        private Crossover _crossover;
        private Elimination _elimination;
        private readonly Random _random = new Random();
        private readonly OptimizationParameters _parameters;

        public GeneticAlgorithm(OptimizationParameters parameters)
        {
            _parameters = parameters;
        }


        public override int[] FindShortestPath(int start)
        {
            Source = new TxtFileSource(_parameters.DataPath);

            _crossover = _parameters.CrossoverMethod switch
            {
                "Aex" => new AexCrossover(),
                "HGreX" => new HGreXCrossover(),
                _ => throw new ArgumentException("Wrong crossover name in parameters json file")
            };

            int[][] population = new int[_parameters.PopulationSize][];
            InitializePopulation(population,start);
            
            switch (_parameters.SelectionMethod)
            {
                case "Tournament":
                    _selection = new TournamentSelection(population);
                    break;
                case "Elitism":
                    _selection = new ElitismSelection(population);
                    break;
                case "RouletteWheel":
                    _selection = new RouletteWheelSelection(population);
                    break;
                default:
                    throw new ArgumentException("Wrong selection name in parameters json file");
            }
            
            switch (_parameters.EliminationMethod)
            {
                case "Elitism":
                    _elimination = new ElitismElimination(ref population);
                    break;
                case "RouletteWheel":
                    _elimination = new RouletteWheelElimination(ref population);
                    break;
                default:
                    throw new ArgumentException("Wrong elimination name in parameters json file");
            }
            
            bool canIncreaseStrictness = true;
            bool canMutate = _parameters.CanMutate; //true
            int terminateAfterCount = _parameters.TerminationValue; //10000
            
            
            int lastBestFitness = population.Min(p => Helper.Fitness(p));
            int[] bestGene = population.First(p => Helper.Fitness(p) == lastBestFitness);
            int countToTerminate = terminateAfterCount;
            
            do
            {
                int[][] parents = _selection.GenerateParents(_parameters.ChildrenPerGeneration*2);
                int[][] offsprings = _crossover.GenerateOffsprings(parents);
                _elimination.EliminateAndReplace(offsprings);

                if (canIncreaseStrictness) canIncreaseStrictness = _selection.IncreaseStrictness(_parameters.ChildrenPerGeneration);

                if (canMutate)
                {
                    foreach (var chromosome in population)
                    {
                        if (_random.Next(0, 1000) <= _parameters.MutationProbability)
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
