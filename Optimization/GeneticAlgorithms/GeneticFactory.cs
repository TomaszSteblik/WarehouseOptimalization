﻿using System;
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

        public static ConflictResolver CreateConflictResolver(OptimizationParameters parameters, ConflictResolveMethod method, Random random)
        {
            ConflictResolver resolver = method switch
            {
                ConflictResolveMethod.Random => new RandomResolve(random, parameters.ResolveRandomizationProbability),
                ConflictResolveMethod.NearestNeighbor => new NearestNeighborResolve(random, parameters.ResolveRandomizationProbability),
                ConflictResolveMethod.Tournament => new TournamentResolver(random, parameters.ResolveRandomizationProbability),
                _ => throw new ArgumentException("Wrong conflict resolve method name")
            };
            return resolver;
        }
        
        
        public static Crossover CreateCrossover(int startingId, CrossoverMethod crossoverMethod, 
            CrossoverMethod[] crossoverMethods, ConflictResolver resolverConflict, ConflictResolver resolverRandomized, Random random)
        {
            Crossover crossover = crossoverMethod switch
            {
                CrossoverMethod.Aex => new AexCrossover(resolverConflict, resolverRandomized,random),
                CrossoverMethod.HGreX => new HGreXCrossover(resolverConflict, resolverRandomized,random),
                CrossoverMethod.HRndX => new HRndXCrossover(resolverConflict, resolverRandomized,random),
                CrossoverMethod.HProX => new HProXCrossover(resolverConflict, resolverRandomized,random),
                CrossoverMethod.KPoint => new KPointCrossover(resolverConflict, resolverRandomized,random),
                CrossoverMethod.Cycle => new CycleCrossover(resolverConflict, resolverRandomized,random),
                CrossoverMethod.Order => new OrderCrossover(resolverConflict, resolverRandomized,random),
                CrossoverMethod.MAC => new MACrossover(crossoverMethods, startingId, resolverConflict, resolverRandomized,random),
                CrossoverMethod.MRC => new MRCrossover(crossoverMethods, startingId, resolverConflict, resolverRandomized,random),
                CrossoverMethod.PMX => new PMXCrossover(resolverConflict,resolverRandomized, random),
                CrossoverMethod.ERX => new ERXCrossover(resolverConflict, resolverRandomized, random),
                _ => throw new ArgumentException("Wrong crossover method name")
            };
            return crossover;
        }

        public static Selection CreateSelection(OptimizationParameters optimizationParameters, int[][] population, Random random)
        {
            Selection selection = optimizationParameters.SelectionMethod switch
            {
                SelectionMethod.Random => new RandomSelection(population, random),
                SelectionMethod.Tournament => new TournamentSelection(population, random) {
                        ParticipantsCount = optimizationParameters.TournamentSelectionParticipantsCount },
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
                EliminationMethod.Tournament => new TournamentElimination(population, random)
                {
                    ParticipantsCount = optimizationParameters.TournamentEliminationParticipantsCount
                },
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