using System;
using Optimization.GeneticAlgorithms.Crossovers;
using Optimization.GeneticAlgorithms.Eliminations;
using Optimization.GeneticAlgorithms.Mutations;
using Optimization.GeneticAlgorithms.Selections;
using Optimization.Parameters;

namespace Optimization.GeneticAlgorithms
{
    internal static class GeneticFactory
    {
        public static Crossover CreateCrossover(OptimizationParameters optimizationParameters, double[][] distancesMatrix)
        {
            int startingPoint = optimizationParameters.StartingId;
            Crossover crossover = optimizationParameters.CrossoverMethod switch
            {
                Crossover.CrossoverType.Aex => new AexCrossover(distancesMatrix),
                Crossover.CrossoverType.HGreX => new HGreXCrossover(distancesMatrix),
                Crossover.CrossoverType.MPHGreX => new MPHGreXCrossover(distancesMatrix),
                Crossover.CrossoverType.HRndX => new HRndXCrossover(distancesMatrix),
                Crossover.CrossoverType.HProX => new HProXCrossover(distancesMatrix),
                Crossover.CrossoverType.MPHRndX => new MPHRndXCrossover(distancesMatrix),
                Crossover.CrossoverType.MPHProX => new MPHProXCrossover(distancesMatrix),
                Crossover.CrossoverType.KPoint => new KPointCrossover(distancesMatrix),
                _ => throw new ArgumentException("Wrong crossover name in parameters json file")
            };
            return crossover;
        }

        public static Selection CreateSelection(OptimizationParameters optimizationParameters, int[][] population)
        {
            Selection selection = optimizationParameters.SelectionMethod switch
            {
                Selection.SelectionType.Random => new RandomSelection(population),
                Selection.SelectionType.Tournament => new TournamentSelection(population),
                Selection.SelectionType.Elitism => new ElitismSelection(population),
                Selection.SelectionType.RouletteWheel => new RouletteWheelSelection(population),
                _ => throw new ArgumentException("Wrong selection name in parameters json file")
            };
            return selection;
        }

        public static Elimination CreateElimination(OptimizationParameters optimizationParameters, int[][] population)
        {
            Elimination elimination = optimizationParameters.EliminationMethod switch
            {
                Elimination.EliminationType.Elitism => new ElitismElimination(population),
                Elimination.EliminationType.RouletteWheel => new RouletteWheelElimination(population),
                _ => throw new ArgumentException("Wrong elimination name in parameters json file")
            };
            return elimination;
        }

        public static Mutation CreateMutation(OptimizationParameters optimizationParameters, int[][] population, double mutationProbability)
        {
            Mutation mutation = optimizationParameters.MutationMethod switch
            {
                Mutation.MutationType.RSM => new RSMutation(population, mutationProbability),
                Mutation.MutationType.TWORS => new TWORSMutation(population,mutationProbability),
                Mutation.MutationType.CIM => new CIMutation(population,mutationProbability),
                Mutation.MutationType.THROAS => new THROASMutation(population,mutationProbability),
                Mutation.MutationType.THRORS => new THRORSMutation(population,mutationProbability),
                _ => throw new AggregateException("Wrong mutation method in parameters json file")
            };
            return mutation;
        }
    }
}