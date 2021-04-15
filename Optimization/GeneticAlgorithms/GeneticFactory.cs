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

        public static PopulationInitialization CreatePopulationInitialization(PopulationInitializationMethod method, Random random)
        {
            PopulationInitialization initialization = method switch
            {
                PopulationInitializationMethod.UniformInitialization => new UniformInitialization(random),
                PopulationInitializationMethod.NonUniformInitialization => new NonUniformInitialization(random),
                PopulationInitializationMethod.StandardPathInitialization => new StandardPathInitialization(random),
                PopulationInitializationMethod.PreferedCloseDistancePathInitialization =>
                    new PreferedCloseDistancePathInitialization(random),
                _ => throw new ArgumentException("Wrong population initialization method name")
            };
            return initialization;
        }

        public static ConflictResolver CreateConflictResolver(ConflictResolveMethod method, Random random)
        {
            ConflictResolver resolver = method switch
            {
                ConflictResolveMethod.Random => new RandomResolve(random),
                ConflictResolveMethod.NearestNeighbor => new NearestNeighborResolve(random),
                _ => throw new ArgumentException("Wrong conflict resolve method name")
            };
            return resolver;
        }
        
        
        public static Crossover CreateCrossover(int startingId, CrossoverMethod crossoverMethod, 
            CrossoverMethod[] crossoverMethods, ConflictResolver resolver, Random random)
        {
            Crossover crossover = crossoverMethod switch
            {
                CrossoverMethod.Aex => new AexCrossover(resolver),
                CrossoverMethod.HGreX => new HGreXCrossover(resolver),
                CrossoverMethod.HRndX => new HRndXCrossover(resolver),
                CrossoverMethod.HProX => new HProXCrossover(resolver),
                CrossoverMethod.KPoint => new KPointCrossover(resolver),
                CrossoverMethod.Cycle => new CycleCrossover(resolver),
                CrossoverMethod.MAC => new MACrossover(crossoverMethods, startingId, resolver),
                CrossoverMethod.MRC => new MRCrossover(crossoverMethods, startingId, resolver),
                _ => throw new ArgumentException("Wrong crossover method name")
            };
            return crossover;
        }

        public static Selection CreateSelection(OptimizationParameters optimizationParameters, int[][] population, Random random)
        {
            Selection selection = optimizationParameters.SelectionMethod switch
            {
                SelectionMethod.Random => new RandomSelection(population, random),
                SelectionMethod.Tournament => new TournamentSelection(population, random),
                SelectionMethod.Elitism => new ElitismSelection(population, random),
                SelectionMethod.RouletteWheel => new RouletteWheelSelection(population, random),
                _ => throw new ArgumentException("Wrong selection name in parameters json file")
            };
            return selection;
        }

        public static Elimination CreateElimination(OptimizationParameters optimizationParameters, int[][] population, Random random)
        {
            Elimination elimination = optimizationParameters.EliminationMethod switch
            {
                EliminationMethod.Elitism => new ElitismElimination(population, random),
                EliminationMethod.RouletteWheel => new RouletteWheelElimination(population, random),
                EliminationMethod.Tournament => new TournamentElimination(population, random),
                _ => throw new ArgumentException("Wrong elimination method name")
            };
            return elimination;
        }

        public static Mutation CreateMutation(MutationMethod mutationMethod, MutationMethod[] mutationMethods , 
            int[][] population, double mutationProbability, Random random)
        {
            Mutation mutation = mutationMethod switch
            {
                MutationMethod.RSM => new RSMutation(mutationProbability,population, random),
                MutationMethod.TWORS => new TWORSMutation(mutationProbability,population, random),
                MutationMethod.CIM => new CIMutation(mutationProbability,population, random),
                MutationMethod.THROAS => new THROASMutation(mutationProbability,population, random),
                MutationMethod.THRORS => new THRORSMutation(mutationProbability,population, random),
                MutationMethod.MAM => new MAMutation(mutationMethods,mutationProbability,population, random),
                MutationMethod.MRM => new MRMutation(mutationMethods,mutationProbability,population, random),
                _ => throw new AggregateException("Wrong mutation method name")
            };
            return mutation;
        }
    }
}