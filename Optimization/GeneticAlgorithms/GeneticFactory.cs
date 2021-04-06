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
        public static Crossover CreateCrossover(int startingId, CrossoverMethod crossoverMethod, 
            CrossoverMethod[] crossoverMethods)
        {
            Crossover crossover = crossoverMethod switch
            {
                CrossoverMethod.Aex => new AexCrossover(),
                CrossoverMethod.HGreX => new HGreXCrossover(),
                CrossoverMethod.HRndX => new HRndXCrossover(),
                CrossoverMethod.HProX => new HProXCrossover(),
                CrossoverMethod.KPoint => new KPointCrossover(),
                CrossoverMethod.AexNN => new AexNNCrossover(),
                CrossoverMethod.Cycle => new CycleCrossover(),
                CrossoverMethod.MAC => new MACrossover(crossoverMethods, startingId),
                CrossoverMethod.MRC => new MRCrossover(crossoverMethods, startingId),
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

        public static Mutation CreateMutation(MutationMethod mutationMethod, MutationMethod[] mutationMethods , 
            int[][] population, double mutationProbability)
        {
            Mutation mutation = mutationMethod switch
            {
                MutationMethod.RSM => new RSMutation(mutationProbability,population),
                MutationMethod.TWORS => new TWORSMutation(mutationProbability,population),
                MutationMethod.CIM => new CIMutation(mutationProbability,population),
                MutationMethod.THROAS => new THROASMutation(mutationProbability,population),
                MutationMethod.THRORS => new THRORSMutation(mutationProbability,population),
                MutationMethod.MAM => new MAMutation(mutationMethods,mutationProbability,population),
                MutationMethod.MRM => new MRMutation(mutationMethods,mutationProbability,population),
                _ => throw new AggregateException("Wrong mutation method name")
            };
            return mutation;
        }
    }
}