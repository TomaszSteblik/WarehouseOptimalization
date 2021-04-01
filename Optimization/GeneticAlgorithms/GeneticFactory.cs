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
        public static Crossover CreateCrossover(OptimizationParameters optimizationParameters)
        {
            int startingPoint = optimizationParameters.StartingId;
            Crossover crossover = optimizationParameters.CrossoverMethod switch
            {
                CrossoverMethod.Aex => new AexCrossover(),
                CrossoverMethod.HGreX => new HGreXCrossover(),
                CrossoverMethod.HRndX => new HRndXCrossover(),
                CrossoverMethod.HProX => new HProXCrossover(),
                CrossoverMethod.KPoint => new KPointCrossover(),
                CrossoverMethod.AexNN => new AexNNCrossover(),
                CrossoverMethod.Cycle => new CycleCrossover(),
                CrossoverMethod.MEPC => new MEPCrossover(optimizationParameters.MultiCrossovers),
                CrossoverMethod.MRPC => new MRPCrossover(optimizationParameters.MultiCrossovers),
                _ => throw new ArgumentException("Wrong crossover method name")
            };
            return crossover;
        }

        public static Selection CreateSelection(OptimizationParameters optimizationParameters, int[][] population)
        {
            Selection selection = optimizationParameters.SelectionMethod switch
            {
                SelectionMethod.Random => new RandomSelection(population),
                SelectionMethod.Tournament => new TournamentSelection(population),
                SelectionMethod.Elitism => new ElitismSelection(population),
                SelectionMethod.RouletteWheel => new RouletteWheelSelection(population),
                _ => throw new ArgumentException("Wrong selection name in parameters json file")
            };
            return selection;
        }

        public static Elimination CreateElimination(OptimizationParameters optimizationParameters, int[][] population)
        {
            Elimination elimination = optimizationParameters.EliminationMethod switch
            {
                EliminationMethod.Elitism => new ElitismElimination(population),
                EliminationMethod.RouletteWheel => new RouletteWheelElimination(population),
                _ => throw new ArgumentException("Wrong elimination method name")
            };
            return elimination;
        }

        public static Mutation CreateMutation(OptimizationParameters optimizationParameters, int[][] population, double mutationProbability)
        {
            Mutation mutation = optimizationParameters.MutationMethod switch
            {
                MutationMethod.RSM => new RSMutation(optimizationParameters.MutationProbability,population),
                MutationMethod.TWORS => new TWORSMutation(optimizationParameters.MutationProbability,population),
                MutationMethod.CIM => new CIMutation(optimizationParameters.MutationProbability,population),
                MutationMethod.THROAS => new THROASMutation(optimizationParameters.MutationProbability,population),
                MutationMethod.THRORS => new THRORSMutation(optimizationParameters.MutationProbability,population),
                MutationMethod.MEPM => new MEPMutation(optimizationParameters.MultiMutations,optimizationParameters.MutationProbability,population),
                MutationMethod.MRPM => new MRPMutation(optimizationParameters.MultiMutations,optimizationParameters.MutationProbability,population),
                _ => throw new AggregateException("Wrong mutation method name")
            };
            return mutation;
        }
    }
}