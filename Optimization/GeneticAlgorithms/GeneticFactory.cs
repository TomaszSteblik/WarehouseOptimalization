using System;
using Optimization.GeneticAlgorithms.Crossovers;
using Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers;
using Optimization.GeneticAlgorithms.Eliminations;
using Optimization.GeneticAlgorithms.Initialization;
using Optimization.GeneticAlgorithms.Mutations;
using Optimization.GeneticAlgorithms.Selections;
using Optimization.Parameters;

namespace Optimization.GeneticAlgorithms
{
    internal static class GeneticFactory
    {

        public static PopulationInitialization CreatePopulationInitialization(PopulationInitializationMethod method)
        {
            PopulationInitialization initialization = method switch
            {
                PopulationInitializationMethod.UniformInitialization => new UniformInitialization(),
                PopulationInitializationMethod.NonUniformInitialization => new NonUniformInitialization(),
                PopulationInitializationMethod.StandardPathInitialization => new StandardPathInitialization(),
                PopulationInitializationMethod.PreferedCloseDistancePathInitialization =>
                    new PreferedCloseDistancePathInitialization(),
                _ => throw new ArgumentException("Wrong population initialization method name")
            };
            return initialization;
        }

        public static ConflictResolver CreateConflictResolver(ConflictResolveMethod method)
        {
            ConflictResolver resolver = method switch
            {
                ConflictResolveMethod.Random => new RandomResolve(),
                ConflictResolveMethod.NearestNeighbor => new NearestNeighborResolve(),
                _ => throw new ArgumentException("Wrong conflict resolve method name")
            };
            return resolver;
        }
        
        
        public static Crossover CreateCrossover(int startingId, CrossoverMethod crossoverMethod, 
            CrossoverMethod[] crossoverMethods, ConflictResolver resolver)
        {
            Crossover crossover = crossoverMethod switch
            {
                CrossoverMethod.Aex => new AexCrossover(resolver, random),
                CrossoverMethod.HGreX => new HGreXCrossover(resolver, random),
                CrossoverMethod.HRndX => new HRndXCrossover(resolver, random),
                CrossoverMethod.HProX => new HProXCrossover(resolver, random),
                CrossoverMethod.KPoint => new KPointCrossover(resolver, random),
                CrossoverMethod.Cycle => new CycleCrossover(resolver, random),
                CrossoverMethod.Order => new OrderCrossover(resolver, random),
                CrossoverMethod.MAC => new MACrossover(crossoverMethods, startingId, resolver, random),
                CrossoverMethod.MRC => new MRCrossover(crossoverMethods, startingId, resolver, random),
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
                EliminationMethod.Tournament => new TournamentElimination(population),
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