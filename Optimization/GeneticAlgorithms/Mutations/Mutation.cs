﻿using System;

namespace Optimization.GeneticAlgorithms.Mutations
{
    internal abstract class Mutation
    {
        protected readonly Random Random;
        private double _mutationProbability;
        private int[][] _population;

        protected Mutation(double mutationProbability, int[][] population)
        {
            _mutationProbability = mutationProbability;
            _population = population;
            Random = new Random();
        }
        
        public virtual void Mutate(int[][] population)
        {
            if (_mutationProbability > 0d)
            {
                for (int m = (int) (0.1 * _population.Length); m < _population.Length; m++)
                {
                    if (Random.Next(0, 1000) <= _mutationProbability)
                    {
                        Mutate(_population[m]);
                    }
                }
            }
        }
        public abstract void Mutate(int[] chromosome);

        public abstract void Mutate();
        
        public enum MutationType
        {
            RSM,
            CIM,
            TWORS,
            THROAS,
            THRORS
        }
    }
}