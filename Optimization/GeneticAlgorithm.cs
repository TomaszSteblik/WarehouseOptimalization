using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Optimization
{
    public class GeneticAlgorithm : Optimization
    {
        private Selection _selection;
        private Crossover _crossover;
        private Elimination _elimination;
        private readonly Random _random = new Random();

        private bool canIncreaseStrictness = true;
        private bool canMutate;
        private int terminateAfterCount;
        private int[][] population;
        
        public GeneticAlgorithm(OptimizationParameters optimizationParameters)
        {
            OptimizationParameters = optimizationParameters;
            
            _crossover = OptimizationParameters.CrossoverMethod switch
            {
                "Aex" => new Crossover.AexCrossover(),
                "HGreX" => new Crossover.HGreXCrossover(),
                _ => throw new ArgumentException("Wrong crossover name in parameters json file")
            };

            population = new int[OptimizationParameters.PopulationSize][];
            
            switch (OptimizationParameters.SelectionMethod)
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
            
            switch (OptimizationParameters.EliminationMethod)
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
            
            canMutate = OptimizationParameters.CanMutate; //true
            terminateAfterCount = OptimizationParameters.TerminationValue; //10000
            
            
        }


        public override int[] FindShortestPath(int start)
        {
            InitializePopulation(population,start);
            int lastBestFitness = population.Min(p => Distances.CalculatePathLength(p));
            int[] bestGene = population.First(p => Distances.CalculatePathLength(p) == lastBestFitness);
            int countToTerminate = terminateAfterCount;
            int numberOfIterations = 0;

            double[] fitness = new double[population.Length];
            
            
            
            do
            {
                Parallel.For(0, population.Length, i =>
                {
                    fitness[i] = Distances.CalculatePathLength(population[i]);
                });


                Log.AddToLog($"--------------------------  ERA NR.{numberOfIterations}  --------------------------");
                numberOfIterations++;
                int[][] parents = _selection.GenerateParents(OptimizationParameters.ChildrenPerGeneration*2,fitness);
                int[][] offsprings = _crossover.GenerateOffsprings(parents);
                _elimination.EliminateAndReplace(offsprings,fitness);
                
                if (canIncreaseStrictness) canIncreaseStrictness = _selection.IncreaseStrictness(OptimizationParameters.ChildrenPerGeneration);

                if (canMutate)
                {
                    foreach (var chromosome in population)
                    {
                        if (_random.Next(0, 1000) <= OptimizationParameters.MutationProbability)
                        {
                            Log.AddToLog($"MUTATION RSM\nBEFORE MUTATION({Distances.CalculatePathLength(chromosome)}): {string.Join(";",chromosome)}");

                            var j = _random.Next(1, Distances.ObjectCount);
                            var i = _random.Next(1, j);
                            Array.Reverse(chromosome,i,j-i);

                            Log.AddToLog($"AFTER MUTATION({Distances.CalculatePathLength(chromosome)}):  {string.Join(";",chromosome)}\n");

                        }
                    }
                }


                int currentBestFitness = population.Min(p => Distances.CalculatePathLength(p));
                
                if (lastBestFitness <= currentBestFitness)
                {
                    countToTerminate--;
                }
                else
                {
                    lastBestFitness = currentBestFitness;
                    bestGene = population.First(p => Distances.CalculatePathLength(p) == lastBestFitness);
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
                Log.AddToLog("USING 2-OPT");
                Optimizer optimizer = new Optimizer();
                return optimizer.Optimize_2opt(result);
            }
            return result;
        }

        public override int[] FindShortestPath(int[] order)
        {
            int populationSize = OptimizationParameters.PopulationSize;
            for (int i = 0; i < populationSize; i++)
            {
                int[] temp = new int[order.Length+1];
                for (int z = 0; z <= order.Length; z++)
                {
                    temp[z] = -1;
                }
                int count = 0;
                temp[0] = 0;
                count++;
                do
                {
                    int a = _random.Next(0,order.Length);
                    if (!IsThereGene(temp,order[a]))
                    {
                        temp[count] = order[a];
                        count++;
                    }
                } while (count<=order.Length);
                population[i] = temp;
            }
            
            
            double lastBestFitness = population.Min(p => Distances.CalculatePathLengthDouble(p));
            int[] bestGene = population.First(p => Distances.CalculatePathLengthDouble(p) == lastBestFitness);
            int countToTerminate = terminateAfterCount;
            int numberOfIterations = 0;

            double[] fitness = new double[population.Length];

            _crossover = new Crossover.HGreXCrossover();
            
            
            do
            {
                Parallel.For(0, population.Length, i =>
                {
                    fitness[i] = Distances.CalculatePathLengthDouble(population[i]);
                });


                Log.AddToLog($"--------------------------  ERA NR.{numberOfIterations}  --------------------------");
                numberOfIterations++;
                int[][] parents = _selection.GenerateParents(OptimizationParameters.ChildrenPerGeneration*2,fitness);
                int[][] offsprings = _crossover.GenerateOffsprings(parents);
                _elimination.EliminateAndReplace(offsprings,fitness);
                
                if (canIncreaseStrictness) canIncreaseStrictness = _selection.IncreaseStrictness(OptimizationParameters.ChildrenPerGeneration);

                if (canMutate)
                {
                    foreach (var chromosome in population)
                    {
                        if (_random.Next(0, 1000) <= OptimizationParameters.MutationProbability)
                        {
                            Log.AddToLog($"MUTATION RSM\nBEFORE MUTATION({Distances.CalculatePathLength(chromosome)}): {string.Join(";",chromosome)}");

                            var j = _random.Next(1, Distances.ObjectCount);
                            var i = _random.Next(1, j);
                            Array.Reverse(chromosome,i,j-i);

                            Log.AddToLog($"AFTER MUTATION({Distances.CalculatePathLength(chromosome)}):  {string.Join(";",chromosome)}\n");

                        }
                    }
                }


                double currentBestFitness = population.Min(p => Distances.CalculatePathLengthDouble(p));
                
                if (lastBestFitness <= currentBestFitness)
                {
                    countToTerminate--;
                }
                else
                {
                    lastBestFitness = currentBestFitness;
                    bestGene = population.First(p => Distances.CalculatePathLengthDouble(p) == lastBestFitness);
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
                Log.AddToLog("USING 2-OPT");
                Optimizer optimizer = new Optimizer();
                return optimizer.Optimize_2opt(result);
            }
            return result;
        }

        private bool IsThereGene(int[] chromosome, int a)
        {
            return chromosome.Any(t => t == a);
        }

        private void InitializePopulation(int[][] pop,int start)
        {
            int populationSize = pop.Length;
            for (int i = 0; i < populationSize; i++)
            {
                int[] temp = new int[Distances.ObjectCount];
                for (int z = 0; z < Distances.ObjectCount; z++)
                {
                    temp[z] = -1;
                }
                int count = 0;
                temp[0] = start;
                count++;
                do
                {
                    int a = _random.Next(0,Distances.ObjectCount);
                    if (!IsThereGene(temp,a))
                    {
                        temp[count] = a;
                        count++;
                    }
                } while (count<Distances.ObjectCount);
                pop[i] = temp;
            }
        }
        
    }
}
