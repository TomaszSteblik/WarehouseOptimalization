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