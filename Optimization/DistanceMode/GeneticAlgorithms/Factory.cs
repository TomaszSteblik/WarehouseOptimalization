using System;
using Optimization.DistanceMode.GeneticAlgorithms.Crossovers;
using Optimization.DistanceMode.GeneticAlgorithms.Eliminations;
using Optimization.DistanceMode.GeneticAlgorithms.Selections;

namespace Optimization.DistanceMode.GeneticAlgorithms
{
    public class Factory
    {
        public static Crossover CreateCrossover(OptimizationParameters optimizationParameters, double[][] distancesMatrix)
        {
            Crossover crossover = optimizationParameters.CrossoverMethod switch
            {
                "Aex" => new AexCrossover(distancesMatrix),
                "HGreX" => new HGreXCrossover(distancesMatrix),
                _ => throw new ArgumentException("Wrong crossover name in parameters json file")
            };
            return crossover;
        }

        public static Selection CreateSelection(OptimizationParameters optimizationParameters, int[][] population)
        {
            Selection selection = optimizationParameters.SelectionMethod switch
            {
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
                "Elitism" => new ElitismElimination(ref population),
                "RouletteWheel" => new RouletteWheelElimination(ref population),
                _ => throw new ArgumentException("Wrong elimination name in parameters json file")
            };
            return elimination;
        }
    }
}