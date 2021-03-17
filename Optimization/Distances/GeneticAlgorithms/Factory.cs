using System;
using Optimization.Distances.GeneticAlgorithms.Crossovers;
using Optimization.Distances.GeneticAlgorithms.Eliminations;
using Optimization.Distances.GeneticAlgorithms.Mutations;
using Optimization.Distances.GeneticAlgorithms.Selections;
using Optimization.Parameters;

namespace Optimization.Distances.GeneticAlgorithms
{
    internal class Factory
    {
        public static Crossover CreateCrossover(OptimizationParameters optimizationParameters, double[][] distancesMatrix)
        {
            Crossover crossover = optimizationParameters.CrossoverMethod switch
            {
                "Aex" => new AexCrossover(distancesMatrix),
                "HGreX" => new HGreXCrossover(distancesMatrix),
                "MPHGreX" => new MultiParentHGreXCrossover(distancesMatrix),
                _ => throw new ArgumentException("Wrong crossover name in parameters json file")
            };
            return crossover;
        }

        public static Selection CreateSelection(OptimizationParameters optimizationParameters, int[][] population)
        {
            Selection selection = optimizationParameters.SelectionMethod switch
            {
                "Random" => new RandomSelection(population),
                "Tournament" => new TournamentSelection(population),
                "Elitism" => new ElitismSelection(population),
                "RouletteWheel" => new RouletteWheelSelection(population),
                _ => throw new ArgumentException("Wrong selection name in parameters json file")
            };
            return selection;
        }

        public static Elimination CreateElimination(OptimizationParameters optimizationParameters, int[][] population)
        {
            Elimination elimination = optimizationParameters.EliminationMethod switch
            {
                "Elitism" => new ElitismElimination(population),
                "RouletteWheel" => new RouletteWheelElimination(population),
                _ => throw new ArgumentException("Wrong elimination name in parameters json file")
            };
            return elimination;
        }

        public static Mutation CreateMutation(OptimizationParameters optimizationParameters, int[][] population, double mutationProbability)
        {
            Mutation mutation = optimizationParameters.MutationMethod switch
            {
                "Inversion" => new InversionMutation(population, mutationProbability),
                _ => throw new AggregateException("Wrong mutation method in parameters json file")
            };
            return mutation;
        }
    }
}