using System;
using System.Linq;


namespace Optimization
{
    public class GeneticAlgorithm : Optimization
    {
        private Selection _selection;
        private Crossover _crossover;
        private Elimination _elimination;
        private readonly Random _random = new Random();

        public GeneticAlgorithm(CityDistances cityDistances, OptimizationParameters optimizationParameters)
        {
            CityDistances = cityDistances;
            OptimizationParameters = optimizationParameters;
        }


        public override int[] FindShortestPath(int start)
        {

            _crossover = OptimizationParameters.CrossoverMethod switch
            {
                "Aex" => new AexCrossover(CityDistances),
                "HGreX" => new HGreXCrossover(CityDistances),
                _ => throw new ArgumentException("Wrong crossover name in parameters json file")
            };

            int[][] population = new int[OptimizationParameters.PopulationSize][];
            InitializePopulation(population,start);
            
            switch (OptimizationParameters.SelectionMethod)
            {
                case "Tournament":
                    _selection = new TournamentSelection(population, CityDistances);
                    break;
                case "Elitism":
                    _selection = new ElitismSelection(population, CityDistances);
                    break;
                case "RouletteWheel":
                    _selection = new RouletteWheelSelection(population, CityDistances);
                    break;
                default:
                    throw new ArgumentException("Wrong selection name in parameters json file");
            }
            
            switch (OptimizationParameters.EliminationMethod)
            {
                case "Elitism":
                    _elimination = new ElitismElimination(ref population, CityDistances);
                    break;
                case "RouletteWheel":
                    _elimination = new RouletteWheelElimination(ref population, CityDistances);
                    break;
                default:
                    throw new ArgumentException("Wrong elimination name in parameters json file");
            }
            
            bool canIncreaseStrictness = true;
            bool canMutate = OptimizationParameters.CanMutate; //true
            int terminateAfterCount = OptimizationParameters.TerminationValue; //10000
            
            
            int lastBestFitness = population.Min(p => CityDistances.CalculatePathLength(p));
            int[] bestGene = population.First(p => CityDistances.CalculatePathLength(p) == lastBestFitness);
            int countToTerminate = terminateAfterCount;
            
            do
            {
                int[][] parents = _selection.GenerateParents(OptimizationParameters.ChildrenPerGeneration*2);
                int[][] offsprings = _crossover.GenerateOffsprings(parents);
                _elimination.EliminateAndReplace(offsprings);

                if (canIncreaseStrictness) canIncreaseStrictness = _selection.IncreaseStrictness(OptimizationParameters.ChildrenPerGeneration);

                if (canMutate)
                {
                    foreach (var chromosome in population)
                    {
                        if (_random.Next(0, 1000) <= OptimizationParameters.MutationProbability)
                        {
                            var a = _random.Next(1, CityDistances.CityCount);
                            var b = _random.Next(1, CityDistances.CityCount);
                            var temp = chromosome[a];
                            chromosome[a] = chromosome[b];
                            chromosome[b] = temp;
                        }
                    }
                }


                int currentBestFitness = population.Min(p => CityDistances.CalculatePathLength(p));
                
                if (lastBestFitness <= currentBestFitness)
                {
                    countToTerminate--;
                }
                else
                {
                    lastBestFitness = currentBestFitness;
                    bestGene = population.First(p => CityDistances.CalculatePathLength(p) == lastBestFitness);
                    countToTerminate = terminateAfterCount;
                }
                
            } while (countToTerminate >0);
            
            int[] result = new int[bestGene.Length+1];
            for (int i = 0; i < bestGene.Length; i++)
            {
                result[i] = bestGene[i];
            }
            result[result.Length - 1] = bestGene[0];
            if (OptimizationParameters.Use2opt)
            {
                Optimizer optimizer = new Optimizer(CityDistances.Create(OptimizationParameters.DataPath), OptimizationParameters);
                return optimizer.Optimize_2opt(result);
            }
            return result;
        }
        
        private bool IsThereGene(int[] chromosome, int a)
        {
            return chromosome.Any(t => t == a);
        }

        private void InitializePopulation(int[][] population,int start)
        {
            int populationSize = population.Length;
            for (int i = 0; i < populationSize; i++)
            {
                int[] temp = new int[CityDistances.CityCount];
                for (int z = 0; z < CityDistances.CityCount; z++)
                {
                    temp[z] = -1;
                }
                int count = 0;
                temp[0] = start;
                count++;
                do
                {
                    int a = _random.Next(0,CityDistances.CityCount);
                    if (!IsThereGene(temp,a))
                    {
                        temp[count] = a;
                        count++;
                    }
                } while (count<CityDistances.CityCount);
                population[i] = temp;
            }
        }
        
    }
}
